# Text Templating

## Introduction

ABP Framework provides a simple, yet efficient text template system. Text templating is used to dynamically render contents based on a template and a model (a data object):

TEMPLATE + MODEL => RENDERED CONTENT

It is very similar to an ASP.NET Core Razor View (or Page):

RAZOR VIEW (or PAGE) + MODEL => HTML CONTENT

### Example

Here, a simple template:

````
Hello {{model.name}} :)
````

You can define a class with a `Name` property to render this template:

````csharp
public class HelloModel
{
    public string Name { get; set; }
}
````

If you render the template with a `HelloModel` with the `Name` is `John`, the rendered output is will be:

````
Hello John :)
````

Template rendering engine is very powerful;

* It is based on the [Scriban](https://github.com/lunet-io/scriban) library, so it supports **conditional logics**, **loops** and much more.
* Template content **can be localized**.
* You can define **layout templates** to be used as the layout while rendering other templates.
* You can pass arbitrary objects to the template context (beside the model) for advanced scenarios.

## Installation

It is suggested to use the [ABP CLI](CLI.md) to install this package.

### Using the ABP CLI

Open a command line window in the folder of the project (.csproj file) and type the following command:

````bash
abp add-package Volo.Abp.TextTemplating
````

### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.TextTemplating](https://www.nuget.org/packages/Volo.Abp.TextTemplating) NuGet package to your project:

````
Install-Package Volo.Abp.TextTemplating
````

2. Add the `AbpTextTemplatingModule` to the dependency list of your module:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpTextTemplatingModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
}
````

## Defining Templates

Before rendering a template, you should define it. Create a class inheriting from the `TemplateDefinitionProvider` base class:

````csharp
public class DemoTemplateDefinitionProvider : TemplateDefinitionProvider
{
    public override void Define(ITemplateDefinitionContext context)
    {
        context.Add(
            new TemplateDefinition("Hello") //template name: "Hello"
                .WithVirtualFilePath(
                    "/Demos/Hello/Hello.tpl", //template content path
                    isInlineLocalized: true
                )
        );
    }
}
````

* `context` object is used to add new templates or get the templates defined by depended modules. Used `context.Add(...)` to define a new template.
* `TemplateDefinition` is the class represents a template. Each template must have a unique name (that will be used while you are rendering the template).
* `/Demos/Hello/Hello.tpl` is the path of the template file.
* `isInlineLocalized` is used to declare if you are using a single template for all languages (`true`) or different templates for each language (`false`). See the Localization section below for more.

### The Template Content

`WithVirtualFilePath` indicates that we are using the [Virtual File System](Virtual-File-System.md) to store the template content. Create a `Hello.tpl` file inside your project and mark it as "**embedded resource**" on the properties window:

![hello-template](images/hello-template.png)

Example `Hello.tpl` content is shown below:

````
Hello {{model.name}} :)
````

The [Virtual File System](Virtual-File-System.md) requires to add your files in the `ConfigureServices` method of your [module](Module-Development-Basics.md) class:

````csharp
Configure<AbpVirtualFileSystemOptions>(options =>
{
    options.FileSets.AddEmbedded<TextTemplateDemoModule>("TextTemplateDemo");
});
````

* `TextTemplateDemoModule` is the module class that you define your template in.
* `TextTemplateDemo` is the root namespace of your project.

## Rendering the Template

`ITemplateRenderer` service is used to render a template content. Example:

````csharp
public class HelloDemo : ITransientDependency
{
    private readonly ITemplateRenderer _templateRenderer;

    public HelloDemo(ITemplateRenderer templateRenderer)
    {
        _templateRenderer = templateRenderer;
    }

    public async Task RunAsync()
    {
        var result = await _templateRenderer.RenderAsync(
            "Hello", //the template name
            new HelloModel
            {
                Name = "John"
            }
        );

        Console.WriteLine(result);
    }
}
````

* `HelloDemo` is a simple class that injects the `ITemplateRenderer` in its constructor and uses it inside the `RunAsync` method.
* `RenderAsync` gets two fundamental parameters:
  * `templateName`: The name of the template to be rendered (`Hello` in this example).
  * `model`: An object that is used as the `model` inside the template (a `HelloModel` object in this example).

