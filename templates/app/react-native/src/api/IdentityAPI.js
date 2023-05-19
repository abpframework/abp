import api from './API';


export const getAllRoles = () => api.get('/api/identity/roles/all').then(({ data }) => data.items);

export const getUserRoles = id =>
  api.get(`/api/identity/users/${id}/roles`).then(({ data }) => data.items);

export const getUsers = (params = { maxResultCount: 10, skipCount: 0 }) =>
  api.get('/api/identity/users', { params }).then(({ data }) => data);

export const getUserById = id => api.get(`/api/identity/users/${id}`).then(({ data }) => data);

export const createUser = body => api.post('/api/identity/users', body).then(({ data }) => data);

export const updateUser = (body, id) =>
  api.put(`/api/identity/users/${id}`, body).then(({ data }) => data);

export const removeUser = id => api.delete(`/api/identity/users/${id}`);

export const getProfileDetail = () => api.get('/api/account/my-profile').then(({ data }) => data);

export const updateProfileDetail = body =>
  api.put('/api/account/my-profile', body).then(({ data }) => data);

export const changePassword = body =>
  api.post('/api/account/my-profile/change-password', body).then(({ data }) => data);
