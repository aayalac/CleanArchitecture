namespace CleanArchitecture.Application.Exceptions;

public sealed record Validationerror(
    string PropertyName,
    string ErrorMessage
);