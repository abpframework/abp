# ASP.NET Core Blazor 9.0 New Features Summary 🆕

In this article, I'll highlight .NET 9's Blazor updates and important features for ASP.NET Core 9.0. These features are based on the latest .NET 9 Preview 7. 

![Cover](cover.png)

## .NET MAUI Blazor Hybrid App and Web App solution template

There's a new solution template to create .**NET MAUI native** and **Blazor web client** apps. This new template allows to choose a Blazor interactive render mode, it uses a shared Razor class library to maintain the UI's Razor components.

For more info:

* [learn.microsoft.com > maui blazor web app tutorial](https://learn.microsoft.com/en-us/aspnet/core/blazor/hybrid/tutorials/maui-blazor-web-app?view=aspnetcore-9.0)
* [reddit.com/r/Blazor/comments/1dabyzk/net_8_blazor_hybrid_maui_app_web_hosting/](https://www.reddit.com/r/Blazor/comments/1dabyzk/net_8_blazor_hybrid_maui_app_web_hosting/)



## A new middleware: `MapStaticAssets` 

This new middleware optimizes the delivery of static assets in any ASP.NET Core app, also for Blazor. Basically it compresses assets via [Gzip](https://datatracker.ietf.org/doc/html/rfc1952), [fingerprints](https://developer.mozilla.org/docs/Glossary/Fingerprinting) for all assets at build time with a Base64 and removes caches when Visual Studio Hot Reload (development time) is in action.

For more info:

* [learn.microsoft.com > optimizing static web assets](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-9.0?view=aspnetcore-8.0#optimizing-static-web-asset-delivery)
* [learn.microsoft.com > fundamentals of static files](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/static-files?view=aspnetcore-9.0#static-asset-middleware)



##  Simplifying the process of querying component states at runtime

1. Finding the component's current execution location: This can be especially helpful for component performance optimization and debugging. 
2. Verifying whether the component is operating in a dynamic environment by checking: This can be useful for parts whose actions vary according to how their surroundings interact.
3. Obtaining the render mode allocated to the component: Comprehending the render mode can aid in enhancing the rendering procedure and augmenting the component's general efficiency.

For more info:

* [learn.microsoft.com > detect rendering location interactivity & render mode runtime](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes?view=aspnetcore-9.0#detect-rendering-location-interactivity-and-assigned-render-mode-at-runtime)



## Detecting component's location, interactivity support and render mode

The `ComponentBase.RendererInfo` and `ComponentBase.AssignedRenderMode`  now allows to detect the following actions:

* `RendererInfo.Name` returns the location where the component is executing
* `RendererInfo.IsInteractive` indicates if the component supports interactivity at the time of rendering. 
* `ComponentBase.AssignedRenderMode` exposes the component's assigned render mode



## Better server-side reconnection 

* When the previous app is disconnected and the user navigates to this app, or browser put this app in sleep mode, Blazor runs reconnection mechanism.

* When reconnection is not successful because your server killed connection, it automatically makes a full page refresh.

* With the new below config, you can adjust your reconnection retry time:

  * ```csharp
    Blazor.start({
      circuit: {
        reconnectionOptions: {
          retryIntervalMilliseconds: (previousAttempts, maxRetries) => 
            previousAttempts >= maxRetries ? null : previousAttempts * 1000
        },
      },
    });
    ```



## Simple serialization for authentication

The new APIs in ASP.NET make it easier to add authentication to existing Blazor Web Apps. These APIs, now part of the Blazor Web App project template, allow authentication state to be serialized on the server and deserialized in the browser, simplifying the process of integrating authentication. This removes the need for developers to manually implement or copy complex code, especially when using WebAssembly-based interactivity.

For more info:

- [learn.microsoft.com > blazor Identity UI individual accounts](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/server/?view=aspnetcore-9.0#blazor-identity-ui-individual-accounts)
- [learn.microsoft.com > manage authentication state](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/server/?view=aspnetcore-9.0#manage-authentication-state-in-blazor-web-apps)



## Easily add static server-side rendering pages

With .NET 9, adding static server-side rendering (SSR) pages to globally interactive Blazor Web Apps has become simpler. The new `[ExcludeFromInteractiveRouting]` attribute allows developers to mark specific Razor component pages that require static SSR, such as those relying on HTTP cookies and the request/response cycle. Pages annotated with this attribute exit interactive routing and trigger a full-page reload, while non-annotated pages default to interactive rendering modes like `InteractiveServer`. This approach enables flexibility between static and interactive rendering depending on the page's requirements.

For more info:

* [learn.microsoft.com > render-modes](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes?view=aspnetcore-9.0#static-ssr-pages-in-a-globally-interactive-app)



## Constructor Injection in Razor Components

Razor components support constructor injection, allowing services like `NavigationManager` to be injected directly into a component's constructor. This can be used to manage navigation actions, such as redirecting the user upon an event like a button click.

For more info:

* [learn.microsoft.com> dependency-injection](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/dependency-injection?view=aspnetcore-9.0#request-a-service-in-a-component)



## Configuring WebSocket Compression and Frame-Ancestors CSP in Interactive Server Components

By default, Interactive Server components enable WebSocket compression and set a `frame-ancestors` Content Security Policy (CSP) to `self`, restricting embedding the app in `<iframe>`. Besides, compression can be disabled to improve security by setting `ConfigureWebSocketOptions` to null, though this may reduce performance. To prevent embedding the app in any `iframe` while maintaining WebSocket compression, set the `ContentSecurityFrameAncestorsPolicy` to 'none'.

For more info:

- [learn.microsoft.com > websocket compression](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/signalr?view=aspnetcore-9.0#websocket-compression-for-interactive-server-components)
- [learn.microsoft.com > interactive server-side rendering when compression enabled](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/server/interactive-server-side-rendering?view=aspnetcore-9.0#interactive-server-components-with-websocket-compression-enabled)



## Tracking Composition State with `KeyboardEventArgs.IsComposing`

The new `KeyboardEventArgs.IsComposing` property indicates whether a keyboard event is part of a composition session, which is essential for properly handling international character input methods.



## Configuring Row Overscan in `QuickGrid` with new `OverscanCount` parameter

The `QuickGrid` component now includes an `OverscanCount` property, which controls how many extra rows are rendered before and after the visible area when virtualization is enabled. By default, `OverscanCount` is set to **3**, but it can be adjusted as below to **5**.

```html
<QuickGrid ItemsProvider="itemsProvider" Virtualize="true" OverscanCount="5">...</QuickGrid>
```



## Range Input Support in `InputNumber<TValue>` Component

The `InputNumber<TValue>` component now supports the `type="range"` [attribute](https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/range), allowing for range inputs like sliders or dials. This feature supports model binding and form validation, offering a more interactive way to input numerical data compared to the traditional text box.

```html
<EditForm>
     <InputNumber @bind-Value="Model.ProductCount" max="999" min="1" step="1" type="range" />
</EditForm>

@code {
    public class MyModel
    {
        [Required, Range(minimum: 1, maximum: 999)]
        public int ProductCount { get; set; }
    }
}
```



