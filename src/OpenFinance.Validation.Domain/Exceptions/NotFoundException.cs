using System.Net;
using OpenFinance.Validation.Domain.ValueObjects;

namespace OpenFinance.Validation.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando um recurso não é encontrado
/// </summary>
public class NotFoundException : ValidationException
{
    public NotFoundException(ErrorResponse errorResponse) 
        : base((int)HttpStatusCode.NotFound, errorResponse, "Not Found")
    {
    }
}