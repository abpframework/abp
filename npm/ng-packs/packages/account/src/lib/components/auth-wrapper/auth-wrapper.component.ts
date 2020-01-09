import { Component, Input, TemplateRef, OnInit, OnDestroy } from '@angular/core';
import { Account } from '../../models/account';
import { Select, Store } from '@ngxs/store';
import { ConfigState } from '@abp/ng.core';
import { Observable } from 'rxjs';

@Component({
  selector: 'abp-auth-wrapper',
  templateUrl: './auth-wrapper.component.html',
  exportAs: 'abpAuthWrapper',
})
export class AuthWrapperComponent
  implements
    Account.AuthWrapperComponentInputs,
    Account.AuthWrapperComponentOutputs,
    OnInit,
    OnDestroy {
  @Input()
  readonly mainContentRef: TemplateRef<any>;

  @Input()
  readonly cancelContentRef: TemplateRef<any>;

  enableLocalLogin = true;

  constructor(private store: Store) {}

  ngOnInit() {
    this.store.select(ConfigState.getSetting('Abp.Account.EnableLocalLogin')).subscribe(value => {
      if (value) {
        this.enableLocalLogin = value.toLowerCase() !== 'false';
      }
    });
  }

  ngOnDestroy() {}
}
