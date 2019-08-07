import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Store } from '@ngxs/store';
import { IdentityGetRoles } from '../actions/identity.actions';
import { Identity } from '../models/identity';
import { IdentityState } from '../states/identity.state';

@Injectable()
export class RoleResolver implements Resolve<Identity.State> {
  constructor(private store: Store) {}

  resolve() {
    const roles = this.store.selectSnapshot(IdentityState.getRoles);
    return roles && roles.length ? null : this.store.dispatch(new IdentityGetRoles());
  }
}
