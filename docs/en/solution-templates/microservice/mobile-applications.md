```json
//[doc-seo]
{
    "Description": "Understand the optional React Native mobile application in ABP Studio's modern microservice solution template."
}
```

# Microservice Solution: Mobile Applications

````json
//[doc-nav]
{
  "Next": {
    "Name": "Built-in features in the Microservice solution",
    "Path": "solution-templates/microservice/built-in-features"
  }
}
````

> You must have an ABP Business or a higher license to be able to create a microservice solution.

The current ABP Studio modern microservice solution template can optionally generate a mobile application under `apps/mobile/react-native`.

The modern template does not generate a MAUI mobile application. Its mobile choices are **None** and **React Native**.

## The Mobile Gateway

If you enable the mobile application, an API gateway named `MobileGateway` is added under `gateways/mobile`. The React Native client calls backend APIs through this gateway.

See *[API Gateways](api-gateways.md)* for the shared gateway structure.

## Authentication

The generated React Native app is configured in `Environment.ts` with:

* the `AuthServer` issuer URL
* the `MobileGateway` base URL
* the `ReactNative` client id and scopes

At runtime, the mobile client uses the password grant to exchange credentials for access and refresh tokens at the `AuthServer` `/connect/token` endpoint, then sends bearer tokens to backend APIs through the `MobileGateway`. Account-related operations such as registration, password reset, profile picture management, and logout use the generated API client under `src/api`.

## Built-in Capabilities

The generated mobile app already includes screens and flows for:

* sign in, registration, forgot password, and reset password
* account and profile picture management
* user-facing settings such as language, theme, and logout

Localization is handled by the bundled locale files under `src/locales` and `src/services/LocalizationService.ts`. Theme handling lives under `src/theme`.

The UI is built with [NativeWind v4](https://www.nativewind.dev/) (Tailwind CSS for React Native) with full light/dark mode support. The `useThemeColors` hook returns light/dark palette values for components that need explicit colors instead of NativeWind `className`. See [Styling with NativeWind](../../framework/ui/react-native/styling-with-nativewind.md) for the styling system reference.

## Solution Structure

The React Native application is based on [React Native](https://reactnative.dev/) and [Expo](https://expo.dev/). The main files and folders in `apps/mobile/react-native` are:

* `Environment.ts`: runtime URLs, client id, scopes, and default localization resource name.
* `src/api`: HTTP clients for token, account, and application configuration calls.
* `src/components`: reusable UI components.
* `src/contexts`: shared React contexts, including localization context.
* `src/hooks`: reusable React hooks.
* `src/interceptors`: request and response interception for the API client.
* `src/locales`: bundled UI translations.
* `src/navigators`: drawer, stack, and tab navigation definitions.
* `src/screens`: application pages such as login, home, settings, and account flows.
* `src/services`: cross-cutting services such as localization helpers.
* `src/store`: Redux actions, listeners, reducers, and selectors.
* `src/theme`, `src/types`, `src/utils`: shared theming, typings, and helper utilities.

## Running the Application

The React Native app is not started by the ABP Studio solution runner. Run `AuthServer`, `MobileGateway`, and the required backend services first, then start the mobile app with the standard React Native / Expo toolchain.

For Android emulators or devices, map the development ports before testing:

```bash
adb reverse tcp:<mobile-gateway-port> tcp:<mobile-gateway-port>
adb reverse tcp:<auth-server-port> tcp:<auth-server-port>
```

Use the ports assigned to `MobileGateway` and `AuthServer` in your generated solution.
