using System.Security.Claims;
using CitraNica.Application.Authentication;
using CitraNica.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitraNica.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/usuarios")]
public sealed class UsuariosController(IUsuarioService usuarioService) : ControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType<UsuarioResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioResponse>> ObtenerPerfil(
        CancellationToken cancellationToken)
    {
        var idTexto = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(idTexto, out var idUsuario))
        {
            return Unauthorized(new { mensaje = "El token no contiene un usuario válido." });
        }

        var usuario = await usuarioService.ObtenerPorIdAsync(
            idUsuario,
            cancellationToken);

        return usuario is null
            ? NotFound(new { mensaje = "Usuario no encontrado." })
            : Ok(usuario);
    }
}
