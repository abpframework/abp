import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UiExtensionsModule } from '@abp/ng.theme.shared/extensions';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CoreModule } from '@abp/ng.core';
import { PageComponent } from './page.component';
import {
  PageTitleContainerComponent,
  PageBreadcrumbContainerComponent,
  PageToolbarContainerComponent,
} from './page-parts.component';
import { PagePartDirective } from './page-part.directive';

const exportedDeclarations = [
  PageComponent,
  PageTitleContainerComponent,
  PageBreadcrumbContainerComponent,
  PageToolbarContainerComponent,
  PagePartDirective,
];

@NgModule({
  declarations: [...exportedDeclarations],
  imports: [CommonModule, UiExtensionsModule, CoreModule, ThemeSharedModule],
  exports: [...exportedDeclarations],
})
export class PageModule {}
