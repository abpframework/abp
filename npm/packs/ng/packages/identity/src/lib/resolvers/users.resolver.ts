import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Store } from '@ngxs/store';
import { IdentityGetUsers } from '../actions/identity.actions';
import { Identity } from '../models/identity';
import { IdentityState } from '../states/identity.state';

@Injectable()
export class UserResolver implements Resolve<Identity.State> {
  constructor(private store: Store) {}

  resolve() {
    const users = this.store.selectSnapshot(IdentityState.getUsers);
    return users && users.length ? null : this.store.dispatch(new IdentityGetUsers());
  }
}
