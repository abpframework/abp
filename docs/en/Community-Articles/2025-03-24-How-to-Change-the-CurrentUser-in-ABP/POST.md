# How to Change the CurrentUser in ABP?

[ABP Framework](https://abp.io/) provides a powerful service for accessing information about the currently authenticated user in your application. Understanding how to use and modify this service (`ICurrentUser`) is essential for both basic and certain advanced scenarios. 

In this article, we'll explore the [`CurrentUser` service](https://abp.io/docs/latest/framework/infrastructure/current-user), its use cases, and how to change it when necessary.

---
> 🛠 Liked this post? I now share all my content on Substack — real-world .NET, AI, and scalable software design.
> 👉 Subscribe here → engincanveske.substack.com
> 🎥 Also, check out my YouTube channel for hands-on demos and deep dives: https://www.youtube.com/@engincanv
---

## Understanding the ICurrentUser Service

The `ICurrentUser` interface is the primary service in ABP Framework for obtaining information about the logged-in user. It provides some key properties, such as `Id`, `UserName`, `TenantId`, `Roles` (roleNames), and more...

`ICurrentUser` is implemented on the `ICurrentPrincipalAccessor` service and works with claims as well. So, all of these properties are actually retrieved from the claims. ICurrentUser has some methods to directly work with the claims, such as:

* FindClaim (finds a single claim by name)
* FindClaims (gets all claims with the given name)
* IsInRole (checks if the user has a specific role)
* GetAllClaims (gets all claims of the user)

## Where the CurrentUser Service is Used?

The CurrentUser service is used extensively throughout ABP applications whenever there's a need to access information about the logged-in user. Common scenarios include: authorization checks, logging, setting common properties like `CreatorId`, `LastModifierId`, `DeleterId`, and more...

## When to Change the CurrentUser Service?

While the CurrentUser service works automatically in the context of HTTP requests (it gets the `User` property of the current `HttpContext`), there are advanced scenarios where you might need to manually set or change the current user:

1. **Background workers:** When executing code outside the context of a user request
2. **Event handlers:** When processing events that may run in a different context
3. **Unit & integration tests:** When simulating a user for testing purposes

## How to Change the CurrentUser Service?

If you need to change the CurrentUser service, you can inject the `ICurrentPrincipalAccessor` service, use its `Change` method to change the current user, and then use the `CurrentUser` service as usual.

Here's how to change the current user for a specific scope:

```csharp
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;

namespace MyProject.Products;

public class ProductEventHandler : IDistributedEventHandler<OrderPlacedEto>, ITransientDependency
{
    private readonly IProductRepository _productRepository;
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;
    private readonly IdentityUserManager _userManager;

    public ProductEventHandler(
        IProductRepository productRepository,
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        IdentityUserManager userManager
    )
    {
        _productRepository = productRepository;
        _currentPrincipalAccessor = currentPrincipalAccessor;
        _userManager = userManager;
    }

    public async Task HandleEventAsync(OrderPlacedEto eventData)
    {
        var product = await _productRepository.FindAsync(eventData.ProductId);
        if (product == null)
        {
            return;
        }
        
        //Get the admin user
        var adminUser = await _userManager.FindByNameAsync("admin");
        if (adminUser == null)
        {
            return;
        }

        var newPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { 
                new Claim(AbpClaimTypes.UserId, adminUser.Id.ToString()),
                new Claim(AbpClaimTypes.UserName, "admin"),
            }));
        
        //IMPORTANT: It will set the CreatorId, LastModifierId, etc. with the admin user
        using (_currentPrincipalAccessor.Change(newPrincipal))
        {
            product.StockCount -= eventData.Quantity;

            // Update the product
            await _productRepository.UpdateAsync(product);
        }
    }
}
```

In this example, we have a distributed event handler that processes an `OrderPlacedEto` event. When an order is placed, we need to update the product's stock count. However, we want this operation to be performed under an admin user's context for auditing purposes.

Here's what the code does step by step:

1. First, it retrieves the product using the product ID from the event data.
2. Then, it finds the admin user by username using the `_userManager.FindByNameAsync("admin")`.
3. A new `ClaimsPrincipal` is created with the admin user's claims (`UserId` and `UserName`).
4. Using the `_currentPrincipalAccessor.Change()` method within a `using` statement, it temporarily changes the current user context to the admin user.
5. Inside this scope, it updates the product's stock count by subtracting the ordered quantity.
6. Finally, it saves the changes to the database using the repository.

**The important part here is that any audit properties (like `CreatorId`, `LastModifierId`, etc.) will be set to the admin user's ID because we changed the current principal. Once the using block ends, the original user context is automatically restored.**

This pattern is particularly useful in background jobs, event handlers, or any scenario where you need to perform operations under a specific user's context, regardless of the actual authenticated user.

---
> 🛠 Liked this post? I now share all my content on Substack — real-world .NET, AI, and scalable software design.
> 👉 Subscribe here → engincanveske.substack.com
> 🎥 Also, check out my YouTube channel for hands-on demos and deep dives: https://www.youtube.com/@engincanv
---

## Conclusion

The `CurrentUser` service in ABP Framework provides a simple way to access information about the authenticated user. While it works automatically in most scenarios, there are cases where you need to explicitly change the current user identity, particularly in background processing scenarios.

By using the ICurrentPrincipalAccessor.Change() method within a using statement, you can temporarily change the current user for a specific scope of execution, enabling your background processes, event handlers, or tests to execute with the identity of a specific user.