import { ModuleWithProviders, NgModule } from '@angular/core';
import { Options } from './models/options';
import { ACCOUNT_OPTIONS, optionsFactory } from './tokens/options.token';

@NgModule({})
export class RootAccountModule {
  static forRoot(options = {} as Options): ModuleWithProviders {
    return {
      ngModule: RootAccountModule,
      providers: [
        { provide: ACCOUNT_OPTIONS, useValue: options },
        {
          provide: 'ACCOUNT_OPTIONS',
          useFactory: optionsFactory,
          deps: [ACCOUNT_OPTIONS],
        },
      ],
    };
  }
}
