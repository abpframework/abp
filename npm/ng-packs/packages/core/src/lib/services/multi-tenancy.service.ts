import { Inject, Injectable } from '@angular/core';
import { map, switchMap } from 'rxjs/operators';
import { AbpTenantService } from '../proxy/pages/abp/multi-tenancy';
import {
  CurrentTenantDto,
  FindTenantResultDto,
} from '../proxy/volo/abp/asp-net-core/mvc/multi-tenancy/models';
import { TENANT_KEY } from '../tokens/tenant-key.token';
import { ConfigStateService } from './config-state.service';
import { RestService } from './rest.service';
import { SessionStateService } from './session-state.service';

@Injectable({ providedIn: 'root' })
export class MultiTenancyService {
  domainTenant: CurrentTenantDto = null;

  isTenantBoxVisible = true;

  apiName = 'abp';

  private setTenantToState = (tenant: FindTenantResultDto) => {
    this.sessionState.setTenant({ id: tenant.tenantId, name: tenant.name, isAvailable: true });
    return this.configStateService.refreshAppState().pipe(map(_ => tenant));
  };

  constructor(
    private restService: RestService,
    private sessionState: SessionStateService,
    private tenantService: AbpTenantService,
    private configStateService: ConfigStateService,
    @Inject(TENANT_KEY) public tenantKey: string,
  ) {}

  setTenantByName(tenantName: string) {
    return this.tenantService
      .findTenantByName(tenantName, { [this.tenantKey]: '' })
      .pipe(switchMap(this.setTenantToState));
  }

  setTenantById(tenantId: string) {
    return this.tenantService
      .findTenantById(tenantId, { [this.tenantKey]: '' })
      .pipe(switchMap(this.setTenantToState));
  }
}
