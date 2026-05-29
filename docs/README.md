# Documentación del Sistema de Alquiler de Vehículos

## Descripción General

Sistema de alquiler de vehículos desarrollado utilizando **Clean Architecture** con principios de **Domain-Driven Design (DDD)** en .NET 8.

## Arquitectura

El proyecto sigue una arquitectura de 4 capas:

```
CleanArchitecture/
├── Domain/          → Lógica de negocio y reglas de dominio
├── Application/     → Casos de uso y orquestación (CQRS + MediatR)
├── Infrastructure/  → Persistencia (EF Core + PostgreSQL), servicios externos
└── Api/             → API REST (ASP.NET Core + Swagger)
```

## Entidades Principales

| Entidad | Descripción |
|---------|-------------|
| **Vehiculo** | Vehículos disponibles para alquiler con modelo, VIN, precio y accesorios |
| **Alquiler** | Reservas de vehículos con estados y cálculo de precios |
| **User** | Usuarios del sistema |
| **Review** | Calificaciones y comentarios de usuarios sobre vehículos alquilados |

## Value Objects

| Value Object | Dominio | Descripción |
|--------------|---------|-------------|
| Moneda / TipoMoneda | Shared | Dinero con moneda (USD/EUR) |
| DateRange | Alquileres | Período de fechas (Inicio/Fin) |
| Email | Users | Correo electrónico |
| Nombre / Apellido | Users | Nombre y apellido |
| Vin | Vehiculos | Número de identificación único del vehículo |
| Modelo | Vehiculos | Marca y modelo |
| Direccion | Vehiculos | Ubicación del vehículo |
| Raiting | Reviews | Calificación numérica (1-5) |
| Comentario | Reviews | Texto de comentario |

## Estados del Alquiler

```
Reservado → Confirmado → Completado
    ↓           ↓
Rechazado    Cancelado
```

## Comandos y Queries

| Operación | Tipo | Descripción |
|-----------|------|-------------|
| **ReservarAlquilerCommand** | Command | Crea una nueva reserva de vehículo |
| **GetAlquilerQuery** | Query | Consulta detalles de un alquiler por ID |
| **SearchVehiculosQuery** | Query | Busca vehículos disponibles por filtros |

## Endpoints API

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/alquileres/{id}` | Obtener alquiler por ID |
| POST | `/api/alquileres` | Crear nueva reserva |
| GET | `/api/vehiculos` | Buscar vehículos disponibles |

## Documentación Detallada

- [Arquitectura](./architecture.md) - Estructura y patrones del sistema
- [Dominio](./domain.md) - Entidades, value objects y eventos
- [Flujos Principales](./workflows.md) - Casos de uso y procesos
- [Consumo de APIs](./consumo-apis/) - Colecciones Postman para probar los endpoints
