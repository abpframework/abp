import { NgModule } from '@angular/core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CoreModule } from '@abp/ng.core';
import { ThemeBasicModule } from '@abp/ng.theme.basic';

@NgModule({
  declarations: [],
  imports: [CoreModule, ThemeSharedModule, ThemeBasicModule],
  exports: [CoreModule, ThemeSharedModule, ThemeBasicModule],
  providers: [],
})
export class SharedModule {}
