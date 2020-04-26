# Toast Overlay

You can use the `ToasterService` in @abp/ng.theme.shared package to display messages in an overlay by placing at the root level in your project.


## Getting Started

You do not have to provide the `ToasterService` at module or component level, because it is already **provided in root**. You can inject and start using it immediately in your components, directives, or services.


```js
import { ToasterService } from '@abp/ng.theme.shared';

@Component({
  /* class metadata here */
})
class DemoComponent {
  constructor(private toaster: ToasterService) {}
}
```

## Usage

You can use the `success`, `warn`, `error`, and `info` methods of `ToasterService` to display an overlay.

### How to Display a Toast Overlay

```js
this.toast.success('Message', 'Title');
```

- The `ToasterService` methods accept three parameters that are `message`, `title`, and `options`.
- `success`, `warn`, `error`, and `info` methods return the id of opened toast overlay. The toast can be removed with this id.

### How to Display a Toast Overlay With Given Options

Options can be passed as the third parameter to `success`, `warn`, `error`, and `info` methods:

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

- `life` option is the closing time in milliseconds. Default value is `5000`.
- `sticky` option keeps toast overlay on the screen by ignoring the `life` option when `true`. Default value is `false`.
- `closable` option displays the close icon on the toast overlay when it is `true`. Default value is `true`.
- `tapToDismiss` option, when `true`, allows closing the toast overlay by clicking over it. Default value is `false`.
- `yesText` is the text of the confirmation button. A localization key or localization object can be passed. Default value is `AbpUi::Yes`.
- `messageLocalizationParams` is the interpolation parameters for the localization of the message.
- `titleLocalizationParams` is the interpolation parameters for the localization of the title.

With the options above, the toast overlay looks like this:

![toast](./images/toast.png)

### How to Remove a Toast Overlay

The open toast overlay can be removed manually via the `remove` method by passing the `id` of toast:

```js
const toastId = this.toast.success('Message', 'Title')

this.toast.remove(toastId);
```

### How to Remove All Toasts

The all open toasts can be removed manually via the `clear` method:

```js
this.toast.clear();
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

- `Config` namespace can be imported from `@abp/ng.core`.
- `Toaster` namespace can be imported from `@abp/ng.theme.shared`.

> See the [`Config.LocalizationParam` type](https://github.com/abpframework/abp/blob/master/npm/ng-packs/packages/core/src/lib/models/config.ts#L46) and [`Toaster` namespace](https://github.com/abpframework/abp/blob/master/npm/ng-packs/packages/theme-shared/src/lib/models/toaster.ts)


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

Removes an open toast by the given id.

### clear

```js
clear(): void
```

Removes all open toasts.

## See Also

- [Confirmation Popup](./Confirmation-Service.md)

## What's Next?

- [Config State](./Config-State.md)
