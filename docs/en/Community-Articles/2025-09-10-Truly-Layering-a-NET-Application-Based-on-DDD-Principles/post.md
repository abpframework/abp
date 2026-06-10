# **Truly Layering a .NET Application Based on DDD Principles**

Okay, so we ALL been there, right? You start new project thinking "this time will be different" - clean code, perfect architecture, everything organized. Fast forward 3 months and your codebase look like someone throw grenade into bowl of spaghetti. Business logic everywhere, your controllers doing database work, and every new feature feel like defusing bomb.

I been there too many times, and honestly, it suck. But here thing - there actually way to build .NET apps that not turn into maintenance nightmare. It called **Layered Architecture** + **Domain-Driven Design (DDD)**, and once you get it, it game changer.

Let me walk you through this step by step, no fluff, just practical stuff that actually work.

### **Layered Architecture 101 (The Foundation)**

So layered architecture basically about keeping your code organized. Instead of having everything mixed together like bad smoothie, you separate concerns into different layers. Think like organizing your room - clothes go in closet, books on shelf, etc.

Here how it typically break down:

  * **Presentation Layer (UI):** This what users actually see and click on - your ASP.NET Core MVC stuff, Razor Pages, Blazor, whatever float your boat.
  * **Application Layer:** The conductor of orchestra. It not do heavy lifting itself, but tell everyone else what to do. It like middle manager of your code.
  * **Domain Layer:** The VIP section. This where all your business rules live - entities, value objects, whole nine yards. This layer pure and not give damn about databases or UI.
  * **Infrastructure Layer:** The "how-to" guy. Database stuff, email sending, API calls - basically all technical plumbing that make everything work.

The golden rule? **Dependency Rule**: Layers can only talk to layers below them (or more central). UI talk to Application, Application talk to Domain, but Domain? Domain not talk to anyone. It the cool kid that everyone want to hang out with.

### **DDD: Where Magic Happen**

Alright, so DDD not some fancy framework you install from NuGet. It more like mindset - basically saying "hey, let make our code actually reflect business we building for." Instead of having bunch of random classes, we organize everything around actual business domain.

Think like this: if you building e-commerce app, your code should scream "I'M E-COMMERCE APP" not "I'M BUNCH OF RANDOM CLASSES."

Here toolkit DDD give you (all living in your Domain Layer):

  * **Entity:** This something that have identity. Like `Customer` - two customers with same name still different people because they have different IDs. It like having two friends named John - they not same person.
  * **Value Object:** Opposite of entity. It defined by what it contain, not who it is. `Address` perfect for this - if two addresses have same street, city, and zip code, they same address. Usually immutable too.
  * **Aggregate & Aggregate Root:** This where it get interesting. Aggregate like family of related objects that stick together. **Aggregate Root** head of family - only one you talk to when you want change something. Like `Order` that contain `OrderItem`s. You not mess with `OrderItem` directly, you tell `Order` to handle it.
  * **Repository (Interface):** Think like your data access contract. It say "here how you can get and save stuff" without caring about whether it SQL Server, MongoDB, or file on your desktop. Interface live in Domain, implementation go in Infrastructure.
  * **Domain Service:** When business logic too complex for single entity or value object, this your go-to. It like utility class but for business rules.

### **Putting It All Together: Real C# Code**

Alright, enough theory. Let see what this actually look like in real .NET solution. You typically have projects like:

  * `MyProject.Domain` (or `.Core`) - The VIP section
  * `MyProject.Application` - The middle manager
  * `MyProject.Infrastructure` - The technical guy
  * `MyProject.Web` (or whatever UI you using) - The pretty face

**1. The Domain Layer (`MyProject.Domain`) - The Heart**

This where magic happen. Zero dependencies on other projects (maybe some basic utility libraries, but that it). Pure business logic, no database nonsense, no UI concerns.

