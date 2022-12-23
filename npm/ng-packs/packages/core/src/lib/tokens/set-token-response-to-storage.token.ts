import { InjectionToken } from '@angular/core';
import { SetTokenResponseToStorageFn } from '../models';

export const SET_TOKEN_RESPONSE_TO_STORAGE_FN_KEY = new InjectionToken<SetTokenResponseToStorageFn>(
  'SET_TOKEN_RESPONSE_TO_STORAGE_FN_KEY',
);
