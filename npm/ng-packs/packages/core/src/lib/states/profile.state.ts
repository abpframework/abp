import { State, Action, StateContext, Selector } from '@ngxs/store';
import { GetProfile, ChangePassword, UpdateProfile } from '../actions/profile.actions';
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

  @Action(GetProfile)
  profileGet({ patchState }: StateContext<Profile.State>) {
    return this.profileService.get().pipe(
      tap(profile =>
        patchState({
          profile,
        }),
      ),
    );
  }

  @Action(UpdateProfile)
  profileUpdate({ patchState }: StateContext<Profile.State>, { payload }: UpdateProfile) {
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
