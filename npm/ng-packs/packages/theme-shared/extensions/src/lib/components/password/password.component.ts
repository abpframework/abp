import { Component, Input } from '@angular/core';
import { EXTENSIBLE_FORM_VIEW_PROVIDER } from '../../tokens/extensible-form-view-provider.token';
import { FormProp } from '../../models/form-props';

@Component({
  selector: 'abp-password',
  templateUrl: `password.component.html`,
  viewProviders: [EXTENSIBLE_FORM_VIEW_PROVIDER],
})
export class PasswordComponent {
  @Input() prop!: FormProp;

  fieldTextType: boolean;

  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }
}
