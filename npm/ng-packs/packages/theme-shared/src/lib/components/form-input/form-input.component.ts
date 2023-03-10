import { AbstractNgModelComponent } from '@abp/ng.core';
import { Component, EventEmitter, forwardRef, Injector, Input, Output } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'abp-form-input',
  template: `
    <div class="mb-3">
      <label *ngIf="label" [ngClass]="labelClass" [for]="inputId">
        {{ label | abpLocalization }}
      </label>
      <input
        type="text"
        [id]="inputId"
        [placeholder]="inputPlaceholder"
        [readonly]="inputReadonly"
        [ngClass]="inputClass"
        [ngStyle]="inputStyle"
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
})
export class FormInputComponent extends AbstractNgModelComponent {
  @Input() inputId!: string;
  @Input() inputReadonly = false;
  @Input() label = '';
  @Input() labelClass = 'form-label';
  @Input() inputPlaceholder = '';
  @Input() inputType = 'text';
  @Input() inputStyle = '';
  @Input() inputClass = 'form-control';
  @Output() formBlur = new EventEmitter<void>();
  @Output() formFocus = new EventEmitter<void>();

  constructor(injector: Injector) {
    super(injector);
  }
}
