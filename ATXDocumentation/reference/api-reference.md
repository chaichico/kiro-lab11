# API Reference

## MVC View Endpoints (HTML responses)

### HomeController

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/` | No | Landing page |

### UserController

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/User/Login` | No | Login form |
| POST | `/User/Login` | No | Process login (CSRF protected) |
| GET | `/User/Create` | No | Registration form |
| POST | `/User/Create` | No | Process registration (CSRF protected) |
| GET | `/User/Logout` | No | Destroy session, redirect home |

### RideController

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/Ride/Map` | Yes | Interactive map view |
| GET | `/Ride/Index` | Yes | Ride history list |

### UnicornController

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/Unicorn` | Yes | Unicorn fleet list |
| GET | `/Unicorn/Create` | Yes | Create unicorn form |
| POST | `/Unicorn/Create` | Yes | Save new unicorn (CSRF protected) |
| GET | `/Unicorn/Delete/{id}` | Yes | Delete confirmation page |
| POST | `/Unicorn/Delete/{id}` | Yes | Confirm deletion (CSRF protected) |

## JSON API Endpoints

### POST /Ride/CreateRide

Create a new ride request.

**Authentication:** Session-based (checked internally, no `[Protected]` attribute)

**Parameters (form-encoded):**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| pickupLat | double | Yes | Pickup latitude |
| pickupLng | double | Yes | Pickup longitude |
| pickupAddress | string | Yes | Pickup address text |
| destLat | double | Yes | Destination latitude |
| destLng | double | Yes | Destination longitude |
| destAddress | string | Yes | Destination address text |
| passengers | int | Yes | Number of passengers |

**Success Response:**
```json
{
  "success": true,
  "rideId": "guid-string",
  "unicornName": "Bucephalus",
  "unicornDesc": "Description...",
  "unicornColor": "Gold"
}
```

**Error Response:**
```json
{
  "success": false,
  "error": "User not logged in"
}
```

### POST /Ride/ReverseGeocode

Convert geographic coordinates to a street address.

**Authentication:** None

**Parameters (form-encoded):**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| lat | double | Yes | Latitude |
| lng | double | Yes | Longitude |

**Success Response:**
```json
{
  "success": true,
  "address": "123 Main St, City, State"
}
```

**Fallback Response:**
```json
{
  "success": false,
  "address": "40.7128, -74.0060",
  "error": "Access denied to AWS Location Service"
}
```

### GET /Ride/GetMapTile/{z}/{x}/{y}

Retrieve a map tile image from AWS Location Service.

**Authentication:** None

**Path Parameters:**
| Parameter | Type | Constraints | Description |
|-----------|------|-------------|-------------|
| z | int | `\d+` | Zoom level |
| x | int | `\d+` | X tile coordinate |
| y | int | `\d+` | Y tile coordinate |

**Success Response:** `image/png` binary data with `Cache-Control: public, max-age=3600`

**Error Responses:**
- `403 Forbidden` - AWS access denied
- `404 Not Found` - Other errors

## Route Configuration

Custom routes defined in `RouteConfig.cs`:
1. `Ride/TestImage/{z}/{x}/{y}` → TestImage action (unused)
2. `Ride/GetMapTile/{z}/{x}/{y}` → GetMapTile action
3. `{controller}/{action}/{id}` → Default convention route

## Cross-References

- [Interfaces](interfaces.md)
- [Business Logic](../behavior/business-logic.md)
- [Workflows](../behavior/workflows.md)
