import { Injector } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigGetAppConfiguration } from '../actions/config.actions';

export function getInitialData(injector: Injector) {
  const fn = function() {
    const store: Store = injector.get(Store);

    return store.dispatch(new ConfigGetAppConfiguration()).toPromise();
  };

  return fn;
}
