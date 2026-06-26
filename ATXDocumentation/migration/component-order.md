# Component Migration Order

## Recommended Migration Sequence

When migrating this application from .NET Framework 4.8.1 to .NET 8+ (ASP.NET Core), the following order respects dependency relationships and minimizes risk.

### Phase 1: Foundation (No Dependencies)

1. **Models** (`Models/UserModel.cs`, `Models/UnicornModel.cs`, `Models/RideModel.cs`)
   - Remove `System.ComponentModel.DataAnnotations.Schema` if using EF Core Fluent API
   - Data annotations are largely compatible
   - `Location` complex type → EF Core owned entity

2. **DbContext** (`Context/DefaultContext.cs`)
   - Change base from `DbContext` (EF6) to `DbContext` (EF Core)
   - Replace `OnModelCreating` convention removal with Fluent API
   - Update connection string configuration

### Phase 2: Infrastructure

3. **Configuration** (Web.config → appsettings.json)
   - Move `appSettings` to `appsettings.json`
   - Move connection strings to `appsettings.json`
   - Remove XML configuration entirely

4. **Startup** (Global.asax.cs → Program.cs)
   - Replace `Application_Start` with `WebApplicationBuilder` setup
   - Register services (DI): DbContext, AWS clients
   - Configure middleware pipeline (auth, routing, static files)

### Phase 3: Cross-Cutting Concerns

5. **Authentication** (`Decorators/Protected.cs` → ASP.NET Core auth middleware)
   - Replace custom filter with `[Authorize]` attribute
   - Implement proper auth handler (Cookie or JWT)
   - Configure authentication services in DI

6. **Routing** (`App_Start/RouteConfig.cs` → attribute routing or endpoint routing)
   - Migrate to ASP.NET Core endpoint routing
   - Update MapTile route constraint syntax

### Phase 4: Controllers (Order by Complexity)

7. **HomeController** - Trivial migration (single action)
8. **UnicornController** - Standard CRUD, replace `DbContext` usage patterns
9. **UserController** - Replace session auth with proper Identity/auth system
10. **RideController** - Most complex: AWS client DI, async patterns, file results

### Phase 5: Views

11. **Layout and shared views** (`_Layout.cshtml`, `Error.cshtml`)
    - Update tag helpers, remove `@Styles.Render`/`@Scripts.Render`
12. **Feature views** (all remaining .cshtml files)
    - Update `@Html.*` helpers to tag helpers where appropriate
    - Update form handling

### Phase 6: Infrastructure

13. **Dockerfile** - Switch from Windows Server Core to Linux-based .NET 8 image
14. **Database** - Recreate EF Core migrations, configure production DB

## Dependency Constraints

```
Models (no deps) ← DbContext ← Controllers ← Views
                                     ↑
Configuration ─────────────────────────┘
Authentication ────────────────────────┘
```

## Cross-References

- [Remediation Plan](../technical-debt/remediation-plan.md)
- [Test Specifications](test-specifications.md)
- [Validation Criteria](validation-criteria.md)
