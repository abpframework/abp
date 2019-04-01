import axios from '@/libs/api.request'

export const getUserList = (data) => {
  return axios.request({
    url: 'rbac/user/list',
    method: 'post',
    data
  })
}

// createUser
export const createUser = (data) => {
  return axios.request({
    url: 'rbac/user/create',
    method: 'post',
    data
  })
}

//loadUser
export const loadUser = (data) => {
  return axios.request({
    url: 'rbac/user/edit/' + data.guid,
    method: 'get'
  })
}

// editUser
export const editUser = (data) => {
  return axios.request({
    url: 'rbac/user/edit',
    method: 'post',
    data
  })
}

// delete user
export const deleteUser = (ids) => {
  return axios.request({
    url: 'rbac/user/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'rbac/user/batch',
    method: 'get',
    params: data
  })
}

// save user roles
export const saveUserRoles = (data) => {
  return axios.request({
    url: 'rbac/user/save_roles',
    method: 'post',
    data
  })
}
