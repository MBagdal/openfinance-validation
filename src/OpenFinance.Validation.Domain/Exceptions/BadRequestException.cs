using System.Net;
using OpenFinance.Validation.Domain.ValueObjects;

namespace OpenFinance.Validation.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando a requisição é inválida
/// </summary>
public class BadRequestException : ValidationException
{
    public BadRequestException(ErrorResponse errorResponse) 
        : base((int)HttpStatusCode.BadRequest, errorResponse, "Bad Request")
    {
    }
}