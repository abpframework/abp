import type { ApplicationLocalizationDto, ApplicationLocalizationRequestDto } from './models';
import { RestService } from '../../../../../../services';
import { Rest } from '../../../../../../models';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AbpApplicationLocalizationService {
  apiName = 'abp';
  

  get = (input: ApplicationLocalizationRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApplicationLocalizationDto>({
      method: 'GET',
      url: '/api/abp/application-localization',
      params: { cultureName: input.cultureName, onlyDynamics: input.onlyDynamics },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
