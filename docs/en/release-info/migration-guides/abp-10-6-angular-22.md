```json
//[doc-seo]
{
    "Description": "Upgrade your ABP solutions to Angular version 22.0.x"
}
```

# Angular 22 and ABP 10.6 Upgrade Guide

This guide explains how to upgrade ABP Angular applications to **Angular 22** and **TypeScript 6**.

## 1. Target Versions

Update all frontend dependencies together to maintain compatibility:

* `@angular/*` → `~22.0.0`
* `typescript` → `~6.0.0`
* `@abp/*` → corresponding ABP release version
* `@volo/*`, `@volosoft/*` (if applicable) → corresponding commercial release version
* `angular-oauth2-oidc` (if applicable) → `~22.0.0`

Avoid mixing Angular 21 and Angular 22 packages within the same workspace.

## 2. Prerequisites

Before starting the upgrade:

1. Use a Node.js version supported by Angular 22.
2. Ensure your backend ABP version is compatible with the frontend package versions you plan to install.
3. Commit or back up your current project state.

## 3. Upgrade Process

1. Update package versions in `package.json`.
2. Run the Angular or Nx migration commands applicable to your project.
3. Remove existing installation artifacts:

   * Delete `node_modules`
   * Delete the lock file (`package-lock.json`, `yarn.lock`, or `pnpm-lock.yaml`)
4. Reinstall all dependencies.
5. Build the application and resolve any compilation or template errors.

## 4. Required Changes

### 4.1 TypeScript 6 Deprecation Handling

Projects that still use `downlevelIteration: true` may encounter TypeScript 6 deprecation diagnostics.

Add the following temporary setting to your root `tsconfig` file (and library production configurations if required):

```json
{
  "compilerOptions": {
    "downlevelIteration": true,
    "ignoreDeprecations": "6.0"
  }
}
```

As a long-term solution, remove `downlevelIteration` when your target runtime environment no longer requires it.

### 4.2 Updated Change Detection Behavior

Angular 22 introduces updated change detection behavior for components that do not explicitly configure a change detection strategy.

Common symptoms include:

* Loading indicators not updating
* Modal busy states not clearing
* Lists or charts not refreshing after asynchronous operations

Recommended approaches:

1. Use `signal()` for component state.
2. Use `toSignal()` when consuming observable streams.
3. Use the `async` pipe for observable-based UI state.
4. Use `ChangeDetectionStrategy.Eager` only as a temporary compatibility measure for legacy components.

### 4.3 ABP List Pages (`ListService`)

When working with `ListService`, prefer converting observable results to signals instead of manually subscribing.

```typescript
readonly data = toSignal(
  this.list.hookToQuery(query => this.service.getList(query)),
  { initialValue: { items: [], totalCount: 0 } },
);
```

Update template bindings accordingly:

* `data.items` → `data().items`
* `data.totalCount` → `data().totalCount`

### 4.4 Modals and Loading States

For components such as `abp-modal`, `abp-button`, and permission or feature management dialogs, maintain state using signals.

```typescript
readonly isModalVisible = signal(false);
readonly modalBusy = signal(false);
```

```html
<abp-button [loading]="modalBusy()" />
<abp-modal
  [visible]="isModalVisible()"
  (visibleChange)="isModalVisible.set($event)"
  [busy]="modalBusy()"
/>
```

If you use `*abpReplaceableTemplate`, pass signal values through `inputs.value` and update state through the corresponding event callbacks.

### 4.5 Template Type Checking

Angular 22 enables `strictTemplates` by default.

Resolve template typing issues where possible, or temporarily disable strict template checking:

```json
{
  "angularCompilerOptions": {
    "strictTemplates": false
  }
}
```

Common adjustments include:

* Updating optional chaining (`?.`) and null coalescing (`??`) usage
* Guarding optional form references before binding
* Resolving duplicate input or output bindings

### 4.6 Upload Progress Events

Applications that rely on upload progress events should include the XHR backend in browser-side HTTP configuration:

```typescript
provideHttpClient(withFetch(), withXhr())
```

Do not enable the XHR backend in server-side rendering (SSR) bootstrap code.

### 4.7 Chart Components (`abp-chart`)

If charts do not update after asynchronous data loading:

* Store chart data in a signal
* Bind chart inputs using signal values (for example, `[data]="chartData()"`)
* Call `reinit()` after assigning new data rather than relying solely on `refresh()`

## 5. Custom or Forked UI Modules

If your project contains customized copies of ABP modules such as Identity, Tenant Management, Account, or CMS Kit:

1. Compare your implementation with the updated package versions.
2. Apply the recommended signal-based state management patterns.
3. Re-test CRUD pages, permission dialogs, feature dialogs, and account-related workflows.

## 6. Validation Checklist

After completing the upgrade, verify that:

* Dependencies are installed correctly without duplicate Angular versions
* The application builds successfully
* Unit tests pass (if applicable)
* Login, registration, and password recovery workflows function correctly
* CRUD list pages refresh as expected
* Modal loading and busy states behave correctly
* Permission and feature dialogs open and close correctly
* Upload progress events work as expected (if applicable)
* Dashboard charts render correctly after data is loaded

## 7. Troubleshooting

| Symptom                                                           | Likely Cause                              | Resolution                                                  |
| ----------------------------------------------------------------- | ----------------------------------------- | ----------------------------------------------------------- |
| Form type conflicts (`AbstractControl`, etc.)                     | Multiple Angular versions installed       | Align package versions and perform a clean reinstall        |
| TypeScript deprecation errors related to `downlevelIteration`     | TypeScript 6 diagnostics                  | Add `ignoreDeprecations: "6.0"` temporarily                 |
| Errors involving optional configuration or environment properties | Stricter type checking                    | Add null checks and optional chaining where appropriate     |
| DTO or library compilation issues                                 | Type incompatibilities in DTO definitions | Prefer interfaces and optional properties where appropriate |
| Upload progress events are not emitted                            | Missing XHR backend configuration         | Add `withXhr()` to browser-side HTTP configuration          |
| Charts remain empty after data loads                              | State changes are not being detected      | Use signals and call `reinit()` after updating chart data   |

## 8. Summary

When upgrading to Angular 22, focus on the following areas:

1. Upgrade Angular, TypeScript, ABP, and commercial packages together.
2. Update UI state management to use signals, `toSignal()`, or the `async` pipe where appropriate.
3. Resolve TypeScript 6 and template type-checking issues.
4. Validate critical application workflows, including modals, list pages, uploads, and chart components.
