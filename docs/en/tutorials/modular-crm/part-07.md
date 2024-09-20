# Integrating the Modules: Communication via Messages (Events)

````json
//[doc-nav]
{
  "Previous": {
    "Name": "Integrating the Modules: Implementing Integration Services",
    "Path": "tutorials/modular-crm/part-06"
  },
  "Next": {
    "Name": "Integrating the Modules: Joining the Products and Orders Data",
    "Path": "tutorials/modular-crm/part-08"
  }
}
````

Another common approach to communicate between modules is messaging. By publishing and handling messages, a module can perform operation when an event happens in another module.

ABP provides two types of event buses for loosely coupled communication:

* [Local Event Bus](../../framework/infrastructure/event-bus/local/index.md) is suitable for in-process messaging. Since in a modular monolith, both of publisher and subscriber are in the same process, they can communicate in-process, without needing an external message broker.
* **[Distributed Event Bus](../../framework/infrastructure/event-bus/distributed/index.md)** is normal for inter-process messaging, like microservices, for publishing and subscribing to distributed events. However, ABP's distributed event bus works as local (in-process) by default (actually, it uses the Local Event Bus under the hood by default), unless you configure an external message broker.

If you consider to convert your modular monolith to a microservice system later, it is best to use the Distributed Event Bus with default local/in-process implementation. It already supports database level transactional event execution and has no performance penalty. If you switch to an external provider (e.g. [RabbitMQ](../../framework/infrastructure/event-bus/distributed/rabbitmq.md) or [Kafka](../../framework/infrastructure/event-bus/distributed/kafka.md)), you don't need to change your application code.

On the other hand, if you want to publish events and always subscribe in the same module, you should use the Local Event Bus. In that way, if you switch to microservices later, you don't accidently (and unnecessarily) make a local event distributed. Both of the event bus types can be used in the same system, just understand these and use them properly.

Since we will use messaging (events) between different modules, we will use the distributed event bus.

## Publishing an Event

In the example scenario, we want to publish an event when a new order is placed. The Ordering module will publish the event since it knows when a new order is placed. The Products module will subscribe to that event and get notified when a new order is placed. It will decrease the stock count of the product that is related to the new order. The scenario is pretty simple, let's implement it.

### Defining the Event Class

Open the `ModularCrm.Ordering` module in your IDE, find the `ModularCrm.Ordering.Contracts` project, create an `Events` folder and create an `OrderPlacedEto` class inside that folder. The final folder structure should be like that:

![visual-studio-order-event](images/visual-studio-order-event.png)

We've placed the `OrderPlacedEto` class inside the `ModularCrm.Ordering.Contracts` project since that project can be referenced and used by other modules without accessing internal implementation of the Ordering module. The `OrderPlacedEto` class definition should be the following:

````csharp
using System;

namespace ModularCrm.Ordering.Contracts.Events
{
    public class OrderPlacedEto
    {
        public string CustomerName { get; set; }
        public Guid ProductId { get; set; }
    }
}
````

`OrderPlacedEto` is very simple. It is a plain C# class and used to transfer data related to the event (*ETO* is an acronym for *Event Transfer Object*, a suggested naming convention, but not required). You can add more properties if it is needed. For this tutorial, it is more than enough.

### Using the `IDistributedEventBus` Service

`IDistributedEventBus` service is used to publish events to the event bus. Until this point, the Ordering module has no functionality to create a new order.

In the Part 3, we had used ABP's Auto HTTP API Controller feature to automatically expose HTTP APIs from application services. In this section, we will create an ASP.NET Core API controller class to create a new order. In that way, you will also see that it is not different than creating a regular ASP.NET Core controller.

Open the `ModularCrm.Ordering` module's .NET solution, create a `Controllers` folder in the `ModularCrm.Ordering` project and place a controller class named `OrdersController` in that new folder. The final folder structure should be like that:

![visual-studio-ordering-controller](images/visual-studio-ordering-controller.png)

Here the full `OrdersController` class:

````csharp
using Microsoft.AspNetCore.Mvc;
using ModularCrm.Ordering.Contracts.Enums;
using ModularCrm.Ordering.Contracts.Events;
using ModularCrm.Ordering.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;

namespace ModularCrm.Ordering.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : AbpControllerBase
    {
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IDistributedEventBus _distributedEventBus;

        public OrdersController(
            IRepository<Order, Guid> orderRepository,
            IDistributedEventBus distributedEventBus)
        {
            _orderRepository = orderRepository;
            _distributedEventBus = distributedEventBus;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(OrderCreationModel input)
        {
            // Create a new Order entity
            var order = new Order
            {
                CustomerName = input.CustomerName,
                ProductId = input.ProductId,
                State = OrderState.Placed
            };

            // Save it to the database
            await _orderRepository.InsertAsync(order);

            // Publish an event, so other modules can be informed
            await _distributedEventBus.PublishAsync(
                new OrderPlacedEto
                {
                    ProductId = order.ProductId,
                    CustomerName = order.CustomerName
                });

            return Created();
        }

        public class OrderCreationModel
        {
            public Guid ProductId { get; set; }

            [Required]
            [StringLength(120)]
            public string CustomerName { get; set; }
        }
    }
}
````

