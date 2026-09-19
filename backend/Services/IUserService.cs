using UserHub.DTOs;
using UserHub.Models;

namespace UserHub.Services;

public interface IUserService
{
    /// <summary>Crea el perfil en Firestore justo después del registro en Firebase Auth. Role = "Usuario" por defecto.</summary>
    Task<AppUser> CreateProfileAsync(string uid, string email, RegisterDto dto);

    Task<AppUser?> GetByIdAsync(string uid);

    /// <summary>Reemplaza el perfil completo (PUT). Nunca toca Role, Id, Email ni CreatedAt.</summary>
    Task<AppUser> UpdateFullAsync(string uid, UpdateUserDto dto);

    /// <summary>Actualiza parcialmente el perfil (PATCH). Nunca toca Role, Id, Email ni CreatedAt.</summary>
    Task<AppUser> UpdatePartialAsync(string uid, PatchUserDto dto);

    /// <summary>Elimina la cuenta en Firebase Authentication y el perfil en Firestore.</summary>
    Task DeleteAsync(string uid);
}
