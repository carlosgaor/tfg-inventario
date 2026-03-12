# 📦 Inventario Pro
### Sistema de Gestión de Inventario  
**Proyecto TFG - Desarrollo de Aplicaciones Multiplataforma (DAM)**  

**Alumno:** Carlos García Ortega

---

# Índice

- [Introducción](#introducción)
- [Funcionalidades del proyecto](#funcionalidades-del-proyecto)
- [Tecnologías utilizadas](#tecnologías-utilizadas)
- [Arquitectura del sistema](#arquitectura-del-sistema)
- [Guía de instalación](#guía-de-instalación)
- [Guía de uso](#guía-de-uso)
- [Documentación](#documentación)
- [Diseño de interfaz (Figma)](#diseño-de-interfaz-figma)
- [Conclusión](#conclusión)
- [Contribuciones y agradecimientos](#contribuciones-y-agradecimientos)
- [Licencia](#licencia)
- [Contacto](#contacto)

---

# Introducción

## Descripción del proyecto

Inventario Pro es una aplicación web desarrollada como Proyecto de Fin de Grado Superior en Desarrollo de Aplicaciones Multiplataforma (DAM)

El sistema permite gestionar el inventario de pequeñas empresas, facilitando el control de productos, movimientos de stock, órdenes de compra y usuarios del sistema

La aplicación sigue una arquitectura cliente-servidor, donde el frontend interactúa con un backend mediante una API REST, que a su vez gestiona la persistencia de datos en una base de datos relacional

---

## Justificación

Muchas pequeñas empresas al realizar el control de inventario de forma manual o mediante hojas de cálculo, lo que lleva provocar errores, pérdida de información o dificultades para obtener informes

Este proyecto propone una solución digital que permite:

- centralizar la gestión del inventario
- mejorar el control del stock
- facilitar la toma de decisiones mediante informes y visualización de datos

---

## Objetivos

Los objetivos principales del proyecto son:

- Desarrollar una aplicación web de gestión de inventario
- Implementar autenticación y control de acceso por roles
- Diseñar una base de datos consistente con integridad de datos
- Implementar operaciones CRUD sobre las entidades principales
- Permitir el registro de movimientos de stock
- Gestionar órdenes de compra y recepción de mercancía
- Mostrar información relevante mediante un dashboard analítico

---

## Motivación

Este proyecto ha sido desarrollado con el objetivo de aplicar los conocimientos adquiridos en el curso, como son:

- desarrollo backend
- desarrollo frontend
- diseño de bases de datos
- arquitectura de aplicaciones
- control de versiones

---

# Funcionalidades del proyecto

El sistema permite realizar las siguientes operaciones:

### Autenticación y seguridad
- Inicio de sesión mediante JWT
- Control de acceso por roles (Admin / Empleado)

### Gestión de productos
- Crear productos
- Editar productos
- Desactivar productos
- Control de stock mínimo

### Gestión de categorías
- Crear categorías
- Editar categorías
- Desactivar categorías

### Movimientos de inventario
- Registro de entradas de stock
- Registro de salidas de stock
- Ajustes de inventario

### Gestión de órdenes de compra
- Crear órdenes
- Añadir líneas a una orden
- Enviar órdenes
- Recepción de pedidos

### Gestión de usuarios
- Crear usuarios
- Cambiar roles
- Activar / desactivar usuarios
- Restablecer contraseñas

### Informes
- Productos con stock bajo
- Movimientos por fechas
- Exportación de datos en CSV

### Home
- Indicadores clave del sistema
- Gráficas de movimientos
- Productos con mayor salida

---

# Tecnologías utilizadas

## Backend
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **JWT Authentication**

## Frontend
- **Blazor WebAssembly**
- **Bootstrap**
- **Chart.js**

## Base de datos
- **MySQL**

## Control de versiones
- **Git**
- **GitHub**

---

# Arquitectura del sistema

El sistema sigue una arquitectura de tres capas:

### Frontend
Aplicación web desarrollada con Blazor WebAssembly

### Backend
API REST desarrollada con ASP.NET Core encargada de:

- lógica de negocio
- autenticación
- autorización
- acceso a datos

### Base de datos
Base de datos MySQL que almacena toda la información del sistema

---

# Guía de instalación

## Requisitos

- .NET SDK
- MySQL Server
- Visual Studio 2022
- Git

---

## Instalación del proyecto

1. Clonar el repositorio

git clone https://github.com/usuario/tfg-inventario.git

2. Abrir la solución en Visual Studio.

3. Configurar la conexión a la base de datos en el archivo:

appsettings.json

4. Ejecutar el backend.

5. Ejecutar el frontend.

---

# Guía de uso

## Inicio de sesión

Usuarios de prueba:

### Administrador

Email:
admin@inventario.local

Contraseña:
Admin123!

### Empleado

Email:
empleado@inventario.local

Contraseña:
Empleado123!

---

## Funciones principales

Una vez iniciada la sesión, el usuario puede:

- Consultar el dashboard del sistema
- Gestionar productos
- Registrar movimientos de inventario
- Consultar informes
- Gestionar usuarios (solo administrador)

---

# Documentación

La documentación completa del proyecto se encuentra disponible en la memoria del TFG, donde se incluyen:

- Descripción del sistema
- Diagramas de arquitectura
- Diagramas de casos de uso
- Explicación del modelo de base de datos
- Explicación del funcionamiento de la aplicación

---

# Diseño de interfaz (Figma)

El diseño inicial de la interfaz de usuario puede consultarse en el siguiente enlace:


---

# Conclusión

Este proyecto ha permitido aplicar de forma práctica los conocimientos adquiridos durante el ciclo formativo de Desarrollo de Aplicaciones Multiplataforma.

Se ha desarrollado una aplicación completa que integra frontend, backend y base de datos, aplicando conceptos de arquitectura de software, control de acceso mediante roles, autenticación segura y gestión estructurada de datos.

Además, el uso de control de versiones mediante Git y GitHub permite mantener un historial del desarrollo del proyecto.

---


# Licencia

Este proyecto se distribuye con fines educativos como parte del TGF del ciclo DAM.

---

# Contacto

Email:
carlos.garciaortega@a.vedrunasevillasj.es

