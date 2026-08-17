using APTRA_Gestion_de_Reservas.Models.Rutas;
using APTRA_Gestion_de_Reservas.Models.Tickets;
using APTRA_Gestion_de_Reservas.Models.Tickets.Enums;
using Microsoft.EntityFrameworkCore;

namespace APTRA_Gestion_de_Reservas.Persistence
{
    /// <summary>
    /// Carga datos de prueba (Rutas y Tickets) en la base de datos para facilitar
    /// el testing manual y de integración durante el desarrollo.
    /// Solo debe invocarse en entornos de Development (ver Program.cs).
    /// </summary>
    public static class DataSeeder
    {
        public static async Task SeedAsync(AptraDbContext context)
        {
            // Idempotente: si ya hay datos, no se vuelve a sembrar.
            if (await context.Rutas.AnyAsync() || await context.Tickets.AnyAsync())
            {
                return;
            }

            var rutas = new List<Ruta>
            {
                new Ruta { Nombre = "Santo Domingo - Santiago", Origen = "Santo Domingo", Destino = "Santiago", Precio = 350.00m, Estado = true },
                new Ruta { Nombre = "Santo Domingo - Puerto Plata", Origen = "Santo Domingo", Destino = "Puerto Plata", Precio = 450.00m, Estado = true },
                new Ruta { Nombre = "Santo Domingo - Punta Cana", Origen = "Santo Domingo", Destino = "Punta Cana", Precio = 500.00m, Estado = true },
                new Ruta { Nombre = "Santiago - La Vega", Origen = "Santiago", Destino = "La Vega", Precio = 150.00m, Estado = true },
                new Ruta { Nombre = "Santo Domingo - Barahona", Origen = "Santo Domingo", Destino = "Barahona", Precio = 400.00m, Estado = false },
                new Ruta { Nombre = "Santo Domingo - San Francisco de Macorís", Origen = "Santo Domingo", Destino = "San Francisco de Macorís", Precio = 300.00m, Estado = true },
            };

            await context.Rutas.AddRangeAsync(rutas);
            await context.SaveChangesAsync();

            var hoy = DateTime.UtcNow.Date;

            var tickets = new List<Ticket>
            {
                new Ticket
                {
                    CodigoValidacion = "APTRA-0001",
                    Pasajero = "Juan Pérez",
                    Documento = "001-1234567-8",
                    RutaId = rutas[0].Id,
                    Trayecto = "Santo Domingo - Santiago",
                    Precio = rutas[0].Precio,
                    FechaViaje = hoy.AddDays(1),
                    FechaEmision = hoy,
                    Estado = EstadoTicket.Active
                },
                new Ticket
                {
                    CodigoValidacion = "APTRA-0002",
                    Pasajero = "María Rodríguez",
                    Documento = "002-2345678-9",
                    RutaId = rutas[1].Id,
                    Trayecto = "Santo Domingo - Puerto Plata",
                    Precio = rutas[1].Precio,
                    FechaViaje = hoy.AddDays(2),
                    FechaEmision = hoy,
                    Estado = EstadoTicket.Active
                },
                new Ticket
                {
                    CodigoValidacion = "APTRA-0003",
                    Pasajero = "Carlos Santana",
                    Documento = "003-3456789-0",
                    RutaId = rutas[2].Id,
                    Trayecto = "Santo Domingo - Punta Cana",
                    Precio = rutas[2].Precio,
                    FechaViaje = hoy.AddDays(-1),
                    FechaEmision = hoy.AddDays(-2),
                    Estado = EstadoTicket.Used
                },
                new Ticket
                {
                    CodigoValidacion = "APTRA-0004",
                    Pasajero = "Ana Gómez",
                    Documento = "004-4567890-1",
                    RutaId = rutas[3].Id,
                    Trayecto = "Santiago - La Vega",
                    Precio = rutas[3].Precio,
                    FechaViaje = hoy.AddDays(3),
                    FechaEmision = hoy,
                    Estado = EstadoTicket.Active
                },
                new Ticket
                {
                    CodigoValidacion = "APTRA-0005",
                    Pasajero = "Luis Fernández",
                    Documento = "005-5678901-2",
                    RutaId = rutas[0].Id,
                    Trayecto = "Santo Domingo - Santiago",
                    Precio = rutas[0].Precio,
                    FechaViaje = hoy.AddDays(-3),
                    FechaEmision = hoy.AddDays(-4),
                    Estado = EstadoTicket.Used
                },
                new Ticket
                {
                    CodigoValidacion = "APTRA-0006",
                    Pasajero = "Georgina Rivas",
                    Documento = "006-6789012-3",
                    RutaId = rutas[5].Id,
                    Trayecto = "Santo Domingo - San Francisco de Macorís",
                    Precio = rutas[5].Precio,
                    FechaViaje = hoy.AddDays(5),
                    FechaEmision = hoy,
                    Estado = EstadoTicket.Active
                },
                new Ticket
                {
                    CodigoValidacion = "APTRA-0007",
                    Pasajero = "Anamilet Soto",
                    Documento = "007-7890123-4",
                    RutaId = rutas[1].Id,
                    Trayecto = "Santo Domingo - Puerto Plata",
                    Precio = rutas[1].Precio,
                    FechaViaje = hoy.AddDays(-5),
                    FechaEmision = hoy.AddDays(-6),
                    Estado = EstadoTicket.Used
                },
                new Ticket
                {
                    CodigoValidacion = "APTRA-0008",
                    Pasajero = "Adonis Buret",
                    Documento = "008-8901234-5",
                    RutaId = rutas[2].Id,
                    Trayecto = "Santo Domingo - Punta Cana",
                    Precio = rutas[2].Precio,
                    FechaViaje = hoy.AddDays(7),
                    FechaEmision = hoy,
                    Estado = EstadoTicket.Active
                },
            };

            await context.Tickets.AddRangeAsync(tickets);
            await context.SaveChangesAsync();
        }
    }
}
