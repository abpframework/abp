```json
//[doc-seo]
{
    "Description": "Learn how to configure CORS in ABP Framework's layered solution template for various web application types, enhancing security and functionality."
}
```

# Layered Solution: CORS Configuration

```json
//[doc-nav]
{
  "Previous": {
    "Name": "BLOB Storing",
    "Path": "solution-templates/layered-web-application/blob-storing"
  },
  "Next": {
    "Name": "Health Check Configuration",
    "Path": "solution-templates/layered-web-application/health-check-configuration"
  }
}
```

Cross-Origin Resource Sharing (CORS) is a security feature that allows web applications to make requests to a different domain than the one that served the web page.

In the layered solution template, CORS configuration is applied in the following cases:
- If you select the [Tiered solution](solution-structure.md#tiered-structure-).
- If you choose [Angular](web-applications.md#angular) as the web application type.
- If you choose [Blazor WebAssembly](web-applications.md#blazor-webassembly) as the web application type.
- If you choose [No UI](web-applications.md#no-ui) as the web application type.

The CORS settings are configured in the `appsettings.json` file of the corresponding project. Typically, the web application serves as the entry point for front-end applications, so it must be configured to accept requests from different origins. 

The default configuration in `appsettings.json` is as follows:

```json
{
  "App": {
    "CorsOrigins": "https://*.MyProjectName.com"
  }
}
```

You can modify the `CorsOrigins` property to include additional domains or wildcard subdomains as required by your application.