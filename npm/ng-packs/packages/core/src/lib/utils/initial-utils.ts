import { Injector } from '@angular/core';
import { Store } from '@ngxs/store';
import { GetAppConfiguration } from '../actions/config.actions';

export function getInitialData(injector: Injector) {
  const fn = function() {
    const store: Store = injector.get(Store);

    return store.dispatch(new GetAppConfiguration()).toPromise();
  };

  return fn;
}
