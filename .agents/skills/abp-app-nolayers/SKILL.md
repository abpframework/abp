---
name: abp-app-nolayers
description: ABP Single-Layer (No-Layers / nolayers) application template - single project structure, feature-based file organization, no separate Domain/Application.Contracts projects. Use when working with the single-layer web application template or when the project has no layered separation.
---

# ABP Single-Layer Application Template

> **Docs**: https://abp.io/docs/latest/solution-templates/single-layer-web-application

## Solution Structure

Single project containing everything:

```
MyProject/
├── src/
│   └── MyProject/
│       ├── Data/              # DbContext, migrations
│       ├── Entities/          # Domain entities
│       ├── Services/          # Application services + DTOs
│       ├── Pages/             # Razor pages / Blazor components
│       └── MyProjectModule.cs
└── test/
    └── MyProject.Tests/
```

## Key Differences from Layered

| Layered Template | Single-Layer Template |
|------------------|----------------------|
| DTOs in Application.Contracts | DTOs in Services folder (same project) |
| Repository interfaces in Domain | Use generic `IRepository<T, TKey>` directly |
| Separate Domain.Shared for constants | Constants in same project |
| Multiple module classes | Single module class |

## File Organization

Group related files by feature:

```
Services/
├── Books/
│   ├── BookAppService.cs
│   ├── BookDto.cs
│   ├── CreateBookDto.cs
│   └── IBookAppService.cs
└── Authors/
    ├── AuthorAppService.cs
    └── ...
```

## Simplified Entity (Still keep invariants)

Single-layer templates are structurally simpler, but you may still have real business invariants.

- For **trivial CRUD** entities, public setters can be acceptable.
- For **non-trivial business rules**, still prefer encapsulation (private setters + methods) to prevent invalid states.

```csharp
public class Book : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }  // OK for trivial CRUD only
    public decimal Price { get; set; }
}
```

## No Custom Repository Needed

Use generic repository directly - no need to define custom interfaces:

```csharp
public class BookAppService : ApplicationService
{
    private readonly IRepository<Book, Guid> _bookRepository;

    // Generic repository is sufficient for single-layer apps
}
```
