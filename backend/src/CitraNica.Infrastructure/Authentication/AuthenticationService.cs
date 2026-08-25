using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CitraNica.Application.Authentication;
using CitraNica.Application.Interfaces;
using CitraNica.Domain.Entities;
using CitraNica.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CitraNica.Infrastructure.Authentication;

public sealed class AuthenticationService(
    CitraNicaDbContext dbContext,
    PasswordHasher<Usuario> passwordHasher,
    IOptions<JwtOptions> jwtOptions) : IAuthenticationService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<AuthenticationResponse> RegistrarAsync(
        RegistrarUsuarioRequest request,
        CancellationToken cancellationToken = default)
    {
        var tipoUsuario = request.TipoUsuario
            ?? throw new ArgumentException("El tipo de usuario es obligatorio.");

        if (tipoUsuario is not TipoUsuario.Productor
            and not TipoUsuario.Consumidor)
        {
            throw new ArgumentException(
                "Solo se permite registrar cuentas de productor o consumidor.");
        }

        var email = NormalizarEmail(request.Email);
        var existe = await dbContext.Usuarios
            .AnyAsync(usuario => usuario.Email == email, cancellationToken);

        if (existe)
        {
            throw new InvalidOperationException("El correo ya está registrado.");
        }

        var usuario = new Usuario
        {
            NombreCompleto = request.NombreCompleto.Trim(),
            Email = email,
            TipoUsuario = tipoUsuario,
            NumeroIdentificacion = request.NumeroIdentificacion?.Trim(),
            TelefonoWhatsapp = request.TelefonoWhatsapp?.Trim(),
            Departamento = request.Departamento?.Trim(),
            Municipio = request.Municipio?.Trim(),
            FrutaPreferida = request.FrutaPreferida?.Trim(),
            AnosExperiencia = request.AnosExperiencia
        };

        usuario.PasswordHash = passwordHasher.HashPassword(
            usuario,
            request.Password);

        dbContext.Usuarios.Add(usuario);
        await dbContext.SaveChangesAsync(cancellationToken);

        return CrearRespuesta(usuario);
    }

    public async Task<AuthenticationResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var email = NormalizarEmail(request.Email);
        var usuario = await dbContext.Usuarios
            .SingleOrDefaultAsync(
                registrado => registrado.Email == email,
                cancellationToken);

        if (usuario is null)
        {
            throw new UnauthorizedAccessException("Credenciales incorrectas.");
        }

        var resultado = passwordHasher.VerifyHashedPassword(
            usuario,
            usuario.PasswordHash,
            request.Password);

        if (resultado == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Credenciales incorrectas.");
        }

        if (resultado == PasswordVerificationResult.SuccessRehashNeeded)
        {
            usuario.PasswordHash = passwordHasher.HashPassword(
                usuario,
                request.Password);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return CrearRespuesta(usuario);
    }

    private AuthenticationResponse CrearRespuesta(Usuario usuario)
    {
        var expiraEnUtc = DateTime.UtcNow.AddMinutes(
            _jwtOptions.ExpirationMinutes);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Name, usuario.NombreCompleto),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.TipoUsuario.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expiraEnUtc,
            signingCredentials: credentials);

        return new AuthenticationResponse(
            usuario.IdUsuario,
            usuario.NombreCompleto,
            usuario.Email,
            usuario.TipoUsuario,
            new JwtSecurityTokenHandler().WriteToken(token),
            expiraEnUtc);
    }

    private static string NormalizarEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }
}
