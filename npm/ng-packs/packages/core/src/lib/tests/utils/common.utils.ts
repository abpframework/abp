import { Observable, of, Subject } from 'rxjs';
import { Store } from '@ngxs/store';
import { AbstractType, InjectFlags, InjectionToken, Injector, Type } from '@angular/core';

export const mockActions = new Subject();
export const mockStore = ({
  selectSnapshot() {
    return true;
  },
  select(): Observable<any> {
    return of(null);
  },
} as unknown) as Store;

export class DummyInjector extends Injector {
  constructor(public payload: { [key: string]: any }) {
    super();
  }
  get<T>(
    token: Type<T> | InjectionToken<T> | AbstractType<T>,
    notFoundValue?: T,
    flags?: InjectFlags,
  ): T;
  get(token: any, notFoundValue?: any): any;
  get(token, notFoundValue?, flags?: InjectFlags): any {
    return this.payload[token.name || token];
  }
}
