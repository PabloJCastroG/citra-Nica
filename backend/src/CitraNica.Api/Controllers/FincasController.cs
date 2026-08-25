using System.Security.Claims;
using CitraNica.Application.Farms;
using CitraNica.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CitraNica.Api.Controllers;

[ApiController]
[Authorize(Roles = "Productor")]
[Route("api/fincas")]
public sealed class FincasController(IFincaService fincaService) : ControllerBase
{
    [HttpGet("mia")]
    [ProducesResponseType<FincaResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FincaResponse>> ObtenerMiFinca(
        CancellationToken cancellationToken)
    {
        var finca = await fincaService.ObtenerPorProductorAsync(
            ObtenerIdUsuario(),
            cancellationToken);

        return finca is null
            ? NotFound(new { mensaje = "El productor todavía no tiene una finca." })
            : Ok(finca);
    }

    [HttpPut("mia")]
    [ProducesResponseType<FincaResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FincaResponse>> GuardarMiFinca(
        GuardarFincaRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await fincaService.GuardarAsync(
                ObtenerIdUsuario(),
                request,
                cancellationToken));
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mensaje = exception.Message });
        }
    }

    private int ObtenerIdUsuario()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
