# Reaction System

CMS kit provides a **reaction** system to add reactions feature to any kind of resource, like blog posts or comments. This section describes the details of the reaction system. 

## Feature

CMS kit uses the [global feature](https://docs.abp.io/en/abp/latest/Global-Features) system for all implemented features. Commercial startup templates come with all the CMS kit related features are enabled by default, if you create the solution with the public website option. If you're installing the CMS kit manually or want to enable reaction management feature individually, open the `GlobalFeatureConfigurator` class in the `Domain.Shared` project and place the following code to the `Configure ` method.

```csharp
GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
{
    cmsKit.Reactions.Enable();
});
```

# Options

## CmsKitReactionOptions

You can use the reaction system to to add reactions feature to any kind of resource, like blog posts or comments etc.The reaction system provides a mechanism to group reactions by entity types. For example, if you want to use reaction system for products, you need to define a entity type named `Product`, and then add reactions under the defined entity type.

`CmsKitReactionOptions` can be configured in the domain layer, in the `ConfigureServices` method of your [module](https://docs.abp.io/en/abp/latest/Module-Development-Basics). Example:

```csharp
Configure<CmsKitReactionOptions>(options =>
{
    options.EntityTypes.Add(
        new ReactionEntityTypeDefinition(
            "Product",
            reactions: new[]
            {
                new ReactionDefinition(StandardReactions.Smile),
                new ReactionDefinition(StandardReactions.ThumbsUp),
                new ReactionDefinition(StandardReactions.ThumbsDown),
                new ReactionDefinition(StandardReactions.Confused),
                new ReactionDefinition(StandardReactions.Eyes),
                new ReactionDefinition(StandardReactions.Heart),
                new ReactionDefinition(StandardReactions.HeartBroken),
                new ReactionDefinition(StandardReactions.Wink),
                new ReactionDefinition(StandardReactions.Pray),
                new ReactionDefinition(StandardReactions.Rocket),
                new ReactionDefinition(StandardReactions.Victory),
                new ReactionDefinition(StandardReactions.Rock),
            }));
}
```

> If you're using the comment or blog features, the ABP framework defines predefined reactions for these features automatically. You can easily override or remove the predefined reactions in `Configure` method like shown above.

`CmsKitReactionOptions` properties:

- `EntityTypes`: List of defined entity types(`CmsKitReactionOptions`) in the reaction system.

`ReactionEntityTypeDefinition` properties:asdas

- `EntityType`: Name of the entity type.
- `Reactions`: List of defined reactions(`ReactionDefinition`) in the entity type.

# Internals

## Domain Layer

#### Aggregates

This module follows the [Entity Best Practices & Conventions](https://docs.abp.io/en/abp/latest/Best-Practices/Entities) guide.

##### UserReaction

A user reaction represents a given reaction from a user.

- `UserReaction` (aggregate root): Represents a given reaction in the system.

#### Repositories

This module follows the [Repository Best Practices & Conventions](https://docs.abp.io/en/abp/latest/Best-Practices/Repositories) guide.

Following custom repositories are defined for this feature:

- `IUserReactionRepository`

#### Domain services

This module follows the [Domain Services Best Practices & Conventions](https://docs.abp.io/en/abp/latest/Best-Practices/Domain-Services) guide.

##### Reaction Manager

`ReactionManager` is used to perform some operations for the `UserReaction` aggregate root.

### Application layer

#### Application services

- `ReactionPublicAppService` (implements `IReactionPublicAppService`): Implements the use cases of reaction system.

### Database providers

#### Common

##### Table / collection prefix & schema

All tables/collections use the `Cms` prefix by default. Set static properties on the `CmsKitDbProperties` class if you need to change the table prefix or set a schema name (if supported by your database provider).

##### Connection string

This module uses `CmsKit` for the connection string name. If you don't define a connection string with this name, it fallbacks to the `Default` connection string.

See the [connection strings](https://docs.abp.io/en/abp/latest/Connection-Strings) documentation for details.

#### Entity Framework Core

##### Tables

- CmsUserReactions

#### MongoDB

##### Collections

- **CmsUserReactions**

### MVC UI

The reaction system provides a reaction widget to allow users send reactions to resources in public websites. You can simply place the widget to the like below. 

```csharp
@await Component.InvokeAsync(typeof(ReactionSelectionViewComponent), new
{
  entityType = "entityType",
  entityId = "entityId"
})
```
You need to specify entity type and entity ID parameters. Entity type represents the group name you specified while defining the reactions in the domain module. Entity ID represents the unique identifier for the resource that users give reactions to such as blog post ID or product ID.

For more information about widgets see [widgets](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Widgets) documentation.