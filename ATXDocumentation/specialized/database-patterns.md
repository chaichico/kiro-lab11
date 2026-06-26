# Database Patterns

## ORM Configuration

### Entity Framework 6 Code-First
- **DbContext:** `DefaultContext` (inherits `System.Data.Entity.DbContext`)
- **Connection:** Named `DefaultConnection` referencing SQL Server LocalDB
- **Convention:** `PluralizingTableNameConvention` removed (table names match `[Table]` attributes)
- **Migration Strategy:** Explicit migrations (`AutomaticMigrationsEnabled = false`)

## Schema Definition

### Tables

```sql
-- Users table
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Email NVARCHAR(MAX) NOT NULL,
    Password NVARCHAR(MAX) NOT NULL
);

-- Unicorns table
CREATE TABLE Unicorns (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(MAX) NOT NULL,
    Color NVARCHAR(MAX) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Rating INT NOT NULL DEFAULT 0
);

-- Rides table (with complex type columns)
CREATE TABLE Rides (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    UnicornId UNIQUEIDENTIFIER NOT NULL,
    UserId UNIQUEIDENTIFIER NOT NULL,
    DateTime DATETIME NOT NULL,
    PickupLocation_Latitude FLOAT NOT NULL,
    PickupLocation_Longitude FLOAT NOT NULL,
    PickupLocation_Address NVARCHAR(MAX),
    DestinationLocation_Latitude FLOAT NOT NULL,
    DestinationLocation_Longitude FLOAT NOT NULL,
    DestinationLocation_Address NVARCHAR(MAX),
    NumberOfPassengers INT NOT NULL DEFAULT 1,
    SpecialRequests NVARCHAR(MAX),
    EstimatedArrival DATETIME NOT NULL,
    EstimatedDistance FLOAT NOT NULL,
    EstimatedDuration INT NOT NULL,
    Status NVARCHAR(MAX) DEFAULT 'Pending',
    Rating INT NULL,
    CONSTRAINT FK_Rides_Unicorns FOREIGN KEY (UnicornId) REFERENCES Unicorns(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Rides_Users FOREIGN KEY (UserId) REFERENCES Unicorns(Id) ON DELETE CASCADE
);

CREATE INDEX IX_Rides_UnicornId ON Rides(UnicornId);
CREATE INDEX IX_Rides_UserId ON Rides(UserId);
```

## Query Patterns

### User Lookup by Email
```csharp
// UserController.Login, RideController.CreateRide, RideController.Index
db.Users.FirstOrDefault(u => u.Email == userEmail)
// Generates: SELECT TOP(1) * FROM Users WHERE Email = @p0
```

### Random Unicorn Selection
```csharp
// RideController.CreateRide
var skip = rand.Next(0, db.Unicorns.Count());
var unicorn = db.Unicorns.OrderBy(x => Guid.NewGuid()).Skip(skip).FirstOrDefault();
// Issues: Two DB round-trips, race condition between Count and query
```

### Ride History with Eager Loading
```csharp
// RideController.Index
db.Rides.Include(r => r.Unicorn)
    .Include(r => r.User)
    .Where(r => r.UserId == user.Id)
    .OrderBy(r => r.DateTime)
    .ToList();
```

### Unicorn CRUD
```csharp
// Standard EF6 patterns
db.Unicorns.ToList()               // List all
db.Unicorns.Find(id)              // Find by PK
db.Unicorns.Add(unicornModel)     // Insert
db.Unicorns.Remove(unicornModel)  // Delete
db.SaveChanges()                   // Commit
```

## Seed Data

Seeded via `Configuration.Seed()` method using `AddOrUpdate`:
- 3 Unicorns (fixed GUIDs for idempotency)
- 1 User (fixed GUID, password pre-hashed)

## Performance Considerations

- No explicit indexing beyond FK-generated indexes
- `NVARCHAR(MAX)` used for all string columns (no length constraints)
- Complex type columns (`Location`) cannot be individually indexed
- No connection pooling configuration (uses EF6/ADO.NET defaults)
- No query caching or compiled queries

## Cross-References

- [Data Models](../reference/data-models.md)
- [Dependency Analysis](../analysis/dependency-analysis.md)
