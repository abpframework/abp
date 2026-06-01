# App Services vs Domain Services: Deep Dive into Two Core Service Types in ABP Framework

In ABP's layered architecture, we frequently encounter two types of services that appear similar but serve distinctly different purposes: Application Services and Domain Services. Understanding the differences between them is crucial for building clear and maintainable enterprise applications.

## Architectural Positioning

In ABP's layered architecture:

- **Application Services** reside in the application layer and are responsible for coordinating use case execution
- **Domain Services** reside in the domain layer and are responsible for implementing core business logic

This layered design follows Domain-Driven Design (DDD) principles, ensuring clear separation of business logic and system maintainability.

## Application Services: Use Case Orchestrators

### Core Responsibilities

Application Services are stateless services primarily used to implement application use cases. They act as a bridge between the presentation layer and domain layer, responsible for:

- **Parameter Validation**: Input validation is automatically handled by ABP using data annotations
- **Authorization**: Checking user permissions and access control using `[Authorize]` attribute or manual authorization checks via `IAuthorizationService`
- **Transaction Management**: Methods automatically run as Unit of Work (transactional by default)
- **Use Case Orchestration**: Organizing and coordinating multiple domain objects to complete specific business use cases
- **Data Transformation**: Handling conversion between DTOs and domain objects using ObjectMapper

### Design Principles

1. **DTO Boundaries**: Application service methods should only accept and return DTOs, never directly expose domain entities
2. **Use Case Oriented**: Each method should correspond to a clear user use case
3. **Thin Layer Design**: Avoid implementing complex business logic in application services

### Typical Execution Flow

A standard application service method typically follows this pattern:

```csharp
[Authorize(BookPermissions.Create)] // Declarative authorization
public virtual async Task<BookDto> CreateBookAsync(CreateBookDto input) // input is automatically validated
{
    // Get related data
    var author = await _authorRepository.GetAsync(input.AuthorId);
    
    // Call domain service to execute business logic (if needed)
    // You can also use the entity constructor directly if no complex business logic is required
    var book = await _bookManager.CreateAsync(input.Title, author, input.Price);
    
    // Persist changes
    await _bookRepository.InsertAsync(book);
    
    // Return DTO
    return ObjectMapper.Map<Book, BookDto>(book);
}
```

### Integration Services: Special kind of Application Service

It's worth mentioning that ABP also provides a special type of application service—Integration Services. They are application services marked with the `[IntegrationService]` attribute, designed for inter-module or inter-microservice communication.

