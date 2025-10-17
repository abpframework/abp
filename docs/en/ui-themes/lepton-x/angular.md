```json
//[doc-seo]
{
    "Description": "Learn how to integrate the LeptonX Angular UI into your project with step-by-step instructions and essential configuration tips."
}
```

# LeptonX Angular UI

To add `LeptonX` into your existing projects, follow the steps below.

- Firstly, install `@volosoft/abp.ng.theme.lepton-x` using the command below.
  `yarn add @volosoft/abp.ng.theme.lepton-x`

* Then, edit `angular.json` as follows:

Add theme-specific styles into the `styles` array of the file. Check the [Theme Configurations](../../framework/ui/angular/theme-configurations.md#lepton-x-commercial) documentation for more information.


- At last, remove `provideThemeLepton` from `app.config.ts`, and add the following providers in `app.config.ts`


```ts
import { provideThemeLeptonX } from '@volosoft/abp.ng.theme.lepton-x';
import { provideSideMenuLayout } from '@volosoft/abp.ng.theme.lepton-x/layouts';
// import { provideThemeLepton } from '@volo/abp.ng.theme.lepton'; 

export const appConfig: ApplicationConfig = {
  providers: [
    // provideThemeLepton() delete this
    provideSideMenuLayout(), // depends on which layout you choose
    provideThemeLeptonX(),
  ],
};
```

If you want to use the **`Top Menu`** instead of the **`Side Menu`**, add `provideTopMenuLayout` as below,and [this style imports](https://docs.abp.io/en/abp/7.4/UI/Angular/Theme-Configurations#lepton-x-commercial)

```ts
import { provideThemeLeptonX } from '@volosoft/abp.ng.theme.lepton-x';
import { provideTopMenuLayout } from '@volosoft/abp.ng.theme.lepton-x/layouts';

export const appConfig: ApplicationConfig = {
  providers: [
    provideTopMenuLayout(),
    provideThemeLeptonX(),
  ],
};
```

- At this point, `LeptonX` theme should be up and running within your application. However, you may need to overwrite some css variables based your needs for every theme available as follows:

```scss
:root {
  .lpx-theme-dark {
    --lpx-logo: url("/assets/images/logo/logo-light.svg");
    --lpx-logo-icon: url("/assets/images/logo/logo-light-icon.svg");
    --lpx-brand: #edae53;
  }

  .lpx-theme-dim {
    --lpx-logo: url("/assets/images/logo/logo-light.svg");
    --lpx-logo-icon: url("/assets/images/logo/logo-light-icon.svg");
    --lpx-brand: #f15835;
  }

  .lpx-theme-light {
    --lpx-logo: url("/assets/images/logo/logo-dark.svg");
    --lpx-logo-icon: url("/assets/images/logo/logo-dark-icon.svg");
    --lpx-brand: #69aada;
  }
}
```

If everything is ok, you can remove the `@volo/abp.ng.theme.lepton` in package.json

## Server Side

In order to migrate to LeptonX on your server side projects (Host and/or IdentityServer projects), please follow [Server Side Migration](https://docs.abp.io/en/commercial/latest/themes/lepton-x/mvc) document.
