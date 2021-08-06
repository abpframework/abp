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

  get isTenantBoxVisibleForCurrentRoute() {
    return this.getMostInnerChild().data.tenantBoxVisible ?? true;
  }

  get isTenantBoxVisible() {
    return this.isTenantBoxVisibleForCurrentRoute && this.multiTenancy.isTenantBoxVisible;
  }

  constructor(
    public readonly multiTenancy: MultiTenancyService,
    private configState: ConfigStateService,
    injector: Injector,
  ) {
    this.route = injector.get(ActivatedRoute);
  }

  private getMostInnerChild() {
    let child = this.route.snapshot;
    let depth = 0;
    const depthLimit = 10;
    while (child.firstChild && depth < depthLimit) {
      child = child.firstChild;
      depth++;
    }
    return child;
  }
}
