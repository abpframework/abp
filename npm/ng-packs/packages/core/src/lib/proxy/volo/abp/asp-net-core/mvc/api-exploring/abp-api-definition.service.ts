import { RestService } from '../../../../../../services';
import { Rest } from '../../../../../../models';
import { Injectable, inject } from '@angular/core';
import type { ApplicationApiDescriptionModel, ApplicationApiDescriptionModelRequestDto } from '../../../http/modeling/models';

@Injectable({
  providedIn: 'root',
})
export class AbpApiDefinitionService {
  private restService = inject(RestService);

  apiName = 'abp';
  

  getByModel = (model: ApplicationApiDescriptionModelRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApplicationApiDescriptionModel>({
      method: 'GET',
      url: '/api/abp/api-definition',
      params: { includeTypes: model.includeTypes },
    },
    { apiName: this.apiName,...config });
}
