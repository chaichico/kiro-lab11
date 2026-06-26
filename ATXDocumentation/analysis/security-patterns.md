# Security Patterns

## Authentication Implementation

### Session-Based Authentication
- **Mechanism:** ASP.NET session state (`Session["user"]`, `Session["role"]`)
- **Storage:** In-process memory (default IIS session state)
- **Login:** Sets session variables on successful credential validation
- **Logout:** `Session.Abandon()` destroys all session data
- **Enforcement:** Custom `[Protected]` action filter attribute

### Password Storage
- **Algorithm:** SHA-256 (unsalted)
- **Implementation:** `UserController.GetHash(string)` and `Configuration.GetHash(string)`
- **Vulnerability:** No per-user salt makes passwords vulnerable to rainbow table attacks
- **Recommendation:** Replace with bcrypt/PBKDF2 with random salt

## Authorization Implementation

### Protected Attribute
- **File:** `sourceCode/wildrydes.net/Decorators/Protected.cs`
- **Logic:** Checks `Session["user"] != null`
- **Failure action:** Redirect to `/User/Login?or={returnUrl}`
- **Scope:** Applied per-action or per-controller

### Authorization Gaps
- `CreateRide` endpoint checks session internally but lacks `[Protected]` attribute
- `ReverseGeocode` and `GetMapTile` have no authentication (by design - map resources)
- No role-based authorization (all authenticated users have equal access)

## CSRF Protection

### Protected Endpoints (with ValidateAntiForgeryToken)
- `POST /User/Login`
- `POST /User/Create`
- `POST /Unicorn/Create`
- `POST /Unicorn/Delete`

### Unprotected Endpoints (missing CSRF)
- `POST /Ride/CreateRide` - No anti-forgery token
- `POST /Ride/ReverseGeocode` - No anti-forgery token

## Input Validation

### Server-Side Validation
- Model validation via Data Annotations (`[Required]`, `[EmailAddress]`)
- `ModelState.IsValid` checked on form submissions
- `[Bind(Include = ...)]` limits mass assignment on Create actions

### Client-Side Validation
- jQuery.Validation library loaded for form validation
- Unobtrusive JavaScript validation enabled in Web.config

### SQL Injection Protection
- Entity Framework parameterized queries (implicit protection)
- No raw SQL strings or `SqlCommand` usage detected

## Identified Vulnerabilities

### 1. Open Redirect (High)
- **Location:** `UserController.cs` line 50
- **Code:** `return Redirect(Request.QueryString["or"])`
- **Risk:** Attacker can craft URL redirecting to malicious site after login

### 2. Unsalted Password Hash (High)
- **Location:** `UserController.cs` line 95
- **Risk:** Rainbow table attacks can reverse password hashes

### 3. Information Disclosure (Medium)
- **Location:** `RideController.CreateRide` catch block
- **Code:** Returns `ex.Message` to client
- **Risk:** Stack traces/internal details may leak

### 4. Missing CSRF on API (Medium)
- **Location:** `RideController.CreateRide`, `RideController.ReverseGeocode`
- **Risk:** Cross-site request forgery attacks possible

### 5. Hardcoded Credentials (Low)
- **Location:** `Configuration.cs` seed data
- **Details:** `user@unicornrides.aws` / `Passw0rd`
- **Risk:** Default credentials if seed runs in production

## Encryption and Transport

- No explicit HTTPS enforcement in code (relies on infrastructure)
- No HSTS headers configured
- AWS SDK communication uses TLS (built-in)

## Cross-References

- [Error Handling](../behavior/error-handling.md)
- [Decision Logic](../behavior/decision-logic.md)
- [Technical Debt Report](../technical-debt-report.md)
