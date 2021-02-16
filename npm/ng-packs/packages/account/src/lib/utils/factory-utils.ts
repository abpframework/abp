import { Options } from '../models/options';

export function accountOptionsFactory(options: Options) {
  return {
    redirectUrl: '/',
    ...options,
  };
}
