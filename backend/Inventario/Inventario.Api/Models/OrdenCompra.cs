namespace Inventario.Api.Models;
public class OrdenCompra
{
    public int IdOrden { get; set; }
    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = "BORRADOR";
    public string? Proveedor { get; set; }
    public int IdUsuario { get; set; }
}