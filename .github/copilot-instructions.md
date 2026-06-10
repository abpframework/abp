# ABP Framework – GitHub Copilot Instructions

> **Scope**: ABP Framework repository (abpframework/abp) — for developing ABP itself, not ABP-based applications.
>
> **Goal**: Enforce ABP module architecture best practices (DDD, layering, DB/ORM independence), maintain backward compatibility, ensure extensibility, and align with ABP contribution guidelines.

---

## Global Defaults

- Follow existing patterns in this repository first. Before generating new code, search for similar implementations and mirror their structure, naming, and conventions.
- Prefer minimal, focused diffs. Avoid drive-by refactors and formatting churn.
- Preserve public APIs. Avoid breaking changes unless explicitly requested and justified.
- Keep layers clean. Do not introduce forbidden dependencies between packages.

---

## Module / Package Architecture (Layering)

Use a layered module structure with explicit dependencies:

| Layer | Purpose | Allowed Dependencies |
|-------|---------|---------------------|
| `*.Domain.Shared` | Constants, enums, shared types safe for all layers and 3rd-party clients. MUST NOT contain entities, repositories, domain services, or business objects. | None |
| `*.Domain` | Entities/aggregate roots, repository interfaces, domain services. | Domain.Shared |
| `*.Application.Contracts` | Application service interfaces and DTOs. | Domain.Shared |
| `*.Application` | Application service implementations. | Domain, Application.Contracts |
| `*.EntityFrameworkCore` / `*.MongoDb` | ORM integration packages. MUST NOT depend on other layers. | Domain only |
| `*.HttpApi` | REST controllers. MUST depend ONLY on Application.Contracts (NOT Application). | Application.Contracts |
| `*.HttpApi.Client` | Remote client proxies. MUST depend ONLY on Application.Contracts. | Application.Contracts |
| `*.Web` | UI layer. MUST depend ONLY on HttpApi. | HttpApi |

### Dependency Direction
```
Web -> HttpApi -> Application.Contracts
Application -> Domain + Application.Contracts
Domain -> Domain.Shared
ORM integration -> Domain
```

Do not leak web concerns into application/domain.

---

## Domain Layer – Entities & Aggregate Roots

- Define entities in the domain layer.
- Entities must be valid at creation:
  - Provide a primary constructor that enforces invariants.
  - Always include a `protected` parameterless constructor for ORMs.
  - Always initialize sub-collections in the primary constructor.
  - Do NOT generate Guid keys inside constructors; accept `id` and generate using `IGuidGenerator` from the calling code.
- Make members `virtual` where appropriate (ORM/proxy compatibility).
- Protect consistency:
  - Use non-public setters (`private`/`protected`/`internal`) when needed.
  - Provide meaningful domain methods for state transitions.

### Aggregate Roots
- Always use a single `Id` property. Do NOT use composite keys.
- Prefer `Guid` keys for aggregate roots.
- Inherit from `AggregateRoot<TKey>` or audited base classes as required.
- Keep aggregates small. Avoid large sub-collections unless necessary.

### References
- Reference other aggregate roots by Id only.
- Do NOT add navigation properties to other aggregate roots.

---

## Repositories

- Define repository interfaces in the domain layer.
- Create one dedicated repository interface per aggregate root (e.g., `IProductRepository`).
- Public repository interfaces exposed by modules:
  - SHOULD inherit from `IBasicRepository<TEntity, TKey>` (or `IReadOnlyRepository<...>` when suitable).
  - SHOULD NOT expose `IQueryable` in the public contract.
  - Internal implementations MAY use `IRepository<TEntity, TKey>` and `IQueryable` as needed.
- Do NOT define repositories for non-aggregate-root entities.

### Method Conventions
- All methods async.
- Include optional `CancellationToken cancellationToken = default` in every method.
- For single-entity returning methods: include `bool includeDetails = true`.
- For list returning methods: include `bool includeDetails = false`.
- Do NOT return composite projection classes like `UserWithRoles`. Use `includeDetails` for eager-loading.
- Avoid projection-only view models from repositories by default; only allow when performance is critical.

---

## Domain Services

- Define domain services in the domain layer.
- Default: do NOT create interfaces for domain services unless necessary (mocking/multiple implementations).
- Naming: use `*Manager` suffix.

### Method Guidelines
- Focus on operations that enforce domain invariants and business rules.
- Query methods are acceptable when they encapsulate domain-specific lookup logic (e.g., normalized lookups, caching, complex resolution). Simple queries belong in repositories.
- Define methods that mutate state and enforce domain rules.
- Use specific, intention-revealing names (avoid generic `UpdateXAsync`).
- Accept valid domain objects as parameters; do NOT accept/return DTOs.
- On rule violations, throw `BusinessException` (or custom business exceptions).
- Use unique, namespaced error codes suitable for localization (e.g., `IssueTracking:ConcurrentOpenIssueLimit`).
- Do NOT depend on authenticated user logic; pass required values from application layer.

---

## Application Services

