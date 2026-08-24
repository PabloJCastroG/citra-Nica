using CitraNica.Application.Interfaces;
using CitraNica.Application.Orders;
using CitraNica.Domain.Entities;
using CitraNica.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CitraNica.Infrastructure.Orders;

public sealed class PedidoService(CitraNicaDbContext dbContext) : IPedidoService
{
    private const decimal ComisionPorcentaje = 3m;

    public async Task<PedidoDetalleResponse> CrearAsync(
        int idConsumidor,
        CrearPedidoRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidarSolicitud(request);
        await ValidarEntregaAsync(idConsumidor, request, cancellationToken);

        var lineas = request.Lineas
            .GroupBy(linea => linea.IdPublicacion)
            .Select(grupo => new
            {
                IdPublicacion = grupo.Key,
                Cantidad = grupo.Sum(linea => linea.Cantidad)
            })
            .ToArray();
        var idsPublicaciones = lineas
            .Select(linea => linea.IdPublicacion)
            .ToList();

        await using var transaccion = await dbContext.Database
            .BeginTransactionAsync(cancellationToken);

        var publicaciones = await dbContext.Publicaciones
            .AsNoTracking()
            .Where(publicacion =>
                idsPublicaciones.Contains(publicacion.IdPublicacion))
            .ToDictionaryAsync(
                publicacion => publicacion.IdPublicacion,
                cancellationToken);

        if (publicaciones.Count != lineas.Length)
        {
            throw new KeyNotFoundException(
                "Una o más publicaciones no existen.");
        }

        var productores = publicaciones.Values
            .Select(publicacion => publicacion.IdProductor)
            .Distinct()
            .ToArray();

        if (productores.Length != 1)
        {
            throw new InvalidOperationException(
                "Un pedido solo puede contener productos de un productor.");
        }

        decimal subtotal = 0;
        var detalles = new List<DetalleMovimiento>(lineas.Length);

        foreach (var linea in lineas)
        {
            var publicacion = publicaciones[linea.IdPublicacion];
            var idPublicacion = linea.IdPublicacion;
            var cantidad = linea.Cantidad;

            if (publicacion.EstadoPublicacion != EstadoPublicacion.Activa)
            {
                throw new InvalidOperationException(
                    $"La publicación {linea.IdPublicacion} no está activa.");
            }

            var actualizadas = await dbContext.Publicaciones
                .Where(registrada =>
                    registrada.IdPublicacion == idPublicacion
                    && registrada.EstadoPublicacion == EstadoPublicacion.Activa
                    && registrada.CantidadDisponible >= cantidad)
                .ExecuteUpdateAsync(
                    cambios => cambios
                        .SetProperty(
                            registrada => registrada.CantidadDisponible,
                            registrada =>
                                registrada.CantidadDisponible - cantidad),
                    cancellationToken);

            if (actualizadas != 1)
            {
                throw new InvalidOperationException(
                    $"No hay inventario suficiente en la publicación "
                    + $"{linea.IdPublicacion}.");
            }

            await dbContext.Publicaciones
                .Where(registrada =>
                    registrada.IdPublicacion == idPublicacion
                    && registrada.CantidadDisponible == 0)
                .ExecuteUpdateAsync(
                    cambios => cambios.SetProperty(
                        registrada => registrada.EstadoPublicacion,
                        EstadoPublicacion.Agotada),
                    cancellationToken);

            subtotal += linea.Cantidad * publicacion.PrecioUnitario;
            detalles.Add(new DetalleMovimiento
            {
                IdPublicacion = publicacion.IdPublicacion,
                CantidadMovimiento = linea.Cantidad,
                PrecioAcordado = publicacion.PrecioUnitario
            });
        }

        subtotal = RedondearMoneda(subtotal);
        var montoComision = RedondearMoneda(
            subtotal * ComisionPorcentaje / 100m);
        var pedido = new Pedido
        {
            IdComprador = idConsumidor,
            IdProductor = productores[0],
            IdDireccionEntrega = request.IdDireccionEntrega,
            IdMetodoPago = request.IdMetodoPago,
            IdLugarAcopio = request.IdLugarAcopio,
            MetodoEntrega = request.MetodoEntrega!.Value,
            EstadoPedido = EstadoPedido.Pendiente,
            Subtotal = subtotal,
            ComisionPorcentaje = ComisionPorcentaje,
            MontoComision = montoComision,
            Total = subtotal + montoComision,
            TipoPagoAcordado = request.TipoPagoAcordado?.Trim()
        };

        dbContext.Pedidos.Add(pedido);
        await dbContext.SaveChangesAsync(cancellationToken);

        foreach (var detalle in detalles)
        {
            detalle.IdPedido = pedido.IdPedido;
        }

        dbContext.DetalleMovimientos.AddRange(detalles);
        await dbContext.SaveChangesAsync(cancellationToken);
        await transaccion.CommitAsync(cancellationToken);

        return await ConstruirDetalleAsync(pedido, cancellationToken);
    }

