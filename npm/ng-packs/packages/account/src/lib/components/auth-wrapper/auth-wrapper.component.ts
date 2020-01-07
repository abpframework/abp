import { Component, Input, TemplateRef } from '@angular/core';
import { Account } from '../../models/account';
import { Select } from '@ngxs/store';
import { ConfigState } from '@abp/ng.core';
import { Observable } from 'rxjs';

@Component({
  selector: 'abp-auth-wrapper',
  templateUrl: './auth-wrapper.component.html',
  exportAs: 'abpAuthWrapper',
})
export class AuthWrapperComponent
  implements Account.AuthWrapperComponentInputs, Account.AuthWrapperComponentOutputs {
  @Select(ConfigState.getSetting('Abp.Account.EnableLocalLogin'))
  enableLocalLogin$: Observable<'True' | 'False'>;

  @Input()
  readonly mainContentRef: TemplateRef<any>;

  @Input()
  readonly cancelContentRef: TemplateRef<any>;
}
