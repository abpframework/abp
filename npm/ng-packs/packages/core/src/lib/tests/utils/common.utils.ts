import { AbstractType, InjectionToken, Injector, Type } from '@angular/core';
import { Subject } from 'rxjs';

export const mockActions = new Subject();

export class DummyInjector extends Injector {
  constructor(public payload: { [key: string]: any }) {
    super();
  }
  get<T>(
    token: Type<T> | InjectionToken<T> | AbstractType<T>,
    notFoundValue?: T,
    flags?: { optional: true; host: true; skipSelf: true; self: true },
  ): T;
  get(token: any, notFoundValue?: any): any;
  get(token, notFoundValue?, flags?: { optional: true; host: true; skipSelf: true }): any {
    return this.payload[token.name || token._desc || token];
  }
}
