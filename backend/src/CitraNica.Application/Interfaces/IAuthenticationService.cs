using CitraNica.Application.Authentication;

namespace CitraNica.Application.Interfaces;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> RegistrarAsync(
        RegistrarUsuarioRequest request,
        CancellationToken cancellationToken = default);

    Task<AuthenticationResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default);
}
