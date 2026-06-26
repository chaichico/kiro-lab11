> ⚠️ **Early Access**: Behavior documentation is in early access. Please review critically.

# Error Handling

## Global Error Handling

### HandleErrorAttribute (Global Filter)
- **Location:** `sourceCode/wildrydes.net/App_Start/FilterConfig.cs` (line 9)
- **Behavior:** Catches unhandled exceptions in controller actions, renders `Views/Shared/Error.cshtml`
- **Scope:** All controllers (registered globally)

## Controller-Level Error Handling

### RideController.CreateRide
- **Location:** `sourceCode/wildrydes.net/Controllers/RideController.cs` (lines 38-91)
- **Pattern:** Try-catch wrapping entire method
- **On exception:** Logs to `Debug.WriteLine`, returns `{success: false, error: ex.Message}`
- **Issue:** Exposes raw exception messages to client (information disclosure)

### RideController.ReverseGeocode
- **Location:** `sourceCode/wildrydes.net/Controllers/RideController.cs` (lines 94-124)
- **Pattern:** Try-catch with specific `AccessDeniedException` handling
- **Handlers:**
  - `AccessDeniedException` → Return `{success: false, error: "Access denied to AWS Location Service"}`
  - General `Exception` → Return `{success: false, address: "{lat}, {lng}", error: ex.Message}`
- **Graceful degradation:** Falls back to coordinate display when geocoding fails

### RideController.GetMapTile
- **Location:** `sourceCode/wildrydes.net/Controllers/RideController.cs` (lines 128-157)
- **Pattern:** Try-catch with HTTP status code responses
- **Handlers:**
  - `AccessDeniedException` → HTTP 403
  - General `Exception` → HTTP 404
- **Note:** Swallows exception details (no logging in catch blocks)

### UnicornController.Delete
- **Location:** `sourceCode/wildrydes.net/Controllers/UnicornController.cs` (lines 46-55)
- **Pattern:** Null checks with HTTP status codes
- **Handlers:**
  - `id == null` → HTTP 400 BadRequest
  - Unicorn not found → HTTP 404 NotFound
- **No try-catch:** Database exceptions propagate to global handler

### UserController.Login
- **Location:** `sourceCode/wildrydes.net/Controllers/UserController.cs` (lines 34-59)
- **Pattern:** Conditional checks (no try-catch)
- **Error display:** Sets `ViewBag.Error = "Invalid login attempt"` for view rendering
- **No exception handling:** Database errors propagate to global handler

## Resource Disposal Pattern

All controllers that use `DefaultContext` implement `Dispose(bool)`:
- **UserController:** Disposes `db`
- **UnicornController:** Disposes `db`
- **RideController:** Disposes `db` and `_locationClient`

## Missing Error Handling

1. **No validation on CreateRide parameters** - No checks for valid latitude/longitude ranges
2. **No null check on random unicorn** - If no unicorns exist, `FirstOrDefault()` returns null → NullReferenceException
3. **No null check on user in RideController.Index** - If user lookup fails, next line throws NullReferenceException
4. **No transaction management** - Database operations are not wrapped in explicit transactions

## Cross-References

- [Decision Logic](decision-logic.md)
- [Security Patterns](../analysis/security-patterns.md)
- [Complexity Analysis](../analysis/complexity-analysis.md)
