using OpenFinance.Validation.Domain.ValueObjects;

namespace OpenFinance.Validation.Application.Interfaces;

/// <summary>
/// Interface para serviços de resposta
/// </summary>
public interface IResponseService
{
    ErrorResponse ReturnUnauthorized();
    ErrorResponse ReturnBadRequest();
    ErrorResponse ReturnNotFound();
    ErrorResponse ReturnBadSignature();
}