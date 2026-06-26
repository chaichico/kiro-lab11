# Interfaces and Contracts

## Inheritance Hierarchy

### Controller Hierarchy
```
System.Web.Mvc.Controller
├── HomeController
├── UserController
├── RideController
└── UnicornController
```

### DbContext Hierarchy
```
System.Data.Entity.DbContext
└── DefaultContext
```

### Filter Hierarchy
```
System.Web.Mvc.ActionFilterAttribute
└── Protected
```

### Model Hierarchy
All models are plain classes (no inheritance):
- `UserModel` → mapped to `[Table("Users")]`
- `UnicornModel` → mapped to `[Table("Unicorns")]`
- `RideModel` → mapped to `[Table("Rides")]`
- `Location` → EF6 complex type (no table)
- `Rating` → enum (stored as int)

## Contracts and Interfaces

### Implicit Contracts

The application does not define explicit interfaces. All contracts are implicit through class usage:

#### DefaultContext Contract
```csharp
public class DefaultContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }
    public DbSet<UnicornModel> Unicorns { get; set; }
    public DbSet<RideModel> Rides { get; set; }
}
```

#### Protected Filter Contract
```csharp
// Requires Session["user"] to be non-null
// Redirects to /User/Login?or={returnUrl} if unauthorized
public class Protected : ActionFilterAttribute
```

#### UserController.GetHash Contract
```csharp
// Static utility method - takes plaintext, returns SHA-256 hex string
public static string GetHash(string input) → string (64 hex chars)
```

### JSON API Contracts

#### CreateRide Request
```
POST /Ride/CreateRide
Parameters: pickupLat, pickupLng, pickupAddress, destLat, destLng, destAddress, passengers
Response: { success: bool, rideId?: Guid, unicornName?: string, unicornDesc?: string, unicornColor?: string, error?: string }
```

#### ReverseGeocode Request
```
POST /Ride/ReverseGeocode
Parameters: lat, lng
Response: { success: bool, address: string, error?: string }
```

#### GetMapTile Request
```
GET /Ride/GetMapTile/{z}/{x}/{y}
Response: image/png binary | HTTP 403 | HTTP 404
```

## Cross-References

- [Data Models](data-models.md)
- [API Reference](api-reference.md)
- [Components](../architecture/components.md)
