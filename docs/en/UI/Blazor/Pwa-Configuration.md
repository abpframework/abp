# PWA Configuration

[PWAs (Progressive Web Apps)](https://web.dev/progressive-web-apps/) are developed using specific technologies to allow applications to take advantage of both web and native app features. 

Here the list of some features that PWA provides:

- **Installable**: Web application can be installed and used like a native/desktop application.
- **Network Independent**: PWAs support offline scenerios. It can work offline or with a poor network connection.
- **Responsive**: It's usable on any devices such as mobile phones, tables, laptops, etc.
- and more...

## Creating a Project with PWA Support

You can create a new web application with PWA support for **Blazor WebAssembly** by using the `--pwa` option like below:

```bash
abp new Acme.BookStore -t blazor --pwa
```

After this command, your application will be created and some additional PWA related files (such as **manifest**, **icons**, **service workers**, etc.) will be added. Then, you can get full advantages of web and native app features.

## Adding PWA Support to an Existing Project

If you started your application without PWA support, it's possible to change your mind and get benefit of PWA later. You only need to make some configurations as listed below:

### 1-) Add `manifest.json` File 

> Web Application Manifest provides information about a web application in a JSON text file and it's required for the web application to be downloaded and be presented to the user similarly to a native application.

First, you need to create a JSON file named **manifest.json** under the **wwwroot** folder and define some informations about your application. You can see an example **manifest.json** file content as below:

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

- Some application specific informations should be defined in this file.
- For example, you can configure which icon need to be seen in which screen size, background color, etc.

### 2-) Icons for Specific Screen Sizes (Optional)

You can add some icons for your application to be seen in specific screen sizes and define in which screen sizes icons should be displayed in the **manifest.json** file. You can see the **icons** section in the **manifest.json** file to an example.

> You can use, our default icons from [here](https://github.com/abpframework/abp/tree/dev/templates/app/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/wwwroot).

### 3-) Configure Service Workers

> Service workers are one of the fundamental part of PWAs. They enable fast loading (regardless of the network), offline access, push notifications, and other web/native app capabilities. They run in the background and doesn't block the main thread so they don't slow your application.

You need to create `service-worker.js` and `service-worker.published.js` files under the **wwwroot** folder of your project. These files will be used by your project to determine which PWA features that you want to use.

You can copy-paste the simple configurations for [service-worker.js](https://github.com/abpframework/abp/blob/dev/templates/app/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/wwwroot/service-worker.js) and [service-worker.published.js](https://github.com/abpframework/abp/blob/dev/templates/app/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/wwwroot/service-worker.published.js) files from our [template](https://github.com/abpframework/abp/tree/dev/templates/app/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/wwwroot).

After the related service worker files added, then we need to define them in our `.csproj` file to notify our application. So open your `*.csproj` file and add the following content:

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

* With the `ServiceWorkerAssetsManifest` MSBuild property, your Blazor application generates a service worker assets manifest with the specified name. This file will be generated in the path of `/bin/Debug/{TARGET FRAMEWORK}/wwwroot/service-worker-assets.js` on runtime. This manifest can list, all resources such as images, stylesheets, JS files etc. by examining the `service-worker.published.js` file (regarding to your configurations in this file).
* `ServiceWorker` property is used to define which files need to be accounted as **Service Worker** files. These files are used to determine which PWA features should be used.


### 4-) Define Web Application Manifest and Register Service Workers

Finally, now you can define `manifest.json` file and **icons** in the **index.html** file and register the **service workers** for your application.

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

You've added the related files and making the related configurations with this final touch to add PWA support to your existing application. Now, you can take advantage of PWA.

## Differences Between the Debug and Release Service Workers

## Customizations

### Customize Web Application Manifest

### Customize Service Workers