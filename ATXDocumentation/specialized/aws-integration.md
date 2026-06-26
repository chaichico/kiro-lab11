# AWS Integration

## Services Used

### Amazon Cognito Identity
- **Purpose:** Obtain temporary AWS credentials for accessing Location Service
- **SDK Package:** AWSSDK.CognitoIdentity 4.0.0.15
- **Usage:** `CognitoAWSCredentials(identityPoolId, region)` in RideController constructor
- **Configuration:** `AWS.CognitoIdentityPoolId` in Web.config
- **Flow:** Unauthenticated identity pool access → temporary STS credentials

### Amazon Location Service - Maps
- **Purpose:** Serve map tile imagery for the Leaflet.js map UI
- **SDK Package:** AWSSDK.LocationService 4.0.1
- **Usage:** `GetMapTileAsync(request)` in RideController.GetMapTile
- **Configuration:** `AWS.LocationService.MapName` in Web.config
- **Response:** PNG image blob, served with 1-hour public cache

### Amazon Location Service - Places
- **Purpose:** Reverse geocoding (coordinates → address)
- **SDK Package:** AWSSDK.LocationService 4.0.1
- **Usage:** `SearchPlaceIndexForPositionAsync(request)` in RideController.ReverseGeocode
- **Configuration:** `AWS.LocationService.PlaceIndexName` in Web.config
- **Input:** [longitude, latitude] (AWS format, note order)
- **Output:** Place label (human-readable address)

### AWS Security Token Service (STS)
- **Purpose:** Implicit credential exchange via Cognito
- **SDK Package:** AWSSDK.SecurityToken 4.0.1.6
- **Usage:** Internal to CognitoAWSCredentials (automatic token refresh)

## Integration Architecture

```
Browser Request
      │
      ▼
RideController (constructor)
      │
      ├── Creates CognitoAWSCredentials(poolId, region)
      │       └── Internally calls STS AssumeRoleWithWebIdentity
      │
      └── Creates AmazonLocationServiceClient(credentials, region)
              │
              ├── GetMapTile(mapName, z, x, y)
              │       └── Returns PNG blob
              │
              └── SearchPlaceIndexForPosition(indexName, [lng, lat])
                      └── Returns Place label
```

## Configuration

All AWS configuration is externalized in Web.config `<appSettings>`:
```xml
<add key="AWS.Region" value="us-east-1" />
<add key="AWS.CognitoIdentityPoolId" value="REPLACE_WITH_COGNITO_ID" />
<add key="AWS.LocationService.MapName" value="REPLACE_WITH_MAP_NAME" />
<add key="AWS.LocationService.PlaceIndexName" value="REPLACE_WITH_INDEX_NAME" />
```

**Note:** All values are placeholders. Actual values must be provided for the application to function.

## AWS SDK Version

The application uses AWS SDK for .NET v4 (latest major version):
- Core: 4.0.0.17
- CognitoIdentity: 4.0.0.15
- LocationService: 4.0.1
- SecurityToken: 4.0.1.6

These are current versions and do not require upgrade.

## Error Handling for AWS Calls

| Operation | Error | Response |
|-----------|-------|----------|
| GetMapTile | AccessDeniedException | HTTP 403 |
| GetMapTile | General exception | HTTP 404 |
| ReverseGeocode | AccessDeniedException | JSON with error message |
| ReverseGeocode | General exception | JSON with coordinates as fallback |

## Security Model

- Server-side credential management (credentials never exposed to browser)
- Cognito Identity Pool provides unauthenticated access (guest role)
- Map tiles proxied through application (not direct browser-to-AWS)
- No IAM user/role keys in source code

## Cross-References

- [System Overview](../architecture/system-overview.md)
- [Components](../architecture/components.md)
- [Security Patterns](../analysis/security-patterns.md)
