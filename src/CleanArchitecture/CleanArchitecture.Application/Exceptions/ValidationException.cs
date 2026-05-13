using CleanArchitecture.Application.Exceptions;

namespace CleanArchitecture.Application.Exceptions;

public sealed class ValidationException : Exception
{
    public ValidationException(IEnumerable<Validationerror> errors)
    {
        Errors = errors;
    }

    public IEnumerable<Validationerror> Errors {get;}
}