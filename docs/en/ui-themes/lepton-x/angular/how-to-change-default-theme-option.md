```json
//[doc-seo]
{
    "Description": "Learn how to easily configure the default theme for your LeptonX application using the ThemeLeptonXModule for a personalized visual experience."
}
```

# Configuring the Default Theme for LeptonX
The LeptonX theme offers multiple appearances to suit your application's visual style. You can easily configure the default theme for your application using the `provideThemeLeptonX` provided by LeptonX.

### Configuration Code
To set the default theme, you need to configure the `provideThemeLeptonX` using the `withThemeLeptonXOptions({...})` function in the main configuration of your application (often referred to as appConfig). Here's an example:

```js
import { provideThemeLeptonX, withThemeLeptonXOptions } from '@volosoft/abp.ng.theme.lepton-x';

export const appConfig: ApplicationConfig = {
  providers: [
    // ...
    provideThemeLeptonX(
      withThemeLeptonXOptions({
        defaultTheme: 'light'
      })
    ),
  ],
};
```

In the example above, we've imported the `provideThemeLeptonX` and `withThemeLeptonXOptions`, then configured it using the option parameters. By providing the `defaultTheme` parameter and setting its value to 'light'.

If you delete the defaultTheme parameter in the configuration object, the LeptonX theme will use the default value of "System" as the default theme appearance.

You can customize the value of the defaultTheme parameter to align with various available theme appearances, including 'dim', 'dark', 'light', or any personally crafted custom themes.
