using System.ComponentModel.DataAnnotations;

namespace UserHub.DTOs;

/// <summary>
/// Usado en PUT /api/User. Reemplaza el perfil completo (excepto Role, Id, Email y CreatedAt).
/// </summary>
public class UpdateUserDto
{
    [Required]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public string Country { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;
}
