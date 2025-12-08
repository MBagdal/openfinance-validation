using Microsoft.AspNetCore.Http;
using OpenFinance.Validation.Domain.Interfaces;

namespace OpenFinance.Validation.Infrastructure.Http;

/// <summary>
/// Implementação do validador de headers
/// </summary>
public class HeaderValidator : IHeaderValidator
{
    public bool ValidateIdempotencyHeaders(HttpRequest request)
    {
        var contentType = request.Headers["Content-Type"].ToString();

        var hasContentType = HasContentType(contentType);
        
        var hasInteractionId = HasInteractionId(request);
        
        var hasIdempotencyKey = HasIdempotencyKey(request);

        return hasContentType && hasInteractionId && hasIdempotencyKey;
    }

    public bool ValidateNonIdempotencyHeaders(HttpRequest request)
    {
        return request.Headers.ContainsKey("x-fapi-interaction-id");
    }

    private bool HasContentType(string contentType)
    {
        return contentType.Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Any(c => c.Trim().Equals("application/jwt", StringComparison.OrdinalIgnoreCase));
    }

    private bool HasInteractionId(HttpRequest request)
    {
        return request.Headers.ContainsKey("x-fapi-interaction-id");
    }

    private bool HasIdempotencyKey(HttpRequest request)
    {
        return request.Headers.ContainsKey("x-idempotency-key");
    }
}