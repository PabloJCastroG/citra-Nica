using System.Security.Claims;
using CitraNica.Application.Chat;
using CitraNica.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitraNica.Api.Controllers;

[ApiController]
[Authorize(Roles = "Consumidor,Productor")]
[Route("api/pedidos/{idPedido:int}/chat")]
public sealed class ChatController(IChatService chatService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<ChatPedidoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ChatPedidoResponse>> Obtener(
        int idPedido,
        CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await chatService.ObtenerAsync(
                idPedido,
                ObtenerIdUsuario(),
                cancellationToken));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { mensaje = exception.Message });
        }
    }

    [HttpPost("mensajes")]
    [ProducesResponseType<MensajeChatResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<MensajeChatResponse>> Enviar(
        int idPedido,
        EnviarMensajeRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var mensaje = await chatService.EnviarAsync(
                idPedido,
                ObtenerIdUsuario(),
                request,
                cancellationToken);

            return StatusCode(StatusCodes.Status201Created, mensaje);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mensaje = exception.Message });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { mensaje = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { mensaje = exception.Message });
        }
    }

    [HttpPut("leidos")]
    [ProducesResponseType<MensajesLeidosResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MensajesLeidosResponse>> MarcarLeidos(
        int idPedido,
        CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await chatService.MarcarLeidosAsync(
                idPedido,
                ObtenerIdUsuario(),
                cancellationToken));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { mensaje = exception.Message });
        }
    }

    private int ObtenerIdUsuario()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
