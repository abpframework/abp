# Angular Resource APIs Explained: Generating Signal-Based Service Proxies

Angular's shift toward **Signals** has changed how applications model state and data loading. Instead of manually coordinating subscriptions, loading flags, and error handling, Angular now provides resource APIs that integrate asynchronous data fetching directly into the signal ecosystem.

To embrace this programming model, ABP now offers an **optional Resource API generation mode** for Angular service proxies. When enabled, the proxy generator produces signal-friendly APIs for read operations while preserving the familiar Observable-based experience for the rest of your application.

This article introduces the new Resource API generation mode, explains why it exists, how it works, and when it should be preferred over traditional Observable-based proxies.

---

## Why Resource APIs

For years, Angular applications have relied on observable-based services to communicate with backend APIs. While Observables remain extremely powerful, consuming them inside components often requires additional state management.

A typical read operation usually involves:

- subscribing to an Observable,
- tracking loading state,
- storing the latest value,
- handling errors,
- cleaning up subscriptions when necessary.

As applications adopt Signals for local state, this pattern starts to feel increasingly imperative. The component becomes responsible not only for displaying data, but also for orchestrating the entire request lifecycle.

Angular's Resource APIs address this problem by treating asynchronous data as reactive state. Instead of manually reacting to changes, you describe **what the request depends on**, and Angular automatically keeps the resource synchronized.

For example, if a request depends on an entity ID represented as a `Signal`, changing that signal automatically triggers a new request. The component no longer needs to manually subscribe or invoke refresh logic simply because an input changed.

This approach offers several benefits:

- loading, error, and value state are managed together,
- requests automatically react to signal changes,
- components become more declarative,
- significantly less boilerplate is required.

Resource APIs are therefore an excellent fit for **read-only data retrieval**, where the primary goal is to keep the UI synchronized with backend state.

---

## What Angular Gives Us

Angular provides several APIs for working with asynchronous resources, each targeting a slightly different use case.

### `resource`

The `resource` API is Angular's generic primitive for asynchronous state.

It is designed for loaders that return promise-based results and automatically exposes:

- the current value,
- loading status,
- errors,
- reload capabilities.

This API is ideal when your asynchronous source is not based on RxJS.

### `rxResource`

Most Angular applications—including ABP applications—already communicate with the backend through `HttpClient`, whose APIs return Observables.

The `rxResource` helper bridges these existing Observable streams with Angular's Resource model. Instead of rewriting existing services to use Promises, it simply wraps an Observable-producing function and exposes it as a reactive resource.

Internally, the new ABP proxy generation mode uses this approach. Generated resource methods eventually call the existing `RestService` request pipeline, while exposing the result as an Angular `ResourceRef`. This means existing authentication, interceptors, multi-tenancy, localization, error handling, and other ABP infrastructure continue to work exactly as before.

### `httpResource`

Angular also provides `httpResource`, which is a specialized resource implementation built directly on top of `HttpClient`.

For applications making direct HTTP requests, this can be a convenient option.

However, ABP applications already centralize HTTP communication through `RestService`, which adds framework-specific behavior around every request. Because of that, the generated proxies rely on `rxResource` rather than `httpResource`, allowing them to preserve the entire ABP request pipeline while still providing a signal-based API.

In short:

| API | Intended for |
| --- | --- |
| `resource` | Generic asynchronous loaders returning Promises |
| `rxResource` | Existing RxJS/Observable-based data sources |
| `httpResource` | Direct `HttpClient` requests without additional abstraction |

Since ABP proxies already build on top of `RestService`, `rxResource` is the natural choice for bringing Signal-based data loading to generated client proxies.

---

## Why This Feature Is Opt-In

One of the primary goals of the new Resource API generation mode is to introduce a modern, Signal-friendly API **without disrupting existing applications**.

ABP's generated Angular service proxies have been Observable-based for years, and they continue to serve a wide range of applications effectively. Many projects already rely on RxJS operators, custom Observable pipelines, and existing component patterns. Replacing those generated APIs would introduce unnecessary breaking changes for little benefit.

Instead, Resource API generation is **completely opt-in**.

By passing the `--resource-api` option to the `abp generate-proxy` command, the generator produces additional resource helpers designed for Angular's Signal ecosystem. If you do not enable the option, proxy generation behaves exactly as it always has, producing the familiar Observable-based services.

This gradual approach offers several advantages:

- Existing applications continue working without modification.
- Teams can adopt Signals incrementally instead of performing a large migration.
- New pages can embrace Resource APIs while older features continue using Observables.
- Developers remain free to choose the programming model that best fits each feature.

Another intentional design decision is that **only read operations receive Resource API helpers**.

