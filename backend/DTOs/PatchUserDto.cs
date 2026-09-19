namespace UserHub.DTOs;

/// <summary>
/// Usado en PATCH /api/User. Todos los campos son opcionales;
/// solo se actualizan los que vengan con valor (no nulos).
/// </summary>
public class PatchUserDto
{
    public string? DisplayName { get; set; }
    public string? Username { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Country { get; set; }
    public string? Bio { get; set; }
}
