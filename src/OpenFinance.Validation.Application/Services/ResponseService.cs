using OpenFinance.Validation.Application.Interfaces;
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
                    Code = "UNAUTHORIZED",
                    Title = "Unauthorised",
                    Detail = "The authorisation token was not sent or is invalid"
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
                    Code = "MISSING_MANDATORY_HEADERS",
                    Title = "Missing mandatory headers",
                    Detail = "A mandatory header was not sent"
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
                    Code = "RESOURCE_NOT_FOUND",
                    Title = "Resource not found",
                    Detail = "Resource not found"
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
                    Code = "BAD_SIGNATURE",
                    Title = "Bad signature",
                    Detail = "Could not verify the message signature"
                }
            },
            Meta = new ResponseMeta()
        };
    }
}