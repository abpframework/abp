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

export type SetTokenResponseToStorageFn = (injector: Injector, tokenRes: unknown) => void;
