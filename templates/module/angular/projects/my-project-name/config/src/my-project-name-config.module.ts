import {makeEnvironmentProviders} from '@angular/core';
import { MY_PROJECT_NAME_ROUTE_PROVIDERS } from './providers/route.provider';

export function provideMyProjectNameConfig() {
  return makeEnvironmentProviders([MY_PROJECT_NAME_ROUTE_PROVIDERS])
}
