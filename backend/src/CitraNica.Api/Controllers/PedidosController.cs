using System.Security.Claims;
using CitraNica.Application.Interfaces;
using CitraNica.Application.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitraNica.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/pedidos")]
public sealed class PedidosController(
    IPedidoService pedidoService,
    ILogger<PedidosController> logger)
    : ControllerBase
{
    [Authorize(Roles = "Consumidor")]
    [HttpPost]
    [ProducesResponseType<PedidoDetalleResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PedidoDetalleResponse>> Crear(
        CrearPedidoRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var pedido = await pedidoService.CrearAsync(
                ObtenerIdUsuario(),
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(ObtenerDetalle),
                new { idPedido = pedido.IdPedido },
                pedido);
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
            logger.LogError(
                exception,
                "No fue posible crear el pedido del usuario {IdUsuario}.",
                ObtenerIdUsuario());
            return Conflict(new { mensaje = exception.Message });
        }
    }

    [Authorize(Roles = "Consumidor")]
    [HttpGet("mis-compras")]
    [ProducesResponseType<IReadOnlyCollection<PedidoResumenResponse>>(
        StatusCodes.Status200OK)]
    public async Task<
        ActionResult<IReadOnlyCollection<PedidoResumenResponse>>> ObtenerCompras(
            CancellationToken cancellationToken)
    {
        return Ok(await pedidoService.ObtenerComprasAsync(
            ObtenerIdUsuario(),
            cancellationToken));
    }

    [Authorize(Roles = "Productor")]
    [HttpGet("mis-ventas")]
    [ProducesResponseType<IReadOnlyCollection<PedidoResumenResponse>>(
        StatusCodes.Status200OK)]
    public async Task<
        ActionResult<IReadOnlyCollection<PedidoResumenResponse>>> ObtenerVentas(
            CancellationToken cancellationToken)
    {
        return Ok(await pedidoService.ObtenerVentasAsync(
            ObtenerIdUsuario(),
            cancellationToken));
    }

    [HttpGet("{idPedido:int}")]
    [ProducesResponseType<PedidoDetalleResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PedidoDetalleResponse>> ObtenerDetalle(
        int idPedido,
        CancellationToken cancellationToken)
    {
        var pedido = await pedidoService.ObtenerDetalleAsync(
            idPedido,
            ObtenerIdUsuario(),
            cancellationToken);

        return pedido is null
            ? NotFound(new { mensaje = "Pedido no encontrado." })
            : Ok(pedido);
    }

    [Authorize(Roles = "Productor")]
    [HttpPut("{idPedido:int}/estado")]
    [ProducesResponseType<PedidoDetalleResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PedidoDetalleResponse>> CambiarEstado(
        int idPedido,
        CambiarEstadoPedidoRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await pedidoService.CambiarEstadoAsync(
                idPedido,
                ObtenerIdUsuario(),
                request.Estado!.Value,
                cancellationToken));
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

    private int ObtenerIdUsuario()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
