# Validation Criteria

## Functional Validation

### Authentication System
- [ ] User registration creates account with hashed password
- [ ] Login with valid credentials sets authentication state
- [ ] Login with invalid credentials shows error message
- [ ] Protected routes redirect unauthenticated users to login
- [ ] Logout destroys session and redirects to home
- [ ] Return URL preserved through login flow

### Ride Management
- [ ] Authenticated users can access map view
- [ ] CreateRide endpoint creates ride with random unicorn assignment
- [ ] Ride history shows rides for current user only
- [ ] ReverseGeocode returns address for valid coordinates
- [ ] Map tiles load correctly from AWS Location Service

### Unicorn Management
- [ ] Authenticated users can list all unicorns
- [ ] New unicorns can be created with required fields
- [ ] Unicorns can be deleted with confirmation
- [ ] Unauthenticated access redirects to login

### Data Integrity
- [ ] Users table stores email and hashed password
- [ ] Unicorns table stores name, color, description, rating
- [ ] Rides table stores all location data and FK relationships
- [ ] Cascade delete removes rides when user/unicorn deleted
- [ ] Seed data creates 3 unicorns and 1 user

## Non-Functional Validation

### Performance
- [ ] Map tile requests complete with acceptable latency
- [ ] Database queries use appropriate includes/eager loading
- [ ] No N+1 query patterns in ride history

### Security
- [ ] Passwords are not stored in plaintext
- [ ] Session state is properly managed
- [ ] CSRF tokens validated on form submissions
- [ ] Database queries are parameterized (no SQL injection)

### Deployment
- [ ] Application builds without errors
- [ ] Docker image builds successfully
- [ ] Database migrations apply cleanly
- [ ] AWS configuration is externalized (not hardcoded)

## Migration-Specific Validation

### .NET 8 Migration Criteria
- [ ] Application compiles on .NET 8
- [ ] All controllers return correct responses
- [ ] Entity Framework Core migrations equivalent to EF6 schema
- [ ] Session/authentication works with new middleware
- [ ] AWS SDK calls function correctly
- [ ] Views render correctly with updated tag helpers
- [ ] Bundling/static file serving works
- [ ] Docker image builds on Linux base

### Regression Checks
- [ ] All existing URLs return same status codes
- [ ] JSON API responses maintain same structure
- [ ] Form submissions process identically
- [ ] Database schema produces equivalent data

## Cross-References

- [Test Specifications](test-specifications.md)
- [Component Order](component-order.md)
- [Business Logic](../behavior/business-logic.md)
