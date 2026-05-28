# Arquitectura del Sistema

## Clean Architecture

El proyecto implementa Clean Architecture con las siguientes capas:

### 1. Domain Layer (`CleanArchitecture.Domain`)

Contiene la lógica de negocio pura sin dependencias externas:

```
Domain/
├── Abstractions/     → Clases base (Entity, Result)
├── Alquileres/       → Entidad Alquiler y reglas de negocio
├── Vehiculos/        → Entidad Vehiculo y value objects
├── Users/            → Entidad User
├── Shared/           → Value objects compartidos (Moneda)
└── Reviews/          → Sistema de calificaciones
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
├── Alquileres/
│   ├── ReservarAlquiler/    → Command + Handler + Validator
│   └── GetAlquiler/         → Query + Handler
├── Vehiculos/               → Operaciones de vehículos
├── Abstractions/            → Interfaces de aplicación
├── Exceptions/              → Excepciones personalizadas
└── DependencyInjection.cs   → Configuración de DI
```

**Patrones utilizados:**
- CQRS (Command Query Responsibility Segregation)
- Mediator (MediatR)
- Fluent Validation para validación

### 3. Infrastructure Layer (`CleanArchitecture.Infrastructure`)

Implementaciones concretas y acceso a datos:

```
Infrastructure/
├── ApplicationDbContext     → Contexto Entity Framework
├── Configurations/          → Configuración EF por entidad
├── Data/                    → Migraciones y datos semilla
├── Repositories/            → Implementación de repositorios
├── Email/                   → Servicio de email
├── Clock/                   → Servicio de tiempo
└── DependencyInjection.cs   → Configuración de DI
```

**Tecnologías:**
- Entity Framework Core 8
- SQL Server (configurable)
- MediatR para publicación de eventos

## Flujo de una Operación

```
1. Client → API (Presentation)
2. API → MediatR → Command/Query Handler (Application)
3. Handler → Repository (Domain/Infrastructure)
4. Repository → EF Core → Database
5. Domain Event → MediatR → Event Handler (Application)
```

## Patrones de Diseño

| Patrón | Uso |
|--------|-----|
| **Repository** | Acceso a datos abstracto |
| **Unit of Work** | Transacciones y publicación de eventos |
| **Domain Events** | Comunicación desacoplada entre capas |
| **Value Objects** | Conceptos inmutables (Moneda, DateRange) |
| **Factory Method** | Creación controlada de entidades |
