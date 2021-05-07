import { ModuleWithProviders, NgModule } from '@angular/core';
import { LeptonXModule } from '@lepton-x/lite';
import { LeptonXCommonSettings } from '@lepton-x/common';
import { ValidationErrorModule } from './components/validation-error';

@NgModule({
  declarations: [],
  imports: [LeptonXModule, ValidationErrorModule],
  exports: [],
})
export class ThemeLeptonXModule {
  static forRoot(settings?: LeptonXCommonSettings): ModuleWithProviders<ThemeLeptonXModule> {
    return {
      ngModule: ThemeLeptonXModule,
      providers: [
        LeptonXModule.forRoot(settings).providers,
        ValidationErrorModule.forRoot().providers,
      ],
    };
  }
}
