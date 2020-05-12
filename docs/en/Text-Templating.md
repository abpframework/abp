# Text Templating

In ABP Framework, `text template` is a mixture of text blocks and control logic that can generate a `string` result. An open source package [Scriban](https://github.com/lunet-io/scriban) is used for the control logic and [Abp.Localization](Localization.md) is used to make content easily localizable. The generated string can be text of any kind, such as a web page, an e-mail content etc.

> **stored content**
```html
<ol>
{{~ for $i in 0..3 ~}}
 <li>{{ L "WelcomeMessage" }}</li>
{{~ endfor ~}}
</ol>
```
> **result** (for en culture)
```
1. Welcome to the abp.io!
2. Welcome to the abp.io!
3. Welcome to the abp.io!
4. Welcome to the abp.io!
```

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

## Logic

A Text Template is a combination of two object.
- `TemplateDefinition`
- `TemplateContent`

### Template Definition

Template Definition is an object that contains some information about your `Text Templates`. Template Definition object contains the following properties.

- `Name` *(string)*
- `IsLayout` *(boolean)*
- `Layout` *(string)* contains the <u>name of layout template</u>
- `LocalizationResource` *(Type)*  <u>Inline Localized</u>
- `IsInlineLocalized`*(boolean)* describes that the template is inline localized or not
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

You can localize your Text Templates by choosing two different method.

- `Inline Localization`
- `Multiple Content Localization`

#### Inline Localization

Inline localized Text Templates is using only one content resource, and it is using the [Abp.Localization](Localization.md) to get content in different languages/cultures. 

> Example Inline Localized Text Template: 
>
> ForgotPasswordEmail.tpl

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

It is typical to use the same layout for some different Text Templates. So, you can define a layout template. 

A text template can be layout for different text templates and also a text template may use a layout.

A layout Text Template must have `{{content}}` area to render the child content. _(just like the `RenderBody()` in the MVC)_

> Example Email Layout Text Template

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

## Definition a Text Template

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

`ITemplateRenderer` service has one method that named `RenderAsync` and to render your content and it is requires some parametres. 

- `templateName` (string) **_Not Null_**
- `model` (object) **_Can Be Null_**
- `cultureName` (string) **_Can Be Null_**
- `globalContext` (dictionary) **_Can Be Null_**

`templateName` is exactly same with Template Definition Name.

`model` is a dynamic object. This is using to put dynamic data into template. For more information, please look at  [Scriban Documentation](https://github.com/lunet-io/scriban).

`cultureName` is your rendering destination culture. When it is not exist, it will use the default culture. 

> If `cultureName` has a language tag it will try to find exact culture with tag, if it is not exist it will use the language family.

> If you try to render content with _"es-MX"_ it will search your template with  _"es-MX"_ culture, when it fails to find, it will try to render _"es"_ culture content. If still can't find it will render the default culture content that you defined.

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

`ITemplateContentContributor` has a one method that named `GetOrNullAsync`. This method must return content **without rendering** if it finds in your resource or returns `null`. 