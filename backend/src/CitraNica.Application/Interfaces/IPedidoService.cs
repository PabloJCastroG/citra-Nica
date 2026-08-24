using CitraNica.Application.Orders;
using CitraNica.Domain.Entities;

namespace CitraNica.Application.Interfaces;

public interface IPedidoService
{
    Task<PedidoDetalleResponse> CrearAsync(
        int idConsumidor,
        CrearPedidoRequest request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<PedidoResumenResponse>> ObtenerComprasAsync(
        int idConsumidor,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<PedidoResumenResponse>> ObtenerVentasAsync(
        int idProductor,
        CancellationToken cancellationToken = default);

    Task<PedidoDetalleResponse?> ObtenerDetalleAsync(
        int idPedido,
        int idUsuario,
        CancellationToken cancellationToken = default);

    Task<PedidoDetalleResponse> CambiarEstadoAsync(
        int idPedido,
        int idProductor,
        EstadoPedido nuevoEstado,
        CancellationToken cancellationToken = default);
}
