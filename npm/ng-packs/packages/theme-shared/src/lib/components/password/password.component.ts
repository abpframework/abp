import { Component, forwardRef, Input } from '@angular/core';
import { AbstractNgModelComponent } from '@abp/ng.core';
import { FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CommonModule } from '@angular/common';

/**
 * @deprecated use ShowPasswordDirective directive 
 * https://docs.abp.io/en/abp/latest/UI/Angular/Show-Password-Directive
 */
@Component({
  selector: 'abp-password',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: `./password.component.html`,
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
  fieldTextType?: boolean;

  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }
}
