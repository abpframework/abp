# FluentValidation 集成

ABP[验证](Validation.md)基础设施是可扩展的. [Volo.Abp.FluentValidation](https://www.nuget.org/packages/Volo.Abp.FluentValidation) NuGet 包扩展了验证系统使其与[FluentValidation](https://fluentvalidation.net/)库一起工作.

## 安装

建议使用[ABP CLI](CLI.md)安装包.

### 使用ABP CLI

在项目(.csproj文件)的文件夹中打开命令行窗口并输入以下命令:

````bash
abp add-package Volo.Abp.FluentValidation
````

### 手动安装

如果你想手动安装;

1. 添加 [Volo.Abp.FluentValidation](https://www.nuget.org/packages/Volo.Abp.FluentValidation) NuGet包到你的项目:

   ````
   Install-Package Volo.Abp.FluentValidation
   ````

2. 添加 `AbpFluentValidationModule` 到你的模块的依赖列表:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpFluentValidationModule) //Add the FluentValidation module
    )]
public class YourModule : AbpModule
{
}
````

## 使用 FluentValidation

按照 [FluentValidation文档](https://fluentvalidation.net/) 创建验证器类.
例如:

````csharp
public class CreateUpdateBookDtoValidator : AbstractValidator<CreateUpdateBookDto>
{
    public CreateUpdateBookDtoValidator()
    {
        RuleFor(x => x.Name).Length(3, 10);
        RuleFor(x => x.Price).ExclusiveBetween(0.0f, 999.0f);
    }
}
````

ABP会自动找到这个类并在对象验证时与 `CreateUpdateBookDto` 关联.

## 另请参阅

* [验证系统](Validation.md)