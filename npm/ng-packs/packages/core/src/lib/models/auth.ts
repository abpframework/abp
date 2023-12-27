import { Injector } from '@angular/core';
import { UnaryFunction } from 'rxjs';
import { AuthErrorEvent } from './auth-events';

export interface LoginParams {
  username: string;
  password: string;
  rememberMe?: boolean;
  redirectUrl?: string;
}

export type PipeToLoginFn = (
  params: Pick<LoginParams, 'redirectUrl' | 'rememberMe'>,
  injector: Injector,
) => UnaryFunction<any, any>;
/**
 * @deprecated The interface should not be used anymore.
 */
export type SetTokenResponseToStorageFn<T = any> = (tokenRes: T) => void;
export type CheckAuthenticationStateFn = (injector: Injector) => void;

export interface AuthErrorFilter<T = AuthErrorEvent> {
  id: string;
  executable: boolean;
  execute: (event: T) => boolean;
}
