# Switch to EF Core Oracle Provider

> [ABP CLI](../../../cli/index.md) and the [Get Started](https://abp.io/get-started) page already provides an option to create a new solution with Oracle. See [that document](other-dbms.md) to learn how to use. This document provides guidance for who wants to manually switch to Oracle after creating the solution.

This document explains how to switch to the **Oracle** database provider for **[the application startup template](../../../solution-templates/layered-web-application/index.md)** which comes with SQL Server provider pre-configured.

ABP provides integrations for two different Oracle packages. See one of the following documents based on your provider decision:

* **[`Volo.Abp.EntityFrameworkCore.Oracle`](./oracle-official.md)** package uses the official & free oracle driver.
* **[`Volo.Abp.EntityFrameworkCore.Oracle.Devart`](./oracle-devart.md)** package uses the commercial (paid) driver of [Devart](https://www.devart.com/) company.

> You can choose one of the package you want. If you don't know the differences of the packages, please search for it. ABP only provides integrations it doesn't provide support for such 3rd-party libraries.
