import { InjectionToken } from '@angular/core';
import { Config } from '../models';

export function environmentFactory(environment: Config.Environment) {
  return {
    ...environment,
  };
}

export function configFactory(config: Config.Requirements) {
  return {
    ...config,
  };
}

export const ENVIRONMENT = new InjectionToken('ENVIRONMENT');

export const CONFIG = new InjectionToken('CONFIG');
