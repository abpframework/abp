import { Signal, signal } from '@angular/core';
import { AuthErrorEvent, AuthErrorFilter } from '../models';

export abstract class AbstractAuthErrorFilter<T, E> {
  readonly #filters = signal<Array<T>>([]);
  filters = this.#filters.asReadonly();

  abstract get(id: string): T;
  abstract add(filter: T): void;
  abstract patch(item: Partial<T>): void;
  abstract remove(id: string): void;
  abstract run(event: E): boolean;
}

export class AuthErrorFilterService<
  T = AuthErrorEvent,
  E = AuthErrorFilter,
> extends AbstractAuthErrorFilter<T, E> {
  get(id: string): T {
    throw new Error('Import AbpOAuthModule from  @abp/ng.oauth or custom implementation');
  }
  add(filter: T): void {
    throw new Error('Import AbpOAuthModule from  @abp/ng.oauth or custom implementation');
  }
  patch(item: Partial<T>): void {
    throw new Error('Import AbpOAuthModule from  @abp/ng.oauth or custom implementation');
  }
  remove(id: string): void {
    throw new Error('Import AbpOAuthModule from  @abp/ng.oauth or custom implementation');
  }
  run(event: E): boolean {
    throw new Error('Import AbpOAuthModule from  @abp/ng.oauth or custom implementation');
  }
}
