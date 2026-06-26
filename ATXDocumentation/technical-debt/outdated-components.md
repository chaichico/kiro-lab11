# Outdated Components

## Runtime and Framework (Severity: High)

### .NET Framework 4.8.1
- **Status:** Maintenance-only (no new features, security patches only)
- **Current:** 4.8.1
- **Recommended:** .NET 8 or .NET 9 (cross-platform, actively developed)
- **Impact:** Windows-only lock-in, no access to modern C# features (records, pattern matching improvements, global usings), no cross-platform deployment, limited cloud-native support
- **Migration Path:** Port to .NET 8+ using .NET Upgrade Assistant or manual migration

### ASP.NET MVC 5.3.0
- **Status:** Legacy, no further development
- **Current:** 5.3.0
- **Recommended:** ASP.NET Core MVC (part of .NET 8+)
- **Impact:** No middleware pipeline, no built-in DI, no minimal APIs, no cross-platform web server (Kestrel)
- **Migration Path:** Migrate controllers to ASP.NET Core MVC, replace System.Web dependencies

### Entity Framework 6.5.1
- **Status:** Maintenance-only
- **Current:** 6.5.1
- **Recommended:** Entity Framework Core 8+ 
- **Impact:** Missing performance features (compiled queries by default, split queries), no cross-platform, limited LINQ translation improvements
- **Migration Path:** Migrate DbContext and models to EF Core, update LINQ queries for compatibility

## Dependencies (Severity: Medium)

### Modernizr 2.8.3
- **Status:** Largely obsolete for modern browser targets
- **Current:** 2.8.3 (released 2014)
- **Recommended:** Remove entirely; modern browsers support HTML5/CSS3 features natively
- **Impact:** Unnecessary JavaScript payload, no functional value for Bootstrap 5 which already handles graceful degradation

### WebGrease 1.6.0
- **Status:** Obsolete, unmaintained
- **Current:** 1.6.0
- **Recommended:** Remove; use modern bundling (webpack, esbuild, or ASP.NET Core bundling)
- **Impact:** Build-time dependency only, but represents unmaintained code

### Antlr 3.5.0.2
- **Status:** Very old (Antlr 4 is current)
- **Current:** 3.5.0.2
- **Recommended:** Remove (only needed as WebGrease dependency)
- **Impact:** Transitive dependency, removed when WebGrease is removed

### Leaflet.js 1.7.1
- **Status:** Outdated (current is 1.9.x)
- **Current:** 1.7.1
- **Recommended:** Update to Leaflet 1.9.x
- **Impact:** Missing bug fixes and performance improvements

## Infrastructure (Severity: Medium)

### Windows Server Core LTSC 2019 Docker Image
- **Status:** Aging base image
- **Current:** `mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019`
- **Recommended:** Linux-based .NET 8+ image (`mcr.microsoft.com/dotnet/aspnet:8.0`)
- **Impact:** ~5GB image size (vs ~200MB for Linux), limited orchestration platform support, Windows licensing costs

### SQL Server LocalDB
- **Status:** Development-only database
- **Current:** LocalDB
- **Recommended:** Amazon RDS for SQL Server, Amazon Aurora PostgreSQL, or Amazon DynamoDB
- **Impact:** Not suitable for production, no HA/DR capabilities

## AWS SDK (Current - No Action Required)

| Package | Version | Status |
|---------|---------|--------|
| AWSSDK.Core | 4.0.0.17 | Current (AWS SDK for .NET v4) |
| AWSSDK.CognitoIdentity | 4.0.0.15 | Current |
| AWSSDK.LocationService | 4.0.1 | Current |
| AWSSDK.SecurityToken | 4.0.1.6 | Current |

## Frontend Libraries (Current - No Action Required)

| Package | Version | Status |
|---------|---------|--------|
| Bootstrap | 5.3.7 | Current |
| jQuery | 3.7.1 | Current |
| jQuery.Validation | 1.21.0 | Current |
| Newtonsoft.Json | 13.0.3 | Current |

## Cross-References

- [Summary](summary.md)
- [Remediation Plan](remediation-plan.md)
- [Dependencies](../architecture/dependencies.md)
