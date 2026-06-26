# Infrastructure and Deployment

## Docker Configuration

### Dockerfile
- **Location:** `sourceCode/wildrydes.net/Dockerfile`
- **Base Image:** `mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019`
- **Platform:** Windows containers only (Windows Server Core)
- **Web Server:** IIS (built into base image)
- **Working Directory:** `/inetpub/wwwroot`
- **Build Strategy:** External publish, copy output to container

```dockerfile
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019
ARG source
WORKDIR /inetpub/wwwroot
COPY ${source:-obj/Docker/publish} .
```

### Image Characteristics
- **Size:** ~5-6 GB (Windows Server Core base)
- **OS:** Windows Server Core LTSC 2019
- **Runtime:** .NET Framework 4.8 + ASP.NET + IIS
- **Constraints:** Requires Windows Docker host (Hyper-V or Windows containers mode)

## Database Configuration

### Connection String (Web.config)
```xml
<add name="DefaultConnection" 
     connectionString="Data Source=(LocalDb)\MSSQLLocalDB;
     AttachDbFilename=|DataDirectory|\wildrydes-net-20250729115125.mdf;
     Initial Catalog=wildrydes-net-20250729115125;
     Integrated Security=True" 
     providerName="System.Data.SqlClient" />
```

### Database Characteristics
- **Engine:** SQL Server LocalDB (development instance)
- **Authentication:** Windows Integrated Security
- **Storage:** MDF file in App_Data directory
- **Limitations:** Single-user, not suitable for production, not accessible remotely

## Application Configuration (Web.config)

### Key Settings
| Key | Value | Purpose |
|-----|-------|---------|
| webpages:Version | 3.0.0.0 | Razor version |
| ClientValidationEnabled | true | jQuery validation |
| UnobtrusiveJavaScriptEnabled | true | Unobtrusive JS |
| AWS.Region | us-east-1 | AWS region |
| AWS.CognitoIdentityPoolId | REPLACE_WITH_COGNITO_ID | Cognito pool |
| AWS.LocationService.MapName | REPLACE_WITH_MAP_NAME | Map resource |
| AWS.LocationService.PlaceIndexName | REPLACE_WITH_INDEX_NAME | Place index |

### Compilation Settings
- **Target Framework:** 4.8.1
- **Debug:** true (should be false in production)
- **Compiler:** Roslyn via DotNetCompilerPlatform

## Build Process

The application uses MSBuild (via Visual Studio or `msbuild.exe`):
1. Compile C# source files
2. Process Razor views
3. Bundle JS/CSS assets
4. Publish to output directory
5. Docker build copies published output

### NuGet Dependencies
- All packages are committed in `references/packages/` directory
- No NuGet restore step required (non-standard approach)
- `.csproj` references DLLs from `references/packages/` directly

## Production Deployment Considerations

### Current State (Not Production-Ready)
- LocalDB cannot handle concurrent connections
- Debug compilation enabled
- AWS config uses placeholder values
- No HTTPS enforcement
- No health check endpoint
- No logging infrastructure

### Recommended Production Architecture
- Replace LocalDB with Amazon RDS for SQL Server or Aurora
- Replace placeholder AWS config with environment variables
- Enable HTTPS with certificate
- Add health check endpoint for load balancer
- Configure proper logging (CloudWatch, Application Insights)
- Set compilation debug=false

## Cross-References

- [System Overview](../architecture/system-overview.md)
- [Maintenance Burden](../technical-debt/maintenance-burden.md)
- [Migration - Component Order](../migration/component-order.md)
