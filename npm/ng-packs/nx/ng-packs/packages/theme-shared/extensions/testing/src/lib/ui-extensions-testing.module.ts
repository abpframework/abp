import { BaseUiExtensionsModule } from '@abp/ng.theme.shared/extensions';
import { NgModule } from '@angular/core';

@NgModule({
  exports: [BaseUiExtensionsModule],
  imports: [BaseUiExtensionsModule],
})
export class UiExtensionsTestingModule {}
