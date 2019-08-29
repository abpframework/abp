import { ModuleWithProviders, NgModule } from '@angular/core';

@NgModule({})
export class RootIdentityModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: RootIdentityModule,
      providers: [],
    };
  }
}
