import { Injectable, inject } from '@angular/core';
import { AuthConfig, OAuthService } from "angular-oauth2-oidc";
import compare from 'just-compare';
import { filter, map } from 'rxjs/operators';
import { ABP, EnvironmentService, CORE_OPTIONS } from '@abp/ng.core';

@Injectable({
  providedIn: 'root',
})
export class OAuthConfigurationHandler {
  private oAuthService = inject(OAuthService);
  private environmentService = inject(EnvironmentService);
  private options = inject<ABP.Root>(CORE_OPTIONS);

  constructor() {
    this.listenToSetEnvironment();
  }

  private listenToSetEnvironment() {
    this.environmentService
      .createOnUpdateStream(state => state)
      .pipe(
        map(environment => environment.oAuthConfig as AuthConfig),
        filter(config => !compare(config, this.options.environment.oAuthConfig)),
      )
      .subscribe((config) => {
        this.oAuthService.configure(config);
      });
  }
}
