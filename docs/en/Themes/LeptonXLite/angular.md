# LeptonX Lite Angular UI
LeptonX Lite has implementation for the ABP Framework Angular Client. It's a simplified variation of the [LeptonX Theme](https://x.leptontheme.com/).

>   If you are looking for a professional, enterprise ready theme, you can check the [LeptonX Theme](https://x.leptontheme.com/), which is a part of [ABP Commercial](https://commercial.abp.io/).

> See the [Theming document](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Theming) to learn about themes.

## Installation

To add `LeptonX-lite` into your project,

* Install `@abp/ng.theme.lepton-x`

`yarn add @abp/ng.theme.lepton-x@preview`

* Install `bootstrap-icons`

`yarn add bootstrap-icons`


* Then, we need to edit the styles array in `angular.json` to replace the existing style with the new one.

Add the following style 

```json
"node_modules/bootstrap-icons/font/bootstrap-icons.css",
```

* Finally, remove `ThemeBasicModule` from `app.module.ts`, and import the related modules in `app.module.ts`

```js
import { ThemeLeptonXModule } from '@abp/ng.theme.lepton-x';
import { SideMenuLayoutModule } from '@abp/ng.theme.lepton-x/layouts';

@NgModule({
  imports: [
    // ...

    // do not forget to remove ThemeBasicModule
    //  ThemeBasicModule.forRoot(),
    ThemeLeptonXModule.forRoot(),
    SideMenuLayoutModule.forRoot(),
  ],
  // ...
})
export class AppModule {}
```

Note: If you employ [Resource Owner Password Flow](https://docs.abp.io/en/abp/latest/UI/Angular/Authorization#resource-owner-password-flow) for authorization, you should import the following module as well:

```js
import { AccountLayoutModule } from '@abp/ng.theme.lepton-x/account';

@NgModule({
  // ...
  imports: [
    // ...
    AccountLayoutModule.forRoot(),
    // ...
  ],
  // ...
})
export class AppModule {}
```

To change the logos and brand color of `LeptonX`, simply add the following CSS to the `styles.scss`

```css
:root {
  --lpx-logo: url('/assets/images/logo.png');
  --lpx-logo-icon: url('/assets/images/logo-icon.png');
  --lpx-brand: #edae53;
}
```

- `--lpx-logo` is used to place the logo in the menu.
- `--lpx-logo-icon` is a square icon used when the menu is collapsed. 
- `--lpx-brand` is a color used throughout the application, especially on active elements. 

### Server Side

In order to migrate to LeptonX on your server side projects (Host and/or IdentityServer projects), please follow the [Server Side Migration](mvc.md) document.
