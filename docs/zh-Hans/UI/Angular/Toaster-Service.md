# Toast Overlay

你可以通常将@abp/ng.theme.shared包提供的 `ToasterService` 放置在你项目的根级别下以覆盖显示消息.

## 入门

你不必在模块或组件级别提供 `ToasterService`,它已经在**根**级别提供,你可以在你的组件,指令或服务直接注入并使用它.

```js
import { ToasterService } from '@abp/ng.theme.shared';

@Component({
  /* class metadata here */
})
class DemoComponent {
  constructor(private toaster: ToasterService) {}
}
```

## 用法

你可以使用 `ToasterService` 的 `success`, `warn`, `error` 和 `info` 方法显示一个overlay.

### 如何显示一个Toast Overlay

```js
this.toaster.success('Message', 'Title');
```

- `ToasterService` 方法接收三个参数,分别是 `message`, `title`, 和 `options`.
- `success`, `warn`, `error`, 和 `info` 方法返回一个已打开的 toast overlay Id. 可以使用此id删除toast.

### 如何显示具有给定选项的Toast Overlay

选项可以作为第三个参数传递给`success`, `warn`, `error`, 和 `info` 方法:

```js
import { Toaster, ToasterService } from '@abp/ng.theme.shared';
//...

constructor(private toaster: ToasterService) {}

//...
const options: Partial<Toaster.ToastOptions> = {
    life: 10000,
    sticky: false,
    closable: true,
    tapToDismiss: true,
    messageLocalizationParams: ['Demo', '1'],
    titleLocalizationParams: []
  };

  this.toaster.error('AbpUi::EntityNotFoundErrorMessage', 'AbpUi::Error', options);
```

- `life` 选项是关闭的时间毫秒数. 默认值是 `5000`.
- `sticky` 选项为 `true` 时忽略 `life` 选项,将toast overlay留在屏幕上. 默认值是 `false`.
- `closable` 选项为 `true` 时在toast overlay上显示关闭图标. 默认值是 `true`.
- `tapToDismiss` 选项为 `true` 允许通过单击关闭toast overlay. 默认值是 `false`.
- `yesText` 是确定按钮的文本,可以传递本地化键或本地化对象. 默认值是 `AbpUi::Yes`.
- `messageLocalizationParams` 是用于消息本地化的插值参数.
- `titleLocalizationParams` 是标题本地化的插值参数.

使用上面的选项,toast overlay看起来像这样:

![toast](./images/toast.png)

### 如何删除 Toast Overlay

已打开的toast overlay可以通过手动调用 `remove` 方法传递指定的 toast `id`删除.

```js
const toastId = this.toaster.success('Message', 'Title')

this.toaster.remove(toastId);
```

### 如何删除所有的Toasts

可以手动调用 `clear` 方法删除所有的已打开的toasts.

```js
this.toaster.clear();
```

## API

### success

```js
success(
  message: Config.LocalizationParam,
  title: Config.LocalizationParam,
  options?: Partial<Toaster.ToastOptions>,
): number
```

- `Config` 命令空间可以从 `@abp/ng.core` 导入.
- `Toaster` 命令空间可以从 `@abp/ng.theme.shared` 导入.

> 请参见[`Config.LocalizationParam`类型](https://github.com/abpframework/abp/blob/master/npm/ng-packs/packages/core/src/lib/models/config.ts#L46)和[`Toaster` namespace](https://github.com/abpframework/abp/blob/master/npm/ng-packs/packages/theme-shared/src/lib/models/toaster.ts)

### warn

```js
warn(
  message: Config.LocalizationParam,
  title: Config.LocalizationParam,
  options?: Partial<Toaster.ToastOptions>,
): number
```

### error

```js
error(
  message: Config.LocalizationParam,
  title: Config.LocalizationParam,
  options?: Partial<Toaster.ToastOptions>,
): number
```

### info

```js
info(
  message: Config.LocalizationParam,
  title: Config.LocalizationParam,
  options?: Partial<Toaster.ToastOptions>,
): number
```

### remove

```js
remove(id: number): void
```

按给定的id移除打开的toast.

### clear

```js
clear(): void
```

删除所有打开的toasts.

## 另请参阅

- [Confirmation Popup](./Confirmation-Service.md)

## 确认弹层?

- [Config State](./Config-State.md)
