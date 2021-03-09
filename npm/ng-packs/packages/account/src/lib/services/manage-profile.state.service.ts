import { Injectable } from '@angular/core';
import { InternalStore, Profile } from '@abp/ng.core';
import { Observable } from 'rxjs';

export interface ManageProfileState {
  profile: Profile.Response;
}

@Injectable({ providedIn: 'root' })
export class ManageProfileStateService {
  private readonly store = new InternalStore({} as ManageProfileState);

  get createOnUpdateStream() {
    return this.store.sliceUpdate;
  }

  getProfile$(): Observable<Profile.Response> {
    return this.store.sliceState(state => state.profile);
  }

  getProfile(): Profile.Response {
    return this.store.state.profile;
  }

  setProfile(profile: Profile.Response) {
    this.store.patch({ profile });
  }
}
