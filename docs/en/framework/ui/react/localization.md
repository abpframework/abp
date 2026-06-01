```json
//[doc-seo]
{
    "Description": "Learn how localization works in ABP React UI applications with i18next and ABP application configuration."
}
```

# Localization

ABP React UI templates use [i18next](https://www.i18next.com/) with [react-i18next](https://react.i18next.com/). The generated app includes local JSON resources and integrates with ABP application configuration through the `@volo/abp-app-config` packages.

## Localization Files

The main React app stores client-side translations under `src/locales/`.

```text
src/
├── locales/
│   └── en.json
└── lib/
    └── i18n/
        └── i18n.ts
```

The default `i18n.ts` imports the English resource and registers it:

```ts
import i18n from 'i18next'
import { initReactI18next } from 'react-i18next'
import en from '@/locales/en.json'

i18n.use(initReactI18next).init({
  resources: {
    en: { translation: en },
  },
  lng: 'en',
  fallbackLng: 'en',
  keySeparator: false,
  nsSeparator: false,
  interpolation: {
    escapeValue: false,
  },
})
```

`keySeparator` and `nsSeparator` are disabled so ABP-style keys such as `AbpIdentity::Users` and `Menu:Home` can be used directly.

## Using Localized Text

Use `useTranslation()` from `react-i18next` in components:

```tsx
import { useTranslation } from 'react-i18next'

export function BooksTitle() {
  const { t } = useTranslation()

  return <h1>{t('Menu:Books')}</h1>
}
```

ABP localization keys commonly use the `ResourceName::Key` format:

```tsx
{t('AbpIdentity::Users')}
{t('AbpAccount::Login')}
{t('AbpUi::SavedSuccessfully')}
```

Application-specific menu keys may use names like `Menu:Home` or `Menu:Books`.

## Adding a Translation

Add the key to `src/locales/en.json`:

```json
{
  "Menu:Reports": "Reports",
  "Reports": "Reports"
}
```

Then use it from a component:

```tsx
const { t } = useTranslation()

return <h1>{t('Reports')}</h1>
```

## Adding a Language

Create a new JSON file, for example `src/locales/tr.json`:

```json
{
  "Menu:Reports": "Raporlar",
  "Reports": "Raporlar"
}
```

Register it in `src/lib/i18n/i18n.ts`:

```ts
import en from '@/locales/en.json'
import tr from '@/locales/tr.json'

i18n.use(initReactI18next).init({
  resources: {
    en: { translation: en },
    tr: { translation: tr },
  },
  lng: 'en',
  fallbackLng: 'en',
})
```

If you add a language selector, call `i18n.changeLanguage('tr')` when the user chooses Turkish.

## Server-Side ABP Localization

ABP's backend localization system is still the source of truth for server-defined resources, validation messages, exception messages, and module texts. The React app uses ABP application configuration through `@volo/abp-app-config` / `@volo/abp-react-app-config` for auth and configuration data, and these packages can include localization resources when configured to do so.

The main template currently creates the app configuration client with:

```ts
export const appConfig = createAbpReactAppConfig({
  baseUrl: () => getApiUrl(),
  includeLocalizationResources: false,
})
```

Because `includeLocalizationResources` is disabled in the main React template, UI text is normally loaded from `src/locales/*.json`. If you enable server-provided localization resources, make sure your UI initialization merges them into i18next before rendering localized components.

## Request Culture

The shared Axios client sends the active i18next language with each request:

```ts
if (i18n?.language) {
  config.headers['Accept-Language'] =
    config.headers['Accept-Language'] ?? i18n.language
}
```

This lets backend responses, validation messages, and exception messages use the selected culture when the server supports it.

## Admin Console Localization

The Admin Console has its own React app and localization setup. In layered and single-layer templates, it is served from the `Volo.Abp.AdminConsole` package. In microservice templates, it is generated as `apps/react-admin-console/`.

The Admin Console host can expose available languages through `AdminConsole:LocalizationLanguages`, and `/admin-console/api/config` returns the normalized language list.

## See Also

- [React UI](./index.md)
- [HTTP Requests](./http-requests.md)
- [Localization](../../../framework/fundamentals/localization.md)
