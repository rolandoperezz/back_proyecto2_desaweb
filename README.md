# 🏀 DesaWebBack - API de Gestión de Equipos de Basquetbol

API REST desarrollada en .NET 8 para la gestión integral de equipos de basquetbol, incluyendo administración de equipos, jugadores, partidos y sistema de anotación en tiempo real.

## 📋 Descripción

DesaWebBack es una API robusta que permite gestionar ligas y equipos de basquetbol. Cuenta con un sistema de autenticación basado en JWT y roles (Administrador/Usuario), permitiendo:

- **Gestión Administrativa**: CRUD completo de equipos, jugadores y partidos
- **Sistema de Partidos**: Creación de partidos, asignación de rosters y actualización de marcadores
- **Autenticación y Autorización**: Sistema seguro con JWT y roles diferenciados
- **Gestión de Usuarios**: Administración de usuarios y permisos

## 🚀 Tecnologías

- **.NET 8.0**
- **Entity Framework Core** (SQL Server)
- **ASP.NET Core Web API**
- **JWT Authentication**
- **Swagger/OpenAPI**

## 📦 Prerequisitos

Antes de comenzar, asegúrate de tener instalado:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server) o SQL Server Express
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/) (opcional)

## ⚙️ Configuración

### 1. Clonar el repositorio

```bash
git clone https://github.com/tu-usuario/desawebback.git
cd desawebback
```

### 2. Configurar la base de datos

Edita el archivo `appsettings.json` con tu configuración de SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=Desaweb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Nota**: Ajusta el `Server` según tu configuración local.

### 3. Configurar CORS

En `appsettings.json`, configura los orígenes permitidos:

```json
{
  "AllowedOrigins": [
    "http://localhost:4200",
    "**"
  ]
}
```

### 4. Configuración JWT

Las claves JWT ya están configuradas en `appsettings.json`. Para producción, **cambia estos valores**:

```json
{
  "Jwt": {
    "Key": "thisisatwentysixbytekeyforaes256",
    "Issuer": "HelloApi",
    "Audience": "HelloApiUsers",
    "ExpiresInMinutes": 60,
    "Iv": "thisis16bytesiv!"
  }
}
```

⚠️ **IMPORTANTE**: Nunca subas estas claves a repositorios públicos. Usa variables de entorno en producción.

### 5. Aplicar migraciones

La aplicación aplica las migraciones automáticamente al iniciar, pero también puedes hacerlo manualmente:

```bash
dotnet ef database update
```

## 🏃‍♂️ Ejecución

### Modo Desarrollo

```bash
dotnet run
```

La API estará disponible en:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `https://localhost:5001/swagger`

### Modo Producción

```bash
dotnet run --configuration Release
```

## 📚 Documentación de la API

Una vez iniciada la aplicación, accede a Swagger UI para ver la documentación interactiva:

```
https://localhost:5001/swagger
```

## 🔑 Autenticación

### Registro de Usuario

```http
POST /api/Auth/register
Content-Type: application/json

{
  "username": "usuario",
  "password": "password123",
  "roleName": "Usuario"
}
```

### Login

```http
POST /api/Auth/login
Content-Type: application/json

{
  "username": "usuario",
  "password": "password123"
}
```

