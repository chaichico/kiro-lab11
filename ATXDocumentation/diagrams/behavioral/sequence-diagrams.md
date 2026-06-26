# Behavioral Diagrams

## Sequence Diagram: User Login

```
Browser          UserController       DefaultContext       Database
  │                   │                    │                  │
  │  GET /User/Login  │                    │                  │
  │──────────────────▶│                    │                  │
  │  Login Form HTML  │                    │                  │
  │◀──────────────────│                    │                  │
  │                   │                    │                  │
  │ POST /User/Login  │                    │                  │
  │ {email, password} │                    │                  │
  │──────────────────▶│                    │                  │
  │                   │  Find user by email│                  │
  │                   │───────────────────▶│  SELECT * FROM   │
  │                   │                    │─────────────────▶│
  │                   │                    │  User record     │
  │                   │   User entity      │◀─────────────────│
  │                   │◀───────────────────│                  │
  │                   │                    │                  │
  │                   │ Compare hash       │                  │
  │                   │ Set session        │                  │
  │  302 Redirect     │                    │                  │
  │◀──────────────────│                    │                  │
```

## Sequence Diagram: Create Ride

```
Browser          RideController       DefaultContext       AWS Location
  │                   │                    │                  │
  │ POST /Ride/       │                    │                  │
  │   CreateRide      │                    │                  │
  │──────────────────▶│                    │                  │
  │                   │ Check session      │                  │
  │                   │                    │                  │
  │                   │ Find user by email │                  │
  │                   │───────────────────▶│                  │
  │                   │  User entity       │                  │
  │                   │◀───────────────────│                  │
  │                   │                    │                  │
  │                   │ Get random unicorn │                  │
  │                   │───────────────────▶│                  │
  │                   │  Unicorn entity    │                  │
  │                   │◀───────────────────│                  │
  │                   │                    │                  │
  │                   │ Create RideModel   │                  │
  │                   │ Save ride          │                  │
  │                   │───────────────────▶│                  │
  │                   │  SaveChanges OK    │                  │
  │                   │◀───────────────────│                  │
  │                   │                    │                  │
  │  JSON response    │                    │                  │
  │  {success, ride}  │                    │                  │
  │◀──────────────────│                    │                  │
```

## Sequence Diagram: Map Tile Loading

```
Browser          RideController       AWS Cognito        AWS Location
  │                   │                    │                  │
  │ GET /Ride/        │                    │                  │
  │  GetMapTile/z/x/y │                    │                  │
  │──────────────────▶│                    │                  │
  │                   │ Get credentials    │                  │
  │                   │───────────────────▶│                  │
  │                   │ Temp credentials   │                  │
  │                   │◀───────────────────│                  │
  │                   │                    │                  │
  │                   │ GetMapTile request │                  │
  │                   │────────────────────────────────────▶│
  │                   │ Tile blob (PNG)    │                  │
  │                   │◀────────────────────────────────────│
  │                   │                    │                  │
  │  image/png        │                    │                  │
  │  Cache: 1hr       │                    │                  │
  │◀──────────────────│                    │                  │
```

## Activity Diagram: Ride Request Flow

```
┌─────────────────┐
│  User opens Map │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Browser detects │
│ geolocation     │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Reverse geocode │
│ pickup location │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ User clicks map │
│ for destination │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Reverse geocode │
│ destination     │
└────────┬────────┘
         │
         ▼
┌─────────────────────┐
│ User confirms ride  │
│ (sets passengers)   │
└────────┬────────────┘
         │
         ▼
┌─────────────────────┐     ┌──────────────────┐
│ POST CreateRide     │────▶│ Random unicorn   │
│                     │     │ assigned         │
└────────┬────────────┘     └──────────────────┘
         │
         ▼
┌─────────────────────┐
│ Ride saved to DB    │
│ Status: "Requested" │
└────────┬────────────┘
         │
         ▼
┌─────────────────────┐
│ Display unicorn     │
│ details to user     │
└─────────────────────┘
```

## Cross-References

- [Workflows](../../behavior/workflows.md)
- [Business Logic](../../behavior/business-logic.md)
