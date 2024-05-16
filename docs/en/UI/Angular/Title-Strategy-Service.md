# Title Strategy For Angular

## **ABP has a default title strategy for Angular UI**.

This strategy is based on the title property. Provide a title property when setting a new route.
**Example:**

```ts
{
  path:  'customers',
  component: CustomersComponent,
  title: 'AbpCustomers::Roles'
},
```

**`Note:`**

- It is better to use localized text in title property. It will be translated by **LocalizationService**.
- **`title`** property is already setted in **ABP internal packages**.

## How it looks

You create a new route and provide a **`title`** property, it will look like this **`<title> | <projectName>`**

### What is `projectName` and How to Customize

- **`projectName`** is name of your application. By default ABP set's a [**`projectName`**](https://github.com/abpframework/abp/blob/f48f78618a326644843c01424b093f0d79448769/templates/app/aspnet-core/src/MyCompanyName.MyProjectName.Domain.Shared/Localization/MyProjectName/en.json#L4) for your application. This localization text is added for customization, it can be changed for different languages.

### Disable `projectName`

- If you dont want to show **`projectName`** in title, you can disable it.

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

- You can check [Angular Documentation](https://angular.io/api/router/TitleStrategy) to write custom **`TitleStrategy`**.
