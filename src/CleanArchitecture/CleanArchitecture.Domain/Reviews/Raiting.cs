using System.Buffers;
using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews;

public sealed record Raiting
{
    public static readonly Error Invalid = new("Raiting.Invalid","El raiting es invalido.");
    public int Value {get;init;}

    // private Raiting(int value)
    // {
    //      Value = value // Se puede usar así tambien 
    // }
    private Raiting(int value) => Value = value;

    public static Result<Raiting>Create(int value)
    {
        if(value < 1 || value > 5)
        {
            return Result.Failure<Raiting>(Invalid);
        }

        return new Raiting(value);
    }
}