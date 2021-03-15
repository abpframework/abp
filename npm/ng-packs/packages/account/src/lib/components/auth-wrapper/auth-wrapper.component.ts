import { ConfigStateService, MultiTenancyService, SubscriptionService } from '@abp/ng.core';
import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { eAccountComponents } from '../../enums/components';

@Component({
  selector: 'abp-auth-wrapper',
  templateUrl: './auth-wrapper.component.html',
  exportAs: 'abpAuthWrapper',
  providers: [SubscriptionService],
})
export class AuthWrapperComponent {
  isMultiTenancyEnabled$ = this.configState.getDeep$('multiTenancy.isEnabled');

  get enableLocalLogin$(): Observable<boolean> {
    return this.configState
      .getSetting$('Abp.Account.EnableLocalLogin')
      .pipe(map(value => value?.toLowerCase() !== 'false'));
  }

  tenantBoxKey = eAccountComponents.TenantBox;

  constructor(
    public readonly multiTenancy: MultiTenancyService,
    private configState: ConfigStateService,
  ) {}
}
