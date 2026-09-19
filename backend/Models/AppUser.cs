using Google.Cloud.Firestore;

namespace UserHub.Models;

/// <summary>
/// Perfil de usuario almacenado en Firestore (colección "users").
/// El Id del documento en Firestore es el mismo que el localId/uid de Firebase Authentication.
/// </summary>
[FirestoreData]
public class AppUser
{
    /// <summary>Identificador del usuario en Firebase (localId / uid). Es el Id del documento.</summary>
    [FirestoreProperty]
    public string Id { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Email { get; set; } = string.Empty;

    [FirestoreProperty]
    public string DisplayName { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Username { get; set; } = string.Empty;

    [FirestoreProperty]
    public string PhoneNumber { get; set; } = string.Empty;

    [FirestoreProperty]
    public DateTime BirthDate { get; set; }

    [FirestoreProperty]
    public string Country { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Bio { get; set; } = string.Empty;

    /// <summary>Solo "Admin" o "Usuario". Se asigna en backend al registrar, nunca desde el cliente.</summary>
    [FirestoreProperty]
    public string Role { get; set; } = "Usuario";

    /// <summary>Asignado por el servidor en UTC al momento del registro.</summary>
    [FirestoreProperty]
    public DateTime CreatedAt { get; set; }
}
