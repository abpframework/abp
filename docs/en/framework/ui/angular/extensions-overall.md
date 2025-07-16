# Angular UI Extensions

Angular UI extensions system allows you to add a new action to the actions menu, a new column to the data table, a new action to the toolbar of a page, and add a new field to the create and/or edit forms.

See the documents below for the details:

* [Entity Action Extensions](entity-action-extensions.md)
* [Data Table Column (or Entity Prop) Extensions](data-table-column-extensions.md)
* [Page Toolbar Extension](page-toolbar-extensions.md)
* [Dynamic Form (or Form Prop) Extensions](dynamic-form-extensions.md)

## Extensible Table Component

Using `ngx-datatable` in extensible table.

```html
<abp-extensible-table
  actionsText="Your Action"
  [data]="items"
  [recordsTotal]="totalCount"
  [actionsColumnWidth]="38"
  [actionsTemplate]="customAction"
  [list]="list"
  [selectable]="true"
  [selectionType]="'single'"
  (tableActivate)="onTableSelect($event)"
  (selectionChanged)="onSelectionChanged($event)">
</abp-extensible-table>
```

### Inputs

| Name                   | Description                                                            | Type            |
|------------------------|------------------------------------------------------------------------|-----------------|
| actionsText            | Column name of the action column                                        | string          |
| data                   | Items shown in your table                                             | Array<any>      |
| list                   | Instance of ListService                                               | ListService     |
| actionsColumnWidth     | Width of the action column                                             | number          |
| actionsTemplate        | Template of the action button (usually a ng-template)                 | TemplateRef<any> |
| recordsTotal           | Total number of records                                                | number          |
| selectable             | Enables row selection                                                  | boolean         |
| selectionType          | Selection mode: 'single' for radio buttons or 'multiClick' for checkboxes | 'single' | 'multiClick' |

### Outputs

| Name              | Description                                                              | Type                        |
|-------------------|--------------------------------------------------------------------------|-----------------------------|
| tableActivate      | Triggered when a cell or row is activated via keyboard or mouse         | EventEmitter<any>           |
| selectionChanged   | Emits the currently selected row(s) whenever selection changes          | EventEmitter<any[] \| any>  |

### Selection Behavior Summary

| selectionType    | Row Control   | Column Header        | Behavior                         |
|------------------|---------------|----------------------|----------------------------------|
| 'single'         | radio         | none                 | Only one row selected at a time  |
| 'multiClick' (default) | checkbox | select all checkbox | Multiple rows selectable         |
