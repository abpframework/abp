import { Component, ViewEncapsulation } from '@angular/core';
import { PageAlertService } from '@abp/ng.theme.shared';

@Component({
  selector: 'abp-page-alert-container',
  templateUrl: './page-alert-container.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class PageAlertContainerComponent {
  constructor(public service: PageAlertService) {}
}
