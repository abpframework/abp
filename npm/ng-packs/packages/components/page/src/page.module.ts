import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { UiExtensionsModule } from '@abp/ng.theme.shared/extensions';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { PagePartDirective } from './page-part.directive';
import {
  PageBreadcrumbContainerComponent,
  PageTitleContainerComponent,
  PageToolbarContainerComponent,
} from './page-parts.component';
import { PageComponent } from './page.component';

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
  exports: [...exportedDeclarations, UiExtensionsModule],
})
export class PageModule {}
