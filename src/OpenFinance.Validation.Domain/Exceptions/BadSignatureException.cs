using System.Net;
using OpenFinance.Validation.Domain.ValueObjects;

namespace OpenFinance.Validation.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando a assinatura JWT é inválida
/// </summary>
public class BadSignatureException : ValidationException
{
    public BadSignatureException(ErrorResponse errorResponse) 
        : base((int)HttpStatusCode.BadRequest, errorResponse, "Bad Signature")
    {
    }
}