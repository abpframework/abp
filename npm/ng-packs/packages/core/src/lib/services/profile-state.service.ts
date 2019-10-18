import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { ProfileState } from '../states';

@Injectable({
  providedIn: 'root',
})
export class ProfileStateService {
  constructor(private store: Store) {}

  getProfile() {
    return this.store.selectSnapshot(ProfileState.getProfile);
  }
}
