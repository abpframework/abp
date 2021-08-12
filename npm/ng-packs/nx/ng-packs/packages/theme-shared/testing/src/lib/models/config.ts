import { Validation } from '@ngx-validate/core';

export interface Config {
  validation?: Partial<Validation.Config>;
}
