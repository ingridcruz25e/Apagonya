namespace UserHub.Firebase;

/// <summary>Configuración leída desde la sección "Firebase" de appsettings.json.</summary>
public class FirebaseOptions
{
    /// <summary>Web API Key del proyecto de Firebase (Project Settings > General).</summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>Project ID de Firebase.</summary>
    public string ProjectId { get; set; } = string.Empty;

    /// <summary>Ruta al archivo de credenciales de la cuenta de servicio (NO subir al repo).</summary>
    public string CredentialsPath { get; set; } = "firebase-credentials.json";
}
