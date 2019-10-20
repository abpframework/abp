import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { IdentityState } from '../states/identity.state';

@Injectable({
  providedIn: 'root',
})
export class IdentityStateService {
  constructor(private store: Store) {}

  getRoles() {
    return this.store.selectSnapshot(IdentityState.getRoles);
  }
  getRolesTotalCount() {
    return this.store.selectSnapshot(IdentityState.getRolesTotalCount);
  }
  getUsers() {
    return this.store.selectSnapshot(IdentityState.getUsers);
  }
  getUsersTotalCount() {
    return this.store.selectSnapshot(IdentityState.getUsersTotalCount);
  }
}
