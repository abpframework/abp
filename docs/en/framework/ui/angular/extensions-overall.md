# Angular UI Extensions

Angular UI extensions system allows you to add a new action to the actions menu, a new column to the data table, a new action to the toolbar of a page, and add a new field to the create and/or edit forms.

See the documents below for the details:

* [Entity Action Extensions](entity-action-extensions.md)
* [Data Table Column (or Entity Prop) Extensions](data-table-column-extensions.md)
* [Page Toolbar Extension](page-toolbar-extensions.md)
* [Dynamic Form (or Form Prop) Extensions](dynamic-form-extensions.md)

##  Extensible Table Component

Using [ngx-datatable](https://github.com/swimlane/ngx-datatable) in extensible table.

````ts
      <abp-extensible-table
         actionsText="Your Action"
         [data]="items"
         [recordsTotal]="totalCount"
         [actionsColumnWidth]="38"
         [actionsTemplate]="customAction"
         [list]="list"
         (tableActivate)="onTableSelect($event)" > 
      </abp-extensible-table>
````

 * `       actionsText : ` ** Column name of action column. **Type** : string
 * `              data : ` Items shows in your table. **Type** : Array<any>
 * `              list : ` Instance of ListService. **Type** : ListService
 * `actionsColumnWidth : ` Width of your action column. **Type** : number
 * `   actionsTemplate : ` Template of the action when "click this button" or whatever. Generally ng-template. **Type** : TemplateRef<any>
 * `      recordsTotal : ` Count of the record total. **Type** : number
 * `    tableActivate  : ` The Output(). A cell or row was focused via the keyboard or a mouse click. **Type** : EventEmitter() 
