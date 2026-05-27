# Modelo de Dominio

## Entidad: Alquiler

**Archivo:** `Domain/Alquileres/Alquiler.cs`

### Propiedades

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| VehiculoId | Guid | ID del vehículo alquilado |
| UserId | Guid | ID del usuario que reserva |
| Duracion | DateRange | Período de alquiler |
| PrecioPorPeriodo | Moneda | Costo por período |
| Mantenimiento | Moneda | Costo de mantenimiento |
| Accesorios | Moneda | Costo de accesorios |
| PrecioTotal | Moneda | Costo total |
| Status | AlquilerStatus | Estado actual |

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

- **Reservar**: Crea un nuevo alquiler y calcula precios
- **Confirmar**: Confirma una reserva pendiente
- **Rechazar**: Rechaza una reserva pendiente
- **Cancelar**: Cancela una reserva confirmada (antes de la fecha)
- **Completar**: Marca el alquiler como finalizado

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

## Value Objects

### Moneda

```csharp
public record Moneda
{
    public string TipoMoneda { get; }
    public decimal Monto { get; }
}
```

### DateRange

```csharp
public record DateRange
{
    public DateOnly Inicio { get; }
    public DateOnly Fin { get; }
}
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

---

## Servicios de Dominio

### PrecioService

Calcula el costo total del alquiler basado en:
- Precio por período del vehículo
- Duración del alquiler
- Costos de mantenimiento
- Accesorios seleccionados
