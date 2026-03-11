# TFG - Sistema de Gestión de Inventario

Proyecto DAM

## Descripción
Aplicación web de gestión de inventario para pequeñas empresas, desarrollada con arquitectura cliente-servidor.

## Tecnologías utilizadas
- Frontend: Blazor WebAssembly
- Backend: ASP.NET Core Web API
- Base de datos: MySQL
- Autenticación: JWT
- Control de versiones: Git

## Funcionalidades principales
- Inicio de sesión con roles (Admin / Empleado)
- Gestión de productos
- Gestión de categorías
- Registro de movimientos de stock
- Gestión de órdenes de compra
- Recepción de órdenes con actualización automática de stock
- Dashboard con KPIs y gráficas
- Gestión de usuarios
- Informes y exportación CSV

## Estructura del proyecto
- `backend/` → API REST ASP.NET Core
- `frontend/` → Aplicación web Blazor
- `database/` → Scripts SQL
- `docs/` → Capturas y documentación auxiliar

## Ejecución del proyecto
### Backend
1. Abrir la solución en Visual Studio
2. Configurar `appsettings.json`
3. Ejecutar `Inventario.Api`

### Frontend
1. Abrir el proyecto `Inventario.Web`
2. Ejecutarlo con el backend encendido

## Usuarios de prueba
### Admin
- Email: `admin@inventario.local`
- Password: `Admin123!`

### Empleado
- Email: `empleado@inventario.local`
- Password: `Empleado123!`