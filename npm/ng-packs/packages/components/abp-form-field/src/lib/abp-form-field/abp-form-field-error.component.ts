import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'abp-form-field-error',
  template: `<div class="invalid-feedback d-block">
    <ng-content></ng-content>
  </div>`,
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class AbpFormFieldErrorComponent {

}
