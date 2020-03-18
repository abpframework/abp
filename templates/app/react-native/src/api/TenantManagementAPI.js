import api from './API';

export function getTenants(params = {}) {
  return api.get('/api/multi-tenancy/tenants', { params }).then(({ data }) => data);
}

export function createTenant(body) {
  return api.post('/api/multi-tenancy/tenants', body).then(({ data }) => data);
}

export function getTenantById(id) {
  return api.get(`/api/multi-tenancy/tenants/${id}`).then(({ data }) => data);
}

export function updateTenant(body, id) {
  return api.put(`/api/multi-tenancy/tenants/${id}`, body).then(({ data }) => data);
}

export function removeTenant(id) {
  return api.delete(`/api/multi-tenancy/tenants/${id}`).then(({ data }) => data);
}
