# Angular 22 State Management: Signals, SignalStore, or NgRx?

Angular has been steadily moving toward a signal-first architecture since the introduction of Signals in Angular 16. With Angular 22, that transition reaches another milestone. Signals are now at the center of Angular's reactive programming model, while APIs such as Resource and Signal Forms have matured into production-ready solutions. Combined with the framework's continued investment in zoneless change detection, these improvements significantly influence how Angular applications should manage state.

This shift also changes the role of NgRx. While the classic NgRx Store remains a powerful solution for large, event-driven applications, many scenarios that previously required reducers, selectors, and effects can now be implemented with much simpler, feature-scoped signal stores. Rather than replacing NgRx, Angular 22 encourages developers to choose the right state management strategy based on the scope and complexity of the problem.

In this article, we'll explore how Angular 22 changes the state management landscape, compare the classic NgRx Store with NgRx SignalStore, and demonstrate best practices for building modern Angular applications. We'll also discuss how Angular's new reactive APIs fit into enterprise applications and what these changes mean for projects built with the ABP Framework.

## Why Angular 22 Changes State Management

Angular Signals introduced a fundamentally different approach by providing fine-grained reactivity built directly into the framework. Instead of propagating changes through Observable streams, Signals allow Angular to track exactly which pieces of state are consumed and update only the affected parts of the UI. This results in more predictable rendering, less boilerplate, and improved runtime performance.

Angular 22 builds on this foundation by making Signals the preferred reactive primitive throughout the framework. New APIs such as **Resource** for asynchronous data loading and **Signal Forms** for reactive forms integrate naturally with Signals, reducing the need for custom RxJS pipelines in many common scenarios.

For developers using NgRx, this doesn't mean abandoning existing applications or rewriting every store. Instead, it changes how state management should be approached. Component-local state can often be managed with plain Signals, feature-level state fits naturally into SignalStore, and the classic NgRx Store continues to excel for large-scale applications that benefit from centralized event streams, auditing, and global state synchronization.

Understanding these changing responsibilities is the key to designing maintainable Angular applications in the Angular 22 era. A practical way to think about state management is to start with the simplest solution and introduce additional abstractions only when the application's complexity requires them.

## Use Signals for Local Component State

Plain Angular Signals are ideal for state that belongs exclusively to a single component. Examples include dialog visibility, selected tabs, loading indicators, filter values, or temporary form data.

Signals provide a straightforward API with minimal overhead and integrate seamlessly with Angular's change detection. For state that never needs to be shared outside a component or its immediate children, introducing a dedicated store often adds unnecessary complexity.

A settings page often contains UI state that doesn't need to be shared with the rest of the application. Using a dedicated store for this would introduce unnecessary complexity.

```ts
@Component({...})
export class UserListComponent {
  readonly search = signal('');
  readonly showInactive = signal(false);

  readonly filteredUsers = computed(() =>
    this.users().filter(user =>
      user.name.includes(this.search()) &&
      (this.showInactive() || user.active)
    )
  );
}
```

This state is entirely local to the component and doesn't justify introducing a SignalStore.

## Use NgRx SignalStore for Feature State

As applications grow, state often needs to be shared across multiple components within the same feature. Examples include user profiles, shopping carts, administration screens, dashboards, or settings pages.

NgRx SignalStore is designed specifically for these scenarios. It combines Angular Signals with a lightweight, feature-oriented architecture where state, computed values, and business logic are defined together. Instead of scattering logic across reducers, selectors, effects, and services, developers can keep everything related to a feature inside a single store.

SignalStore also integrates naturally with Angular's signal-based APIs, making it an excellent choice for modern Angular applications built around Resources and Signal Forms.

A User Management module is shared by multiple pages. The selected user, filters, and loaded entities should remain synchronized across those pages.

```ts
export const UserStore = signalStore(
  withState({
    users: [] as User[],
    selectedUserId: null as number | null,
    loading: false,
  }),

  withComputed(({ users, selectedUserId }) => ({
    selectedUser: computed(() =>
      users().find(x => x.id === selectedUserId())
    ),
  })),

  withMethods((store) => ({
    selectUser(id: number) {
      patchState(store, { selectedUserId: id });
    },
  })),
);
```

