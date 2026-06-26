# Complexity Analysis

## Complexity Hotspots

### 1. RideController.CreateRide (Highest Complexity)
- **File:** `sourceCode/wildrydes.net/Controllers/RideController.cs` (lines 38-91)
- **Cyclomatic Complexity:** ~5
- **Reasons:**
  - Multiple null checks (session, user)
  - Random unicorn selection with database queries
  - Object construction with many properties
  - Try-catch wrapper
- **Risk:** Null reference exceptions if unicorn count is 0

### 2. UserController.Login (Medium Complexity)
- **File:** `sourceCode/wildrydes.net/Controllers/UserController.cs` (lines 34-59)
- **Cyclomatic Complexity:** ~5
- **Reasons:**
  - Nested conditionals (email/password empty → user exists → password matches → redirect check)
  - Multiple exit paths
- **Risk:** Open redirect vulnerability in redirect logic

### 3. RideController.ReverseGeocode (Medium Complexity)
- **File:** `sourceCode/wildrydes.net/Controllers/RideController.cs` (lines 94-124)
- **Cyclomatic Complexity:** ~4
- **Reasons:**
  - Async AWS API call
  - Null-conditional response checking
  - Multiple catch blocks (specific + general)

### 4. RideController.GetMapTile (Low-Medium Complexity)
- **File:** `sourceCode/wildrydes.net/Controllers/RideController.cs` (lines 128-157)
- **Cyclomatic Complexity:** ~3
- **Reasons:**
  - Async AWS API call
  - Stream-to-byte-array conversion
  - Multiple catch blocks

## Low Complexity Areas

All other files have cyclomatic complexity of 1-2:
- HomeController (single action, no branches)
- UnicornController (standard CRUD with minimal branching)
- Models (no methods, data-only)
- DefaultContext (override with single statement)
- Protected filter (single if-check)

## Coupling Analysis

| Component | Afferent (incoming) | Efferent (outgoing) | Instability |
|-----------|--------------------|--------------------|-------------|
| DefaultContext | 4 (all controllers + migrations) | 3 (models) | 0.43 |
| UserModel | 3 (context, user ctrl, ride ctrl) | 0 | 0.00 |
| UnicornModel | 3 (context, unicorn ctrl, ride ctrl) | 0 | 0.00 |
| RideModel | 2 (context, ride ctrl) | 2 (UserModel, UnicornModel) | 0.50 |
| RideController | 0 | 5 (context, models, AWS, config) | 1.00 |
| UserController | 0 | 2 (context, UserModel) | 1.00 |
| Protected | 2 (ride ctrl, unicorn ctrl) | 0 | 0.00 |

## Code Duplication

| Pattern | Occurrences | Locations |
|---------|-------------|-----------|
| SHA-256 GetHash method | 2 | UserController.cs:95, Configuration.cs:56 |
| `new DefaultContext()` | 4 | All 4 controllers |
| Session["user"] check | 3 | Protected.cs, RideController (2x) |

## Cross-References

- [Code Metrics](code-metrics.md)
- [Maintenance Burden](../technical-debt/maintenance-burden.md)
- [Patterns](../architecture/patterns.md)
