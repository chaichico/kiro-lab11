# Modules

## Project Organization

The application is a single-project ASP.NET MVC solution. There is no multi-project structure or modular architecture - all code resides in the `wildrydes.net` project.

## Logical Modules

### 1. Authentication Module
- **Namespace:** `wildrydes.net.Controllers` (UserController), `wildrydes.net.Decorators`
- **Files:** `UserController.cs`, `Protected.cs`
- **Responsibility:** User registration, login, logout, session management
- **Dependencies:** DefaultContext, SHA256

### 2. Ride Management Module
- **Namespace:** `wildrydes.net.Controllers` (RideController)
- **Files:** `RideController.cs`, `RideModel.cs`
- **Responsibility:** Ride creation, history, map integration, geocoding
- **Dependencies:** DefaultContext, AWS SDK (Location, Cognito)

### 3. Fleet Management Module
- **Namespace:** `wildrydes.net.Controllers` (UnicornController)
- **Files:** `UnicornController.cs`, `UnicornModel.cs`
- **Responsibility:** Unicorn CRUD operations
- **Dependencies:** DefaultContext

### 4. Data Access Module
- **Namespace:** `wildrydes.net.Context`, `wildrydes.net.Models`
- **Files:** `DefaultContext.cs`, `UserModel.cs`, `UnicornModel.cs`, `RideModel.cs`
- **Responsibility:** Database schema, entity definitions, data access
- **Dependencies:** Entity Framework 6

### 5. Configuration Module
- **Namespace:** `wildrydes.net` (App_Start)
- **Files:** `BundleConfig.cs`, `FilterConfig.cs`, `RouteConfig.cs`, `Global.asax.cs`
- **Responsibility:** Application startup, routing, bundling, global filters

### 6. Presentation Module
- **Location:** `Views/` directory
- **Files:** 12 Razor templates + `_ViewStart.cshtml`
- **Responsibility:** HTML rendering with Razor syntax
- **Dependencies:** Models (strongly-typed views), jQuery, Bootstrap, Leaflet.js

## Module Dependencies Graph

```
Configuration Module
    │
    ├──> Authentication Module ──> Data Access Module
    │
    ├──> Ride Management Module ──> Data Access Module
    │                           └──> AWS SDK
    │
    ├──> Fleet Management Module ──> Data Access Module
    │
    └──> Presentation Module (Views)
              │
              ├──> Authentication Module (forms)
              ├──> Ride Management Module (map, history)
              └──> Fleet Management Module (CRUD views)
```

## External Module Dependencies

| Internal Module | External Dependencies |
|-----------------|----------------------|
| Data Access | Entity Framework 6, System.Data.SqlClient |
| Ride Management | AWSSDK.LocationService, AWSSDK.CognitoIdentity |
| Authentication | System.Security.Cryptography (SHA256) |
| Presentation | Bootstrap, jQuery, Leaflet.js, jQuery.Validation |
| Configuration | System.Web.Mvc, System.Web.Optimization, System.Web.Routing |

## Cross-References

- [Program Structure](program-structure.md)
- [Components](../architecture/components.md)
- [Dependencies](../architecture/dependencies.md)
