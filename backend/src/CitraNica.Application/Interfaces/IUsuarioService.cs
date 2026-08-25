using CitraNica.Application.Authentication;

namespace CitraNica.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioResponse?> ObtenerPorIdAsync(
        int idUsuario,
        CancellationToken cancellationToken = default);
}
