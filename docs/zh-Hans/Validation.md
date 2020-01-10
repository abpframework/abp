# 验证

验证系统用于验证对于特定的控制器操作或服务的方法的用户输入或客户端请求.

ABP与ASP.NET Core模型验证系统系统兼容,[模型验证文档](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation)中的内容对于基于ABP应用程序同样有效.所以本文主要集中在ABP特征,而不是重复微软文档.

ABP增加了以下优点:

* 定义 `IValidationEnabled` 向任意类添加自动验证. 所有的[应用服务](Application-Services.md)都实现了该接口,所以它们会被自动验证.
* 自动将数据注解属性的验证错误信息本地化.
* 提供可扩展的服务来验证方法调用或对象的状态。
* 提供[FluentValidation](https://fluentvalidation.net/)的集成.

## 验证DTO

本节简要介绍了验证系统.有关详细信息请参阅[ASP.NET Core验证文档](https://docs.microsoft.com/zh-cn/aspnet/core/mvc/models/validation).

### 数据注解 Attribute

使用数据注解是一种以声明式对[DTO](Data-Transfer-Objects.md)进行验证的简单方法.
示例 :

````csharp
public class CreateBookDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(1000)]
    public string Description { get; set; }

    [Range(0, 999.99)]
    public decimal Price { get; set; }
}
````

当使用该类作为[应用服务](Application-Services.md)或控制器的参数时,将对其自动验证并抛出本地化异常(由ABP框架[处理](Exception-Handling.md)).

### IValidatableObject

`IValidatableObject` can be implemented by a DTO to perform custom validation logic. `CreateBookDto`  in the following example implements this interface and checks if the `Name` is equals to the `Description` and returns a validation error in this case.
可以将DTO实现 `IValidatableObject` 接口进行自定义验证逻辑. 下面的示例中 `CreateBookDto` 实现了这个接口,并检查 `Name` 是否等于 `Description` 并返回一个验证错误.

````csharp
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Acme.BookStore
{
    public class CreateBookDto : IValidatableObject
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Range(0, 999.99)]
        public decimal Price { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (Name == Description)
            {
                yield return new ValidationResult(
                    "Name and Description can not be the same!",
                    new[] { "Name", "Description" }
                );
            }
        }
    }
}
````

#### 解析服务

如果你需要从[依赖注入系统](Dependency-Injection.md)解析服务,可以使用 `ValidationContext` 对象.
例：

````csharp
var myService = validationContext.GetRequiredService<IMyService>();
````

> 虽然可以在 `Validate` 方法中解析服务实现任何可能性,但在DTO中实现领域验证逻辑不是一个很好的做法. 应保持简单的DTO,他们的目的是传输数据(DTO:数据传输对象).

## 验证基础设施

本节介绍了ABP框架提供的一些额外的服务.

### IValidationEnabled 接口

`IValidationEnabled` 是可以由任何类来实现的空标记接口(注册到[DI](Dependency-Injection.md)并从中解析),让ABP框架为该类执行验证系统.
示例 :

````csharp
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;

namespace Acme.BookStore
{
    public class MyService : ITransientDependency, IValidationEnabled
    {
        public virtual async Task DoItAsync(MyInput input)
        {
            //...
        }
    }
}
````

> ABP框架使用[动态代理/拦截](Dynamic-Proxying-Interceptors.md)系统来执行验证.为了使其工作,你的方法应该是 **virtual** 的,服务应该被注入并通过接口(如`IMyService`)使用.

### AbpValidationException

一旦ABP确定了一个验证错误,它就会抛出类型为 `AbpValidationException` 的异常. 你的应用程序代码可以抛出 `AbpValidationException`,但大多数情况不会使用它.

* `ValidationErrors` 是 `AbpValidationException` 的属性,它包含了验证错误列表.
* `AbpValidationException` 的日志级别设置为 `Warning`. 它将所有验证错误记录到[日志系统](Logging.md).
* `AbpValidationException` 由ABP框架自动捕获并将HTTP状态码设置为400转换成可用的错误响应. 参阅[异常处理](Exception-Handling.md)文档了解更多.

## 高级主题

### IObjectValidator

除了自动验证你可能需要手动验证对象,这种情况下[注入](Dependency-Injection.md)并使用 `IObjectValidator` 服务:

* `Validate` 方法根据验证​​规则验证给定对象,如果对象没有被验证通过会抛出 `AbpValidationException` 异常.
* `GetErrors` 不会抛出异常,只返回验证错误.

`IObjectValidator` 默认由 `ObjectValidator` 实现. `ObjectValidator`是可扩展的; 可以实现`IObjectValidationContributor`接口提供自定义逻辑.
示例 :

````csharp
public class MyObjectValidationContributor
    : IObjectValidationContributor, ITransientDependency
{
    public void AddErrors(ObjectValidationContext context)
    {
        //Get the validating object
        var obj = context.ValidatingObject;

        //Add the validation errors if available
        context.Errors.Add(...);
    }
}
````

* 记录将类注册到[DI](Dependency-Injection.md)(实现`ITransientDependency` 如同本例)
* ABP会自动发现验证类,并用于任何类型的对象验证(包括自动方法调用验证).

### IMethodInvocationValidator

`IMethodInvocationValidator` 用于验证方法调用. 它在内部使用 `IObjectValidator` 来验证传递给方法调用的对象. 由于框架会自动使用此服务,通常你并不需要此服务,但在少数情况下你可能在应用程序中重用或替换它.

## FluentValidation Integration

Volo.Abp.FluentValidation 包将FluentValidation库集成到了验证系统(通过实现 `IObjectValidationContributor`). 请参阅[FluentValidation集成文档](FluentValidation.md)了解更多信息.