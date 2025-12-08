namespace OpenFinance.Validation.Domain.Constants;

/// <summary>
/// Códigos de erro padronizados Open Finance
/// </summary>
public static class ErrorCodes
{
    /// <summary>
    /// Token de autorização não foi enviado ou é inválido
    /// </summary>
    public const string Unauthorized = "UNAUTHORIZED";

    /// <summary>
    /// Headers obrigatórios não foram enviados
    /// </summary>
    public const string MissingMandatoryHeaders = "MISSING_MANDATORY_HEADERS";

    /// <summary>
    /// Recurso não encontrado
    /// </summary>
    public const string ResourceNotFound = "RESOURCE_NOT_FOUND";

    /// <summary>
    /// Assinatura JWT inválida
    /// </summary>
    public const string BadSignature = "BAD_SIGNATURE";

    /// <summary>
    /// Token expirado
    /// </summary>
    public const string TokenExpired = "TOKEN_EXPIRED";

    /// <summary>
    /// Escopo insuficiente
    /// </summary>
    public const string InsufficientScope = "INSUFFICIENT_SCOPE";

    /// <summary>
    /// Certificado inválido
    /// </summary>
    public const string InvalidCertificate = "INVALID_CERTIFICATE";

    /// <summary>
    /// Erro interno do servidor
    /// </summary>
    public const string InternalServerError = "INTERNAL_SERVER_ERROR";
}