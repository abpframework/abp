import axios from '@/libs/api.request'

export const getRoleList = (data) => {
  return axios.request({
    url: 'identity/roles',
    method: 'get',
    params: data
  })
}

// createRole
export const createRole = (data) => {
  return axios.request({
    url: 'identity/roles',
    method: 'post',
    data
  })
}

//loadRole
export const loadRole = (data) => {
  return axios.request({
    url: 'identity/roles/' + data.id,
    method: 'get'
  })
}

// editRole
export const editRole = (data) => {
  return axios.request({
    url: 'identity/roles/' + data.id,
    method: 'put',
    data
  })
}

// delete role
export const deleteRole = (ids) => {
  return axios.request({
    url: 'identity/roles/' + ids,
    method: 'delete'
  })
}

export const loadSimpleList = () => {
  return axios.request({
    url: 'identity/roles',
    method: 'get'
  })
}

export const loadRoleListByUserGuid = (user_guid) => {
  return axios.request({
    url: 'identity/users/' + user_guid + '/roles',
    method: 'get'
  })
}