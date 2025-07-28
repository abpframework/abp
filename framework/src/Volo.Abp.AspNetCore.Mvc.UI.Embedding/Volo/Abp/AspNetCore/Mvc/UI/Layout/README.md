# Page Embedding Feature

This feature allows ABP MVC pages to be embedded in other UI technologies (like iframes) by conditionally disabling layout elements.

## Configuration

### 1. Using Query Parameters

Add `?embed=true` to any page URL to render it without layout elements:

```
https://myapp.com/MyPage?embed=true
https://myapp.com/Identity/Users?embed=1
```

### 2. Automatic IFrame Detection

Enable automatic iframe detection to embed all pages when accessed from iframes:

```csharp
Configure<PageEmbeddingOptions>(options =>
{
    // Automatically embed pages when accessed from iframes
    options.AlwaysEmbedIFrameRequests = true;
});
```

### 3. Configuring Specific Paths

Configure specific paths and patterns for embedding:

```csharp
Configure<PageEmbeddingOptions>(options =>
{
    // Specific paths that should always be embedded
    options.EmbeddedPaths.Add("/embed/dashboard");
    options.EmbeddedPaths.Add("/reports/widget");
    
    // Path patterns with wildcards
    options.EmbeddedPathPatterns.Add("/api/embed/*");
    options.EmbeddedPathPatterns.Add("*/widget");
    
    // Customize query parameter name and values
    options.QueryParameterName = "iframe";
    options.QueryParameterValues.Add("yes");
    
    // Enable automatic iframe detection
    options.AlwaysEmbedIFrameRequests = true;
});
```

## How It Works

1. The `PageLayout.RenderLayoutElements` property is automatically set to `false` when embedding conditions are met
2. Themes check this property to conditionally render navigation, headers, footers, etc.
3. The `PageEmbeddingService` evaluates multiple factors:
   - **IFrame detection** (when `AlwaysEmbedIFrameRequests = true`):
     - `Sec-Fetch-Dest: iframe` header (most reliable)
     - `Sec-Fetch-Site` + `Sec-Fetch-Mode` headers
     - Custom headers (`X-Frame-Request`, `X-Iframe-Request`, etc.)
     - `X-Requested-With` header
     - Referer header analysis (least reliable)
   - Query parameters (`?embed=true`)
   - Configured embedded paths
   - Path patterns with wildcards

## Theme Integration

Themes already support this feature through the existing `PageLayout.RenderLayoutElements` property. No changes needed in theme implementations.

## Security Considerations

- Embedding should be carefully controlled to prevent clickjacking attacks
- Consider implementing CORS policies for cross-origin embedding
- Validate the source of embedding requests when necessary 