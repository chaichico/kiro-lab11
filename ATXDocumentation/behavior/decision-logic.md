> ⚠️ **Early Access**: Behavior documentation is in early access. Please review critically.

# Decision Logic

## Authentication Decisions

### Protected Attribute Filter
- **Location:** `sourceCode/wildrydes.net/Decorators/Protected.cs` (line 12)
- **Decision:** `Session["user"] == null`
  - **True:** Redirect to `/User/Login?or={currentUrl}`
  - **False:** Allow action to proceed

### Login Credential Validation
- **Location:** `sourceCode/wildrydes.net/Controllers/UserController.cs` (lines 36-58)
- **Decision Tree:**
  1. `Email` or `Password` empty? → Show error
  2. User exists in DB with matching email? → No: Show error
  3. `GetHash(password) == user.Password`? → No: Show error
  4. All pass → Set session, check redirect

### Post-Login Redirect
- **Location:** `sourceCode/wildrydes.net/Controllers/UserController.cs` (lines 48-55)
- **Decision:** `Request.QueryString["or"]` is not null/empty
  - **True:** `Redirect(Request.QueryString["or"])` (unvalidated)
  - **False:** `RedirectToAction("Index", "Home")`

## Ride Creation Decisions

### User Validation in CreateRide
- **Location:** `sourceCode/wildrydes.net/Controllers/RideController.cs` (lines 42-52)
- **Decision Tree:**
  1. `Session["user"]` is null/empty? → Return `{success: false, error: "User not logged in"}`
  2. User not found in DB? → Return `{success: false, error: "User not found"}`
  3. Both pass → Proceed with ride creation

### Unicorn Selection
- **Location:** `sourceCode/wildrydes.net/Controllers/RideController.cs` (lines 54-57)
- **Logic:** Random selection using `Random.Next(0, count)` and `OrderBy(Guid.NewGuid()).Skip(n).FirstOrDefault()`
- **Note:** This approach has a race condition - count may change between the count query and the skip query

## Geocoding Decisions

### ReverseGeocode Response Handling
- **Location:** `sourceCode/wildrydes.net/Controllers/RideController.cs` (lines 108-114)
- **Decision:** `response.Results?.Count > 0`
  - **True:** Return `{success: true, address: label}`
  - **False:** Return `{success: false, address: "{lat}, {lng}"}`

## Map Tile Decisions

### GetMapTile Error Handling
- **Location:** `sourceCode/wildrydes.net/Controllers/RideController.cs` (lines 147-155)
- **Decision Tree:**
  1. AccessDeniedException → HTTP 403 Forbidden
  2. Any other Exception → HTTP 404 Not Found

## Unicorn Management Decisions

### Delete Validation
- **Location:** `sourceCode/wildrydes.net/Controllers/UnicornController.cs` (lines 46-55)
- **Decision:** `id == null`
  - **True:** HTTP 400 Bad Request
- **Decision:** Unicorn not found in DB
  - **True:** HTTP 404 Not Found

### Create Binding
- **Location:** `sourceCode/wildrydes.net/Controllers/UnicornController.cs` (line 35)
- **Decision:** `[Bind(Include = "Id,Name,Description,Rating")]` - explicitly excludes `Color` from model binding (likely a bug - Color is Required but not bound)

## Model Validation Decisions

### UserModel Validation
- Email: `[Required]`, `[EmailAddress]`
- Password: `[Required]`

### UnicornModel Validation
- Name: `[Required]`
- Color: `[Required]`
- Description: `[Required]`

## Cross-References

- [Business Logic](business-logic.md)
- [Error Handling](error-handling.md)
- [Security Patterns](../analysis/security-patterns.md)
