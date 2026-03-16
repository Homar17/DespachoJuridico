# Sistema de Despacho Jurídico ⚖️

> Una aplicación web Full-Stack diseñada para optimizar la administración de casos legales y el agendamiento de citas para un despacho jurídico.

## 📖 Descripción

Este proyecto es un sistema integral cliente-servidor desarrollado para facilitar el flujo de trabajo de un despacho de abogados. Permite la gestión segura de usuarios (con roles específicos), el seguimiento detallado de casos jurídicos (fechas, estatus, ramas legales) y la coordinación de citas, todo respaldado por una base de datos relacional robusta.

## ✨ Funcionalidades Principales

- **Gestión de Usuarios y Roles:** Sistema de autenticación y autorización para diferenciar entre administradores, abogados y clientes.
- **Control de Casos Legales:** Registro y seguimiento de casos, incluyendo fechas de inicio/fin, descripciones, estatus actual y rama jurídica a la que pertenecen.
- **Agendamiento de Citas:** Módulo para programar y visualizar reuniones vinculadas a usuarios específicos.
- **Trazabilidad de Estatus:** Catálogos dinámicos para el estado de los casos y las ramas legales.

## 🛠️ Tecnologías Utilizadas

### Backend
- **Framework:** .NET 8 (ASP.NET Core Web API)
- **Lenguaje:** C#
- **ORM:** Entity Framework Core
- **Autenticación:** [Mencionar aquí si usas JWT más adelante]

### Frontend
- **Framework:** Vue.js 3
- **Build Tool:** Vite
- **Peticiones HTTP:** Axios
- **Estilos:** [Mencionar si usas Tailwind CSS, Bootstrap, PrimeVue, etc.]

### Base de Datos
- **Motor:** PostgreSQL
- **Diseño:** Modelo Entidad-Relación

## 🚀 Instalación y Configuración Local

Sigue estos pasos para ejecutar el proyecto en tu entorno local.

### Prerrequisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (versión 18 o superior)
- [PostgreSQL](https://www.postgresql.org/)

### 1. Clonar el repositorio
```bash
git clone [https://github.com/](https://github.com/)[Tu-Usuario-GitHub]/[Nombre-Del-Repo].git
cd [Nombre-Del-Repo]
