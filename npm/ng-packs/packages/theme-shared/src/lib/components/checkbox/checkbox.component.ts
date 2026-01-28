import { Component, EventEmitter, forwardRef, Output, input } from '@angular/core';
import { NG_VALUE_ACCESSOR, FormsModule } from '@angular/forms';
import { AbstractNgModelComponent, LocalizationPipe } from '@abp/ng.core';

@Component({
  selector: 'abp-checkbox',
  template: `
    <div class="mb-3">
      <input
        type="checkbox"
        [(ngModel)]="value"
        [id]="checkboxId()"
        [readonly]="checkboxReadonly()"
        [class]="checkboxClass()"
        [style]="checkboxStyle()"
        (blur)="checkboxBlur.next()"
        (focus)="checkboxFocus.next()"
      />
      @if (label()) {
        <label [class]="labelClass()" [for]="checkboxId()">
          {{ label() | abpLocalization }}
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
  readonly label = input<string>(undefined);
  readonly labelClass = input('form-check-label');
  readonly checkboxId = input.required<string>();
  readonly checkboxStyle = input<{
    [klass: string]: any;
} | null | undefined>(undefined);
  readonly checkboxClass = input('form-check-input');
  readonly checkboxReadonly = input(false);
  @Output() checkboxBlur = new EventEmitter<void>();
  @Output() checkboxFocus = new EventEmitter<void>();
}
