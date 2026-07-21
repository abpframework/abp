```json
//[doc-seo]
{
    "Description": "Learn how ABP Studio's modern Modular Monolith solution template organizes a main application and reusable modules in a single deployable solution."
}
```

# ABP Studio: Modular Monolith Solution Template

````json
//[doc-nav]
{
  "Previous": {
    "Name": "Layered Solution",
    "Path": "solution-templates/layered-web-application/index.md"
  },
  "Next": {
    "Name": "Microservice Solution",
    "Path": "solution-templates/microservice/index.md"
  }
}
````

ABP Studio's modern solution wizard includes a dedicated **Modular Monolith** architecture. Under the hood, it uses the modern no-layers application template for the main application, then enables modularity automatically.

> **This page documents the modern modular monolith path. If you are working with the classic template family, see the [Solution Template Selection Guide](../guide.md) for the host + module composition approach.**

## What ABP Studio Creates

When you choose `Modular Monolith` in the modern solution wizard, ABP Studio:

* creates the main application by using the modern no-layers host template.
* locks the solution into modular mode.
* creates a `main` folder in the solution model and moves the main application into it.
* creates a `modules` folder for reusable module solutions.
* lets you add additional modules during solution creation and optionally install them into the main application immediately.

## Typical Layout

In ABP Studio's *Solution Explorer*, the solution is organized around these areas:

* `main`: the main application and its host-side assets.
* `modules`: one module solution per business capability.
* `etc`: shared infrastructure, run profiles, and deployment files.

Additional modules are generated under the solution's `modules/<module-name>` directory, while the main application remains the single deployable host.

## How Modules Fit In

The module solutions created for a modular monolith use the same reusable module concepts documented in the [Application Module Template](../application-module/index.md) page.

Use this template when you want:

* clear module boundaries without a distributed deployment model.
* separate module solutions for teams or business domains.
* a monolith today, with a cleaner path toward microservices later.

## See Also

* [Solution Template Selection Guide](../guide.md)
* [Application Module Template](../application-module/index.md)
* [Modular Monolith Application Development Tutorial](../../tutorials/modular-crm/index.md)
