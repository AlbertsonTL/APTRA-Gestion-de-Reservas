# Modelo Ticket

## Descripción
La entidad `Ticket` representa los boletos o pasajes emitidos en el sistema.

## Relación con Ruta
Cada Ticket está estrictamente asociado a una `Ruta` de transporte. Esto se maneja mediante una clave foránea `RutaId`. 
En el acceso a datos (`TicketRepository`), las consultas incluyen automáticamente el objeto `Ruta` completo utilizando `.Include(t => t.Ruta)`. Esto permite que, en una sola petición al backend (por ejemplo en un `GET`), el frontend pueda recibir tanto los datos del ticket como los de la ruta asociada (como origen y destino) sin tener que hacer cruces adicionales o múltiples peticiones REST.

## Restricciones y Reglas de Negocio
- **Código de Validación**: Es un campo `string` configurado con un índice único a nivel de base de datos (`IsUnique`) para prevenir la duplicación de tickets.
- **Estado**: Se utiliza el `enum` fuertemente tipado `EstadoTicket` (`Active`, `Used`). Para facilitar su legibilidad en reportes o vistas de base de datos directa, este campo se guarda convertido a cadena de texto (`string`) en SQL Server, pero se maneja de forma tipada en C#.
- **Campos Obligatorios**: Todos los campos principales (Pasajero, Documento, Precio, Fechas) son obligatorios. El precio se restringe para que sea estrictamente mayor a 0 mediante Data Annotations `[Range]`.

## Esquema
| Campo | Tipo (C#) | Tipo (SQL) | Notas / Restricción |
| --- | --- | --- | --- |
| Id | int | int | PRIMARY KEY, IDENTITY |
| CodigoValidacion | string | nvarchar(50) | NOT NULL, UNIQUE INDEX |
| Pasajero | string | nvarchar(100) | NOT NULL |
| Documento | string | nvarchar(50) | NOT NULL |
| RutaId | int | int | NOT NULL, FOREIGN KEY a `Rutas` |
| Trayecto | string | nvarchar(200) | NULLABLE |
| Precio | decimal | decimal(18,2) | NOT NULL, > 0 |
| FechaViaje | DateTime | datetime2 | NOT NULL |
| FechaEmision | DateTime | datetime2 | NOT NULL |
| Estado | EstadoTicket | nvarchar(max) | NOT NULL, ('Active', 'Used') |
