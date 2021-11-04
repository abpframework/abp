import type { ExtensibleObject } from '@abp/ng.core';

export interface ChangePasswordInput {
  currentPassword?: string;
  newPassword: string;
}

export interface ProfileDto extends ExtensibleObject {
  userName?: string;
  email?: string;
  name?: string;
  surname?: string;
  phoneNumber?: string;
  isExternal: boolean;
  hasPassword: boolean;
  concurrencyStamp?: string;
}

export interface RegisterDto extends ExtensibleObject {
  userName: string;
  emailAddress: string;
  password: string;
  appName: string;
}

export interface ResetPasswordDto {
  userId?: string;
  resetToken: string;
  password: string;
}

export interface SendPasswordResetCodeDto {
  email: string;
  appName: string;
  returnUrl?: string;
  returnUrlHash?: string;
}

export interface UpdateProfileDto extends ExtensibleObject {
  userName?: string;
  email?: string;
  name?: string;
  surname?: string;
  phoneNumber?: string;
  concurrencyStamp?: string;
}
