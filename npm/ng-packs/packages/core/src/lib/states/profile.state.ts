import { Action, Selector, State, StateContext } from '@ngxs/store';
import { tap } from 'rxjs/operators';
import { ChangePassword, GetProfile, UpdateProfile } from '../actions/profile.actions';
import { Profile } from '../models/profile';
import { ProfileService } from '../services/profile.service';

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

  @Action(GetProfile)
  getProfile({ patchState }: StateContext<Profile.State>) {
    return this.profileService.get().pipe(
      tap(profile =>
        patchState({
          profile,
        }),
      ),
    );
  }

  @Action(UpdateProfile)
  updateProfile({ patchState }: StateContext<Profile.State>, { payload }: UpdateProfile) {
    return this.profileService.update(payload).pipe(
      tap(profile =>
        patchState({
          profile,
        }),
      ),
    );
  }

  @Action(ChangePassword)
  changePassword(_, { payload }: ChangePassword) {
    return this.profileService.changePassword(payload, true);
  }
}
