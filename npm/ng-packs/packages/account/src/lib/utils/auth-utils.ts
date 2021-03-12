import { Injector } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ACCOUNT_CONFIG_OPTIONS } from '../tokens/config-options.token';

export function getRedirectUrl(injector: Injector) {
  const route = injector.get(ActivatedRoute);
  const options = injector.get(ACCOUNT_CONFIG_OPTIONS);
  return route.snapshot.queryParams.returnUrl || options.redirectUrl || '/';
}
