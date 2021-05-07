import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ValidationErrorComponent } from './validation-error.component';
import {
  NgxValidateCoreModule,
  VALIDATION_ERROR_TEMPLATE,
  VALIDATION_INVALID_CLASSES,
  VALIDATION_TARGET_SELECTOR,
} from '@ngx-validate/core';
import { CoreModule } from '@abp/ng.core';

@NgModule({
  declarations: [ValidationErrorComponent],
  imports: [CommonModule, CoreModule, NgxValidateCoreModule],
  exports: [ValidationErrorComponent, NgxValidateCoreModule],
})
export class ValidationErrorModule {
  static forRoot(): ModuleWithProviders<ValidationErrorModule> {
    return {
      ngModule: ValidationErrorModule,
      providers: [
        {
          provide: VALIDATION_ERROR_TEMPLATE,
          useValue: ValidationErrorComponent,
        },
        {
          provide: VALIDATION_TARGET_SELECTOR,
          useValue: '.form-group',
        },
        {
          provide: VALIDATION_INVALID_CLASSES,
          useValue: 'is-invalid',
        },
      ],
    };
  }
}
