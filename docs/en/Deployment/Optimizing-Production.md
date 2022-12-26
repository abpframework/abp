# Optimizing Your Application for Production Environments

ABP Framework and the startup solution templates are configured well to get the maximum performance on production environments. However, there are still some points you need to pay attention to in order to optimize your system in production. In this document, we will mention some of these topics.

## Caching Static Contents

The following items are contents that can be cached in the client side (typically in the Browser) or in a CDN server:

* **Static images** can always be cached. Here, you should be careful that if you change an image, use a different file name, or use a versioning query-string parameter, so the browser (or CDN) understands it's been changed.
* **CSS and JavaScript files**. ABP's [bundling & minification](../UI/AspNetCore/Bundling-Minification.md) system always uses a query-string versioning parameter and a hash value in the files names of the CSS & JavaScript files for the [MVC (Razor Pages)](../UI/AspNetCore/Overall.md) UI. So, you can safely cache these files in the client side or in a CDN server.
* **Application bundle files** of an [Angular UI](../UI/Angular/Quick-Start.md) application.
* **[Application Localization Endpoint](../API/Application-Localization.md)** can be cached per culture (it already has a `cultureName` query string parameter) if you don't use dynamic localization on the server-side. ABP Commercial's [Language Management](https://commercial.abp.io/modules/Volo.LanguageManagement) module provides dynamic localization. If you're using it, you can't cache that endpoint forever. However, you can still cache it for a while. Applying dynamic localization text changes to the application can delay for a few minutes, even for a few hours in a real life scenario.

There may be more ways based on your solution structure and deployment environment, but these are the essential points you should consider to client-side cache in a production environment.

## Bundling & Minification for MVC (Razor Pages) UI

ABP's [bundling & minification](../UI/AspNetCore/Bundling-Minification.md) system automatically bundles, minifies and versions your CSS and JavaScript files in production environment. Normally, you don't need to do anything, if you haven't disabled it yourself in your application code. It is important to follow the [bundling & minification](../UI/AspNetCore/Bundling-Minification.md) document and truly use the system to get the maximum optimization.

## Background Jobs

ABP's [Background Jobs](../Background-Jobs.md) system provides an abstraction with a basic implementation to enqueue jobs and execute them in a background thread. ABP's Default Background Job Manager may not be enough if you are adding too many jobs to the queue and want them to be executed in parallel by multiple servers with a high performance. If you need these, you should consider to configure a dedicated background job software, like [Hangfire](https://www.hangfire.io/). ABP has a pre-built [Hangfire integration](../Background-Jobs-Hangfire.md), so you can switch to Hangfire without changing your application code.
