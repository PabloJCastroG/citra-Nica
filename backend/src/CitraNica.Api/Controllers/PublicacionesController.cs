using System.Security.Claims;
using CitraNica.Application.Interfaces;
using CitraNica.Application.Publications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitraNica.Api.Controllers;

[ApiController]
[Route("api/publicaciones")]
public sealed class PublicacionesController(
    IPublicacionService publicacionService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<PublicacionResumenResponse>>(
        StatusCodes.Status200OK)]
    public async Task<
        ActionResult<IReadOnlyCollection<PublicacionResumenResponse>>>
        ObtenerActivas(
            [FromQuery] int? idProducto,
            CancellationToken cancellationToken)
    {
        return Ok(await publicacionService.ObtenerActivasAsync(
            idProducto,
            cancellationToken));
    }

    [HttpGet("{idPublicacion:int}")]
    [ProducesResponseType<PublicacionDetalleResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PublicacionDetalleResponse>> ObtenerDetalle(
        int idPublicacion,
        CancellationToken cancellationToken)
    {
        var publicacion = await publicacionService.ObtenerDetalleAsync(
            idPublicacion,
            cancellationToken);

        return publicacion is null
            ? NotFound(new { mensaje = "Publicación no encontrada." })
            : Ok(publicacion);
    }

    [Authorize(Roles = "Productor")]
    [HttpGet("mias")]
    [ProducesResponseType<IReadOnlyCollection<PublicacionResumenResponse>>(
        StatusCodes.Status200OK)]
    public async Task<
        ActionResult<IReadOnlyCollection<PublicacionResumenResponse>>>
        ObtenerMias(CancellationToken cancellationToken)
    {
        return Ok(await publicacionService.ObtenerDelProductorAsync(
            ObtenerIdUsuario(),
            cancellationToken));
    }

    [Authorize(Roles = "Productor")]
    [HttpPost]
    [ProducesResponseType<PublicacionDetalleResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PublicacionDetalleResponse>> Crear(
        CrearPublicacionRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var publicacion = await publicacionService.CrearAsync(
                ObtenerIdUsuario(),
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(ObtenerDetalle),
                new { idPublicacion = publicacion.IdPublicacion },
                publicacion);
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

    [Authorize(Roles = "Productor")]
    [HttpPut("{idPublicacion:int}")]
    [ProducesResponseType<PublicacionDetalleResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PublicacionDetalleResponse>> Actualizar(
        int idPublicacion,
        ActualizarPublicacionRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await publicacionService.ActualizarAsync(
                ObtenerIdUsuario(),
                idPublicacion,
                request,
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
    }

    private int ObtenerIdUsuario()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
