```json
//[doc-seo]
{
  "Description": "Combine backend and UI localizations in Angular: use JSON files to override or extend server-sidtexts with the same key format and abpLocalization pipe."
}
```

# Hybrid Localization

Hybrid localization lets you combine **backend localizations** (from the ABP server) with **UI localizations** (JSON files in your Angular app). UI values take priority over backend values for the same key, so you can override or extend server-side texts without changing the backend.

## How It Works

- **Backend localizations**: Loaded from the server (e.g. `ApplicationLocalizationResourceDto`). Keys use the format `ResourceName::Key`.
- **UI localizations**: Loaded from static JSON files under your app's assets (e.g. `/assets/localization/en.json`). The same key format `ResourceName::Key` is used.
- **Priority**: When a key exists in both backend and UI, the **UI value is used** (UI overrides backend).

The existing `abpLocalization` pipe and localization APIs work unchanged; they resolve keys from the merged set (backend + UI), with UI winning on conflicts.

## Configuration

Enable hybrid localization in your app config via `provideAbpCore` and `withOptions`:

```typescript
// app.config.ts
import { provideAbpCore, withOptions } from "@abp/ng.core";

export const appConfig: ApplicationConfig = {
  providers: [
    provideAbpCore(
      withOptions({
        // ...other options
        uiLocalization: {
          enabled: true,
          basePath: "/assets/localization", // optional; default is '/assets/localization'
        },
      }),
    ),
    // ...
  ],
};
```

| Option     | Description                                                                  | Default                  |
| ---------- | ---------------------------------------------------------------------------- | ------------------------ |
| `enabled`  | Turn on UI localization loading from `{basePath}/{culture}.json`.            | —                        |
| `basePath` | Base path for JSON files. Files are loaded from `{basePath}/{culture}.json`. | `'/assets/localization'` |

When `enabled` is `true`, the app loads a JSON file for the current language (e.g. `en`, `tr`) whenever the user changes language. Loaded data is merged with backend localizations (UI overrides backend for the same key).

## UI Localization File Format

Place one JSON file per culture under your `basePath`. File name must be `{culture}.json` (e.g. `en.json`, `tr.json`).

Structure: **resource name → key → value**.

```json
{
  "MyProjectName": {
    "Welcome": "Welcome from UI (en.json)",
    "CustomKey": "This is a UI-only localization",
    "TestMessage": "UI localization is working!"
  },
  "AbpAccount": {
    "Login": "Sign In (UI Override)"
  }
}
```

- Top-level keys are **resource names** (e.g. `MyProjectName`, `AbpAccount`).
- Nested keys are **localization keys**; values are the display strings for that culture.

In templates you keep using the same key format: `ResourceName::Key`.

## Using in Templates

Use the `abpLocalization` pipe as usual. Keys can come from backend only, UI only, or both (UI wins):

```html
<!-- Backend (if available) or UI -->
<p>{%{{ 'MyProjectName::Welcome' | abpLocalization }}%}</p>

<!-- UI-only key (from /assets/localization/{{ culture }}.json) -->
<p>{%{{ 'MyProjectName::CustomKey' | abpLocalization }}%}</p>

<!-- Backend key overridden by UI -->
<p>{%{{ 'AbpAccount::Login' | abpLocalization }}%}</p>
```

No template changes are needed; only the configuration and the JSON files.

## UILocalizationService

The `UILocalizationService` (`@abp/ng.core`) manages UI localizations and merges them with backend data.

### Get loaded UI data

To inspect what was loaded from the UI JSON files (e.g. for debugging or display):

```typescript
import { UILocalizationService, SessionStateService } from "@abp/ng.core";

export class MyComponent {
  private uiLocalizationService = inject(UILocalizationService);
  private sessionState = inject(SessionStateService);

  currentLanguage$ = this.sessionState.getLanguage$();

  ngOnInit() {
    // All loaded UI resources for current language
    const loaded = this.uiLocalizationService.getLoadedLocalizations();
    // Or for a specific culture
    const loadedEn = this.uiLocalizationService.getLoadedLocalizations("en");
  }
}
```

`getLoadedLocalizations(culture?: string)` returns an object of the form `{ [resourceName: string]: Record<string, string> }` for the given culture (or current language if omitted).

### Add translations at runtime

You can also add or merge UI translations programmatically (e.g. from another source or lazy-loaded module):

```typescript
this.uiLocalizationService.addAngularLocalizeLocalization(
  'en',                    // culture
  'MyProjectName',         // resource name
  { MyKey: 'My value' },   // key-value map
);
```

This merges into the existing UI localizations and is taken into account by the `abpLocalization` pipe with the same UI-over-backend priority.

## Example: Dev App

The ABP dev app demonstrates hybrid localization:

1. **Config** (`app.config.ts`):

```typescript
uiLocalization: {
  enabled: true,
  basePath: '/assets/localization',
},
```

2. **Files**: `src/assets/localization/en.json` and `src/assets/localization/tr.json` with the structure shown above.

3. **Component** (`localization-test.component.ts`): Uses `abpLocalization` for backend keys, UI-only keys, and overrides; and uses `UILocalizationService.getLoadedLocalizations()` to show loaded UI data.

See `apps/dev-app/src/app/localization-test/localization-test.component.ts` and `apps/dev-app/src/assets/localization/*.json` in the repository for the full example.

## Summary

| Topic            | Description |
|------------------|-------------|
| **Purpose**      | Combine backend and UI localizations; UI overrides backend for the same key. |
| **Config**       | `provideAbpCore(withOptions({ uiLocalization: { enabled: true, basePath?: string } }))`. |
| **File location**| `{basePath}/{culture}.json` (e.g. `/assets/localization/en.json`). |
| **JSON format**   | `{ "ResourceName": { "Key": "Value", ... }, ... }`. |
| **Template usage** | Same as before: `{%{{ 'ResourceName::Key' \| abpLocalization }}%}`. |
| **API**          | `UILocalizationService`: `getLoadedLocalizations(culture?)`, `addAngularLocalizeLocalization(culture, resourceName, translations)`. |
