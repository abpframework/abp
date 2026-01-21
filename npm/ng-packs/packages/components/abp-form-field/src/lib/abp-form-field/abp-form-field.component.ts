import {
  Component,
  ChangeDetectionStrategy,
  input,
  HostBinding,
  InjectionToken,
  QueryList,
  ContentChild,
  contentChild,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbpFormFieldLabelComponent } from './abp-form-field-label.component';

export const ABP_FORM_FIELD = new InjectionToken<AbpFormFieldComponent>('AbpFormFieldComponent');

@Component({
  selector: 'abp-form-field',
  templateUrl: './abp-form-field.component.html',
  imports: [CommonModule],
  exportAs: 'abpFormField',
  providers: [{ provide: ABP_FORM_FIELD, useExisting: AbpFormFieldComponent }],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AbpFormFieldComponent {

  containerClass = input<string>('mb-3');
  labelComponent = contentChild(AbpFormFieldLabelComponent);

  @HostBinding('class')
  get hostClasses(): string {
    return `d-block mb-3 ${this.containerClass()}`;
  }
}
