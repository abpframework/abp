```json
//[doc-seo]
{
    "Description": "This migration guide provides a comprehensive overview of the necessary code changes when upgrading your ABP solution from Blazorise 1.x to 2.0, ensuring a smooth transition to the latest version."
}
```

# ABP Blazorise 1.x to 2.0 Migration Guide

This document summarizes the required code changes when upgrading ABP solutions from Blazorise 1.x to 2.0.

## 1. Package upgrades

Upgrade Blazorise-related packages to `2.0.0`.

- `Blazorise`
- `Blazorise.Components`
- `Blazorise.DataGrid`
- `Blazorise.Snackbar`
- `Blazorise.Bootstrap5`
- `Blazorise.Icons.FontAwesome`

## 2. Input component renames

Blazorise 2.0 uses new input component names:

- `TextEdit` -> `TextInput`
- `MemoEdit` -> `MemoInput`
- `DateEdit` -> `DateInput`
- `TimeEdit` -> `TimeInput`
- `NumericEdit` -> `NumericInput`
- `ColorEdit` -> `ColorInput`
- `FileEdit` -> `FileInput`

## 3. Binding API normalization to Value/ValueChanged

Migrate old binding/value APIs to the new `Value` model.

- `@bind-Text` -> `@bind-Value`
- `Text` / `TextChanged` -> `Value` / `ValueChanged`
- `@bind-Checked` -> `@bind-Value`
- `Checked` / `CheckedChanged` -> `Value` / `ValueChanged`
- `CheckedValue` / `CheckedValueChanged` -> `Value` / `ValueChanged`
- `@bind-Date` / `@bind-Time` -> `@bind-Value`
- `Date` / `DateChanged` -> `Value` / `ValueChanged`
- `Time` / `TimeChanged` -> `Value` / `ValueChanged`
- `@bind-SelectedValue` (for `Select`) -> `@bind-Value`
- `SelectedValue` / `SelectedValueChanged` (for `Select`) -> `Value` / `ValueChanged`
- `@bind-Checked` (for `Switch`) -> `@bind-Value`
- `Checked` / `CheckedChanged` (for `Switch`) -> `Value` / `ValueChanged`

## 4. DatePicker and Select multiple changes

### DatePicker range mode

For `SelectionMode="DateInputSelectionMode.Range"`, the old `Dates` / `DatesChanged` parameters are replaced by the unified `Value` / `ValueChanged`. Use an array or `IReadOnlyList<T>` as `TValue`:

- `@bind-Dates` -> `@bind-Value` (with `TValue="DateTime[]"` or `TValue="IReadOnlyList<DateTime>"`)
- `Dates` / `DatesChanged` -> `Value` / `ValueChanged`

### DatePicker single value mode

For non-range `DatePicker` usage:

- `Date` / `DateChanged` -> `Value` / `ValueChanged`

### Select multiple mode

For `<Select Multiple ...>`, the old `SelectedValues` / `SelectedValuesChanged` parameters are replaced by the unified `Value` / `ValueChanged`. Use an array or `IReadOnlyList<T>` as `TValue`:

- `@bind-SelectedValues` -> `@bind-Value` (with `TValue="string[]"` or `TValue="IReadOnlyList<string>"`)
- `SelectedValues` / `SelectedValuesChanged` -> `Value` / `ValueChanged`

Example:

```razor
<Select TValue="int[]" @bind-Value="Selected" Multiple>
    <SelectItem Value="1">One</SelectItem>
    <SelectItem Value="2">Two</SelectItem>
</Select>

@code {
    private int[] Selected { get; set; } = new int[] { 1 };
}
```

### Empty SelectItem type requirement

For empty placeholder items, set explicit `TValue`:

- `<SelectItem></SelectItem>` -> `<SelectItem TValue="string"></SelectItem>` (or another correct type such as `Guid?`)

## 5. DataGrid migration

### 5.1 Page parameter rename

- `CurrentPage` -> `Page` on `DataGrid`

Important: `AbpExtensibleDataGrid` still uses `CurrentPage` (for example ABP v10.2). Do not rename it to `Page`.

### 5.2 DisplayTemplate context type change

Inside `DisplayTemplate`, use `context.Item` instead of directly using `context`.

Typical updates:

- `context.Property` -> `context.Item.Property`
- `Method(context)` -> `Method(context.Item)`
- `() => Method(context)` -> `() => Method(context.Item)`
- For custom template variable names, same rule applies: `row.Property` -> `row.Item.Property`

The same rule also applies to action handlers in `DataGridEntityActionsColumn` and `DataGridCommandColumn` (such as `Clicked`, `Visible`, and `ConfirmationMessage`):

- `Clicked="async () => await action.Clicked(context)"` -> `Clicked="async () => await action.Clicked(context.Item)"`
- `Visible="action.Visible(context)"` -> `Visible="action.Visible(context.Item)"`

Important: This change applies to DataGrid template contexts only (`DisplayTemplate` in `DataGridColumn`, `DataGridEntityActionsColumn`, etc.). In non-DataGrid templates (for example `TreeView` `NodeContent`), `context` is already the item and should remain unchanged (for example `DeleteMenuItemAsync(context)`).

### 5.3 Width type change (string -> Fluent sizing)

DataGrid column `Width` moved from plain string to fluent sizing APIs:

- `Width="30px"` -> `Width="Width.Px(30)"`
- `Width="60px"` -> `Width="Width.Px(60)"`
- `Width="0.5rem"` -> `Width="Width.Px(8)"` (or another equivalent pixel value)
- `Width="50%"` -> `Width="Width.Percent(50)"` or `Width="Width.Is50"`
- `Width="100%"` -> `Width="Width.Is100"`

For dynamic string widths (for example `column.Width`), ABP introduces `BlazoriseFluentSizingParse.Parse(...)` to convert string values into `IFluentSizingStyle`.

```csharp
Width="@BlazoriseFluentSizingParse.Parse(column.Width)" // column.Width is a string
```

## 6. Modal parameter placement changes

`Size` and `Centered` should be placed on `<Modal>`, not on `<ModalContent>`.

- `<ModalContent Size="..." Centered="true">` -> `<Modal Size="..." Centered="true">` + `<ModalContent>`

## 7. Other component parameter changes

- `Dropdown RightAligned="true"` -> `Dropdown EndAligned="true"`
- `Autocomplete MinLength` -> `MinSearchLength`

## 8. Notes from ABP migration implementation

- Keep component-specific behavior in mind. Not every component follows exactly the same rename pattern.
- `Autocomplete` usage can still involve `SelectedValue` / `SelectedValueChanged`, depending on component API.
- `BarDropdown` and `Dropdown` are different components; align parameter names according to the actual component type.

# Reference

This document may not cover all Blazorise 2.0 changes. For completeness, refer to the official migration guide and release notes:

- [Blazorise 2.0 - Release Notes](https://blazorise.com/news/release-notes/200)
- [Blazorise 2.0 - Migration Guide](https://blazorise.com/news/migration/200)
