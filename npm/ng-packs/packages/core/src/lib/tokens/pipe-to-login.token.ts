import { InjectionToken } from '@angular/core';
import { PipeToLoginFn } from '../models/auth';

export const PIPE_TO_LOGIN_FN_KEY = new InjectionToken<PipeToLoginFn>('PIPE_TO_LOGIN_FN_KEY');