We have a community article dedicated to integration services: [Integration Services Explained — What they are, when to use them, and how they behave](https://abp.io/community/articles/integration-services-explained-what-they-are-when-to-use-lienmsy8)

## Domain Services: Guardians of Business Logic

### Core Responsibilities

Domain Services implement core business logic and are particularly needed when:

- **Core domain logic depends on services**: You need to implement logic that requires repositories or other external services
- **Logic spans multiple aggregates**: The business logic is related to more than one aggregate/entity and doesn't properly fit in any single aggregate
- **Complex business rules**: Complex domain rules that don't naturally belong in a single entity

### Design Principles

1. **Domain Object Interaction**: Method parameters and return values should be domain objects (entities, value objects), never DTOs
2. **Business Logic Focus**: Focus on implementing pure business rules
3. **Stateless Design**: Maintain the stateless nature of services
4. **State-Changing Operations Only**: Domain services should only define methods that mutate data, not query methods
5. **No Authorization Logic**: Domain services should not perform authorization checks or depend on current user context
6. **Specific Method Names**: Use descriptive, business-meaningful method names (e.g., `AssignToAsync`) instead of generic names (e.g., `UpdateAsync`)

### Implementation Example

```csharp
public class IssueManager : DomainService
{
    private readonly IRepository<Issue, Guid> _issueRepository;
    
    public virtual async Task AssignToAsync(Issue issue, Guid userId)
    {
        // Business rule: Check user's unfinished task count
        var openIssueCount = await _issueRepository.GetCountAsync(i => i.AssignedUserId == userId && !i.IsClosed);
            
        if (openIssueCount >= 3)
        {
            throw new BusinessException("IssueTracking:ConcurrentOpenIssueLimit");
        }
        
        // Execute assignment logic
        issue.AssignedUserId = userId;
        issue.AssignedDate = Clock.Now;
    }
}
```

## Key Differences Comparison

| Dimension | Application Services | Domain Services |
|-----------|---------------------|-----------------|
| **Layer Position** | Application Layer | Domain Layer |
| **Primary Responsibility** | Use Case Orchestration | Business Logic Implementation |
| **Data Interaction** | DTOs | Domain Objects |
| **Callers** | Presentation Layer/Client Applications | Application Services/Other Domain Services |
| **Authorization** | Responsible for permission checks | No authorization logic |
| **Transaction Management** | Manages transaction boundaries (Unit of Work) | Participates in transactions but doesn't manage |
| **Current User Context** | Can access current user information | Should not depend on current user context |
| **Return Types** | Returns DTOs | Returns domain objects only |
| **Query Operations** | Can perform query operations | Should not define GET/query methods |
| **Naming Convention** | `*AppService` | `*Manager` or `*Service` |

## Collaboration Patterns in Practice

In real-world development, these two types of services typically work together:

```csharp
// Application Service
public class BookAppService : ApplicationService
{
    private readonly BookManager _bookManager;
    private readonly IRepository<Book> _bookRepository;
    
    [Authorize(BookPermissions.Update)]
    public virtual async Task<BookDto> UpdatePriceAsync(Guid id, decimal newPrice)
    {
        var book = await _bookRepository.GetAsync(id);

        await _bookManager.ChangePriceAsync(book, newPrice);
        
        await _bookRepository.UpdateAsync(book);
        
        return ObjectMapper.Map<Book, BookDto>(book);
    }
}

// Domain Service
public class BookManager : DomainService
{
    public virtual async Task ChangePriceAsync(Book book, decimal newPrice)
    {
        // Domain service focuses on business rules
        if (newPrice <= 0)
        {
            throw new BusinessException("Book:InvalidPrice");
        }
        
        if (book.IsDiscounted && newPrice > book.OriginalPrice)
        {
            throw new BusinessException("Book:DiscountedPriceCannotExceedOriginal");
        }

        if (book.Price == newPrice)
        {
            return;
        }

        // Additional business logic: Check if price change requires approval
        if (await RequiresApprovalAsync(book, newPrice))
        {
            throw new BusinessException("Book:PriceChangeRequiresApproval");
        }

        book.ChangePrice(newPrice);
    }
    
    private Task<bool> RequiresApprovalAsync(Book book, decimal newPrice)
    {
        // Example business rule: Large price increases require approval
        var increasePercentage = ((newPrice - book.Price) / book.Price) * 100;
        return Task.FromResult(increasePercentage > 50); // 50% increase threshold
    }
}
```

## Best Practice Recommendations

### Application Services
- Create a corresponding application service for each aggregate root
- Use clear naming conventions (e.g., `IBookAppService`)
- Implement standard CRUD operation methods (`GetAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`)
- Avoid inter-application service calls within the same module/application
- Always return DTOs, never expose domain entities directly
- Use the `[Authorize]` attribute for declarative authorization or manual checks via `IAuthorizationService`
- Methods automatically run as Unit of Work (transactional)
- Input validation is handled automatically by ABP

### Domain Services
- Use the `Manager` suffix for naming (e.g., `BookManager`)
- Only define state-changing methods, avoid query methods (use repositories directly in Application Services for queries)
- Throw `BusinessException` with clear, unique error codes for domain validation failures
- Keep methods pure, avoid involving user context or authorization logic
- Accept and return domain objects only, never DTOs
- Use descriptive, business-meaningful method names (e.g., `AssignToAsync`, `ChangePriceAsync`)
- Do not implement interfaces unless there's a specific need for multiple implementations

## Summary

Application Services and Domain Services each have their distinct roles in the ABP framework: Application Services serve as use case orchestrators, handling authorization, validation, transaction management, and DTO transformations; Domain Services focus purely on business logic implementation without any infrastructure concerns. Integration Services are a special type of Application Service designed for inter-service communication.

Correctly understanding and applying these service patterns is key to building high-quality ABP applications. Through clear separation of responsibilities, we can not only build more maintainable code but also flexibly switch between monolithic and microservice architectures—this is precisely the elegance of ABP framework design.

## References

- [Application Services](https://abp.io/docs/latest/framework/architecture/domain-driven-design/application-services)
- [Integration Services](https://abp.io/docs/latest/framework/api-development/integration-services)
- [Domain Services](https://abp.io/docs/latest/framework/architecture/domain-driven-design/domain-services)