### Contracts
- Define one interface per application service in `*.Application.Contracts`.
- Interfaces must inherit from `IApplicationService`.
- Naming: `I*AppService`.
- Do NOT accept/return entities. Use DTOs and primitive parameters.

### Method Naming & Shapes
- All service methods async and end with `Async`.
- Do not repeat entity names in method names (use `GetAsync`, not `GetProductAsync`).

**Standard CRUD:**
```csharp
Task<ProductDto> GetAsync(Guid id);
Task<PagedResultDto<ProductDto>> GetListAsync(GetProductListInput input);
Task<ProductDto> CreateAsync(CreateProductInput input);
Task<ProductDto> UpdateAsync(Guid id, UpdateProductInput input);  // id NOT inside DTO
Task DeleteAsync(Guid id);
```

### DTO Usage (Inputs)
- Do not include unused properties.
- Do NOT share input DTOs between methods.
- Do NOT use inheritance between input DTOs (except rare abstract base DTO cases; be very cautious).

### Implementation
- Application layer must be independent of web.
- Implement interfaces in `*.Application`, name `ProductAppService` for `IProductAppService`.
- Inherit from `ApplicationService`.
- Make all public methods `virtual`.
- Avoid private helper methods; prefer `protected virtual` helpers for extensibility.

### Data Access
- Use dedicated repositories (e.g., `IProductRepository`).
- Do NOT put LINQ/SQL queries inside application service methods; repositories perform queries.

### Entity Mutation
- Load required entities from repositories.
- Mutate using domain methods.
- Call repository `UpdateAsync` after updates (do not assume change tracking).

### Files
- Do NOT use web types like `IFormFile` or `Stream` in application services.
- Controllers handle upload; pass `byte[]` (or similar) to application services.

### Cross-Service Calls
- Do NOT call other application services within the same module.
- For reuse, push logic into domain layer or extract shared helpers carefully.
- You MAY call other modules' application services only via their Application.Contracts.

---

## DTO Conventions

- Define DTOs in `*.Application.Contracts`.
- Prefer ABP base DTO types (`EntityDto<TKey>`, audited DTOs).
- For aggregate roots, prefer extensible DTO base types so extra properties can map.
- DTO properties: public getters/setters.

### Input DTO Validation
- Use data annotations.
- Reuse constants from Domain.Shared wherever possible.

### General Rules
- Avoid logic in DTOs; only implement `IValidatableObject` when necessary.
- Do NOT use `[Serializable]` attribute (BinaryFormatter is obsolete); ABP uses JSON serialization.

### Output DTO Strategy
- Prefer a Basic DTO and a Detailed DTO; avoid many variants.
- Detailed DTOs: include reference details as nested basic DTOs; avoid duplicating raw FK ids unnecessarily.

---

## EF Core Integration

- Define a separate DbContext interface + class per module.
- Do NOT rely on lazy loading; do NOT enable lazy loading.

### DbContext Interface
```csharp
[ConnectionStringName("ModuleName")]
public interface IModuleNameDbContext : IEfCoreDbContext
{
    DbSet<Product> Products { get; }  // No setters, aggregate roots only
}
```

### DbContext Class
```csharp
[ConnectionStringName("ModuleName")]
public class ModuleNameDbContext : AbpDbContext<ModuleNameDbContext>, IModuleNameDbContext
{
    public static string TablePrefix { get; set; } = ModuleNameConsts.DefaultDbTablePrefix;
    public static string? Schema { get; set; } = ModuleNameConsts.DefaultDbSchema;
    
    public DbSet<Product> Products { get; set; }
}
```

### Table Prefix/Schema
- Provide static `TablePrefix` and `Schema` defaulted from constants.
- Use short prefixes; `Abp` prefix reserved for ABP core modules.
- Default schema should be `null`.

### Model Mapping
- Do NOT configure entities directly inside `OnModelCreating`.
- Create `ModelBuilder` extension method `ConfigureX()` and call it.
- Call `b.ConfigureByConvention()` for each entity.

### Repository Implementations
- Inherit from `EfCoreRepository<TDbContextInterface, TEntity, TKey>`.
- Use DbContext interface as generic parameter.
- Pass cancellation tokens using `GetCancellationToken(cancellationToken)`.
- Implement `IncludeDetails(include)` extension per aggregate root with sub-collections.
- Override `WithDetailsAsync()` where needed.

---

## MongoDB Integration

- Define a separate MongoDbContext interface + class per module.

### MongoDbContext Interface
```csharp
[ConnectionStringName("ModuleName")]
public interface IModuleNameMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<Product> Products { get; }  // Aggregate roots only
}
```

### MongoDbContext Class
```csharp
public class ModuleNameMongoDbContext : AbpMongoDbContext, IModuleNameMongoDbContext
{
    public static string CollectionPrefix { get; set; } = ModuleNameConsts.DefaultDbTablePrefix;
}
```

### Mapping
- Do NOT configure directly inside `CreateModel`.
- Create `IMongoModelBuilder` extension method `ConfigureX()` and call it.

