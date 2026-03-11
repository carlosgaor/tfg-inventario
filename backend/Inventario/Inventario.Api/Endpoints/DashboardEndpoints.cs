using Inventario.Api.Data;
using Microsoft.EntityFrameworkCore;
namespace Inventario.Api.Endpoints;

public static class DashboardEndpoints
{
    public static void MapDashboard(this WebApplication app)
    {
        var group = app.MapGroup("/dashboard").RequireAuthorization();

        group.MapGet("", async (AppDbContext db) =>
        {
            var totalProductos = await db.Productos.CountAsync(p => p.Activo);
            var stockBajo = await db.Productos.CountAsync(p => p.Activo && p.StockActual < p.StockMinimo);

            var ordenesBorrador = await db.OrdenesCompra.CountAsync(o => o.Estado == "BORRADOR");
            var ordenesEnviadas = await db.OrdenesCompra.CountAsync(o => o.Estado == "ENVIADA");

            var desde = DateTime.Today.AddDays(-7);
            var movimientos7d = await db.Database.SqlQuery<int>($@"
                SELECT COUNT(*) AS Value
                FROM movimientos_stock
                WHERE fecha >= {desde}
            ").FirstAsync();

            return Results.Ok(new
            {
                totalProductos,
                stockBajo,
                ordenesBorrador,
                ordenesEnviadas,
                movimientos7d
            });
        });

        group.MapGet("/charts", async (int days, AppDbContext db) =>
        {
            if (days <= 0 || days > 365) days = 30;

            var desde = DateTime.Today.AddDays(-(days - 1));

            var serieTipos = await db.Database.SqlQuery<MovsDiaTipo>($@"
    SELECT 
        DATE(m.fecha) AS Dia,
        SUM(CASE WHEN m.tipo = 'ENTRADA' THEN m.cantidad ELSE 0 END) AS Entradas,
        SUM(CASE WHEN m.tipo = 'SALIDA'  THEN m.cantidad ELSE 0 END) AS Salidas
    FROM movimientos_stock m
    WHERE m.fecha >= {desde}
    GROUP BY DATE(m.fecha)
    ORDER BY DATE(m.fecha)
").ToListAsync();

            var topSalidas = await db.Database.SqlQuery<TopProducto>($@"
        SELECT
            p.nombre AS Nombre,
            SUM(m.cantidad) AS Total
        FROM movimientos_stock m
        JOIN productos p ON p.id_producto = m.id_producto
        WHERE m.fecha >= {desde}
          AND m.tipo = 'SALIDA'
        GROUP BY p.nombre
        ORDER BY SUM(m.cantidad) DESC
        LIMIT 5
    ").ToListAsync();

            var map = serieTipos.ToDictionary(x => x.Dia.Date, x => x);

            var filled = new List<MovsDiaTipoOut>();
            for (var i = 0; i < days; i++)
            {
                var d = desde.AddDays(i).Date;

                if (map.TryGetValue(d, out var v))
                    filled.Add(new MovsDiaTipoOut(d, v.Entradas, v.Salidas));
                else
                    filled.Add(new MovsDiaTipoOut(d, 0, 0));
            }

            return Results.Ok(new
            {
                days,
                desde,
                hasta = DateTime.Today,
                movimientosPorDia = filled,
                topSalidas
            });

        });
    }
}
record TopProducto(string Nombre, long Total);
record MovsDiaTipo(DateTime Dia, long Entradas, long Salidas);
record MovsDiaTipoOut(DateTime Dia, long Entradas, long Salidas);