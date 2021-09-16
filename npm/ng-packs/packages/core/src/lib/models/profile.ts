import { ExtensibleObject } from './dtos';

export interface ChangePasswordInput {
  currentPassword: string;
  newPassword: string;
}

export interface ProfileDto extends ExtensibleObject {
  userName: string;
  email: string;
  name: string;
  surname: string;
  phoneNumber: string;
  isExternal?: boolean;
  hasPassword?: boolean;
  emailConfirmed?: boolean;
  phoneNumberConfirmed?: boolean;
}

export interface UpdateProfileDto extends ExtensibleObject {
  userName: string;
  email: string;
  name: string;
  surname: string;
  phoneNumber: string;
}
