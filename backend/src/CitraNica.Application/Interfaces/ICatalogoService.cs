using CitraNica.Application.Catalog;

namespace CitraNica.Application.Interfaces;

public interface ICatalogoService
{
    Task<IReadOnlyCollection<CategoriaResponse>> ObtenerCategoriasAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ProductoCatalogoResponse>> ObtenerProductosAsync(
        int? idCategoria,
        CancellationToken cancellationToken = default);

    Task<ProductoCatalogoResponse?> ObtenerProductoPorIdAsync(
        int idProducto,
        CancellationToken cancellationToken = default);

    Task<CategoriaResponse> CrearCategoriaAsync(
        CrearCategoriaRequest request,
        CancellationToken cancellationToken = default);

    Task<ProductoCatalogoResponse> CrearProductoAsync(
        CrearProductoCatalogoRequest request,
        CancellationToken cancellationToken = default);
}
