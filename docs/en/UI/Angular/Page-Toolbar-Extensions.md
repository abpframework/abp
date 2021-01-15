# Page Toolbar Extensions for Angular UI

## Introduction

Page toolbar extension system allows you to add a new action to the toolbar of a page. A "Click Me" action was added to the user management page below:

<img alt="Page Toolbar Extension Example: 'Click Me!' Action" src="./images/toolbar-action-extensions---click-me.gif" width="800px" style="max-width:100%">

You can take any action (open a modal, make an HTTP API call, redirect to another page... etc) by writing your custom code. You can also access to page data (the main record, usually an entity list) in your code. Additionally, you can pass in custom components instead of using the default button.

## How to Add an Action to Page Toolbar

In this example, we will add a "Click Me!" action and log `userName` of all users in the user management page of the [Identity Module](../../Modules/Identity.md) to the console.

### Step 1. Create Toolbar Action Contributors

The following code prepares a constant named `identityToolbarActionContributors`, ready to be imported and used in your root module:

```js
// src/app/toolbar-action-contributors.ts

import {
  eIdentityComponents,
  IdentityToolbarActionContributors,
  IdentityUserDto,
} from '@abp/ng.identity';
import { ToolbarAction, ToolbarActionList } from '@abp/ng.theme.shared/extensions';

const logUserNames = new ToolbarAction<IdentityUserDto[]>({
  text: 'Click Me!',
  action: data => {
    // Replace log with your custom code
    data.record.forEach(user => console.log(user.userName));
  },
  // See ToolbarActionOptions in API section for all options
});

export function logUserNamesContributor(actionList: ToolbarActionList<IdentityUserDto[]>) {
  actionList.addHead(logUserNames);
}

export const identityToolbarActionContributors: IdentityToolbarActionContributors = {
  // enum indicates the page to add contributors to
  [eIdentityComponents.Users]: [
    logUserNamesContributor,
    // You can add more contributors here
  ],
};

```

The list of actions, conveniently named as `actionList`, is a **doubly linked list**. That is why we have used the `addHead` method, which adds the given value to the beginning of the list. You may find [all available methods here](../Common/Utils/Linked-List.md).

### Step 2. Import and Use Toolbar Action Contributors

Import `identityToolbarActionContributors` in your routing module and pass it to the static `forLazy` method of `IdentityModule` as seen below:

```js
// src/app/app-routing.module.ts

// other imports
import { identityToolbarActionContributors } from './toolbar-action-contributors';

const routes: Routes = [
  // other routes

  {
    path: 'identity',
    loadChildren: () =>
      import('@abp/ng.identity').then(m =>
        m.IdentityModule.forLazy({
          toolbarActionContributors: identityToolbarActionContributors,
        })
      ),
  },

  // other routes
];
```

That is it, `logUserNames` toolbar action will be added as the first action on the page toolbar in the users page (`UsersComponent`) of the `IdentityModule`.

## How to Add a Custom Component to Page Toolbar

In this example, we will add a custom "Click Me!" button and log `userName` of all users in the user management page of the [Identity Module](../../Modules/Identity.md) to the console.

<img alt="Page Toolbar Extension Example: Custom Component" src="./images/toolbar-action-extensions---custom-component.gif" width="800px" style="max-width:100%">

### Step 1. Create A Custom Component

We need to have a component before we can pass it to the toolbar action contributors:

```js
// src/app/click-me-button.component.ts

import { IdentityUserDto } from '@abp/ng.identity';
import { ActionData, EXTENSIONS_ACTION_DATA } from '@abp/ng.theme.shared/extensions';
import { Component, Inject } from '@angular/core';

@Component({
  selector: 'app-click-me-button',
  template: `<button class="btn btn-warning" (click)="handleClick()">Click Me!</button>`,
})
export class ClickMeButtonComponent {
  constructor(
    @Inject(EXTENSIONS_ACTION_DATA)
    private data: ActionData<IdentityUserDto[]>
  ) {}

  handleClick() {
    this.data.record.forEach(user => console.log(user.userName));
  }
}

```

Here, `EXTENSIONS_ACTION_DATA` token provides us the context from the page toolbar. Therefore, we are able to reach the page data via `record`, which is an array of users, i.e. `IdentityUserDto[]`.

> We could also import `EXTENSIONS_ACTION_CALLBACK` from **@abp/ng.theme.shared/extensions** package, which is a higher order function that triggers the predefined `action` when called. It passes `ActionData` as the first parameter, so you do not have to pass it explicitly. In other words, `EXTENSIONS_ACTION_CALLBACK` can be called without any parameters and it will not fail.

### Step 2. Create Toolbar Action Contributors

The following code prepares a constant named `identityToolbarActionContributors`, ready to be imported and used in your root module. When `ToolbarComponent` is used instead of `ToolbarAction`, we can pass a component in:

