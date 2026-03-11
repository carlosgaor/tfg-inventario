using Inventario.Api.Data;
using Inventario.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Api.Endpoints;

public static class CategoriasEndpoints
{
    public static void MapCategorias(this WebApplication app)
    {
        var group = app.MapGroup("/categorias").RequireAuthorization();

        group.MapGet("", async (AppDbContext db) =>
            await db.Categorias.Where(c => c.Activo).ToListAsync());

        group.MapPost("", [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")] async (Categoria categoria, AppDbContext db) =>
        {
            db.Categorias.Add(categoria);
            await db.SaveChangesAsync();
            return Results.Created($"/categorias/{categoria.IdCategoria}", categoria);
        });

        group.MapPut("/{id}", [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")] async (int id, Categoria input, AppDbContext db) =>
        {
            var categoria = await db.Categorias.FindAsync(id);
            if (categoria is null) return Results.NotFound();

            categoria.Nombre = input.Nombre;
            categoria.Activo = input.Activo;

            await db.SaveChangesAsync();
            return Results.Ok(categoria);
        });

        group.MapDelete("/{id}", [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")] async (int id, AppDbContext db) =>
        {
            var categoria = await db.Categorias.FindAsync(id);
            if (categoria is null) return Results.NotFound();

            categoria.Activo = false;
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}