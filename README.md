# Programacion-Web-1
Repositorio de referencia para el Proyecto Integrador

Este proyecto es una Web API creada en .NET Core utilizando una arquitectura N Capas, que incluye una capa de Datos para el mapeo de entidades y una capa de Servicios que gestiona la lógica de negocio. Se hace uso de Bibliotecas y referencias entre las mismas.

## Estructura del Proyecto

El proyecto sigue una arquitectura limpia que incluye las siguientes capas:

- **Capa de Presentación**: Web API que expone los endpoints.
- **Capa de Aplicación**: Capa de servicio que maneja la lógica de negocio.
- **Capa de Datos**: Capa de acceso a datos, responsable del mapeo de entidades y la interacción con la base de datos.

### Directorios principales

- **WebApi.Api**: Contiene los controladores y configuración de la Web API.
- **WebApi.Interface**: Contiene las interfaces que definen la lógica de negocio.
- **WebApi.Implementation**: Implementación de las interfaces de servicios de la capa de aplicación.
- **WebApi.Data**: Implementación del acceso a datos y mapeo de entidades.

## Prerrequisitos

Asegúrate de tener instalados los siguientes componentes antes de ejecutar el proyecto:

- [.NET SDK](https://dotnet.microsoft.com/download) (versión 6.0 o superior)

## Instalación

1. **Clonar el repositorio**:
   ```bash
   git clone https://github.com/SamuelUnan/Programacion-Web-1.git
   cd Programacion-Web-1







