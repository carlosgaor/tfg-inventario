namespace Inventario.Api.Models;
public class Categoria
{
    public int IdCategoria { get; set; }
    public string Nombre { get; set; } = "";
    public bool Activo { get; set; }
}