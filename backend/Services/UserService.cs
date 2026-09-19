using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using UserHub.DTOs;
using UserHub.Models;

namespace UserHub.Services;

public class UserService : IUserService
{
    private const string CollectionName = "users";
    private readonly FirestoreDb _firestore;

    public UserService(FirestoreDb firestore)
    {
        _firestore = firestore;
    }

    public async Task<AppUser> CreateProfileAsync(string uid, string email, RegisterDto dto)
    {
        var user = new AppUser
        {
            Id = uid,
            Email = email,
            DisplayName = dto.DisplayName,
            Username = dto.Username,
            PhoneNumber = dto.PhoneNumber,
            BirthDate = dto.BirthDate,
            Country = dto.Country,
            Bio = dto.Bio,
            Role = "ciudadano", // Siempre por defecto. Nunca se toma del cliente.
            CreatedAt = DateTime.UtcNow
        };

        var docRef = _firestore.Collection(CollectionName).Document(uid);
        await docRef.SetAsync(user);

        return user;
    }

    public async Task<AppUser?> GetByIdAsync(string uid)
    {
        var snapshot = await _firestore.Collection(CollectionName).Document(uid).GetSnapshotAsync();
        return snapshot.Exists ? snapshot.ConvertTo<AppUser>() : null;
    }

    public async Task<AppUser> UpdateFullAsync(string uid, UpdateUserDto dto)
    {
        var existing = await GetByIdAsync(uid)
            ?? throw new KeyNotFoundException("El perfil del usuario no existe en Firestore.");

        existing.DisplayName = dto.DisplayName;
        existing.Username = dto.Username;
        existing.PhoneNumber = dto.PhoneNumber;
        existing.BirthDate = dto.BirthDate;
        existing.Country = dto.Country;
        existing.Bio = dto.Bio;
        // Id, Email, Role y CreatedAt se preservan intencionalmente.

        var docRef = _firestore.Collection(CollectionName).Document(uid);
        await docRef.SetAsync(existing, SetOptions.Overwrite);

        return existing;
    }

    public async Task<AppUser> UpdatePartialAsync(string uid, PatchUserDto dto)
    {
        var existing = await GetByIdAsync(uid)
            ?? throw new KeyNotFoundException("El perfil del usuario no existe en Firestore.");

        var updates = new Dictionary<string, object>();

        if (dto.DisplayName is not null) { existing.DisplayName = dto.DisplayName; updates["DisplayName"] = dto.DisplayName; }
        if (dto.Username is not null) { existing.Username = dto.Username; updates["Username"] = dto.Username; }
        if (dto.PhoneNumber is not null) { existing.PhoneNumber = dto.PhoneNumber; updates["PhoneNumber"] = dto.PhoneNumber; }
        if (dto.BirthDate is not null) { existing.BirthDate = dto.BirthDate.Value; updates["BirthDate"] = dto.BirthDate.Value; }
        if (dto.Country is not null) { existing.Country = dto.Country; updates["Country"] = dto.Country; }
        if (dto.Bio is not null) { existing.Bio = dto.Bio; updates["Bio"] = dto.Bio; }
        // Role nunca se incluye aquí: no puede modificarse desde este endpoint.

        if (updates.Count > 0)
        {
            var docRef = _firestore.Collection(CollectionName).Document(uid);
            await docRef.UpdateAsync(updates);
        }

        return existing;
    }

    public async Task DeleteAsync(string uid)
    {
        // 1. Eliminar la cuenta en Firebase Authentication (Admin SDK).
        await FirebaseAuth.DefaultInstance.DeleteUserAsync(uid);

        // 2. Eliminar el perfil en Firestore.
        await _firestore.Collection(CollectionName).Document(uid).DeleteAsync();
    }
}
