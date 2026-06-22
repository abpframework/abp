import { Component, ViewEncapsulation, input, effect, signal, contentChild } from '@angular/core';
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
  readonly title = input<string | undefined>(undefined);
  readonly toolbarInput = input<any>(undefined, { alias: 'toolbar' });
  readonly breadcrumb = input(true);

  protected readonly toolbarVisible = signal(false);
  protected readonly toolbarData = signal<any>(undefined);

  pageParts = {
    title: PageParts.title,
    breadcrumb: PageParts.breadcrumb,
    toolbar: PageParts.toolbar,
  };

  readonly customTitle = contentChild(PageTitleContainerComponent);
  readonly customBreadcrumb = contentChild(PageBreadcrumbContainerComponent);
  readonly customToolbar = contentChild(PageToolbarContainerComponent);

  constructor() {
    effect(() => {
      const toolbar = this.toolbarInput();
      if (toolbar !== undefined) {
        this.toolbarData.set(toolbar);
        this.toolbarVisible.set(true);
      }
    });
  }

  get shouldRenderRow() {
    return !!(
      this.title() ||
      this.toolbarVisible() ||
      this.breadcrumb() ||
      this.customTitle() ||
      this.customBreadcrumb() ||
      this.customToolbar() ||
      this.pageParts
    );
  }
}
