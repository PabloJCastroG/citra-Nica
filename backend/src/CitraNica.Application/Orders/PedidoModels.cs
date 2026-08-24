using System.ComponentModel.DataAnnotations;
using CitraNica.Domain.Entities;

namespace CitraNica.Application.Orders;

public sealed class CrearPedidoRequest
{
    [Required, MinLength(1)]
    public IReadOnlyCollection<CrearLineaPedidoRequest> Lineas { get; init; } =
        Array.Empty<CrearLineaPedidoRequest>();

    [Required]
    public MetodoEntrega? MetodoEntrega { get; init; }

    public int? IdDireccionEntrega { get; init; }

    public int? IdMetodoPago { get; init; }

    public int? IdLugarAcopio { get; init; }

    [MaxLength(50)]
    public string? TipoPagoAcordado { get; init; }
}

public sealed class CrearLineaPedidoRequest
{
    [Range(1, int.MaxValue)]
    public int IdPublicacion { get; init; }

    [Range(typeof(decimal), "0.01", "99999999")]
    public decimal Cantidad { get; init; }
}

public sealed class CambiarEstadoPedidoRequest
{
    [Required]
    public EstadoPedido? Estado { get; init; }
}

public sealed record LineaPedidoResponse(
    int IdPublicacion,
    int IdProducto,
    string NombreProducto,
    decimal Cantidad,
    string UnidadMedida,
    decimal PrecioAcordado,
    decimal Importe);

public sealed record PedidoResumenResponse(
    int IdPedido,
    int IdComprador,
    string NombreComprador,
    int IdProductor,
    string NombreProductor,
    MetodoEntrega MetodoEntrega,
    EstadoPedido EstadoPedido,
    decimal Subtotal,
    decimal MontoComision,
    decimal Total,
    DateTime FechaCreacion);

public sealed record PedidoDetalleResponse(
    int IdPedido,
    int IdComprador,
    string NombreComprador,
    int IdProductor,
    string NombreProductor,
    MetodoEntrega MetodoEntrega,
    EstadoPedido EstadoPedido,
    int? IdDireccionEntrega,
    int? IdMetodoPago,
    int? IdLugarAcopio,
    string? TipoPagoAcordado,
    decimal Subtotal,
    decimal ComisionPorcentaje,
    decimal MontoComision,
    decimal Total,
    DateTime FechaCreacion,
    IReadOnlyCollection<LineaPedidoResponse> Lineas);
