import { Component, Input } from '@angular/core';

@Component({
  selector: 'abp-breadcrumb-items',
  templateUrl: './breadcrumb-items.component.html',
})
export class BreadcrumbItemsComponent {
  @Input() items = [];
}
