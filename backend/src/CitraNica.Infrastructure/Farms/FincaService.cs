using CitraNica.Application.Farms;
using CitraNica.Application.Interfaces;
using CitraNica.Domain.Entities;
using CitraNica.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CitraNica.Infrastructure.Farms;

public sealed class FincaService(CitraNicaDbContext dbContext) : IFincaService
{
    public Task<FincaResponse?> ObtenerPorProductorAsync(
        int idProductor,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Fincas
            .AsNoTracking()
            .Where(finca => finca.IdProductor == idProductor)
            .Select(finca => CrearRespuesta(finca))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<FincaResponse> GuardarAsync(
        int idProductor,
        GuardarFincaRequest request,
        CancellationToken cancellationToken = default)
    {
        var esProductor = await dbContext.Usuarios.AnyAsync(
            usuario =>
                usuario.IdUsuario == idProductor
                && usuario.TipoUsuario == TipoUsuario.Productor,
            cancellationToken);

        if (!esProductor)
        {
            throw new UnauthorizedAccessException(
                "La cuenta no corresponde a un productor.");
        }

        var nombre = request.NombreFinca.Trim();
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre de la finca es obligatorio.");
        }

        var finca = await dbContext.Fincas.SingleOrDefaultAsync(
            registrada => registrada.IdProductor == idProductor,
            cancellationToken);

        if (finca is null)
        {
            finca = new Finca { IdProductor = idProductor };
            dbContext.Fincas.Add(finca);
        }

        finca.NombreFinca = nombre;
        finca.Latitud = request.Latitud;
        finca.Longitud = request.Longitud;
        finca.HistoriaCultivo = request.HistoriaCultivo?.Trim();

        await dbContext.SaveChangesAsync(cancellationToken);
        return CrearRespuesta(finca);
    }

    private static FincaResponse CrearRespuesta(Finca finca)
    {
        return new FincaResponse(
            finca.IdFinca,
            finca.IdProductor,
            finca.NombreFinca,
            finca.Latitud,
            finca.Longitud,
            finca.HistoriaCultivo);
    }
}
