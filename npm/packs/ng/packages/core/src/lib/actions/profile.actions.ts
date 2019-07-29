import { Profile } from '../models';

export class ProfileGet {
  static readonly type = '[Profile] Get';
}

export class ProfileUpdate {
  static readonly type = '[Profile] Update';
  constructor(public payload: Profile.Response) {}
}

export class ProfileChangePassword {
  static readonly type = '[Profile] Change Password';
  constructor(public payload: Profile.ChangePasswordRequest) {}
}
