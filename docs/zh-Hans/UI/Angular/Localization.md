# 本地化

在阅读本地化管道和本地化服务之前你应该了解本地化Key.

本地化key格式由两个部分组成,分别是**资源名**和**Key**
`ResourceName::Key`

> 如果你没有指定资源名称,它默认是在 `environment.ts` 中声明的 `defaultResourceName`.

```js
const environment = {
  //...
  localization: {
    defaultResourceName: 'MyProjectName',
  },
};
```

所以这两个结果是一样的:

```html
<h1>{%{{{ '::Key' | abpLocalization }}}%}</h1>

<h1>{%{{{ 'MyProjectName::Key' | abpLocalization }}}%}</h1>
```

## 使用本地化管道

你可以使用 `abpLocalization` 管道来获取本地化的文本. 例:

```html
<h1>{%{{{ 'Resource::Key' | abpLocalization }}}%}</h1>
```

管道将用本地化的文本替换Key.

你还可以指定一个默认值,如下所示:

```html
<h1>{%{{{ { key: 'Resource::Key', defaultValue: 'Default Value' } | abpLocalization }}}%}</h1>
```

要使用插值,必须将插值作为管道参数给出. 例如:

本地化数据存储在键值对中:

```js
{
  //...
  AbpAccount: { // AbpAccount is the resource name
    Key: "Value",
    PagerInfo: "Showing {0} to {1} of {2} entries"
  }
}
```

所以我们可以这样使用Key:

```html
<h1>{%{{{ 'AbpAccount::PagerInfo' | abpLocalization:'20':'30':'50' }}}%}</h1>

<!-- Output: Showing 20 to 30 of 50 entries -->
```

### 使用本地化服务

首先应该从 **@abp/ng.core** 导入 `LocalizationService`.

```js
import { LocalizationService } from '@abp/ng.core';

class MyClass {
  constructor(private localizationService: LocalizationService) {}
}
```

之后你就可以使用本地化服务.

> 你可以将插值参数作为参数添加到 `instant()` 和 `get()` 方法中.

```js
this.localizationService.instant('AbpIdentity::UserDeletionConfirmation', 'John');

// with fallback value
this.localizationService.instant(
  { key: 'AbpIdentity::UserDeletionConfirmation', defaultValue: 'Default Value' },
  'John',
);

// Output
// User 'John' will be deleted. Do you confirm that?
```

要获取[_Observable_](https://rxjs.dev/guide/observable)的本地化文本,应该使用 `get` 方法而不是 `instant`:

```js
this.localizationService.get('Resource::Key');

// with fallback value
this.localizationService.get({ key: 'Resource::Key', defaultValue: 'Default Value' });
```

### 使用配置状态

要使用 `getLocalization` 方法,你应该导入 `ConfigState`.

```js
import { ConfigState } from '@abp/ng.core';
```

然后你可以按以下方式使用它:

```js
this.store.selectSnapshot(ConfigState.getLocalization('ResourceName::Key'));
```

`getLocalization` 方法可以与 `本地化key` 和  [`LocalizationWithDefault`](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/config.ts#L34) 接口一起使用.

```js
this.store.selectSnapshot(
  ConfigState.getLocalization(
    {
      key: 'AbpIdentity::UserDeletionConfirmation',
      defaultValue: 'Default Value',
    },
    'John',
  ),
);
```

本地化资源存储在 `ConfigState` 的 `localization` 属性中.

## 另请参阅

* [ASP.NET Core中的本地化](../../Localization.md)

## 下一步是什么?

* [权限管理](./Permission-Management.md)