### Repository Implementations
- Inherit from `MongoDbRepository<TMongoDbContextInterface, TEntity, TKey>`.
- Pass cancellation tokens using `GetCancellationToken(cancellationToken)`.
- Ignore `includeDetails` for MongoDB in most cases (documents load sub-collections).
- Prefer `GetQueryableAsync()` to ensure ABP data filters are applied.

---

## ABP Module Classes

- Every package must have exactly one `AbpModule` class.
- Naming: `Abp[ModuleName][Layer]Module` (e.g., `AbpIdentityDomainModule`, `AbpIdentityApplicationModule`).
- Use `[DependsOn(typeof(...))]` to declare module dependencies explicitly.
- Override `ConfigureServices` for DI registration and configuration.
- Override `OnApplicationInitialization` sparingly; prefer `ConfigureServices` when possible.
- Each module must be usable standalone; avoid hidden cross-module coupling.

---

## Framework Extensibility

- All public and protected members should be `virtual` for inheritance-based extensibility.
- Prefer `protected virtual` over `private` for helper methods to allow overriding.
- Use `[Dependency(ReplaceServices = true)]` patterns for services intended to be replaceable.
- Provide extension points via interfaces and virtual methods.
- Document extension points with XML comments explaining intended usage.
- Consider providing `*Options` classes for configuration-based extensibility.

---

## Backward Compatibility

- Do NOT remove or rename public API members without a deprecation cycle.
- Use `[Obsolete("Message. Use X instead.")]` with clear migration guidance before removal.
- Maintain binary and source compatibility within major versions.
- Add new optional parameters with defaults; do not change existing method signatures.
- When adding new abstract members to base classes, provide default implementations if possible.
- Prefer adding new interfaces over modifying existing ones.

---

## Localization Resources

- Define localization resources in Domain.Shared.
- Resource class naming: `[ModuleName]Resource` (e.g., `IdentityResource`, `PermissionManagementResource`).
- JSON files under `/Localization/[ModuleName]/` directory.
- Use `LocalizableString.Create<TResource>("Key")` for localizable exceptions and messages.
- All user-facing strings must be localized; no hardcoded English text in code.
- Error codes should be namespaced: `ModuleName:ErrorCode` (e.g., `Identity:UserNameAlreadyExists`).

---

## Settings & Features

- Define settings in `*SettingDefinitionProvider` in Domain.Shared or Domain.
- Setting names must follow `Abp.[ModuleName].[SettingName]` convention.
- Define features in `*FeatureDefinitionProvider` in Domain.Shared.
- Feature names must follow `[ModuleName].[FeatureName]` convention.
- Use constants for setting/feature names; never hardcode strings.

---

## Permissions

- Define permissions in `*PermissionDefinitionProvider` in Application.Contracts.
- Permission names must follow `[ModuleName].[Permission]` convention.
- Use constants for permission names (e.g., `IdentityPermissions.Users.Create`).
- Group related permissions logically.

---

## Event Bus & Distributed Events

- Use `ILocalEventBus` for intra-module communication within the same process.
- Use `IDistributedEventBus` for cross-module or cross-service communication.
- Define Event Transfer Objects (ETOs) in Domain.Shared for distributed events.
- ETO naming: `[EntityName][Action]Eto` (e.g., `UserCreatedEto`, `OrderCompletedEto`).
- Event handlers belong in the Application layer.
- ETOs should be simple, serializable, and contain only primitive types or nested ETOs.

---

## Testing

- Unit tests: `*.Tests` projects for isolated logic testing with mocked dependencies.
- Integration tests: `*.EntityFrameworkCore.Tests` / `*.MongoDB.Tests` for repository and DB tests.
- Use `AbpIntegratedTest<TModule>` or `AbpApplicationTestBase<TModule>` base classes.
- Test modules should use `[DependsOn]` on the module under test.
- Use `Shouldly` assertions (ABP convention).
- Test both EF Core and MongoDB implementations when the module supports both.
- Include tests for permission checks, validation, and edge cases.
- Name test methods: `MethodName_Scenario_ExpectedResult` or `Should_ExpectedBehavior_When_Condition`.

---

## Contribution Discipline (PR / Issues / Tests)

- Before significant changes, align via GitHub issue/discussion.

### PRs
- Keep changes scoped and reviewable.
- Add/update unit/integration tests relevant to the change.
- Build and run tests for the impacted area when possible.

### Localization
- Prefer the `abp translate` workflow for adding missing translations (generate `abp-translation.json`, fill, apply, then PR).

---

## Review Checklist

- [ ] Layer dependencies respected (no forbidden references).
- [ ] No `IQueryable` leaking into public repository contracts.
- [ ] Entities maintain invariants; Guid id generation not inside constructors.
- [ ] Repositories follow async + CancellationToken + includeDetails conventions.
- [ ] No web types in application services.
- [ ] DTOs in contracts, validated, minimal, no logic.
- [ ] EF/Mongo integration follows context + mapping + repository patterns.
- [ ] Public members are `virtual` for extensibility.
- [ ] Backward compatibility maintained; no breaking changes without deprecation.
- [ ] Minimal diff; no unnecessary API surface expansion.
