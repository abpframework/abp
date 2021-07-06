import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
import { ConfigStateService, MultiTenancyService } from '@abp/ng.core';

@Injectable()
export class AuthWrapperService {
  isMultiTenancyEnabled$ = this.configState.getDeep$('multiTenancy.isEnabled');

  get enableLocalLogin$(): Observable<boolean> {
    return this.configState
      .getSetting$('Abp.Account.EnableLocalLogin')
      .pipe(map(value => value?.toLowerCase() !== 'false'));
  }

  tenantBoxKey = 'Account.TenantBoxComponent';
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
