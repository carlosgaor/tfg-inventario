namespace Inventario.Api.Models;
public class Producto
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = "";
    public int IdCategoria { get; set; }
    public string? Codigo { get; set; }
    public int StockMinimo { get; set; }
    public int StockActual { get; set; }
    public bool Activo { get; set; }
}