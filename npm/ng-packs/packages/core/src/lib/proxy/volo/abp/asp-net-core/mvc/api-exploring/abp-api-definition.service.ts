import { Injectable } from '@angular/core';
import { RestService } from '../../../../../../services/rest.service';
import type {
  ApplicationApiDescriptionModel,
  ApplicationApiDescriptionModelRequestDto,
} from '../../../http/modeling/models';

@Injectable({
  providedIn: 'root',
})
export class AbpApiDefinitionService {
  apiName = 'abp';

  getByModel = (model: ApplicationApiDescriptionModelRequestDto) =>
    this.restService.request<any, ApplicationApiDescriptionModel>(
      {
        method: 'GET',
        url: '/api/abp/api-definition',
        params: { includeTypes: model.includeTypes },
      },
      { apiName: this.apiName },
    );

  constructor(private restService: RestService) {}
}
