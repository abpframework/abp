# Introducing the Angular Service Proxy Generation

ABP Angular Service Proxy System **generates TypeScript services and models** to consume your backend HTTP APIs developed using the ABP Framework. So, you **don't manually create** models for your server side DTOs and perform raw HTTP calls to the server.

ABP Framework has introduced the **new** Angular Service Proxy Generation system with the **version 3.1**. While this feature was available since the [v2.3](https://blog.abp.io/abp/ABP-Framework-v2_3_0-Has-Been-Released), it was not well covering some scenarios, like inheritance and generic types and had some known problems. **With the v3.1, we've re-written** it using the [Angular Schematics](https://angular.io/guide/schematics) system. Now, it is much more stable and feature rich.

This post introduces the service proxy generation system and highlights some important features.

## Installation

### ABP CLI

You need to have the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) to use the system. So, install it if you haven't installed before:

````bash
dotnet tool install -g Volo.Abp.Cli
````

If you already have installed it before, you can update to the latest version:

````shell
dotnet tool update -g Volo.Abp.Cli
````

### Project Configuration

> If you've created your project with version 3.1 or later, you can skip this part since it will be already installed in your solution.

For a solution that was created before v3.1, follow the steps below to configure the angular application:

* Add `@abp/ng.schematics` package to the `devDependencies` of the Angular project. Run the following command in the root folder of the angular application:

````bash
npm install @abp/ng.schematics --save-dev
````

- Add `rootNamespace` entry into the `apis/default` section in the `/src/environments/environment.ts`, as shown below:

```json
apis: {
  default: {
    ...
    rootNamespace: 'Acme.BookStore'
  },    
}
```

`Acme.BookStore` should be replaced by the root namespace of your .NET project. This ensures to not create unnecessary nested folders while creating the service proxy code. This value is `AngularProxyDemo` for the example solution explained below.

* Finally, add the following paths to the `tsconfig.base.json` to have a shortcut while importing proxies:

```json
"paths": {
    "@proxy": ["src/app/proxy/index.ts"],
    "@proxy/*": ["src/app/proxy/*"]
}
```

## Basic Usage

### Project Creation

> If you already have a solution, you can skip this section.

