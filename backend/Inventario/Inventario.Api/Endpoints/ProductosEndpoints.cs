using Inventario.Api.Contracts;
using Inventario.Api.Data;
using Inventario.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Api.Endpoints;

public static class ProductosEndpoints
{
    public static void MapProductos(this WebApplication app)
    {
        var group = app.MapGroup("/productos").RequireAuthorization();
        group.MapGet("", async (AppDbContext db) =>
            await db.Productos
                .Where(p => p.Activo)
                .OrderBy(p => p.Nombre)
                .ToListAsync());

        // Detalle - cualquiera
        group.MapGet("/{id:int}", async (int id, AppDbContext db) =>
        {
            var p = await db.Productos.FirstOrDefaultAsync(x => x.IdProducto == id);
            return p is null ? Results.NotFound() : Results.Ok(p);
        });

        // Crear - admin
        group.MapPost("", [Authorize(Roles = "Admin")] async (CreateProductoRequest req, AppDbContext db) =>
        {
            var categoriaOk = await db.Categorias.AnyAsync(c => c.IdCategoria == req.IdCategoria && c.Activo);
            if (!categoriaOk) return Results.BadRequest(new { message = "La categoría no existe o está inactiva." });

            if (!string.IsNullOrWhiteSpace(req.Codigo))
            {
                var codeExists = await db.Productos.AnyAsync(p => p.Codigo == req.Codigo);
                if (codeExists) return Results.Conflict(new { message = "El código ya existe." });
            }

            var producto = new Producto
            {
                Nombre = req.Nombre,
                IdCategoria = req.IdCategoria,
                Codigo = string.IsNullOrWhiteSpace(req.Codigo) ? null : req.Codigo,
                StockMinimo = req.StockMinimo,
                StockActual = 0,
                Activo = true
            };

            db.Productos.Add(producto);
            await db.SaveChangesAsync();

            return Results.Created($"/productos/{producto.IdProducto}", producto);
        });

        //Editar - admin
        group.MapPut("/{id:int}", [Authorize(Roles = "Admin")] async (int id, UpdateProductoRequest req, AppDbContext db) =>
        {
            var producto = await db.Productos.FirstOrDefaultAsync(p => p.IdProducto == id);
            if (producto is null) return Results.NotFound();

            var categoriaOk = await db.Categorias.AnyAsync(c => c.IdCategoria == req.IdCategoria && c.Activo);
            if (!categoriaOk) return Results.BadRequest(new { message = "La categoría no existe o está inactiva." });

            if (!string.IsNullOrWhiteSpace(req.Codigo))
            {
                var codeExists = await db.Productos.AnyAsync(p => p.Codigo == req.Codigo && p.IdProducto != id);
                if (codeExists) return Results.Conflict(new { message = "El código ya existe." });
            }

            producto.Nombre = req.Nombre;
            producto.IdCategoria = req.IdCategoria;
            producto.Codigo = string.IsNullOrWhiteSpace(req.Codigo) ? null : req.Codigo;
            producto.StockMinimo = req.StockMinimo;
            producto.Activo = req.Activo;

            await db.SaveChangesAsync();
            return Results.Ok(producto);
        });

        // Borrae - admin
        group.MapDelete("/{id:int}", [Authorize(Roles = "Admin")] async (int id, AppDbContext db) =>
        {
            var producto = await db.Productos.FirstOrDefaultAsync(p => p.IdProducto == id);
            if (producto is null) return Results.NotFound();

            producto.Activo = false;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}