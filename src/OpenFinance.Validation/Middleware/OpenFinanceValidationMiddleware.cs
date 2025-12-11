using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenFinance.Validation.Domain.Constants.Errors;
using OpenFinance.Validation.Domain.Exceptions;
using OpenFinance.Validation.Domain.ValueObjects;

namespace OpenFinance.Validation.Middleware;

/// <summary>
/// Middleware para tratamento automático de exceções de validação Open Finance
/// </summary>
public class OpenFinanceValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<OpenFinanceValidationMiddleware> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public OpenFinanceValidationMiddleware(
        RequestDelegate next, 
        ILogger<OpenFinanceValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleUnexpectedExceptionAsync(context, ex);
        }
    }

    private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
    {
        _logger.LogWarning(
            ex, 
            "Validation error. StatusCode: {StatusCode}, Path: {Path}", 
            ex.StatusCode,
            context.Request.Path);

        context.Response.StatusCode = (int)ex.StatusCode;
        context.Response.ContentType = "application/json";

        var json = JsonSerializer.Serialize(ex.ErrorResponse, JsonOptions);
        await context.Response.WriteAsync(json, Encoding.UTF8);
    }

    private async Task HandleUnexpectedExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(
            ex, 
            "Unexpected error. Path: {Path}, Method: {Method}", 
            context.Request.Path,
            context.Request.Method);

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var errorResponse = CreateInternalServerErrorResponse();

        var json = JsonSerializer.Serialize(errorResponse, JsonOptions);
        await context.Response.WriteAsync(json, Encoding.UTF8);
    }

    /// <summary>
    /// Cria ErrorResponse para erro interno usando a mesma estrutura
    /// </summary>
    private ErrorResponse CreateInternalServerErrorResponse()
    {
        return new ErrorResponse
        {
            Errors = new List<ErrorDetail>
            {
                new()
                {
                    Code = ErrorCodes.InternalServerError,
                    Title = ErrorTitles.InternalServerError,
                    Detail = ErrorDetails.InternalServerError
                }
            },
            Meta = new ResponseMeta { RequestDateTime = DateTime.UtcNow }
        };
    }
}