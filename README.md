## TinyMarket - API de Gestión de Productos

<p class="ds-markdown-paragraph">
  <img src="https://img.shields.io/badge/.NET-8.0-blue" alt=".NET"><br>
  <img src="https://img.shields.io/badge/SQL%2520Server-2022+-red" alt="SQL Server"><br>
  <img src="https://img.shields.io/badge/Swagger-UI-green" alt="Swagger">
</p>

TinyMarket es una API RESTful desarrollada en .NET 8 para la gestión de productos, categorías y proveedores de una tienda, implementando el patrón Repository con ADO.NET.

----
### Características principales
- CRUD completo para Productos, Categorías y Proveedores
- Documentación automática con Swagger
- Arquitectura limpia con separación de responsabilidades
- Pruebas unitarias con xUnit
- Interfaz web básica incluida (HTML/CSS/JS)
- Patrón Repository con ADO.NET

----
### Tecnologías utilizadas
- .NET 8
- ASP.NET Core Web API
- ADO.NET
- SQL Server
- Swagger/OpenAPI
- xUnit
- HTML5/CSS3/JavaScript

----
### Estructura de la solución
```text
TinyMarket/
├── TinyMarketCore/                  # Núcleo de la aplicación
│   ├── Entities/                    # Entidades del dominio
│   ├── Interfaces/                  # Interfaces de repositorios
│   └── Services/                    # Lógica de negocio implementada
│
├── TinyMarketDTO/                   # Objetos de Transferencia de Datos
│   ├── RequestsDTO/                 # DTOs para peticiones
│   └── ResponseDTO/                 # DTOs para respuestas
│
├── TinyMarketData/                  # Acceso a datos
│   ├── Extensions/                  # Métodos de extensión
│   └── Repositories/                # Implementaciones de repositorios (ADO.NET)
│
├── TinyMarketWebAPI/                # Capa de presentación
│   ├── wwwroot/                     # Frontend básico (HTML/CSS/JS)
│   └── Controllers/                 # Controladores API
│
└── TinyMarketTests/                 # Pruebas automatizadas
    └── Controllers/                 # Pruebas de controladores API
```

----
### Requisitos previos
- .NET 8 SDK
- SQL Server 2019+ o SQL Server Express
- Visual Studio 2022 (recomendado) o VS Code

----
### Configuración inicial
1 - Clonar el repositorio:
```
git clone https://github.com/tu-usuario/TinyMarket.git
cd TinyMarket
```
2 - Configurar la base de datos:
- Ejecutar el script SQL ubicado en TinyMarketData/Scripts/CreateDatabase.sql
- Ejecutar los scripts de cada Procedimiento.
- Modificar la cadena de conexión en appsettings.json del proyecto TinyMarketWebAPI.
3 - Ejecutar la aplicación.

----
### Uso de las API
La API está documentada con Swagger. Una vez ejecutada la aplicación, accede a:
```
http://localhost:9999/swagger/index.html
```

#### Endpoints principales
- GET `/products` - Obtiene todos los productos habilitados
- POST `/products/filtered` - Obtiene los productos con los filtros seleccionados
- POST `/products` - Crea un nuevo producto
- PUT `/products` - Actualiza un producto existente
- DELETE `/products/{id}` - Elimina un producto

Endpoints similares disponibles para categorías (`/categories`) y proveedores (`/suppliers`).

----
### Interfaz Web
La aplicación incluye una interfaz web básica accesible en:
```
http://localhost:9999/index.html
```
![image](https://github.com/user-attachments/assets/86217b15-ec32-497d-babd-d1bc449824a5)

----
### Ejecución de pruebas
1 - Para ejecutar las pruebas en Visual Studio sólo basta con posicionarnos en el proyecto "TinyMarketTests" y hacer clic derecho, luego selecciona "Ejecutar pruebas".

----
### Estructura de la base de datos

![image](https://github.com/user-attachments/assets/d2f4fe9d-ab3b-4313-8ca7-8aeddf4a611f)









