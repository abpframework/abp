import { AuthErrorEvent, AuthErrorFilter } from '../models';

export abstract class AbstractAuthErrorFilter<T, E> {
  abstract get(id: string): T;
  abstract add(filter: T): void;
  abstract patch(item: Partial<T>): void;
  abstract remove(id: string): void;
  abstract run(event: E): boolean;
}

export class AuthErrorFilterService<
  T = AuthErrorFilter,
  E = AuthErrorEvent,
> extends AbstractAuthErrorFilter<T, E> {
  private warningMessage() {
    console.error('You should add @abp/ng-oauth packages or create your own auth packages.');
  }

  get(id: string): T {
    this.warningMessage();
    throw new Error('not implemented');
  }

  add(filter: T): void {
    this.warningMessage();
  }

  patch(item: Partial<T>): void {
    this.warningMessage();
  }

  remove(id: string): void {
    this.warningMessage();
  }

  run(event: E): boolean {
    this.warningMessage();
    throw new Error('not implemented');
  }
}
