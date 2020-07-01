# Organization Unit Management

Organization units (OU) is a part of **Identity Module** and can be used to **hierarchically group users and entities**. 

### OrganizationUnit Entity

An OU is represented by the **OrganizationUnit** entity. The fundamental properties of this entity are:

- **TenantId**: Tenant's Id of this OU. Can be null for host OUs.
- **ParentId**: Parent OU's Id. Can be null if this is a root OU.
- **Code**: A hierarchical string code that is unique for a tenant.
- **DisplayName**: Shown name of the OU.

The OrganizationUnit entity's primary key (Id) is a **Guid** type and it derives from the [**FullAuditedAggregateRoot**](../Entities.md#aggregateroot-class) class which provides audit information with **IsDeleted** property (OUs are not deleted from the database, they are just marked as deleted).

#### Organization Tree

Since an OU can have a parent, all OUs of a tenant are in a **tree** structure. There are some rules for this tree;

- There can be more than one root (where the ParentId is null).
- There is a limit for the first-level children count of an OU (because of the fixed OU Code unit length explained below).

#### OU Code

OU code is automatically generated and maintained by the OrganizationUnit Manager. It's a string that looks something like this:

"**00001.00042.00005**"

This code can be used to easily query the database for all the children of an OU (recursively). There are some rules for this code:

- It must be **unique** for a [tenant](../Multi-Tenancy.md).
- All the children of the same OU have codes that **start with the parent OU's code**.
- It's **fixed length** and based on the level of the OU in the tree, as shown in the sample.
- While the OU code is unique, it can be **changeable** if you move an OU.
- You must reference an OU by Id, not Code.

### OrganizationUnit Manager

The **OrganizationUnitManager** class can be [injected](../Dependency-Injection.md) and used to manage OUs. Common use cases are:

- Create, Update or Delete an OU
- Move an OU in the OU tree.
- Getting information about the OU tree and its items.

#### Multi-Tenancy

The OrganizationUnitManager is designed to work for a **single tenant** at a time. It works for the **current tenant** by default.

### Common Use Cases

There are some common use cases for OUs. You can find the source code of the samples [here](https://github.com/abpframework/abp-samples/tree/master/OrganizationUnitSample).

#### Creating An Entity That Belongs To An Organization Unit

The most obvious usage of OUs is to assign an entity to an OU. Sample entity:

```csharp
public class Product : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid OrganizationUnitId { get; private set; }
    public virtual Guid? TenantId { get; }
    public virtual string Name { get; private set; }
    public virtual float Price { get; private set; }
}
```

You need to create **OrganizationUnitId** property to assign this entity to an OU. Depending on requirement, this property can be nullable. You can now relate a Product to an OU and query the products of a specific OU.

You can use **IMultiTenant** interface if you want to distinguish products of different tenants in a multi-tenant application (see the [Multi-Tenancy document](https://aspnetboilerplate.com/Pages/Documents/Multi-Tenancy#data-filters) for more info). If your application is not multi-tenant, you don't need this interface and property.

#### Getting Entities In An Organization Unit

To get the Products of an OU,  you can implement a simple domain service; [Product Manager](https://github.com/abpframework/abp-samples/blob/master/OrganizationUnitSample/src/OrganizationUnitSample.Domain/Products/ProductManager.cs) in this case to get the filtered data:

```csharp
public class ProductManager : IDomainService
{
    private readonly IProductRepository<Product> _productRepository;

    public ProductManager(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public List<Product> GetProductsInOu(OrganizationUnit organizationUnit)
    {
        return (await _productRepository.GetListAsync()).Where(q => q.OrganizationUnitId == 			organizationUnit.Id).ToList();
    }                
}
```

**For better practice**, you should consider querying it on domain layer for performance and scalability. To do so, add a method to your repository interface:

```csharp
public interface IProductRepository : IRepository<Product, Guid>
{
    public Task<List<Product>> GetProductsOfOrganizationUnitAsync(Guid organizationUnitId);
}
```

Then implement it on your ORM layer (which is EntityFrameworkCore in this [sample](https://github.com/abpframework/abp-samples/blob/master/OrganizationUnitSample/src/OrganizationUnitSample.EntityFrameworkCore/Products/ProductRepository.cs)):

```csharp
public Task<List<Product>> GetProductsOfOrganizationUnitAsync(Guid organizationUnitId)
{
    return DbSet.Where(p => p.OrganizationUnitId == organizationUnitId).ToListAsync();
}
```

Afterwards, you can modify your domain service like below:

```csharp
public List<Product> GetProductsInOu(OrganizationUnit organizationUnit)
{
	return await _productRepository.GetProductsOfOrganizationUnitAsync(organizationUnit.Id);
}
```

### Settings

You can find **MaxUserMembershipCount** settings under SettingManagement:

- **MaxUserMembershipCount**: Maximum allowed membership count for a user.
  Default value is **int.MaxValue** which allows a user to be a member of unlimited OUs at the same time.