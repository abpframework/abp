# Optimizing Static Asset Delivery feature in ASP.NET Core 9.0

Delivering static assets efficiently is a key factor in building performant web applications. By optimizing how assets like CSS, JavaScript, and images are served to the browser, you can reduce load times, decrease network traffic, and improve the overall user experience.

One powerful tool to help achieve this is **MapStaticAssets**, a feature in ASP.NET Core that significantly optimizes the delivery of static resources. Whether you're working with Blazor, Razor Pages, MVC, or other UI frameworks, **MapStaticAssets** streamlines asset management and ensures that your web app delivers resources in the most efficient way possible.

## Why Optimizing Static Assets Matters

Serving static assets without optimization can lead to several performance bottlenecks:

- **Excessive network requests**: The browser may need to request the same resources multiple times, even if they haven’t changed.
- **Unnecessary data transfer**: Larger files are sent over the network, consuming bandwidth and slowing down page loads.
- **Outdated assets**: Without proper cache management, users may receive stale versions of files after an app update.

Optimizing static assets involves compressing files, managing caching headers, and ensuring that only the necessary resources are sent to the client. **MapStaticAssets** takes care of all these issues in a seamless, automated way.

## What is MapStaticAssets?

**MapStaticAssets** is designed to enhance the default static asset serving mechanism in ASP.NET Core. It can replace `UseStaticFiles` in most scenarios and comes with several built-in optimizations. These optimizations are executed at both build and publish time, ensuring that static resources are served in the most efficient way possible when your app is running.

Here's how you can implement **MapStaticAssets** in your app:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

// Replacing UseStaticFiles with MapStaticAssets
app.MapStaticAssets();
app.MapRazorPages();

app.Run();
```

## Key Features of MapStaticAssets

1. **Build-time Compression**:  
   **MapStaticAssets** automatically compresses all static assets during the build process. It uses **gzip** compression during development and **gzip + brotli** compression when publishing. This reduces the file size significantly, ensuring faster download times.

   For example, in a default Razor Pages template, assets like `bootstrap.min.css` and `jquery.js` are compressed by over 80%, resulting in significantly reduced file sizes:

   | File                 | Original Size | Compressed Size | Compression Reduction |
   |----------------------|---------------|-----------------|-----------------------|
   | `bootstrap.min.css`  | 163 KB        | 17.5 KB         | 89.26%                |
   | `jquery.js`          | 89.6 KB       | 28 KB           | 68.75%                |
   | `bootstrap.min.js`   | 78.5 KB       | 20 KB           | 74.52%                |
   | **Total**            | 331.1 KB      | 65.5 KB         | 80.20%                |

2. **Content-based ETags**:  
   **MapStaticAssets** generates **ETags** based on the SHA-256 hash of the file content, encoded in Base64. This ensures that the browser only re-downloads a resource if its content has changed. This eliminates unnecessary network requests, improving page load speeds.

3. **Smaller File Sizes for Libraries**:  
   Popular component libraries, such as **Fluent UI Blazor** and **MudBlazor**, benefit from similar compression optimizations. For example, the size of the **MudBlazor** library is reduced by over 90%, from 588 KB to just 46.7 KB after compression.

   | File                 | Original Size | Compressed Size | Compression Reduction |
   |----------------------|---------------|-----------------|-----------------------|
   | `MudBlazor.min.css`  | 541 KB        | 37.5 KB         | 93.07%                |
   | `MudBlazor.min.js`   | 47.4 KB       | 9.2 KB          | 80.59%                |
   | **Total**            | 588.4 KB      | 46.7 KB         | 92.07%                |

4. **Automatic Optimization**:  
   As libraries or components are added or updated, **MapStaticAssets** automatically optimizes the assets as part of the build process. This includes minimizing the size of JavaScript and CSS files, reducing the impact of mobile or low-bandwidth environments.

5. **Serving Assets with a CDN**:  
   Although **MapStaticAssets** is focused on server-side optimizations, integrating a **CDN (Content Delivery Network)** can further boost performance by serving static assets from servers geographically closer to the user, reducing latency.

## Comparing MapStaticAssets to IIS Dynamic Compression

**MapStaticAssets** provides several advantages over traditional dynamic compression techniques, such as IIS **gzip** compression:

- **Simplicity**: There is no need for server-specific configuration, making **MapStaticAssets** easy to implement.
- **Performance**: By compressing assets at build time, the app doesn't need to perform compression during every request, which improves server performance.
- **Optimization**: Developers can focus on ensuring that assets are compressed to the smallest possible size during the build process.

For example, using **MapStaticAssets**, a file like `MudBlazor.min.css` is compressed down to 37.5 KB, whereas IIS dynamic compression might result in a size of 90 KB. This represents a **59%** reduction in size.

## About MapAbpStaticAssets

The ABP framework is 100% compatible with this new feature.

However, some JavaScript, CSS, and image files exist in the [Virtual File System](https://abp.io/docs/latest/framework/infrastructure/virtual-file-system), which ASP.NET Core's **MapStaticAssets** can't handle. For these files, additional **StaticFileMiddleware** is needed to serve them, which is where **MapAbpStaticAssets** comes in.

**MapAbpStaticAssets** adds the necessary **StaticFileMiddleware** to ensure that virtual files are correctly served. This middleware setup ensures seamless delivery of virtual resources alongside static assets.

You can view the source code of **MapAbpStaticAssets** on [GitHub](https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.AspNetCore/Microsoft/AspNetCore/Builder/AbpApplicationBuilderExtensions.cs#L129-L198).

## Conclusion

Optimizing static asset delivery is essential for building fast, efficient web applications. **MapStaticAssets** simplifies and automates the optimization of static files by providing build-time compression, caching headers, and content-based ETags. This ensures that your app's static assets are always delivered in the most efficient way, whether users are on fast broadband or slower mobile connections. By using **MapStaticAssets**, you can deliver a faster, more reliable experience for your users with minimal effort.

## References

* [Static files in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-9.0)
* [What's new in ASP.NET Core 9.0](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-9.0?view=aspnetcore-8.0#optimize-static-web-asset-delivery)
