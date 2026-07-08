```json
//[doc-seo]
{
    "Description": "Upgrade your ABP solutions to Angular version 22.0.x"
}
```

# Release Notes: Angular 22 and TypeScript 6 Upgrade

## Overview

This release updates ABP Angular UI applications to:

* Angular `22.x`
* TypeScript `6.x`

This upgrade aligns ABP projects with the latest Angular ecosystem and provides access to the newest framework improvements while ensuring long-term maintainability and support.

## What's Changed

### 1. Frontend Stack Upgrades

The core frontend stack has been updated:

* `@angular/*` packages have been upgraded to version 22
* `typescript` has been upgraded to version 6
* ABP and ABP Commercial npm packages must be upgraded to the corresponding ABP release line (version 10.6)

### 2. Change Detection Behavior

Angular 22 introduces updated change detection behavior.

* Components without explicit change detection configuration now follow OnPush-style behavior by default
* Some existing pages may no longer update automatically after asynchronous operations
* UI state should be managed using Angular Signals or the `async` pipe where appropriate

### 3. Stricter Type and Template Checks

Angular 22 and TypeScript 6 introduce additional compile-time validations.

* More template and type-related issues may be reported during builds
* Existing assumptions around nullable values and optional properties may require additional guards or type refinements
* Applications with strict template checking enabled may require code updates

### 4. Upload Progress Handling

Applications that rely on file upload progress events may require additional HTTP client configuration.

* Browser-side HTTP configuration may need `withXhr()` enabled to ensure upload progress events are emitted correctly

### 5. Chart Update Behavior

Chart components may require additional updates when used with asynchronous data sources.

* Under OnPush-style change detection, chart updates may not be detected automatically
* Consider using Signals for chart data bindings
* Calling `reinit()` after asynchronous data updates may be necessary in some scenarios

## Required Actions

### 1. Upgrade Related Packages Together

Keep Angular, TypeScript, ABP, and ABP Commercial packages on compatible versions.

To use Angular 22:

* Angular: `22.x`
* TypeScript: `6.x`
* ABP Framework: `10.6.x`

### 2. Review UI State Management

Review pages that depend on asynchronous state updates, including:

* List and table data
* Loading and busy indicators
* Modal dialog state
* Dashboard and chart data

Consider migrating these scenarios to Angular Signals or the `async` pipe.

### 3. Apply a Temporary TypeScript Compatibility Setting (If Needed)

If your project uses `downlevelIteration`, you may temporarily add the following configuration:

```json
{
  "ignoreDeprecations": "6.0"
}
```

This can help ease the migration process while addressing TypeScript 6 deprecation warnings.

### 4. Perform Regression Testing

We recommend validating all critical application flows after upgrading, including:

* Authentication and account management pages
* CRUD list and detail pages
* Permission and feature management dialogs
* File upload workflows
* Dashboard and chart components

## Areas to Validate Carefully

Pay particular attention to the following scenarios:

* Busy or loading indicators not updating correctly
* Modal open/close state inconsistencies
* List pages not refreshing after asynchronous operations
* Upload progress events not being emitted
* Charts rendering without data after API responses

## References

* Detailed migration guide: [Upgrade ABP to 10.6](../../../../release-info/migration-guides/abp-10-6-angular-22.md)
