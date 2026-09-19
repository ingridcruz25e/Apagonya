using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserHub.DTOs;
using UserHub.Services;
using AppUserResponse = UserHub.Models.AppUser;

namespace UserHub.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Extrae el uid del usuario autenticado desde el claim del JWT de Firebase.
    /// Nunca se acepta un UserId enviado por el cliente.
    /// </summary>
    private string GetUserId()
    {
        // Firebase ID tokens incluyen tanto "user_id" como "sub" con el mismo uid.
        var uid = User.FindFirst("user_id")?.Value ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(uid))
            throw new UnauthorizedAccessException("No se pudo determinar el usuario a partir del token.");

        return uid;
    }

    [HttpGet]
    public async Task<ActionResult<AppUserResponse>> GetProfile()
    {
        var uid = GetUserId();
        var user = await _userService.GetByIdAsync(uid);
        return user is null ? NotFound() : Ok(user);
    }

    /// <summary>Actualiza el perfil completo del usuario autenticado.</summary>
    [HttpPut]
    public async Task<ActionResult<AppUserResponse>> UpdateFull([FromBody] UpdateUserDto dto)
    {
        var uid = GetUserId();
        var updated = await _userService.UpdateFullAsync(uid, dto);
        return Ok(updated);
    }

    /// <summary>Actualiza parcialmente uno o varios campos del perfil del usuario autenticado.</summary>
    [HttpPatch]
    public async Task<ActionResult<AppUserResponse>> UpdatePartial([FromBody] PatchUserDto dto)
    {
        var uid = GetUserId();
        var updated = await _userService.UpdatePartialAsync(uid, dto);
        return Ok(updated);
    }

    /// <summary>Elimina la cuenta del usuario autenticado (Firebase Auth + Firestore).</summary>
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        var uid = GetUserId();
        await _userService.DeleteAsync(uid);
        return NoContent();
    }
}
