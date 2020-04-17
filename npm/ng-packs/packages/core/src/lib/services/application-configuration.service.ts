import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Rest } from '../models/rest';
import { ApplicationConfiguration } from '../models/application-configuration';
import { RestService } from './rest.service';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states/config.state';

@Injectable({
  providedIn: 'root',
})
export class ApplicationConfigurationService {
  get apiName(): string {
    return this.store.selectSnapshot(ConfigState.getDeep('environment.application.name'));
  }

  constructor(private rest: RestService, private store: Store) {}

  getConfiguration(): Observable<ApplicationConfiguration.Response> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: '/api/abp/application-configuration',
    };

    return this.rest.request<null, ApplicationConfiguration.Response>(request, {
      apiName: this.apiName,
    });
  }
}
