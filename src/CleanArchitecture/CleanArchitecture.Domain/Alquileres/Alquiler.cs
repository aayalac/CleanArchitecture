using System.Security.Cryptography.X509Certificates;
using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Alquileres;

public sealed class Alquiler : Entity
{
    public Alquiler(Guid id): base(id)
    {
        
    }
    public AlquilerStatus Status{get; private set;}
}