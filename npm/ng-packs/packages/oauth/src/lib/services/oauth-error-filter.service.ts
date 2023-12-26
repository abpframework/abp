import { Injectable, signal } from '@angular/core';
import { OAuthErrorEvent } from 'angular-oauth2-oidc';
import { AbstractAuthErrorFilter, AuthErrorFilter } from '@abp/ng.core';

@Injectable({ providedIn: 'root' })
export class OAuthErrorFilterService extends AbstractAuthErrorFilter<
  AuthErrorFilter<OAuthErrorEvent>,
  OAuthErrorEvent
> {
  protected readonly _filters = signal<Array<AuthErrorFilter<OAuthErrorEvent>>>([]);
  readonly filters = this._filters.asReadonly();

  get(id: string): AuthErrorFilter<OAuthErrorEvent> {
    return this._filters().find(({ id: _id }) => _id === id);
  }

  add(filter: AuthErrorFilter<OAuthErrorEvent>): void {
    this._filters.update(items => [...items, filter]);
  }

  patch(item: Partial<AuthErrorFilter<OAuthErrorEvent>>): void {
    const _item = this.filters().find(({ id }) => id === item.id);
    if (!_item) {
      return;
    }

    Object.assign(_item, item);
  }

  remove(id: string): void {
    const item = this.filters().find(({ id: _id }) => _id === id);
    if (!item) {
      return;
    }

    this._filters.update(items => items.filter(({ id: _id }) => _id !== id));
  }

  run(event: OAuthErrorEvent): boolean {
    return this.filters()
      .filter(({ executable }) => !!executable)
      .map(({ execute }) => execute(event))
      .some(item => item);
  }
}
