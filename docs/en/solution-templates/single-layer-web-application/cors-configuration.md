```json
//[doc-seo]
{
    "Description": "Learn how to configure CORS for single-layer solutions in ABP Framework, ensuring secure cross-origin requests for your web applications."
}
```

# Single Layer Solution: CORS Configuration

```json
//[doc-nav]
{
  "Previous": {
    "Name": "BLOB Storing",
    "Path": "solution-templates/single-layer-web-application/blob-storing"
  },
  "Next": {
    "Name": "Health Check Configuration",
    "Path": "solution-templates/single-layer-web-application/health-check-configuration"
  }
}
```

Cross-Origin Resource Sharing (CORS) is a security feature that allows web applications to make requests to a different domain than the one that served the web page.

In the single-layer solution template, CORS configuration is applied in the following cases:
- When [Angular](web-applications.md#angular) is selected as the web application type.
- When [Blazor WebAssembly](web-applications.md#blazor-webassembly) is selected as the web application type.
- When [No UI](web-applications.md#no-ui) is selected as the web application type.

CORS settings are configured in the `appsettings.json` file of the corresponding project. The web application usually serves as the entry point for front-end applications, so it must be set up to accept requests from different origins.

The default configuration in `appsettings.json` is as follows:

```json
{
  "App": {
    "CorsOrigins": "https://*.MyProjectName.com"
  }
}
```

You can modify the `CorsOrigins` property to include additional domains or wildcard subdomains as needed for your application.