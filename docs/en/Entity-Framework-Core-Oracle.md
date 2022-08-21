# Switch to EF Core Oracle Provider

> [ABP CLI](CLI.md) and the [Get Started](https://abp.io/get-started) page already provides an option to create a new solution with Oracle. See [that document](Entity-Framework-Core-Other-DBMS.md) to learn how to use. This document provides guidance for who wants to manually switch to Oracle after creating the solution.

This document explains how to switch to the **Oracle** database provider for **[the application startup template](Startup-Templates/Application.md)** which comes with SQL Server provider pre-configured.

ABP Framework provides integrations for two different Oracle packages. See one of the following documents based on your provider decision:

* **[Volo.Abp.EntityFrameworkCore.Oracle](Entity-Framework-Core-Oracle-Official.md)** package uses the official & free oracle driver.
* **[Volo.Abp.EntityFrameworkCore.Oracle.Devart](Entity-Framework-Core-Oracle-Devart.md)** package uses the commercial (paid) driver of [Devart](https://www.devart.com/) company.

> You can choose one of the package you want. If you don't know the differences of the packages, please search for it. ABP Framework only provides integrations it doesn't provide support for such 3rd-party libraries.
