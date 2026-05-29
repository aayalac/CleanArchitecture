# Flujos Principales

## 1. Reservar un Vehículo

### Flujo

```
1. Cliente envía ReservarAlquilerCommand (POST /api/alquileres)
   - vehiculoId
   - userId
   - fechaInicio
   - fechaFin

2. Validación (ReservarAlquilerCommandValidator)
   - Fechas válidas (inicio <= fin)
   - Vehiculo existe
   - Usuario existe

3. Ejecución (ReservarAlquilerCommandHandler)
   - Consulta vehículo y usuario
   - Verifica disponibilidad (IsOverLappingAsync)
   - Calcula precio con PrecioService
   - Crea entidad Alquiler (factory method Reservar)
   - Guarda en repositorio

4. Evento de Dominio
   - AlquilerReservadoDomainEvent
   - ReserverAlquilerDomainEventHandler envía email de confirmación
   - Actualiza FechaUltimaAlquiler del vehículo
```

### Código Relevante

```csharp
// Command
public record ReservarAlquilerCommand(
    Guid VehiculoId,
    Guid UserId,
    DateOnly FechaInicio,
    DateOnly FechaFin
) : IRequest<Result<Guid>>;

// Handler
public async Task<Result<Guid>> Handle(
    ReservarAlquilerCommand request,
    CancellationToken cancellationToken)
{
    // 1. Obtener vehiculo
    // 2. Obtener usuario
    // 3. Verificar solapamiento
    // 4. Calcular precio
    // 5. Crear alquiler (Alquiler.Reservar)
    // 6. Guardar
    // 7. Retornar ID
}
```

---

## 2. Confirmar un Alquiler

### Flujo

```
1. Administrador envía comando de confirmación
2. Se verifica que el estado sea "Reservado"
3. Se cambia estado a "Confirmado"
4. Se registra FechaConfirmacion
5. Se publica AlquilerConfirmadoDomainEvent
```

---

## 3. Cancelar un Alquiler

### Reglas de Negocio

- Solo se puede cancelar si el estado es "Confirmado"
- No se puede cancelar si ya pasó la fecha de inicio
- Se registra FechaCancelacion

### Flujo

```
1. Usuario solicita cancelación
2. Se valida estado y fecha
3. Se cambia estado a "Cancelado"
4. Se publica AlquilerCanceladoDomainEvent
```

---

## 4. Completar un Alquiler

### Flujo

```
1. Sistema marca alquiler como completado
2. Se verifica estado "Confirmado"
3. Se cambia estado a "Completado"
4. Se registra FechaCompletado
5. Se publica AlquilerCompletadoDomainEvent
```

---

## 5. Consultar un Alquiler

### Flujo

```
1. Cliente envía GetAlquilerQuery (GET /api/alquileres/{id})
2. Handler consulta repositorio
3. Se mapea a AlquilerResponse
4. Se retorna la información
```

### Response

```csharp
public record AlquilerResponse(
    Guid Id,
    Guid VehiculoId,
    Guid UserId,
    DateOnly FechaInicio,
    DateOnly FechaFin,
    string Status,
    decimal PrecioTotal
);
```

---

## 6. Buscar Vehículos Disponibles

### Flujo

```
1. Cliente envía SearchVehiculosQuery (GET /api/vehiculos?fechaInicio=...&fechaFin=...)
2. Validación: fechaInicio <= fechaFin
3. Handler ejecuta consulta SQL con Dapper
   - SELECT vehículos
   - WHERE NO EXISTE alquiler activo que se solape con el rango de fechas
   - Estados activos: Reservado, Confirmado, Completado
4. Se mapea resultado a VehiculoResponse con DireccionResponse
5. Se retorna lista de vehículos disponibles
```

### Query

```csharp
public record SearchVehiculosQuery(
    DateOnly fechaInicio,
    DateOnly fechaFin
) : IQuery<IReadOnlyList<VehiculoResponse>>;
```

### Responses

```csharp
public sealed class VehiculoResponse
{
    public Guid Id { get; init; }
    public string? Modelo { get; init; }
    public string? Vin { get; init; }
    public decimal Precio { get; init; }
    public string? TipoMoneda { get; set; }
    public DireccionResponse? Direccion { get; set; }
}

public sealed class DireccionResponse
{
    public string? Pais { get; init; }
    public string? Departamento { get; init; }
    public string? Provincia { get; init; }
    public string? Calle { get; init; }
}
```

### Lógica de Disponibilidad

Un vehículo está disponible si NO tiene ningún alquiler con estado Reservado, Confirmado o Completado que se solape con el rango de fechas consultado:

```
NOT EXISTS (
    SELECT 1 FROM alquileres
    WHERE vehiculo_id = vehiculo.id
      AND duracion_inicio <= fechaFin
      AND duracion_fin >= fechaInicio
      AND status IN (Reservado, Confirmado, Completado)
)
```