```js
// src/app/toolbar-action-contributors.ts

import {
  eIdentityComponents,
  IdentityToolbarActionContributors,
  IdentityUserDto,
} from '@abp/ng.identity';
import { ToolbarActionList, ToolbarComponent } from '@abp/ng.theme.shared/extensions';
import { ClickMeButtonComponent } from './click-me-button.component';

const logUserNames = new ToolbarComponent<IdentityUserDto[]>({
  component: ClickMeButtonComponent,
  // See ToolbarActionOptions in API section for all options
});

export function logUserNamesContributor(actionList: ToolbarActionList<IdentityUserDto[]>) {
  actionList.addHead(logUserNames);
}

export const identityToolbarActionContributors: IdentityToolbarActionContributors = {
  // enum indicates the page to add contributors to
  [eIdentityComponents.Users]: [
    logUserNamesContributor,
    // You can add more contributors here
  ],
};

```

The list of actions, conveniently named as `actionList`, is a **doubly linked list**. That is why we have used the `addHead` method, which adds the given value to the beginning of the list. You may find [all available methods here](../Common/Utils/Linked-List.md).

### Step 3. Import and Use Toolbar Action Contributors

Import `identityToolbarActionContributors` in your routing module and pass it to the static `forLazy` method of `IdentityModule` as seen below.

```js
// src/app/app-routing.module.ts

// other imports
import { identityToolbarActionContributors } from './toolbar-action-contributors';

const routes: Routes = [
  // other routes

  {
    path: 'identity',
    loadChildren: () =>
      import('@abp/ng.identity').then(m =>
        m.IdentityModule.forLazy({
          toolbarActionContributors: identityToolbarActionContributors,
        })
      ),
  },

  // other routes
];
```

That is it, `logUserNames` toolbar action will be added as the first action on the page toolbar in the users page (`UsersComponent`) of the `IdentityModule` and it will be triggered by a custom button, i.e. `ClickMeButtonComponent`. Please note that **component projection is not limited to buttons** and you may use other UI components.

## How to Place a Custom Modal and Trigger It by Toolbar Actions

Please check the same topic in [entity action extensions document](Entity-Action-Extensions.md) and replace entity action with a toolbar action.

## API

### ActionData\<R = any\>

`ActionData` is the shape of the parameter passed to all callbacks or predicates in a `ToolbarAction`.

It has the following properties:

- **record** is the page data, the main record on a page, usually an entity list (e.g. list of users).

  ```js
  {
    text: 'Click Me!',
    action: data => {
      data.record.forEach(user => {
        console.lof(user.userName);
      });
    },
  }
  ```