Everything related to the feature lives in one place: state, derived values, and business operations.

## NgRx Store vs. NgRx SignalStore

Although both solutions belong to the NgRx ecosystem, they are designed to solve different architectural problems.

The classic NgRx Store follows the Redux pattern, where every state change is represented by an action that flows through reducers before producing a new immutable state. This explicit, event-driven architecture provides excellent traceability and scales well for applications with extensive global interactions.

SignalStore takes a different approach. Instead of centering the application around dispatched actions, it treats state as a reactive service built with Angular Signals. A SignalStore typically contains three core building blocks:

- **State**, which represents the application's reactive data.
- **Computed signals**, which derive values from existing state.
- **Methods**, which encapsulate business logic and state updates.

This functional model significantly reduces boilerplate while remaining predictable and testable. Since it builds directly on Angular Signals, it also integrates naturally with Angular's fine-grained change detection without requiring selectors or `async` pipes for many common scenarios.

The following comparison summarizes the strengths of each approach.


| Feature          | Classic NgRx Store              | NgRx SignalStore                  |
| ---------------- | ------------------------------- | --------------------------------- |
| Architecture     | Redux-based global store        | Feature-oriented reactive store   |
| Reactivity       | RxJS Observables                | Angular Signals                   |
| Boilerplate      | Higher                          | Lower                             |
| State Scope      | Global application state        | Feature or route state            |
| Side Effects     | Effects                         | Store methods or `rxMethod`       |
| Change Detection | Observable subscriptions        | Native signal reactivity          |
| Best For         | Large event-driven applications | Modern feature-based applications |


For most new Angular 22 applications, SignalStore is an excellent default choice for feature-level state management because it embraces the framework's signal-first architecture while keeping code concise and maintainable. The classic NgRx Store remains indispensable for applications that rely heavily on centralized event processing, global synchronization, or advanced debugging capabilities.

Instead of asking *"Which one should I use?"*, the better question is *"Which scope of state am I trying to manage?"* The answer usually determines the appropriate solution.

## Angular 22 Features That Improve State Management

Angular 22 introduces several framework APIs that naturally complement modern state management patterns. Rather than replacing NgRx, these APIs reduce the amount of custom infrastructure developers previously had to build around it.

### Resource API

One of the most significant additions is the **Resource API**, which provides a signal-based approach to asynchronous data loading.

Historically, fetching remote data in Angular involved coordinating `HttpClient`, RxJS operators, subscriptions, loading flags, and error handling. While these patterns remain valid, they often require considerable boilerplate even for straightforward scenarios.

Resources encapsulate these concerns into a single reactive abstraction. A Resource automatically tracks the signals it depends on, performs requests when those dependencies change, cancels obsolete requests, and exposes its lifecycle through reactive state such as the current value, loading status, and errors.

This makes Resources particularly well suited for read-oriented operations where data should stay synchronized with application state.

For example, changing a selected user ID can automatically trigger a new request without manually wiring `switchMap` or managing subscription lifecycles.

```tsx
const userResource = httpResource(() => ({
  url: `/api/users/${selectedUserId()}`
}));
```

### Signal Forms

Another major improvement is the stabilization of **Signal Forms**.

Traditional Reactive Forms expose their state through `FormControl` and `FormGroup` instances, requiring developers to query validation status, dirty state, touched state, and values through an imperative API.

Signal Forms expose these properties as signals instead. Every field becomes reactive by default, making templates easier to read while eliminating much of the manual state synchronization commonly found in form-heavy applications.

```html
@if (profileForm.email.invalid() && profileForm.email.touched()) {
  <span>Please enter a valid email.</span>
}
```

Because field state is already reactive, components rarely need additional subscriptions or helper observables to keep the UI synchronized.

It's important to note that Signal Forms are responsible for **UI state**, while business operations such as saving data, loading entities, or handling server responses still belong in a dedicated service or SignalStore. Keeping these responsibilities separate results in components that remain focused on presentation while stores continue to own application logic.

