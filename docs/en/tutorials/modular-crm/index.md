## Modular Monolith Application Development Tutorial

ABP provides a great infrastructure and tooling to build modular software solutions. In this tutorial, you will learn how to create application modules, compose and communicate them to build a monolith modular web application.

## Tutorial Outline

This tutorial is organized as the following parts:

* *TODO*

## Creating the Solution

Follow the [Get Stared](../../get-started/layered-web-application.md) guide to create a new layered web application with [ABP Studio](../../studio/index.md) the following configuration:

* **Solution name**: `ModularCrm`
* **UI Framework**: ASP.NET Core MVC / Razor Pages
* **Database Provider**: Entity Framework Core

You can select the other options based on your preference.

> **Please complete the [Get Stared](../../get-started/layered-web-application.md) guide and run the web application.**

The initial solution structure should be like the following in ABP Studio's *Solution Explorer*:

![solution-explorer-modular-crm-initial](images/solution-explorer-modular-crm-initial.png)

Initially, you see a `ModularCrm` solution and a `ModularCrm` module under that solution.

> An ABP Studio module is typically a .NET solution and an ABP Studio solution is an umbrella concept for multiple .NET Solutions (see the [concepts](../../studio/concepts.md) document for more).

`ModularCrm` module is your main application, which is a layered .NET solution that consists of several packages (.NET projects). You can expand the `ModularCrm` module to see these packages:

![solution-explorer-modular-crm-expanded](images/solution-explorer-modular-crm-expanded.png)







## Download the Source Code

*TODO*