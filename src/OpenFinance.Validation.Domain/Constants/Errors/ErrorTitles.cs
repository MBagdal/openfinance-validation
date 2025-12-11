namespace OpenFinance.Validation.Domain.Constants.Errors;

/// <summary>
/// Títulos de erro padronizados Open Finance
/// </summary>
public static class ErrorTitles
{
    public const string Unauthorized = "Unauthorised";
    public const string MissingMandatoryHeaders = "Missing mandatory headers";
    public const string ResourceNotFound = "Resource not found";
    public const string BadSignature = "Bad signature";
    public const string TokenExpired = "Token Expired";
    public const string InsufficientScope = "Insufficient Scope";
    public const string InvalidCertificate = "Invalid Certificate";
    public const string InternalServerError = "Internal Server Error";
}