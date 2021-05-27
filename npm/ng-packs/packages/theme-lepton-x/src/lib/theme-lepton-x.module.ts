import { ModuleWithProviders, NgModule } from '@angular/core';
import { LeptonXModule } from '@volo/ngx-lepton-x.lite';
import { LeptonXCoreSettings } from '@volo/ngx-lepton-x.core';
import { ValidationErrorModule } from './components/validation-error';
import { LPX_USER_PROVIDER } from './providers/user.provider';
import { LPX_LANGUAGE_PROVIDER } from './providers/language.provider';
import { LPX_BREADCRUMB_PROVIDER } from './providers/breadcrumb.provider';

@NgModule({
  declarations: [],
  imports: [LeptonXModule, ValidationErrorModule],
  exports: [],
})
export class ThemeLeptonXModule {
  static forRoot(settings?: LeptonXCoreSettings): ModuleWithProviders<ThemeLeptonXModule> {
    return {
      ngModule: ThemeLeptonXModule,
      providers: [
        LeptonXModule.forRoot(settings).providers,
        ValidationErrorModule.forRoot().providers,
        LPX_USER_PROVIDER,
        LPX_LANGUAGE_PROVIDER,
        LPX_BREADCRUMB_PROVIDER,
      ],
    };
  }
}
