import { NgModule } from '@angular/core';
import { PagePartDirective } from './page-part.directive';
import {
  PageBreadcrumbContainerComponent,
  PageTitleContainerComponent,
  PageToolbarContainerComponent,
} from './page-parts.component';
import { PageComponent } from './page.component';

export const PAGE_EXPORTS = [
  PageComponent,
  PageTitleContainerComponent,
  PageBreadcrumbContainerComponent,
  PageToolbarContainerComponent,
  PagePartDirective,
];

@NgModule({
  declarations: [],
  imports: [...PAGE_EXPORTS],
  exports: [...PAGE_EXPORTS],
})
export class PageModule {}
