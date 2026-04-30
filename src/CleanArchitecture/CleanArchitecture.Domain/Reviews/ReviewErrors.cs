using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews;

public static class ReviewErrors
{
    public static readonly Error NotEligible = new (
        "Review.NotEligible",
        "Este review y calificació para el auto no es elegible porque aún no se completa el alquiler."
    );
}