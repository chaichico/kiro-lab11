# Technical Debt Assessment

## Summary Score

| Category | Severity | Count |
|----------|----------|-------|
| High | EOL/deprecated runtimes and frameworks | 3 |
| High | Security vulnerabilities | 4 |
| Medium | Outdated dependencies | 4 |
| Medium | Architectural issues | 4 |
| Low | Code quality | 3 |

## Detailed Findings

### High Severity - EOL/Deprecated Runtimes and Frameworks

1. **.NET Framework 4.8.1** - Maintenance-only mode, Windows-only, no cross-platform support
2. **ASP.NET MVC 5.3.0** - Legacy framework, no further feature development
3. **Entity Framework 6.5.1** - Superseded by EF Core, maintenance-only

### High Severity - Security Vulnerabilities

1. **Unsalted SHA-256 password hashing** - Vulnerable to rainbow table and precomputation attacks
2. **Open redirect** - Login redirects to unvalidated user-supplied URL
3. **Missing CSRF on API endpoints** - CreateRide and ReverseGeocode lack anti-forgery protection
4. **Information disclosure** - Exception messages returned to clients

### Medium Severity - Outdated Dependencies

1. **Modernizr 2.8.3** (2014) - Unnecessary for modern browsers
2. **WebGrease 1.6.0** - Obsolete bundling library
3. **Antlr 3.5.0.2** - Very old parser (Antlr 4 is current)
4. **Leaflet.js 1.7.1** - Outdated (1.9.x is current)

### Medium Severity - Architectural Issues

1. **No dependency injection** - Controllers create dependencies directly
2. **Windows-only Docker** - ~5GB images, limited orchestration
3. **Mixed API patterns** - JSON and HTML from same controllers without clear separation
4. **SQL Server LocalDB** - Development-only database in committed config

### Low Severity - Code Quality

1. **No test coverage** - Zero tests, no test project
2. **Duplicated code** - Password hashing method in two locations
3. **Inconsistent auth enforcement** - Mix of attribute and manual session checks

## Quantified Impact

- **Security exposure:** 4 exploitable vulnerabilities in production
- **Platform limitation:** Cannot deploy on Linux/ARM, restricted to Windows Server
- **Maintenance cost:** All features must be hand-built (no DI, no Identity framework)
- **Quality risk:** 0% test coverage means any change is high-risk

## Cross-References

- [Technical Debt Report](../technical-debt-report.md)
- [Outdated Components](../technical-debt/outdated-components.md)
- [Security Patterns](security-patterns.md)
- [Remediation Plan](../technical-debt/remediation-plan.md)
