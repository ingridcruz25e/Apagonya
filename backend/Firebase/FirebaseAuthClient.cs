using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace UserHub.Firebase;

/// <summary>
/// Cliente HTTP para la API REST de Firebase Identity Toolkit (Firebase Authentication).
/// Se usa exclusivamente para registrar y autenticar usuarios con email/password,
/// ya que el Admin SDK no puede generar un idToken a partir de una contraseña.
/// </summary>
public class FirebaseAuthClient
{
    private const string BaseUrl = "https://identitytoolkit.googleapis.com/v1/accounts";

    private readonly HttpClient _http;
    private readonly FirebaseOptions _options;

    public FirebaseAuthClient(HttpClient http, IOptions<FirebaseOptions> options)
    {
        _http = http;
        _options = options.Value;
    }

    public async Task<FirebaseSignUpResponse> SignUpAsync(string email, string password)
    {
        var url = $"{BaseUrl}:signUp?key={_options.ApiKey}";
        var payload = new { email, password, returnSecureToken = true };

        var response = await _http.PostAsJsonAsync(url, payload);
        await EnsureSuccessAsync(response);

        var result = await response.Content.ReadFromJsonAsync<FirebaseSignUpResponse>();
        return result ?? throw new InvalidOperationException("Firebase no devolvió una respuesta válida al registrar.");
    }

    public async Task<FirebaseSignInResponse> SignInAsync(string email, string password)
    {
        var url = $"{BaseUrl}:signInWithPassword?key={_options.ApiKey}";
        var payload = new { email, password, returnSecureToken = true };

        var response = await _http.PostAsJsonAsync(url, payload);
        await EnsureSuccessAsync(response);

        var result = await response.Content.ReadFromJsonAsync<FirebaseSignInResponse>();
        return result ?? throw new InvalidOperationException("Firebase no devolvió una respuesta válida al iniciar sesión.");
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;

        var body = await response.Content.ReadFromJsonAsync<FirebaseErrorResponse>();
        var message = body?.Error?.Message ?? "Error desconocido de Firebase Authentication.";
        throw new FirebaseAuthException(message);
    }
}

public class FirebaseSignUpResponse
{
    [JsonPropertyName("idToken")]
    public string IdToken { get; set; } = string.Empty;

    [JsonPropertyName("localId")]
    public string LocalId { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}

public class FirebaseSignInResponse
{
    [JsonPropertyName("idToken")]
    public string IdToken { get; set; } = string.Empty;

    [JsonPropertyName("localId")]
    public string LocalId { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}

internal class FirebaseErrorResponse
{
    [JsonPropertyName("error")]
    public FirebaseErrorDetail? Error { get; set; }
}

internal class FirebaseErrorDetail
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}

/// <summary>Excepción lanzada cuando Firebase Authentication rechaza una operación.</summary>
public class FirebaseAuthException : Exception
{
    public FirebaseAuthException(string message) : base(message) { }
}
