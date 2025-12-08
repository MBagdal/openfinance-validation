namespace OpenFinance.Validation.Domain.Constants.Erros;

/// <summary>
/// Detalhes de erro padronizados Open Finance
/// </summary>
public static class ErrorDetails
{
    public const string Unauthorized = "The authorisation token was not sent or is invalid";
    public const string MissingMandatoryHeaders = "A mandatory header was not sent";
    public const string ResourceNotFound = "Resource not found";
    public const string BadSignature = "Could not verify the message signature";
    public const string TokenExpired = "The access token has expired";
    public const string InsufficientScope = "The token does not have the required scope";
    public const string InvalidCertificate = "The client certificate is invalid or does not match the token";
    public const string InternalServerError = "An unexpected error occurred";
}