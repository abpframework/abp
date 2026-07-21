```json
//[doc-seo]
{
    "Description": "Learn how reusable ABP modules keep module-specific user records synchronized with the Identity module."
}
```

# User Lookup and Synchronization

Reusable modules sometimes need a small, module-specific user entity. For example, a module may need the user name and display information next to its own records without depending on the concrete `IdentityUser` aggregate.

The `Volo.Abp.Users` packages provide shared contracts and base services for this pattern. They do not replace the [Identity module](../identity.md), the current-user service or the Identity user-management UI.

## Core Contracts

The main contracts are:

* `IUser` defines the common aggregate data, including the user ID, tenant ID, user name, contact information and active state.
* `IUserData` is the transport-neutral representation used by lookup providers and synchronization code.
* `IUpdateUserData` lets a module-specific user entity opt in to updates from an `IUserData` value.
* `IUserRepository<TUser>` defines lookup, search and count operations for a module-specific user repository.
* `IUserLookupService<TUser>` is the service consumed by the rest of the module.

The `UserLookupService<TUser, TUserRepository>` base class implements the local and external lookup workflow. A derived service only needs to define how an external `IUserData` value creates the module's user entity.

```csharp
public interface IMyUserLookupService : IUserLookupService<MyUser>
{
}

public class MyUserLookupService :
    UserLookupService<MyUser, IMyUserRepository>,
    IMyUserLookupService
{
    public MyUserLookupService(
        IMyUserRepository userRepository,
        IUnitOfWorkManager unitOfWorkManager)
        : base(userRepository, unitOfWorkManager)
    {
    }

    protected override MyUser CreateUser(IUserData externalUser)
    {
        return new MyUser(externalUser);
    }
}
```

In this example, `MyUser` implements `IUser` and `IUpdateUserData`, while `IMyUserRepository` implements `IUserRepository<MyUser>`. The Blogging and CMS Kit modules use this same pattern for their local user entities.

## Lookup Behavior

`FindByIdAsync` and `FindByUserNameAsync` first query the module's local repository.

* Without an `IExternalUserLookupServiceProvider`, the local result is returned.
* By default, an existing local user is returned without querying the external provider.
* If no local user exists, the external result is converted to the module's user entity and persisted.
* If the local entity implements `IUpdateUserData`, data returned by the external provider can update that entity.
* If the external provider throws an exception, the exception is logged and the existing local result is returned. An exception does not delete local data.

The default favors local reads over checking the external source on every request. A derived lookup service can set `SkipExternalLookupIfLocalUserExists` to `false` when every find operation must check the external source:

```csharp
public class FreshMyUserLookupService :
    UserLookupService<MyUser, IMyUserRepository>
{
    public FreshMyUserLookupService(
        IMyUserRepository userRepository,
        IUnitOfWorkManager unitOfWorkManager)
        : base(userRepository, unitOfWorkManager)
    {
        SkipExternalLookupIfLocalUserExists = false;
    }

    protected override MyUser CreateUser(IUserData externalUser)
    {
        return new MyUser(externalUser);
    }
}
```

With this option disabled, a missing external result causes the corresponding local user to be deleted. This behavior applies to `FindByIdAsync` and `FindByUserNameAsync`; it is not a background synchronization process.

`SearchAsync` and `GetCountAsync` follow a different rule. If an external provider is available, they delegate directly to that provider instead of combining local and external results. The provider must apply consistent filtering to both methods and implement the search method's paging and sorting.

The shared repository bases compare `UserName` directly. The Users module does not define a normalization algorithm for custom repositories or providers. If a provider normalizes user names, keep the local repository and external provider lookup rules consistent rather than relying on case-insensitive behavior.

## External Lookup Providers

The Identity module supplies two common `IExternalUserLookupServiceProvider` implementations:

* `IdentityUserRepositoryExternalUserLookupServiceProvider` reads from the local Identity repository.
* `HttpClientExternalUserLookupServiceProvider` calls the Identity integration service in a remote application.

See the [External User Lookup Service](../identity.md#external-user-lookup-service) section for monolithic and distributed application setup.

You can replace the external provider with your own implementation. It is responsible for ID and user-name lookups and for matching `SearchAsync` and `GetCountAsync` behavior.

## Synchronizing with Distributed Events

`UserEto` implements `IUserData` and carries the tenant ID. The Identity module maps `IdentityUser` to this ETO and enables the standard created, updated and deleted distributed entity events.

The Users module does not register synchronization handlers for your entity. A reusable module decides which events it needs. A common design is:

* Create the local user lazily through `IUserLookupService<TUser>` when the module first needs it.
* Subscribe to `EntityUpdatedEto<UserEto>` to refresh an existing local copy.
* Subscribe to `EntityDeletedEto<UserEto>` when deleted users must be removed immediately. With the default local-first setting, an existing local record is not rechecked against the external source.

Keep the local entity ID and tenant ID equal to the values in `IUserData`. An `IUpdateUserData.Update` implementation should reject data for another user or tenant and return `false` when nothing changed.

See the [Distributed Events](../identity.md#distributed-events) section for the Identity event configuration and a basic event-handler example.

## Database Providers

The Users module provides `EfCoreUserRepositoryBase<TDbContext, TUser>` and `MongoUserRepositoryBase<TDbContext, TUser>`. Both implement the shared repository search and count contract. The EF Core package also provides `ConfigureAbpUser` for the common user properties and length constraints.

Your module still owns its user table or collection, indexes and additional properties. Provider-specific schema decisions remain in the module that defines the concrete user entity.
