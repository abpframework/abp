# Announcing Server-Side Rendering (SSR) Support for ABP Framework Angular Applications

We are pleased to announce that **Server-Side Rendering (SSR)** has become available for ABP Framework Angular applications! This highly requested feature brings major gains in performance, SEO, and user experience to your Angular applications based on ABP Framework.

## What is Server-Side Rendering (SSR)?

Server-Side Rendering refers to an approach which renders your Angular application on the server as opposed to the browser. The server creates the complete HTML for a page and sends it to the client, which can then show the page to the user. This poses many advantages over traditional client-side rendering.

## Why SSR Matters for ABP Angular Applications

### Improved Performance
- **Quicker visualization of the first contentful paint (FCP)**: Because prerendered HTML is sent over from the server, users will see content quicker.
- **Better perceived performance**: Even on slower devices, the page will be displaying something sooner.
- **Less JavaScript parsing time**: For example, the initial page load will not require parsing and executing a large bundle of JavaScript.

### Enhanced SEO
- **Improved indexing by search engines**: Search engine bots are able to crawl and index your content quicker.
- **Improved rankings in search**: The quicker the content loads and the easier it is to access, the better your SEO score.
- **Preview when sharing on social channels**: Rich previews with the appropriate meta tags are generated when sharing links on social platforms.

### Better User Experience
- **Support for low bandwidth**: Users with slower Internet connections will have a better experience
- **Progressive enhancement**: Users can start accessing the content before JavaScript has loaded
- **Better accessibility**: Screen readers and other assistive technologies can access the content immediately

## Getting Started with SSR

### Adding SSR to an Existing Project

You can easily add SSR support to your existing ABP Angular application using the Angular CLI with ABP schematics:

> Adds SSR configuration to your project
```bash
ng generate @abp/ng.schematics:ssr-add
```
> Short form
```bash
ng g @abp/ng.schematics:ssr-add
```
If you have multiple projects in your workspace, you can specify which project to add SSR to:

```bash
ng g @abp/ng.schematics:ssr-add --project=my-project
```

If you want to skip the automatic installation of dependencies:

```bash
ng g @abp/ng.schematics:ssr-add --skip-install
```

## What Gets Configured

When you add SSR to your ABP Angular project, the schematic automatically:

1. **Installs necessary dependencies**: Adds `@angular/ssr` and related packages
2. **Creates Server Configuration**: Creates `server.ts` and related files
3. **Updates Project Structure**:
    - Creates `main.server.ts` to bootstrap the server
    - Adds `app.config.server.ts` for standalone apps (or `app.module.server.ts` for NgModule apps)
    - Configures server routes in `app.routes.server.ts`
4. **Updates Build Configuration**: updates `angular.json` to include:
    - a `serve-ssr` target for local SSR development
    - a `prerender` target for static site generation
    - Proper output paths for browser and server bundles

## Supported Configurations

The ABP SSR schematic supports both modern and legacy Angular build configurations:

### Application Builder (Suggested)
- The new `@angular-devkit/build-angular:application` builder
- Optimized for Angular 17+ apps
- Enhanced performance and smaller bundle sizes

### Server Builder (Legacy)
- The original `@angular-devkit/build-angular:server` builder
- Designed for legacy Angular applications
- Compatible with legacy applications

## Running Your SSR Application

After adding SSR to your project, you can run your application in SSR mode:

```bash
# Development mode with SSR
ng serve

# Or specifically target SSR development server
npm run serve:ssr

# Build for production
npm run build:ssr

# Preview production build
npm run serve:ssr:production
```

## Important Considerations

### Browser-Only APIs
Some browser APIs are not available on the server. Use platform checks to conditionally execute code:

```typescript
import { isPlatformBrowser } from '@angular/common';
import { PLATFORM_ID, inject } from '@angular/core';

export class MyComponent {
  private platformId = inject(PLATFORM_ID);
  
  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      // Code that uses browser-only APIs
      console.log('Running in browser');
      localStorage.setItem('key', 'value');
    }
  }
}
```

### Storage APIs
`localStorage` and `sessionStorage` are not accessible on the server. Consider using:
- Cookies for server-accessible data.
- The state transfer API for hydration.
- ABP's built-in storage abstractions.

### Third-Party Libraries
Please ensure that any third-party libraries you use are compatible with SSR. These libraries can require:
- Dynamic imports for browser-only code.
- Platform-specific service providers.
- Custom Angular Universal integration.

## ABP Framework Integration

The SSR implementation is natively integrated with all of the ABP Framework features:

- **Authentication & Authorization**: The OAuth/OpenID Connect flow functions seamlessly with ABP
- **Multi-tenancy**: Fully supports tenant resolution and switching
- **Localization**: Server-side rendering respects the locale
- **Permission Management**: Permission checks work on both server and client
- **Configuration**: The ABP configuration system is SSR-ready
## Performance Tips

1. **Utilize State Transfer**: Send data from server to client to eliminate redundant HTTP requests
2. **Optimize Images**: Proper image loading strategies, such as lazy loading and responsive images.
3. **Cache API Responses**: At the server, implement proper caching strategies.
4. **Monitor Bundle Size**: Keep your server bundle optimized
5. **Use Prerendering**: The prerender target should be used for static content.

## Conclusion

Server-side rendering can be a very effective feature in improving your ABP Angular application's performance, SEO, and user experience. Our new SSR schematic will make it easier than ever to add SSR to your project.

Try it today and let us know what you think!

---
