using CitraNica.Application.Catalog;
using CitraNica.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitraNica.Api.Controllers;

[ApiController]
[Route("api/catalogo")]
public sealed class CatalogoController(ICatalogoService catalogoService)
    : ControllerBase
{
    [HttpGet("categorias")]
    [ProducesResponseType<IReadOnlyCollection<CategoriaResponse>>(
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<CategoriaResponse>>>
        ObtenerCategorias(CancellationToken cancellationToken)
    {
        return Ok(await catalogoService.ObtenerCategoriasAsync(cancellationToken));
    }

    [HttpGet("productos")]
    [ProducesResponseType<IReadOnlyCollection<ProductoCatalogoResponse>>(
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ProductoCatalogoResponse>>>
        ObtenerProductos(
            [FromQuery] int? idCategoria,
            CancellationToken cancellationToken)
    {
        return Ok(await catalogoService.ObtenerProductosAsync(
            idCategoria,
            cancellationToken));
    }

    [HttpGet("productos/{idProducto:int}")]
    [ProducesResponseType<ProductoCatalogoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductoCatalogoResponse>> ObtenerProducto(
        int idProducto,
        CancellationToken cancellationToken)
    {
        var producto = await catalogoService.ObtenerProductoPorIdAsync(
            idProducto,
            cancellationToken);

        return producto is null
            ? NotFound(new { mensaje = "Producto no encontrado." })
            : Ok(producto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("categorias")]
    [ProducesResponseType<CategoriaResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CategoriaResponse>> CrearCategoria(
        CrearCategoriaRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var categoria = await catalogoService.CrearCategoriaAsync(
                request,
                cancellationToken);
            return StatusCode(StatusCodes.Status201Created, categoria);
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

    [Authorize(Roles = "Admin")]
    [HttpPost("productos")]
    [ProducesResponseType<ProductoCatalogoResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProductoCatalogoResponse>> CrearProducto(
        CrearProductoCatalogoRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var producto = await catalogoService.CrearProductoAsync(
                request,
                cancellationToken);
            return StatusCode(StatusCodes.Status201Created, producto);
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
}
