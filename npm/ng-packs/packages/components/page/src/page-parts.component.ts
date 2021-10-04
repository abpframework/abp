import { Component, ViewEncapsulation } from '@angular/core';

export enum PageParts {
  title = 'PageTitleContainerComponent',
  breadcrumb = 'PageBreadcrumbContainerComponent',
  toolbar = 'PageToolbarContainerComponent',
}

@Component({
  selector: 'abp-page-title-container',
  template: ` <ng-content></ng-content> `,
  encapsulation: ViewEncapsulation.None,
})
export class PageTitleContainerComponent {}

@Component({
  selector: 'abp-page-breadcrumb-container',
  template: ` <ng-content></ng-content> `,
  encapsulation: ViewEncapsulation.None,
})
export class PageBreadcrumbContainerComponent {}

@Component({
  selector: 'abp-page-toolbar-container',
  template: ` <ng-content></ng-content> `,
  encapsulation: ViewEncapsulation.None,
})
export class PageToolbarContainerComponent {}
