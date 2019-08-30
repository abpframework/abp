import { ModuleWithProviders, NgModule } from '@angular/core';

@NgModule({})
export class RootFeatureManagementModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: RootFeatureManagementModule,
      providers: [],
    };
  }
}
