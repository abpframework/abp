import { AbstractNgModelComponent } from '@abp/ng.core';
import { Component, EventEmitter, forwardRef, Injector, Input, Output } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'abp-checkbox',
  templateUrl: './checkbox.component.html',
  styleUrls: ['./checkbox.component.scss'],
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
  @Input() checkboxId!: string;
  @Input() checkboxStyle = '';
  @Input() checkboxClass = '';
  @Input() checkboxReadonly = false;
  @Output() onBlur = new EventEmitter<void>();
  @Output() onFocus = new EventEmitter<void>();
  
  constructor(injector: Injector) {
    super(injector);
  }

}
