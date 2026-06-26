# Test Specifications

## Overview

The current application has zero test coverage. The following test specifications define the cases needed to validate functionality during and after migration.

## Unit Tests

### UserController Tests

| Test Case | Input | Expected Output |
|-----------|-------|-----------------|
| Login_ValidCredentials_SetsSession | email="user@unicornrides.aws", password="Passw0rd" | Session["user"] = email, redirect |
| Login_InvalidPassword_ShowsError | email="user@unicornrides.aws", password="wrong" | ViewBag.Error set, returns view |
| Login_NonexistentUser_ShowsError | email="notfound@test.com", password="any" | ViewBag.Error set, returns view |
| Login_EmptyFields_ShowsError | email="", password="" | ViewBag.Error set, returns view |
| Create_ValidUser_SavesHashedPassword | email="new@test.com", password="Test123" | User saved with SHA-256 hash |
| Create_InvalidModel_ReturnsView | email="" (required field missing) | Returns view with model errors |
| Logout_AbandonSession | (authenticated session) | Session abandoned, redirect to Home |
| GetHash_ReturnsConsistentHash | "Passw0rd" | "b03ddf3ca2e714a6548e7495e2a03f5e824eaac9837cd7f159c67b90fb4b7342" |

### RideController Tests

| Test Case | Input | Expected Output |
|-----------|-------|-----------------|
| CreateRide_Authenticated_CreatesRide | valid params + session | JSON {success: true, rideId: guid} |
| CreateRide_NoSession_ReturnsError | valid params, no session | JSON {success: false, error: "User not logged in"} |
| CreateRide_UserNotFound_ReturnsError | session with unknown email | JSON {success: false, error: "User not found"} |
| Map_Authenticated_ReturnsView | authenticated session | View result |
| Map_Unauthenticated_Redirects | no session | Redirect to /User/Login |
| Index_Authenticated_ReturnsUserRides | authenticated, has rides | View with ride list |
| ReverseGeocode_Success_ReturnsAddress | lat=40.7, lng=-74.0 | JSON {success: true, address: "..."} |
| ReverseGeocode_NoResults_ReturnsFallback | lat=0, lng=0 | JSON {success: false, address: "0.0000, 0.0000"} |
| GetMapTile_ValidCoords_ReturnsPng | z=10, x=300, y=400 | image/png content |
| GetMapTile_AccessDenied_Returns403 | (AWS access denied) | HTTP 403 |

### UnicornController Tests

| Test Case | Input | Expected Output |
|-----------|-------|-----------------|
| Index_Authenticated_ReturnsAllUnicorns | authenticated session | View with unicorn list |
| Index_Unauthenticated_Redirects | no session | Redirect to /User/Login |
| Create_ValidModel_SavesUnicorn | Name="Test", Color="Blue", Description="Desc" | Unicorn saved, redirect to Index |
| Create_InvalidModel_ReturnsView | Name="" (required missing) | Returns view with errors |
| Delete_ValidId_ShowsConfirmation | id=existing-guid | View with unicorn details |
| Delete_NullId_Returns400 | id=null | HTTP 400 |
| Delete_NotFound_Returns404 | id=non-existing-guid | HTTP 404 |
| DeleteConfirmed_RemovesUnicorn | id=existing-guid | Unicorn removed, redirect |

### Protected Filter Tests

| Test Case | Input | Expected Output |
|-----------|-------|-----------------|
| OnActionExecuting_NoSession_Redirects | Session["user"] = null | Redirect to /User/Login?or={url} |
| OnActionExecuting_WithSession_Allows | Session["user"] = "test@test.com" | No redirect, action proceeds |

## Integration Tests

| Test Case | Description |
|-----------|-------------|
| FullRegistrationFlow | Register → Login → Access protected page |
| RideRequestFlow | Login → Map → CreateRide → View in Index |
| UnicornCRUD | Login → Create unicorn → Verify in list → Delete |
| DatabaseSeed | Run migrations → Verify 3 unicorns and 1 user exist |

## Cross-References

- [Validation Criteria](validation-criteria.md)
- [Component Order](component-order.md)
- [Business Logic](../behavior/business-logic.md)
