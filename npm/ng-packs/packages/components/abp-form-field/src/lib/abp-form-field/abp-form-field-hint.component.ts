import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'abp-form-field-hint',
  template: `<small class="form-text text-muted">
    <ng-content></ng-content>
  </small>`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AbpFormFieldHintComponent {
}
