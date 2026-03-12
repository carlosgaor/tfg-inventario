# 📦 Sistema de Gestión de Inventario

## Proyecto de Fin de Grado  
### Desarrollo de Aplicaciones Multiplataforma (DAM)

---

## Alumno

Carlos [Apellidos]

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

El presente documento describe la arquitectura y diseño del sistema desarrollado como Trabajo de Fin de Grado del ciclo formativo de Desarrollo de Aplicaciones Multiplataforma.

El sistema permite gestionar el inventario de una empresa mediante una aplicación web que incluye funcionalidades de control de stock, gestión de productos, órdenes de compra, usuarios e informes.

La aplicación está basada en una arquitectura cliente-servidor donde un frontend desarrollado con Blazor WebAssembly se comunica con un backend implementado en ASP.NET Core Web API, el cual gestiona la lógica de negocio y el acceso a una base de datos MySQL.

---

# Diagrama de casos de uso

El siguiente diagrama representa las principales interacciones entre los actores del sistema y las funcionalidades disponibles.

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

A continuación se muestran algunos casos de prueba realizados para verificar el correcto funcionamiento del sistema.

---

## Caso de prueba 1

Funcionalidad: Inicio de sesión

Entrada:
Email y contraseña válidos

Resultado esperado:
El sistema autentica al usuario y permite acceder al dashboard.

Resultado obtenido:
Correcto.

---

## Caso de prueba 2

Funcionalidad: Crear producto

Entrada:
Datos válidos de producto

Resultado esperado:
El producto se guarda correctamente en la base de datos.

Resultado obtenido:
Correcto.

---

## Caso de prueba 3

Funcionalidad: Registrar movimiento de stock

Entrada:
Producto existente y cantidad válida

Resultado esperado:
Se registra el movimiento y se actualiza el stock.

Resultado obtenido:
Correcto.

---

## Caso de prueba 4

Funcionalidad: Crear usuario

Entrada:
Datos válidos de usuario

Resultado esperado:
El usuario se crea correctamente y se asigna el rol correspondiente.

Resultado obtenido:
Correcto.

---

## Caso de prueba 5

Funcionalidad: Generar informe de stock bajo

Entrada:
Consulta de productos con stock menor al mínimo

Resultado esperado:
Se muestran los productos que necesitan reposición.

Resultado obtenido:
Correcto.