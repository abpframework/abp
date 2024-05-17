# Title Strategy For Angular

## **ABP has a default title strategy for Angular UI**.

This strategy is based on the title property. Provide a title property when setting a new route.

**Example**

```ts
{
  path:  'customers',
  component: CustomersComponent,
  title: 'AbpCustomers::Roles'
},
```

- It is better to use localized text in the title property. It will be translated by **LocalizationService**.
- The **`title`** property is already set in **ABP internal packages**.

## How it looks

When you create a new route and provide a **`title`** property, it will look like this **`<title> | <projectName>`**

### What is `projectName` and How to Customize

- **`projectName`** is the name of your application. By default, ABP sets a [**`projectName`**](https://github.com/abpframework/abp/blob/f48f78618a326644843c01424b093f0d79448769/templates/app/aspnet-core/src/MyCompanyName.MyProjectName.Domain.Shared/Localization/MyProjectName/en.json#L4) for your application. This localization text is added for customization and can be changed for different languages.

### Disable `projectName`

- If you don't want to show **`projectName`** in the title, you can disable it.

**app.module.ts**

```ts
import { DISABLE_PROJECT_NAME } from  '@abp/ng.core';

providers: [
  ...,
  { provide:  DISABLE_PROJECT_NAME, useValue:  true}
],
```

- Now only title will be shown.

## Override ABP's Default Title Strategy

**app.module.ts**

```ts
import { TitleStrategy } from  '@angular/router';
import { YourCustomStrategy } from  './title-strategy.service.ts';

providers: [
  ...,
  { provide: TitleStrategy, useExisting: YourCustomStrategy },
],
```

- You can check [Angular Documentation](https://angular.io/api/router/TitleStrategy) to write a custom **`TitleStrategy`**.
