import { AbstractNgModelComponent } from '@abp/ng.core';
import { Component, EventEmitter, forwardRef, Injector, Input, Output } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'abp-form-input',
  template: `
    <div class="mb-3">
      <label class= *ngIf="label" [ngClass]="labelClass" [for]="inputId" > {{label | abpLocalization}} </label>
      <input
        type="text"
        [id]="inputId" 
        [placeholder]="inputPlaceholder" 
        [readonly]="inputReadonly"
        [ngClass]="inputClass" 
        [ngStyle]="inputStyle" 
        (blur)="onBlur.next()"
        (focus)="onFocus.next()" 
        [(ngModel)]="value">
    </div>
  `,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormInputComponent),
      multi: true,
    },
  ]
})
export class FormInputComponent extends AbstractNgModelComponent {
  @Input() inputId!: string;
  @Input() inputReadonly: boolean = false;
  @Input() label: string = '';
  @Input() labelClass = 'form-label';
  @Input() inputPlaceholder: string = '';
  @Input() inputType: string = 'text';
  @Input() inputStyle: string = '';
  @Input() inputClass: string = 'form-control';
  @Output() onBlur = new EventEmitter<void>();
  @Output() onFocus = new EventEmitter<void>();

  constructor(injector: Injector) {
    super(injector);
  }

}
