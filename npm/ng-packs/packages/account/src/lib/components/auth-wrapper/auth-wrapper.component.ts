import { ConfigState, SubscriptionService, MultiTenancyService } from '@abp/ng.core';
import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { eAccountComponents } from '../../enums/components';
import { Account } from '../../models/account';

@Component({
  selector: 'abp-auth-wrapper',
  templateUrl: './auth-wrapper.component.html',
  exportAs: 'abpAuthWrapper',
  providers: [SubscriptionService],
})
export class AuthWrapperComponent
  implements Account.AuthWrapperComponentInputs, Account.AuthWrapperComponentOutputs, OnInit {
  @Input()
  readonly mainContentRef: TemplateRef<any>;

  @Input()
  readonly cancelContentRef: TemplateRef<any>;

  @Select(ConfigState.getDeep('multiTenancy.isEnabled'))
  isMultiTenancyEnabled$: Observable<boolean>;

  enableLocalLogin = true;

  tenantBoxKey = eAccountComponents.TenantBox;

  constructor(
    public readonly multiTenancy: MultiTenancyService,
    private store: Store,
    private subscription: SubscriptionService,
  ) {}

  ngOnInit() {
    this.subscription.addOne(
      this.store.select(ConfigState.getSetting('Abp.Account.EnableLocalLogin')),
      value => {
        if (value) {
          this.enableLocalLogin = value.toLowerCase() !== 'false';
        }
      },
    );
  }
}
