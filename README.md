# CleanArchitecture

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![GitHub](https://img.shields.io/badge/github-aayalac/CleanArchitecture-informational)](https://github.com/aayalac/CleanArchitecture)
![.NET 8](https://img.shields.io/badge/.NET-8.0-purple)
![Language: C#](https://img.shields.io/badge/language-C%23-239120)

## 📋 Descripción

Este proyecto implementa una **arquitectura de software limpia** combinada con principios de **Domain-Driven Design (DDD)**. Está diseñado para demostrar las mejores prácticas en la construcción de aplicaciones C# escalables, mantenibles y desacopladas.

## 🎯 Características Principales

- ✅ **Clean Architecture**: Separación clara de capas (Domain, Application, Infrastructure, Presentation)
- ✅ **Domain-Driven Design (DDD)**: Entidades, agregados y especificaciones de dominio
- ✅ **C# con .NET 8**: Aprovechando las características modernas del framework
- ✅ **Código modular**: Fácil de mantener, probar y extender
- ✅ **Desacoplamiento**: Bajo acoplamiento entre capas mediante patrones SOLID

## 📚 Documentación

Para información detallada, consulta la carpeta [`docs/`](./docs/):

- [Arquitectura](./docs/architecture.md) - Estructura y patrones del sistema
- [Modelo de Dominio](./docs/domain.md) - Entidades, value objects y eventos
- [Flujos Principales](./docs/workflows.md) - Casos de uso y procesos

## 🏗️ Estructura del Proyecto

```
CleanArchitecture/
├── CleanArchitecture.sln
├── global.json                          → Pinned .NET SDK 8.0.419
├── docs/                                → Documentación del proyecto
│   ├── architecture.md
│   ├── domain.md
│   ├── workflows.md
│   └── consumo-apis/                    → Colecciones Postman
└── src/CleanArchitecture/
    ├── CleanArchitecture.Domain/        → Entidades, value objects, eventos de dominio
    ├── CleanArchitecture.Application/   → Casos de uso (CQRS), validación, abstracciones
    ├── CleanArchitecture.Infrastructure/→ EF Core, repositorios, servicios externos
    └── CleanArchitecture.Api/           → Controladores REST, middleware, Swagger
```

### Capas

| Capa | Proyecto | Responsabilidad |
|------|----------|-----------------|
| **Domain** | `CleanArchitecture.Domain` | Lógica de negocio pura: entidades, value objects, eventos de dominio, interfaces de repositorio |
| **Application** | `CleanArchitecture.Application` | Casos de uso con CQRS (MediatR), validación (FluentValidation), abstracciones de infraestructura |
| **Infrastructure** | `CleanArchitecture.Infrastructure` | Persistencia (EF Core + PostgreSQL), repositorios concretos, servicios de email y reloj |
| **Api** | `CleanArchitecture.Api` | ASP.NET Core Web API: controladores, middleware de excepciones, Swagger, datos semilla |

### Tecnologías

- **.NET 8** / C# con nullable references habilitado
- **Entity Framework Core 7** con PostgreSQL (Npgsql)
- **MediatR** para CQRS y publicación de eventos de dominio
- **FluentValidation** para validación de comandos
- **Dapper** para consultas de lectura
- **Bogus** para generación de datos semilla
- **Swashbuckle** para documentación Swagger/OpenAPI

### Ejecutar el proyecto

```bash
cd src/CleanArchitecture/CleanArchitecture.Api
dotnet run
```

La API se inicia en `http://localhost:9000` con Swagger habilitado en `/swagger`.
