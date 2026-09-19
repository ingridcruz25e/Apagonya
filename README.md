# ApagónYa — entrega base

Proyecto construido tomando como base el frontend entregado por el ingeniero: Angular standalone, `provideRouter`, `provideHttpClient`, Angular Material, servicios con `HttpClient`, `authGuard` y `localStorage` para el token. Se conservaron las páginas y patrones del proyecto original y se agregaron las funciones de ApagónYa.

## Estructura
- `frontend/` Angular 22 + Angular Material + Firebase Web SDK + Chart.js
- `backend/` ASP.NET Core .NET 10 + Firebase Admin + Firestore

## 1. Firebase
En Firebase Console:
1. Crea/abre tu proyecto.
2. Authentication -> Sign-in method -> activa Email/Password.
3. Firestore Database -> crea la base.
4. Storage -> habilítalo.
5. Project settings -> Service accounts -> Generate new private key.
6. Guarda ese JSON como `backend/firebase-credentials.json`. **No lo subas a GitHub.**
7. Project settings -> General -> Your apps -> agrega una app Web y copia la configuración en:
   `frontend/src/app/config/firebase.config.ts`.
8. En `backend/appsettings.json`, coloca `ProjectId` y `ApiKey`.

## 2. Backend
Desde `backend/`:
```bash
dotnet restore
dotnet run
```
Queda en `http://localhost:5080` según `Properties/launchSettings.json`.

Primero crea las zonas demo:
```bash
curl -X POST http://localhost:5080/api/Setup/zones
```
También puedes abrir Swagger en el perfil de desarrollo.

## 3. Frontend
Desde `frontend/`:
```bash
npm install
npm start
```
Abre `http://localhost:4200`.

## 4. Roles para la demostración
El registro crea ciudadanos como `ciudadano`.
Para probar técnico/admin, después de crear las cuentas cambia manualmente el campo `Role` del documento correspondiente en Firestore a `tecnico` o `administrador` y vuelve a iniciar sesión.

## 5. Escenarios de la guía
1. Ciudadano registra cuenta y crea reporte con foto -> estado `nuevo`.
2. Otro ciudadano intenta reportar en la misma zona -> el backend devuelve `409` por duplicado.
3. Otro ciudadano pulsa `A mí también` -> al llegar a 2 confirmaciones pasa a `confirmado`.
4. Usuario técnico entra a `/tecnico` y registra causa/hora -> `resuelto`.
5. Administrador entra a `/admin` y visualiza conteos y reportes.

## 6. GitHub
Desde la carpeta raíz:
```bash
git init
git add .
git commit -m "ApagonYa - entrega"
git branch -M main
git remote add origin https://github.com/TU-USUARIO/apagonya.git
git push -u origin main
```
Reemplaza la URL por tu repositorio.

**Nunca subas `backend/firebase-credentials.json`.**
