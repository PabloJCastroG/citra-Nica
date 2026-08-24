using System.ComponentModel.DataAnnotations;
using CitraNica.Domain.Entities;

namespace CitraNica.Application.Authentication;

public sealed class RegistrarUsuarioRequest
{
    [Required, MaxLength(150)]
    public string NombreCompleto { get; init; } = null!;

    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; init; } = null!;

    [Required, MinLength(8), MaxLength(100)]
    public string Password { get; init; } = null!;

    [Required]
    public TipoUsuario? TipoUsuario { get; init; }

    [MaxLength(30)]
    public string? NumeroIdentificacion { get; init; }

    [MaxLength(20)]
    public string? TelefonoWhatsapp { get; init; }

    [MaxLength(60)]
    public string? Departamento { get; init; }

    [MaxLength(60)]
    public string? Municipio { get; init; }

    [MaxLength(60)]
    public string? FrutaPreferida { get; init; }

    [Range(0, 100)]
    public int? AnosExperiencia { get; init; }
}

public sealed class LoginRequest
{
    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; init; } = null!;

    [Required, MaxLength(100)]
    public string Password { get; init; } = null!;
}

public sealed record AuthenticationResponse(
    int IdUsuario,
    string NombreCompleto,
    string Email,
    TipoUsuario TipoUsuario,
    string Token,
    DateTime ExpiraEnUtc);

public sealed record UsuarioResponse(
    int IdUsuario,
    string NombreCompleto,
    string Email,
    TipoUsuario TipoUsuario,
    string? TelefonoWhatsapp,
    string? Departamento,
    string? Municipio,
    DateTime FechaRegistro);