- **getInjected** is the equivalent of [Injector.get](https://angular.io/api/core/Injector#get). You can use it to reach injected dependencies of `PageToolbarComponent`, including, but not limited to, its parent component.

  ```js
  {
    text: 'Click Me!',
    action: data => {
      const restService = data.getInjected(RestService);

      // Use restService public props and methods here
    },
    visible: data => {
      const usersComponent = data.getInjected(UsersComponent);

      // Use usersComponent public props and methods here
    },
  }
  ```

### ActionCallback\<T, R = any\>

`ActionCallback` is the type of the callback function that can be passed to a `ToolbarAction` as `action` parameter. An action callback gets a single parameter, the `ActionData`. The return type may be anything, including `void`. Here is a simplified representation:

```js
type ActionCallback<T, R = any> = (data?: ActionData<T>) => R;
```

### ActionPredicate\<T\>

`ActionPredicate` is the type of the predicate function that can be passed to a `ToolbarAction` as `visible` parameter. An action predicate gets a single parameter, the `ActionData`. The return type must be `boolean`. Here is a simplified representation:

```js
type ActionPredicate<T> = (data?: ActionData<T>) => boolean;
```

### ToolbarActionOptions\<R = any\>

`ToolbarActionOptions` is the type that defines required and optional properties you have to pass in order to create an toolbar action.

Its type definition is as follows:

```js
type ToolbarActionOptions<R = any> = {
  action: ActionCallback<R>,
  text: string,
  icon?: string,
  permission?: string,
  visible?: ActionPredicate<R>,
};
```

As you see, passing `action` and `text` is enough to create an toolbar action. Here is what each property is good for:

- **action** is a callback that is called when the toolbar action is clicked. (_required_)
- **text** is the button text which will be localized. (_required_)
- **icon** is the classes that define an icon to be placed before the text. (_default:_ `''`)
- **permission** is the permission context which will be used to decide if this toolbar action should be displayed to the user or not. (_default:_ `undefined`)
- **visible** is a predicate that will be used to decide if the page toolbar should have this action or not. (_default:_ `() => true`)

You may find a full example below.

### ToolbarAction\<R = any\>

`ToolbarAction` is the class that defines your toolbar actions. It takes an `ToolbarActionOptions` and sets the default values to the properties, creating an toolbar action that can be passed to an toolbar contributor.

```js
const options: ToolbarActionOptions<IdentityUserDto[]> = {
  action: data => {
    const service = data.getInjected(MyCustomIdentityService);
    const lockedUsers = data.record.filter(user => user.isLockedOut);
    service.unlockAll(lockedUsers);
  },
  text: 'MyProjectName::UnlockAll',
  icon: 'fa fa-unlock',
  permission: 'AbpIdentity.Users.Update',
  visible: data => data.record.some(user => user.isLockedOut),
};

const action = new ToolbarAction(options);
```

It also has two static methods to create its instances:

- **ToolbarAction.create\<R = any\>\(options: ToolbarActionOptions\<R\>\)** is used to create an instance of `ToolbarAction`.
  ```js
  const action = ToolbarAction.create(options);
  ```
- **ToolbarAction.createMany\<R = any\>\(options: ToolbarActionOptions\<R\>\[\]\)** is used to create multiple instances of `ToolbarAction` with given array of `ToolbarActionOptions`.

### ToolbarComponentOptions\<R = any\>

`ToolbarComponentOptions` is the type that defines required and optional properties you have to pass in order to create an toolbar component.

Its type definition is as follows:

```js
type ToolbarComponentOptions<R = any> = {
  component: Type<any>,
  action?: ActionCallback<R>,
  permission?: string,
  visible?: ActionPredicate<R>,
};
```

As you see, passing `action` and `text` is enough to create an toolbar action. Here is what each property is good for:

- **component** is the constructor of the component to be projected. (_required_)
- **action** is a predefined callback that you can reach in your component via `EXTENSIONS_ACTION_CALLBACK` token and trigger. (_optional_)
- **permission** is the permission context which will be used to decide if this toolbar action should be displayed to the user or not. (_default:_ `undefined`)
- **visible** is a predicate that will be used to decide if the page toolbar should have this action or not. (_default:_ `() => true`)

You may find a full example below.

### ToolbarComponent\<R = any\>

`ToolbarComponent` is the class that defines toolbar actions which project a custom component. It takes an `ToolbarComponentOptions` and sets the default values to the properties, creating a toolbar action that can be passed to an toolbar contributor.

```js
const options: ToolbarComponentOptions<IdentityUserDto[]> = {
  component: UnlockAllButton,
  action: data => {
    const service = data.getInjected(MyCustomIdentityService);
    const lockedUsers = data.record.filter(user => user.isLockedOut);
    service.unlockAll(lockedUsers);
  },
  permission: 'AbpIdentity.Users.Update',
  visible: data => data.record.some(user => user.isLockedOut),
};

const action = new ToolbarComponent(options);
```

It also has two static methods to create its instances:

- **ToolbarComponent.create\<R = any\>\(options: ToolbarComponentOptions\<R\>\)** is used to create an instance of `ToolbarComponent`.
  ```js
  const action = ToolbarComponent.create(options);
  ```
- **ToolbarComponent.createMany\<R = any\>\(options: ToolbarComponentOptions\<R\>\[\]\)** is used to create multiple instances of `ToolbarComponent` with given array of `ToolbarComponentOptions`.
  ```js
  const actions = ToolbarComponent.createMany(optionsArray);
  ```

### ToolbarActionList\<R = any\>

`ToolbarActionList` is the list of actions passed to every action contributor callback as the first parameter named `actionList`. It is a **doubly linked list**. You may find [all available methods here](../Common/Utils/Linked-List.md).

The items in the list will be displayed according to the linked list order, i.e. from head to tail. If you want to re-order them, all you have to do is something like this:

```js
export function reorderUserContributors(
  actionList: ToolbarActionList<IdentityUserDto[]>,
) {
  // drop "New User" button
  const newUserActionNode = actionList.dropByValue(
    'AbpIdentity::NewUser',
    (action, text) => action['text'] === text,
  );

  // add it back to the head of the list
  actionList.addHead(newUserActionNode.value);
}

export const identityEntityActionContributors = {
  [eIdentityComponents.Users]: [
    logUserNamesContributor,
    reorderUserContributors,
  ],
};
```

### ToolbarActionContributorCallback\<R = any\>

`ToolbarActionContributorCallback` is the type that you can pass as toolbar action contributor callbacks to static `forLazy` methods of the modules.

```js
// exportUsersContributor should have ToolbarActionContributorCallback<IdentityUserDto[]> type

export function exportUsersContributor(
  actionList: ToolbarActionList<IdentityUserDto[]>,
) {
  // add exportUsers just before the last action
  actionList.add(exportUsers).byIndex(-1);
}

export const identityEntityActionContributors = {
  [eIdentityComponents.Users]: [exportUsersContributor],
};
```

## See Also

- [Customizing Application Modules Guide](../../Customizing-Application-Modules-Guide.md)
