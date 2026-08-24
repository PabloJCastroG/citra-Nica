using CitraNica.Application.Interfaces;
using CitraNica.Application.Publications;
using CitraNica.Domain.Entities;
using CitraNica.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CitraNica.Infrastructure.Publications;

public sealed class PublicacionService(CitraNicaDbContext dbContext)
    : IPublicacionService
{
    private static readonly HashSet<string> UnidadesPermitidas =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "libra",
            "kg",
            "caja",
            "quintal",
            "docena",
            "unidad"
        };

    public async Task<IReadOnlyCollection<PublicacionResumenResponse>>
        ObtenerActivasAsync(
            int? idProducto,
            CancellationToken cancellationToken = default)
    {
        return await CrearConsultaResumen(
                EstadoPublicacion.Activa,
                idProducto,
                null)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<PublicacionResumenResponse>>
        ObtenerDelProductorAsync(
            int idProductor,
            CancellationToken cancellationToken = default)
    {
        return await CrearConsultaResumen(null, null, idProductor)
            .ToListAsync(cancellationToken);
    }

    public Task<PublicacionDetalleResponse?> ObtenerDetalleAsync(
        int idPublicacion,
        CancellationToken cancellationToken = default)
    {
        return CrearConsultaDetalle(idPublicacion)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<PublicacionDetalleResponse> CrearAsync(
        int idProductor,
        CrearPublicacionRequest request,
        CancellationToken cancellationToken = default)
    {
        await ValidarProductoAsync(request.IdProducto, cancellationToken);
        var finca = await ObtenerFincaAsync(idProductor, cancellationToken);
        var unidad = ValidarUnidad(request.UnidadMedida);

        var existe = await dbContext.Publicaciones.AnyAsync(
            publicacion =>
                publicacion.IdProductor == idProductor
                && publicacion.IdProducto == request.IdProducto,
            cancellationToken);

        if (existe)
        {
            throw new InvalidOperationException(
                "Ya existe una publicación para ese producto. Debes actualizarla.");
        }

        var publicacion = new Publicacion
        {
            IdProductor = idProductor,
            IdProducto = request.IdProducto,
            IdFinca = finca.IdFinca,
            CantidadDisponible = request.CantidadDisponible,
            UnidadMedida = unidad,
            PrecioUnitario = request.PrecioUnitario,
            FechaCosechaEstimada = request.FechaCosechaEstimada,
            EstadoMadurez = request.EstadoMadurez?.Trim(),
            EstadoPublicacion = EstadoPublicacion.Activa
        };

        dbContext.Publicaciones.Add(publicacion);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await ObtenerDetalleRequeridoAsync(
            publicacion.IdPublicacion,
            cancellationToken);
    }

    public async Task<PublicacionDetalleResponse> ActualizarAsync(
        int idProductor,
        int idPublicacion,
        ActualizarPublicacionRequest request,
        CancellationToken cancellationToken = default)
    {
        var publicacion = await dbContext.Publicaciones.SingleOrDefaultAsync(
            registrada =>
                registrada.IdPublicacion == idPublicacion
                && registrada.IdProductor == idProductor,
            cancellationToken);

        if (publicacion is null)
        {
            throw new KeyNotFoundException(
                "La publicación no existe o no pertenece al productor.");
        }

        if (!request.EstadoPublicacion.HasValue
            || !Enum.IsDefined(request.EstadoPublicacion.Value))
        {
            throw new ArgumentException("El estado de la publicación no es válido.");
        }

        publicacion.CantidadDisponible = request.CantidadDisponible;
        publicacion.UnidadMedida = ValidarUnidad(request.UnidadMedida);
        publicacion.PrecioUnitario = request.PrecioUnitario;
        publicacion.FechaCosechaEstimada = request.FechaCosechaEstimada;
        publicacion.EstadoMadurez = request.EstadoMadurez?.Trim();
        publicacion.EstadoPublicacion = request.CantidadDisponible == 0
            ? EstadoPublicacion.Agotada
            : request.EstadoPublicacion.Value;

        await dbContext.SaveChangesAsync(cancellationToken);
        return await ObtenerDetalleRequeridoAsync(
            publicacion.IdPublicacion,
            cancellationToken);
    }

    private IQueryable<PublicacionResumenResponse> CrearConsultaResumen(
        EstadoPublicacion? estado,
        int? idProducto,
        int? idProductor)
    {
        return
            from publicacion in dbContext.Publicaciones.AsNoTracking()
            where (!estado.HasValue
                    || publicacion.EstadoPublicacion == estado.Value)
                && (!idProducto.HasValue
                    || publicacion.IdProducto == idProducto.Value)
                && (!idProductor.HasValue
                    || publicacion.IdProductor == idProductor.Value)
            join producto in dbContext.CatalogoProductos.AsNoTracking()
                on publicacion.IdProducto equals producto.IdProducto
            join categoria in dbContext.Categorias.AsNoTracking()
                on producto.IdCategoria equals categoria.IdCategoria
            join productor in dbContext.Usuarios.AsNoTracking()
                on publicacion.IdProductor equals productor.IdUsuario
            orderby publicacion.FechaPublicacion descending
            select new PublicacionResumenResponse(
                publicacion.IdPublicacion,
                producto.IdProducto,
                producto.NombreProducto,
                categoria.NombreCategoria,
                productor.IdUsuario,
                productor.NombreCompleto,
                publicacion.CantidadDisponible,
                publicacion.UnidadMedida,
                publicacion.PrecioUnitario,
                publicacion.EstadoPublicacion);
    }

    private IQueryable<PublicacionDetalleResponse> CrearConsultaDetalle(
        int idPublicacion)
    {
        return
            from publicacion in dbContext.Publicaciones.AsNoTracking()
            where publicacion.IdPublicacion == idPublicacion
            join producto in dbContext.CatalogoProductos.AsNoTracking()
                on publicacion.IdProducto equals producto.IdProducto
            join categoria in dbContext.Categorias.AsNoTracking()
                on producto.IdCategoria equals categoria.IdCategoria
            join productor in dbContext.Usuarios.AsNoTracking()
                on publicacion.IdProductor equals productor.IdUsuario
            join finca in dbContext.Fincas.AsNoTracking()
                on publicacion.IdFinca equals finca.IdFinca
            select new PublicacionDetalleResponse(
                publicacion.IdPublicacion,
                producto.IdProducto,
                producto.NombreProducto,
                categoria.NombreCategoria,
                productor.IdUsuario,
                productor.NombreCompleto,
                finca.IdFinca,
                finca.NombreFinca,
                finca.Latitud,
                finca.Longitud,
                finca.HistoriaCultivo,
                publicacion.CantidadDisponible,
                publicacion.UnidadMedida,
                publicacion.PrecioUnitario,
                publicacion.FechaCosechaEstimada,
                publicacion.EstadoMadurez,
                publicacion.EstadoPublicacion,
                publicacion.FechaPublicacion);
    }

    private async Task ValidarProductoAsync(
        int idProducto,
        CancellationToken cancellationToken)
    {
        if (!await dbContext.CatalogoProductos.AnyAsync(
                producto => producto.IdProducto == idProducto,
                cancellationToken))
        {
            throw new KeyNotFoundException("El producto del catálogo no existe.");
        }
    }

    private async Task<Finca> ObtenerFincaAsync(
        int idProductor,
        CancellationToken cancellationToken)
    {
        return await dbContext.Fincas.SingleOrDefaultAsync(
            finca => finca.IdProductor == idProductor,
            cancellationToken)
            ?? throw new InvalidOperationException(
                "Debes registrar tu finca antes de publicar un producto.");
    }

    private async Task<PublicacionDetalleResponse> ObtenerDetalleRequeridoAsync(
        int idPublicacion,
        CancellationToken cancellationToken)
    {
        return await ObtenerDetalleAsync(idPublicacion, cancellationToken)
            ?? throw new InvalidOperationException(
                "No fue posible recuperar la publicación guardada.");
    }

    private static string ValidarUnidad(string unidad)
    {
        var unidadNormalizada = unidad.Trim().ToLowerInvariant();

        if (!UnidadesPermitidas.Contains(unidadNormalizada))
        {
            throw new ArgumentException(
                "La unidad debe ser libra, kg, caja, quintal, docena o unidad.");
        }

        return unidadNormalizada;
    }
}
