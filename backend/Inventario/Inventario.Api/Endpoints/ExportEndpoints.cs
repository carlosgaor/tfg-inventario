using Inventario.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Inventario.Api.Endpoints;

public static class ExportEndpoints
{
    public static void MapExport(this WebApplication app)
    {
        var group = app.MapGroup("/export").RequireAuthorization();

        group.MapGet("/stock-bajo", async (AppDbContext db) =>
        {
            var productos = await db.Productos
                .Where(p => p.Activo && p.StockActual < p.StockMinimo)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine("IdProducto,Nombre,StockActual,StockMinimo");

            foreach (var p in productos)
            {
                sb.AppendLine($"{p.IdProducto},{p.Nombre},{p.StockActual},{p.StockMinimo}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());

            return Results.File(bytes,
                "text/csv",
                $"stock_bajo_{DateTime.Now:yyyyMMddHHmmss}.csv");
        });
    }
}