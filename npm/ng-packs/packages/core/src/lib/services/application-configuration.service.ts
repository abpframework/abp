import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApplicationConfiguration } from '../models/application-configuration';
import { Rest } from '../models/rest';
import { EnvironmentService } from './environment.service';
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

    return this.rest.request<null, ApplicationConfiguration.Response>(request, {});
  }
}
