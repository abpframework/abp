import { ConfigStateService, MultiTenancyService, SubscriptionService } from '@abp/ng.core';
import { Component, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { eAccountComponents } from '../../enums/components';
import { ActivatedRoute } from '@angular/router';

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
  route: ActivatedRoute;

  private _tenantBoxVisible = true;

  private setTenantBoxVisibility = () => {
    this._tenantBoxVisible = this.route.snapshot.firstChild.data.tenantBoxVisible ?? true;
  };

  get isTenantBoxVisible() {
    return this._tenantBoxVisible && this.multiTenancy.isTenantBoxVisible;
  }

  constructor(
    public readonly multiTenancy: MultiTenancyService,
    private configState: ConfigStateService,
    injector: Injector,
  ) {
    this.route = injector.get(ActivatedRoute);
    this.setTenantBoxVisibility();
  }
}
