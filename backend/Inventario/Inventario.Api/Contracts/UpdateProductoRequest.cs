namespace Inventario.Api.Contracts;
public record UpdateProductoRequest(
    string Nombre,
    int IdCategoria,
    string? Codigo,
    int StockMinimo,
    bool Activo
);