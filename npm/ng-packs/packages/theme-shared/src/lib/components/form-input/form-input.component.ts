import { Component, EventEmitter, forwardRef, Output, input } from '@angular/core';
import { FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import { AbstractNgModelComponent, LocalizationPipe } from '@abp/ng.core';

@Component({
  selector: 'abp-form-input',
  template: `
    <div class="mb-3">
      @if (label()) {
        <label [class]="labelClass()" [for]="inputId()">
          {{ label() | abpLocalization }}
        </label>
      }
      <input
        type="text"
        [id]="inputId()"
        [placeholder]="inputPlaceholder()"
        [readonly]="inputReadonly()"
        [class]="inputClass()"
        [style]="inputStyle()"
        (blur)="formBlur.next()"
        (focus)="formFocus.next()"
        [(ngModel)]="value"
      />
    </div>
  `,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormInputComponent),
      multi: true,
    },
  ],
  imports: [LocalizationPipe, FormsModule],
})
export class FormInputComponent extends AbstractNgModelComponent {
  readonly inputId = input.required<string>();
  readonly inputReadonly = input(false);
  readonly label = input('');
  readonly labelClass = input('form-label');
  readonly inputPlaceholder = input('');
  readonly inputStyle = input<{
    [klass: string]: any;
} | null | undefined>(undefined);
  readonly inputClass = input('form-control');
  @Output() formBlur = new EventEmitter<void>();
  @Output() formFocus = new EventEmitter<void>();
}
