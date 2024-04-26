import { ChangeDetectionStrategy, Component, ViewEncapsulation } from '@angular/core';
import { Validation, ValidationErrorComponent as ErrorComponent } from '@ngx-validate/core';

@Component({
  selector: 'abp-validation-error',
  template: `
    @for (error of abpErrors; track $index) {
      <div class="invalid-feedback" >
        {{ error.message | abpLocalization: error.interpoliteParams }}
      </div>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
})
export class ValidationErrorComponent extends ErrorComponent {
  get abpErrors(): (Validation.Error & { interpoliteParams?: string[] })[] {
    if (!this.errors || !this.errors.length) return [];

    return this.errors.map(error => {
      if (!error.message) return error;

      const index = error.message.indexOf('[');

      if (index > -1) {
        return {
          ...error,
          message: error.message.slice(0, index),
          interpoliteParams: error.message.slice(index + 1, error.message.length - 1).split(','),
        };
      }

      return error;
    });
  }
}
