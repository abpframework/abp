```json
//[doc-seo]
{
    "Description": "Learn how to implement resource-based authorization in ABP Framework for fine-grained access control on specific resource instances like documents, projects, or any entity."
}
```

# Resource-Based Authorization

**Resource-Based Authorization** is a powerful feature that enables fine-grained access control based on specific resource instances. While the standard [authorization system](./index.md) grants permissions at a general level (e.g., "can edit documents"), resource-based authorization allows you to grant permissions for a **specific** document, project, or any other entity rather than granting a permission for all of them.

## When to Use Resource-Based Authorization?

Consider resource-based authorization when you need to:

* Allow users to edit **only their own blog posts or documents**
* Grant access to **specific projects** based on team membership
* Implement document sharing **where different users have different access levels to the same document**
* Control access to resources based on ownership or custom sharing rules

**Example Scenarios:**

Imagine a document management system where:

- User A can view and edit Document 1
- User B can only view Document 1
- User A has no access to Document 2
- User C can manage permissions for Document 2

This level of granular control is what resource-based authorization provides.

## Usage

Implementing resource-based authorization involves three main steps:

1. **Define** resource permissions in your `PermissionDefinitionProvider`
2. **Check** permissions using `IResourcePermissionChecker`
3. **Manage** permissions via UI or using `IResourcePermissionManager` for programmatic usages

### Defining Resource Permissions

Define resource permissions in your `PermissionDefinitionProvider` class using the `AddResourcePermission` method:

```csharp
namespace Acme.BookStore.Permissions;

public static class BookStorePermissions
{
    public const string GroupName = "BookStore";

    public static class Books
    {
        public const string Default = GroupName + ".Books";
        public const string ManagePermissions = Default + ".ManagePermissions";
        
        public static class Resources
        {
            public const string Name = "Acme.BookStore.Books.Book";
            public const string View = Name + ".View";
            public const string Edit = Name + ".Edit";
            public const string Delete = Name + ".Delete";
        }
    }
}
```

```csharp
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Acme.BookStore.Permissions
{
    public class BookStorePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup("BookStore");

            // Standard permissions
            myGroup.AddPermission(BookStorePermissions.Books.Default, L("Permission:Books"));
            
            // Permission to manage resource permissions (required)
            myGroup.AddPermission(BookStorePermissions.Books.ManagePermissions, L("Permission:Books:ManagePermissions"));

            // Resource-based permissions
            context.AddResourcePermission(
                name: BookStorePermissions.Books.Resources.View,
                resourceName: BookStorePermissions.Books.Resources.Name,
                managementPermissionName: BookStorePermissions.Books.ManagePermissions,
                displayName: L("Permission:Books:View")
            );

            context.AddResourcePermission(
                name: BookStorePermissions.Books.Resources.Edit,
                resourceName: BookStorePermissions.Books.Resources.Name,
                managementPermissionName: BookStorePermissions.Books.ManagePermissions,
                displayName: L("Permission:Books:Edit")
            );

            context.AddResourcePermission(
                name: BookStorePermissions.Books.Resources.Delete,
                resourceName: BookStorePermissions.Books.Resources.Name,
                managementPermissionName: BookStorePermissions.Books.ManagePermissions,
                displayName: L("Permission:Books:Delete"),
                multiTenancySide: MultiTenancySides.Host
            );
        }
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BookStoreResource>(name);
    }
}
```

The `AddResourcePermission` method requires the following parameters:

* `name`: A unique name for the resource permission.
* `resourceName`: An identifier for the resource type. This is typically the full name of the entity class (e.g., `Acme.BookStore.Books.Book`).
* `managementPermissionName`: A standard permission that controls who can manage resource permissions. Users with this permission can grant/revoke resource permissions for specific resources.
* `displayName`: (Optional) A localized display name shown in the UI.
* `multiTenancySide`: (Optional) Specifies on which side of a multi-tenant application this permission can be used. Accepts `MultiTenancySides.Host` (only for the host side), `MultiTenancySides.Tenant` (only for tenants), or `MultiTenancySides.Both` (default, available on both sides). 

### Checking Resource Permissions

Use the `IAuthorizationService` service to check if a user/role/client has a specific permission for a resource:

```csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions.Resources;

namespace Acme.BookStore.Books
{
    public class BookAppService : ApplicationService, IBookAppService
    {
        private readonly IBookRepository _bookRepository;

        public BookAppService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public virtual async Task<BookDto> GetAsync(Guid id)
        {
            var book = await _bookRepository.GetAsync(id);

            // Check if the current user can view this specific book
            var isGranted = await AuthorizationService.IsGrantedAsync(book, BookStorePermissions.Books.Resources.View); // AuthorizationService is a property of the ApplicationService class and will be automatically injected.
            if (!isGranted)
            {
                throw new AbpAuthorizationException("You don't have permission to view this book.");
            }

            return ObjectMapper.Map<Book, BookDto>(book);
        }

        public virtual async Task UpdateAsync(Guid id, UpdateBookDto input)
        {
            var book = await _bookRepository.GetAsync(id);

            // Check if the current user can edit this specific book
            var isGranted = await AuthorizationService.IsGrantedAsync(book, BookStorePermissions.Books.Resources.Edit); // AuthorizationService is a property of the ApplicationService class and will be automatically injected.
            if (!isGranted)
            {
                throw new AbpAuthorizationException("You don't have permission to edit this book.");
            }

            book.Title = input.Title;
            book.Content = input.Content;
            await _bookRepository.UpdateAsync(book);
        }
    }
}
```

In this example, the `BookAppService` uses `IAuthorizationService` to check if the current user has the required permission for a specific book before performing the operation. The method takes the `Book` entity object and resource permission name as parameters.

#### IKeyedObject

The `IAuthorizationService` internally uses `IResourcePermissionChecker` to check resource permissions, and gets the resource key by calling the `GetObjectKey()` method of the `IKeyedObject` interface. All ABP entities implement the `IKeyedObject` interface, so you can directly pass entity objects to the `IsGrantedAsync` method.

> See the [Entities documentation](../../architecture/domain-driven-design/entities.md) for more information about the `IKeyedObject` interface.

#### IResourcePermissionChecker

You can also directly use the `IResourcePermissionChecker` service to check resource permissions which provides more advanced features, such as checking multiple permissions at once:

> You have to pass the resource key (obtained via `GetObjectKey()`) explicitly when using `IResourcePermissionChecker`.

```csharp
public class BookAppService : ApplicationService, IBookAppService
{
    private readonly IBookRepository _bookRepository;
    private readonly IResourcePermissionChecker _resourcePermissionChecker;

    public BookAppService(IBookRepository bookRepository, IResourcePermissionChecker resourcePermissionChecker)
    {
        _bookRepository = bookRepository;
        _resourcePermissionChecker = resourcePermissionChecker;
    }

    public async Task<BookPermissionsDto> GetPermissionsAsync(Guid id)
    {
        var book = await _bookRepository.GetAsync(id);

        var result = await _resourcePermissionChecker.IsGrantedAsync(new[]
            {
                BookStorePermissions.Books.Resources.View,
                BookStorePermissions.Books.Resources.Edit,
                BookStorePermissions.Books.Resources.Delete
            },
            BookStorePermissions.Books.Resources.Name,
            book.GetObjectKey()!);

        return new BookPermissionsDto
        {
            CanView = result.Result[BookStorePermissions.Books.Resources.View] == PermissionGrantResult.Granted,
            CanEdit = result.Result[BookStorePermissions.Books.Resources.Edit] == PermissionGrantResult.Granted,
            CanDelete = result.Result[BookStorePermissions.Books.Resources.Delete] == PermissionGrantResult.Granted
        };
    }
}
```

### Managing Resource Permissions

Once you have defined resource permissions, you need a way to grant or revoke them for specific users, roles, or clients. The [Permission Management Module](../../../modules/permission-management.md) provides the infrastructure for managing resource permissions:

- **UI Components**: Built-in modal dialogs for managing resource permissions on all supported UI frameworks (MVC/Razor Pages, Blazor, and Angular). These components allow administrators to grant or revoke permissions for users and roles on specific resource instances through a user-friendly interface.
- **`IResourcePermissionManager` Service**: A service for programmatically granting, revoking, and querying resource permissions at runtime. This is useful for scenarios like automatically granting permissions when a resource is created, implementing sharing functionality, or integrating with external systems.

> See the [Permission Management Module](../../../modules/permission-management.md#resource-permission-management-dialog) documentation for detailed information on using the UI components and the `IResourcePermissionManager` service.

## See Also

* [Authorization](./index.md)
* [Permission Management Module](../../../modules/permission-management.md)
* [Entities](../../architecture/domain-driven-design/entities.md)
