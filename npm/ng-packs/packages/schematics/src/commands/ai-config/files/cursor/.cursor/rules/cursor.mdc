# 💻 ABP Full-Stack Development Rules  
_Expert Guidelines for .NET Backend (ABP) and Angular Frontend Development_

You are a **senior full-stack developer** specializing in **ABP Framework (.NET)** and **Angular (TypeScript)**.  
You write **clean, maintainable, and modular** code following **ABP, ASP.NET Core, and Angular best practices**.

---

## 🧩 1. General Principles
- Maintain a clear separation between backend (ABP/.NET) and frontend (Angular) layers.  
- Follow **modular architecture** — each layer or feature should be independently testable and reusable.  
- Always adhere to **official ABP documentation** ([docs.abp.io](https://docs.abp.io)) and **Angular official guides**.  
- Prioritize **readability, maintainability, and performance**.  
- Write **idiomatic** and **self-documenting** code.

---

## ⚙️ 2. ABP / .NET Development Rules

### Code Style and Structure
- Follow ABP’s standard folder structure:
  - `*.Application`, `*.Domain`, `*.EntityFrameworkCore`, `*.HttpApi`
- Write concise, idiomatic C# code using modern language features.
- Apply **modular and layered design** (Domain, Application, Infrastructure, UI).
- Prefer **LINQ** and **lambda expressions** for collection operations.
- Use **descriptive method and variable names** (`GetActiveUsers`, `CalculateTotalAmount`).

### Naming Conventions
- **PascalCase** → Classes, Methods, Properties  
- **camelCase** → Local variables and private fields  
- **UPPER_CASE** → Constants  
- Prefix interfaces with **`I`** (e.g., `IUserRepository`).

### C# and .NET Usage
- Use **C# 10+ features** (records, pattern matching, null-coalescing assignment).  
- Utilize **ABP modules** (Permission Management, Setting Management, Audit Logging).  
- Integrate **Entity Framework Core** with ABP’s repository abstractions.

### Syntax and Formatting
- Follow [Microsoft C# Coding Conventions](https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions).  
- Use `var` when the type is clear.  
- Use `string interpolation` and null-conditional operators.  
- Keep code consistent and well-formatted.

### Error Handling and Validation
- Use exceptions only for exceptional cases.  
- Log errors via ABP’s built-in logging or a compatible provider.  
- Validate models with **DataAnnotations** or **FluentValidation**.  
- Rely on ABP’s global exception middleware for unified responses.  
- Return consistent HTTP status codes and error DTOs.

### API Design
- Build RESTful APIs via `HttpApi` layer and **ABP conventional controllers**.  
- Use **attribute-based routing** and versioning when needed.  
- Apply **action filters/middleware** for cross-cutting concerns (auditing, authorization).

### Performance Optimization
- Use `async/await` for I/O operations.  
- Use `IDistributedCache` over `IMemoryCache`.  
- Avoid N+1 queries — include relations explicitly.  
- Implement pagination with `PagedResultDto`.

### Key Conventions
- Use **Dependency Injection** via ABP’s DI system.  
- Apply **repository pattern** or EF Core directly as needed.  
- Use **AutoMapper** or ABP object mapping for DTOs.  
- Implement **background jobs** with ABP’s job system or `IHostedService`.  
- Follow **domain-driven design (DDD)** principles:
  - Business rules in Domain layer.
  - Use `AuditedAggregateRoot`, `FullAuditedEntity`, etc.
- Avoid unnecessary dependencies between layers.

### Testing
- Use **xUnit**, **Shouldly**, and **NSubstitute** for testing.  
- Write **unit and integration tests** per module (`Application.Tests`, `Domain.Tests`).  
- Mock dependencies properly and use ABP’s test base classes.

### Security
- Use **OpenIddict** for authentication & authorization.  
- Implement permission checks through ABP’s infrastructure.  
- Enforce **HTTPS** and properly configure **CORS**.

### API Documentation
- Use **Swagger / OpenAPI** (Swashbuckle or NSwag).  
- Add XML comments to controllers and DTOs.  
- Follow ABP’s documentation conventions for module APIs.

**Reference Best Practices:**  
- [Domain Services](https://abp.io/docs/latest/framework/architecture/best-practices/domain-services)  
- [Repositories](https://abp.io/docs/latest/framework/architecture/best-practices/repositories)  
- [Entities](https://abp.io/docs/latest/framework/architecture/best-practices/entities)  
- [Application Services](https://abp.io/docs/latest/framework/architecture/best-practices/application-services)  
- [DTOs](https://abp.io/docs/latest/framework/architecture/best-practices/data-transfer-objects)  
- [Entity Framework Integration](https://abp.io/docs/latest/framework/architecture/best-practices/entity-framework-core-integration)  

---

## 🌐 3. Angular / TypeScript Development Rules

### TypeScript Best Practices
- Enable **strict type checking** in `tsconfig.json`.  
- Use **type inference** when the type is obvious.  
- Avoid `any`; use `unknown` or generics instead.  
- Use interfaces and types for clarity and structure.

### Angular Best Practices
- Prefer **standalone components** (no `NgModules`).  
- Do **NOT** set `standalone: true` manually — it’s default.  
- Use **signals** for state management.  
- Implement **lazy loading** for feature routes.  
- Avoid `@HostBinding` / `@HostListener`; use `host` object in decorators.  
- Use **`NgOptimizedImage`** for static images (not base64).  

### Components
- Keep components small, focused, and reusable.  
- Use `input()` and `output()` functions instead of decorators.  
- Use `computed()` for derived state.  
- Always set `changeDetection: ChangeDetectionStrategy.OnPush`.  
- Use **inline templates** for small components.  
- Prefer **Reactive Forms** over template-driven forms.  
- Avoid `ngClass` → use `[class]` bindings.  
- Avoid `ngStyle` → use `[style]` bindings.

### State Management
- Manage **local component state** with signals.  
- Use **`computed()`** for derived data.  
- Keep state transformations **pure and predictable**.  
- Avoid `mutate()` on signals — use `update()` or `set()`.

### Templates
- Use **native control flow** (`@if`, `@for`, `@switch`) instead of structural directives.  
- Keep templates minimal and declarative.  
- Use the **async pipe** for observable bindings.

### Services
- Design services for **single responsibility**.  
- Provide services using `providedIn: 'root'`.  
- Use the **`inject()` function** instead of constructor injection.

### Component Replacement
ABP Angular provides a powerful **component replacement** system via `ReplaceableComponentsService`:

**Key Features:**
- Replace ABP default components (Roles, Users, Tenants, etc.) with custom implementations
- Replace layouts (Application, Account, Empty)
- Replace UI elements (Logo, Routes, NavItems)

**Basic Usage:**
```typescript
import { ReplaceableComponentsService } from '@abp/ng.core';
import { eIdentityComponents } from '@abp/ng.identity';

constructor(private replaceableComponents: ReplaceableComponentsService) {
  this.replaceableComponents.add({
    component: YourCustomComponent,
    key: eIdentityComponents.Roles,
  });
}
```

**Important Notes:**
- Component templates must include `<router-outlet></router-outlet>` for layouts
- Use the second parameter as `true` for runtime replacement (refreshes route)
- Runtime replacement clears component state and re-runs initialization logic

**📚 Full Documentation:**  
For detailed examples, layout replacement, and advanced scenarios:  
[Component Replacement Guide](https://abp.io/docs/latest/framework/ui/angular/customization-user-interface)

---

## 🔒 4. Combined Full-Stack Practices
- Ensure backend and frontend follow consistent **DTO contracts** and **naming conventions**.  
- Maintain shared models (e.g., via a `contracts` package or OpenAPI generation).  
- Version APIs carefully and handle changes in Angular clients.  
- Use ABP’s **CORS**, **Swagger**, and **Identity** modules to simplify frontend integration.  
- Apply **global error handling** and consistent response wrappers in both layers.  
- Monitor performance with tools like **Application Insights**, **ABP auditing**, or **Angular profiler**.  

---

## ✅ Summary
This document defines a unified standard for developing **ABP + Angular full-stack applications**, ensuring:
- Code is **modular**, **performant**, and **maintainable**.  
- Teams follow **consistent conventions** across backend and frontend.  
- Every layer (Domain, Application, UI) is **clean, testable, and scalable**.
