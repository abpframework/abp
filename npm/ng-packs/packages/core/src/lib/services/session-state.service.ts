import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import {
  SetLanguage,
  SetRemember,
  SetTenant,
  ModifyOpenedTabCount,
} from '../actions/session.actions';
import { SessionState } from '../states';

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

  getSessionDetail() {
    return this.store.selectSnapshot(SessionState.getSessionDetail);
  }

  dispatchSetLanguage(...args: ConstructorParameters<typeof SetLanguage>) {
    return this.store.dispatch(new SetLanguage(...args));
  }

  dispatchSetTenant(...args: ConstructorParameters<typeof SetTenant>) {
    return this.store.dispatch(new SetTenant(...args));
  }

  dispatchSetRemember(...args: ConstructorParameters<typeof SetRemember>) {
    return this.store.dispatch(new SetRemember(...args));
  }

  dispatchModifyOpenedTabCount(...args: ConstructorParameters<typeof ModifyOpenedTabCount>) {
    return this.store.dispatch(new ModifyOpenedTabCount(...args));
  }
}
