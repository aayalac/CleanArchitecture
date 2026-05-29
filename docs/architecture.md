# Arquitectura del Sistema

## Clean Architecture

El proyecto implementa Clean Architecture con las siguientes capas:

```
        ┌─────────────────────────────────────┐
        │        Presentation (Api)           │
        │   Controladores, Middleware, DI     │
        └──────────────┬──────────────────────┘
                       │
        ┌──────────────▼──────────────────────┐
        │         Application                  │
        │   Casos de uso, CQRS, Validación     │
        └──────────────┬──────────────────────┘
                       │
        ┌──────────────▼──────────────────────┐
        │           Domain                     │
        │   Entidades, Value Objects, Events   │
        └──────────────┬──────────────────────┘
                       │
        ┌──────────────▼──────────────────────┐
        │        Infrastructure                │
        │   EF Core, Repositorios, Servicios   │
        └─────────────────────────────────────┘
```

### 1. Domain Layer (`CleanArchitecture.Domain`)

Contiene la lógica de negocio pura sin dependencias externas:

```
Domain/
├── Abstractions/        → Clases base (Entity, Result, Error, IUnitOfWork, IDomainEvent)
├── Shared/              → Value objects compartidos (Moneda, TipoMoneda)
├── Alquileres/
│   ├── Alquiler.cs      → Entidad principal con estados y reglas de negocio
│   ├── AlquilerStatus.cs
│   ├── AlquilerErrors.cs
│   ├── DateRange.cs     → Value object de período
│   ├── PrecioService.cs → Servicio de dominio para cálculo de precios
│   ├── PrecioDetalle.cs
│   ├── IAlquilerRepository.cs
│   └── Events/          → 5 eventos de dominio del ciclo de vida del alquiler
├── Vehiculos/
│   ├── Vehiculo.cs
│   ├── VehiculoErrors.cs
│   ├── IVehiculoRepository.cs
│   ├── Vin.cs, Modelo.cs, Direccion.cs, Accesorio.cs
├── Users/
│   ├── User.cs
│   ├── UserErrors.cs
│   ├── IUserRepository.cs
│   ├── Email.cs, Nombre.cs, Apellido.cs
│   └── Events/
└── Reviews/
    ├── Review.cs
    ├── ReviewErrors.cs
    ├── Raiting.cs, Comentario.cs
    └── Events/
```

**Principios:**
- Entidades con constructor privado (factory method)
- Value objects para conceptos inmutables
- Eventos de dominio para comunicar cambios
- Repositorios como interfaces (abstracciones)

### 2. Application Layer (`CleanArchitecture.Application`)

Capa de orquestación y casos de uso:

```
Application/
├── DependencyInjection.cs
├── Abstractions/
│   ├── Behaviors/
│   │   ├── LoggingBehavior.cs        → Logging de requests
│   │   └── ValidationBehavior.cs     → Pipeline de validación
│   ├── Messaging/
│   │   ├── ICommand.cs / ICommandHandler.cs
│   │   └── IQuery.cs / IQueryHandler.cs
│   ├── Email/
│   │   └── IEmailservice.cs
│   ├── Clock/
│   │   └── IDateTimeProvider.cs
│   └── Data/
│       └── ISqlConnectionFactory.cs
├── Exceptions/
│   ├── ConcurrencyException.cs
│   ├── ValidationException.cs
│   └── ValidationError.cs
├── Alquileres/
│   ├── ReservarAlquiler/
│   │   ├── ReservarAlquilerCommand.cs
│   │   ├── ReservarAlquilerCommandHandler.cs
│   │   ├── ReservarAlquilerCommandValidator.cs
│   │   └── ReserverAlquilerDomainEventHandler.cs
│   └── GetAlquiler/
│       ├── GetAlquilerQuery.cs
│       ├── GetAlquilerQueryHandler.cs
│       └── AlquilerResponse.cs
└── Vehiculos/
    └── SearchVehiculos/
        ├── SearchVehiculosQuery.cs
        ├── SeachVehiculoQueryHandler.cs
        ├── VehiculoResponse.cs
        └── DireccionResponse.cs
```

**Patrones utilizados:**
- CQRS (Command Query Responsibility Segregation)
- Mediator (MediatR)
- Fluent Validation para validación
- Pipeline Behaviors (logging, validación)

