import { State, Action, StateContext, Selector } from '@ngxs/store';
import { ProfileGet, ProfileChangePassword, ProfileUpdate } from '../actions/profile.actions';
import { Profile } from '../models/profile';
import { ProfileService } from '../services/profile.service';
import { tap } from 'rxjs/operators';

@State<Profile.State>({
  name: 'ProfileState',
  defaults: {} as Profile.State,
})
export class ProfileState {
  @Selector()
  static getProfile({ profile }: Profile.State): Profile.Response {
    return profile;
  }

  constructor(private profileService: ProfileService) {}

  @Action(ProfileGet)
  profileGet({ patchState }: StateContext<Profile.State>) {
    return this.profileService.get().pipe(
      tap(profile =>
        patchState({
          profile,
        }),
      ),
    );
  }

  @Action(ProfileUpdate)
  profileUpdate({ patchState }: StateContext<Profile.State>, { payload }: ProfileUpdate) {
    return this.profileService.update(payload).pipe(
      tap(profile =>
        patchState({
          profile,
        }),
      ),
    );
  }

  @Action(ProfileChangePassword)
  changePassword(_, { payload }: ProfileChangePassword) {
    return this.profileService.changePassword(payload);
  }
}
