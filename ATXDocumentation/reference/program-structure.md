# Program Structure

## Solution Structure

```
wildrydes/
├── .gitignore
├── references/
│   ├── packages/                    (NuGet package DLLs - 22 packages)
│   └── reference assemblies/       (.NET Framework 4.8.1 reference assemblies)
└── sourceCode/
    ├── wildrydes.net.sln            (Visual Studio Solution)
    └── wildrydes.net/               (Main Web Project)
        ├── wildrydes.net.csproj     (Project file)
        ├── packages.config          (NuGet package references)
        ├── Web.config               (Application configuration)
        ├── Web.Debug.config         (Debug transform)
        ├── Web.Release.config       (Release transform)
        ├── Global.asax              (Application entry point markup)
        ├── Global.asax.cs           (Application startup code)
        ├── Dockerfile               (Container definition)
        ├── .dockerignore
        ├── favicon.ico
        ├── App_Data/
        │   └── save.txt
        ├── App_Start/
        │   ├── BundleConfig.cs      (JS/CSS bundle definitions)
        │   ├── FilterConfig.cs      (Global action filters)
        │   └── RouteConfig.cs       (URL routing configuration)
        ├── Content/
        │   ├── Site.css             (Custom styles)
        │   ├── bootstrap*.css       (Bootstrap framework)
        │   └── Images/              (Static images - 13 files)
        ├── Context/
        │   └── DefaultContext.cs    (Entity Framework DbContext)
        ├── Controllers/
        │   ├── HomeController.cs    (Landing page)
        │   ├── UserController.cs    (Auth: login/register/logout)
        │   ├── RideController.cs    (Rides, map, geocoding)
        │   └── UnicornController.cs (Unicorn CRUD)
        ├── Decorators/
        │   └── Protected.cs         (Auth filter attribute)
        ├── Migrations/
        │   ├── Configuration.cs     (Seed data)
        │   ├── 202508191721056_current-state.cs
        │   ├── 202508191721056_current-state.Designer.cs
        │   └── 202508191721056_current-state.resx
        ├── Models/
        │   ├── UserModel.cs         (User entity)
        │   ├── UnicornModel.cs      (Unicorn entity)
        │   └── RideModel.cs         (Ride entity + Location + Rating)
        ├── Properties/
        │   └── AssemblyInfo.cs      (Assembly metadata)
        ├── Scripts/
        │   ├── bootstrap*.js        (Bootstrap JS)
        │   ├── jquery-3.7.1*.js     (jQuery)
        │   ├── jquery.validate*.js  (Validation plugin)
        │   └── modernizr-2.8.3.js   (Feature detection)
        └── Views/
            ├── _ViewStart.cshtml    (Layout selection)
            ├── Web.config           (View-specific config)
            ├── Home/
            │   └── Index.cshtml     (Landing page)
            ├── Ride/
            │   ├── Index.cshtml     (Ride history)
            │   └── Map.cshtml       (Map + ride request)
            ├── Shared/
            │   ├── _Layout.cshtml   (Master layout)
            │   └── Error.cshtml     (Error page)
            ├── Unicorn/
            │   ├── Index.cshtml     (Unicorn list)
            │   ├── Create.cshtml    (Add unicorn form)
            │   └── Delete.cshtml    (Delete confirmation)
            └── User/
                ├── Create.cshtml    (Registration form)
                ├── Login.cshtml     (Login form)
                └── Logout.cshtml    (Logout page)
```

## Namespace Hierarchy

```
wildrydes.net
├── Controllers
│   ├── HomeController
│   ├── UserController
│   ├── RideController
│   └── UnicornController
├── Models
│   ├── UserModel
│   ├── UnicornModel
│   ├── RideModel
│   ├── Location
│   └── Rating (enum)
├── Context
│   └── DefaultContext
├── Decorators
│   └── Protected
└── Migrations
    └── Configuration
```

## Cross-References

- [Components](../architecture/components.md)
- [Modules](modules.md)
- [Interfaces](interfaces.md)
