import { Component, ChangeDetectionStrategy, input, HostBinding } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'abp-form-field',
  templateUrl: './abp-form-field.component.html',
  imports: [CommonModule],
  exportAs: 'abpFormField',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AbpFormFieldComponent {

  containerClass = input<string>('mb-3');

  @HostBinding('class')
  get hostClasses(): string {
    return `d-block mb-3 ${this.containerClass()}`;
  }
}
