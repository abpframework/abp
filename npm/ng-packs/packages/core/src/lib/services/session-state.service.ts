import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { SessionState } from '../states';
import { ABP } from '../models';
import { SetLanguage, SetTenant } from '../actions';

@Injectable({
  providedIn: 'root',
})
export class SessionStateService {
  constructor(private store: Store) {}

  getLanguage() {
    return this.store.selectSnapshot(SessionState.getLanguage);
  }

  getTenant() {
    return this.store.selectSnapshot(SessionState.getTenant);
  }

  dispatchSetLanguage(...args: ConstructorParameters<typeof SetLanguage>) {
    return this.store.dispatch(new SetLanguage(...args));
  }

  dispatchSetTenant(...args: ConstructorParameters<typeof SetTenant>) {
    return this.store.dispatch(new SetTenant(...args));
  }
}
