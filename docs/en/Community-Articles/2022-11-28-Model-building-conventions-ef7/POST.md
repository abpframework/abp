# Model building conventions in Entity Framework Core 7.0

In this article, I will show you one of the new features of EF Core 7 named "Model building conventions".

Entity Framework Core uses a metadata model to describe how entity types are mapped to the database. Before EF Core 7.0, it was not possible to remove or replace existing conventions or add new conventions. With EF Core 7.0, this is now possible. To read more about it, you can visit its [documentation](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew#model-building-conventions).

EF Core uses many built-in conventions. You can see the full list of the conventions on `IConvetion` Interface API [documentation](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.metadata.conventions.iconvention?view=efcore-7.0).

If you want to add, remove or replace a convention, you need to override `ConfigureConventions` method of your DbContext as shown below;

```csharp
protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
{
    configurationBuilder.Conventions.Add(_ =>  new MyCustomConvention());
}
```

## Allowed Operations

### Removing an existing convention

Existing conventions provided by EF Core are well thought and useful, but sometimes some of them might not be a good candidate for your application. In such cases, you can remove an existing convention as shown below;

```csharp
protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
{
    configurationBuilder.Conventions.Remove(typeof(ForeignKeyIndexConvention));
}
```

### Adding a new convention

Just like removing a convention, we can add a completely new convention as well. You can define many different conventions here. For example, you can specify a standard precision for all decimal fields in your entities.

```csharp
public class DecimalPrecisionConvention : IModelFinalizingConvention
    {
        public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
        {
            foreach (var property in modelBuilder.Metadata.GetEntityTypes()
                         .SelectMany(
                             entityType => entityType.GetDeclaredProperties()
                                 .Where(
                                     property => property.ClrType == typeof(decimal))))
            {
                property.Builder.HasPrecision(2);
            }
        }
    }
```

Note that, conventions are executed in the order they are added. So you need to be careful in which order they are added.

### Replacing an existing convention

Sometimes, a default convention might work slightly different than what your app expects. In such cases, you can create your own implementation by inheriting from that convention and replace the default one. For example, you can create a convention as shown below;

```csharp
public class MyCustomConvention : ADefaultEfCoreConvention
{
    public MyCustomConvention(ProviderConventionSetBuilderDependencies dependencies)
            : base(dependencies)
        {
        }    
    // override the methods you want to change.
}
```

Then, you can replace the default one with your implementation as shown below;

```csharp
protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
{
    configurationBuilder.Conventions.Replace<ADefaultEfCoreConvention>(
        serviceProvider => new MyCustomConvention(
            serviceProvider.GetRequiredService<ProviderConventionSetBuilderDependencies>()));
}
```

As a final note, conventions never override configuration marked as **DataAnnotation** or **Explicit**. This means that, even if there is a convention, if the property has a `DataAnnotation` attribute or configuration in `OnModelCreating`, convetion will not be used. Here are the configuration types EF Core uses;

* **Explicit:** The model element was explicitly configured in OnModelCreating
* **DataAnnotation:** The model element was configured using a mapping attribute (aka data annotation) on the CLR type
* **Convention:** The model element was configured by a model building convention

## Using in ABP-based solution

Since ABP uses EF Core, you can use this feature in ABP as well. 