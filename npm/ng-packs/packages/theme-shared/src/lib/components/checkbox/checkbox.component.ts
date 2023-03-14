import { AbstractNgModelComponent } from '@abp/ng.core';
import { Component, EventEmitter, forwardRef, Injector, Input, Output } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'abp-checkbox',
  template: `
    <div class="mb-3">
      <input
        type="checkbox"
        [(ngModel)]="value"
        [id]="checkboxId"  
        [readonly]="checkboxReadonly"
        [ngClass]="checkboxClass" 
        [ngStyle]="checkboxStyle" 
        (blur)="onBlur.next()"
        (focus)="onFocus.next()" 
      >    
      <label *ngIf="label" [ngClass]="labelClass" [for]="checkboxId" > {{label | abpLocalization}} </label>
    </div>
  `,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FormCheckboxComponent),
      multi: true,
    },
  ]
})
export class FormCheckboxComponent extends AbstractNgModelComponent {

  @Input() label?: string;
  @Input() labelClass = 'form-check-label';
  @Input() checkboxId!: string;
  @Input() checkboxStyle = '';
  @Input() checkboxClass = 'form-check-input';
  @Input() checkboxReadonly = false;
  @Output() onBlur = new EventEmitter<void>();
  @Output() onFocus = new EventEmitter<void>();

  constructor(injector: Injector) {
    super(injector);
  }

}
