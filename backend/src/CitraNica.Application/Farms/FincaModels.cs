using System.ComponentModel.DataAnnotations;

namespace CitraNica.Application.Farms;

public sealed class GuardarFincaRequest
{
    [Required, MaxLength(100)]
    public string NombreFinca { get; init; } = null!;

    [Range(typeof(decimal), "-90", "90")]
    public decimal? Latitud { get; init; }

    [Range(typeof(decimal), "-180", "180")]
    public decimal? Longitud { get; init; }

    [MaxLength(3000)]
    public string? HistoriaCultivo { get; init; }
}

public sealed record FincaResponse(
    int IdFinca,
    int IdProductor,
    string NombreFinca,
    decimal? Latitud,
    decimal? Longitud,
    string? HistoriaCultivo);
