import { Component, forwardRef, Injector, Input } from '@angular/core';
import { AbstractNgModelComponent } from '@abp/ng.core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'abp-password',
  templateUrl: `password.component.html`,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => PasswordComponent),
      multi: true,
    },
  ],
})
export class PasswordComponent extends AbstractNgModelComponent {
  @Input() inputId!: string;
  @Input() formControlName!: string;
  fieldTextType: boolean;

  constructor(injector: Injector) {
    super(injector);
  }

  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }
}
