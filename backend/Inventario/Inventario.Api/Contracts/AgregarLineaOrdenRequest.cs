namespace Inventario.Api.Contracts;
public record AgregarLineaOrdenRequest(int IdProducto, int Cantidad, decimal? PrecioEstimado);