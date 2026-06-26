# Data Models

## Entity Models

### UserModel
- **File:** `sourceCode/wildrydes.net/Models/UserModel.cs`
- **Table:** `Users`

| Property | Type | Attributes | Description |
|----------|------|-----------|-------------|
| Id | Guid | PK | Unique identifier |
| Email | string | [Required], [EmailAddress] | User's email address |
| Password | string | [Required], [DataType(Password)] | SHA-256 hash of password |

### UnicornModel
- **File:** `sourceCode/wildrydes.net/Models/UnicornModel.cs`
- **Table:** `Unicorns`

| Property | Type | Attributes | Description |
|----------|------|-----------|-------------|
| Id | Guid | PK | Unique identifier |
| Name | string | [Required] | Unicorn's name |
| Color | string | [Required] | Unicorn's color |
| Description | string | [Required] | Bio/description text |
| Rating | int | - | Quality rating (0-5) |

### RideModel
- **File:** `sourceCode/wildrydes.net/Models/RideModel.cs`
- **Table:** `Rides`

| Property | Type | Attributes | Description |
|----------|------|-----------|-------------|
| Id | Guid | PK | Unique identifier |
| UnicornId | Guid | [ForeignKey("Unicorn")] | FK to Unicorns |
| Unicorn | UnicornModel | virtual (navigation) | Related unicorn |
| UserId | Guid | [ForeignKey("User")] | FK to Users |
| User | UserModel | virtual (navigation) | Related user |
| DateTime | DateTime | - | Request timestamp |
| PickupLocation | Location | complex type | Pickup coordinates + address |
| DestinationLocation | Location | complex type | Destination coordinates + address |
| NumberOfPassengers | int | default: 1 | Passenger count |
| SpecialRequests | string | nullable | Optional notes |
| EstimatedArrival | DateTime | - | ETA (always +15 min) |
| EstimatedDistance | double | - | Distance (always 0) |
| EstimatedDuration | int | - | Duration in minutes (always 15) |
| Status | string | default: "Pending" | Ride status |
| Rating | Rating? | nullable enum | Post-ride rating |

## Complex Types

### Location
- **File:** `sourceCode/wildrydes.net/Models/RideModel.cs`
- **Storage:** Embedded columns (e.g., `PickupLocation_Latitude`, `PickupLocation_Longitude`, `PickupLocation_Address`)

| Property | Type | Description |
|----------|------|-------------|
| Latitude | double | Geographic latitude |
| Longitude | double | Geographic longitude |
| Address | string | Human-readable address |

## Enumerations

### Rating
- **File:** `sourceCode/wildrydes.net/Models/RideModel.cs`
- **Values:** One=1, Two=2, Three=3, Four=4, Five=5
- **Usage:** Optional post-ride rating on RideModel

## Entity Relationships

```
Users (1) ──────< Rides (many)    [FK: UserId, cascade delete]
Unicorns (1) ──< Rides (many)    [FK: UnicornId, cascade delete]
```

## Database Schema (from Migration)

```sql
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Email NVARCHAR(MAX) NOT NULL,
    Password NVARCHAR(MAX) NOT NULL
)

CREATE TABLE Unicorns (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(MAX) NOT NULL,
    Color NVARCHAR(MAX) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Rating INT NOT NULL
)

CREATE TABLE Rides (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    UnicornId UNIQUEIDENTIFIER NOT NULL REFERENCES Unicorns(Id) ON DELETE CASCADE,
    UserId UNIQUEIDENTIFIER NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    DateTime DATETIME NOT NULL,
    PickupLocation_Latitude FLOAT NOT NULL,
    PickupLocation_Longitude FLOAT NOT NULL,
    PickupLocation_Address NVARCHAR(MAX),
    DestinationLocation_Latitude FLOAT NOT NULL,
    DestinationLocation_Longitude FLOAT NOT NULL,
    DestinationLocation_Address NVARCHAR(MAX),
    NumberOfPassengers INT NOT NULL,
    SpecialRequests NVARCHAR(MAX),
    EstimatedArrival DATETIME NOT NULL,
    EstimatedDistance FLOAT NOT NULL,
    EstimatedDuration INT NOT NULL,
    Status NVARCHAR(MAX),
    Rating INT
)
```

## Cross-References

- [Interfaces](interfaces.md)
- [Components](../architecture/components.md)
- [Database Patterns](../specialized/database-patterns.md)
