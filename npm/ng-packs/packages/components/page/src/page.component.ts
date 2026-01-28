import { Component, Input, ViewEncapsulation, ContentChild, input } from '@angular/core';
import {
  PageTitleContainerComponent,
  PageBreadcrumbContainerComponent,
  PageToolbarContainerComponent,
  PageParts,
} from './page-parts.component';
import { BreadcrumbComponent } from '@abp/ng.theme.shared';
import { PageToolbarComponent } from '@abp/ng.components/extensible';
import { PagePartDirective } from './page-part.directive';

@Component({
  selector: 'abp-page',
  templateUrl: './page.component.html',
  encapsulation: ViewEncapsulation.None,
  imports: [BreadcrumbComponent, PageToolbarComponent, PagePartDirective],
})
export class PageComponent {
  readonly title = input<string>(undefined);

  toolbarVisible = false;
  _toolbarData: any;
  @Input() set toolbar(val: any) {
    this._toolbarData = val;
    this.toolbarVisible = true;
  }

  get toolbarData() {
    return this._toolbarData;
  }

  readonly breadcrumb = input(true);

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
      this.title() ||
      this.toolbarVisible ||
      this.breadcrumb() ||
      this.customTitle ||
      this.customBreadcrumb ||
      this.customToolbar ||
      this.pageParts
    );
  }
}
