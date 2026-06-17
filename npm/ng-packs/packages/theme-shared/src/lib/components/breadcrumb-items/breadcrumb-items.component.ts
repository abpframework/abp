import {Component, input, ChangeDetectionStrategy,} from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ABP, AbpRouteCultureUrlPipe, LocalizationPipe } from '@abp/ng.core';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-breadcrumb-items',
  templateUrl: './breadcrumb-items.component.html',
  imports: [NgTemplateOutlet, RouterLink, LocalizationPipe, AbpRouteCultureUrlPipe],
})
export class BreadcrumbItemsComponent {
  readonly items = input<Partial<ABP.Route>[]>([]);
}
