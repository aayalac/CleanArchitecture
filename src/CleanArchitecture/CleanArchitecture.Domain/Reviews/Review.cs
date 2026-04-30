using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Reviews.Events;

namespace CleanArchitecture.Domain.Reviews;

public sealed class Review : Entity
{
    private Review(
        Guid id,
        Guid vehiculoId,
        Guid alquilerId,
        Guid userId,
        Raiting raiting,
        Comentario comentario,
        DateTime fechaCreacion
    ): base(id)
    {
        Id = id;
        VehiculoId = vehiculoId;
        AlquilerId = alquilerId;
        UserId = userId;
        Raiting = raiting;
        Comentario = comentario;
        FechaCreacion = fechaCreacion;
    }
    public Guid VehiculoId {get; private set;}
    public Guid AlquilerId {get; private set;} 
    public Guid UserId {get; private set;}
    public Raiting Raiting {get; private set;}
    public Comentario Comentario {get; private set;}
    public DateTime FechaCreacion {get; private set;}

    public static Result<Review> Create(
        Alquiler alquiler,
        Raiting raiting,
        Comentario comentario,
        DateTime fechaCreacion
    )
    {
        if(alquiler.Status != AlquilerStatus.Completado)
        {
            return Result.Failure<Review>(ReviewErrors.NotEligible);
        }

        var review = new Review(
            Guid.NewGuid(),
            alquiler.VehiculoId,
            alquiler.Id,
            alquiler.UserId,
            raiting,
            comentario,
            fechaCreacion
        );

        review.RaiseDomainEvent(new ReviewCreateDomainEvent(review.Id));

        return review;
    }
}