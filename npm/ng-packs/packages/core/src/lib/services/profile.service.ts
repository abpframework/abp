import { Injectable } from '@angular/core';
import { ChangePasswordInput, ProfileDto, UpdateProfileDto } from '../models/profile';
import { RestService } from './rest.service';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  apiName = 'AbpIdentity';

  changePassword = (input: ChangePasswordInput, skipHandleError = false) =>
    this.restService.request<ChangePasswordInput, void>(
      {
        method: 'POST',
        url: '/api/identity/my-profile/change-password',
        body: input,
      },
      { apiName: this.apiName, skipHandleError },
    );

  get = () =>
    this.restService.request<null, ProfileDto>(
      {
        method: 'GET',
        url: '/api/identity/my-profile',
      },
      { apiName: this.apiName },
    );

  update = (input: UpdateProfileDto) =>
    this.restService.request<UpdateProfileDto, ProfileDto>(
      {
        method: 'PUT',
        url: '/api/identity/my-profile',
        body: input,
      },
      { apiName: this.apiName },
    );

  constructor(private restService: RestService) {}
}
