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
  @Input() title?: string;

  toolbarVisible = false;
  _toolbarData: any;
  @Input() set toolbar(val: any) {
    this._toolbarData = val;
    this.toolbarVisible = true;
  }

  get toolbarData() {
    return this._toolbarData;
  }

  @Input() breadcrumb = true;

  pageParts = {
    title: PageParts.title,
    breadcrumb: PageParts.breadcrumb,
    toolbar: PageParts.toolbar,
  };

  @ContentChild(PageTitleContainerComponent) customTitle?: PageTitleContainerComponent;
  @ContentChild(PageBreadcrumbContainerComponent)
  customBreadcrumb?: PageBreadcrumbContainerComponent;
  @ContentChild(PageToolbarContainerComponent) customToolbar?: PageToolbarContainerComponent;

  get shouldRenderRow() {
    return !!(
      this.title ||
      this.toolbarVisible ||
      this.breadcrumb ||
      this.customTitle ||
      this.customBreadcrumb ||
      this.customToolbar
    );
  }
}
