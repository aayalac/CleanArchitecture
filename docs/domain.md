# Modelo de Dominio

## Entidad: Alquiler

**Archivo:** `Domain/Alquileres/Alquiler.cs`

### Propiedades

| Propiedad | Tipo | Nullable | Descripción |
|-----------|------|----------|-------------|
| VehiculoId | Guid | No | ID del vehículo alquilado |
| UserId | Guid | No | ID del usuario que reserva |
| Duracion | DateRange | Sí | Período de alquiler |
| PrecioPorPeriodo | Moneda | Sí | Costo por período |
| Mantenimiento | Moneda | Sí | Costo de mantenimiento |
| Accesorios | Moneda | Sí | Costo de accesorios |
| PrecioTotal | Moneda | Sí | Costo total |
| Status | AlquilerStatus | No | Estado actual |
| FechaCreacion | DateTime | Sí | Fecha de creación de la reserva |
| FechaConfirmacion | DateTime | Sí | Fecha de confirmación |
| FechaDenegacion | DateTime | Sí | Fecha de rechazo |
| FechaCompletado | DateTime | Sí | Fecha de finalización |
| FechaCancelacion | DateTime | Sí | Fecha de cancelación |

### Estados del Alquiler

```csharp
public enum AlquilerStatus
{
    Reservado,    // Reserva creada
    Confirmado,   // Reserva confirmada por administrador
    Rechazado,    // Reserva rechazada
    Cancelado,    // Reserva cancelada por usuario
    Completado    // Alquiler finalizado
}
```

### Transiciones de Estado

```
[Reservado] ──→ [Confirmado] ──→ [Completado]
     │                │
     └──→ [Rechazado] └──→ [Cancelado]
```

### Métodos de Negocio

- **Reservar**: Crea un nuevo alquiler y calcula precios (factory method estático)
- **Confirmar**: Confirma una reserva pendiente → Estado: Confirmado
- **Rechazar**: Rechaza una reserva pendiente → Estado: Rechazado
- **Cancelar**: Cancela una reserva confirmada (antes de la fecha de inicio) → Estado: Cancelado
- **Completar**: Marca el alquiler como finalizado → Estado: Completado

---

## Entidad: Vehiculo

**Archivo:** `Domain/Vehiculos/Vehiculo.cs`

### Propiedades

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| Modelo | Modelo | Marca y modelo del vehículo |
| Vin | Vin | Número de identificación único |
| Precio | Moneda | Precio base de alquiler |
| Mantenimiento | Moneda | Costo de mantenimiento |
| Accesorios | List\<Accesorio\> | Lista de accesorios disponibles |
| Direccion | Direccion | Ubicación del vehículo |
| FechaUltimaAlquiler | DateTime? | Fecha del último alquiler (se actualiza al reservar) |

---

## Entidad: User

**Archivo:** `Domain/Users/User.cs`

### Propiedades

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| Nombre | Nombre | Nombre del usuario |
| Apellido | Apellido | Apellido del usuario |
| Email | Email | Correo electrónico |

---

## Entidad: Review

**Archivo:** `Domain/Reviews/Review.cs`

### Propiedades

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| Raiting | Raiting | Calificación numérica (1-5) |
| Comentario | Comentario | Texto de comentario del usuario |
| VehiculoId | Guid | ID del vehículo calificado |
| AlquilerId | Guid | ID del alquiler asociado |
| UserId | Guid | ID del usuario que califica |

---

## Value Objects

### Moneda

```csharp
public sealed record Moneda
{
    public TipoMoneda TipoMoneda { get; }
    public decimal Monto { get; }
}
```

### TipoMoneda (enum)

```csharp
public enum TipoMoneda
{
    USD,
    EUR
}
```

### DateRange

```csharp
public sealed record DateRange
{
    public DateOnly Inicio { get; init; }
    public DateOnly Fin { get; init; }
    public int CantidadDias => Fin.DayNumber - Inicio.DayNumber;

    public static DateRange Create(DateOnly inicio, DateOnly fin);
}
```

### Email

```csharp
public sealed record Email(string Value);
```

### Nombre

```csharp
public sealed record Nombre(string Value);
```

### Apellido

```csharp
public sealed record Apellido(string Value);
```

### Vin

```csharp
public sealed record Vin(string Value);
```

### Modelo

```csharp
public sealed record Modelo(string Value);
```

### Direccion

```csharp
public sealed record Direccion
{
    public string Pais { get; }
    public string Departamento { get; }
    public string Ciudad { get; }
}
```

### Raiting

```csharp
public sealed record Raiting
{
    public int Value { get; init; }

    public static Result<Raiting> Create(int value);
    // Valores válidos: 1 a 5
}
```

### Comentario

```csharp
public sealed record Comentario(string Value);
```

### Accesorio

```csharp
public sealed record Accesorio(string Name, decimal Costo);
```

---

## Eventos de Dominio

Los eventos se publican después de cada operación exitosa:

| Evento | Descripción |
|--------|-------------|
| AlquilerReservadoDomainEvent | Se creó una nueva reserva |
| AlquilerConfirmadoDomainEvent | Se confirmó una reserva |
| AlquilerRechazadoDomainEvent | Se rechazó una reserva |
| AlquilerCanceladoDomainEvent | Se canceló una reserva |
| AlquilerCompletadoDomainEvent | Se completó un alquiler |
| ReviewCreateDomainEvent | Se creó una nueva review |
| UserCreateDomainEvent | Se creó un nuevo usuario |

---

## Servicios de Dominio

### PrecioService

Calcula el costo total del alquiler basado en:
- Precio por período del vehículo
- Duración del alquiler (CantidadDias)
- Costos de mantenimiento
- Costo de accesorios seleccionados

Devuelve un `PrecioDetalle` con el desglose de costos.
