# 权限管理

权限是为特定用户,角色或客户端授予或禁止的简单策略. 你可以在[ABP授权文档](../../Authorization.md)中阅读更多信息.

你可以使用 `ConfigState` 的 `getGrantedPolicy` 选择器获取经过身份验证的用户的权限.

你可以从Store中获取权限的布尔值:

```js
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';

export class YourComponent {
  constructor(private store: Store) {}

  ngOnInit(): void {
    const canCreate = this.store.selectSnapshot(ConfigState.getGrantedPolicy('AbpIdentity.Roles.Create'));
  }

  // ...
}
```

或者你可以通过 `ConfigStateService` 获取它:

```js
import { ConfigStateService } from '../services/config-state.service';

export class YourComponent {
  constructor(private configStateService: ConfigStateService) {}

  ngOnInit(): void {
    const canCreate = this.configStateService.getGrantedPolicy('AbpIdentity.Roles.Create');
  }

  // ...
}
```

## 权限指令

你可以使用 `PermissionDirective` 来根据用户的权限控制DOM元素是否可见.

```html
<div *abpPermission="AbpIdentity.Roles">
  仅当用户具有`AbpIdentity.Roles`权限时,此内容才可见.
</div>
```

如上所示,你可以使用 `abpPermission` 结构指令从DOM中删除元素.

该指令也可以用作属性指令,但是我们建议你将其用作结构指令.

## 权限守卫

如果你想要在导航过程中控制经过身份验证的用户对路由的访问权限,可以使用 `PermissionGuard`.

将 `requiredPolicy` 添加到路由模块中的 `routes`属性.

```js
const routes: Routes = [
  {
    path: 'path',
    component: YourComponent,
    canActivate: [PermissionGuard],
    data: {
      routes: {
        requiredPolicy: 'AbpIdentity.Roles.Create',
      },
    },
  },
];
```

授予的策略存储在 `ConfigState` 的 `auth` 属性中.

## 下一步是什么?

* [确认弹层](./Confirmation-Service.md)