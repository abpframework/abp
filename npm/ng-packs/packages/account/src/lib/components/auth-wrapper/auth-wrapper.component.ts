import { Component, Input, TemplateRef } from '@angular/core';
import { Account } from '../../models/account';

@Component({
  selector: 'abp-auth-wrapper',
  templateUrl: './auth-wrapper.component.html',
  exportAs: 'abpAuthWrapper',
})
export class AuthWrapperComponent
  implements Account.AuthWrapperComponentInputs, Account.AuthWrapperComponentOutputs {
  @Input()
  readonly mainContentRef: TemplateRef<any>;

  @Input()
  readonly cancelContentRef: TemplateRef<any>;
}
