using CitraNica.Application.Farms;

namespace CitraNica.Application.Interfaces;

public interface IFincaService
{
    Task<FincaResponse?> ObtenerPorProductorAsync(
        int idProductor,
        CancellationToken cancellationToken = default);

    Task<FincaResponse> GuardarAsync(
        int idProductor,
        GuardarFincaRequest request,
        CancellationToken cancellationToken = default);
}
