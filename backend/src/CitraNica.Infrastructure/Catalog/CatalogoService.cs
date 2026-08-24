using CitraNica.Application.Catalog;
using CitraNica.Application.Interfaces;
using CitraNica.Domain.Entities;
using CitraNica.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CitraNica.Infrastructure.Catalog;

public sealed class CatalogoService(CitraNicaDbContext dbContext) : ICatalogoService
{
    public async Task<IReadOnlyCollection<CategoriaResponse>> ObtenerCategoriasAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Categorias
            .AsNoTracking()
            .OrderBy(categoria => categoria.NombreCategoria)
            .Select(categoria => new CategoriaResponse(
                categoria.IdCategoria,
                categoria.NombreCategoria))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ProductoCatalogoResponse>>
        ObtenerProductosAsync(
            int? idCategoria,
            CancellationToken cancellationToken = default)
    {
        var consulta =
            from producto in dbContext.CatalogoProductos.AsNoTracking()
            join categoria in dbContext.Categorias.AsNoTracking()
                on producto.IdCategoria equals categoria.IdCategoria
            where !idCategoria.HasValue || producto.IdCategoria == idCategoria.Value
            orderby producto.NombreProducto
            select new ProductoCatalogoResponse(
                producto.IdProducto,
                producto.NombreProducto,
                categoria.IdCategoria,
                categoria.NombreCategoria);

        return await consulta.ToListAsync(cancellationToken);
    }

    public Task<ProductoCatalogoResponse?> ObtenerProductoPorIdAsync(
        int idProducto,
        CancellationToken cancellationToken = default)
    {
        return (
            from producto in dbContext.CatalogoProductos.AsNoTracking()
            join categoria in dbContext.Categorias.AsNoTracking()
                on producto.IdCategoria equals categoria.IdCategoria
            where producto.IdProducto == idProducto
            select new ProductoCatalogoResponse(
                producto.IdProducto,
                producto.NombreProducto,
                categoria.IdCategoria,
                categoria.NombreCategoria))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<CategoriaResponse> CrearCategoriaAsync(
        CrearCategoriaRequest request,
        CancellationToken cancellationToken = default)
    {
        var nombre = LimpiarNombre(request.NombreCategoria);
        var existe = await dbContext.Categorias.AnyAsync(
            categoria => categoria.NombreCategoria == nombre,
            cancellationToken);

        if (existe)
        {
            throw new InvalidOperationException("La categoría ya existe.");
        }

        var categoria = new Categoria
        {
            NombreCategoria = nombre
        };

        dbContext.Categorias.Add(categoria);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CategoriaResponse(
            categoria.IdCategoria,
            categoria.NombreCategoria);
    }

    public async Task<ProductoCatalogoResponse> CrearProductoAsync(
        CrearProductoCatalogoRequest request,
        CancellationToken cancellationToken = default)
    {
        var categoria = await dbContext.Categorias
            .SingleOrDefaultAsync(
                registrada => registrada.IdCategoria == request.IdCategoria,
                cancellationToken);

        if (categoria is null)
        {
            throw new KeyNotFoundException("La categoría indicada no existe.");
        }

        var nombre = LimpiarNombre(request.NombreProducto);
        var existe = await dbContext.CatalogoProductos.AnyAsync(
            producto =>
                producto.IdCategoria == request.IdCategoria
                && producto.NombreProducto == nombre,
            cancellationToken);

        if (existe)
        {
            throw new InvalidOperationException(
                "El producto ya existe dentro de esa categoría.");
        }

        var producto = new ProductoCatalogo
        {
            NombreProducto = nombre,
            IdCategoria = request.IdCategoria
        };

        dbContext.CatalogoProductos.Add(producto);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ProductoCatalogoResponse(
            producto.IdProducto,
            producto.NombreProducto,
            categoria.IdCategoria,
            categoria.NombreCategoria);
    }

    private static string LimpiarNombre(string nombre)
    {
        var nombreLimpio = nombre.Trim();

        if (string.IsNullOrWhiteSpace(nombreLimpio))
        {
            throw new ArgumentException("El nombre no puede estar vacío.");
        }

        return nombreLimpio;
    }
}
