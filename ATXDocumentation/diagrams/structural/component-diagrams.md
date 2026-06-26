# Structural Diagrams

## Component Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                    Wild Rydes Application                         │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────────────┐  │
│  │ HomeController│  │UserController │  │  UnicornController   │  │
│  │              │  │              │  │                      │  │
│  │ • Index()    │  │ • Login()    │  │ • Index()            │  │
│  └──────────────┘  │ • Create()   │  │ • Create()           │  │
│                     │ • Logout()   │  │ • Delete()           │  │
│                     │ • GetHash()  │  └──────────┬───────────┘  │
│                     └──────┬───────┘             │              │
│                            │                     │              │
│  ┌─────────────────────────┼─────────────────────┼───────────┐  │
│  │           RideController │                     │           │  │
│  │                         │                     │           │  │
│  │ • Map()                 │                     │           │  │
│  │ • Index()               │                     │           │  │
│  │ • CreateRide()          │                     │           │  │
│  │ • ReverseGeocode()      │                     │           │  │
│  │ • GetMapTile()          │                     │           │  │
│  └────────────┬────────────┘                     │           │  │
│               │                                  │              │
│  ┌────────────▼──────────────────────────────────▼───────────┐  │
│  │                    DefaultContext                          │  │
│  │  ┌──────────┐  ┌──────────────┐  ┌─────────────────┐    │  │
│  │  │  Users   │  │   Unicorns   │  │      Rides      │    │  │
│  │  └──────────┘  └──────────────┘  └─────────────────┘    │  │
│  └───────────────────────────────────────────────────────────┘  │
│                                                                   │
│  ┌────────────────────────┐  ┌────────────────────────────────┐ │
│  │   Protected (Filter)   │  │      Configuration (Seed)      │ │
│  └────────────────────────┘  └────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────┘
```

## Class Diagram

```
┌─────────────────────┐     ┌─────────────────────────┐
│     UserModel       │     │      UnicornModel       │
├─────────────────────┤     ├─────────────────────────┤
│ + Id: Guid          │     │ + Id: Guid              │
│ + Email: string     │     │ + Name: string          │
│ + Password: string  │     │ + Color: string         │
└─────────┬───────────┘     │ + Description: string   │
          │                  │ + Rating: int           │
          │ 1            *   └──────────┬──────────────┘
          ├──────────────────┐          │ 1
          │                  │          │          *
          │    ┌─────────────▼──────────▼───────────────────┐
          │    │            RideModel                        │
          │    ├────────────────────────────────────────────┤
          │    │ + Id: Guid                                 │
          │    │ + UnicornId: Guid [FK]                     │
          │    │ + UserId: Guid [FK]                        │
          │    │ + DateTime: DateTime                       │
          │    │ + PickupLocation: Location                 │
          │    │ + DestinationLocation: Location            │
          │    │ + NumberOfPassengers: int                  │
          │    │ + SpecialRequests: string?                 │
          │    │ + EstimatedArrival: DateTime               │
          │    │ + EstimatedDistance: double                │
          │    │ + EstimatedDuration: int                   │
          │    │ + Status: string                           │
          │    │ + Rating: Rating?                          │
          │    └────────────────────────────────────────────┘
          │
          │    ┌─────────────────────┐    ┌────────────────┐
          │    │      Location       │    │     Rating     │
          │    ├─────────────────────┤    ├────────────────┤
          │    │ + Latitude: double  │    │ One = 1        │
          │    │ + Longitude: double │    │ Two = 2        │
          │    │ + Address: string   │    │ Three = 3      │
          │    └─────────────────────┘    │ Four = 4       │
          │                               │ Five = 5       │
          │                               └────────────────┘
```

## Package/Module Dependency Graph

```
┌─────────────────┐
│   App_Start     │
│ (Configuration) │
└────────┬────────┘
         │ configures
         ▼
┌─────────────────┐     ┌──────────────┐
│   Controllers   │────▶│  Decorators  │
└────────┬────────┘     │  (Protected) │
         │              └──────────────┘
         │ uses
         ▼
┌─────────────────┐
│    Context      │
│ (DefaultContext)│
└────────┬────────┘
         │ maps
         ▼
┌─────────────────┐
│     Models      │
└─────────────────┘
```

## Cross-References

- [Components](../../architecture/components.md)
- [Data Models](../../reference/data-models.md)
