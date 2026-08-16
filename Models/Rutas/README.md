# Modelo Ruta

## Descripción
La entidad `Ruta` representa las rutas de transporte disponibles en el sistema.

## Restricciones y Reglas de Negocio
- **Campos Obligatorios**: Todos los campos (Nombre, Origen, Destino, Precio, Estado) son requeridos a nivel de aplicación (usando Data Annotations) y a nivel de base de datos (columnas `NOT NULL`).
- **Precio Numérico Positivo**: El `Precio` está limitado a valores mayores a cero (`> 0`). Esto se valida tanto en la aplicación con `[Range]` como en la base de datos a través de un `CHECK constraint` configurado mediante *Fluent API* en el DbContext.
- **Estado**: El campo `Estado` es un booleano, donde `true` indica una ruta Activa y `false` una Inactiva.

## Arquitectura (Monolito Modular)
- **Modelos**: Ubicados en `Models/Rutas/` para mejor cohesión.
- **Persistencia**: Centralizada en la carpeta `Persistence/`, utilizando `AptraDbContext`.
- **Acceso a Datos**: Implementado a través del patrón Repositorio (`IRutaRepository` y `RutaRepository`), permitiendo abstracción e inyección de dependencias. Se encuentra en `Persistence/Repositories/`.

## Seguridad y Configuración
- **Cadenas de Conexión**: Por decisión de diseño y seguridad, las claves y cadenas de conexión a la base de datos (SQL Server) no se almacenan en el archivo `appsettings.json`. En su lugar, se utilizan **User Secrets** (`dotnet user-secrets`) durante el desarrollo para evitar la exposición accidental de credenciales en el control de versiones.

## Esquema
| Campo | Tipo de Dato (C#) | Tipo de Dato (SQL) | Restricción |
| --- | --- | --- | --- |
| Id | int | int | PRIMARY KEY, IDENTITY |
| Nombre | string | nvarchar(100) | NOT NULL |
| Origen | string | nvarchar(100) | NOT NULL |
| Destino | string | nvarchar(100) | NOT NULL |
| Precio | decimal | decimal(18,2) | NOT NULL, CHECK (Precio > 0) |
| Estado | bool | bit | NOT NULL |
