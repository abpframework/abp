import { Component, Input, ViewEncapsulation, ContentChild } from '@angular/core';
import {
  PageTitleContainerComponent,
  PageBreadcrumbContainerComponent,
  PageToolbarContainerComponent,
  PageParts,
} from './page-parts.component';

@Component({
  selector: 'abp-page',
  templateUrl: './page.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class PageComponent {
  @Input() title: string;
  @Input() record: any;

  @Input() titleVisible = true;
  @Input() breadcrumbVisible = true;
  @Input() toolbarVisible = true;

  pageParts = {
    title: PageParts.title,
    breadcrumb: PageParts.breadcrumb,
    toolbar: PageParts.toolbar,
  };

  @ContentChild(PageTitleContainerComponent) customTitle: PageTitleContainerComponent;
  @ContentChild(PageBreadcrumbContainerComponent)
  customBreadcrumb: PageBreadcrumbContainerComponent;
  @ContentChild(PageToolbarContainerComponent) customToolbar: PageToolbarContainerComponent;
}
