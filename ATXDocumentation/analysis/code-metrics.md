# Code Metrics

## File Counts

| Category | Count |
|----------|-------|
| C# source files | 17 |
| Razor view files | 12 |
| Configuration files | 9 |
| Total source lines (including bundled libraries) | ~88,678 |
| Application C# code lines (estimated) | ~600 |

## Per-File Metrics (Application Code)

| File | Lines | Methods | Complexity |
|------|-------|---------|-----------|
| RideController.cs | 175 | 6 | Medium-High |
| UserController.cs | 100 | 5 | Low-Medium |
| UnicornController.cs | 80 | 5 | Low |
| HomeController.cs | 12 | 1 | Low |
| RideModel.cs | 57 | 0 | Low |
| UnicornModel.cs | 27 | 0 | Low |
| UserModel.cs | 25 | 0 | Low |
| DefaultContext.cs | 20 | 1 | Low |
| Protected.cs | 22 | 2 | Low |
| Configuration.cs | 60 | 2 | Low |
| Global.asax.cs | 18 | 1 | Low |
| BundleConfig.cs | 28 | 1 | Low |
| FilterConfig.cs | 11 | 1 | Low |
| RouteConfig.cs | 28 | 1 | Low |

## Method Count by Controller

| Controller | Actions | Helper Methods | Total |
|-----------|---------|---------------|-------|
| HomeController | 1 | 0 | 1 |
| UserController | 4 (Login GET/POST, Create GET/POST, Logout) | 1 (GetHash) | 5 |
| RideController | 5 (Map, Index, CreateRide, ReverseGeocode, GetMapTile) | 1 (Dispose) | 6 |
| UnicornController | 4 (Index, Create GET/POST, Delete GET/POST) | 1 (Dispose) | 5 |

## Dependency Counts

| Category | Count |
|----------|-------|
| NuGet packages | 28 |
| CDN dependencies | 2 (Leaflet.js, Leaflet CSS) |
| AWS services | 3 (Cognito, Location, STS) |
| Database tables | 3 |

## Test Coverage

- **Unit tests:** 0
- **Integration tests:** 0
- **Total test coverage:** 0%

## Cross-References

- [Complexity Analysis](complexity-analysis.md)
- [Dependency Analysis](dependency-analysis.md)
