## Angular UI Extensions

Angular UI extensions system allows you to add a new action to the actions menu, a new column to the data table, a new action to the toolbar of a page, and add a new field to the create and/or edit forms.

See the documents below for the details:

* [Entity Action Extensions](Entity-Action-Extensions.md)
* [Data Table Column (or Entity Prop) Extensions](Data-Table-Column-Extensions.md)
* [Page Toolbar Extension](Page-Toolbar-Extensions.md)
* [Dynamic Form (or Form Prop) Extensions](Dynamic-Form-Extensions.md)

##  Extensible Table Component

Using [ngx-datatable](https://github.com/swimlane/ngx-datatable) in extensinble table.

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

`       actionsText : ` ** Column name of action column.

`              data : ` Items shows in your table. 

`              list : ` Instance of ListService. 

`actionsColumnWidth : ` Width of your action column. 

`   actionsTemplate : ` Template of action when click this button or whatever. Generally ng-template.

`      recordsTotal : ` Count of record total. 

`    tableActivate  : ` It is Output(). A cell or row was focused via keyboard or mouse click.
A cell or row was focused via keyboard or mouse click.