You need to [create](https://abp.io/get-started) your solution with the Angular UI. You can use the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) to create a new solution:

````bash
abp new AngularProxyDemo -u angular
````

#### Run the Application

The backend application must be up and running to be able to use the service proxy code generation system.

> See the [getting started](https://docs.abp.io/en/abp/latest/Getting-Started?UI=NG&DB=EF&Tiered=No) guide if you don't know details of creating and running the solution.

### Backend

Assume that we have an `IBookAppService` interface:

````csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AngularProxyDemo.Books
{
    public interface IBookAppService : IApplicationService
    {
        public Task<List<BookDto>> GetListAsync();
    }
}
````

That uses a `BookDto` defined as shown:

```csharp
using System;
using Volo.Abp.Application.Dtos;

namespace AngularProxyDemo.Books
{
    public class BookDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public DateTime PublishDate { get; set; }
    }
}
```

And implemented as the following:

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AngularProxyDemo.Books
{
    public class BookAppService : ApplicationService, IBookAppService
    {
        public async Task<List<BookDto>> GetListAsync()
        {
            //TODO: get books from a database...
        }
    }
}
```

It simply returns a list of books. You probably want to get the books from a database, but it doesn't matter for this article.

### HTTP API

Thanks to the [auto API controllers](https://docs.abp.io/en/abp/latest/API/Auto-API-Controllers) system of the ABP Framework, we don't have to develop API controllers manually. Just **run the backend (*HttpApi.Host*) application** that shows the [Swagger UI](https://swagger.io/tools/swagger-ui/) by default. You will see the **GET** API for the books:

![swagger-book-list](swagger-book-list.png)

### Service Proxy Generation

Open a **command line** in the **root folder of the Angular application** and execute the following command:

````bash
abp generate-proxy
````

It should produce an output like the following:

````bash
...
CREATE src/app/proxy/books/book.service.ts (446 bytes)
CREATE src/app/proxy/books/models.ts (148 bytes)
CREATE src/app/proxy/books/index.ts (57 bytes)
CREATE src/app/proxy/index.ts (33 bytes)
````

> `generate-proxy` command can take some some optional parameters for advanced scenarios (like [modular development](https://docs.abp.io/en/abp/latest/Module-Development-Basics)). You can take a look at the [documentation](https://docs.abp.io/en/abp/latest/UI/Angular/Service-Proxies).

#### The Generated Code

`src/app/proxy/books/book.service.ts`: This is the service that can be injected and used to get the list of books;

````js
import type { BookDto } from './models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BookService {
  apiName = 'Default';

  getList = () =>
    this.restService.request<any, BookDto[]>({
      method: 'GET',
      url: `/api/app/book`,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
````

`src/app/proxy/books/models.ts`: This file contains the modal classes corresponding to the DTOs defined in the server side;

````js
import type { EntityDto } from '@abp/ng.core';

export interface BookDto extends EntityDto<string> {
  name: string;
  publishDate: string;
}
````

> There are a few more files have been generated to help you import the types easier.

#### How to Import

You can now import the `BookService` into any Angular component and use the `getList()` method to get the list of books.

````js
import { BookService, BookDto } from '../proxy/books';
````

You can also use the `@proxy` as a shortcut of the proxy folder:

````js
import { BookService, BookDto } from '@proxy/books';
````

### About the Generated Code

The generated code is;

* **Simple**: It is almost identical to the code if you've written it yourself.
* **Splitted**: Instead of a single, large file;
  * It creates a separate `.ts` file for every backend **service**. **Model** (DTO) classes are also grouped per service.
  * It understands the [modularity](https://docs.abp.io/en/abp/latest/Module-Development-Basics), so creates the services for your own **module** (or the module you've specified).
* **Object oriented**;
  * Supports **inheritance** of server side DTOs and generates the code respecting to the inheritance structure.
  * Supports **generic types**.
  * Supports **re-using type definitions** across services and doesn't generate the same DTO multiple times.
* **Well-aligned to the backend**;
  * Service **method signatures** match exactly with the services on the backend services. This is achieved by a special endpoint exposed by the ABP Framework that well defines the backend contracts.
  * **Namespaces** are exactly matches to the backend services and DTOs.
* **Well-aligned with the ABP Framework**;
  * Recognizes the **standard ABP Framework DTO types** (like `EntityDto`, `ListResultDto`... etc) and doesn't repeat these classes in the application code, but uses from the `@abp/ng.core` package.
  * Uses the `RestService` defined by the `@abp/ng.core` package which simplifies the generated code, keeps it short and re-uses all the logics implemented by the `RestService` (including error handling, authorization token injection, using multiple server endpoints... etc).

These are the main motivations behind the decision of creating a service proxy generation system, instead of using a pre-built tool like [NSWAG](https://github.com/RicoSuter/NSwag).

## Other Examples

Let me show you a few more examples.

### Updating an Entity

Assume that you added a new method to the server side application service, to update a book:

```csharp
public Task<BookDto> UpdateAsync(Guid id, BookUpdateDto input);
```

`BookUpdateDto` is a simple class defined shown below:

```csharp
using System;

namespace AngularProxyDemo.Books
{
    public class BookUpdateDto
    {
        public string Name { get; set; }

        public DateTime PublishDate { get; set; }
    }
}
```

Let's re-run the `generate-proxy` command:

````bash
abp generate-proxy
````

This command will re-generate the proxies by updating some files. Let's see some of the changes;

**book.service.ts**

````js
import type { BookDto, BookUpdateDto } from './models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BookService {
  apiName = 'Default';

  getList = () =>
    this.restService.request<any, BookDto[]>({
      method: 'GET',
      url: `/api/app/book`,
    },
    { apiName: this.apiName });

  update = (id: string, input: BookUpdateDto) =>
    this.restService.request<any, BookDto>({
      method: 'PUT',
      url: `/api/app/book/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
````

`update` function has been added to the `BookService` that gets an `id` and a `BookUpdateDto` as the parameters.

**models.ts**

````typescript
import type { EntityDto } from '@abp/ng.core';

export interface BookDto extends EntityDto<string> {
  name: string;
  publishDate: string;
}

export interface BookUpdateDto {
  name: string;
  publishDate: string;
}
````

Added a new DTO class: `BookUpdateDto`.

### Advanced Example

In this example, I want to show a DTO structure using inheritance, generics, arrays and dictionaries.

I've created an `IOrderAppService` as shown below:

````csharp
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AngularProxyDemo.Orders
{
    public interface IOrderAppService : IApplicationService
    {
        public Task CreateAsync(OrderCreateDto input);
    }
}
````

`OrderCreateDto` and the related DTOs are as the followings;

````csharp
using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace AngularProxyDemo.Orders
{
    public class OrderCreateDto : IHasExtraProperties
    {
        public Guid CustomerId { get; set; }

        public DateTime CreationTime { get; set; }

        //ARRAY of DTOs
        public OrderDetailDto[] Details { get; set; }

        //DICTIONARY
        public Dictionary<string, object> ExtraProperties { get; set; }
    }
    
    public class OrderDetailDto : GenericDetailDto<int> //INHERIT from GENERIC
    {
        public string Note { get; set; }
    }
    
    //GENERIC class
    public abstract class GenericDetailDto<TCount>
    {
        public Guid ProductId { get; set; }

        public TCount Count { get; set; }
    }
}
````

When I run the `abp generate-proxy` command again, I see there are some created and updated files. Let's see some important ones;

`src/app/proxy/orders/order.service.ts`

````js
import type { OrderCreateDto } from './models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  apiName = 'Default';

  create = (input: OrderCreateDto) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/order`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
````

`src/app/proxy/orders/models.ts`

````typescript
export interface GenericDetailDto<TCount> {
  productId: string;
  count: TCount;
}

export interface OrderCreateDto {
  customerId: string;
  creationTime: string;
  details: OrderDetailDto[];
  extraProperties: Record<string, object>;
}

export interface OrderDetailDto extends GenericDetailDto<number> {
  note: string;
}
````

## Conclusion

`abp generate-proxy` is a very handy command that creates all the necessary code to consume your ABP based backend HTTP APIs. It generates a clean code that is well aligned to the backend services and benefits from the power of TypeScript (by using generics, inheritance...).

## The Documentation

See [the documentation](https://docs.abp.io/en/abp/latest/UI/Angular/Service-Proxies) for details of the Angular Service Proxy Generation.