The result shown below for this example:

````csharp
Hello John :)
````

### Anonymous Model

While it is suggested to create model classes for the templates, it would be practical (and possible) to use an anonymous object for simple cases:

````csharp
var result = await _templateRenderer.RenderAsync(
    "Hello", //the template name
    new
    {
        Name = "John"
    }
);
````

In this case, we haven't created a model class, but created an anonymous object as the model.

### PascalCase vs CamelCase

PascalCase property names (like `UserName`) is used as camelCase (like `userName`) in the template as a convention.

























## Logic

A Text Template is a combination of two parts: template definition and template content.
### Template Definition

Template Definition is an object that contains some information about your text templates. Template Definition object contains the following properties.

- `Name` *(string)*: Unique name of the template. It is then used to render the template.
- `IsLayout` *(boolean)*:
- `Layout` *(string)* contains the <u>name of layout template</u>
- `LocalizationResource` *(Type)* The localization resource type that is used if this template is inline localized.
- `IsInlineLocalized` *(boolean)* describes that the template is inline localized or not
- `DefaultCultureName` *(string)* defines the default culture for the template

### Template Content

This is a simple content for your templates. For default, template contents stored as `Virtual File`.

> Example: ForgotPasswordEmail.tpl

```html
<h3>{{L "PasswordReset"}}</h3>

<p>{{L "PasswordResetInfoInEmail"}}</p>

<div>
    <a href="{{model.link}}">{{L "ResetMyPassword"}}</a>
</div>
```

### Localization

You can localize your Text Templates by choosing two different methods.

- `Inline Localization`
- `Multiple Content Localization`

#### Inline Localization

An inline localized text template is using only one content resource, and it is using the [Abp.Localization](Localization.md) to get content in different languages/cultures. 

Example Inline Localized Text Template content: 

```html
<h3>{{L "PasswordReset"}}</h3>

<p>{{L "PasswordResetInfoInEmail"}}</p>

<div>
    <a href="{{model.link}}">{{L "ResetMyPassword"}}</a>
</div>
```

#### Multiple Content Localization

You can store your Text Templates for any culture in different content resource.

> Example Multiple Content Localization

> ForgotPasswordEmail / en.tpl

```html
<h3>Reset Your Password</h3>

<p>Hello, this is a password changing email.</p>

<div>
    <a href="{{model.link}}">Click To Reset Your Password</a>
</div>
```

> ForgotPasswordEmail / tr.tpl

```html
<h3>Şifrenizi Değiştirin</h3>

<p>Merhaba, bu bir şifre yenileme e postasıdır.</p>

<div>
    <a href="{{model.link}}">Şifrenizi Yenilemek İçin Tıklayınız</a>
</div>
```

### Layout System

It is typical to use the same layout for some different text templates. So, you can define a layout template. 

A text template can be layout for different text templates and also a text template may use a layout.

A layout Text Template must have `{{content}}` area to render the child content. _(just like the `RenderBody()` in the MVC)_

Example Email Layout Text Template

```html
<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
</head>
<body>
    {{content}}
</body>
</html>
```

## Definition of a Text Template

First of all, create a class that inherited from `TemplateDefinitionProvider` abstract class and create `Define` method that derived from the base class.

`Define` method requires a context that is `ITemplateDefinitionContext`. This `context` is a storage for template definitions and we will add our template definitions to the context.

> **NOTE!** For default, ABP uses **Virtual File System** for text templates. Do not forget to register your files as an `Embedded Resource`. Please check the [Virtual File System Documentation](Virtual-File-System.md) for more details.

> All given examples are for `Virtual File Text Template Definitions`.

