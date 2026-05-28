# Flujos Principales

## 1. Reservar un Vehículo

### Flujo

```
1. Cliente envía ReservarAlquilerCommand
   - vehiculoId
   - userId
   - fechaInicio
   - fechaFin

2. Validación (ReservarAlquilerCommandValidator)
   - Fechas válidas
   - Vehiculo existe
   - Usuario existe

3. Ejecución (ReservarAlquilerCommandHandler)
   - Consulta vehículo y usuario
   - Verifica disponibilidad
   - Calcula precio con PrecioService
   - Crea entidad Alquiler
   - Guarda en repositorio

4. Evento de Dominio
   - AlquilerReservadoDomainEvent
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
    // 3. Calcular precio
    // 4. Crear alquiler
    // 5. Guardar
    // 6. Retornar ID
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
1. Cliente envía GetAlquilerQuery con ID
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
