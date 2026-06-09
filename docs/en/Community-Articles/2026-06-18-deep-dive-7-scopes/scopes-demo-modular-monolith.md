# Scopes Demo: Modular Monolith Or Microservice Solution

This is a working demo outline for the Scopes deep-dive article.

The goal is to create a small, visual story that shows why AI Scopes matter in ABP Studio AI Coding Agent. The demo should not only show that the agent can edit files. It should show that the agent can work inside a deliberate boundary.

## Demo Message

The main message of the demo:

> ABP AI Coding Agent can be powerful without being pointed at the whole solution. With AI Scopes, the developer chooses the relevant business area first, then lets the agent plan, implement, and validate inside that boundary.

This is especially useful for ABP solutions because they often have strong module, package, and application boundaries.

## Recommended Demo Shape

Use one of these solution shapes:

* a modular monolith with `Public`, `Admin`, `Catalog`, `Ordering`, and `Identity` areas,
* or a microservice solution with `CatalogService`, `OrderingService`, `AdministrationService`, `IdentityService`, and a gateway or public web application.

The article can describe the demo with a modular monolith because it is easier to understand in a short community article. The same mental model can then be applied to a microservice solution.

## Suggested Folder Or Module Layout

Use a structure like this for the screenshots and explanation:

```text
Acme.BookStore
  modules/
    catalog/
      Acme.BookStore.Catalog.Domain
      Acme.BookStore.Catalog.Application
      Acme.BookStore.Catalog.Application.Contracts
      Acme.BookStore.Catalog.EntityFrameworkCore
      Acme.BookStore.Catalog.HttpApi
    ordering/
      Acme.BookStore.Ordering.Domain
      Acme.BookStore.Ordering.Application
      Acme.BookStore.Ordering.Application.Contracts
    administration/
      Acme.BookStore.Administration.Application
      Acme.BookStore.Administration.Web
  apps/
    public-web
    admin-web
    auth-server
```

The exact names do not matter. What matters is that the screenshots clearly show multiple business areas and that the selected scope intentionally includes only the area needed for the task.

## Scope Configuration

For the demo, create a scope such as:

```text
Public Catalog Work
```

Include:

* the public web application,
* the Catalog module or packages,
* shared contracts only if the task genuinely needs them.

Exclude:

* admin UI packages,
* ordering packages,
* identity or account packages,
* unrelated infrastructure folders,
* generated output folders.

If the selected scope UI supports module/package selection, prefer that over plain folder naming because it shows ABP Studio's solution awareness more clearly.

## Demo Task

Use a realistic task that belongs to the selected scope:

```text
Use the selected Public Catalog Work scope.
Create a plan to improve the product search filter on the public side.
Keep the change inside the public catalog flow.
If you need a package outside the selected scope, explain why before implementation.
```

This prompt shows the intended behavior:

1. The agent starts inside the selected scope.
2. It plans the change without scanning unrelated modules.
3. It should not inspect or edit the admin or ordering areas.
4. It should ask for scope expansion if the requirement crosses a real boundary.

## Expected Agent Behavior

The successful demo should show the agent:

* identifying the relevant public/catalog files,
* explaining the likely application service, DTO, or UI surface involved,
* avoiding unrelated admin and ordering files,
* producing a focused plan,
* implementing only inside the selected scope when switched to Agent mode,
* validating the changed area with a targeted build or workflow.

The ideal result is a small diff that clearly matches the selected scope.

## Suggested Screenshots

> **TODO:** Capture these screenshots in ABP Studio and copy them into the Scopes article folder.

1. Solution explorer showing multiple modules or services.
2. AI Scope selector with `Public Catalog Work` selected.
3. Scope configuration showing included modules/packages.
4. Prompt sent to the agent.
5. Agent plan that lists only scoped files.
6. Agent message explaining that another module would require scope expansion, if applicable.
7. Final diff or Git view showing focused changes.

## Optional Microservice Variant

For a microservice version, use a task like:

```text
Use the Catalog Service scope.
Add a small validation rule to product creation.
Do not inspect Ordering Service or Administration Service unless a shared contract requires it.
```

The point is the same: the agent works with the service that owns the change, not the whole platform.

This variant is useful if the article wants to emphasize that ABP Studio AI Scopes are not only for small applications. They are also useful when a solution has many runnable services, each with its own package set and runtime role.

## Article Section Draft

The final article section can say:

> In this demo, I configured a scope for the public catalog flow. The solution also had admin and ordering areas, but they were intentionally outside the active scope. Then I asked ABP AI Coding Agent to improve product search filtering. The interesting part was not only the generated code. The interesting part was the boundary: the agent stayed focused on the public catalog flow and treated any cross-module dependency as something that needed an explicit scope decision.

That is the moment the feature becomes easy to understand. Scopes turn "please be careful" into a visible part of the agent workflow.
