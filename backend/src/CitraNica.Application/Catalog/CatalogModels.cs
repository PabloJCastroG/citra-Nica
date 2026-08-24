using System.ComponentModel.DataAnnotations;

namespace CitraNica.Application.Catalog;

public sealed record CategoriaResponse(
    int IdCategoria,
    string NombreCategoria);

public sealed record ProductoCatalogoResponse(
    int IdProducto,
    string NombreProducto,
    int IdCategoria,
    string NombreCategoria);

public sealed class CrearCategoriaRequest
{
    [Required, MaxLength(50)]
    public string NombreCategoria { get; init; } = null!;
}

public sealed class CrearProductoCatalogoRequest
{
    [Required, MaxLength(80)]
    public string NombreProducto { get; init; } = null!;

    [Range(1, int.MaxValue)]
    public int IdCategoria { get; init; }
}
