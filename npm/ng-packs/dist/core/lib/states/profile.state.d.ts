import { StateContext } from '@ngxs/store';
import { ChangePassword, UpdateProfile } from '../actions/profile.actions';
import { Profile } from '../models/profile';
import { ProfileService } from '../services/profile.service';
export declare class ProfileState {
    private profileService;
    static getProfile({ profile }: Profile.State): Profile.Response;
    constructor(profileService: ProfileService);
    profileGet({ patchState }: StateContext<Profile.State>): import("rxjs").Observable<Profile.Response>;
    profileUpdate({ patchState }: StateContext<Profile.State>, { payload }: UpdateProfile): import("rxjs").Observable<Profile.Response>;
    changePassword(_: any, { payload }: ChangePassword): import("rxjs").Observable<null>;
}
