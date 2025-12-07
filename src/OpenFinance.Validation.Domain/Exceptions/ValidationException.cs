using OpenFinance.Validation.Domain.ValueObjects;

namespace OpenFinance.Validation.Domain.Exceptions;

/// <summary>
/// Exceção base para erros de validação
/// </summary>
public class ValidationException : Exception
{
    public int StatusCode { get; }
    public ErrorResponse ErrorResponse { get; }

    public ValidationException(int statusCode, ErrorResponse errorResponse, string? message = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorResponse = errorResponse;
    }
}