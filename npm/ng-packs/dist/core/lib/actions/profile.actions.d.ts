import { Profile } from '../models';
export declare class ProfileGet {
    static readonly type = "[Profile] Get";
}
export declare class ProfileUpdate {
    payload: Profile.Response;
    static readonly type = "[Profile] Update";
    constructor(payload: Profile.Response);
}
export declare class ProfileChangePassword {
    payload: Profile.ChangePasswordRequest;
    static readonly type = "[Profile] Change Password";
    constructor(payload: Profile.ChangePasswordRequest);
}
