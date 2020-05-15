## 配置状态

`ConfigStateService` 是一个单例服务,即在应用程序的根级别提供,用于与 `Store` 中的应用程序配置状态进行交互.

## 使用前

为了使用 `ConfigStateService`,你必须将其注入到你的类中.

```js
import { ConfigStateService } from '@abp/ng.core';

@Component({
  /* class metadata here */
})
class DemoComponent {
  constructor(private config: ConfigStateService) {}
}
```

你不必在模块或组件/指令级别提供 `ConfigStateService`,因为它已经在**根中**提供.

## 选择器方法

`ConfigStateService` 有许多选择器方法允许你从 `Store` 获取特定或所有的配置.

### 如何从Store获取所有的配置

你可以使用 `ConfigStateService` 的 `getAll` 方法从Store获取所有的配置对象. 用法如下:

```js
// this.config is instance of ConfigStateService

const config = this.config.getAll();
```

### 如何从Store获取特定的配置

你可以使用 `ConfigStateService` 的 `getOne` 方法从Store获取特定的配置属性. 你需要将属性名做为参数传递给方法:

```js
// this.config is instance of ConfigStateService

const currentUser = this.config.getOne("currentUser");
```

有时你想要获取具体信息,而不是当前用户. 例如你只想获取到 `tenantId`:

```js
const tenantId = this.config.getDeep("currentUser.tenantId");
```

或通过提供键数组作为参数:

```js
const tenantId = this.config.getDeep(["currentUser", "tenantId"]);
```

`getDeep` 可以执行 `getOne` 的所有操作. 但 `getOne` 的执行效率要高一些.

#### 配置状态属性

