# Integration Testing Best Practices for Building a Robust Application Layer 

## 1. Introduction to Integration Testing

For software development purposes, it is not sufficient to validate individual components individually in a vacuum. In practical usage, programs consist of holistic systems comprising a myriad of components such as **databases, services, APIs, and existing tools**, which **must work together**. An application would fail testing parameters for an individual component sufficiently but fail as a whole if such components fail to interact appropriately.

Whereas unit tests verify individual parts one by one, **integration tests confirm software reliability with integrated parts**. Integration testing guarantees that the overall system performs as per design parameters whenever parts of the system are interfederated.

**Why Integration Testing Is Important:**

* **Reliability:** Checks the system works as it should, even with network problems, service stops, or incorrect data.
* **Easy to Update:** Makes sure adding or changing parts doesn't break what already works.
* **Good Quality:** Finds problems before users do.
* **Strong Application Layer:** Checks that database actions, service handling, and API communications all work together correctly.

**Example: Detailed Integration Test with Dependency Injection**

```csharp
[Fact]
public async Task OrderCreation_WithValidInput_ShouldPersistAndReturnSuccess()
{
    // Set up: Use a complete Test Server
    var factory = new WebApplicationFactory<Startup>();
    var client = factory.CreateClient();

    // Do:
    var request = new CreateOrderRequest { ProductId = 1, Quantity = 2 };
    var response = await client.PostAsJsonAsync("/api/orders", request);

    // Check
    response.EnsureSuccessStatusCode(); // Stops if the code isn't successful
    var order = await response.Content.ReadFromJsonAsync<Order>();
    Assert.NotNull(order);
    Assert.Equal(2, order.Quantity);
}
```

This example uses a memory `TestServer` to act like the full HTTP process, from the controller to the database, for a more real test.

## 2. Setting Up an Isolated Integration Test Environment

Integration tests should happen in their own area to prevent:

* **Slow tests** because of network issues or big data amounts
* **Unreliable results** from live data changes
* **Tests messing with each other**

**Tips for Keeping Tests Separate:**

* **Memory databases:** Fast and easy to reset for simple data tasks.
* **Container-based areas (Docker/TestContainers):** Copy real areas safely for complex setups (like PostgreSQL, Redis).
* **Database Actions:** Undo changes after each test to keep things separate and fast.

**Example: Using Actions to Keep Things Separate**

```csharp
[Fact]
public async Task OrderCreation_RollsBack_AfterTest()
{
    // Set up: Start an action that will be undone later
    await using var transaction = await _dbContext.Database.BeginTransactionAsync();

    var service = new OrderService(_dbContext);
    var result = service.CreateOrder(1, 2);
    Assert.True(result.IsSuccess);

    // Do & Check
    var orderCount = await _dbContext.Orders.CountAsync();
    Assert.Equal(1, orderCount);

    // Clean up: Undo the action to reset all changes
    await transaction.RollbackAsync();
}
```

Using actions makes sure tests stay separate and can run at the same time safely.

## 3. Seeding Test Data for Consistent Integration Results

Start data gives a steady base for tests. Include complex links for real situations.

```csharp
// Use a special way to get the data started
private static async Task SeedData(AppDbContext dbContext)
{
    await dbContext.Products.AddRangeAsync(
        new Product { Id = 1, Name = "Coffee", Price = 5, Stock = 50 },
        new Product { Id = 2, Name = "Tea", Price = 3, Stock = 20 }
    );
    await dbContext.Users.AddAsync(new User { Id = 1, Name = "Alice", IsActive = true });
    await dbContext.Orders.AddAsync(new Order { Id = 1, UserId = 1, ProductId = 1, Quantity = 2 });

    await dbContext.SaveChangesAsync();
}
```

**Tip:** Use the Builder style or a special `TestSeeder` class to make detailed, reusable data setups.

## 4. Validating API and Service Layer Interactions

Integration tests should copy what real users do to check the whole request-response process.

```csharp
// Detailed API Test with CancellationToken and Custom Headers
var request = new CreateOrderRequest { ProductId = 1, Quantity = 2 };
var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/orders")
{
    Content = JsonContent.Create(request)
};
requestMessage.Headers.Add("X-Request-Id", Guid.NewGuid().ToString());

// Act like a timeout is happening
var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));
await Assert.ThrowsAsync<TaskCanceledException>(async () =>
{
    await _client.SendAsync(requestMessage, cts.Token);
});
```

