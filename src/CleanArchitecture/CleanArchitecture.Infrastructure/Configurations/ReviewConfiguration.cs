using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Reviews;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");

        builder.HasKey(review => review.Raiting);

        builder.Property(review => review.Raiting)
            .HasConversion(raiting => raiting!.Value, value => Raiting.Create(value).Value);
        
        builder.Property(review => review.Comentario)
            .HasMaxLength(200)
            .HasConversion(comnetario => comnetario!.Value, value => new Comentario(value));

        builder.HasOne<Vehiculo>()
            .WithMany()
            .HasForeignKey(review => review.VehiculoId);

        builder.HasOne<Alquiler>()
            .WithMany()
            .HasForeignKey(review => review.AlquilerId);
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(review => review.UserId);
    }
}