    public Task<IReadOnlyCollection<PedidoResumenResponse>> ObtenerComprasAsync(
        int idConsumidor,
        CancellationToken cancellationToken = default)
    {
        return ObtenerResumenAsync(
            idConsumidor,
            null,
            cancellationToken);
    }

    public Task<IReadOnlyCollection<PedidoResumenResponse>> ObtenerVentasAsync(
        int idProductor,
        CancellationToken cancellationToken = default)
    {
        return ObtenerResumenAsync(
            null,
            idProductor,
            cancellationToken);
    }

    public async Task<PedidoDetalleResponse?> ObtenerDetalleAsync(
        int idPedido,
        int idUsuario,
        CancellationToken cancellationToken = default)
    {
        var pedido = await dbContext.Pedidos
            .AsNoTracking()
            .SingleOrDefaultAsync(
                registrado =>
                    registrado.IdPedido == idPedido
                    && (registrado.IdComprador == idUsuario
                        || registrado.IdProductor == idUsuario),
                cancellationToken);

        return pedido is null
            ? null
            : await ConstruirDetalleAsync(pedido, cancellationToken);
    }

    public async Task<PedidoDetalleResponse> CambiarEstadoAsync(
        int idPedido,
        int idProductor,
        EstadoPedido nuevoEstado,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(nuevoEstado))
        {
            throw new ArgumentException("El estado indicado no es válido.");
        }

        await using var transaccion = await dbContext.Database
            .BeginTransactionAsync(cancellationToken);
        var pedido = await dbContext.Pedidos.SingleOrDefaultAsync(
            registrado =>
                registrado.IdPedido == idPedido
                && registrado.IdProductor == idProductor,
            cancellationToken)
            ?? throw new KeyNotFoundException(
                "El pedido no existe o no pertenece al productor.");

        if (!TransicionPermitida(pedido.EstadoPedido, nuevoEstado))
        {
            throw new InvalidOperationException(
                $"No se puede cambiar el pedido de {pedido.EstadoPedido} "
                + $"a {nuevoEstado}.");
        }

        if (nuevoEstado == EstadoPedido.Rechazado)
        {
            await RestaurarInventarioAsync(idPedido, cancellationToken);
        }

        if (EstadoFinal(nuevoEstado))
        {
            await EliminarChatAsync(idPedido, cancellationToken);
        }

        pedido.EstadoPedido = nuevoEstado;
        await dbContext.SaveChangesAsync(cancellationToken);
        await transaccion.CommitAsync(cancellationToken);

