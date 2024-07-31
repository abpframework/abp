# Blazor UI: SubmitButton component

`SubmitButton` is a simple wrapper around `Button` component. It is used to be placed inside of page Form or Modal dialogs where it can response to user actions and to be activated as a default button by pressing an ENTER key. Once clicked it will go into the `disabled` state and also it will show a small loading indicator until clicked event is finished.

## Quick Example

```html
<SubmitButton Clicked="@YourSaveOperation" />
```

Notice that we didn't specify any text, like `Save Changes`. This is because `SubmitButton` will by default pull text from the localization. If you want to change that you either specify a localization key or you can add custom content.

### With localization key

```html
<SubmitButton Clicked="@YourSaveOperation" SaveResourceKey="YourSaveName" />
```

### With custom content

```html
<SubmitButton Clicked="@YourSaveOperation">
    @L["Save"]
</SubmitButton>
```