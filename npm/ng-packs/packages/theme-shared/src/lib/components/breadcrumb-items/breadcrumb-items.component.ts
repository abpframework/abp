import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ABP, LocalizationPipe } from '@abp/ng.core';

@Component({
  selector: 'abp-breadcrumb-items',
  templateUrl: './breadcrumb-items.component.html',
  imports: [CommonModule, RouterModule, LocalizationPipe],
})
export class BreadcrumbItemsComponent {
  @Input() items: Partial<ABP.Route>[] = [];
}