请参阅 `Config.State` 类型,你可以通过 `getOne` 和 `getDeep` 获取所有属性. 你可以在[config.ts 文件](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/config.ts#L7)中找到.

### 如何从Store获取应用程序信息

`getApplicationInfo` 方法从存储为配置状态存储的环境变量中获取应用程序信息. 你可以这样使用它:

```js
// this.config is instance of ConfigStateService

const appInfo = this.config.getApplicationInfo();
```

该方法不会返回 `undefined` 或 `null`,而是会返回一个空对象(`{}`). 换句话说,当你使用上面代码中的 `appInfo` 属性时,永远不会出现错误.

#### 应用程序信息属性

请参阅 `Config.State` 类型,你可以通过 `getApplicationInfo` 获取所有属性. 你可以在[config.ts 文件](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/config.ts#L21)中找到.

### 如何从Store获取

`getApplicationInfo` 方法从存储为配置状态存储的环境变量中获取特定的API URL. 你可以这样使用它:

```js
// this.config is instance of ConfigStateService

const apiUrl = this.config.getApiUrl();
// environment.apis.default.url

const searchUrl = this.config.getApiUrl("search");
// environment.apis.search.url
```

该方法返回给定键的特定的API `url`. 如果没有Key,则使用 `default`.

### 如何从Store获取所有的设置

你可以使用 `ConfigStateService` 的 `getSettings` 获取配置状态所有的设置对象. 你可以这样使用它:

```js
// this.config is instance of ConfigStateService

const settings = this.config.getSettings();
```

实际上该方法可以通过**传递关键字**来搜索设置.

```js
const localizationSettings = this.config.getSettings("Localization");
/*
{
	'Abp.Localization.DefaultLanguage': 'en'
}
*/
```

请注意, **设置搜索区分大小写**.

### 如何从Store获取特定的设置

你可以使用 `ConfigStateService` 的 `getSetting` 获取配置状态特定的设置. 你可以这样使用它:

```js
// this.config is instance of ConfigStateService

const defaultLang = this.config.getSetting("Abp.Localization.DefaultLanguage");
// 'en'
```

### 如何从Store获取特定的权限

你可以使用 `ConfigStateService` 的 `getGrantedPolicy` 获取配置状态特定的权限. 你应该将策略key做为参数传递给方法:

```js
// this.config is instance of ConfigStateService

const hasIdentityPermission = this.config.getGrantedPolicy("Abp.Identity");
// true
```

你还可以使用 **组合策略key** 来微调你的选择:

```js
// this.config is instance of ConfigStateService

const hasIdentityAndAccountPermission = this.config.getGrantedPolicy(
  "Abp.Identity && Abp.Account"
);
// false

const hasIdentityOrAccountPermission = this.config.getGrantedPolicy(
  "Abp.Identity || Abp.Account"
);
// true
```

创建权限选择器时,请考虑以下**规则**:

- 最多可组合两个键.
- `&&` 操作符查找两个键.
- `||` 操作符查找任意一个键.
- 空字符串 `''` 做为键将返回 `true`
- 使用没有第二个键的操作符将返回 `false`

### 如何从Store中获取翻译

`ConfigStateService` 的 `getLocalization` 方法用于翻译. 这里有一些示例:

```js
// this.config is instance of ConfigStateService

const identity = this.config.getLocalization("AbpIdentity::Identity");
// 'identity'

const notFound = this.config.getLocalization("AbpIdentity::IDENTITY");
// 'AbpIdentity::IDENTITY'

const defaultValue = this.config.getLocalization({
  key: "AbpIdentity::IDENTITY",
  defaultValue: "IDENTITY"
});
// 'IDENTITY'
```

请参阅[本地化文档](./Localization.md)了解详情.

## 分发方法

`ConfigStateService` 有几种分发方法,让你方便地将预定义操作分发到 `Store`.

### 如何从服务器获取应用程序配置

`dispatchGetAppConfiguration` 触发对端点的请求,该端点使用应用程序状态进行响应,然后将此响应作为配置状态放置到 `Store`中.

```js
// this.config is instance of ConfigStateService

this.config.dispatchGetAppConfiguration();
// returns a state stream which emits after dispatch action is complete
```

请注意,**你不必在应用程序启动时调用此方法**,因为在启动时已经从服务器收到了应用程序配置.

### 如何修补路由配置

`dispatchPatchRouteByName` 根据名称查找路由, 并将其在 `Store` 中的配置替换为作为第二个参数传递的新配置.

```js
// this.config is instance of ConfigStateService

const newRouteConfig: Partial<ABP.Route> = {
  name: "Home",
  path: "home",
  children: [
    {
      name: "Dashboard",
      path: "dashboard"
    }
  ]
};

this.config.dispatchPatchRouteByName("::Menu:Home", newRouteConfig);
// returns a state stream which emits after dispatch action is complete
```

### 如何添加新路由配置
 
`dispatchAddRoute` 向 `Store` 的配置状态添加一个新路由. 应该将路由配置做为方法参数传递.

```js
// this.config is instance of ConfigStateService

const newRoute: ABP.Route = {
  name: "My New Page",
  iconClass: "fa fa-dashboard",
  path: "page",
  invisible: false,
  order: 2,
  requiredPolicy: "MyProjectName.MyNewPage"
};

this.config.dispatchAddRoute(newRoute);
// returns a state stream which emits after dispatch action is complete
```

`newRoute` 将被放置在根级别,没有任何父路由,并且其url将存储为 `'/path'`.

如果你想要**添加一个子路由,你可以这样做:**

```js
import { eIdentityRouteNames } from '@abp/ng.identity';
// this.config is instance of ConfigStateService

const newRoute: ABP.Route = {
  parentName: eIdentityRouteNames.IdentityManagement,
  name: "My New Page",
  iconClass: "fa fa-dashboard",
  path: "page",
  invisible: false,
  order: 2,
  requiredPolicy: "MyProjectName.MyNewPage"
};

this.config.dispatchAddRoute(newRoute);
// returns a state stream which emits after dispatch action is complete
```

`newRoute` 做为 `'AbpAccount::Login'` 父路由的子路由被放置,它的url被设置为 `'/account/login/page'`.

#### 路由配置属性

请参阅 `ABP.Route` 类型,获取可在参数中传递给 `dispatchSetEnvironment` 的所有属性. 你可以在[common.ts 文件](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/common.ts#L27)中找到.

### 如何设置环境

`dispatchSetEnvironment` 将传递给它的环境变量放在 `Store` 中的配置状态下. 使用方法如下:

```js
// this.config is instance of ConfigStateService

this.config.dispatchSetEnvironment({
  /* environment properties here */
});
// returns a state stream which emits after dispatch action is complete
```

注意,**你不必在应用程序启动时调用此方法**,因为环境变量已经在启动时存储了.

#### 环境属性

请参阅 `Config.Environment` 类型,获取可在参数中传递给 `dispatchSetEnvironment` 的所有属性. 你可以在[config.ts 文件](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/config.ts#L13)中找到.

## 下一步是什么?

- [修改菜单](./Modifying-the-Menu.md)