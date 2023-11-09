export interface AbpAuthResponse {
  access_token: string;
  id_token: string;
  token_type: string;
  expires_in: number;
  refresh_token: string;
  scope: string;
  state?: string;
  tenant_domain?: string;
}
