> ⚠️ **Early Access**: Behavior documentation is in early access. Please review critically.

# Business Logic

## User Management

### UserController - Registration (`Create` action)
- **File:** `sourceCode/wildrydes.net/Controllers/UserController.cs` (lines 63-79)
- **Rules:**
  1. Validate model state (Email required + valid format, Password required)
  2. Generate new GUID for user ID
  3. Hash password with SHA-256 (no salt)
  4. Store user in database
  5. Redirect to home page on success
  6. Return form with validation errors on failure

### UserController - Authentication (`Login` action)
- **File:** `sourceCode/wildrydes.net/Controllers/UserController.cs` (lines 34-59)
- **Rules:**
  1. Require non-empty Email and Password
  2. Look up user by email in database
  3. Hash provided password and compare to stored hash
  4. On success: set `Session["user"]` = email, `Session["role"]` = "user"
  5. If `?or=` query parameter exists, redirect to that URL (open redirect)
  6. Otherwise redirect to Home/Index
  7. On failure: set ViewBag.Error = "Invalid login attempt"

### UserController - Logout
- **File:** `sourceCode/wildrydes.net/Controllers/UserController.cs` (lines 20-24)
- **Rules:**
  1. Abandon session
  2. Redirect to Home/Index

## Ride Management

### RideController - CreateRide
- **File:** `sourceCode/wildrydes.net/Controllers/RideController.cs` (lines 38-91)
- **Rules:**
  1. Check session for logged-in user email
  2. Look up user record by email
  3. Select random unicorn from database using `OrderBy(Guid.NewGuid()).Skip(random)`
  4. Create RideModel with:
     - New GUID ID
     - User and Unicorn FK references
     - Current timestamp
     - Pickup and destination locations (from request parameters)
     - Passenger count (from request)
     - EstimatedArrival = Now + 15 minutes (hardcoded)
     - EstimatedDistance = 0 (placeholder)
     - EstimatedDuration = 15 minutes (hardcoded)
     - Status = "Requested"
  5. Save to database
  6. Return JSON with ride ID, unicorn details

### RideController - ReverseGeocode
- **File:** `sourceCode/wildrydes.net/Controllers/RideController.cs` (lines 94-124)
- **Rules:**
  1. Call AWS Location Service `SearchPlaceIndexForPosition`
  2. Pass coordinates as [longitude, latitude] (AWS format)
  3. Request max 1 result
  4. Return place label if found, otherwise return formatted coordinates

### RideController - GetMapTile
- **File:** `sourceCode/wildrydes.net/Controllers/RideController.cs` (lines 128-157)
- **Rules:**
  1. Request tile from AWS Location Service by z/x/y coordinates
  2. Read response blob into byte array
  3. Set content type to image/png
  4. Set public caching with 1-hour max-age
  5. Return tile as file result

### RideController - Index (Ride History)
- **File:** `sourceCode/wildrydes.net/Controllers/RideController.cs` (lines 160-172)
- **Rules:**
  1. Get current user email from session
  2. Look up user by email
  3. Query rides for that user with Unicorn and User includes
  4. Order by DateTime ascending
  5. Return view with ride list

## Unicorn Management

### UnicornController - Index
- **File:** `sourceCode/wildrydes.net/Controllers/UnicornController.cs` (lines 20-23)
- **Rules:** List all unicorns from database

### UnicornController - Create
- **File:** `sourceCode/wildrydes.net/Controllers/UnicornController.cs` (lines 31-43)
- **Rules:**
  1. Bind only Id, Name, Description, Rating (excludes Color from binding)
  2. Validate model state
  3. Generate new GUID
  4. Save to database
  5. Anti-forgery token required

### UnicornController - Delete
- **File:** `sourceCode/wildrydes.net/Controllers/UnicornController.cs` (lines 46-66)
- **Rules:**
  1. GET: Show confirmation view with unicorn details
  2. POST: Find unicorn by ID, remove from database
  3. Anti-forgery token required on POST
  4. Cascade delete removes associated rides (FK constraint)

## Seed Data / Initial State

### Configuration.Seed()
- **File:** `sourceCode/wildrydes.net/Migrations/Configuration.cs`
- **Rules:**
  1. Seed 3 unicorns: Bucephalus (Gold, Rating 4), Shadowfox (White, Rating 5), Rocinante (Brown, Rating 4)
  2. Seed 1 user: `user@unicornrides.aws` with password `Passw0rd` (SHA-256 hashed)
  3. Uses `AddOrUpdate` for idempotency

## Cross-References

- [Workflows](workflows.md)
- [Decision Logic](decision-logic.md)
- [API Reference](../reference/api-reference.md)
