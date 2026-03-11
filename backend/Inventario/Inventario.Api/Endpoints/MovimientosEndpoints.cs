using Inventario.Api.Contracts;
using Inventario.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Inventario.Api.Endpoints;

public static class MovimientosEndpoints
{
    public static void MapMovimientos(this WebApplication app)
    {
        var group = app.MapGroup("/movimientos").RequireAuthorization();

        group.MapPost("", [Authorize(Roles = "Admin,Empleado")] async (
            RegistrarMovimientoRequest req,
            AppDbContext db,
            ClaimsPrincipal user) =>
        {
            // validación
            var tipo = req.Tipo?.Trim().ToUpperInvariant();
            if (tipo is not ("ENTRADA" or "SALIDA" or "AJUSTE"))
                return Results.BadRequest(new { message = "Tipo inválido. Usa ENTRADA, SALIDA o AJUSTE." });

            if (req.Cantidad <= 0)
                return Results.BadRequest(new { message = "La cantidad debe ser mayor que 0." });

            var userIdStr = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return Results.Unauthorized();

            try
            {
                await db.Database.ExecuteSqlRawAsync(
                    "CALL sp_registrar_movimiento({0}, {1}, {2}, {3}, {4});",
                    req.IdProducto,
                    tipo,
                    req.Cantidad,
                    userId,
                    req.Nota
                );

                var stock = await db.Database.SqlQuery<int>($@"
                    SELECT stock_actual AS Value
                    FROM productos
                    WHERE id_producto = {req.IdProducto}
                    LIMIT 1
                ").FirstAsync();

                return Results.Ok(new { message = "Movimiento registrado", stockActual = stock });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        });
    }
}