**Respuesta**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "usuario",
  "role": "Usuario"
}
```

### Uso del Token

Incluye el token en el header `Authorization` de tus peticiones:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 📍 Endpoints Principales

### Autenticación (`/api/Auth`)
- `POST /register` - Registrar nuevo usuario
- `POST /login` - Iniciar sesión
- `GET /users` - Listar usuarios
- `PUT /users/{id}` - Actualizar usuario
- `DELETE /users/{id}` - Eliminar usuario
- `GET /roles` - Listar roles
- `POST /roles` - Crear rol

### Administración - Equipos (`/api/Admin`) 🔒 Requiere rol "Administrador"
- `POST /equipos` - Crear equipo
- `GET /equipos` - Listar equipos (soporta filtros)
- `GET /equipos/{id}` - Obtener equipo por ID
- `PUT /equipos/{id}` - Actualizar equipo
- `DELETE /equipos/{id}` - Eliminar equipo

### Administración - Jugadores (`/api/Admin`) 🔒 Requiere rol "Administrador"
- `POST /jugadores` - Crear jugador
- `GET /equipos/{equipoId}/jugadores` - Listar jugadores de un equipo
- `GET /jugadores/{id}` - Obtener jugador por ID
- `PUT /jugadores/{id}` - Actualizar jugador
- `DELETE /jugadores/{id}` - Eliminar jugador

### Administración - Partidos (`/api/Admin`) 🔒 Requiere rol "Administrador"
- `POST /partidos` - Crear partido
- `GET /partidos` - Listar partidos (soporta filtros por fecha)
- `GET /partidos/{id}` - Obtener partido por ID
- `PUT /partidos/{id}` - Actualizar partido
- `DELETE /partidos/{id}` - Eliminar partido
- `POST /partidos/{partidoId}/roster/equipo/{equipoId}` - Asignar roster
- `PUT /partidos/{id}/marcador` - Actualizar marcador

### Partidos (`/api/Partidos`) 🔒 Requiere autenticación
- `GET /` - Listar partidos
- `GET /{id}` - Obtener partido por ID
- `PUT /{id}/marcador` - Actualizar marcador del partido

## 🛡️ Roles y Permisos

| Rol | Permisos |
|-----|----------|
| **Administrador** | Acceso completo a todos los endpoints de administración (equipos, jugadores, partidos) |
| **Usuario** | Consulta de partidos y actualización de marcadores |

## 📁 Estructura del Proyecto

```
back_proyecto2_desaweb/
├── bin/                        # Archivos compilados
├── obj/                        # Archivos temporales de compilación
├── Controllers/                # Controladores de la API
│   ├── AdminController.cs      # Endpoints administrativos
│   ├── AuthController.cs       # Autenticación y usuarios
│   └── PartidosController.cs   # Gestión de partidos
├── Data/                       # Contexto de base de datos
│   └── AppDbContext.cs         # DbContext de Entity Framework
├── DTOs/                       # Data Transfer Objects
│   └── [Request/Response DTOs]
├── Migrations/                 # Migraciones de Entity Framework
│   └── [Archivos de migración]
├── Models/                     # Entidades/Modelos de dominio
│   ├── User.cs
│   ├── Role.cs
│   ├── Equipo.cs
│   ├── Jugador.cs
│   └── Partido.cs
├── Properties/                 # Configuración del proyecto
│   └── launchSettings.json
├── Repositories/               # Capa de acceso a datos
│   ├── Interfaces/
│   └── [Implementaciones]
├── Services/                   # Lógica de negocio
│   ├── Interfaces/
│   │   ├── IAuthService.cs
│   │   ├── IAdminService.cs
│   │   └── IPartidoService.cs
│   ├── AuthService.cs
│   ├── AdminService.cs
│   └── PartidoService.cs
├── Utils/                      # Utilidades
│   └── CryptoHelper.cs         # Encriptación JWT
├── .gitignore                  # Archivos ignorados por Git
├── appsettings.json            # Configuración de la aplicación
├── appsettings.Development.json # Configuración de desarrollo
├── Dockerfile                  # Configuración Docker
├── HelloApi.csproj             # Archivo del proyecto .NET
├── HelloApi.sln                # Solución de Visual Studio
└── Program.cs                  # Punto de entrada de la aplicación
```

## 🔧 Scripts Disponibles

```bash
# Restaurar dependencias
dotnet restore

# Compilar el proyecto
dotnet build

# Ejecutar la aplicación
dotnet run

# Ejecutar tests (si existen)
dotnet test

# Crear una migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones
dotnet ef database update

# Publicar para producción
dotnet publish -c Release
```

## 🌐 Variables de Entorno (Producción)

Para producción, configura estas variables de entorno en lugar de usar `appsettings.json`:

```bash
ConnectionStrings__DefaultConnection="Server=...;Database=...;"
Jwt__Key="tu-clave-secreta-segura"
Jwt__Issuer="TuApp"
Jwt__Audience="TuAppUsers"
Jwt__ExpiresInMinutes="60"
Jwt__Iv="tu-iv-seguro"
```

## 🐛 Troubleshooting

### Error de conexión a la base de datos
- Verifica que SQL Server esté corriendo
- Confirma que la cadena de conexión sea correcta
- Asegúrate de tener permisos en la base de datos

### Error 401 Unauthorized
- Verifica que el token JWT sea válido
- Confirma que el token no haya expirado
- Asegúrate de incluir el header `Authorization: Bearer {token}`

### Error 403 Forbidden
- Verifica que tu usuario tenga el rol correcto para el endpoint
- Los endpoints de `/api/Admin` requieren rol "Administrador"

## 🤝 Contribución

Las contribuciones son bienvenidas. Por favor:

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📝 Licencia

Este proyecto está bajo la Licencia MIT.

## 👥 Autor

Tu Nombre - [@rolandoperezz](https://github.com/rolandoperezz)

## 📧 Contacto

Para preguntas o sugerencias, contacta a: perezqrolando@gmail.com

---

⭐ Si este proyecto te fue útil, considera darle una estrella en GitHub!
