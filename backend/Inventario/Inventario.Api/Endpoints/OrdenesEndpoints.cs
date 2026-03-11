using Inventario.Api.Contracts;
using Inventario.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Inventario.Api.Endpoints;

public static class OrdenesEndpoints
{
    public static void MapOrdenes(this WebApplication app)
    {
        var group = app.MapGroup("/ordenes").RequireAuthorization();

        // Listar ordenes
        group.MapGet("", async (AppDbContext db) =>
           await db.OrdenesCompra.OrderByDescending(o => o.Fecha).ToListAsync());

        //Crear orden (borrador)
        group.MapPost("", async (CrearOrdenRequest req, AppDbContext db, ClaimsPrincipal user) =>
        {
            var userIdStr = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Results.Unauthorized();

            var orden = new Models.OrdenCompra
            {
                Proveedor = string.IsNullOrWhiteSpace(req.Proveedor) ? null : req.Proveedor,
                Estado = "BORRADOR",
                IdUsuario = userId
            };

            db.OrdenesCompra.Add(orden);
            await db.SaveChangesAsync();

            return Results.Created($"/ordenes/{orden.IdOrden}", orden);
        });

        // Añadir línea (orden)
        group.MapPost("/{idOrden:int}/lineas", async (int idOrden, AgregarLineaOrdenRequest req, AppDbContext db) =>
        {
            var orden = await db.OrdenesCompra.FirstOrDefaultAsync(o => o.IdOrden == idOrden);
            if (orden is null) return Results.NotFound(new { message = "Orden no existe" });

            if (orden.Estado != "BORRADOR")
                return Results.BadRequest(new { message = "Solo se pueden añadir líneas en estado BORRADOR" });

            var prodOk = await db.Productos.AnyAsync(p => p.IdProducto == req.IdProducto && p.Activo);
            if (!prodOk) return Results.BadRequest(new { message = "Producto no existe o está inactivo" });

            if (req.Cantidad <= 0) return Results.BadRequest(new { message = "Cantidad inválida" });

            var linea = new Models.LineaOrden
            {
                IdOrden = idOrden,
                IdProducto = req.IdProducto,
                Cantidad = req.Cantidad,
                PrecioEstimado = req.PrecioEstimado
            };

            db.LineasOrden.Add(linea);
            await db.SaveChangesAsync();

            return Results.Created($"/ordenes/{idOrden}/lineas/{linea.IdLinea}", linea);
        });

        // ver detalles
        group.MapGet("/{idOrden:int}", async (int idOrden, AppDbContext db) =>
        {
            var orden = await db.OrdenesCompra.FirstOrDefaultAsync(o => o.IdOrden == idOrden);
            if (orden is null) return Results.NotFound();

            var lineas = await db.Database.SqlQuery<LineaDetalle>($@"
                SELECT
                  l.id_linea    AS IdLinea,
                  l.id_producto AS IdProducto,
                  p.nombre      AS Producto,
                  l.cantidad    AS Cantidad,
                  l.precio_estimado AS PrecioEstimado
                FROM lineas_orden l
                JOIN productos p ON p.id_producto = l.id_producto
                WHERE l.id_orden = {idOrden}
                ORDER BY l.id_linea
            ").ToListAsync();

            return Results.Ok(new { orden, lineas });
        });

        // cambiar estado
        group.MapPut("/{idOrden:int}/estado/{estado}", async (int idOrden, string estado, AppDbContext db) =>
        {
            var est = estado.Trim().ToUpperInvariant();
            if (est is not ("BORRADOR" or "ENVIADA" or "RECIBIDA" or "CANCELADA"))
                return Results.BadRequest(new { message = "Estado inválido" });

            var orden = await db.OrdenesCompra.FirstOrDefaultAsync(o => o.IdOrden == idOrden);
            if (orden is null) return Results.NotFound();

            orden.Estado = est;
            await db.SaveChangesAsync();
            return Results.Ok(orden);
        });

        group.MapPut("/{idOrden:int}/recibir", [Authorize(Roles = "Admin")] async (int idOrden, AppDbContext db, ClaimsPrincipal user) =>
        {
            var userIdStr = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Results.Unauthorized();

            var orden = await db.OrdenesCompra.FirstOrDefaultAsync(o => o.IdOrden == idOrden);
            if (orden is null) return Results.NotFound(new { message = "Orden no existe" });

            if (orden.Estado != "ENVIADA")
                return Results.BadRequest(new { message = "Solo se puede recibir una orden en estado ENVIADA." });

            var lineas = await db.LineasOrden.Where(l => l.IdOrden == idOrden).ToListAsync();
            if (lineas.Count == 0)
                return Results.BadRequest(new { message = "La orden no tiene líneas." });

            await using var tx = await db.Database.BeginTransactionAsync();

            try
            {
                foreach (var l in lineas)
                {
                    await db.Database.ExecuteSqlRawAsync(
                        "CALL sp_registrar_movimiento({0}, {1}, {2}, {3}, {4});",
                        l.IdProducto,
                        "ENTRADA",
                        l.Cantidad,
                        userId,
                        $"Recepción orden #{idOrden}"
                    );
                }
                orden.Estado = "RECIBIDA";
                await db.SaveChangesAsync();

                await tx.CommitAsync();

                return Results.Ok(new { message = "Orden recibida y stock actualizado." });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return Results.BadRequest(new { message = ex.Message });
            }
        });

    }
    public record LineaDetalle(int IdLinea, int IdProducto, string Producto, int Cantidad, decimal? PrecioEstimado);
}