# PWA Configuration

[PWAs (Progressive Web Apps)](https://web.dev/progressive-web-apps/) are developed using specific technologies to allow applications to take advantage of both web and native app features. 

Here is a list of some features that PWA provides:

- **Installable**: A web application can be installed and used like a native/desktop application.
- **Network Independent**: PWAs support offline scenarios. It can work offline or with a poor network connection.
- **Responsive**: It's usable on any devices such as mobile phones, tablets, laptops, etc.

## Creating a Project with PWA Support

You can create a new web application with PWA support for **Blazor WebAssembly** by using the `--pwa` option as below:

```bash
abp new Acme.BookStore -t blazor --pwa
```

After this command, your application will be created and some additional PWA related files (such as **manifest**, **icons**, **service workers**, etc.) will be added. Then, you can get the full advantages of web and native app features.

## Adding PWA Support to an Existing Project

If you started your application without PWA support, it's possible to change your mind and get the benefit of PWA later. You only need to make some configurations as listed below:

### 1-) Add the `manifest.json` File 

> Web Application Manifest provides information about a web application in a JSON text file and it's required for the web application to be downloaded and be presented to the user similarly to a native application.

First, you need to create a JSON file named **manifest.json** under the **wwwroot** folder and define some pieces of information about your application. You can see an example **manifest.json** file content below:

```json
{
  "name": "MyProjectName",
  "short_name": "MyCompanyName.MyProjectName",
  "start_url": "./",
  "display": "standalone",
  "background_color": "#ffffff",
  "theme_color": "#03173d",
  "prefer_related_applications": false,
  "icons": [
    {
      "src": "icon-512.png",
      "type": "image/png",
      "sizes": "512x512"
    },
    {
      "src": "icon-192.png",
      "type": "image/png",
      "sizes": "192x192"
    }
  ]
}
```

- Some application specific information should be defined in this file.
- For example, you can configure which icon needs to be seen in which screen size, background color, description, etc.

### 2-) Add Icons for Specific Screen Sizes (Optional)

You can add some icons for your application to be seen in specific screen sizes and define in which screen sizes icons should be displayed in the **manifest.json** file. You can see the **icons** section in the **manifest.json** file as an example above.

> You can use, default icons from our [template](https://github.com/abpframework/abp/tree/dev/templates/app/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Client/wwwroot).

### 3-) Configure Service Workers

> Service workers are one of the fundamental parts of PWAs. They enable fast loading (regardless of the network), offline access, push notifications, and other web/native app capabilities. They run in the background and don't block the main thread so they don't slow your application.

You need to create `service-worker.js` and `service-worker.published.js` files under the **wwwroot** folder of your project. These files will be used by your project to determine which PWA features you want to use.

You can get the simple configurations for the [service-worker.js](https://github.com/abpframework/abp/blob/dev/templates/app/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Client/wwwroot/service-worker.js) and [service-worker.published.js](https://github.com/abpframework/abp/blob/dev/templates/app/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Client/wwwroot/service-worker.published.js) files from our [template](https://github.com/abpframework/abp/tree/dev/templates/app/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Client/wwwroot).

After the related service worker files are added, then we need to define them in our `.csproj` file to notify our application. So open your `*.csproj` file and add the following content:

```xml
<PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <BlazorWebAssemblyLoadAllGlobalizationData>true</BlazorWebAssemblyLoadAllGlobalizationData>

    <!-- Add the following line -->
    <ServiceWorkerAssetsManifest>service-worker-assets.js</ServiceWorkerAssetsManifest>
</PropertyGroup>

<!-- Add the following item group -->
<ItemGroup>
    <ServiceWorker Include="wwwroot\service-worker.js" PublishedContent="wwwroot\service-worker.published.js" />
</ItemGroup>
```

* With the `ServiceWorkerAssetsManifest` MSBuild property, your Blazor application generates a service worker assets manifest with the specified name. This file will be generated in the path of `/bin/Debug/{TARGET FRAMEWORK}/wwwroot/service-worker-assets.js` on runtime. This manifest can list all resources such as images, stylesheets, JS files etc. by examining the `service-worker.published.js` file (regarding to your configurations in this file).
* The `ServiceWorker` property is used to define which files need to be accounted as **Service Worker** files and service workers are used to determine which PWA features should be used.


### 4-) Define Web Application Manifest and Register Service Workers

Finally, now you can define the `manifest.json` file and **icons** in the **index.html** file and register the **service workers** for your application.

Let's start with adding `<link>` elements (between `<head>` tags) for the manifest and app icon in the **index.html** file (under the **wwwroot** folder):

```html
<head>
  <!-- ... -->  
  <link href="manifest.json" rel="manifest" />
  <link rel="apple-touch-icon" sizes="512x512" href="icon-512.png" />
  <link rel="apple-touch-icon" sizes="192x192" href="icon-192.png" />
</head>
```

Then, add the following `<script>` tag inside the closing `</body>` tag in the same file:

```html
<body>
  <!-- ... --> 
  <script>
    if ('serviceWorker' in navigator) {
      navigator.serviceWorker.register("service-worker.js");
    }
  </script>
</body>
```

You've added the related files and made the related configurations with this final touch to add PWA support to your existing application. Now, you can take full advantage of PWAs.

## Differences Between the Debug and Published Service Workers

Application Template produces two service worker files, if you create your application with PWA support:

* The `service-worker.js` file is used during development and does nothing by default. 
* The `service-worker.published.js` file, which is used after the app is published. Caches certain file extensions and supports offline scenarios by default (uses a *cache-first* strategy). A user must first visit the app while they're online. The browser automatically downloads and caches all of the resources required to operate offline and then when the network connection is disconnected, it can be used like before.

You can configure those files as mentioned in the *Customize Service Workers* section down below.

> If you want to share logic between those two service worker files, you can consider creating a third JS file and hold the common logic in this file or use the [self.importScripts](https://developer.mozilla.org/docs/Web/API/WorkerGlobalScope/importScripts)s to load the common logic into both service worker files.

## Customization

You can customize the `manifest.json`, `service-worker.js` and `service-worker.published.js` files generated by the ABP if you created an application with PWA support.

### Customize Web Application Manifest (`manifest.json`)

> The web app manifest is a JSON file that tells the browser about your Progressive Web App and how it should behave when installed on the user's desktop or mobile device. A typical manifest file includes the app name, the icons the app should use, and the URL that should be opened when the app is launched. - From [web.dev](https://web.dev/add-manifest)

You can customize the `manifest.json` file (under the **wwwroot** folder) to your needs. You can set the **name**, **short_name**, **icons**, **description**, **start_url**, etc. You can see an example `manifest.json` file content below:

```json
{
  "name": "Acme.BookStore",
  "short_name": "BookStore",
  "description": "My application description",
  "theme_color": "#000000",
  "background_color": "#ffffff",
  "icons": [
    {
      "src": "../icon-192.png",
      "sizes": "192x192",
      "type": "image/png"
    },
    {
      "src": "../icon-512.png",
      "sizes": "512x512",
      "type": "image/png"
    }
  ]
  // other properties...
}
```

* You must provide at least the `short_name` or `name` property. If both of these properties are provided, the `short_name` property is used almost anywhere like the **launcher** and the **home** screen.
* For Chromium based browsers, you must provide at least a *192x192* px icon and a *512x512* px icon. If only those two icon sizes are provided, the browsers will automatically scale the icons to fit the device. If you don't want to let the browser auto-scale icons, you need to add icons for other sizes too.

> You can see the other properties from [here](https://web.dev/add-manifest/#manifest-properties).

### Customize Service Workers

If you create your application with PWA support, two service worker files will be generated: `service-worker.js` and `service-worker.published.js`.

ABP's service-worker files are the same as the .NET Core's and it's valid for most of the time and you'll probably not need to configure it manually. However, if you want to configure the service workers you can do it easily.

You can configure the `service-worker.js` file for debug mode and the `service-worker.published.js` file for release mode according to your own needs. 

#### `service-worker.js`

```js
// Caution: In development, always fetch from the network and do not enable offline support.
self.addEventListener('fetch', () => { });
```

* Configuring this file, you can use additional PWA features in debug mode.
* By default, it does nothing in debug mode, it fetches from the network (recommended) and does not support offline scenarios. You can change this behavior by configuring this file and also benefit from additional features of PWAs.

#### `service-worker.published.js`

```js
// Caution: Be sure you understand the caveats before publishing an application with
// offline support. See https://aka.ms/blazor-offline-considerations

self.importScripts('./service-worker-assets.js');
self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));

const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
const offlineAssetsInclude = [ /\.dll$/, /\.pdb$/, /\.wasm/, /\.html/, /\.js$/, /\.json$/, /\.css$/, /\.woff$/, /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/, /\.blat$/, /\.dat$/ ];
const offlineAssetsExclude = [ /^service-worker\.js$/ ];

async function onInstall(event) {
    console.info('Service worker: Install');

    // Fetch and cache all matching items from the assets manifest
    const assetsRequests = self.assetsManifest.assets
        .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
        .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)))
        .map(asset => new Request(asset.url, { integrity: asset.hash, cache: 'no-cache' }));
    await caches.open(cacheName).then(cache => cache.addAll(assetsRequests));
}

async function onActivate(event) {
    console.info('Service worker: Activate');

    // Delete unused caches
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
}

async function onFetch(event) {
    let cachedResponse = null;
    if (event.request.method === 'GET') {
        // For all navigation requests, try to serve index.html from cache
        // If you need some URLs to be server-rendered, edit the following check to exclude those URLs
        const shouldServeIndexHtml = event.request.mode === 'navigate';

        const request = shouldServeIndexHtml ? 'index.html' : event.request;
        const cache = await caches.open(cacheName);
        cachedResponse = await cache.match(request);
    }

    return cachedResponse || fetch(event.request);
}
```

* You can configure this file if you want to cache additional file extensions such as `.webp` or etc. You can also use some additional features of PWA by configuring this file.
* By default, dll files (`*.dll`) and some static assets (`*.js`, `*.css`, etc.) are cached.
* Cached files will be stored in the `service-worker-assets.js` (**/bin/Debug/{TARGET FRAMEWORK}/wwwroot/service-worker-assets.js**). You can change this file name by renaming it in between the `ServiceWorkerAssetsManifest` tags on your `*.csproj` file.

## See Also

* [ASP.NET Core Blazor Progressive Web Application (PWA)](https://docs.microsoft.com/en-us/aspnet/core/blazor/progressive-web-app).
