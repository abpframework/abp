# ABP Version 5.2 Migration Guide

This document is a guide for upgrading ABP 5.1 solutions to ABP 5.2. Please read them all since 5.2 has some important breaking changes.


## Blazor UI
If you use Blazor WASM or Blazor Server UI, you should follow this section.

### Blazorise 1.0
We've upgraded to Blazorise 1.0 stable version. So there is some breaking changes that you have to apply in your project.

Also You can review that pull request [#11649 - Blazorise 1.0 Migration](https://github.com/abpframework/abp/pull/11649)

- `NumericEdit` is now made around the native `input type="number"` so a lot of its formating features are moved to the new `NumericPicker` component. Replace NumericEdit with NumericPicker.
- Rename `DecimalsSeparator` to `DecimalSeparator` on the `DataGridColumn` and `NumericPicker`.
- Rename `MaxMessageSize` to `MaxChunkSize`.
- Remove `Fullscreen` parameter on `<ModalContent>` and replace it with `Size="ModalSize.Fullscreen"` parameter.
- Remove `NotificationType`, `Message`, and `Title` parameter from `<NotificationAlert>` component.
- Move `RightAligned` parameter from `<BarDropdownMenu>` to `<BarDropdown>` component.
- Rename any usage of the `ChangeTextOnKeyPress` parameter into `Immediate`.
- Rename any usage of `DelayTextOnKeyPress` parameter into `Debounce` and `DelayTextOnKeyPressInterval` into DebounceInterval.
- Replace all `Left` and `Right` enums with `Start` and `End` for the following enum types: `Direction`, `Float`, `Placement`, `NotificationLocation`, `Side`, `SnackbarLocation`, `SnackbarStackLocation`, `TabPosition`, and `TextAlignment`.
- Replace all `FromLeft`, `FromRight`, `RoundedLeft`, and `RoundedRight` enums with `FromStart`, `FromEnd`, `RoundedStart`, and `RoundedEnd` for the `Border` utilities.
- Replace all `FromLeft` and `FromRight` with `FromStart`, `FromEnd` for the Margin and `Padding` utilities.
- Replace all `AddLabel` with `AddLabels` method on chart instance.
- Change enum value from `None` to `Default` for the following enum types: `Color`, `Background`, `TextColor`, `Alignment`, `BorderRadius`, `BorderSize`, `Direction`, `DisplayDirection`, `FigureSize`, `IconSize`, `JustifyContent`, `OverflowType`, `SnackbarColor`, `Target`, `TextAlignment`, `TextOverflow`, `TextTransform`, `TextWeight`, `VerticalAlignment`, `Visibility`, `Size`, and `SnackbarLocation`.
- Obsolete typography parameters `Alignment`, `Color`, `Transform`, and `Weight` are removed in favor of `TextAlignment`, `TextColor`, `TextTransform`, and `TextWeight`.
- Remove any use of an obsolete component `<InlineField>`.
- The Datagrid's obsolete `Direction` parameter has now been removed. Instead, please use the `SortDirection` parameter if you weren't already..
- Rename `<Tabs>` `Mode` parameter into `RenderMode`.

> _Check out [Blazorise Release Notes](https://preview.blazorise.com/news/release-notes/100) for more information._

---

## MVC - Razor Pages UI

If you use MVC Razor Pages UI, you should follow this section.

### Client libraries
The `libs` folder no longer exists in templates after v5.2. That change greatly reduced the size of templates and brought some other advantages.

You can use `abp install-libs` command for installing or updating client libraries. You should run this command after updating v5.2.

> If you're creating a new project, you don't have to be concerned about it, ABP CLI installs client libraries after automatically.

