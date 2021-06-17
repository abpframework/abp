import type { LoginResultType } from './login-result-type.enum';

export interface AbpLoginResult {
  result: LoginResultType;
  description?: string;
}

export interface UserLoginInfo {
  userNameOrEmailAddress: string;
  password: string;
  rememberMe: boolean;
}
