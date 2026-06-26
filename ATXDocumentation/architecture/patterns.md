# Architectural Patterns

## Primary Pattern: Model-View-Controller (MVC)

The application follows the classic MVC pattern as implemented by ASP.NET MVC 5:

- **Models** (`Models/`): Entity classes with data annotations for validation and database mapping
- **Views** (`Views/`): Razor templates (.cshtml) organized by controller name
- **Controllers** (`Controllers/`): Handle HTTP requests, coordinate models and views

## Design Patterns Identified

### 1. Repository Pattern (Implicit via DbContext)
- `DefaultContext` acts as a unit-of-work/repository through EF6 `DbSet<T>` properties
- Controllers directly consume DbContext (no abstraction layer)

### 2. Action Filter Pattern
- `Protected` attribute implements `ActionFilterAttribute` for cross-cutting authentication
- Applied at method level (`[Protected]` on individual actions) or class level (UnicornController)

### 3. Complex Type (Value Object)
- `Location` class is used as an EF6 complex type embedded in `RideModel`
- Stores Latitude, Longitude, Address as columns prefixed with property name

### 4. Seed Data Pattern
- `Configuration.cs` uses EF6 `DbMigrationsConfiguration<T>.Seed()` for initial data
- Uses `AddOrUpdate` for idempotent seeding

### 5. Proxy Pattern
- `GetMapTile` action proxies requests to AWS Location Service
- Hides AWS credentials from client, adds caching headers

### 6. Front Controller
- ASP.NET MVC routing (`RouteConfig.cs`) acts as front controller
- All requests flow through `{controller}/{action}/{id}` convention

## Anti-Patterns Identified

### 1. Service Locator / Tight Coupling
- Controllers instantiate `DefaultContext` directly (`new DefaultContext()`)
- `RideController` creates AWS clients in constructor
- No dependency injection container configured

### 2. Fat Controller
- `RideController` handles ride CRUD, map tiles, geocoding, and user lookup
- Mixes view rendering and JSON API responses

### 3. Anemic Domain Model
- Models are pure data containers with no business logic
- All behavior lives in controllers

### 4. Security Through Obscurity
- Password hashing duplicated in `UserController.GetHash()` and `Configuration.GetHash()`
- No service/utility class for shared security operations

### 5. Mixed Authentication Enforcement
- Some endpoints use `[Protected]` attribute, others check session manually
- `CreateRide` checks session but lacks the attribute (inconsistent)

## Cross-References

- [Components](components.md)
- [Complexity Analysis](../analysis/complexity-analysis.md)
- [Maintenance Burden](../technical-debt/maintenance-burden.md)
