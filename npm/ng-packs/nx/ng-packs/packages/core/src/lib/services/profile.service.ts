import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RestService } from './rest.service';
import { Profile, Rest } from '../models';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  apiName = 'AbpIdentity';

  constructor(private rest: RestService) {}

  get(): Observable<Profile.Response> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: '/api/identity/my-profile',
    };

    return this.rest.request<null, Profile.Response>(request, { apiName: this.apiName });
  }

  update(body: Profile.Response): Observable<Profile.Response> {
    const request: Rest.Request<Profile.Response> = {
      method: 'PUT',
      url: '/api/identity/my-profile',
      body,
    };

    return this.rest.request<Profile.Response, Profile.Response>(request, {
      apiName: this.apiName,
    });
  }

  changePassword(
    body: Profile.ChangePasswordRequest,
    skipHandleError: boolean = false,
  ): Observable<null> {
    const request: Rest.Request<Profile.ChangePasswordRequest> = {
      method: 'POST',
      url: '/api/identity/my-profile/change-password',
      body,
    };

    return this.rest.request<Profile.ChangePasswordRequest, null>(request, {
      skipHandleError,
      apiName: this.apiName,
    });
  }
}
