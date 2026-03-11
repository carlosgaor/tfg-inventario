using Inventario.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<UsuarioRol> UsuarioRoles => Set<UsuarioRol>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<OrdenCompra> OrdenesCompra => Set<OrdenCompra>();
    public DbSet<LineaOrden> LineasOrden => Set<LineaOrden>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("usuarios");
            e.HasKey(x => x.IdUsuario);
            e.Property(x => x.IdUsuario).HasColumnName("id_usuario");
            e.Property(x => x.Nombre).HasColumnName("nombre");
            e.Property(x => x.Email).HasColumnName("email");
            e.Property(x => x.PasswordHash).HasColumnName("password_hash");
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<Rol>(e =>
        {
            e.ToTable("roles");
            e.HasKey(x => x.IdRol);
            e.Property(x => x.IdRol).HasColumnName("id_rol");
            e.Property(x => x.Nombre).HasColumnName("nombre");
        });

        modelBuilder.Entity<UsuarioRol>(e =>
        {
            e.ToTable("usuario_roles");
            e.HasKey(x => new { x.IdUsuario, x.IdRol });
            e.Property(x => x.IdUsuario).HasColumnName("id_usuario");
            e.Property(x => x.IdRol).HasColumnName("id_rol");
        });

        modelBuilder.Entity<Categoria>(e =>
        {
            e.ToTable("categorias");
            e.HasKey(x => x.IdCategoria);
            e.Property(x => x.IdCategoria).HasColumnName("id_categoria");
            e.Property(x => x.Nombre).HasColumnName("nombre");
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<Producto>(e =>
        {
            e.ToTable("productos");
            e.HasKey(x => x.IdProducto);

            e.Property(x => x.IdProducto).HasColumnName("id_producto");
            e.Property(x => x.Nombre).HasColumnName("nombre");
            e.Property(x => x.IdCategoria).HasColumnName("id_categoria");
            e.Property(x => x.Codigo).HasColumnName("codigo");
            e.Property(x => x.StockMinimo).HasColumnName("stock_minimo");
            e.Property(x => x.StockActual).HasColumnName("stock_actual");
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<OrdenCompra>(e =>
        {
            e.ToTable("ordenes_compra");
            e.HasKey(x => x.IdOrden);
            e.Property(x => x.IdOrden).HasColumnName("id_orden");
            e.Property(x => x.Fecha).HasColumnName("fecha");
            e.Property(x => x.Estado).HasColumnName("estado");
            e.Property(x => x.Proveedor).HasColumnName("proveedor");
            e.Property(x => x.IdUsuario).HasColumnName("id_usuario");
        });

        modelBuilder.Entity<LineaOrden>(e =>
        {
            e.ToTable("lineas_orden");
            e.HasKey(x => x.IdLinea);
            e.Property(x => x.IdLinea).HasColumnName("id_linea");
            e.Property(x => x.IdOrden).HasColumnName("id_orden");
            e.Property(x => x.IdProducto).HasColumnName("id_producto");
            e.Property(x => x.Cantidad).HasColumnName("cantidad");
            e.Property(x => x.PrecioEstimado).HasColumnName("precio_estimado");
        });

    }
}