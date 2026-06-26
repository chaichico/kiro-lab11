# Remediation Plan

## Priority 1: Critical Security Fixes (Severity: High)

### 1.1 Replace SHA-256 Password Hashing
- **Current:** `SHA256.Create().ComputeHash()` without salt
- **Action:** Replace with bcrypt, scrypt, or PBKDF2 with per-user salt
- **Files:** `UserController.cs` (line 95), `Configuration.cs` (line 56)
- **Complexity:** Medium (must re-hash all existing passwords or force reset)

### 1.2 Fix Open Redirect Vulnerability
- **Current:** `Redirect(Request.QueryString["or"])` - accepts any URL
- **Action:** Validate redirect URL is local using `Url.IsLocalUrl()`
- **File:** `UserController.cs` (line 50)
- **Complexity:** Low

### 1.3 Add CSRF Protection to API Endpoints
- **Current:** `CreateRide` and `ReverseGeocode` lack `[ValidateAntiForgeryToken]`
- **Action:** Add anti-forgery validation or implement token-based API auth
- **File:** `RideController.cs`
- **Complexity:** Low

### 1.4 Remove Hardcoded Credentials from Seed Data
- **Current:** Default user `user@unicornrides.aws` / `Passw0rd` in source
- **Action:** Move to environment-specific seed mechanism or remove
- **File:** `Configuration.cs`
- **Complexity:** Low

## Priority 2: Platform Modernization (Severity: High)

### 2.1 Migrate to .NET 8+
- **Current:** .NET Framework 4.8.1
- **Action:** Port application to .NET 8 using .NET Upgrade Assistant
- **Key Changes:**
  - Replace `System.Web.Mvc` with `Microsoft.AspNetCore.Mvc`
  - Replace `System.Web.HttpContext` with `Microsoft.AspNetCore.Http`
  - Migrate `Web.config` to `appsettings.json`
  - Replace `Global.asax` with `Program.cs` / `Startup.cs`
  - Add dependency injection for DbContext and AWS clients
- **Complexity:** High (affects all files)

### 2.2 Migrate Entity Framework 6 to EF Core
- **Current:** Entity Framework 6.5.1
- **Action:** Port DbContext, models, and migrations to EF Core 8
- **Key Changes:**
  - Update DbContext configuration
  - Recreate migrations
  - Update LINQ queries for EF Core compatibility
  - Replace complex type configuration
- **Complexity:** Medium

### 2.3 Containerize on Linux
- **Current:** Windows Server Core Docker image (~5GB)
- **Action:** After .NET 8 migration, switch to Linux-based image (~200MB)
- **Complexity:** Low (after .NET 8 migration)

## Priority 3: Architecture Improvements (Severity: Medium)

### 3.1 Introduce Dependency Injection
- **Action:** Register DbContext and AWS clients in DI container
- **Benefit:** Testability, lifetime management, configuration flexibility
- **Complexity:** Medium

### 3.2 Separate API and MVC Concerns
- **Action:** Create dedicated API controller for JSON endpoints, keep MVC controllers for views
- **Benefit:** Clear separation, easier to add API consumers
- **Complexity:** Medium

### 3.3 Implement Proper Authentication
- **Action:** Replace custom session auth with ASP.NET Core Identity or integrate Cognito for user auth
- **Benefit:** Industry-standard security, built-in features (lockout, 2FA, password reset)
- **Complexity:** High

### 3.4 Add Credential Caching for AWS Clients
- **Action:** Register AWS clients as singletons with credential refresh
- **Benefit:** Reduced API calls, better performance
- **Complexity:** Low

## Priority 4: Quality Infrastructure (Severity: Low)

### 4.1 Add Unit Test Project
- **Action:** Create test project with xUnit/NUnit, add tests for business logic
- **Complexity:** Medium

### 4.2 Remove Committed Binaries
- **Action:** Delete `references/` folder, configure NuGet package restore
- **Complexity:** Low

### 4.3 Remove Obsolete Libraries
- **Action:** Remove Modernizr, WebGrease, Antlr; update bundling approach
- **Complexity:** Low

## Recommended Execution Order

1. Security fixes (1.1-1.4) - Immediate
2. Platform migration (2.1-2.3) - Next phase
3. Architecture improvements (3.1-3.4) - During/after migration
4. Quality infrastructure (4.1-4.3) - Ongoing

## Cross-References

- [Outdated Components](outdated-components.md)
- [Maintenance Burden](maintenance-burden.md)
- [Technical Debt Report](../technical-debt-report.md)
- [Migration - Component Order](../migration/component-order.md)
