using CitraNica.Application.Authentication;
using CitraNica.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CitraNica.Api.Controllers;

[ApiController]
[Route("api/autenticacion")]
public sealed class AuthenticationController(
    IAuthenticationService authenticationService) : ControllerBase
{
    [HttpPost("registro")]
    [ProducesResponseType<AuthenticationResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AuthenticationResponse>> Registrar(
        RegistrarUsuarioRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await authenticationService.RegistrarAsync(
                request,
                cancellationToken);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mensaje = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { mensaje = exception.Message });
        }
    }

    [HttpPost("login")]
    [ProducesResponseType<AuthenticationResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthenticationResponse>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await authenticationService.LoginAsync(
                request,
                cancellationToken));
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(new { mensaje = exception.Message });
        }
    }
}
