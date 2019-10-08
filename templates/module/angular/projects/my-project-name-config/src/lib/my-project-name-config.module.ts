import { NgModule, APP_INITIALIZER } from '@angular/core';
import { MyProjectNameConfigService } from './services/my-project-name-config.service';
import { noop } from '@abp/ng.core';
import { MyProjectNameSettingsComponent } from './components/my-project-name-settings.component';

@NgModule({
  declarations: [MyProjectNameSettingsComponent],
  providers: [{ provide: APP_INITIALIZER, deps: [MyProjectNameConfigService], multi: true, useFactory: noop }],
  exports: [MyProjectNameSettingsComponent],
  entryComponents: [MyProjectNameSettingsComponent],
})
export class MyProjectNameConfigModule {}
