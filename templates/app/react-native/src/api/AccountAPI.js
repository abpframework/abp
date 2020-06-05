import api from './API';
import { getEnvVars } from '../../Environment';

const { oAuthConfig } = getEnvVars();

export const login = ({ username, password }) => {
  // eslint-disable-next-line no-undef
  const formData = new FormData();
  formData.append('username', username);
  formData.append('password', password);
  formData.append('grant_type', 'password');
  formData.append('scope', `${oAuthConfig.scope} offline_access`);
  formData.append('client_id', oAuthConfig.clientId);
  formData.append('client_secret', oAuthConfig.clientSecret);

  return api({
    method: 'POST',
    url: '/connect/token',
    headers: { 'Content-Type': 'multipart/form-data' },
    data: formData,
    baseURL: oAuthConfig.issuer,
  }).then(({ data }) => data);
};

export const logout = () =>
  api({
    method: 'GET',
    url: '/api/account/logout',
  }).then(({ data }) => data);

export const getTenant = tenantName =>
  api({
    method: 'GET',
    url: `/api/abp/multi-tenancy/tenants/by-name/${tenantName}`,
  }).then(({ data }) => data);

export const getTenantById = tenantId =>
  api({
    method: 'GET',
    url: `/api/abp/multi-tenancy/tenants/by-id/${tenantId}`,
  }).then(({ data }) => data);
