import axios from '@/libs/api.request'

export const getPermissionList = (data) => {
  return axios.request({
    url: 'rbac/permission/list',
    method: 'post',
    data
  })
}

// create
export const createPermission = (data) => {
  return axios.request({
    url: 'rbac/permission/create',
    method: 'post',
    data
  })
}

//edit
export const loadPermission = (data) => {
  return axios.request({
    url: 'rbac/permission/edit/' + data.code,
    method: 'get'
  })
}

// edit submit
export const editPermission = (data) => {
  return axios.request({
    url: 'rbac/permission/edit',
    method: 'post',
    data
  })
}

// delete
export const deletePermission = (ids) => {
  return axios.request({
    url: 'rbac/permission/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'rbac/permission/batch',
    method: 'get',
    params: data
  })
}


//load role-permission tree
export const loadPermissionTree = (role_code) => {
  return axios.request({
    url: 'rbac/permission/permission_tree/' + role_code,
    method: 'get'
  })
}
