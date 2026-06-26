# Dependency Analysis

## Internal Dependencies

### Controller → Model Dependencies
```
HomeController → (none)
UserController → UserModel, DefaultContext
RideController → RideModel, UnicornModel, UserModel, DefaultContext, Location
UnicornController → UnicornModel, DefaultContext
```

### Controller → Service Dependencies
```
RideController → AmazonLocationServiceClient, CognitoAWSCredentials, ConfigurationManager
UserController → SHA256 (System.Security.Cryptography)
```

### Model → Model Dependencies
```
RideModel → UnicornModel (FK navigation)
RideModel → UserModel (FK navigation)
RideModel → Location (complex type)
RideModel → Rating (enum)
```

### Configuration → Component Dependencies
```
Global.asax.cs → FilterConfig, RouteConfig, BundleConfig
FilterConfig → HandleErrorAttribute
RouteConfig → (framework only)
BundleConfig → (framework only)
```

## External Dependency Graph

```
Application
├── .NET Framework 4.8.1 (CRITICAL - runtime)
│   ├── System.Web.Mvc 5.3.0
│   ├── System.Web.Optimization 1.1.3
│   ├── System.Web.Routing
│   └── System.Security.Cryptography
│
├── Entity Framework 6.5.1 (CRITICAL - data access)
│   └── System.Data.SqlClient (SQL Server LocalDB)
│
├── AWS SDK for .NET v4
│   ├── AWSSDK.Core 4.0.0.17
│   │   ├── System.Text.Json 8.0.5
│   │   ├── System.Text.Encodings.Web 8.0.0
│   │   ├── Microsoft.Bcl.AsyncInterfaces 8.0.0
│   │   ├── System.Runtime.CompilerServices.Unsafe 6.0.0
│   │   ├── System.Buffers 4.5.1
│   │   ├── System.Memory 4.5.5
│   │   ├── System.Numerics.Vectors 4.5.0
│   │   ├── System.Threading.Tasks.Extensions 4.5.4
│   │   └── System.ValueTuple 4.5.0
│   ├── AWSSDK.CognitoIdentity 4.0.0.15
│   ├── AWSSDK.LocationService 4.0.1
│   └── AWSSDK.SecurityToken 4.0.1.6
│
├── Frontend (bundled)
│   ├── Bootstrap 5.3.7
│   ├── jQuery 3.7.1
│   ├── jQuery.Validation 1.21.0
│   └── Modernizr 2.8.3
│
├── Frontend (CDN)
│   └── Leaflet.js 1.7.1
│
├── Build/Tooling
│   ├── WebGrease 1.6.0
│   │   └── Antlr 3.5.0.2
│   ├── Microsoft.CodeDom.Providers.DotNetCompilerPlatform 4.1.0
│   └── Microsoft.Web.Infrastructure 2.0.0
│
└── Serialization
    └── Newtonsoft.Json 13.0.3
```

## Dependency Risk Assessment

| Dependency | Risk | Reason |
|-----------|------|--------|
| .NET Framework 4.8.1 | High | Platform lock-in, maintenance-only |
| ASP.NET MVC 5 | High | No further development |
| Entity Framework 6 | Medium | Maintenance-only, EF Core is successor |
| SQL Server LocalDB | Medium | Dev-only, not production-ready |
| Windows Server Core | Medium | Large images, limited platforms |
| Modernizr | Low | Unnecessary, no functional impact |
| WebGrease/Antlr | Low | Build-time only, obsolete |

## Cross-References

- [Dependencies](../architecture/dependencies.md)
- [Outdated Components](../technical-debt/outdated-components.md)
- [Code Metrics](code-metrics.md)
