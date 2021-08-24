import type { ChangePasswordInput, ProfileDto, UpdateProfileDto } from './models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  apiName = 'AbpIdentity';

  changePassword = (input: ChangePasswordInput) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/identity/my-profile/change-password',
        body: input,
      },
      { apiName: this.apiName },
    );

  get = () =>
    this.restService.request<any, ProfileDto>(
      {
        method: 'GET',
        url: '/api/identity/my-profile',
      },
      { apiName: this.apiName },
    );

  update = (input: UpdateProfileDto) =>
    this.restService.request<any, ProfileDto>(
      {
        method: 'PUT',
        url: '/api/identity/my-profile',
        body: input,
      },
      { apiName: this.apiName },
    );

  constructor(private restService: RestService) {}
}
