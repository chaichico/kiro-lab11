# Project Overview - Wild Rydes

## Application Summary

**Name:** Wild Rydes  
**Type:** Web Application (Server-rendered MVC)  
**Domain:** Ride-sharing service (unicorn-themed)  
**Platform:** .NET Framework 4.8.1 / ASP.NET MVC 5  
**Database:** SQL Server LocalDB (Entity Framework 6.5.1 Code-First)  
**Deployment:** Docker (Windows Server Core LTSC 2019)

## Technology Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| Runtime | .NET Framework | 4.8.1 |
| Web Framework | ASP.NET MVC | 5.3.0 |
| ORM | Entity Framework | 6.5.1 |
| Database | SQL Server LocalDB | - |
| Frontend CSS | Bootstrap | 5.3.7 |
| Frontend JS | jQuery | 3.7.1 |
| Maps | Leaflet.js | 1.7.1 (CDN) |
| JSON | Newtonsoft.Json | 13.0.3 |
| AWS SDK Core | AWSSDK.Core | 4.0.0.17 |
| AWS Cognito | AWSSDK.CognitoIdentity | 4.0.0.15 |
| AWS Location | AWSSDK.LocationService | 4.0.1 |
| Container | Windows Server Core | LTSC 2019 |

## Functional Summary

Wild Rydes allows users to:
1. Register and authenticate with email/password
2. View an interactive map (AWS Location Service)
3. Request unicorn rides by selecting pickup/destination locations
4. View ride history
5. Manage unicorn fleet (CRUD operations)

## Source Code Statistics

- **C# source files:** 17
- **Razor view files:** 12
- **Total source lines:** ~88,678 (including bundled JS/CSS libraries)
- **Controllers:** 4 (Home, User, Ride, Unicorn)
- **Models:** 3 (User, Unicorn, Ride) + 2 supporting types (Location, Rating)
- **Test files:** 0

## AWS Services Used

- **Amazon Cognito Identity** - Temporary credentials for Location Service access
- **Amazon Location Service** - Map tiles and reverse geocoding
- **AWS STS** - Token service (implicit via Cognito)
