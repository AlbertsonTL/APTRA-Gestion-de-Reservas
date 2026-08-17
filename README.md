# APTRA - Sistema de Gestión de Reservas, Rutas y Tickets

API REST construida en **ASP.NET Core 9** para la administración de rutas de transporte y la emisión/validación de tickets. Este README documenta la arquitectura técnica del repositorio y cómo levantar el proyecto en local.

## Stack técnico

| Componente | Tecnología |
|---|---|
| Framework | ASP.NET Core 9 (Web API + MVC host) |
| Lenguaje | C# / .NET 9 |
| Base de datos | SQL Server |
| ORM | Entity Framework Core 9 |
| Mapeo objeto-objeto | Mapster |
| Documentación de API | Swagger / Swashbuckle |
| Gestión de secretos (dev) | .NET User Secrets |
| CI | GitHub Actions (`.github/workflows/dotnet.yml`) |

## Arquitectura

El proyecto sigue un enfoque de **monolito modular**: un único proyecto ASP.NET Core organizado por capas y, dentro de cada capa, por dominio (`Rutas`, `Tickets`), en lugar de separar en múltiples proyectos/microservicios.

```
APTRA-Gestion-de-Reservas/
├── Modules/
│   ├── Controllers/Api/       # Controladores REST (RutasController, TicketsController)
│   └── Common/
│       ├── DTOs/               # Contratos compartidos (PagedResponse<T>)
│       └── Middlewares/        # GlobalExceptionMiddleware
├── Models/
│   ├── Rutas/                  # Entidad Ruta + DTOs de Ruta
│   └── Tickets/                # Entidad Ticket, Enums, DTOs de Ticket
├── Persistence/
│   ├── AptraDbContext.cs       # DbContext de EF Core
│   └── Repositories/           # Patrón Repositorio (interfaz + implementación por entidad)
├── Mappings/
│   └── MapsterConfiguration.cs # Configuración global de mapeo Entidad <-> DTO
├── Migrations/                 # Migraciones de EF Core
├── Controllers/, Views/        # Host MVC por defecto de la plantilla (Home/Error)
└── Program.cs                  # Composición de servicios y pipeline HTTP
```

**Flujo de una petición:** `Controller` (API) → `Repository` (acceso a datos vía `AptraDbContext`) → EF Core → SQL Server. El mapeo entre entidades y DTOs se hace con Mapster (`dto.Adapt<Entidad>()` / `entidad.Adapt<Dto>()`), configurado centralizadamente en `Mappings/MapsterConfiguration.cs`.

### Decisiones de diseño relevantes

- **Patrón Repositorio**: cada entidad tiene su interfaz (`IRutaRepository`, `ITicketRepository`) inyectada por DI, para desacoplar los controladores de EF Core.
- **Mapster sobre AutoMapper**: elegido por rendimiento y menor configuración (ver `Mappings/README.md`).
- **Respuestas paginadas uniformes**: `PagedResponse<T>` en `Modules/Common/DTOs` envuelve toda respuesta de listado (`Data`, `TotalRecords`, `PageNumber`, `PageSize`, `TotalPages`, etc.).
- **Manejo global de errores**: `GlobalExceptionMiddleware` captura excepciones no controladas y devuelve un JSON uniforme con código 500, evitando try/catch repetidos en cada controlador.
- **Estado de Ticket tipado**: `EstadoTicket` es un enum (`Active`, `Used`) que EF Core convierte a `string` en la base de datos para legibilidad al inspeccionar la tabla directamente.
- **Validaciones en dos niveles**: Data Annotations en las entidades/DTOs (`[Required]`, `[Range]`) a nivel de aplicación, y `CHECK constraints` / índices únicos vía Fluent API (`AptraDbContext.OnModelCreating`) a nivel de base de datos (ej. `Precio > 0` en Rutas, `CodigoValidacion` único en Tickets).
- **Secretos fuera del control de versiones**: la cadena de conexión a SQL Server no está en `appsettings.json`; se maneja con `dotnet user-secrets` en desarrollo.

## Datos de prueba (seed data)

Para facilitar el testing manual y de integración, el proyecto incluye un `DataSeeder` (`Persistence/DataSeeder.cs`) que carga datos de ejemplo automáticamente al iniciar la aplicación **en entorno Development**:

- **6 rutas** con orígenes/destinos variados (5 activas, 1 inactiva) para poder probar filtros de estado.
- **8 tickets** distribuidos entre varias rutas, con una mezcla de estados `Active` y `Used`, y fechas de viaje/emisión tanto pasadas como futuras — útil para probar la validación (`PUT /api/tickets/{id}/validar`, que debe rechazar los que ya están en `Used`) y la eliminación (`DELETE /api/tickets/{id}`).

