import { Profile } from '../models';
export declare class GetProfile {
    static readonly type = "[Profile] Get";
}
export declare class UpdateProfile {
    payload: Profile.Response;
    static readonly type = "[Profile] Update";
    constructor(payload: Profile.Response);
}
export declare class ChangePassword {
    payload: Profile.ChangePasswordRequest;
    static readonly type = "[Profile] Change Password";
    constructor(payload: Profile.ChangePasswordRequest);
}