The `OrdersController.CreateAsync` method simply creates a new `Order` entity, saves it to the database and finally publishes an `OrderPlacedEto` event.

## Subscribing to an Event

In this section, we will subscribe to the `OrderPlacedEto` event in the Products module and decrease the related product's stock count once a new order is placed.

### Adding a Reference to the `ModularCrm.Ordering.Contracts` Package

Since the `OrderPlacedEto` class is located inside the `ModularCrm.Ordering.Contracts` project, we need to add that package's reference to the Products module. This time, we will use the *Import Module* feature of ABP Studio (as an alternative to approach we used in the *Adding a Reference to the `ModularCrm.Products.Application.Contracts` Package* section of the [previous part](part-06.md)).

Open the ABP Studio UI and stop the application if it is already running. Then open the *Solution Explorer* in ABP Studio, right-click the `ModularCrm.Products` module and select the *Import Module* command:

![abp-studio-import-module-ordering](images/abp-studio-import-module-ordering.png)

In the opening dialog, find and select the `ModularCrm.Ordering` module, check the *Install this module* option and click the OK button:

![abp-studio-import-module-dialog-for-ordering](images/abp-studio-import-module-dialog-for-ordering.png)

Once you click the OK button, the Ordering module is imported to the Products module and an installation dialog is open:

![abp-studio-install-module-dialog-for-ordering](images/abp-studio-install-module-dialog-for-ordering.png)

Here, select the `ModularCrm.Ordering.Contracts` package on the left side (because we want to add that package reference) and `ModularCrm.Products.Domain` package on the middle area (because we want to add the package reference to that project). We installed it to the [domain layer](../../framework/architecture/domain-driven-design/domain-layer.md) of the Products module since we will create our event handler into that layer. Click the OK button to finish the installation operation.

You can check the ABP Studio's *Solution Explorer* panel to see the module import and the project reference (dependency).

![abp-studio-imports-and-dependencies](images/abp-studio-imports-and-dependencies.png)

### Handling the `OrderPlacedEto` Event

Now, it is possible to use the `OrderPlacedEto` class inside the Product module's domain layer since it has the `ModularCrm.Ordering.Contracts` package reference.

Open the Product module's .NET solution in your IDE, locate the `ModularCrm.Products.Domain` project, create a new `Orders` folder and an `OrderEventHandler` class inside that folder. The final folder structure should be like that:

![visual-studio-order-event-handler](images/visual-studio-order-event-handler.png)

Replace the `OrderEventHandler.cs` file's content with the following code block:

````csharp
using ModularCrm.Ordering.Contracts.Events;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;

namespace ModularCrm.Products.Orders
{
    public class OrderEventHandler :
        IDistributedEventHandler<OrderPlacedEto>, 
        ITransientDependency
    {
        private readonly IRepository<Product, Guid> _productRepository;

        public OrderEventHandler(IRepository<Product, Guid> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task HandleEventAsync(OrderPlacedEto eventData)
        {
            // Find the related product
            var product = await _productRepository.FindAsync(eventData.ProductId);
            if (product == null)
            {
                return;
            }

            // Decrease the stock count
            product.StockCount = product.StockCount - 1;

            // Update the entity in the database
            await _productRepository.UpdateAsync(product);
        }
    }
}
````

`OrderEventHandler` implements the `IDistributedEventHandler<OrderPlacedEto>` interface. In that way, ABP recognizes that class and subscribes to the related event automatically. Implementing `ITransientDependency` simply registers the `OrderEventHandler` class to the [dependency injection](../../framework/fundamentals/dependency-injection.md) system as a transient object.

We are injecting the product repository and updating the stock count in the event handler method (`HandleEventAsync`). That's all.

### Testing the Order Creation

We will not create a UI for creating an order, to keep this tutorial more focused. You can easily create a form to create an order on your user interface. We will test it just using the Swagger UI in this section.

Graph build the `ModularCrm.Web` application, run it on the ABP Studio's *Solution Runner* panel and browse the application UI as demonstrated earlier.

Once the application is running and ready, manually type `/swagger` to the end of the URL and press the ENTER key. You should see the Swagger UI that is used to discover and test your HTTP APIs:

![abp-studio-swagger-create-order](images/abp-studio-swagger-create-order.png)

Find the *Orders* API, click the *Try it out* button, enter a sample value the the *Request body*:

````json
{
  "productId": "0fbf7dd0-d7e9-0d18-9214-3a14d9fa1b74",
  "customerName": "David"
}
````

> **IMPORTANT:** Here, you should type a valid Product Id from the Products table of your database!

Once you press the *Execute* button, a new order is created. At that point, you can check the `/Orders` page to see if the new order is shown on the UI, and check the `/Products` page to see if the related product's stock count has decreased.

Here, sample screenshots from the Products and Orders pages:

![products-orders-pages-crop](images/products-orders-pages-crop.png)

We placed a new order for Product C. As a result, Product C's stock count has decreased from 55 to 54 and a new line is added to the Orders page.