        return await ConstruirDetalleAsync(pedido, cancellationToken);
    }

    private async Task<IReadOnlyCollection<PedidoResumenResponse>>
        ObtenerResumenAsync(
            int? idConsumidor,
            int? idProductor,
            CancellationToken cancellationToken)
    {
        return await (
            from pedido in dbContext.Pedidos.AsNoTracking()
            where (!idConsumidor.HasValue
                    || pedido.IdComprador == idConsumidor.Value)
                && (!idProductor.HasValue
                    || pedido.IdProductor == idProductor.Value)
            join comprador in dbContext.Usuarios.AsNoTracking()
                on pedido.IdComprador equals comprador.IdUsuario
            join productor in dbContext.Usuarios.AsNoTracking()
                on pedido.IdProductor equals productor.IdUsuario
            orderby pedido.FechaCreacion descending
            select new PedidoResumenResponse(
                pedido.IdPedido,
                comprador.IdUsuario,
                comprador.NombreCompleto,
                productor.IdUsuario,
                productor.NombreCompleto,
                pedido.MetodoEntrega,
                pedido.EstadoPedido,
                pedido.Subtotal,
                pedido.MontoComision,
                pedido.Total,
                pedido.FechaCreacion))
            .ToListAsync(cancellationToken);
    }

    private async Task<PedidoDetalleResponse> ConstruirDetalleAsync(
        Pedido pedido,
        CancellationToken cancellationToken)
    {
        var nombres = await dbContext.Usuarios
            .AsNoTracking()
            .Where(usuario =>
                usuario.IdUsuario == pedido.IdComprador
                || usuario.IdUsuario == pedido.IdProductor)
            .ToDictionaryAsync(
                usuario => usuario.IdUsuario,
                usuario => usuario.NombreCompleto,
                cancellationToken);
        var lineas = await (
            from detalle in dbContext.DetalleMovimientos.AsNoTracking()
            where detalle.IdPedido == pedido.IdPedido
            join publicacion in dbContext.Publicaciones.AsNoTracking()
                on detalle.IdPublicacion equals publicacion.IdPublicacion
            join producto in dbContext.CatalogoProductos.AsNoTracking()
                on publicacion.IdProducto equals producto.IdProducto
            orderby detalle.IdDetalle
            select new LineaPedidoResponse(
                publicacion.IdPublicacion,
                producto.IdProducto,
                producto.NombreProducto,
                detalle.CantidadMovimiento,
                publicacion.UnidadMedida,
                detalle.PrecioAcordado,
                detalle.CantidadMovimiento * detalle.PrecioAcordado))
            .ToListAsync(cancellationToken);

        return new PedidoDetalleResponse(
            pedido.IdPedido,
            pedido.IdComprador,
            nombres[pedido.IdComprador],
            pedido.IdProductor,
            nombres[pedido.IdProductor],
            pedido.MetodoEntrega,
            pedido.EstadoPedido,
            pedido.IdDireccionEntrega,
            pedido.IdMetodoPago,
            pedido.IdLugarAcopio,
            pedido.TipoPagoAcordado,
            pedido.Subtotal,
            pedido.ComisionPorcentaje,
            pedido.MontoComision,
            pedido.Total,
            pedido.FechaCreacion,
            lineas);
    }

    private async Task ValidarEntregaAsync(
        int idConsumidor,
        CrearPedidoRequest request,
        CancellationToken cancellationToken)
    {
        switch (request.MetodoEntrega)
        {
            case MetodoEntrega.EntregaDirecta:
                if (!request.IdDireccionEntrega.HasValue
                    || !await dbContext.Direcciones.AnyAsync(
                        direccion =>
                            direccion.IdDireccion
                                == request.IdDireccionEntrega.Value
                            && direccion.IdUsuario == idConsumidor,
                        cancellationToken))
                {
                    throw new ArgumentException(
                        "La entrega directa requiere una dirección del consumidor.");
                }

                break;
            case MetodoEntrega.RetiroFinca:
                break;
            case MetodoEntrega.LugarAcopio:
                if (!request.IdLugarAcopio.HasValue
                    || !await dbContext.LugaresAcopio.AnyAsync(
                        lugar => lugar.IdLugarAcopio
                            == request.IdLugarAcopio.Value,
                        cancellationToken))
                {
                    throw new ArgumentException(
                        "Debes seleccionar un lugar de acopio válido.");
                }

                break;
            default:
                throw new ArgumentException("El método de entrega no es válido.");
        }

        if (request.IdMetodoPago.HasValue
            && !await dbContext.MetodosPago.AnyAsync(
                metodo =>
                    metodo.IdMetodoPago == request.IdMetodoPago.Value
                    && metodo.IdUsuario == idConsumidor,
                cancellationToken))
        {
            throw new ArgumentException(
                "El método de pago no pertenece al consumidor.");
        }
    }

    private async Task RestaurarInventarioAsync(
        int idPedido,
        CancellationToken cancellationToken)
    {
        var detalles = await dbContext.DetalleMovimientos
            .AsNoTracking()
            .Where(detalle => detalle.IdPedido == idPedido)
            .ToListAsync(cancellationToken);

        foreach (var detalle in detalles)
        {
            await dbContext.Publicaciones
                .Where(publicacion =>
                    publicacion.IdPublicacion == detalle.IdPublicacion)
                .ExecuteUpdateAsync(
                    cambios => cambios
                        .SetProperty(
                            publicacion => publicacion.CantidadDisponible,
                            publicacion => publicacion.CantidadDisponible
                                + detalle.CantidadMovimiento)
                        .SetProperty(
                            publicacion => publicacion.EstadoPublicacion,
                            publicacion => publicacion.EstadoPublicacion
                                == EstadoPublicacion.Agotada
                                ? EstadoPublicacion.Activa
                                : publicacion.EstadoPublicacion),
                    cancellationToken);
        }
    }

    private async Task EliminarChatAsync(
        int idPedido,
        CancellationToken cancellationToken)
    {
        var idConversacion = await dbContext.Conversaciones
            .Where(conversacion => conversacion.IdPedido == idPedido)
            .Select(conversacion => (int?)conversacion.IdConversacion)
            .SingleOrDefaultAsync(cancellationToken);

        if (!idConversacion.HasValue)
        {
            return;
        }

        await dbContext.Mensajes
            .Where(mensaje =>
                mensaje.IdConversacion == idConversacion.Value)
            .ExecuteDeleteAsync(cancellationToken);
        await dbContext.Conversaciones
            .Where(conversacion =>
                conversacion.IdConversacion == idConversacion.Value)
            .ExecuteDeleteAsync(cancellationToken);
    }

    private static void ValidarSolicitud(CrearPedidoRequest request)
    {
        if (request.Lineas.Count == 0)
        {
            throw new ArgumentException(
                "El pedido debe contener al menos un producto.");
        }

        if (request.Lineas.Any(linea =>
                linea.IdPublicacion <= 0 || linea.Cantidad <= 0))
        {
            throw new ArgumentException(
                "Las publicaciones y cantidades deben ser válidas.");
        }
    }

    private static bool TransicionPermitida(
        EstadoPedido actual,
        EstadoPedido siguiente)
    {
        return (actual, siguiente) switch
        {
            (EstadoPedido.Pendiente, EstadoPedido.Aceptado) => true,
            (EstadoPedido.Pendiente, EstadoPedido.Rechazado) => true,
            (EstadoPedido.Aceptado, EstadoPedido.Preparando) => true,
            (EstadoPedido.Aceptado, EstadoPedido.EnCamino) => true,
            (EstadoPedido.Preparando, EstadoPedido.EnCamino) => true,
            (EstadoPedido.EnCamino, EstadoPedido.Completado) => true,
            _ => false
        };
    }

    private static bool EstadoFinal(EstadoPedido estado)
    {
        return estado is EstadoPedido.Rechazado
            or EstadoPedido.Cancelado
            or EstadoPedido.Completado;
    }

    private static decimal RedondearMoneda(decimal monto)
    {
        return Math.Round(monto, 2, MidpointRounding.AwayFromZero);
    }
}
