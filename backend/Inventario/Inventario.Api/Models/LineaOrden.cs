namespace Inventario.Api.Models;
public class LineaOrden
{
    public int IdLinea { get; set; }
    public int IdOrden { get; set; }
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal? PrecioEstimado { get; set; }
}