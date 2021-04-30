# Text Templating

## Introduction

ABP Framework provides a simple, yet efficient text template system. Text templating is used to dynamically render contents based on a template and a model (a data object):

Template + Model =renderer=> Rendered Content

It is very similar to an ASP.NET Core Razor View (or Page):

*RAZOR VIEW (or PAGE) + MODEL ==render==> HTML CONTENT*

You can use the rendered output for any purpose, like sending emails or preparing some reports.

Template rendering engine is very powerful;

* It supports **conditional logics**, **loops** and much more.
* Template content **can be localized**.
* You can define **layout templates** to be used as the layout while rendering other templates.
* You can pass arbitrary objects to the template context (beside the model) for advanced scenarios.

ABP Framework provides two types of engines;

* **[Razor](Text-Templating-Razor.md)**
* **[Scriban](Text-Templating-Scriban.md)**

## Source Code

Get [the source code of the sample application](https://github.com/abpframework/abp-samples/tree/master/TextTemplateDemo) developed and referred through this document.

## See Also

* [The source code of the sample application](https://github.com/abpframework/abp-samples/tree/master/TextTemplateDemo) developed and referred through this document.
* [Localization system](Localization.md).
* [Virtual File System](Virtual-File-System.md).
