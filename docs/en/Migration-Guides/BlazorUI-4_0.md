# Migration Guide for the Blazor UI from the v3.3 to the v4.0

## AbpCrudPageBase Changes

- `OpenEditModalAsync` method is requires `EntityDto` instead of id (`Guid`) parameter.
- `DeleteEntityAsync` method doesn't display confirmation dialog anymore. You can use the new `EntityActions` component in DataGrids to show
 confirmation messages. You can also inject `IUiMessageService` to your page or component and call `ConfirmAsync` explicitly.
