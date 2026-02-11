# Elsa Workflows - Sample Workflow Demo

The `ElsaDemoApp` is a sample application that demonstrates how to use the [Elsa](https://github.com/elsa-workflows/elsa-core) module in an ABP application. The demo application consists of four projects:

- `ElsaDemoApp.Server` is an ABP application with Identity and Elsa modules. It is used as the authentication server and Elsa workflow server.
- `ElsaDemoApp.Studio.WASM` is a Blazor WebAssembly application with Elsa Studio. It is used as the Elsa Studio client application.
- `ElsaDemoApp.Ordering` and `ElsaDemoApp.Payment` are two microservices that can be used to test the Elsa workflows in distributed systems.

![Elsa Module Structure](../images/elsa-module-structure.png)

> **This sample workflow demonstrates how to integrate ABP with Elsa Workflows.** For more detailed information about Elsa itself, please refer to the official [Elsa documentation](https://docs.elsaworkflows.io/) and related guides.

## Download

> **Note:** The `ElsaDemoApp` sample application is only for the **ABP customers**. Therefore, you need to have a commercial license to be able to download the source code.

* You can download the complete source-code from [https://abp.io/api/download/samples/elsaworkflow](https://abp.io/Account/Login?returnUrl=/api/download/samples/elsaworkflow)

## Running the Demo Application

The `ElsaDemoApp.Server` has a pre-defined Elsa workflow that creates an order and processes the payment using Elsa workflows, and uses ABP distributed event bus to coordinate the workflow.

Here is the complete workflow in code:

```cs
public class OrderWorkflow : WorkflowBase
{
    public const string Name = "OrderWorkflow";

    protected override void Build(IWorkflowBuilder builder)
    {
        builder.WithDefinitionId(Name);
        builder.Root = new Sequence
        {
            Activities =
            {
                // Will publish NewOrderEto event to the Ordering microservice,  Ordering microservice will create the order and publish OrderPlaced event
                new CreateOrderActivity(),

                // Wait for the OrderPlaced event, This event is triggered by the Ordering microservice, and Elsa will make workflow continue to the next activity
                new OrderPlacedEvent(),

                // This activity will publish RequestPaymentEto event to the Payment microservice, Payment microservice will process the payment and publish PaymentCompleted event
                new RequestPaymentActivity(),

                //  Wait for the PaymentCompleted event, This event is triggered by the Payment microservice, and Elsa will make workflow continue to the next activity
                new PaymentCompletedEvent(),

                // This activity will send an email to the customer indicating that the payment is completed
                new PaymentCompletedActivity()
            }
        };
    }
}
```

> The demo application uses SQL Server LocalDB as the database provider, Redis as the caching server and RabbitMQ for the message broker. Please make sure you have them installed and running on your machine and then follow the instructions below to run the application.

You can apply the following steps to run the demo application:

1. Run `ElsaDemoApp.Server` project to migrate the database(`dotnet run --migrate-database`) and start the server.
2. Run `ElsaDemoApp.Studio.WASM` project to start the Elsa Studio client application.
3. Run `ElsaDemoApp.Ordering` project to start the Ordering microservice.
4. Run `ElsaDemoApp.Payment` project to start the Payment microservice.

After running all the applications, you can log in to the `ElsaDemoApp.Server` application (with the default credentials) and navigate to the `https://localhost:5001/Ordering` page to create an order:

![Create Order](../images/elsa-create-order.png)

After that, you can navigate to the `ElsaDemoApp.Studio.WASM` application and see the workflow instance created, running, and completed:

![Workflow Instances](../images/elsa-workflow-instances.png)

