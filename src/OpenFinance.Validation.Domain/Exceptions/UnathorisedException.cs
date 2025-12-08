using System.Net;
using OpenFinance.Validation.Domain.ValueObjects;

namespace OpenFinance.Validation.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando a autenticação falha
/// </summary>
public class UnauthorisedException : ValidationException
{
    public UnauthorisedException(ErrorResponse errorResponse) 
        : base(HttpStatusCode.Unauthorized, errorResponse, "Unauthorised")
    {
    }
}