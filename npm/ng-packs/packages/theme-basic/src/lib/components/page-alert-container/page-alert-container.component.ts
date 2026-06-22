import { Component, ViewEncapsulation, inject } from '@angular/core';
import { PageAlertService } from '@abp/ng.theme.shared';
import { AsyncPipe } from '@angular/common';
import { LocalizationPipe, SafeHtmlPipe } from '@abp/ng.core';

@Component({
  selector: 'abp-page-alert-container',
  templateUrl: './page-alert-container.component.html',
  encapsulation: ViewEncapsulation.None,
  imports: [AsyncPipe, LocalizationPipe, SafeHtmlPipe],
})
export class PageAlertContainerComponent {
  service = inject(PageAlertService);
}
