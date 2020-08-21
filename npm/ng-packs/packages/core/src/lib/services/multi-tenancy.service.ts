import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { SetTenant } from '../actions/session.actions';
import { ABP } from '../models/common';
import { FindTenantResultDto } from '../models/find-tenant-result-dto';
import { RestService } from './rest.service';

@Injectable({ providedIn: 'root' })
export class MultiTenancyService {
  private _domainTenant: ABP.BasicItem = null;

  set domainTenant(value: ABP.BasicItem) {
    this._domainTenant = value;
    this.store.dispatch(new SetTenant(value));
  }

  get domainTenant() {
    return this._domainTenant;
  }

  isTenantBoxVisible = true;

  apiName = 'abp';

  constructor(private restService: RestService, private store: Store) {}

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

  findTenantById(id: string, headers: ABP.Dictionary<string>): Observable<FindTenantResultDto> {
    return this.restService.request(
      { url: `/api/abp/multi-tenancy/tenants/by-id/${id}`, method: 'GET', headers },
      { apiName: this.apiName },
    );
  }
}