Test both normal uses and unusual ones like timeouts, bad requests, and too many users at once for a truly reliable API.

## 5. Mocking External Dependencies in Integration Tests

Use copies or fake versions for services like payment systems, emails, or other APIs to keep your application logic separate.

```csharp
// Set up: Copy an outside payment service
var paymentServiceMock = new Mock<IPaymentService>();
paymentServiceMock.Setup(x => x.ProcessPayment(It.IsAny<decimal>(), It.IsAny<string>()))
                  .ThrowsAsync(new TimeoutException("Payment gateway timeout"));

// Do: Put the copy into the test area
services.AddScoped(_ => paymentServiceMock.Object);

// Check: See how the system handles the timeout
var response = await _client.PostAsJsonAsync("/api/payments", new { Amount = 100 });
Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
Assert.Contains("Payment service unavailable", await response.Content.ReadAsStringAsync());
```

Mimicking lets you act like things are failing (like APIs timing out or services being down) and check that your application handles these issues well without using real network calls.

## 6. Covering Success and Failure Scenarios in Tests

A good test set has full coverage of both successful actions and error handling.

```csharp
// Normal use: A successful order creation
var validOrder = new CreateOrderRequest { ProductId = 1, Quantity = 1 };
var successResponse = await _client.PostAsJsonAsync("/api/orders", validOrder);
Assert.Equal(HttpStatusCode.Created, successResponse.StatusCode);

// Bad request: Wrong product ID
var invalidProductOrder = new CreateOrderRequest { ProductId = 999, Quantity = 1 };
var badProductResponse = await _client.PostAsJsonAsync("/api/orders", invalidProductOrder);
Assert.Equal(HttpStatusCode.NotFound, badProductResponse.StatusCode);

// Conflict: Not enough stock
var largeOrder = new CreateOrderRequest { ProductId = 1, Quantity = 1000 };
var stockResponse = await _client.PostAsJsonAsync("/api/orders", largeOrder);
Assert.Equal(HttpStatusCode.Conflict, stockResponse.StatusCode);
```

Testing everything makes sure your application layer gives helpful and correct error messages to users.

## 7. Ensuring Cleanup and Test Isolation

Keep test results steady by making sure each test is totally separate from others.

```csharp
// Use a special cleanup method or a test setup
public class OrderTests : IDisposable
{
    private readonly AppDbContext _dbContext;

    public OrderTests()
    {
        _dbContext = new AppDbContext(GetDbContextOptions());
        // Setup logic here
    }

    // Cleanup logic
    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
```

Clean up automatically for tests that run at the same time using `IDisposable` or by putting each test in a database action.

## 8. Automating Integration Tests with CI/CD Pipelines

Automate tests in CI/CD lines for regular, consistent checking.

```yaml
jobs:
  test-and-build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: 7.0.x
      - name: Build
        run: dotnet build --configuration Release
      - name: Run Integration Tests
        run: dotnet test --filter Category=Integration --logger trx;LogFileName=testresults.trx
```

Regular testing finds integration problems early, saving effort over time.

## 9. Complete Integration Test Example with a Basic Comparison

```csharp
[Fact]
public async Task CompleteOrderWorkflow_ShouldPersistAllSteps()
{
    // Set up: Start complex data for a full action
    await SeedData(_dbContext);

    // Do: Act like a real user request
    var request = new CreateOrderRequest { ProductId = 1, Quantity = 2, UserId = 1 };
    var response = await _client.PostAsJsonAsync("/api/orders", request);
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);

    // Check 1: See the database state
    var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.UserId == 1 && o.ProductId == 1);
    Assert.NotNull(order);
    Assert.Equal(2, order.Quantity);

    // Check 2: See side effects (like stock going down)
    var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == 1);
    Assert.Equal(48, product.Stock);
}
```

🍪 **Cookie Comparison**

* **Starting products** are like the dough you have ready.
* **The new order** is like the new stuff you mix in.
* **The oven (application)** mixes both, baking (business logic).
* **The cookie** is the right outcome, using the old dough and new stuff well.

This shows how a good application layer uses new and old data well together.

---

### 10. Summary:

* **Separate Area:** Ensures tests run on their own.
* **Start Data:** Gives results that are consistent.
* **Mimicking Outside Parts:** Keeps tests stable.
* **Testing Both Good and Bad:** Makes the situation solid.
* **CI/CD Automation:** Keeps the system stable.
