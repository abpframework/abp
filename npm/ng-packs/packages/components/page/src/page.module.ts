import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { PagePartDirective } from './page-part.directive';
import {
  PageBreadcrumbContainerComponent,
  PageTitleContainerComponent,
  PageToolbarContainerComponent,
} from './page-parts.component';
import { PageComponent } from './page.component';
import {PageToolbarComponent} from "@abp/ng.components/extensible";

const exportedDeclarations = [
  PageComponent,
  PageTitleContainerComponent,
  PageBreadcrumbContainerComponent,
  PageToolbarContainerComponent,
  PagePartDirective,
];

@NgModule({
  declarations: [...exportedDeclarations],
  imports: [CoreModule, ThemeSharedModule, PageToolbarComponent],
  exports: [...exportedDeclarations],
})
export class PageModule {}
