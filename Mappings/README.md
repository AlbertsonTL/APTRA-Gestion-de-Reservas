# Mapeo de Datos (Mappings)

## Decisión Técnica: Uso de Mapster

En este proyecto se ha optado por utilizar **Mapster** en lugar de otras alternativas populares como AutoMapper o el mapeo manual (métodos de extensión), por las siguientes razones:

1. **Rendimiento**: Mapster es significativamente más rápido y consume menos memoria que AutoMapper al evitar en gran medida el uso excesivo de reflexión en tiempo de ejecución.
2. **Simplicidad**: Permite mapear objetos sin configuraciones complejas ni perfiles obligatorios (`dto.Adapt<Entidad>()`), logrando un código más limpio en los controladores y repositorios.
3. **Mantenibilidad**: Mapster mapea automáticamente las propiedades que coinciden por convención de nombres. El archivo `MapsterConfiguration.cs` existe para centralizar configuraciones globales (ej. ignorar valores nulos) y para tener un lugar donde añadir reglas específicas o *overrides* cuando los DTOs y las Entidades comiencen a diferir en el futuro.

Al utilizar esta herramienta, mantenemos la capa de Controladores enfocada exclusivamente en recibir solicitudes HTTP y devolver respuestas HTTP, delegando la responsabilidad de transformación de objetos.
