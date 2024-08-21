# Service Proxies

Calling a REST endpoint from Angular applications is common. We usually create **services** matching server-side controllers and **interfaces** matching [DTOs](../../architecture/domain-driven-design/data-transfer-objects.md) to interact with the server. This often results in manually transforming C# code into TypeScript equivalents and that is unfortunate, if not intolerable.

To avoid manual effort, we might use a tool like [NSWAG](https://github.com/RicoSuter/NSwag) that generates service proxies. However, NSWAG has some disadvantages:

- It generates **a single .ts file** which gets **too large** as your application grows. Also, this single file does not fit the **[modular](../../architecture/modularity/basics.md) approach** of ABP.
- To be honest, the generated code is a bit **ugly**. We would like to produce code that looks as if someone wrote it.
- Since swagger.json **does not reflect the exact method signature** of backend services, NSWAG cannot reflect them on the client-side as well.

ABP introduces an endpoint that exposes server-side method contracts. When the `generate-proxy` command is run, ABP CLI makes an HTTP request to this endpoint and generates better-aligned client proxies in TypeScript. It organizes folders according to namespaces, adds barrel exports, and reflects method signatures in Angular services.

> Before you start, please make sure you start the backend application with `dotnet run`. There is a [known limitation about Visual Studio](#known-limitations), so please do not run the project using its built-in web server.

Run the following command in the **root folder** of the angular application:

```bash
abp generate-proxy -t ng
```

### Note 
- If you're utilizing NX, be aware that the Angular schematics-based ABP package may not work as expected. Instead, there's a specialized package for NX-based repositories named @abp/nx.generators. We recommend using this generator for your package. For detailed instructions and more information, refer to this section.
- The command without any parameters creates proxies for your own application's services only and places them in your default Angular application. There are several parameters you may use to modify this behavior. See the [Details](#abp-nx-proxy-generator).

The generated files will be placed in a folder called `proxy` at the root of the target project.

![generated-files-via-generate-proxy](./images/generated-files-via-generate-proxy.png)

Each folder will have models, enums, and services defined at related namespace accompanied by a barrel export, i.e. an `index.ts` file for easier imports.

> The command can find application/library roots by reading the `angular.json` file. Make sure you have either defined your target project as the `defaultProject` or pass the `--target` parameter to the command. This also means that you may have a monorepo workspace.

### Angular Project Configuration

> If you've created your project with version 3.1 or later, you can skip this part since it will be already installed in your solution.

For a solution that was created before v3.1, follow the steps below to configure your Angular application:

1. Add `@abp/ng.schematics` package to the `devDependencies` of the Angular project. Run the following command in the root folder of the angular application:

```bash
npm install @abp/ng.schematics -D
```

2. Add `rootNamespace` property to the `/src/environments/environment.ts` in your application project as shown below. `MyCompanyName.MyProjectName` should be replaced by the root namespace of your .NET project.

```js
export const environment: Config.Environment = {
  // other environment variables...
  apis: {
    default: {
      rootNamespace: "MyCompanyName.MyProjectName",
      // other environment variables...
    },
  },
};
```

3. [OPTIONAL] Add the following paths to `tsconfig.base.json` in order to have a shortcut for importing proxies:

```json
{
  // other TS configuration...
  "compilerOptions": {
    // other TS configuration...
    "paths": {
      "@proxy": ["src/app/proxy/index.ts"],
      "@proxy/*": ["src/app/proxy/*"]
    }
  }
}
```

> The destination the `proxy` folder is created and the paths above may change based on your project structure.

## Parameters of generate-proxy

- **module or -m:** The backend module name. The default is `app`. The object key of the modules defined in response of `api/abp/api-definition`. For example, if you want to generate-proxy of PermissionManagement, you should pass `permissionManagement` as a value.
- **apiName or -a:** The Backend api name, also known as remoteServiceName. It is defined in the selected module (in response of `api/abp/api-definition`). The property(key) name is `remoteServiceName`. For example for the PermissionManagement, you should pass `AbpPermissionManagement`
- **source:** Source of the Angular project for the API definition URL & root namespace resolution. 
- **target:** Target for the Angular project to place the generated code. For example, if it's `permission-management`, it'll look like this (npm/ng-packs/packages/*permission-management*).
- **entryPoint:** To create the generated proxy folder in the target. The directory is `permission-management/proxy/src/lib/proxy` and the `permission-management` is the value of target. If you want to create a folder for the generated proxy, there are two options, you should either set the value `proxy` as the entryPoint or go to project.json and change the `sourceRoot` from `packages/permission-management/src` to `packages/permission-management/proxy/src`. No need to change the sourceRoot of project with the property. if you keep it empty, the proxy will be generated into the folder defined in the sourceRoot property.
- **serviceType:** The service type of the generated proxy. The options are `application`, `integration` and `all`. The default value is `application`. A developer can mark a service "integration service". If you want to skip proxy generation for the service, then this is the correct setting. More info about [Integration Services](../../api-development/integration-services.md) 


### Services

The `generate-proxy` command generates one service per back-end controller and a method (property with a function value actually) for each action in the controller. These methods call backend APIs via [RestService](./http-requests#restservice).

A variable named `apiName` (available as of v2.4) is defined in each service. `apiName` matches the module's `RemoteServiceName`. This variable passes to the `RestService` as a parameter at each request. If there is no microservice API defined in the environment, `RestService` uses the default. See [getting a specific API endpoint from application config](./http-requests#how-to-get-a-specific-api-endpoint-from-application-config)

The `providedIn` property of the services is defined as `'root'`. Therefore there is no need to provide them in a module. You can use them directly by injecting them into the constructor as shown below:

```js
import { BookService } from '@proxy/books';

@Component(/* component metadata here */)
export class BookComponent implements OnInit {
  constructor(private service: BookService) {}

  ngOnInit() {
    this.service.get().subscribe(
      // do something with the response
    );
  }
}
```

The Angular compiler removes the services that have not been injected anywhere from the final output. See the [tree-shakable providers documentation](https://angular.io/guide/dependency-injection-providers#tree-shakable-providers).

### Models

The `generate-proxy` command generates interfaces matching DTOs in the back-end. There are also a few [core DTOs](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/dtos.ts) in the `@abp/ng.core` package. In combination, these models can be used to reflect the APIs.

```js
import { PagedResultDto } from "@abp/ng.core";
import { BookDto } from "@proxy/books";

@Component(/* component metadata here */)
export class BookComponent implements OnInit {
  data: PagedResultDto<BookDto> = {
    items: [],
    totalCount: 0,
  };
}
```

### Enums

Enums have always been difficult to populate in the frontend. The `generate-proxy` command generates enums in a separate file and exports a ready-to-use "options constant" from the same file. So you can import them as follows:

```js
import { bookGenreOptions } from "@proxy/books";

@Component(/* component metadata here */)
export class BookComponent implements OnInit {
  genres = bookGenreOptions;
}
```

...and consume the options in the template as follows:

```html
<!-- simplified for sake of clarity -->
<select formControlName="genre">
  <option [ngValue]="null">Select a genre</option>
  <option *ngFor="let genre of genres" [ngValue]="genre.value">
    {%{{{ genre.key }}}%}
  </option>
</select>
```

> Please [see this article](https://github.com/abpframework/abp/blob/dev/docs/en/Blog-Posts/2020-09-07%20Angular-Service-Proxies/POST.md) to learn more about service proxies.


### ABP NX Proxy Generator
 For projects that utilize NX, the @abp/nx.generators package offers seamless integration. Essentially, this package serves as a wrapper specifically tailored for NX-based repositories
 **Installation**
To incorporate this package into your project, run the following command:
```bash
yarn add @abp/nx.generators
```
### Usage
To use the generator, execute the following command:

```bash
yarn nx generate @abp/nx.generators:generate-proxy
// or
yarn nx g @abp/nx.generators:generate-proxy
```

**Note:** The parameters you'd use with this generator are consistent with the standard ABP proxy generator.


### Known Limitations

When you run a project on Visual Studio using IIS Express as the web server, there will be no remote access to your endpoints. This is the default behavior of IIS Express since it explicitly protects you from the security risks of running over the network. However, that will cause the proxy generator to fail because it needs a response from the `/api/abp/api-definition` endpoint. You may serve your endpoints via Kestrel to avoid this. Running `dotnet run` in your command line (at your project folder) will do that for you.

## See Also

* [Video tutorial](https://abp.io/video-courses/essentials/generating-client-proxies)