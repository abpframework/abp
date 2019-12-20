import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { ProfileState } from '../states';
import { Profile } from '../models';
import { GetProfile, UpdateProfile, ChangePassword } from '../actions';

@Injectable({
  providedIn: 'root',
})
export class ProfileStateService {
  constructor(private store: Store) {}

  getProfile() {
    return this.store.selectSnapshot(ProfileState.getProfile);
  }

  fetchProfile() {
    return this.store.dispatch(new GetProfile());
  }

  updateProfile(payload: Profile.Response) {
    return this.store.dispatch(new UpdateProfile(payload));
  }

  changePassword(payload: Profile.ChangePasswordRequest) {
    return this.store.dispatch(new ChangePassword(payload));
  }
}