Resource APIs naturally model asynchronous state that can be refreshed whenever their reactive inputs change. This makes them an excellent fit for `GET` requests, where the goal is to retrieve and synchronize data.

Write operations such as `POST`, `PUT`, `PATCH`, and `DELETE` represent user actions rather than continuously synchronized state. These operations are typically composed with RxJS pipelines, optimistic updates, notifications, or custom error handling, making Observables the more appropriate abstraction.

By limiting Resource APIs to read operations, generated proxies follow Angular's recommended usage patterns while keeping mutation APIs predictable and familiar.

---

## Generator Behavior

Enabling Resource API generation requires only a single additional option:

```bash
abp generate-proxy -t ng --resource-api
```

Without this option, the generator behaves exactly as before, producing standard Observable-based service methods.

For example, a generated service might expose a method like:

```tsx
bookService.getList(input);
```

which returns an `Observable` and can be consumed with RxJS operators or converted into Signals using Angular's interoperability utilities.

When `--resource-api` is enabled, the generator **preserves these existing methods** and additionally generates companion Resource API methods for every `GET` endpoint.

For example:

```tsx
bookService.getList(...);          // Observable
bookService.getListResource(...);  // ResourceRef
```

These generated resource methods internally use Angular's `rxResource` while continuing to execute requests through ABP's existing `RestService`. As a result, the complete ABP request pipeline—including authentication, interceptors, localization, multi-tenancy, and error handling—remains unchanged.

The generator only creates resource helpers for `GET` endpoints. Mutation methods such as `create`, `update`, and `delete` continue to return Observables exactly as before.

This selective generation keeps the generated API straightforward:

- **Read operations** gain Signal-friendly Resource APIs.
- **Write operations** continue using Observables.
- **Existing Observable methods remain available**, allowing gradual adoption without forcing a migration.

In the next section, we'll examine the shape of the generated Resource API methods and see how they integrate naturally with Angular Signals.

---

## Generated API Shape

The generated Resource API methods closely resemble their Observable counterparts, making them easy to adopt. Rather than introducing an entirely new programming model, they adapt the existing proxy APIs to Angular's Signal ecosystem.

There are, however, a few important differences.

### Signal-Based Inputs

Traditional proxy methods accept plain values:

```tsx
bookService.get(id);
```

The generated Resource API methods instead accept **Signals**.

```tsx
const id = signal('42');

const book = bookService.getResource(id);
```

Because the input is reactive, Angular automatically re-executes the request whenever the signal value changes.

```tsx
id.set('43');
```

No additional method calls, subscriptions, or refresh logic are required—the resource stays synchronized with its dependencies automatically.

### Returning a `ResourceRef`

Observable-based proxy methods return an `Observable<T>`.

Resource methods instead return a `ResourceRef<T>`, which exposes the request state as Signals.

This gives components access to everything they typically need during data loading:

- the current value,
- loading status,
- any request error,
- reload functionality.

A component can consume the resource directly:

```tsx
const book = bookService.getResource(id);

book.value();
book.isLoading();
book.error();
book.reload();
```

Instead of maintaining separate signals for loading, data, and errors, the resource keeps these states together in a single reactive object.

### Reactive Request Construction

Many API endpoints require multiple parameters or request objects.

Rather than asking developers to manually rebuild those objects whenever an input changes, the generated methods construct the request reactively using Angular's `computed()` API.

Suppose a request depends on pagination and a search keyword:

```tsx
const page = signal(1);
const filter = signal('');
```

The generated proxy internally derives the request object from these Signals.

Whenever either value changes, Angular recomputes the request and automatically performs a new HTTP request.

This means developers only manage application state. The generated proxy takes care of determining **when** a request should be refreshed.

---

## Examples

Let's compare a few common scenarios to see how generated Resource APIs look in practice.

### Fetching a Single Entity

A traditional generated proxy might be used like this:

```tsx
bookService.get(id).subscribe(...);
```

With Resource API generation enabled:

```tsx
const id = signal('42');

const book = bookService.getResource(id);
```

Changing the ID automatically reloads the resource.

```tsx
id.set('84');
```

The component simply reads the latest value:

```tsx
book.value();
```

Angular handles the request lifecycle automatically.

### Fetching a List with Query Parameters

List endpoints become even more interesting because they usually depend on multiple reactive inputs.

Consider a search page with pagination.

```tsx
const filter = signal('');
const skipCount = signal(0);
const maxResultCount = signal(10);
```

Using the generated proxy is straightforward:

```tsx
const books = bookService.getListResource({
  filter,
  skipCount,
  maxResultCount,
});
```

Whenever any of these Signals changes, Angular automatically refreshes the resource.

```tsx
filter.set('Angular');
skipCount.set(20);
```

There is no need to manually call `getList()` again or synchronize subscriptions with UI state.

### Behind the Scenes

