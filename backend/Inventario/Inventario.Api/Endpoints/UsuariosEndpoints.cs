using Inventario.Api.Data;
using Inventario.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Api.Endpoints;
public static class UsuariosEndpoints
{
    public static void MapUsuarios(this WebApplication app)
    {
        var group = app.MapGroup("/usuarios").RequireAuthorization("AdminOnly");

        group.MapGet("", async (AppDbContext db) =>
        {
            var users = await db.Usuarios
                .OrderBy(u => u.Email)
                .Select(u => new
                {
                    u.IdUsuario,
                    u.Nombre,
                    u.Email,
                    u.Activo
                })
                .ToListAsync();

            var rolesByUser = await db.Database.SqlQuery<UserRoleRow>($@"
                SELECT ur.id_usuario AS IdUsuario, r.nombre AS Rol
                FROM usuario_roles ur
                JOIN roles r ON r.id_rol = ur.id_rol
            ").ToListAsync();

            var roleMap = rolesByUser
                .GroupBy(x => x.IdUsuario)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Rol).FirstOrDefault() ?? "Empleado");

            var result = users.Select(u => new
            {
                u.IdUsuario,
                u.Nombre,
                u.Email,
                u.Activo,
                Rol = roleMap.TryGetValue(u.IdUsuario, out var r) ? r : "Empleado"
            });

            return Results.Ok(result);
        });

        group.MapPost("", async (CreateUserRequest req, AppDbContext db) =>
        {
            req = req with
            {
                Email = req.Email.Trim().ToLowerInvariant(),
                Nombre = req.Nombre.Trim()
            };

            if (string.IsNullOrWhiteSpace(req.Nombre) || string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
                return Results.BadRequest(new { message = "Nombre, Email y Password son obligatorios." });

            if (req.Password.Length < 6)
                return Results.BadRequest(new { message = "La contraseña debe tener al menos 6 caracteres." });

            var exists = await db.Usuarios.AnyAsync(u => u.Email == req.Email);
            if (exists) return Results.Conflict(new { message = "Ya existe un usuario con ese email." });

            var roleName = (req.Rol ?? "Empleado").Trim();
            if (roleName is not ("Admin" or "Empleado"))
                return Results.BadRequest(new { message = "Rol inválido. Usa Admin o Empleado." });

            var rol = await db.Roles.FirstAsync(r => r.Nombre == roleName);

            var user = new Usuario
            {
                Nombre = req.Nombre,
                Email = req.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
                Activo = true
            };

            db.Usuarios.Add(user);
            await db.SaveChangesAsync();

            db.UsuarioRoles.Add(new UsuarioRol
            {
                IdUsuario = user.IdUsuario,
                IdRol = rol.IdRol
            });

            await db.SaveChangesAsync();

            return Results.Created($"/usuarios/{user.IdUsuario}", new
            {
                user.IdUsuario,
                user.Nombre,
                user.Email,
                user.Activo,
                Rol = roleName
            });
        });

        group.MapPut("/{id:int}/activo", async (int id, SetActivoRequest req, AppDbContext db) =>
        {
            var user = await db.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (user is null) return Results.NotFound();

            user.Activo = req.Activo;
            await db.SaveChangesAsync();
            return Results.Ok(new { user.IdUsuario, user.Activo });
        });

        group.MapPut("/{id:int}/password", async (int id, ResetPasswordRequest req, AppDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(req.NewPassword) || req.NewPassword.Length < 6)
                return Results.BadRequest(new { message = "NewPassword mínimo 6 caracteres." });

            var user = await db.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (user is null) return Results.NotFound();

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);
            await db.SaveChangesAsync();

            return Results.Ok(new { message = "Password actualizado" });
        });

        group.MapPut("/{id:int}/rol", async (int id, SetRoleRequest req, AppDbContext db) =>
        {
            var roleName = (req.Rol ?? "").Trim();
            if (roleName is not ("Admin" or "Empleado"))
                return Results.BadRequest(new { message = "Rol inválido. Usa Admin o Empleado." });

            var user = await db.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (user is null) return Results.NotFound();

            var rol = await db.Roles.FirstAsync(r => r.Nombre == roleName);

            // Asegurar 1 rol por usuario (modelo simple)
            var ur = await db.UsuarioRoles.FirstOrDefaultAsync(x => x.IdUsuario == id);
            if (ur is null)
            {
                db.UsuarioRoles.Add(new UsuarioRol { IdUsuario = id, IdRol = rol.IdRol });
            }
            else
            {
                ur.IdRol = rol.IdRol;
            }

            await db.SaveChangesAsync();
            return Results.Ok(new { message = "Rol actualizado", rol = roleName });
        });
    }

    // dtos
    public record CreateUserRequest(string Nombre, string Email, string Password, string? Rol);
    public record SetActivoRequest(bool Activo);
    public record ResetPasswordRequest(string NewPassword);
    public record SetRoleRequest(string Rol);

    // Mapper SQL
    public record UserRoleRow(int IdUsuario, string Rol);
}