using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Application.Vehiculos.SearchVehiculos;

public sealed record SearchVehiculosQuery(
    DateOnly  fechaInicio,
    DateOnly fechaFin
    ): IQuery<IReadOnlyList<VehiculoResponse>>;