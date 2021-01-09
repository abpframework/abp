# Blazor UI: Page Progress

Page Progress is used to show a progress bar indicator on top of the page and to show to the user that currently a long running process is in the work.

By default you don't need to do anything to show the progress indicator, as all the work is done automatically by the ABP Framework internals. This means that all calls to the ABP backend (through your HTTP API) will activate page progress and show the loading indicator.

This doesn't mean that you don't have the control over it. On the contrary, if you want to show progress for your own processes, it is really easy to do. All you have to do is to use inject and use the `IUiPageProgressService`.

## Example

First, inject the `IUiPageProgressService` into your page/component.

```cs
@inject IUiPageProgressService pageProgressService
```

Next, invoke the `Go` method in `IUiPageProgressService`. It's that simple:

```cs
Task OnClick()
{
    return pageProgressService.Go(null);
}
```

The previous example will show the progress with a default settings. If, for example you want to change the progress color you can override it by setting the options through the `Go` method.

```cs
Task OnClick()
{
  return pageProgressService.Go(null, options =>
  {
      options.Type = UiPageProgressType.Warning;
  });
}
```

## Breakdown

The first parameter of the `Go` needs a little explanation. In the previous example we have set it to `null` which means, once called it will show an _indeterminate_ indicator and will cycle the loading animation indefinitely, until we hide the progress. You also have the option of defining the actual percentage of the progress and the code is the same, just instead of sending it the `null` you will send it a number between `0` and `100`.

```cs
pageProgressService.Go(25)
```

### Valid values

1. `null` - show _indeterminate_ indicator
2. `>= 0` and `<= 100` - show the regular _percentage_ progress

### Hiding progress

To hide the progress just set the actual values to something other then the _Valid value_.

```cs
pageProgressService.Go(-1)
```