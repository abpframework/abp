import { UnaryFunction } from 'rxjs';
import { Injector } from '@angular/core';

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
