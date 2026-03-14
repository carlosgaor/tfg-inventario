# 📦 Sistema de Gestión de Inventario

## TFG
### Desarrollo de Aplicaciones Multiplataforma (DAM)

---

## Alumno

Carlos García Ortega

---

## Título del proyecto

Sistema de Gestión de Inventario para Pequeñas Empresas

---

# Índice

1. [Introducción](#introducción)
2. [Diagrama de casos de uso](#diagrama-de-casos-de-uso)
3. [Diagrama de clases](#diagrama-de-clases)
4. [Diagrama Entidad-Relación](#diagrama-entidad-relación)
5. [Diagrama de componentes](#diagrama-de-componentes)
6. [Casos de prueba](#casos-de-prueba)

---

# Introducción

En este documento describiremos la arquitectura y diseño del sistema

El sistema permite gestionar el inventario de una empresa mediante una aplicación web que incluye funciones de control de stock, gestión de productos, órdenes de compra, usuarios e informes

La aplicación está basada en una arquitectura cliente-servidor donde un frontend desarrollado con Blazor WebAssembly se comunica con un backend implementado en ASP.NET Core Web API, el cual gestiona la lógica de negocio y el acceso a una base de datos MySQL

---

# Diagrama de casos de uso

Aquí representaremos las principales interacciones entre los actores del sistema y las funcionalidades disponibles

Actores principales:

- Administrador
- Empleado

Casos de uso principales:

- Iniciar sesión
- Consultar dashboard
- Gestionar productos
- Gestionar categorías
- Registrar movimientos de stock
- Gestionar órdenes de compra
- Consultar informes
- Gestionar usuarios

*(Aquí puedes insertar el diagrama generado previamente)*

---

# Diagrama de clases

El diagrama de clases representa las entidades principales del sistema y sus relaciones.

Clases principales del sistema:

- Usuario
- Rol
- UsuarioRol
- Producto
- Categoria
- MovimientoStock
- OrdenCompra
- OrdenCompraLinea

Este diagrama permite visualizar la estructura del modelo de datos utilizado en el backend.

*(Aquí puedes insertar el diagrama de clases que ya hicimos anteriormente)*

---

# Diagrama Entidad-Relación

El modelo entidad-relación describe la estructura de la base de datos utilizada en el sistema.

Entidades principales:

- usuarios
- roles
- usuario_roles
- productos
- categorias
- movimientos_stock
- ordenes_compra
- ordenes_compra_lineas

Relaciones principales:

- Un usuario puede tener uno o varios roles
- Un producto pertenece a una categoría
- Un movimiento de stock está asociado a un producto
- Una orden de compra puede contener varias líneas de pedido

*(Aquí puedes insertar el diagrama ER de la base de datos)*

---

# Diagrama de componentes

El sistema sigue una arquitectura de tres capas:

Frontend  
Backend  
Base de datos

Componentes principales:

Frontend:
- Blazor WebAssembly
- Interfaz de usuario
- Comunicación con API

Backend:
- ASP.NET Core Web API
- Lógica de negocio
- Autenticación JWT
- Control de acceso por roles

Base de datos:
- MySQL
- Tablas relacionales
- Procedimientos almacenados
- Triggers de integridad

*(Aquí puedes insertar el diagrama de arquitectura que ya hicimos)*

---

# Casos de prueba

| ID | Funcionalidad | Descripción | Entrada | Resultado esperado | Resultado obtenido | Estado |
|----|---------------|-------------|---------|--------------------|--------------------|--------|
| CP-01 | Login | Inicio de sesión con usuario administrador | admin@inventario.local / Admin123! | Acceso correcto al dashboard | Acceso correcto | Aceptado |
| CP-02 | Login | Inicio de sesión con credenciales incorrectas | admin@inventario.local / contraseña incorrecta | Mostrar error de credenciales | Error mostrado correctamente | Aceptado |
| CP-03 | Login | Inicio de sesión con usuario empleado | empleado@inventario.local / Empleado123! | Acceso correcto con permisos limitados | Acceso correcto | Aceptado |
| CP-04 | Productos | Crear un nuevo producto | Nombre, categoría, código y stock mínimo válidos | Producto creado en base de datos | Producto creado correctamente | Aceptado |
| CP-05 | Productos | Editar producto existente | Cambiar nombre o stock mínimo | Producto actualizado | Producto actualizado correctamente | Aceptado |
| CP-06 | Productos | Desactivar producto | Seleccionar producto y pulsar desactivar | Producto marcado como inactivo | Producto desactivado correctamente | Aceptado |
| CP-07 | Productos | Filtrar productos por nombre | Texto parcial del nombre | Mostrar solo productos coincidentes | Filtrado correcto | Aceptado |
| CP-08 | Categorías | Crear una categoría | Nombre válido | Categoría creada correctamente | Categoría creada correctamente | Aceptado |
| CP-09 | Categorías | Editar categoría | Modificar nombre | Categoría actualizada | Categoría actualizada correctamente | Aceptado |
| CP-10 | Categorías | Desactivar categoría | Seleccionar categoría | Categoría marcada como inactiva | Categoría desactivada correctamente | Aceptado |
| CP-11 | Movimientos | Registrar entrada de stock | Producto válido, cantidad positiva | Se incrementa el stock | Movimiento registrado y stock actualizado | Aceptado |
| CP-12 | Movimientos | Registrar salida de stock válida | Producto con stock suficiente | Se reduce el stock | Movimiento registrado correctamente | Aceptado |
| CP-13 | Movimientos | Registrar salida con stock insuficiente | Cantidad superior al stock disponible | Mostrar error y no actualizar stock | Error mostrado correctamente | Aceptado |
| CP-14 | Movimientos | Registrar ajuste de stock | Producto válido y nueva cantidad | Stock ajustado correctamente | Ajuste realizado correctamente | Aceptado |
| CP-15 | Órdenes | Crear orden de compra | Proveedor válido | Orden creada en estado BORRADOR | Orden creada correctamente | Aceptado |
| CP-16 | Órdenes | Añadir línea a la orden | Producto y cantidad válidos | Línea añadida a la orden | Línea añadida correctamente | Aceptado |
| CP-17 | Órdenes | Cambiar estado de orden | BORRADOR → ENVIADA | Estado actualizado | Estado actualizado correctamente | Aceptado |
| CP-18 | Órdenes | Recibir orden | Orden en estado ENVIADA | Se actualiza stock y estado pasa a RECIBIDA | Recepción realizada correctamente | Aceptado |
| CP-19 | Informes | Consultar productos con stock bajo | Acceso al informe | Mostrar productos con stock inferior al mínimo | Informe generado correctamente | Aceptado |
| CP-20 | Informes | Consultar movimientos por fechas | Selección de rango de fechas | Mostrar movimientos del periodo | Consulta correcta | Aceptado |
| CP-21 | Informes | Exportar CSV de stock bajo | Pulsar botón exportar | Descarga de archivo CSV | Exportación correcta | Aceptado |
| CP-22 | Usuarios | Crear nuevo usuario | Nombre, email, contraseña y rol válidos | Usuario creado correctamente | Usuario creado correctamente | Aceptado |
| CP-23 | Usuarios | Cambiar rol de usuario | Usuario existente y nuevo rol | Rol actualizado correctamente | Rol actualizado correctamente | Aceptado |
| CP-24 | Usuarios | Activar o desactivar usuario | Usuario seleccionado | Cambio de estado correcto | Estado actualizado correctamente | Aceptado |
| CP-25 | Usuarios | Resetear contraseña | Nueva contraseña válida | Contraseña actualizada | Contraseña actualizada correctamente | Aceptado |
| CP-26 | Dashboard | Cargar KPIs | Acceso al dashboard | Mostrar métricas del sistema | KPIs mostrados correctamente | Aceptado |
| CP-27 | Dashboard | Mostrar gráficas analíticas | Selección de rango temporal | Mostrar gráficas actualizadas | Gráficas cargadas correctamente | Aceptado |
