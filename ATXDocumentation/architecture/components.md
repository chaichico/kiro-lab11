# Components

## Controllers

### HomeController
- **File:** `sourceCode/wildrydes.net/Controllers/HomeController.cs`
- **Responsibility:** Landing page rendering
- **Actions:** `Index()` - Returns home page view
- **Authentication:** None required

### UserController
- **File:** `sourceCode/wildrydes.net/Controllers/UserController.cs`
- **Responsibility:** User registration, login, logout
- **Actions:**
  - `Login()` [GET/POST] - Authenticate user, set session
  - `Create()` [GET/POST] - Register new user with hashed password
  - `Logout()` [GET] - Abandon session, redirect to home
- **Authentication:** None (handles auth itself)
- **Dependencies:** DefaultContext, SHA256

### RideController
- **File:** `sourceCode/wildrydes.net/Controllers/RideController.cs`
- **Responsibility:** Ride management, map tiles, geocoding
- **Actions:**
  - `Map()` [GET, Protected] - Render map view for ride requests
  - `Index()` [GET, Protected] - List user's ride history
  - `CreateRide()` [POST] - Create new ride (JSON API)
  - `ReverseGeocode()` [POST] - Convert lat/lng to address (JSON API)
  - `GetMapTile()` [GET] - Proxy map tiles from AWS Location Service
- **Authentication:** Mixed - Map/Index use `[Protected]`, APIs check session manually
- **Dependencies:** DefaultContext, AmazonLocationServiceClient, CognitoAWSCredentials

### UnicornController
- **File:** `sourceCode/wildrydes.net/Controllers/UnicornController.cs`
- **Responsibility:** Unicorn fleet CRUD operations
- **Actions:**
  - `Index()` [GET] - List all unicorns
  - `Create()` [GET/POST] - Add new unicorn
  - `Delete()` [GET/POST] - Remove unicorn
- **Authentication:** Entire controller protected via `[Protected]` class-level attribute
- **Dependencies:** DefaultContext

## Models

### UserModel
- **File:** `sourceCode/wildrydes.net/Models/UserModel.cs`
- **Table:** `Users`
- **Properties:** Id (Guid), Email (Required, EmailAddress), Password (Required, hashed)

### UnicornModel
- **File:** `sourceCode/wildrydes.net/Models/UnicornModel.cs`
- **Table:** `Unicorns`
- **Properties:** Id (Guid), Name, Color, Description (all Required), Rating (int)

### RideModel
- **File:** `sourceCode/wildrydes.net/Models/RideModel.cs`
- **Table:** `Rides`
- **Properties:** Id, UnicornId (FK), UserId (FK), DateTime, PickupLocation, DestinationLocation, NumberOfPassengers, SpecialRequests, EstimatedArrival, EstimatedDistance, EstimatedDuration, Status, Rating

### Location (Complex Type)
- **File:** `sourceCode/wildrydes.net/Models/RideModel.cs`
- **Properties:** Latitude (double), Longitude (double), Address (string)
- **Used as:** Embedded complex type in RideModel (PickupLocation, DestinationLocation)

### Rating (Enum)
- **File:** `sourceCode/wildrydes.net/Models/RideModel.cs`
- **Values:** One=1, Two=2, Three=3, Four=4, Five=5

## Data Access

### DefaultContext
- **File:** `sourceCode/wildrydes.net/Context/DefaultContext.cs`
- **Base:** `DbContext` (Entity Framework 6)
- **Connection:** `DefaultConnection` (SQL Server LocalDB)
- **DbSets:** Users, Unicorns, Rides
- **Configuration:** Removes PluralizingTableNameConvention

## Decorators/Filters

### Protected (ActionFilterAttribute)
- **File:** `sourceCode/wildrydes.net/Decorators/Protected.cs`
- **Purpose:** Session-based authorization guard
- **Behavior:** Redirects to `/User/Login?or={returnUrl}` if `Session["user"]` is null

## App_Start Configuration

### BundleConfig
- **File:** `sourceCode/wildrydes.net/App_Start/BundleConfig.cs`
- **Bundles:** jquery, jqueryval, modernizr, bootstrap (JS), css (Bootstrap + Site.css)

### FilterConfig
- **File:** `sourceCode/wildrydes.net/App_Start/FilterConfig.cs`
- **Global Filters:** HandleErrorAttribute

### RouteConfig
- **File:** `sourceCode/wildrydes.net/App_Start/RouteConfig.cs`
- **Routes:** TestImage, MapTile (parameterized), Default `{controller}/{action}/{id}`

## Cross-References

- [System Overview](system-overview.md)
- [Data Models](../reference/data-models.md)
- [API Reference](../reference/api-reference.md)
