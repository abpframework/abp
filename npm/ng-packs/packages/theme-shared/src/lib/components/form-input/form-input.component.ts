import { Component, EventEmitter, forwardRef, Input, Output } from '@angular/core';
import { FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import { AbstractNgModelComponent, LocalizationPipe } from '@abp/ng.core';

@Component({
  selector: 'abp-form-input',
  template: `
    <div class="mb-3">
      @if (label) {
        <label [class]="labelClass" [for]="inputId">
          {{ label | abpLocalization }}
        </label>
      }
      <input
        type="text"
        [id]="inputId"
        [placeholder]="inputPlaceholder"
        [readonly]="inputReadonly"
        [class]="inputClass"
        [style]="inputStyle"
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
  @Input() inputId!: string;
  @Input() inputReadonly = false;
  @Input() label = '';
  @Input() labelClass = 'form-label';
  @Input() inputPlaceholder = '';
  @Input() inputStyle:
    | {
      [klass: string]: any;
    }
    | null
    | undefined;
  @Input() inputClass = 'form-control';
  @Output() formBlur = new EventEmitter<void>();
  @Output() formFocus = new EventEmitter<void>();
}
