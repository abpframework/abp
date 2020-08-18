import { ExtensibleObject } from './dtos';

export namespace Profile {
  export interface State {
    profile: Response;
  }

  export interface Response extends Partial<ExtensibleObject> {
    userName: string;
    email: string;
    name: string;
    surname: string;
    phoneNumber: string;
    /**
     * Following 4 fields are added as optional (for backward compatibility) on 17.08.2020
     * Also, this interface extends `ExtensibleObject` as partial for extraProperties field.
     */
    isExternal?: boolean;
    hasPassword?: boolean;
    emailConfirmed?: boolean;
    phoneNumberConfirmed?: boolean;
  }

  export interface ChangePasswordRequest {
    currentPassword: string;
    newPassword: string;
  }
}
