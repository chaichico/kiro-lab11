# Architecture Diagrams

## System Context Diagram

```
┌─────────────────────────────────────────────────────────────────────┐
│                         External Actors                               │
│                                                                       │
│  ┌──────────┐                                        ┌────────────┐ │
│  │  Browser  │                                        │ AWS Cloud  │ │
│  │  (User)   │                                        │            │ │
│  └─────┬─────┘                                        └──────┬─────┘ │
└────────┼─────────────────────────────────────────────────────┼───────┘
         │ HTTP/HTTPS                                          │
         ▼                                                     │
┌─────────────────────────────────────────────────────────────────────┐
│                    Docker Container (Windows)                         │
│  ┌───────────────────────────────────────────────────────────────┐  │
│  │                         IIS                                    │  │
│  │  ┌─────────────────────────────────────────────────────────┐  │  │
│  │  │              Wild Rydes ASP.NET MVC 5                    │  │  │
│  │  │                                                          │  │  │
│  │  │  ┌────────────┐  ┌───────────┐  ┌──────────────────┐   │  │  │
│  │  │  │   Views    │  │Controllers│  │   AWS SDK Client │───┼──┼──┼──▶ AWS
│  │  │  │  (Razor)   │  │           │  │                  │   │  │  │
│  │  │  └────────────┘  └─────┬─────┘  └──────────────────┘   │  │  │
│  │  │                        │                                 │  │  │
│  │  │                  ┌─────▼─────┐                          │  │  │
│  │  │                  │  EF6 +    │                          │  │  │
│  │  │                  │ DbContext │                          │  │  │
│  │  │                  └─────┬─────┘                          │  │  │
│  │  └────────────────────────┼────────────────────────────────┘  │  │
│  └───────────────────────────┼───────────────────────────────────┘  │
│                              │                                       │
│  ┌───────────────────────────▼───────────────────────────────────┐  │
│  │                SQL Server LocalDB                              │  │
│  │  ┌──────────┐  ┌──────────────┐  ┌─────────────────────┐    │  │
│  │  │  Users   │  │   Unicorns   │  │       Rides         │    │  │
│  │  └──────────┘  └──────────────┘  └─────────────────────┘    │  │
│  └───────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────┘
```

## Integration Patterns

```
┌──────────────────────────────────────────────────────────────┐
│                    Wild Rydes Application                      │
│                                                                │
│  RideController                                                │
│       │                                                        │
│       ├──▶ CognitoAWSCredentials ──▶ AWS Cognito Identity Pool│
│       │         (temporary credentials)                        │
│       │                                                        │
│       ├──▶ AmazonLocationServiceClient                        │
│       │         │                                              │
│       │         ├──▶ GetMapTile() ──▶ AWS Location Maps       │
│       │         │                                              │
│       │         └──▶ SearchPlaceIndexForPosition()            │
│       │                   └──▶ AWS Location Places             │
│       │                                                        │
│       └──▶ DefaultContext ──▶ SQL Server LocalDB              │
│                                                                │
└──────────────────────────────────────────────────────────────┘
```

## Security Boundaries

```
┌─────────────────────────────────────────────────────────────┐
│                    PUBLIC ZONE (No Auth)                      │
│                                                               │
│  • GET /                (Home page)                          │
│  • GET /User/Login      (Login form)                         │
│  • POST /User/Login     (Process login)                      │
│  • GET /User/Create     (Registration form)                  │
│  • POST /User/Create    (Process registration)               │
│  • GET /User/Logout     (Destroy session)                    │
│  • GET /Ride/GetMapTile (Map tile proxy)                     │
│  • POST /Ride/ReverseGeocode (Geocoding API)                │
│                                                               │
├───────────────────────────────────────────────────────────────┤
│                  PROTECTED ZONE (Session Required)            │
│                                                               │
│  • GET /Ride/Map        [Protected] attribute                │
│  • GET /Ride/Index      [Protected] attribute                │
│  • POST /Ride/CreateRide  (session check in code)           │
│  • GET /Unicorn/*       [Protected] class-level             │
│  • POST /Unicorn/*      [Protected] class-level             │
│                                                               │
├───────────────────────────────────────────────────────────────┤
│                  AWS SERVICE ZONE (IAM Credentials)           │
│                                                               │
│  • Cognito Identity Pool → Temporary STS credentials         │
│  • Location Service      → Map tiles + geocoding             │
│                                                               │
└─────────────────────────────────────────────────────────────┘
```

## Data Flow

```
User Input → Browser → IIS → ASP.NET MVC Router
                                    │
                    ┌───────────────┼───────────────┐
                    ▼               ▼               ▼
              View Actions    API Actions    Map/Geo Actions
              (HTML resp)     (JSON resp)    (Binary/JSON)
                    │               │               │
                    ▼               ▼               ▼
              DefaultContext  DefaultContext  AWS Location
                    │               │          Service
                    ▼               ▼
              SQL Server       SQL Server
              (Read/Write)    (Read/Write)
```

## Cross-References

- [System Overview](../../architecture/system-overview.md)
- [Security Patterns](../../analysis/security-patterns.md)
- [Components](../../architecture/components.md)
