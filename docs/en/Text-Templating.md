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

Inline localized Text Templates is using only one content resource, and it is using the `Abp.Localization` to get content in different languages/cultures. 

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

> For default, ABP uses **`Virtual Files`** for text templates. All given examples are for `Virtual File Text Template Definitions`.

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

As you see in the given example all Text Templates are added with `(ITemplateDefinitionContext)context.Add` method. This method requires a `TemplateDefinition` object. Then we call `WithVirtualFilePath` method with chaining for the describe where is the virtual files.



## Getting Template Definitions

### Template Definition Manager

## Getting Template Contents

### Template Content Contributor

## Rendering
