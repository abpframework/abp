# ABP Platform 10.7 RC Has Been Released

We are happy to release [ABP](https://abp.io) version **10.7 RC** (Release Candidate). This blog post introduces the new features and important changes in this version.

Try this version and provide feedback to help us deliver a more stable ABP v10.7 release. Thanks in advance!

## Get Started with the 10.7 RC

You can check the [Get Started page](https://abp.io/get-started) to see how to get started with ABP. You can either download [ABP Studio](https://abp.io/get-started#abp-studio-tab) (**recommended**, if you prefer a user-friendly GUI application - desktop application) or use the [ABP CLI](https://abp.io/docs/latest/cli).

By default, ABP Studio uses stable versions to create solutions. Therefore, if you want to create a solution with a preview version, first you need to create a solution and then switch your solution to the preview version from the ABP Studio UI:

![studio-switch-to-preview](https://raw.githubusercontent.com/abpframework/abp/refs/heads/dev/docs/en/Blog-Posts/2026-08-05%20v10_7_Preview/studio-switch-to-preview.png)

## Migration Guide

Check the [ABP Version 10.7 Migration Guide](https://abp.io/docs/10.7/release-info/migration-guides/abp-10-7) before upgrading from v10.6 or earlier. It covers the services that take new constructor dependencies, the Blazor antiforgery middleware order, the dependency updates, and the AI Management schema change that requires a new EF Core migration.

## What's New with ABP v10.7?

In this section, I will introduce some major features released in this version.
Here is a brief list of titles explained in the next sections:

- BLOB Encryption at Rest and Content Pipeline
- HTTP QUERY Method Support
- Angular Resource API Proxies
- ABP Suite React CRUD Page Generation
- ABP Suite Decimal Precision
- ABP Studio MCP Configuration
- AI Management Web Page Data Sources
- Dependency Updates
- Other Improvements and Enhancements

### BLOB Encryption at Rest and Content Pipeline

ABP v10.7 adds opt-in, transparent encryption at rest for the BLOB Storing system. Encryption uses AES-256-GCM and works on top of the configured storage provider, so application code can continue using `IBlobContainer` as before. It requires a platform with AES-GCM support and is not available on .NET Standard 2.0 targets.

You can enable encryption per container and configure the passphrase from your application's secure configuration:

```csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.Configure<ProfilePictureContainer>(container =>
    {
        container.UseEncryption();
    });
});

Configure<AbpBlobStoringEncryptionOptions>(options =>
{
    options.DefaultPassPhrase = context.Configuration["MyApp:BlobPassPhrase"];
});
```

The new BLOB content pipeline lets you transparently transform content when it is saved and read. You can create contributors for compression, validation, watermarking, or other stream transformations without changing the storage provider or the code that uses the container.

Both features are disabled by default. When enabling encryption for a container that already contains plaintext BLOBs, first allow legacy plaintext reads, re-save the existing content, and then remove the legacy option so the container reads encrypted data only.

> See the [BLOB Encryption](https://abp.io/docs/10.7/framework/infrastructure/blob-storing/encryption) and [BLOB Content Pipeline](https://abp.io/docs/10.7/framework/infrastructure/blob-storing/pipeline) documents and [#25836](https://github.com/abpframework/abp/pull/25836) for details.

### HTTP QUERY Method Support

ABP now supports the HTTP `QUERY` method for endpoints that need to send request data without using a query string. A `QUERY` endpoint is treated as a safe method: like `GET`, it starts a non-transactional unit of work and is not audited by default. `GET`, `HEAD` and `QUERY` share the `AbpAuditingOptions.IsEnabledForGetRequests` setting.

To expose an action as a `QUERY` endpoint, use the ASP.NET Core `[AcceptVerbs("QUERY")]` attribute. Because the method carries a request body, it still requires an anti-forgery token.

> See the [Auto API Controllers](https://abp.io/docs/10.7/framework/api-development/auto-controllers#http-method) documentation and [#25797](https://github.com/abpframework/abp/pull/25797) for details.

### Angular Resource API Proxies

The Angular proxy generator can now generate the `GET` endpoints against the Resource API. Pass the `--resource-api` option and every generated `GET` member returns an `rxResource`-based `ResourceRef` instead of an `Observable`. An endpoint with parameters takes them as a single `Signal`, a parameterless endpoint has no signal parameter, and the optional request configuration stays a normal argument. The other HTTP methods keep the Observable-based form.

This option requires Angular 22 or later and is disabled by default, so existing generated proxies continue to work without changes. Regenerate the proxies with the option only when you are ready to consume the resource form in your components.

> See the [Angular Service Proxies](https://abp.io/docs/10.7/framework/ui/angular/service-proxies) documentation and [#25761](https://github.com/abpframework/abp/pull/25761) for details.

### ABP Suite React CRUD Page Generation

ABP Suite now supports generating CRUD pages for the React applications in modern solutions, bringing the same productive code-generation experience available for other ABP UI options to React projects. The generation is template-based and does not use AI.

Generated React pages include list, search, sorting, paging, filtering, export, create, edit, single and bulk delete operations. They also support validation, permissions, localization, file upload, navigation properties, many-to-many relationships, and master-detail pages with child create, edit, delete, and paging operations.

The generator respects the entity and field configuration you define in ABP Suite, including `ShowOn*`, `IsFilterable`, and `ReadonlyOnEditModal` options. Navigation lookups use server-side search.

![React CRUD page generation demo](react-crud-page.mp4)

### ABP Suite Decimal Precision

You can now set the precision and scale of a `decimal` property in ABP Suite. For the relational database providers that support fixed-point columns, the generated entity configuration includes the matching `HasPrecision(...)` call.

### ABP Studio MCP Configuration

ABP Studio provides a simpler experience for configuring Model Context Protocol (MCP) integrations. You can add common integrations through focused configuration forms or manage the complete MCP server list as JSON, with support for secret placeholders and secure platform storage. It is available in ABP Studio v3.0.9 and later.

> See the [ABP Studio AI Agent configuration](https://abp.io/docs/10.7/studio/ai-agent-configuration) documentation and [#25870](https://github.com/abpframework/abp/pull/25870) for details.

### AI Management Web Page Data Sources

A workspace data source can now be created from a web page URL, not only from an uploaded file. The page content is converted to markdown and indexed like any other data source, and you can refresh it later to pick up changes to the page.

The model name fields of the workspace configuration can also suggest the available models of the selected provider, so you don't have to remember the exact model names. The OpenAI and Ollama model catalogs are included; a provider without a registered catalog simply has no suggestions.

### Dependency Updates

ABP v10.7 RC includes the following dependency updates:

- MudBlazor upgraded to **9.7.0**
- `MySql.EntityFrameworkCore` upgraded to **10.0.9**

> Check the [Package Version Changes](https://abp.io/docs/10.7/package-version-changes) document for all updates.

### Other Improvements and Enhancements

- **BLOB storing**: The storage providers have improved support for transformed and non-seekable streams.
- **Identity sessions**: The inactive session cleanup uses the sign-in time when a session has not recorded a last-accessed time yet, so valid token sessions are not removed too early.
- **Identity**: The user's last sign-in time is written as a best-effort update in its own unit of work, so a concurrency conflict no longer fails the sign-in request ([#25905](https://github.com/abpframework/abp/pull/25905)).
- **Blazor templates**: `UseAntiforgery()` is called after `UseAuthorization()`, which is the order required by ASP.NET Core. Existing solutions keep their own middleware order, so check the migration guide ([#25874](https://github.com/abpframework/abp/pull/25874)).
- **MySQL**: The `Guid[]` query parameters are mapped correctly, and the passkey and user invitation columns are stored as `json`.

## Community News

### New ABP Community Articles

As always, exciting articles have been contributed by the ABP community. I will highlight some of them here:

- [How I Use a Custom AI Skill to Upgrade a Large ABP Solution](https://abp.io/community/articles/how-i-use-a-custom-ai-skill-to-upgrade-a-large-abp-solution-h5fllft1) by [Kori Francis](https://github.com/kfrancis)
- [Why Does My Tiered ABP App Show an Empty Menu While the User Is Still Signed In?](https://abp.io/community/articles/why-does-my-tiered-abp-app-show-an-empty-menu-while-the-user-7g46886w) by [Kori Francis](https://github.com/kfrancis)

Thanks to the ABP Community for all the content they have published. You can also [post your ABP related (text or video) content](https://abp.io/community/posts/create) to the ABP Community.

## Conclusion

This version comes with some new features and a lot of enhancements to the existing features. You can see the [Road Map](https://abp.io/docs/10.7/release-info/road-map) documentation to learn about the release schedule and planned features for the next releases. Please try ABP v10.7 RC and provide feedback to help us release a more stable version.

For the complete list of changes, see the [ABP 10.7.0-rc.1 release notes](https://github.com/abpframework/abp/releases/tag/10.7.0-rc.1).

Thanks for being a part of this community!
