# Configuring the Default Theme for LeptonX
The LeptonX theme offers multiple appearances to suit your application's visual style. You can easily configure the default theme for your application using the ThemeLeptonXModule provided by LeptonX.

### Configuration Code
To set the default theme, you need to configure the ThemeLeptonXModule using the forRoot() function in your application's main module (often referred to as AppModule). Here's an example:
```js
import { ThemeLeptonXModule } from 'leptonx'; // Import the LeptonX theme module

@NgModule({
  // ... Other module configurations
  imports: [
    // ... Other imported modules
    ThemeLeptonXModule.forRoot({
      defaultTheme: 'light', // Set the default theme to 'light'
    }),
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
```
 

In the example above, we've imported the ThemeLeptonXModule and configured it using the forRoot() function. By providing the defaultTheme parameter and setting its value to 'light',

If you delete the defaultTheme parameter in the configuration object, the LeptonX theme will use the default value of "System" as the default theme appearance.

You can customize the value of the defaultTheme parameter to align with various available theme appearances, including 'dim', 'dark', 'light', or any personally crafted custom themes.
