```json
//[doc-seo]
{
    "Description": "Define ABP Low-Code page groups to organize runtime pages into dynamic menu folders with ordering, icons, and nesting."
}
```

# Page Groups

Page groups are dynamic menu folders used by low-code pages. They let you organize runtime pages into nested navigation structures without hardcoding menu items in the frontend.

## What a Page Group Stores

A page group descriptor stores:

| Field | Description |
|-------|-------------|
| `name` | Stable identifier used by pages in their `group` field |
| `title` | Display label shown in the runtime menu |
| `icon` | CSS class string for the menu icon |
| `order` | Sort order among sibling groups |
| `parent` | Optional parent group name for nesting |

If a group does not define an icon, the runtime menu uses a folder-style default icon.

## How Pages Use Groups

Pages reference a group by name:

```json
{
  "name": "products",
  "title": "Products",
  "type": "dataGrid",
  "entityName": "Acme.Catalog.Product",
  "group": "inventory"
}
```

If `group` is omitted, the page becomes a top-level runtime menu item.

A group is only a container. It does not define page data, routes, or entity behavior by itself.

## Nesting Rules

Groups can nest by pointing `parent` to another page group name.

Important constraints:

* Group nesting cannot be circular.
* Maximum depth is `3` levels.
* Root groups are groups whose `parent` is empty.

This keeps runtime navigation predictable and avoids unbounded nesting in the generated menu.

## Visibility and Permissions

Page groups do not replace page permissions. Runtime visibility still depends on the pages inside the group.

In practice:

* Pages are added to the runtime menu only if the current user can access them.
* Groups are containers for those visible pages.
* Groups without visible page descendants are omitted from the runtime root menu.

This means page permissions remain the real security boundary, while page groups control menu organization.

## Split Descriptor Example

In split descriptor projects, a page group file in `pageGroups/` stores **one descriptor object**, not a wrapper document:

```json
{
  "name": "inventory-insights",
  "title": "Insights",
  "icon": "fa-solid fa-folder-tree",
  "order": 20,
  "parent": "inventory"
}
```

For example, `pageGroups/inventory-insights.json` would use that shape directly.

If you are looking at the logical aggregate model instead of split files, the same data appears under the top-level `pageGroups` array:

```json
{
  "pageGroups": [
    {
      "name": "inventory",
      "title": "Inventory",
      "icon": "fa-solid fa-boxes-stacked",
      "order": 10
    },
    {
      "name": "inventory-insights",
      "title": "Insights",
      "icon": "fa-solid fa-folder-tree",
      "order": 20,
      "parent": "inventory"
    }
  ]
}
```

In split descriptor projects, page groups belong to the `pageGroups/` descriptor category.

## Icon Guidance

Use CSS class strings for icons, for example:

* `fa-solid fa-folder`
* `fa-solid fa-folder-tree`
* `fa-solid fa-boxes-stacked`

Do not treat `icon` as an image URL or file path.

## See Also

* [Dashboards](dashboards.md)
* [React Runtime](react-runtime.md)
* [Low-Code Designer](designer.md)
* [Model Descriptor Files](model-json.md)
