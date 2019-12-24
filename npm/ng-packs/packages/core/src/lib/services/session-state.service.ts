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

  dispatchSetLanguage(payload: string) {
    return this.store.dispatch(new SetLanguage(payload));
  }

  dispatchSetTenant(payload: ABP.BasicItem) {
    return this.store.dispatch(new SetTenant(payload));
  }
}
