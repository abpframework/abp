import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApplicationConfiguration, Rest } from '../models';
import { RestService } from './rest.service';

@Injectable({
  providedIn: 'root',
})
export class ApplicationConfigurationService {
  constructor(private rest: RestService) {}

  getConfiguration(): Observable<ApplicationConfiguration.Response> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: '/api/abp/application-configuration',
    };

    return this.rest.request<null, ApplicationConfiguration.Response>(request);
  }
}