Although the generated API feels simple to consume, the proxy performs a considerable amount of work internally.

Conceptually, the generated method looks similar to the following:

```tsx
getListResource(input: {
  filter: Signal<string>;
  skipCount: Signal<number>;
  maxResultCount: Signal<number>;
}): ResourceRef<PagedResultDto<BookDto>> {
  return this.restService.requestResource({
    method: 'GET',
    url: '/api/app/books',
    params: computed(() => ({
      filter: input.filter(),
      skipCount: input.skipCount(),
      maxResultCount: input.maxResultCount(),
    })),
  });
}
```

Notice that every request parameter is read inside a `computed()` callback. This establishes Angular's reactive dependency tracking, ensuring that any change to an input Signal automatically triggers a new request.

As a consumer, you never need to think about this implementation detail. You simply update your Signals, and the generated proxy keeps the resource synchronized with your application's state.

---

## Best Practices

As with any new Angular feature, Resource APIs are most effective when used in the scenarios they were designed for. The following recommendations can help you get the most out of generated Resource-based proxies while keeping your codebase clean and maintainable.

### Use Resource APIs for Read Operations

Resource APIs are designed to represent **reactive, read-only state**.

Whenever your UI needs to retrieve data that should stay synchronized with one or more Signals, generated resource methods are an excellent choice.

Typical examples include:

- entity details,
- paginated lists,
- search results,
- dashboard widgets,
- lookup data.

These scenarios naturally benefit from automatic reloading whenever their reactive inputs change.

### Keep Mutations as Observable Methods

Operations that modify server state—such as creating, updating, or deleting data—should continue using the standard Observable-based proxy methods.

Mutation requests often involve additional workflows such as:

- displaying success notifications,
- handling validation errors,
- optimistic UI updates,
- chaining additional requests,
- navigation after completion.

These workflows fit naturally into RxJS pipelines, which is why the generator intentionally keeps write operations unchanged.

### Let Signals Drive Your Requests

One of the biggest advantages of Resource APIs is that you no longer decide **when** to issue a request.

Instead, you describe **what the request depends on**, and Angular takes care of the rest.

Rather than writing code like:

```tsx
filter.valueChanges.subscribe(() => loadBooks());
```

simply update the underlying Signals.

Whenever those Signals change, the generated resource automatically refreshes itself.

This declarative approach removes much of the manual subscription and synchronization logic that traditionally accumulates in Angular components.

### Build Request Objects with `computed()`

When an endpoint accepts multiple parameters, derive the request object using `computed()` instead of constructing it manually.

For example:

```tsx
const request = computed(() => ({
  filter: filter(),
  skipCount: skipCount(),
  maxResultCount: maxResultCount(),
}));
```

This ensures Angular can correctly track every dependency involved in the request.

Likewise, avoid reading Signal values outside of the reactive computation.

Instead of extracting Signal values first:

```tsx
const currentFilter = filter();

const request = computed(() => ({
  filter: currentFilter,
}));
```

read them directly inside the `computed()` callback:

```tsx
const request = computed(() => ({
  filter: filter(),
}));
```

Doing so allows Angular to react whenever those Signals change.

### Consume the Entire `ResourceRef`

A generated resource provides much more than the retrieved value.

Instead of creating separate Signals for loading and error state, consume the `ResourceRef` directly.

For example:

- `value()` exposes the latest successful result.
- `isLoading()` indicates whether a request is in progress.
- `error()` provides the latest request failure.
- `reload()` manually refreshes the resource when needed.

Keeping these states together makes components easier to understand and reduces state duplication.

### Choose the Right Resource API

Angular offers multiple Resource APIs, each serving a different purpose.

For generated ABP service proxies, the recommended approach is the generated Resource API based on `rxResource`, since it preserves the existing `RestService` pipeline and all of its framework integrations.

If you're writing standalone Angular services that communicate directly with `HttpClient`, `httpResource` may be a better fit.

Choosing the appropriate abstraction helps keep both your generated code and handwritten services consistent with Angular's intended usage patterns.

---

## Conclusion

Angular Signals have introduced a more declarative way to manage application state, and Resource APIs naturally extend that model to asynchronous data loading.

With Resource API generation, ABP brings this programming model directly into generated Angular service proxies. Read operations become reactive, Signal-driven, and significantly easier to consume, while write operations continue using the familiar Observable-based APIs that already work well for mutations.

Most importantly, this evolution is completely opt-in. Existing applications can continue using their current proxies without modification, while new features can gradually adopt Resource APIs wherever they provide the greatest benefit.

Whether you're building a new Signal-first Angular application or incrementally modernizing an existing ABP solution, Resource-based proxy generation offers a straightforward path toward cleaner components, less boilerplate, and a more reactive data-fetching experience.