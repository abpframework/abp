import { ConfigState, takeUntilDestroy } from '@abp/ng.core';
import { Component, Input, OnDestroy, OnInit, TemplateRef } from '@angular/core';
import { Store } from '@ngxs/store';
import { Account } from '../../models/account';

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
    this.store
      .select(ConfigState.getSetting('Abp.Account.EnableLocalLogin'))
      .pipe(takeUntilDestroy(this))
      .subscribe(value => {
        if (value) {
          this.enableLocalLogin = value.toLowerCase() !== 'false';
        }
      });
  }

  ngOnDestroy() {}
}