## Best Practices for Building Modern SignalStores

SignalStore significantly reduces the ceremony traditionally associated with state management, but the same architectural principles still apply. A well-designed store should encapsulate business logic without becoming responsible for concerns that belong elsewhere.

1. Keep Stores Focused on a Single Feature
  A SignalStore should represent a cohesive business feature rather than becoming a global container for unrelated state.
  For example, an administration module might expose separate stores for users, roles, and permissions instead of combining all administrative functionality into a single, monolithic store. Smaller stores are easier to test, understand, and maintain over time.
2. Store Business State, Not UI State
  Not every piece of state belongs in a store.
  Transient UI concerns such as dialog visibility, selected tabs, expanded panels, or temporary input values are usually better managed with plain Signals inside the component.
  Stores should own state that represents the application's business domain—entities, filters, permissions, settings, or data shared across multiple components.
3. Derive State Instead of Duplicating It
  Whenever possible, compute values instead of storing them.
  SignalStore's `withComputed()` feature makes it easy to derive reactive values from existing state, reducing the likelihood of inconsistent or stale data.
  Instead of storing both a list of users and an active user count, derive the count directly from the collection.
  ```tsx
  withComputed(({ users }) => ({
    activeUsers: computed(() =>
      users().filter(user => user.active).length
    ),
  }))
  ```
  Keeping a single source of truth simplifies updates and reduces maintenance.
4. Prefer Immutable State Updates
  Although SignalStore simplifies updates through `patchState()`, state should still be treated as immutable.
  Updating only the affected portions of state makes changes predictable and allows Angular's signal system to     efficiently notify dependent computations.
  ```tsx
  patchState(store, {
    users: [...store.users(), newUser]
  });
  ```

## Integrating Resources with SignalStore

Resources and SignalStore solve different problems, and understanding their responsibilities leads to a cleaner architecture.

A **Resource** is responsible for synchronizing data with a remote source. It knows how to load data, react to parameter changes, expose loading and error states, and keep requests up to date.

A **SignalStore**, on the other hand, owns the application's business state. It coordinates operations, exposes domain-specific methods, derives computed values, and serves as the single source of truth for a feature.

Rather than replacing one another, they work best together.

A common pattern is to use a Resource for loading entities while allowing the store to expose business operations that modify those entities.

```tsx
export const UserStore = signalStore(
  withState({
    selectedUserId: undefined as number | undefined,
  }),

  withComputed(({ selectedUserId }) => ({
    userResource: httpResource<User>(() => {
      const id = selectedUserId();

      return id
        ? {
            url: `/api/users/${id}`,
          }
        : undefined;
    }),
  })),

  withMethods((store) => ({
    selectUser(id: number) {
      patchState(store, {
        selectedUserId: id,
      });
    },
  })),
);
```

In this example, changing the selected user automatically causes the Resource to fetch new data. The store doesn't need to manage subscriptions or manually coordinate loading indicators because the Resource already exposes this information through signals.

This separation keeps data synchronization declarative while allowing the store to remain focused on business behavior.

## Integrating Signal Forms with SignalStore

Signal Forms and SignalStore naturally complement one another because both are built on Angular Signals. However, they should not be treated as interchangeable.

Signal Forms are responsible for managing user input and validation, while SignalStore coordinates business operations such as loading, updating, and persisting data.

A common workflow consists of four steps:

1. Load the entity through the store.
2. Populate the Signal Form.
3. Allow the user to edit the data.
4. Submit the updated values back to the store.

The component remains responsible only for orchestrating the interaction between the form and the store.

```tsx
@Component({
  // ...
})
export class UserEditorComponent {
  readonly store = inject(UserStore);

  readonly form = form({
    name: '',
    email: '',
  });

  async save() {
    if (this.form.invalid()) {
      return;
    }

    await this.store.updateUser(this.form.value());
  }
}
```

This approach keeps presentation concerns inside the component while allowing business rules to remain centralized in the store.

## Handling Asynchronous Operations

One challenge when combining Signal Forms with SignalStore is coordinating asynchronous operations.

