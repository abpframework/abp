## 替换组件

你可以将一些ABP的组件替换为你自己的自定义组件.

你可以**替换**但**不能自定义**默认ABP组件的原因是禁用或更改该组件的一部分可能会导致问题. 所以我们把这些组件称为可替换组件.

### 如何替换组件

创建一个你想要使用的新组件,添加到 `AppModule` 中的 `declarations` 和`entryComponents` 中.

然后打开 `app.component.ts` 使用 `AddReplaceableComponent` 将你的组件替换ABP组件. 如下所示:

```js
import { ..., AddReplaceableComponent } from '@abp/ng.core'; // imported AddReplaceableComponent action
import { eIdentityComponents } from '@abp/ng.identity'; // imported eIdentityComponents enum
import { Store } from '@ngxs/store'; // imported Store
//...
export class AppComponent {
  constructor(..., private store: Store) {} // injected Store

  ngOnInit() {
    this.store.dispatch(
      new AddReplaceableComponent({
        component: YourNewRoleComponent,
        key: eIdentityComponents.Roles,
      }),
    );
    //...
  }
}
```

![Example Usage](./images/component-replacement.gif)

### 如何替换布局

每个ABP主题模块有3个布局,分别是`ApplicationLayoutComponent`, `AccountLayoutComponent`, `EmptyLayoutComponent`. 这些布局可以用相同的方式替换.

> 一个布局组件模板应该包含 `<router-outlet></router-outlet>` 元素.

下面的例子解释了如何更换 `ApplicationLayoutComponent`:

运行以下命令在 `angular` 文件夹中生成布局:

```bash
yarn ng generate component shared/my-application-layout --export --entryComponent

# You don't need the --entryComponent option in Angular 9
```

在你的布局模板(`my-layout.component.html`)中添加以下代码:

```html
<router-outlet></router-outlet>
```

打开 `app.component.ts` 添加以下内容:

```js
import { ..., AddReplaceableComponent } from '@abp/ng.core'; // imported AddReplaceableComponent
import { eThemeBasicComponents } from '@abp/ng.theme.basic'; // imported eThemeBasicComponents enum for component keys
import { MyApplicationLayoutComponent } from './shared/my-application-layout/my-application-layout.component'; // imported MyApplicationLayoutComponent
import { Store } from '@ngxs/store'; // imported Store
//...
export class AppComponent {
  constructor(..., private store: Store) {} // injected Store

  ngOnInit() {
    // added below content
    this.store.dispatch(
      new AddReplaceableComponent({
        component: MyApplicationLayoutComponent,
        key: eThemeBasicComponents.ApplicationLayout,
      }),
    );

    //...
  }
}
```

## 下一步是什么?

- [自定义设置页面](./Custom-Setting-Page.md)
