import { Inject, Injectable } from '@angular/core';
import { Actions, ofActionSuccessful } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import compare from 'just-compare';
import { filter, map } from 'rxjs/operators';
import { SetEnvironment } from '../actions/config.actions';
import { ABP } from '../models/common';
import { CORE_OPTIONS } from '../tokens/options.token';

@Injectable({
  providedIn: 'root',
})
export class OAuthConfigurationHandler {
  constructor(
    private actions: Actions,
    private oAuthService: OAuthService,
    @Inject(CORE_OPTIONS) private options: ABP.Root,
  ) {
    this.listenToSetEnvironment();
  }

  private listenToSetEnvironment() {
    this.actions
      .pipe(ofActionSuccessful(SetEnvironment))
      .pipe(
        map(({ environment }: SetEnvironment) => environment.oAuthConfig),
        filter(config => !compare(config, this.options.environment.oAuthConfig)),
      )
      .subscribe(config => {
        this.oAuthService.configure(config);
      });
  }
}
