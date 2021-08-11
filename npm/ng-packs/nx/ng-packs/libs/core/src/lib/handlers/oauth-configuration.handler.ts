import { Inject, Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import compare from 'just-compare';
import { filter, map } from 'rxjs/operators';
import { ABP } from '../models/common';
import { EnvironmentService } from '../services/environment.service';
import { CORE_OPTIONS } from '../tokens/options.token';

@Injectable({
  providedIn: 'root',
})
export class OAuthConfigurationHandler {
  constructor(
    private oAuthService: OAuthService,
    private environmentService: EnvironmentService,
    @Inject(CORE_OPTIONS) private options: ABP.Root,
  ) {
    this.listenToSetEnvironment();
  }

  private listenToSetEnvironment() {
    this.environmentService
      .createOnUpdateStream(state => state)
      .pipe(
        map(environment => environment.oAuthConfig),
        filter(config => !compare(config, this.options.environment.oAuthConfig)),
      )
      .subscribe(config => {
        this.oAuthService.configure(config);
      });
  }
}
