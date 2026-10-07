```json
//[doc-seo]
{
    "Description": "Learn best practices for implementing Domain Services in your applications, guided by Domain-Driven Design principles."
}
```

# Domain Services Best Practices & Conventions

> This document offers best practices for implementing Domain Service classes in your modules and applications based on Domain-Driven-Design principles.
>
> **Ensure you've read the [*Domain Services*](../domain-driven-design/domain-services.md) document first.**

## Domain Services

- **Do** define domain services in the **domain layer**.
- **Do not** create interfaces for the domain services **unless** you have a good reason to (like mocking and testing different implementations).
- **Do** name your domain service with `Manager` suffix.

For the example of a domain service:
```cs
public class IssueManager : DomainService
{
	//...
}
```

## Domain Service Methods

- **Do not** define `GET` methods. `GET` methods do not change the state of an entity. Hence, use the repository directly in the Application Service instead of Domain Service method.

- **Do** define methods that mutate data by changing the state of an entity or an aggregate root.

- **Do not** define methods with generic names (like `UpdateIssueAsync`). 

- **Do** define methods with self-explanatory names (like `AssignToAsync`) that implement the specific domain logic.


- **Do** accept valid domain objects as parameters.

```cs
public async Task AssignToAsync(Issue issue, IdentityUser user)
{
    //...
}
```

- **Do** throw `BusinessException` or a custom business exception if validation fails.

  - **Do** use domain error codes with a unique code-namespace for exception localization.

```cs
public async Task AssignToAsync(Issue issue, IdentityUser user)
{
    var openIssueCount = await _issueRepository.GetCountAsync(
            i => i.AssignedUserId == user.Id && !i.IsClosed
        );

        if (openIssueCount >= 3)
        {
            throw new BusinessException("IssueTracking:ConcurrentOpenIssueLimit");
        }

        issue.AssignedUserId = user.Id;
}
```

- **Do not** return `DTO`. Return only domain objects when you need.
- **Do not** involve authenticated user logic. Instead, define an extra parameter and send the related data of ` CurrentUser` from the Application Service layer.

## See Also

* [Video tutorial](https://abp.io/video-courses/essentials/domain-services)
* [Domain Services](../domain-driven-design/domain-services.md)
* [Exception Handling](../../fundamentals/exception-handling.md)
