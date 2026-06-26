# Dependencies

## External Package Dependencies

All packages are managed via NuGet (`packages.config`), targeting .NET Framework 4.8.1:

| Package | Version | Category | Purpose |
|---------|---------|----------|---------|
| AWSSDK.Core | 4.0.0.17 | AWS | Base AWS SDK functionality |
| AWSSDK.CognitoIdentity | 4.0.0.15 | AWS | Cognito Identity Pool credentials |
| AWSSDK.LocationService | 4.0.1 | AWS | Map tiles and geocoding |
| AWSSDK.SecurityToken | 4.0.1.6 | AWS | STS token support |
| EntityFramework | 6.5.1 | Data | ORM for SQL Server |
| Microsoft.AspNet.Mvc | 5.3.0 | Web | MVC framework |
| Microsoft.AspNet.Razor | 3.3.0 | Web | View engine |
| Microsoft.AspNet.WebPages | 3.3.0 | Web | Razor infrastructure |
| Microsoft.AspNet.Web.Optimization | 1.1.3 | Web | Bundling/minification |
| Newtonsoft.Json | 13.0.3 | Serialization | JSON handling |
| bootstrap | 5.3.7 | Frontend | CSS framework |
| jQuery | 3.7.1 | Frontend | JavaScript library |
| jQuery.Validation | 1.21.0 | Frontend | Client-side validation |
| Modernizr | 2.8.3 | Frontend | Browser feature detection |
| WebGrease | 1.6.0 | Build | CSS/JS optimization |
| Antlr | 3.5.0.2 | Build | Parser (WebGrease dependency) |
| Microsoft.Bcl.AsyncInterfaces | 8.0.0 | Runtime | Async support polyfill |
| Microsoft.CodeDom.Providers.DotNetCompilerPlatform | 4.1.0 | Build | Roslyn compiler |
| Microsoft.Web.Infrastructure | 2.0.0 | Web | ASP.NET infrastructure |
| System.Buffers | 4.5.1 | Runtime | Buffer pooling polyfill |
| System.Memory | 4.5.5 | Runtime | Span/Memory polyfill |
| System.Numerics.Vectors | 4.5.0 | Runtime | SIMD polyfill |
| System.Runtime.CompilerServices.Unsafe | 6.0.0 | Runtime | Low-level utilities |
| System.Text.Encodings.Web | 8.0.0 | Runtime | HTML/URL encoding |
| System.Text.Json | 8.0.5 | Runtime | JSON (AWS SDK dependency) |
| System.Threading.Tasks.Extensions | 4.5.4 | Runtime | ValueTask polyfill |
| System.ValueTuple | 4.5.0 | Runtime | ValueTuple polyfill |

## CDN Dependencies (loaded at runtime)

| Library | Version | Purpose |
|---------|---------|---------|
| Leaflet.js | 1.7.1 | Interactive map rendering |
| Leaflet CSS | 1.7.1 | Map styling |

## Internal Component Dependencies

```
HomeController ──> (none)

UserController ──> DefaultContext ──> UserModel
                └──> SHA256 (System.Security.Cryptography)

RideController ──> DefaultContext ──> RideModel, UnicornModel, UserModel
               ├──> AmazonLocationServiceClient
               ├──> CognitoAWSCredentials
               └──> ConfigurationManager (Web.config)

UnicornController ──> DefaultContext ──> UnicornModel

Protected (Filter) ──> Session state
                   └──> HttpContext

DefaultContext ──> UserModel, UnicornModel, RideModel
               └──> Entity Framework DbContext

Configuration (Migration) ──> DefaultContext
                          └──> UserModel, UnicornModel
```

## External Service Dependencies

| Service | Provider | Purpose | Configuration |
|---------|----------|---------|---------------|
| SQL Server LocalDB | Microsoft | Data persistence | Web.config connectionStrings |
| Cognito Identity Pool | AWS | Temporary credentials | Web.config appSettings |
| Location Service (Maps) | AWS | Map tile imagery | Web.config appSettings |
| Location Service (Places) | AWS | Reverse geocoding | Web.config appSettings |

## Dependency Criticality

- **Critical:** .NET Framework 4.8.1 (entire runtime), Entity Framework 6 (all data access), ASP.NET MVC 5 (all routing/views)
- **High:** AWS SDK (map functionality), SQL Server (data persistence)
- **Medium:** Bootstrap (UI), jQuery (client interactivity), Leaflet.js (map rendering)
- **Low:** Modernizr, WebGrease, Antlr (build-time only)

## Cross-References

- [Outdated Components](../technical-debt/outdated-components.md)
- [Dependency Analysis](../analysis/dependency-analysis.md)
