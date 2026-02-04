import { Component, EventEmitter, forwardRef, Input, Output } from '@angular/core';
import { NG_VALUE_ACCESSOR, FormsModule } from '@angular/forms';
import { AbstractNgModelComponent, LocalizationPipe } from '@abp/ng.core';

@Component({
  selector: 'abp-checkbox',
  template: `
    <div class="mb-3">
      <input
        type="checkbox"
        [(ngModel)]="value"
        [id]="checkboxId"
        [readonly]="checkboxReadonly"
        [class]="checkboxClass"
        [style]="checkboxStyle"
        (blur)="checkboxBlur.next()"
        (focus)="checkboxFocus.next()"
      />
      @if (label) {
        <label [class]="labelClass" [for]="checkboxId">
          {{ label | abpLocalization }}
        </label>
      }
    </div>
  `,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormCheckboxComponent),
      multi: true,
    },
  ],
  imports: [FormsModule, LocalizationPipe],
})
export class FormCheckboxComponent extends AbstractNgModelComponent {
  @Input() label?: string;
  @Input() labelClass = 'form-check-label';
  @Input() checkboxId!: string;
  @Input() checkboxStyle:
    | {
      [klass: string]: any;
    }
    | null
    | undefined;
  @Input() checkboxClass = 'form-check-input';
  @Input() checkboxReadonly = false;
  @Output() checkboxBlur = new EventEmitter<void>();
  @Output() checkboxFocus = new EventEmitter<void>();
}
