using System.ComponentModel.DataAnnotations;
using CitraNica.Domain.Entities;

namespace CitraNica.Application.Publications;

public sealed class CrearPublicacionRequest
{
    [Range(1, int.MaxValue)]
    public int IdProducto { get; init; }

    [Range(typeof(decimal), "0.01", "99999999")]
    public decimal CantidadDisponible { get; init; }

    [Required, MaxLength(20)]
    public string UnidadMedida { get; init; } = null!;

    [Range(typeof(decimal), "0.01", "99999999")]
    public decimal PrecioUnitario { get; init; }

    public DateOnly? FechaCosechaEstimada { get; init; }

    [MaxLength(30)]
    public string? EstadoMadurez { get; init; }
}

public sealed class ActualizarPublicacionRequest
{
    [Range(typeof(decimal), "0", "99999999")]
    public decimal CantidadDisponible { get; init; }

    [Required, MaxLength(20)]
    public string UnidadMedida { get; init; } = null!;

    [Range(typeof(decimal), "0.01", "99999999")]
    public decimal PrecioUnitario { get; init; }

    public DateOnly? FechaCosechaEstimada { get; init; }

    [MaxLength(30)]
    public string? EstadoMadurez { get; init; }

    [Required]
    public EstadoPublicacion? EstadoPublicacion { get; init; }
}

public sealed record PublicacionResumenResponse(
    int IdPublicacion,
    int IdProducto,
    string NombreProducto,
    string NombreCategoria,
    int IdProductor,
    string NombreProductor,
    decimal CantidadDisponible,
    string UnidadMedida,
    decimal PrecioUnitario,
    EstadoPublicacion EstadoPublicacion);

public sealed record PublicacionDetalleResponse(
    int IdPublicacion,
    int IdProducto,
    string NombreProducto,
    string NombreCategoria,
    int IdProductor,
    string NombreProductor,
    int IdFinca,
    string NombreFinca,
    decimal? Latitud,
    decimal? Longitud,
    string? HistoriaCultivo,
    decimal CantidadDisponible,
    string UnidadMedida,
    decimal PrecioUnitario,
    DateOnly? FechaCosechaEstimada,
    string? EstadoMadurez,
    EstadoPublicacion EstadoPublicacion,
    DateTime FechaPublicacion);
