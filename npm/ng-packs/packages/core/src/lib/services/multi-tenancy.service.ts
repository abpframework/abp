import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ABP } from '../models/common';
import {
  CurrentTenantDto,
  FindTenantResultDto,
} from '../proxy/volo/abp/asp-net-core/mvc/multi-tenancy/models';
import { RestService } from './rest.service';
import { SessionStateService } from './session-state.service';

@Injectable({ providedIn: 'root' })
export class MultiTenancyService {
  private _domainTenant: CurrentTenantDto = null;

  set domainTenant(value: CurrentTenantDto) {
    this._domainTenant = value;
    this.sessionState.setTenant(value);
  }

  get domainTenant() {
    return this._domainTenant;
  }

  isTenantBoxVisible = true;

  apiName = 'abp';

  constructor(private restService: RestService, private sessionState: SessionStateService) {}

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
}
