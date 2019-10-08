import { NgModule, APP_INITIALIZER } from '@angular/core';
import { MyProjectNameConfigService } from './services/my-project-name-config.service';
import { noop } from '@abp/ng.core';

@NgModule({
  declarations: [],
  providers: [{ provide: APP_INITIALIZER, deps: [MyProjectNameConfigService], multi: true, useFactory: noop }],
  exports: [],
})
export class MyProjectNameConfigModule {}
