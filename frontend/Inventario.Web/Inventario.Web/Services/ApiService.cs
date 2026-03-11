using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;

namespace Inventario.Web.Services;
public class ApiService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public ApiService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }
    private void ApplyTokenToHttpClient(string token)
    {
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
    public async Task SetTokenAsync(string token, string? role = null, string? email = null)
    {
        await _localStorage.SetItemAsync("token", token);
        await _localStorage.SetItemAsync("role", role ?? "");
        await _localStorage.SetItemAsync("email", email ?? "");
        ApplyTokenToHttpClient(token);
    }
    public async Task<bool> TryLoadTokenAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("token");
        if (string.IsNullOrWhiteSpace(token)) return false;

        ApplyTokenToHttpClient(token);
        return true;
    }
    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("token");
        await _localStorage.RemoveItemAsync("role");
        await _localStorage.RemoveItemAsync("email");
        _http.DefaultRequestHeaders.Authorization = null;
    }
    public async Task<LoginResponse?> Login(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("/auth/login", new { email, password });

        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        if (result is not null && !string.IsNullOrWhiteSpace(result.Token))
            await SetTokenAsync(result.Token, result.Role, email);
        return result;
    }
    public async Task<string?> GetEmailAsync()
    {
        var email = await _localStorage.GetItemAsync<string>("email");
        return string.IsNullOrWhiteSpace(email) ? null : email;
    }
    public async Task<List<ProductoDto>> GetProductosAsync()
    {
        return await _http.GetFromJsonAsync<List<ProductoDto>>("/productos")
               ?? new List<ProductoDto>();
    }
    public async Task<ApiResult> RegistrarMovimientoAsync(int idProducto, string tipo, int cantidad, string? nota)
    {
        var resp = await _http.PostAsJsonAsync("/movimientos", new
        {
            idProducto,
            tipo,
            cantidad,
            nota
        });

        if (resp.IsSuccessStatusCode)
            return ApiResult.Ok();

        var body = await resp.Content.ReadAsStringAsync();
        return ApiResult.Fail(body);
    }
    public async Task<List<ProductoSimpleDto>> GetProductosSimpleAsync()
    {
        var productos = await GetProductosAsync();
        return productos
            .Select(p => new ProductoSimpleDto { IdProducto = p.IdProducto, Nombre = p.Nombre })
            .ToList();
    }
    public async Task<List<OrdenDto>> GetOrdenesAsync()
    {
        return await _http.GetFromJsonAsync<List<OrdenDto>>("/ordenes") ?? new();
    }
    public async Task<OrdenDto?> CrearOrdenAsync(string? proveedor)
    {
        var resp = await _http.PostAsJsonAsync("/ordenes", new { proveedor });
        if (!resp.IsSuccessStatusCode) return null;
        return await resp.Content.ReadFromJsonAsync<OrdenDto>();
    }
    public async Task<OrdenDetalleDto?> GetOrdenDetalleAsync(int idOrden)
    {
        return await _http.GetFromJsonAsync<OrdenDetalleDto>($"/ordenes/{idOrden}");
    }
    public async Task<bool> AgregarLineaAsync(int idOrden, int idProducto, int cantidad, decimal? precioEstimado)
    {
        var resp = await _http.PostAsJsonAsync($"/ordenes/{idOrden}/lineas", new
        {
            idProducto,
            cantidad,
            precioEstimado
        });
        return resp.IsSuccessStatusCode;
    }
    public async Task<bool> CambiarEstadoOrdenAsync(int idOrden, string estado)
    {
        var resp = await _http.PutAsync($"/ordenes/{idOrden}/estado/{estado}", null);
        return resp.IsSuccessStatusCode;
    }
    public async Task<bool> RecibirOrdenAsync(int idOrden)
    {
        var resp = await _http.PutAsync($"/ordenes/{idOrden}/recibir", null);
        return resp.IsSuccessStatusCode;
    }
    public async Task<List<StockBajoDto>> GetStockBajoAsync()
    {
        return await _http.GetFromJsonAsync<List<StockBajoDto>>("/informes/stock-bajo") ?? new();
    }
    public async Task<MovimientosInformeDto?> GetMovimientosInformeAsync(DateTime? desde, DateTime? hasta)
    {
        var url = "/informes/movimientos";
        var qs = new List<string>();

        if (desde.HasValue) qs.Add($"desde={desde:yyyy-MM-dd}");
        if (hasta.HasValue) qs.Add($"hasta={hasta:yyyy-MM-dd}");

        if (qs.Count > 0) url += "?" + string.Join("&", qs);

        return await _http.GetFromJsonAsync<MovimientosInformeDto>(url);
    }
    public async Task<DashboardDto?> GetDashboardAsync()
    {
        return await _http.GetFromJsonAsync<DashboardDto>("/dashboard");
    }
    public async Task<string?> GetRoleAsync()
    {
        var role = await _localStorage.GetItemAsync<string>("role");
        return string.IsNullOrWhiteSpace(role) ? null : role;
    }
    public async Task<List<CategoriaDto>> GetCategoriasAsync()
    {
        return await _http.GetFromJsonAsync<List<CategoriaDto>>("/categorias") ?? new();
    }
    public async Task<bool> CrearCategoriaAsync(string nombre)
    {
        var resp = await _http.PostAsJsonAsync("/categorias", new { idCategoria = 0, nombre, activo = true });
        return resp.IsSuccessStatusCode;
    }
    public async Task<bool> EditarCategoriaAsync(int idCategoria, string nombre, bool activo)
    {
        var resp = await _http.PutAsJsonAsync($"/categorias/{idCategoria}", new { idCategoria, nombre, activo });
        return resp.IsSuccessStatusCode;
    }
    public async Task<bool> DesactivarCategoriaAsync(int idCategoria)
    {
        var resp = await _http.DeleteAsync($"/categorias/{idCategoria}");
        return resp.IsSuccessStatusCode;
    }
    public async Task<bool> CrearProductoAsync(CreateProductoUiRequest req)
    {
        var resp = await _http.PostAsJsonAsync("/productos", req);
        return resp.IsSuccessStatusCode;
    }
    public async Task<bool> EditarProductoAsync(int idProducto, UpdateProductoUiRequest req)
    {
        var resp = await _http.PutAsJsonAsync($"/productos/{idProducto}", req);
        return resp.IsSuccessStatusCode;
    }
    public async Task<bool> DesactivarProductoAsync(int idProducto)
    {
        var resp = await _http.DeleteAsync($"/productos/{idProducto}");
        return resp.IsSuccessStatusCode;
    }
    public async Task<int?> ReponerProductoAsync(int idProducto, int cantidad)
    {
        // 1) Crear orden
        var orden = await CrearOrdenAsync("Auto (Stock bajo)");
        if (orden is null) return null;

        // 2) Añadir línea
        var ok = await AgregarLineaAsync(orden.IdOrden, idProducto, cantidad, null);
        if (!ok) return null;

        return orden.IdOrden;
    }
    public async Task<byte[]?> ExportStockBajoAsync()
    {
        var resp = await _http.GetAsync("/export/stock-bajo");

        if (!resp.IsSuccessStatusCode)
            return null;

        return await resp.Content.ReadAsByteArrayAsync();
    }
    public async Task<DashboardChartsDto?> GetDashboardChartsAsync(int days = 30)
    {
        return await _http.GetFromJsonAsync<DashboardChartsDto>($"/dashboard/charts?days={days}");
    }
    public async Task<List<UsuarioDto>> GetUsuariosAsync()
    {
        var resp = await _http.GetAsync("/usuarios");

        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync();
            throw new Exception($"Error API {(int)resp.StatusCode}: {body}");
        }

        return await resp.Content.ReadFromJsonAsync<List<UsuarioDto>>() ?? new();
    }
    public async Task<ApiResult> CrearUsuarioAsync(CreateUsuarioRequest req)
    {
        var resp = await _http.PostAsJsonAsync("/usuarios", req);
        var body = await resp.Content.ReadAsStringAsync();

        if (resp.IsSuccessStatusCode)
            return ApiResult.Ok();

        return ApiResult.Fail($"HTTP {(int)resp.StatusCode}: {body}");
    }
    public async Task<ApiResult> SetUsuarioActivoAsync(int idUsuario, bool activo)
    {
        var resp = await _http.PutAsJsonAsync($"/usuarios/{idUsuario}/activo", new { activo });
        if (resp.IsSuccessStatusCode) return ApiResult.Ok();

        return ApiResult.Fail(await resp.Content.ReadAsStringAsync());
    }
    public async Task<ApiResult> ResetPasswordAsync(int idUsuario, string newPassword)
    {
        var resp = await _http.PutAsJsonAsync($"/usuarios/{idUsuario}/password", new { newPassword });
        if (resp.IsSuccessStatusCode) return ApiResult.Ok();

        return ApiResult.Fail(await resp.Content.ReadAsStringAsync());
    }
    public async Task<ApiResult> SetUserRoleAsync(int idUsuario, string rol)
    {
        var resp = await _http.PutAsJsonAsync($"/usuarios/{idUsuario}/rol", new { rol });
        if (resp.IsSuccessStatusCode) return ApiResult.Ok();

        return ApiResult.Fail(await resp.Content.ReadAsStringAsync());
    }   
    public record CreateUsuarioRequest(string Nombre, string Email, string Password, string Rol);

    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
        public bool Activo { get; set; }
        public string Rol { get; set; } = "Empleado";
    }
    public class DashboardChartsDto
    {
        public int Days { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public List<MovDiaDto> MovimientosPorDia { get; set; } = new();
        public List<TopProductoDto> TopSalidas { get; set; } = new();
    }
    public class MovDiaDto
    {
        public DateTime Dia { get; set; }
        public long Entradas { get; set; }
        public long Salidas { get; set; }
    }
    public class TopProductoDto
    {
        public string Nombre { get; set; } = "";
        public long Total { get; set; }
    }
    public record ApiResult(bool Success, string? Error)
    {
        public static ApiResult Ok() => new(true, null);
        public static ApiResult Fail(string? error) => new(false, error);
    }
    public class ProductoSimpleDto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = "";
    }
    public class ProductoDto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = "";
        public int IdCategoria { get; set; }
        public string? Codigo { get; set; }
        public int StockMinimo { get; set; }
        public int StockActual { get; set; }
        public bool Activo { get; set; }
    }
    public class OrdenDto
    {
        public int IdOrden { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = "";
        public string? Proveedor { get; set; }
        public int IdUsuario { get; set; }
    }
    public class OrdenDetalleDto
    {
        public OrdenDto Orden { get; set; } = new();
        public List<LineaDetalleDto> Lineas { get; set; } = new();
    }
    public class LineaDetalleDto
    {
        public int IdLinea { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal? PrecioEstimado { get; set; }
    }
    public class StockBajoDto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = "";
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
    }
    public class MovimientosInformeDto
    {
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public int Total { get; set; }
        public List<MovimientoItemDto> Items { get; set; } = new();
    }
    public class MovimientoItemDto
    {
        public int IdMovimiento { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = "";
        public int Cantidad { get; set; }
        public string? Nota { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; } = "";
    }
    public class DashboardDto
    {
        public int TotalProductos { get; set; }
        public int StockBajo { get; set; }
        public int OrdenesBorrador { get; set; }
        public int OrdenesEnviadas { get; set; }
        public int Movimientos7d { get; set; }
    }
    public class CategoriaDto
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = "";
        public bool Activo { get; set; }
    }
    public record CreateProductoUiRequest(string Nombre, int IdCategoria, string? Codigo, int StockMinimo);
    public record UpdateProductoUiRequest(string Nombre, int IdCategoria, string? Codigo, int StockMinimo, bool Activo);
}

public class LoginResponse
{
    public string Token { get; set; } = "";
    public string Role { get; set; } = "";
}