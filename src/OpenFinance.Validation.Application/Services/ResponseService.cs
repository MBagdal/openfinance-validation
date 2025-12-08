using OpenFinance.Validation.Application.Interfaces;
using OpenFinance.Validation.Domain.Constants.Erros;
using OpenFinance.Validation.Domain.ValueObjects;

namespace OpenFinance.Validation.Application.Services;

/// <summary>
/// Serviço para criação de respostas de erro padronizadas
/// </summary>
public class ResponseService : IResponseService
{
    public ErrorResponse ReturnUnauthorized()
    {
        return new ErrorResponse
        {
            Errors = new List<ErrorDetail>
            {
                new()
                {
                    Code = ErrorCodes.Unauthorized,
                    Title = ErrorTitles.Unauthorized,
                    Detail = ErrorDetails.Unauthorized
                }
            },
            Meta = new ResponseMeta()
        };
    }

    public ErrorResponse ReturnBadRequest()
    {
        return new ErrorResponse
        {
            Errors = new List<ErrorDetail>
            {
                new()
                {
                    Code = ErrorCodes.MissingMandatoryHeaders,
                    Title = ErrorTitles.MissingMandatoryHeaders,
                    Detail = ErrorDetails.MissingMandatoryHeaders
                }
            },
            Meta = new ResponseMeta()
        };
    }

    public ErrorResponse ReturnNotFound()
    {
        return new ErrorResponse
        {
            Errors = new List<ErrorDetail>
            {
                new()
                {
                    Code = ErrorCodes.ResourceNotFound,
                    Title = ErrorTitles.ResourceNotFound,
                    Detail = ErrorDetails.ResourceNotFound
                }
            },
            Meta = new ResponseMeta()
        };
    }

    public ErrorResponse ReturnBadSignature()
    {
        return new ErrorResponse
        {
            Errors = new List<ErrorDetail>
            {
                new()
                {
                    Code = ErrorCodes.BadSignature,
                    Title = ErrorTitles.BadSignature,
                    Detail = ErrorDetails.BadSignature
                }
            },
            Meta = new ResponseMeta()
        };
    }
}