namespace Inventario.Api.Contracts;
public record RegistrarMovimientoRequest(
    int IdProducto,
    string Tipo,
    int Cantidad,
    string? Nota
);