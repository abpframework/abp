import { StateContext } from '@ngxs/store';
import { ProfileChangePassword, ProfileUpdate } from '../actions/profile.actions';
import { Profile } from '../models/profile';
import { ProfileService } from '../services/profile.service';
export declare class ProfileState {
    private profileService;
    static getProfile({ profile }: Profile.State): Profile.Response;
    constructor(profileService: ProfileService);
    profileGet({ patchState }: StateContext<Profile.State>): import("rxjs").Observable<Profile.Response>;
    profileUpdate({ patchState }: StateContext<Profile.State>, { payload }: ProfileUpdate): import("rxjs").Observable<Profile.Response>;
    changePassword(_: any, { payload }: ProfileChangePassword): import("rxjs").Observable<null>;
}
