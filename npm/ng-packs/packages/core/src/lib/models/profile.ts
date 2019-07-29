export namespace Profile {
  export interface State {
    profile: Response;
  }

  export interface Response {
    userName: string;
    email: string;
    name: string;
    surname: string;
    phoneNumber: string;
  }

  export interface ChangePasswordRequest {
    currentPassword: string;
    newPassword: string;
  }
}
