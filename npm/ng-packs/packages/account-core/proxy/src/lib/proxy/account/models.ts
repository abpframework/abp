import type { ExtensibleObject } from '@abp/ng.core';

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
