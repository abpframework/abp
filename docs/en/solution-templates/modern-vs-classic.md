```json
//[doc-seo]
{
    "Description": "Compare Modern and Classic ABP Studio templates, including architecture mapping, UI choices, mobile options, and when to choose each template family."
}
```

# Modern vs Classic Templates

ABP Studio provides two solution template families: **Modern** and **Classic**. Both families create production-ready ABP solutions and share the same ABP backend concepts, such as Entity Framework Core and MongoDB database options, OpenIddict authentication, multi-tenancy, optional modules, test projects, language selection and deployment-related configuration where they are supported.

The main difference is how you choose and shape the solution. **Classic templates** are the traditional template-first flow. You first choose a concrete template, such as Single-Layer, Layered or Microservice, and then select UI, database, mobile and module options. **Modern templates** are the newer architecture-first flow. You first choose the backend architecture, then ABP Studio maps your selection to the proper modern template.

## Template Families

| Template family | Template names | Main idea |
| --- | --- | --- |
| Classic | `app-nolayers`, `app`, `microservice` | The traditional ABP Studio solution templates with the broadest UI framework choices. |
| Modern | `app-nolayers-modern`, `app-modern`, `microservice-modern` | The newer ABP Studio templates focused on React-based web applications and an architecture-first creation flow. |

## Modern Architecture Mapping

When you use the Modern solution creation flow in ABP Studio, your selected architecture maps to a modern template:

| Modern architecture | Generated template | Notes |
| --- | --- | --- |
| Simple Monolith | `app-nolayers-modern` | A simpler application structure with the main backend code in one host project. |
| Layered Monolith | `app-modern` | A layered solution based on Domain Driven Design practices. |
| Modular Monolith | `app-nolayers-modern` | Uses the modern single-layer template with modular solution options enabled. |
| Microservice | `microservice-modern` | A distributed solution with dedicated services, gateways and applications. |

## Practical Differences

| Area | Classic templates | Modern templates |
| --- | --- | --- |
| Creation flow | Template-first: select Single-Layer, Layered or Microservice first. | Architecture-first: select Simple Monolith, Layered Monolith, Modular Monolith or Microservice first. |
| Web UI choices | Supports MVC / Razor Pages, Angular, Blazor WebAssembly, Blazor Server, Blazor Web App, MAUI Blazor and No UI depending on the selected template. | Supports React or No UI. |
| Mobile choices | Keeps broader mobile choices, including MAUI and React Native in templates that support mobile applications. | Supports React Native or no mobile application. |
| Public website | Uses the classic public website structure where the selected template supports it. | Uses React-based public web assets where the selected modern template supports a public website. |
| Frontend assets | Uses the established ABP UI framework integrations and theme options for MVC, Angular and Blazor applications. | Uses newer React and `shadcn`-oriented frontend assets. |
| Best fit | Existing projects, teams using MVC / Angular / Blazor / MAUI, and scenarios that need the broadest UI framework choices. | New React-focused solutions and teams that prefer the newer Studio creation experience. |

## Which One Should I Choose?

Choose **Modern** if you are starting a new solution with React UI, want a React Native mobile application, prefer the newer architecture-first ABP Studio flow, or plan to use AI-assisted programming for an AI-driven project.

Choose **Classic** if you want MVC / Razor Pages, Angular, Blazor, MAUI Blazor or MAUI mobile options, or if your team is following existing Classic-template tutorials, conventions or project structure.

ABP Studio may show or hide some templates and options based on your license. For example, Microservice templates require a higher license level than regular application templates, and Modern application templates are not shown in the Community Edition.

## See Also

* [Solution Template Selection Guide](guide.md)
* [Startup Solution Templates](index.md)
* [Get Started](../get-started/index.md)
