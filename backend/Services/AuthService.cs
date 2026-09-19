using UserHub.DTOs;
using UserHub.Firebase;

namespace UserHub.Services;

public class AuthService : IAuthService
{
    private readonly FirebaseAuthClient _firebaseAuth;
    private readonly IUserService _userService;

    public AuthService(FirebaseAuthClient firebaseAuth, IUserService userService)
    {
        _firebaseAuth = firebaseAuth;
        _userService = userService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        // 1. Crear el usuario en Firebase Authentication.
        var signUp = await _firebaseAuth.SignUpAsync(dto.Email, dto.Password);

        // 2. Crear el perfil en Firestore con Role = "Usuario" por defecto.
        //    Toda esta lógica de negocio vive aquí, no en el controller.
        await _userService.CreateProfileAsync(signUp.LocalId, signUp.Email, dto);

        return new AuthResponseDto
        {
            IdToken = signUp.IdToken,
            LocalId = signUp.LocalId,
            Email = signUp.Email
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var signIn = await _firebaseAuth.SignInAsync(dto.Email, dto.Password);

        return new AuthResponseDto
        {
            IdToken = signIn.IdToken,
            LocalId = signIn.LocalId,
            Email = signIn.Email
        };
    }
}
