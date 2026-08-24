using CitraNica.Application.Publications;

namespace CitraNica.Application.Interfaces;

public interface IPublicacionService
{
    Task<IReadOnlyCollection<PublicacionResumenResponse>> ObtenerActivasAsync(
        int? idProducto,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<PublicacionResumenResponse>> ObtenerDelProductorAsync(
        int idProductor,
        CancellationToken cancellationToken = default);

    Task<PublicacionDetalleResponse?> ObtenerDetalleAsync(
        int idPublicacion,
        CancellationToken cancellationToken = default);

    Task<PublicacionDetalleResponse> CrearAsync(
        int idProductor,
        CrearPublicacionRequest request,
        CancellationToken cancellationToken = default);

    Task<PublicacionDetalleResponse> ActualizarAsync(
        int idProductor,
        int idPublicacion,
        ActualizarPublicacionRequest request,
        CancellationToken cancellationToken = default);
}
