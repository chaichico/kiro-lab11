# Maintenance Burden

## High-Maintenance Areas

### 1. Authentication System
- **Location:** `UserController.cs`, `Protected.cs`
- **Burden:** Custom session-based auth without standard framework support
- **Issues:**
  - No built-in session timeout configuration exposed
  - No "remember me" functionality
  - No password reset workflow
  - No account lockout after failed attempts
  - Password hashing logic duplicated in two files
- **Impact:** Every authentication feature must be built from scratch

### 2. AWS Credential Management
- **Location:** `RideController.cs` constructor
- **Burden:** Cognito credentials created per-controller instantiation
- **Issues:**
  - No credential caching or pooling
  - Credentials renewed on every request (RideController is not singleton)
  - No graceful handling of credential expiry
- **Impact:** Potential performance issues and AWS API throttling

### 3. Database Context Management
- **Location:** All controllers
- **Burden:** Each controller creates and disposes its own `DefaultContext`
- **Issues:**
  - No connection pooling configuration
  - No retry logic for transient failures
  - No unit of work pattern for cross-controller operations
- **Impact:** Difficult to add transaction boundaries or shared state

### 4. Windows-Only Infrastructure
- **Location:** `Dockerfile`, `.csproj`, all .NET Framework dependencies
- **Burden:** Cannot run on Linux CI/CD, restricted to Windows hosts
- **Issues:**
  - Windows Docker containers require Windows Server hosts
  - Limited Kubernetes support (Windows node pools only)
  - Large image sizes increase deployment time
  - Windows Server licensing costs
- **Impact:** Significant infrastructure cost and limited deployment flexibility

### 5. Committed Binary References
- **Location:** `references/packages/`, `references/reference assemblies/`
- **Burden:** NuGet packages and framework assemblies committed to source control
- **Issues:**
  - Repository size bloated with binary DLLs
  - Manual update process for dependencies
  - No NuGet restore workflow
  - Version conflicts harder to detect
- **Impact:** Slow clones, manual dependency management

## Moderate-Maintenance Areas

### 6. Map Tile Proxy
- **Location:** `RideController.GetMapTile()`
- **Burden:** Server-side proxy for every map tile request
- **Issues:**
  - All map traffic flows through application server
  - No CDN caching
  - Single hour cache directive (client-side only)
- **Impact:** Server load proportional to map interactions

### 7. Mixed API Patterns
- **Location:** `RideController` (CreateRide, ReverseGeocode return JSON; Map, Index return views)
- **Burden:** Same controller handles both page rendering and API endpoints
- **Issues:**
  - No consistent error response format
  - Authentication applied inconsistently
  - No API versioning or documentation
- **Impact:** Difficult to separate concerns or add API consumers

## Low-Maintenance Areas

### 8. Unicorn CRUD
- **Location:** `UnicornController.cs`
- **Burden:** Standard scaffolded CRUD, straightforward
- **Issues:** Minor - Color field not bound in Create action (likely bug)

### 9. Home Page
- **Location:** `HomeController.cs`
- **Burden:** Minimal - single action returning a view

## Cross-References

- [Summary](summary.md)
- [Remediation Plan](remediation-plan.md)
- [Patterns](../architecture/patterns.md)