```csharp
public class MyTemplateDefinitionProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            // Layout Text Template
            context.Add(
                new TemplateDefinition(
                    name: "MySampleTemplateLayout", // Template Definition Name
                    isLayout: true 
                ).WithVirtualFilePath("/SampleTemplates/SampleTemplateLayout.tpl", true)
            );

            // Inline Localized Text Template
            context.Add(
                new TemplateDefinition(
                    name: "ForgotPasswordEmail",
                    localizationResource: typeof(MyLocalizationResource),
                    layout: TestTemplates.TestTemplateLayout1
                ).WithVirtualFilePath("/SampleTemplates/ForgotPasswordEmail.tpl", true)
            );

            // Multiple File Localized Text Template
            context.Add(
                new TemplateDefinition(
                    name: "ForgotPasswordEmail",
                    defaultCultureName: "en"
                ).WithVirtualFilePath("/SampleTemplates/ForgotPasswordEmail", false)
            );
        }
    }
```

As you see in the given example, all Text Templates are added with `(ITemplateDefinitionContext)context.Add` method. This method requires a `TemplateDefinition` object. Then we call `WithVirtualFilePath` method with chaining for the describe where is the virtual files.

`WithVirtualFilePath` is requires one `tpl` file path for the `Inline Localized` Text Templates. If your Text Tempalte is `Multi Localized` you should create a folder and store each different culture files under that. So you can send the folder path as a parameter to `WithVirtualFilePath`.

> Inline Localized File

```
/ Folder / ForgotPasswordEmail.tpl
```

> Multi Content Localization

```
/ Folder / ForgotPasswordEmail / en.tpl
/ Folder / ForgotPasswordEmail / tr.tpl
```


## Rendering

When one template is registered, it is easy to render and get the result with `ITemplateRenderer` service. 

`ITemplateRenderer` service has one method that named `RenderAsync` and to render your content and it is requires some parameters. 

- `templateName` (_string_)
- `model` (_object_)
- `cultureName` (_string_)
- `globalContext` (_dictionary_)

`templateName` is exactly same with Template Definition Name.

`model` is a dynamic object. This is using to put dynamic data into template. For more information, please look at  [Scriban Documentation](https://github.com/lunet-io/scriban).

`cultureName` is your destination rendering culture. When it is not exist, it will use the default culture. 

> If `cultureName` has a language tag it will try to find exact culture with tag, if it is not exist it will use the language family.

> Example: If you try to render content with _"es-MX"_ it will search your template with  _"es-MX"_ culture, when it fails to find, it will try to render _"es"_ culture content. If still can't find it will render the default culture content that you defined.

`globalContext` = TODO

## Template Content Provider

When you want to get your stored template content you can use `ITemplateContentProvider`. 

`ITemplateContentProvider` has one method that named `GetContentOrNullAsync` with two different overriding, and it returns you a string of template content or null. (**without rendering**)

- `templateName` (_string_) or `templateDefinition` (_`TemplateDefinition`_)
- `cultureName` (_string_)
- `tryDefaults` (_bool_)
- `useCurrentCultureIfCultureNameIsNull` (_bool_)

### Usage

First parametres of `GetContentOrNullAsync` (`templateName` or `templateDefinition`) are required, the other three parametres can be null.

If you want to get exact culture content, set `tryDefaults` and `useCurrentCultureIfCultureNameIsNull` as a `false`. Because the `GetContentOrNullAsync` tries to return content of template.

> Example Scenario

> If you have a template content that culture "`es`", when you try to get template content with "`es-MX`" it will try to return first "`es-MX`", if it fails it will return "`es`" content. If you set `tryDefaults` and `useCurrentCultureIfCultureNameIsNull` as `false` it will return `null`.

## Template Definition Manager

When you want to get your `Template Definitions`, you can use a singleton service that named `Template Definition Manager` in runtime.

To use it, inject `ITemplateDefinitionManager` service. 

It has three method that you can get your Template Definitions.

- `Get`
- `GetOrNull`
- `GetAll`

`Get` and `GetOrNull` requires a string parameter that name of template definition. `Get` will throw error when it is not exist but `GetOrNull` returns `null`.

`GetAll` returns you all registered template definitions.

## Template Content Contributor

You can store your `Template Contents` in any resource. To make it, just create a class that implements `ITemplateContentContributor` interface. 

`ITemplateContentContributor` has a one method that named `GetOrNullAsync`. This method must return content **without rendering** if that is exist in your resource or must return `null`. 