using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UserHub.Firebase;
using UserHub.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------- Configuración de Firebase ----------
builder.Services.Configure<FirebaseOptions>(builder.Configuration.GetSection("Firebase"));
var firebaseOptions = builder.Configuration.GetSection("Firebase").Get<FirebaseOptions>()
    ?? throw new InvalidOperationException("Falta la sección 'Firebase' en la configuración.");

if (string.IsNullOrWhiteSpace(firebaseOptions.ProjectId))
    throw new InvalidOperationException("Firebase:ProjectId no está configurado.");

// ---------- Credenciales de la cuenta de servicio (Admin SDK + Firestore) ----------
var credentialsPath = Path.IsPathRooted(firebaseOptions.CredentialsPath)
    ? firebaseOptions.CredentialsPath
    : Path.Combine(builder.Environment.ContentRootPath, firebaseOptions.CredentialsPath);

if (!File.Exists(credentialsPath))
{
    throw new FileNotFoundException(
        $"No se encontró el archivo de credenciales de Firebase en '{credentialsPath}'. " +
        "Descárgalo desde Firebase Console > Configuración del proyecto > Cuentas de servicio, " +
        "y NO lo subas al repositorio (agrégalo a .gitignore).");
}

GoogleCredential googleCredential = GoogleCredential.FromFile(credentialsPath);

// Firebase Admin SDK (se usa para eliminar usuarios de Firebase Authentication)
if (FirebaseApp.DefaultInstance is null)
{
    FirebaseApp.Create(new AppOptions
    {
        Credential = googleCredential,
        ProjectId = firebaseOptions.ProjectId
    });
}

// Firestore
var firestoreDb = new FirestoreDbBuilder
{
    ProjectId = firebaseOptions.ProjectId,
    Credential = googleCredential
}.Build();

builder.Services.AddSingleton(firestoreDb);

// ---------- HttpClient para la API REST de Firebase (register/login) ----------
builder.Services.AddHttpClient<FirebaseAuthClient>();

// ---------- Inyección de dependencias ----------
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

// ---------- Autenticación JWT: valida los ID tokens emitidos por Firebase Authentication ----------
var issuer = $"https://securetoken.google.com/{firebaseOptions.ProjectId}";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = issuer;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = firebaseOptions.ProjectId,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

// ---------- MVC / Swagger ----------
builder.Services.AddCors(options => options.AddPolicy("frontend", policy => policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Ingrese el idToken de Firebase con el prefijo 'Bearer '. Ej: Bearer eyJhbGciOi...",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
