# Documentación del Sistema de Alquiler de Vehículos

## Descripción General

Sistema de alquiler de vehículos desarrollado utilizando **Clean Architecture** con principios de **Domain-Driven Design (DDD)** en .NET 8.

## Arquitectura

El proyecto sigue una arquitectura de 4 capas:

```
CleanArchitecture/
├── Domain/          → Lógica de negocio y reglas de dominio
├── Application/     → Casos de uso y orquestación
├── Infrastructure/  → Persistencia, servicios externos
└── Presentation/    → API REST (no implementada aún)
```

## Entidades Principales

| Entidad | Descripción |
|---------|-------------|
| **Vehiculo** | Vehículos disponibles para alquiler con modelo, VIN, precio y accesorios |
| **Alquiler** | Reservas de vehículos con estados y cálculo de precios |
| **User** | Usuarios del sistema |

## Estados del Alquiler

```
Reservado → Confirmado → Completado
    ↓           ↓
Rechazado    Cancelado
```

## Comandos y Queries

- **ReservarAlquilerCommand**: Crea una nueva reserva de vehículo
- **GetAlquilerQuery**: Consulta detalles de un alquiler

## Documentación Detallada

- [Arquitectura](./architecture.md)
- [Dominio](./domain.md)
- [Flujos Principales](./workflows.md)