### 3. Infrastructure Layer (`CleanArchitecture.Infrastructure`)

Implementaciones concretas y acceso a datos:

```
Infrastructure/
├── ApplicationDbContext.cs          → Contexto EF Core
├── DependencyInjection.cs
├── Configurations/
│   ├── AlquilerConfiguration.cs
│   ├── VehiculoConfiguration.cs
│   ├── UserConfiguration.cs
│   └── ReviewConfiguration.cs
├── Repositories/
│   ├── Repository.cs               → Repositorio genérico base
│   ├── AlquilerRepository.cs
│   ├── VehiculoRepository.cs
│   └── UserRepository.cs
├── Data/
│   ├── SqlConnectionFactory.cs     → Conexión Dapper para consultas
│   └── DateOnlyTypeHandler.cs
├── Migrations/
│   └── InitialCreate/
├── Email/
│   └── EmailService.cs
└── Clock/
    └── DateTimeProvider.cs
```

**Tecnologías:**
- Entity Framework Core 7 con PostgreSQL (Npgsql)
- Convención de naming snake_case (EFCore.NamingConventions)
- Dapper para consultas de lectura
- Publicación de eventos de dominio en SaveChangesAsync

### 4. Api Layer (`CleanArchitecture.Api`)

Capa de presentación con ASP.NET Core:

```
Api/
├── Program.cs                        → Punto de entrada y configuración
├── appsettings.json / appsettings.Development.json
├── Controllers/
│   ├── Alquileres/
│   │   ├── AlquileresController.cs  → GET /api/alquileres/{id}, POST /api/alquileres
│   │   └── AlquilerReservaRequest.cs
│   └── Vehiculos/
│       └── VehiculosController.cs   → GET /api/vehiculos
├── Extensions/
│   ├── ApplicationBuilderExtensions.cs  → Auto-migración y seed data
│   └── SeedDataExtensions.cs         → Genera 100 vehículos con Bogus
├── Middleware/
│   └── ExceptionHandlingMiddleware.cs → Manejo global de excepciones
└── Properties/
    └── launchSettings.json
```

**Tecnologías:**
- ASP.NET Core 8 Web API
- Swashbuckle (Swagger/OpenAPI)
- Bogus para datos semilla

## Flujo de una Operación

```
1. Client → API Controller (Api)
2. Controller → MediatR → Command/Query Handler (Application)
3. Handler → Repository (Infrastructure)
4. Repository → EF Core → PostgreSQL
5. Domain Event → MediatR → Event Handler (Application)
```

## Paquetes NuGet por Capa

| Capa | Paquete | Versión | Propósito |
|------|---------|---------|-----------|
| Domain | MediatR.Contracts | 2.0.1 | Contratos de eventos de dominio |
| Application | MediatR | 12.1.1 | CQRS y mediator |
| Application | FluentValidation.DependencyInjectionExtensions | 11.8.0 | Validación de comandos |
| Application | Dapper | 2.1.15 | Consultas de lectura |
| Application | Microsoft.Extensions.Logging.Abstractions | 8.0.0-rc.2 | Logging |
| Infrastructure | Microsoft.EntityFrameworkCore | 7.0.11 | ORM |
| Infrastructure | Npgsql.EntityFrameworkCore.PostgreSQL | 7.0.11 | Provider PostgreSQL |
| Infrastructure | EFCore.NamingConventions | 7.0.2 | snake_case |
| Infrastructure | Microsoft.AspNetCore.Authentication.JwtBearer | 8.0.0-rc.1 | Autenticación JWT |
| Api | Swashbuckle.AspNetCore | 6.6.2 | Swagger |
| Api | Bogus | 34.0.2 | Datos semilla |

## Patrones de Diseño

| Patrón | Uso |
|--------|-----|
| **Repository** | Acceso a datos abstracto por entidad |
| **Unit of Work** | Transacciones y publicación de eventos |
| **Domain Events** | Comunicación desacoplada entre capas |
| **Value Objects** | Conceptos inmutables (Moneda, DateRange, Email, etc.) |
| **Factory Method** | Creación controlada de entidades |
| **CQRS** | Separación de comandos y consultas |
| **Pipeline Behavior** | Validación y logging transversal |
