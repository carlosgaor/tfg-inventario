using Inventario.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Api.Endpoints;

public static class InformesEndpoints
{
    public static void MapInformes(this WebApplication app)
    {
        var group = app.MapGroup("/informes").RequireAuthorization();

        group.MapGet("/stock-bajo", async (AppDbContext db) =>
        {
            var data = await db.Database.SqlQuery<StockBajoItem>($@"
                SELECT
                    p.id_producto   AS IdProducto,
                    p.nombre        AS Nombre,
                    p.stock_actual  AS StockActual,
                    p.stock_minimo  AS StockMinimo
                FROM productos p
                WHERE p.activo = 1
                  AND p.stock_actual < p.stock_minimo
                ORDER BY (p.stock_minimo - p.stock_actual) DESC, p.nombre
            ").ToListAsync();

            return Results.Ok(data);
        });

        group.MapGet("/movimientos", async (DateTime? desde, DateTime? hasta, AppDbContext db) =>
        {
            var d = (desde ?? DateTime.Today.AddDays(-7)).Date;
            var h = (hasta ?? DateTime.Today).Date.AddDays(1).AddTicks(-1);

            var data = await db.Database.SqlQuery<MovimientoItem>($@"
        SELECT
            m.id_movimiento AS IdMovimiento,
            m.fecha         AS Fecha,
            m.tipo          AS Tipo,
            m.cantidad      AS Cantidad,
            m.nota          AS Nota,
            p.id_producto   AS IdProducto,
            p.nombre        AS Producto
        FROM movimientos_stock m
        JOIN productos p ON p.id_producto = m.id_producto
        WHERE m.fecha BETWEEN {d} AND {h}
        ORDER BY m.fecha DESC
    ").ToListAsync();

            return Results.Ok(new { desde = d, hasta = h, total = data.Count, items = data });
        });
    }

    public record StockBajoItem(int IdProducto, string Nombre, int StockActual, int StockMinimo);
    public record MovimientoItem(
    int IdMovimiento,
    DateTime Fecha,
    string Tipo,
    int Cantidad,
    string? Nota,
    int IdProducto,
    string Producto
    );
}