namespace Inventario.Api.Contracts;
public record CreateProductoRequest(
    string Nombre,
    int IdCategoria,
    string? Codigo,
    int StockMinimo
);