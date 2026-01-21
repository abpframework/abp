import { ChangeDetectionStrategy, Component, input } from '@angular/core';

@Component({
  selector: 'abp-form-field-label',
  template: `<ng-content></ng-content>`,
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class AbpFormFieldLabelComponent {
  for= input<string>('');
}
