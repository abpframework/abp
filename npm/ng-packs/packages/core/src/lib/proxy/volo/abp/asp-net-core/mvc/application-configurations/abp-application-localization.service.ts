import type { ApplicationLocalizationDto, ApplicationLocalizationRequestDto } from './models';
import { RestService } from '../../../../../../services/rest.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AbpApplicationLocalizationService {
  apiName = 'abp';

  get = (input: ApplicationLocalizationRequestDto) =>
    this.restService.request<any, ApplicationLocalizationDto>(
      {
        method: 'GET',
        url: '/api/abp/application-localization',
        params: { cultureName: input.cultureName, onlyDynamics: input.onlyDynamics },
      },
      { apiName: this.apiName },
    );

  constructor(private restService: RestService) {}
}
