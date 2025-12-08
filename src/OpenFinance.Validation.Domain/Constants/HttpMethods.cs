namespace OpenFinance.Validation.Domain.Constants;

/// <summary>
/// Constantes para métodos HTTP
/// </summary>
public static class HttpMethods
{
    public const string Get = "GET";
    public const string Post = "POST";
    public const string Put = "PUT";
    public const string Delete = "DELETE";
    public const string Patch = "PATCH";
    public const string Head = "HEAD";
    public const string Options = "OPTIONS";
}

/// <summary>
/// Helper para determinar características de métodos HTTP
/// </summary>
public static class HttpMethodHelper
{
    /// <summary>
    /// Verifica se o método HTTP requer validação de idempotência
    /// POST e PATCH requerem validação de idempotência
    /// </summary>
    public static bool RequiresIdempotencyValidation(string method)
    {
        return !IsIdempotent(method);
    }

    /// <summary>
    /// Verifica se o método HTTP é idempotente por natureza
    /// GET, PUT, DELETE são idempotentes
    /// </summary>
    public static bool IsIdempotent(string method)
    {
        return method == HttpMethods.Get 
            || method == HttpMethods.Put 
            || method == HttpMethods.Delete
            || method == HttpMethods.Head
            || method == HttpMethods.Options;
    }

    /// <summary>
    /// Verifica se o método HTTP pode ter body (POST, PUT, PATCH)
    /// </summary>
    public static bool HaveRequestBody(string method)
    {
        return method == HttpMethods.Post 
            || method == HttpMethods.Put 
            || method == HttpMethods.Patch;
    }
}