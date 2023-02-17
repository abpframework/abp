import { AbstractNgModelComponent } from '@abp/ng.core';
import { Component, EventEmitter, forwardRef, Injector, Input, Output } from '@angular/core';
import { CheckboxControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'abp-checkbox',
  templateUrl: './checkbox.component.html',
  styleUrls: ['./checkbox.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CheckboxComponent),
      multi: true,
    },
  ]
})
export class CheckboxComponent extends AbstractNgModelComponent {

  @Input() label: string;
  @Input() checkboxId!: string;
  @Input() formControl!: string;
  @Input() checkboxStyle: string = '';
  @Input() checkboxClass: string = '';
  @Input() checkboxReadonly: boolean = false;
  @Output() onBlur = new EventEmitter<void>();
  @Output() onFocus = new EventEmitter<void>();
  
  constructor(injector: Injector) {
    super(injector);
  }

}
