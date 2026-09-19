# UserHub

API REST en ASP.NET Core (.NET 10) conectada a Firebase Authentication y Firestore.
Permite registrar, actualizar y eliminar usuarios autenticados, con manejo de roles.

## Arquitectura

```
UserHub/
├── Controllers/       # AuthController, UserController — solo reciben/responden HTTP
├── Services/           # AuthService, UserService — toda la lógica de negocio
├── Models/              # AppUser (documento de Firestore)
├── DTOs/                  # RegisterDto, LoginDto, AuthResponseDto, UpdateUserDto, PatchUserDto
├── Firebase/            # FirebaseAuthClient (REST API), FirebaseOptions
└── Program.cs         # DI, JWT auth, Firestore, Admin SDK, Swagger
```

- **Auth (register/login)**: se implementó llamando directamente a la API REST de
  Firebase Identity Toolkit (`accounts:signUp` / `accounts:signInWithPassword`),
  porque es la única forma de obtener un `idToken` verificado con email/password.
- **Validación de tokens en endpoints protegidos**: JWT Bearer estándar de ASP.NET Core,
  configurado con `Authority = https://securetoken.google.com/{ProjectId}`. El uid del
  usuario se extrae del claim `user_id` (o `sub`) del token — nunca del body.
- **Eliminación de cuentas**: se usa el **Firebase Admin SDK** (`FirebaseAdmin` NuGet),
  que sí puede borrar un usuario por uid usando la cuenta de servicio.
- **Firestore**: acceso vía `Google.Cloud.Firestore`, usando las mismas credenciales
  de la cuenta de servicio.

## Requisitos previos

1. .NET 10 SDK instalado.
2. Un proyecto de Firebase con:
   - **Authentication** habilitado, con el proveedor **Email/Password** activado.
   - **Firestore** creado (modo nativo).
3. La **Web API Key** de Firebase (Configuración del proyecto → General → tus apps).
4. Una **cuenta de servicio**: Configuración del proyecto → Cuentas de servicio →
   "Generar nueva clave privada". Descarga el JSON.

## Configuración

1. Copia el JSON de la cuenta de servicio a la raíz del proyecto y renómbralo:
   ```
   firebase-credentials.json
   ```
   Este archivo **ya está en `.gitignore`** — no se sube al repositorio.

2. Edita `appsettings.json` (o mejor, usa `dotnet user-secrets` en desarrollo):
   ```json
   {
     "Firebase": {
       "ApiKey": "TU_WEB_API_KEY",
       "ProjectId": "tu-proyecto-firebase",
       "CredentialsPath": "firebase-credentials.json"
     }
   }
   ```

   Alternativa recomendada (para no tocar appsettings.json con datos sensibles):
   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "Firebase:ApiKey" "TU_WEB_API_KEY"
   dotnet user-secrets set "Firebase:ProjectId" "tu-proyecto-firebase"
   ```

## Ejecutar

```bash
cd UserHub
dotnet restore
dotnet build
dotnet run
```

Swagger quedará disponible en `https://localhost:7080/swagger` (o el puerto que
asigne `dotnet run`). Desde ahí puedes probar `register`, `login`, y luego pegar el
`idToken` recibido en el botón **Authorize** (`Bearer <idToken>`) para probar
PUT/PATCH/DELETE de `/api/User`.

## Endpoints

| Método | Ruta                 | Auth | Descripción                                   |
|--------|----------------------|------|------------------------------------------------|
| POST   | `/api/Auth/register` | No   | Crea usuario en Firebase Auth + perfil en Firestore |
| POST   | `/api/Auth/login`    | No   | Autentica y devuelve idToken                   |
| PUT    | `/api/User`           | Sí   | Reemplaza el perfil completo                   |
| PATCH  | `/api/User`           | Sí   | Actualiza campos parciales del perfil          |
| DELETE | `/api/User`           | Sí   | Elimina la cuenta (Auth + Firestore)           |

En todos los endpoints de `/api/User`, el uid se toma del token JWT — nunca se acepta
desde el cliente — y el campo `Role` nunca puede modificarse desde estas rutas.

## Trabajo en equipo (ramas)

Cada integrante debe:
1. Crear su propia rama con nombre identificable, ej: `feature/nombre-parte`.
2. Hacer un aporte real y funcional (una parte del código: un endpoint, un servicio, DTOs, etc.).
3. Abrir un Pull Request y hacer merge a `main`.

No se acepta presentar la rama de otro compañero como aporte propio.

## Nota sobre esta entrega

Este proyecto fue generado en un entorno sin acceso a NuGet (nuget.org), por lo que
**no pudo compilarse ni ejecutarse aquí**. Antes de subirlo, ejecuta localmente:
```bash
dotnet restore
dotnet build
```
y corrige cualquier detalle de versión de paquete si tu SDK de .NET 10 instalado
difiere de las versiones fijadas en `UserHub.csproj`.
