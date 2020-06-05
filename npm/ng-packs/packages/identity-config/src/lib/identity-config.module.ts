import { noop } from '@abp/ng.core';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { IdentityConfigService } from './services/identity-config.service';

@NgModule({
  providers: [{ provide: APP_INITIALIZER, deps: [IdentityConfigService], useFactory: noop, multi: true }],
})
export class IdentityConfigModule {}