El seeder es **idempotente**: verifica si ya existen registros en `Rutas` o `Tickets` antes de insertar, por lo que es seguro reiniciar la aplicación varias veces sin duplicar datos.

Se ejecuta automáticamente al correr `dotnet run` en Development, después de aplicar las migraciones (paso 3 más abajo). No requiere ningún comando adicional.

> Si en algún momento quieres partir de una base vacía, simplemente elimina la base de datos (`dotnet ef database drop`) y vuelve a aplicar las migraciones (`dotnet ef database update`); el seeder volverá a poblarla en el siguiente arranque.

## Requisitos previos

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (local, Docker o instancia remota accesible)
- (Opcional) [EF Core CLI tools](https://learn.microsoft.com/ef/core/cli/dotnet): `dotnet tool install --global dotnet-ef`

## Cómo ejecutar el proyecto en local

### 1. Clonar y restaurar dependencias

```bash
git clone https://github.com/AlbertsonTL/APTRA-Gestion-de-Reservas/
cd APTRA-Gestion-de-Reservas
dotnet restore
```

### 2. Configurar la cadena de conexión (User Secrets)

El proyecto **no** trae una cadena de conexión en `appsettings.json` por diseño. Hay que inicializarla localmente:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=AptraDb;Trusted_Connection=True;TrustServerCertificate=True;"
// DB usando sqllocaldb
dotnet user-secrets set "ConnectionStrings:DefaultConnection" 'Server=(localdb)\MSSQLLocalDB;Database=APTRA_GestionReservas;Trusted_Connection=True;TrustServerCertificate=True'
```

Ajusta el connection string según tu instancia de SQL Server (usuario/contraseña, puerto, etc.). El `UserSecretsId` ya está declarado en el `.csproj`, por lo que basta con el comando anterior.

> Alternativa rápida con Docker para SQL Server:
> ```bash
> docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=TuPasswordFuerte123!" \
>   -p 1433:1433 --name aptra-sql -d mcr.microsoft.com/mssql/server:2022-latest
> ```

### 3. Aplicar las migraciones

```bash
dotnet ef database update
```

Esto crea la base de datos `AptraDb` (o el nombre configurado) con las tablas `Rutas` y `Tickets`, incluyendo los `CHECK constraints` e índices definidos en `AptraDbContext`.

### 4. Levantar la aplicación

```bash
dotnet run
```

Por defecto (`Properties/launchSettings.json`):
- HTTP: `http://localhost:5075`
- HTTPS: `https://localhost:7227`

En entorno `Development`, la app abre automáticamente en **Swagger UI** (`/swagger`), donde se pueden probar todos los endpoints sin necesidad de un cliente REST externo.

## Endpoints disponibles

### Rutas — `api/rutas`
| Método | Ruta | Descripción |
|---|---|---|
| `POST` | `/api/rutas` | Crea una ruta |
| `GET` | `/api/rutas?pageNumber=&pageSize=` | Lista rutas paginadas |
| `PUT` | `/api/rutas/{id}` | Edita una ruta existente |
| `DELETE` | `/api/rutas/{id}` | Elimina una ruta |

### Tickets — `api/tickets`
| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `/api/tickets?pageNumber=&pageSize=` | Lista tickets paginados (con la ruta asociada incluida) |
| `PUT` | `/api/tickets/{id}/validar` | Cambia el estado del ticket de `Active` a `Used` |
| `DELETE` | `/api/tickets/{id}` | Elimina un ticket |

Todos los endpoints devuelven códigos HTTP estándar (`200`, `201`, `204`, `400`, `404`, `409`) y mensajes de error descriptivos. El detalle completo de request/response está documentado vía comentarios XML y visible directamente en Swagger.

## Migraciones de base de datos

Para crear una nueva migración después de modificar una entidad:

```bash
dotnet ef migrations add NombreDescriptivoDelCambio
dotnet ef database update
```

Las migraciones existentes viven en `Migrations/` y reflejan la evolución del esquema: `InitialCreate_Ruta` → `AgregarEntidadTicket`.

## Integración continua

El workflow `.github/workflows/dotnet.yml` corre en cada push/PR a `main`: restaura dependencias, compila (`dotnet build`) y ejecuta pruebas (`dotnet test`). No requiere configuración adicional de secretos para el build en sí, ya que no ejecuta migraciones contra una base real.