A form submission typically expects an asynchronous operation to complete before updating its own state. Meanwhile, the store is responsible for managing loading indicators, server errors, and successful updates.

Instead of placing HTTP requests directly inside components, expose descriptive methods such as `createUser()`, `updateProfile()`, or `changePassword()` from the store. Components simply invoke these methods and react to the outcome.

This keeps components lightweight while making business logic reusable across multiple views.

## A Clear Separation of Responsibilities

A useful guideline is to divide responsibilities as follows:


| Concern             | Recommended Owner                          |
| ------------------- | ------------------------------------------ |
| User input          | Signal Forms                               |
| Validation          | Signal Forms                               |
| Loading remote data | Resource                                   |
| Business rules      | SignalStore                                |
| State mutations     | SignalStore                                |
| HTTP persistence    | Service or repository invoked by the store |


Following these boundaries results in components that focus on presentation, stores that encapsulate business logic, and Resources that handle server synchronization. Each part has a single responsibility, making the application easier to understand, test, and maintain as it grows.

## Migrating from Classic NgRx to SignalStore

Migrating to SignalStore doesn't require replacing an entire application's state management strategy overnight. In fact, most enterprise applications can adopt SignalStore incrementally while continuing to use the classic NgRx Store where it provides the greatest value.

A practical migration strategy is to start with isolated features rather than the application's global state.

### Keep the Classic Store for Global State

Global concerns such as authentication, user sessions, application configuration, notifications, and cross-feature communication often continue to benefit from the centralized architecture of the classic NgRx Store.

These areas typically rely on dispatched actions and event-driven workflows that remain well suited to Redux patterns.

### Introduce SignalStore for New Features

New feature modules are excellent candidates for SignalStore.

Instead of creating actions, reducers, selectors, and effects, developers can define state, computed values, and business methods in a single store. This reduces boilerplate while aligning the feature with Angular's signal-first architecture.

Existing features can also be migrated gradually as they evolve, avoiding large-scale refactoring efforts.

### Move Component State First

The easiest migration is often replacing component-local Observables and `BehaviorSubject`s with Signals.

Many components don't require a dedicated store at all. Converting temporary UI state to Signals simplifies the codebase immediately and familiarizes teams with Angular's reactive model before introducing SignalStore.

Incremental adoption minimizes risk while allowing teams to modernize applications at a sustainable pace.

## What This Means for ABP Applications

Angular 22's signal-first architecture aligns well with ABP's modular application model.

Most ABP applications consist of independent feature modules such as Identity, Tenant Management, SaaS, or CMS. These modules naturally map to feature-scoped SignalStores, allowing state and business logic to remain encapsulated within each module. However, the full support will be introduced in the next version.

As Angular continues investing in Signals, Resources, and Signal Forms, future ABP applications can increasingly rely on the framework's native reactive APIs instead of custom state management patterns.

This doesn't diminish the importance of RxJS or the classic NgRx Store. RxJS remains an essential foundation of Angular's HTTP infrastructure and many third-party libraries, while the traditional Store continues to provide an excellent solution for complex global state management.

Instead, Angular 22 encourages developers to use each reactive tool where it provides the greatest value.

Whether you're upgrading an existing ABP application or starting a new project, adopting SignalStore for feature-level state can simplify development while remaining fully compatible with Angular's evolving ecosystem.

## Conclusion

Angular's evolution toward a signal-first architecture represents more than a new reactive API—it changes how applications should be designed.

Rather than treating every piece of state as part of a centralized store, Angular now encourages developers to choose the appropriate abstraction for each responsibility. Plain Signals excel at local component state, SignalStore provides a lightweight solution for feature-level business logic, Resources simplify server synchronization, and Signal Forms modernize user input management.

The classic NgRx Store continues to play an important role in large, event-driven applications, but it no longer needs to be the default choice for every state management scenario.

By embracing these complementary tools, developers can build Angular applications that are simpler to maintain, require less boilerplate, and integrate naturally with the framework's latest capabilities.

As Angular continues to evolve around Signals and fine-grained reactivity, adopting these patterns today will help applications remain aligned with the framework's direction while providing a solid foundation for future improvements.