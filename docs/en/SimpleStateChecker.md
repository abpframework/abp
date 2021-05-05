# Simple State Checker

The **state** of some objects may depend on multiple conditions, such as some permissions are required, features need to be enabled, or it is only for special users and roles are available.

## Definition state checker.

Any class can inherit `IHasSimpleStateCheckers` to support state checks.

````csharp
public class MyObject : IHasSimpleStateCheckers<MyObject>
{
    public int Id { get; set; }

    public List<ISimpleStateChecker<MyObject>> SimpleStateCheckers { get; }

    public MyObject()
    {
        SimpleStateCheckers = new List<ISimpleStateChecker<MyObject>>();
    }
}
````

The `MyObject` class contains a collection of state checkers, you can add your custom checkers to it.

````csharp
public class MyObjectStateChecker : ISimpleStateChecker<MyObject>
{
    public Task<bool> IsEnabledAsync(SimpleStateCheckerContext<MyObject> context)
    {
        var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
        return Task.FromResult(currentUser.IsInRole("Admin"));
    }
}
````

````csharp
var myobj = new MyObject()
{
    Id = 100
};

myobj.SimpleStateCheckers.Add(new MyObjectStateChecker());
````

## Definition global state checker.

`AbpSimpleStateCheckerOptions` is the options class that used to set the global state checkers for specific object.

Example: Add global state for `MyObject`:

````csharp
services.Configure<AbpSimpleStateCheckerOptions<MyObject>>(options =>
{
    options.GlobalSimpleStateCheckers.Add<MyGlobalObjectStateChecker>();
    //options.GlobalSimpleStateCheckers.Add<>(); //Add more global state checkers
});
````

> Write this inside the `ConfigureServices` method of your module.

### Check the state

You can inject `ISimpleStateCheckerManager<MyObject>` service to check state.

````csharp
bool enabled = await stateCheckerManager.IsEnabledAsync(myobj);
````

### Batch check the states

If there are many instance items that require state checking, there may be performance problems.

In this case, you can implement `ISimpleBatchStateChecker`. It can check multiple items at once.
You need to make sure that the same `ISimpleBatchStateChecker` instance is added to the `SimpleStateCheckers` of multiple instances.

> `SimpleBatchStateCheckerBase` inherits the `ISimpleBatchStateChecker` interface and implements the `IsEnabledAsync` method of a single object by default.

````csharp
public class MyObjectBatchStateChecker : SimpleBatchStateCheckerBase<MyObject>
{
    public override Task<SimpleStateCheckerResult<MyObject>> IsEnabledAsync(SimpleBatchStateCheckerContext<MyObject> context)
    {
        var result = new SimpleStateCheckerResult<MyObject>(context.States);

        foreach (var myObject in context.States)
        {
            if (myObject.Id > 100)
            {
                result[myObject] = true;
            }
        }

        return Task.FromResult(result);
    }
}
````

````csharp
var myobj1 = new MyObject()
{
    Id = 100
};
var myobj2 = new MyObject()
{
    Id = 99
};

var myObjectBatchStateChecker = new MyObjectBatchStateChecker();

myobj1.SimpleStateCheckers.Add(myObjectBatchStateChecker);
myobj2.SimpleStateCheckers.Add(myObjectBatchStateChecker);

SimpleStateCheckerResult<MyObject> stateCheckerResult = await stateCheckerManager.IsEnabledAsync(new []{ myobj1, myobj2 });
````

## Built-in state checker

The `PermissionDefinition`, `ApplicationMenuItem` and `ToolbarItem` objects have implemented state checks and have built-in general state checkers, you can directly use their extension methods.

````csharp
RequireAuthenticated();
RequirePermissions(bool requiresAll, params string[] permissions);
RequireFeatures(bool requiresAll, params string[] features);
RequireGlobalFeatures(bool requiresAll, params Type[] globalFeatures);
````