```csharp
// In MyProject.Domain/Orders/Order.cs
public class Order : AggregateRoot<Guid>
{
    public Address ShippingAddress { get; private set; }
    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    // Private constructor for ORM
    private Order() { }

    public Order(Guid id, Address shippingAddress) : base(id)
    {
        ShippingAddress = shippingAddress;
    }

    public void AddOrderItem(Guid productId, int quantity, decimal price)
    {
        if (quantity <= 0)
        {
            throw new BusinessException("Quantity must be greater than zero.");
        }
        // More business rules...
        _orderItems.Add(new OrderItem(productId, quantity, price));
    }
}

// In MyProject.Domain/Orders/IOrderRepository.cs
public interface IOrderRepository
{
    Task<Order> GetAsync(Guid id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
}
```

See what I mean? The `Order` class all about business rules (`AddOrderItem` with validation and all that jazz). It not give damn about databases or how it get saved. That someone else problem.

**2. The Application Layer (`MyProject.Application`) - The Conductor**

This where we orchestrate everything. It talk to domain objects and use repositories to get/save data. Think like middle manager that coordinate work but not do heavy lifting.

```csharp
// In MyProject.Application/Orders/OrderAppService.cs
public class OrderAppService
{
    private readonly IOrderRepository _orderRepository;

    public OrderAppService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task CreateOrderAsync(CreateOrderDto input)
    {
        var shippingAddress = new Address(input.Street, input.City, input.ZipCode);
        var order = new Order(Guid.NewGuid(), shippingAddress);

        foreach (var item in input.Items)
        {
            order.AddOrderItem(item.ProductId, item.Quantity, item.Price);
        }

        await _orderRepository.AddAsync(order);
    }
}
```

The application service coordinate everything but let domain objects handle actual business rules. Clean separation!

**3. The Infrastructure Layer (`MyProject.Infrastructure`) - The Technical Guy**

This where we implement all interfaces we defined in domain. Entity Framework Core, email services, API clients - all technical plumbing live here.

```csharp
// In MyProject.Infrastructure/Orders/EfCoreOrderRepository.cs
public class EfCoreOrderRepository : IOrderRepository
{
    private readonly MyDbContext _dbContext;

    public EfCoreOrderRepository(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order> GetAsync(Guid id)
    {
        // EF Core logic to get the order
        return await _dbContext.Orders.FindAsync(id);
    }

    public async Task AddAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
    }
    
    // ... other implementations
}
```

### **ABP Framework: The Shortcut (Because We Lazy)**

Look, setting all this up from scratch pain. That where **ABP Framework** come in clutch. It basically DDD and layered architecture on steroids, and it do all boring setup work for you.

ABP not just talk talk - it walk walk. When you create new ABP solution, boom! Perfect project structure, all layered and DDD-compliant, ready to go.

Here what you get out of box:

  * **Base Classes:** `AggregateRoot`, `Entity`, `ValueObject` - all with good stuff like optimistic concurrency and domain events. No more writing boilerplate.
  * **Generic Repositories:** No more writing `IRepository` interfaces for every single entity. ABP give you `IRepository<TEntity, TKey>` with all standard CRUD methods. Just inject it and go.
  * **Application Services:** Inherit from `ApplicationService` and boom - you done. It handle validation, authorization, exception handling, all that cross-cutting concern stuff without cluttering your actual business logic.

With ABP, our `OrderAppService` become way cleaner:

```csharp
// In ABP project, this much cleaner
public class OrderAppService : ApplicationService, IOrderAppService
{
    private readonly IRepository<Order, Guid> _orderRepository;

    public OrderAppService(IRepository<Order, Guid> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task CreateAsync(CreateOrderDto input)
    {
        // ... same logic as before, but using ABP generic repository
        var order = new Order(...);
        await _orderRepository.InsertAsync(order);
    }
}
```

### **Wrapping Up**

Look, I get it - this stuff take discipline and it not always fastest way to get features out door. But here thing: when you actually layer your app properly and put solid Domain Model at center, you end up with software that not suck to maintain.

Your code start speaking language of business instead of some random technical jargon. That whole point of DDD - make your code reflect what you actually building for.

Yeah, it take work upfront, but payoff huge. And frameworks like ABP make journey way less painful. Trust me, your future self will thank you when you not debugging spaghetti code at 2 AM.

What you think? You try this approach before, or you still stuck in spaghetti code phase? Let me know in comments!