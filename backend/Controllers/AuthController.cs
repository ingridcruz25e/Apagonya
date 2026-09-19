using Microsoft.AspNetCore.Mvc;
using UserHub.DTOs;
using UserHub.Firebase;
using UserHub.Services;

namespace UserHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>Registra un usuario en Firebase Authentication y crea su perfil en Firestore.</summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
        try
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }
        catch (FirebaseAuthException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Autentica a un usuario existente contra Firebase Authentication.</summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        try
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }
        catch (FirebaseAuthException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
