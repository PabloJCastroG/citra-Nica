using CitraNica.Application.Chat;
using CitraNica.Application.Interfaces;
using CitraNica.Domain.Entities;
using CitraNica.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CitraNica.Infrastructure.Chat;

public sealed class ChatService(CitraNicaDbContext dbContext) : IChatService
{
    public async Task<ChatPedidoResponse> ObtenerAsync(
        int idPedido,
        int idUsuario,
        CancellationToken cancellationToken = default)
    {
        var pedido = await ObtenerPedidoAsync(
            idPedido,
            idUsuario,
            cancellationToken);
        var conversacion = await dbContext.Conversaciones
            .AsNoTracking()
            .SingleOrDefaultAsync(
                registrada => registrada.IdPedido == idPedido,
                cancellationToken);

        if (conversacion is null)
        {
            return new ChatPedidoResponse(
                idPedido,
                null,
                ChatActivo(pedido.EstadoPedido),
                Array.Empty<MensajeChatResponse>());
        }

        var mensajes = await ObtenerMensajesAsync(
            conversacion.IdConversacion,
            cancellationToken);

        return new ChatPedidoResponse(
            idPedido,
            conversacion.IdConversacion,
            ChatActivo(pedido.EstadoPedido),
            mensajes);
    }

    public async Task<MensajeChatResponse> EnviarAsync(
        int idPedido,
        int idUsuario,
        EnviarMensajeRequest request,
        CancellationToken cancellationToken = default)
    {
        var contenido = request.Contenido.Trim();

        if (string.IsNullOrWhiteSpace(contenido))
        {
            throw new ArgumentException("El mensaje no puede estar vacío.");
        }

        var pedido = await ObtenerPedidoAsync(
            idPedido,
            idUsuario,
            cancellationToken);

        if (!ChatActivo(pedido.EstadoPedido))
        {
            throw new InvalidOperationException(
                "El chat ya no está disponible porque el pedido terminó.");
        }

        var conversacion = await dbContext.Conversaciones
            .SingleOrDefaultAsync(
                registrada => registrada.IdPedido == idPedido,
                cancellationToken);

        if (conversacion is null)
        {
            conversacion = new Conversacion { IdPedido = idPedido };
            dbContext.Conversaciones.Add(conversacion);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var idReceptor = idUsuario == pedido.IdComprador
            ? pedido.IdProductor
            : pedido.IdComprador;
        var mensaje = new Mensaje
        {
            IdConversacion = conversacion.IdConversacion,
            IdEmisor = idUsuario,
            IdReceptor = idReceptor,
            Contenido = contenido
        };

        dbContext.Mensajes.Add(mensaje);
        await dbContext.SaveChangesAsync(cancellationToken);

        var nombreEmisor = await dbContext.Usuarios
            .Where(usuario => usuario.IdUsuario == idUsuario)
            .Select(usuario => usuario.NombreCompleto)
            .SingleAsync(cancellationToken);

        return new MensajeChatResponse(
            mensaje.IdMensaje,
            mensaje.IdEmisor,
            nombreEmisor,
            mensaje.IdReceptor,
            mensaje.Contenido,
            mensaje.FechaEnvio,
            mensaje.Leido);
    }

    public async Task<MensajesLeidosResponse> MarcarLeidosAsync(
        int idPedido,
        int idUsuario,
        CancellationToken cancellationToken = default)
    {
        await ObtenerPedidoAsync(idPedido, idUsuario, cancellationToken);
        var idConversacion = await dbContext.Conversaciones
            .Where(conversacion => conversacion.IdPedido == idPedido)
            .Select(conversacion => (int?)conversacion.IdConversacion)
            .SingleOrDefaultAsync(cancellationToken);

        if (!idConversacion.HasValue)
        {
            return new MensajesLeidosResponse(0);
        }

        var actualizados = await dbContext.Mensajes
            .Where(mensaje =>
                mensaje.IdConversacion == idConversacion.Value
                && mensaje.IdReceptor == idUsuario
                && !mensaje.Leido)
            .ExecuteUpdateAsync(
                cambios => cambios.SetProperty(
                    mensaje => mensaje.Leido,
                    true),
                cancellationToken);

        return new MensajesLeidosResponse(actualizados);
    }

    private async Task<Pedido> ObtenerPedidoAsync(
        int idPedido,
        int idUsuario,
        CancellationToken cancellationToken)
    {
        return await dbContext.Pedidos
            .AsNoTracking()
            .SingleOrDefaultAsync(
                pedido =>
                    pedido.IdPedido == idPedido
                    && (pedido.IdComprador == idUsuario
                        || pedido.IdProductor == idUsuario),
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "El pedido no existe o no pertenece al usuario.");
    }

    private async Task<IReadOnlyCollection<MensajeChatResponse>>
        ObtenerMensajesAsync(
            int idConversacion,
            CancellationToken cancellationToken)
    {
        return await (
            from mensaje in dbContext.Mensajes.AsNoTracking()
            where mensaje.IdConversacion == idConversacion
            join emisor in dbContext.Usuarios.AsNoTracking()
                on mensaje.IdEmisor equals emisor.IdUsuario
            orderby mensaje.FechaEnvio, mensaje.IdMensaje
            select new MensajeChatResponse(
                mensaje.IdMensaje,
                emisor.IdUsuario,
                emisor.NombreCompleto,
                mensaje.IdReceptor,
                mensaje.Contenido,
                mensaje.FechaEnvio,
                mensaje.Leido))
            .ToListAsync(cancellationToken);
    }

    private static bool ChatActivo(EstadoPedido estado)
    {
        return estado is not EstadoPedido.Rechazado
            and not EstadoPedido.Cancelado
            and not EstadoPedido.Completado;
    }
}
