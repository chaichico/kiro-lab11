# System Overview

## Architecture Style

Traditional server-rendered **Model-View-Controller (MVC)** web application using ASP.NET MVC 5. The application follows a monolithic architecture with all components deployed as a single unit within an IIS web server running in a Windows Docker container.

## Technology Stack

### Backend
- **.NET Framework 4.8.1** - Runtime platform
- **ASP.NET MVC 5.3.0** - Web framework (Razor views, controllers, routing)
- **Entity Framework 6.5.1** - ORM with Code-First approach
- **SQL Server LocalDB** - Relational database

### Frontend
- **Razor** (.cshtml) - Server-side view templates
- **Bootstrap 5.3.7** - CSS framework
- **jQuery 3.7.1** - DOM manipulation and AJAX
- **Leaflet.js 1.7.1** - Interactive map rendering (loaded from CDN)

### AWS Integration
- **AWSSDK.Core 4.0.0.17** - Base AWS SDK
- **AWSSDK.CognitoIdentity 4.0.0.15** - Credential provider
- **AWSSDK.LocationService 4.0.1** - Map tiles + geocoding
- **AWSSDK.SecurityToken 4.0.1.6** - STS support

### Deployment
- **Docker** with `mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019`
- **IIS** (Internet Information Services) as the web server
- Published output copied to `/inetpub/wwwroot`

## Deployment Model

```
[Browser] --> [IIS in Windows Docker Container]
                    |
                    ├── ASP.NET MVC 5 Application
                    │       ├── Controllers (4)
                    │       ├── Models (3)
                    │       ├── Views (12 Razor templates)
                    │       └── Entity Framework DbContext
                    |
                    ├── [SQL Server LocalDB] (in-container)
                    |
                    └── [AWS Services] (external)
                            ├── Cognito Identity Pool
                            └── Location Service (Maps + Geocoding)
```

## Key Architectural Decisions

1. **Custom authentication over ASP.NET Identity** - Simple session-based auth with SHA-256 hashing
2. **Server-side map tile proxy** - Map tiles proxied through backend to avoid CORS/credential exposure
3. **Code-First database** - Schema defined in C# models with EF migrations
4. **No API layer** - Mixed server-rendered views and JSON endpoints on same controllers
5. **Windows-only deployment** - Depends on full .NET Framework (not .NET Core/5+)

## Cross-References

- [Components](components.md) - Detailed component breakdown
- [Dependencies](dependencies.md) - Full dependency graph
- [Patterns](patterns.md) - Design patterns used
- [Technical Debt Report](../technical-debt-report.md) - Modernization recommendations
