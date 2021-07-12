import { Injectable, Inject } from '@angular/core';
import { switchMap, map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ABP } from '../models/common';
import { FindTenantResultDto } from '../proxy/volo/abp/asp-net-core/mvc/multi-tenancy/models';
import { RestService } from './rest.service';
import { AbpTenantService } from '../proxy/pages/abp/multi-tenancy';
import { ConfigStateService } from './config-state.service';
import { SessionStateService } from './session-state.service';
import { TENANT_KEY } from '../tokens/tenant-key.token';

@Injectable({ providedIn: 'root' })
export class MultiTenancyService {
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

  /**
   * @deprecated Use AbpTenantService.findTenantByName method instead. To be deleted in v5.0.
   */
  findTenantByName(name: string, headers: ABP.Dictionary<string>): Observable<FindTenantResultDto> {
    return this.restService.request(
      {
        url: `/api/abp/multi-tenancy/tenants/by-name/${name}`,
        method: 'GET',
        headers,
      },
      { apiName: this.apiName },
    );
  }

  /**
   * @deprecated Use AbpTenantService.findTenantById method instead. To be deleted in v5.0.
   */
  findTenantById(id: string, headers: ABP.Dictionary<string>): Observable<FindTenantResultDto> {
    return this.restService.request(
      { url: `/api/abp/multi-tenancy/tenants/by-id/${id}`, method: 'GET', headers },
      { apiName: this.apiName },
    );
  }

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
