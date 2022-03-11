## 规范

规范模式用于为实体和其他业务对象定义 **命名、可复用、可组合和可测试的过滤器** 。

> 规范是领域层的一部分。

## 安装

> 这个包 **已经安装** 在启动模板中。所以，大多数时候你不需要手动去安装。

添加 [Volo.Abp.Specifications](https://abp.io/package-detail/Volo.Abp.Specifications) 包到你的项目. 如果当前文件夹是您的项目的根目录(`.csproj`)时，您可以在命令行终端中使用 [ABP CLI](CLI.md) *add package* 命令:

````bash
abp add-package Volo.Abp.Specifications
````

## 定义规范

假设您定义了如下的顾客实体：

````csharp
using System;
using Volo.Abp.Domain.Entities;

namespace MyProject
{
    public class Customer : AggregateRoot<Guid>
    {
        public string Name { get; set; }

        public byte Age { get; set; }

        public long Balance { get; set; }

        public string Location { get; set; }
    }
}
````

您可以创建一个由 `Specification<Customer>` 派生的新规范类。

**例如：规定选择一个18岁以上的顾客**

````csharp
using System;
using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace MyProject
{
    public class Age18PlusCustomerSpecification : Specification<Customer>
    {
        public override Expression<Func<Customer, bool>> ToExpression()
        {
            return c => c.Age >= 18;
        }
    }
}
````

您只需通过定义一个lambda[表达式](https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/operators/lambda-expressions)来定义规范。

> 您也可以直接实现`ISpecification<T>`接口，但是基类`Specification<T>`做了大量简化。

## 使用规范

这里有两种常见的规范用例。

### IsSatisfiedBy

`IsSatisfiedBy` 方法可以用于检查单个对象是否满足规范。

**例如：如果顾客不满足年龄规定，则抛出异常**

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace MyProject
{
    public class CustomerService : ITransientDependency
    {
        public async Task BuyAlcohol(Customer customer)
        {
            if (!new Age18PlusCustomerSpecification().IsSatisfiedBy(customer))
            {
                throw new Exception(
                    "这位顾客不满足年龄规定!"
                );
            }
            
            //TODO...
        }
    }
}
````

### ToExpression & Repositories

`ToExpression()` 方法可用于将规范转化为表达式。通过这种方式，您可以使用规范在**数据库查询时过滤实体**。

````csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace MyProject
{
    public class CustomerManager : DomainService, ITransientDependency
    {
        private readonly IRepository<Customer, Guid> _customerRepository;

        public CustomerManager(IRepository<Customer, Guid> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<Customer>> GetCustomersCanBuyAlcohol()
        {
            var queryable = await _customerRepository.GetQueryableAsync();
            var query = queryable.Where(
                new Age18PlusCustomerSpecification().ToExpression()
            );
            
            return await AsyncExecuter.ToListAsync(query);
        }
    }
}
````

> 规范被正确地转换为SQL/数据库查询语句，并且在DBMS端高效执行。虽然它与规范无关，但如果您想了解有关 `AsyncExecuter` 的更多信息，请参阅[仓储](Repositories.md)文档。

实际上，没有必要使用 `ToExpression()` 方法，因为规范会自动转换为表达式。这也会起作用：

````csharp
var queryable = await _customerRepository.GetQueryableAsync();
var query = queryable.Where(
    new Age18PlusCustomerSpecification()
);
````

## 编写规范

规范有一个强大的功能是，它们可以与`And`、`Or`、`Not`以及`AndNot`扩展方法组合使用。

假设您有另一个规范，定义如下：

```csharp
using System;
using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace MyProject
{
    public class PremiumCustomerSpecification : Specification<Customer>
    {
        public override Expression<Func<Customer, bool>> ToExpression()
        {
            return (customer) => (customer.Balance >= 100000);
        }
    }
}
```

您可以将 `PremiumCustomerSpecification` 和 `Age18PlusCustomerSpecification` 结合起来，查询优质成人顾客的数量，如下所示：

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Specifications;

namespace MyProject
{
    public class CustomerManager : DomainService, ITransientDependency
    {
        private readonly IRepository<Customer, Guid> _customerRepository;

        public CustomerManager(IRepository<Customer, Guid> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<int> GetAdultPremiumCustomerCountAsync()
        {
            return await _customerRepository.CountAsync(
                new Age18PlusCustomerSpecification()
                .And(new PremiumCustomerSpecification()).ToExpression()
            );
        }
    }
}
````

如果你想让这个组合成为一个可复用的规范，你可以创建这样一个组合的规范类，它派生自`AndSpecification`：

````csharp
using Volo.Abp.Specifications;

namespace MyProject
{
    public class AdultPremiumCustomerSpecification : AndSpecification<Customer>
    {
        public AdultPremiumCustomerSpecification() 
            : base(new Age18PlusCustomerSpecification(),
                   new PremiumCustomerSpecification())
        {
        }
    }
}
````

现在，您就可以向下面一样重新编写 `GetAdultPremiumCustomerCountAsync` 方法：

````csharp
public async Task<int> GetAdultPremiumCustomerCountAsync()
{
    return await _customerRepository.CountAsync(
        new AdultPremiumCustomerSpecification()
    );
}
````

> 你可以从这些例子中看到规范的强大之处。如果您之后想要更改 `PremiumCustomerSpecification` ，比如将余额从 `100.000` 修改为 `200.000` ，所有查询语句和合并的规范都将受到本次更改的影响。这是减少代码重复的好方法！

## 讨论

虽然规范模式通常与C#的lambda表达式相比较，算是一种更老的方式。一些开发人员可能认为不再需要它，我们可以直接将表达式传入到仓储或领域服务中，如下所示：

````csharp
var count = await _customerRepository.CountAsync(c => c.Balance > 100000 && c.Age => 18);
````

自从ABP的[仓储](Repositories.md)支持表达式，这是一个完全有效的用法。您不必在应用程序中定义或使用任何规范，可以直接使用表达式。

所以，规范的意义是什么？为什么或者应该在什么时候考虑去使用它？

### 何时使用?

使用规范的一些好处：

- **可复用**：假设您在代码库的许多地方都需要用到优质顾客过滤器。如果使用表达式而不创建规范，那么如果以后更改“优质顾客”的定义会发生什么？假设您想将最低余额从100000美元更改为250000美元，并添加另一个条件，成为顾客超过3年。如果使用了规范，只需修改一个类。如果在任何其他地方重复（复制/粘贴）相同的表达式，则需要更改所有的表达式。
- **可组合**：可以组合多个规范来创建新规范。这是另一种可复用性。
- **命名**：`PremiumCustomerSpecification` 更好地解释了为什么使用规范，而不是复杂的表达式。因此，如果在您的业务中使用了一个有意义的表达式，请考虑使用规范。
- **可测试**：规范是一个单独（且易于）测试的对象。

### 什么时侯不要使用?

- **没有业务含义的表达式**：不要对与业务无关的表达式和操作使用规范。
- **报表**：如果只是创建报表，不要创建规范，而是直接使用 `IQueryable` 和LINQ表达式。您甚至可以使用普通SQL、视图或其他工具生成报表。DDD不关心报表，因此从性能角度来看，查询底层数据存储的方式可能很重要。
