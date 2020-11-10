# Blazor UI 3.3 to 4.0 Migration Guide

## Startup Template Changes

* Change `<app>...</app>` to `<div id="ApplicationContainer">...</div>` in the `wwwroot/index.html`.

## AbpCrudPageBase Changes

- `OpenEditModalAsync` method requires `EntityDto` instead of id (`Guid`) parameter.
- `DeleteEntityAsync` method doesn't display confirmation dialog anymore. You can use the new `EntityActions` component in DataGrids to show confirmation messages. You can also inject `IUiMessageService` to your page or component and call `ConfirmAsync` explicitly.
- Added `GetListInput`.

## Others

- Refactored namespaces for some Blazor components ([#6015](https://github.com/abpframework/abp/issues/6015)).
- Remove Async Suffix from IUiMessageService ([#6123](https://github.com/abpframework/abp/pull/6123)).