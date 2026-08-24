using CitraNica.Application.Authentication;
using CitraNica.Application.Interfaces;
using CitraNica.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CitraNica.Infrastructure.Users;

public sealed class UsuarioService(CitraNicaDbContext dbContext) : IUsuarioService
{
    public Task<UsuarioResponse?> ObtenerPorIdAsync(
        int idUsuario,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Usuarios
            .AsNoTracking()
            .Where(usuario => usuario.IdUsuario == idUsuario)
            .Select(usuario => new UsuarioResponse(
                usuario.IdUsuario,
                usuario.NombreCompleto,
                usuario.Email,
                usuario.TipoUsuario,
                usuario.TelefonoWhatsapp,
                usuario.Departamento,
                usuario.Municipio,
                usuario.FechaRegistro))
            .SingleOrDefaultAsync(cancellationToken);
    }
}
