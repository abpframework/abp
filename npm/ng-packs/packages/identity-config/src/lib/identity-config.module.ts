import { NgModule, APP_INITIALIZER } from '@angular/core';
import { noop, CoreModule } from '@abp/ng.core';
import { IdentityConfigService } from './services/identity-config.service';
import { ThemeSharedModule } from '@abp/ng.theme.shared';

@NgModule({
  imports: [CoreModule, ThemeSharedModule],
  providers: [{ provide: APP_INITIALIZER, deps: [IdentityConfigService], useFactory: noop, multi: true }],
})
export class IdentityConfigModule {}
