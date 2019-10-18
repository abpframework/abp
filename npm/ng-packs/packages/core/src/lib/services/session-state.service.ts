import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { SessionState } from '../states';

@Injectable()
export class SessionStateService {
  constructor(private store: Store) {}

  getLanguage() {
    return this.store.selectSnapshot(SessionState.getLanguage);
  }

  getTenant() {
    return this.store.selectSnapshot(SessionState.getTenant);
  }
}
