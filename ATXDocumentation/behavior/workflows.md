> ⚠️ **Early Access**: Behavior documentation is in early access. Please review critically.

# Workflows

## 1. User Registration Workflow

**Entry Point:** `UserController.Create()` [GET] → Form submission → `UserController.Create()` [POST]

```
User navigates to /User/Create
    │
    ├── [GET] Render registration form
    │
    └── [POST] Form submitted
            │
            ├── Model validation fails → Return form with errors
            │
            └── Model valid
                    │
                    ├── Generate new GUID
                    ├── Hash password (SHA-256)
                    ├── Save to database
                    └── Redirect to Home/Index
```

## 2. User Login Workflow

**Entry Point:** `UserController.Login()` [GET] → Form submission → `UserController.Login()` [POST]

```
User navigates to /User/Login(?or=returnUrl)
    │
    ├── [GET] Render login form
    │
    └── [POST] Credentials submitted
            │
            ├── Email or Password empty → Show error
            │
            ├── User not found in DB → Show error
            │
            ├── Password hash mismatch → Show error
            │
            └── Authentication success
                    │
                    ├── Set Session["user"] = email
                    ├── Set Session["role"] = "user"
                    │
                    ├── If ?or= parameter exists → Redirect to that URL
                    └── Else → Redirect to Home/Index
```

## 3. Ride Request Workflow

**Entry Point:** `RideController.Map()` [GET] → Client-side map interaction → `RideController.CreateRide()` [POST]

```
User navigates to /Ride/Map (requires auth)
    │
    ├── Load Leaflet.js map with AWS Location Service tiles
    ├── Browser geolocation → Detect pickup location
    ├── ReverseGeocode pickup coordinates → Get address
    │
    ├── User clicks destination on map
    │   ├── ReverseGeocode destination coordinates → Get address
    │   └── Display destination marker
    │
    └── User confirms ride request
            │
            └── POST /Ride/CreateRide (AJAX)
                    │
                    ├── Session check fails → Return error JSON
                    ├── User not found → Return error JSON
                    │
                    └── Success path:
                            ├── Select random unicorn
                            ├── Create RideModel (status="Requested")
                            ├── Save to database
                            └── Return JSON {rideId, unicornName, unicornDesc, unicornColor}
```

## 4. Ride History Workflow

**Entry Point:** `RideController.Index()` [GET]

```
User navigates to /Ride/Index (requires auth)
    │
    ├── Get user email from session
    ├── Find user by email
    ├── Query rides for user (include Unicorn, User)
    ├── Order by DateTime
    └── Render view with ride list
```

## 5. Unicorn Management Workflow

**Entry Point:** `UnicornController.Index()` [GET]

```
User navigates to /Unicorn (requires auth)
    │
    ├── List all unicorns
    │
    ├── Create: /Unicorn/Create
    │       ├── [GET] Show form
    │       └── [POST] Validate → Save → Redirect to Index
    │
    └── Delete: /Unicorn/Delete/{id}
            ├── [GET] Show confirmation with unicorn details
            └── [POST] Remove from DB → Redirect to Index
```

## Cross-References

- [Business Logic](business-logic.md)
- [Decision Logic](decision-logic.md)
- [API Reference](../reference/api-reference.md)
