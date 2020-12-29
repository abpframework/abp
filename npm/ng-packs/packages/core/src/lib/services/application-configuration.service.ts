import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Rest } from '../models/rest';
import { ApplicationConfigurationDto } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/models';
import { RestService } from './rest.service';

/**
 * @deprecated Use AbpApplicationConfigurationService instead. To be deleted in v5.0.
 */
@Injectable({
  providedIn: 'root',
})
export class ApplicationConfigurationService {
  constructor(private rest: RestService) {}

  getConfiguration(): Observable<ApplicationConfigurationDto> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: '/api/abp/application-configuration',
    };

    return this.rest.request<null, ApplicationConfigurationDto>(request, {});
  }
}
