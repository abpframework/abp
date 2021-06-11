import { AccountConfigOptions } from '../models/config-options';

export function accountConfigOptionsFactory(options: AccountConfigOptions) {
  return {
    redirectUrl: '/',
    ...options,
  };
}
