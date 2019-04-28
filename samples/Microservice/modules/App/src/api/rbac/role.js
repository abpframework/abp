import axios from '@/libs/api.request'

export const getRoleList = (data) => {
  return axios.request({
    url: 'identity/identityRole',
    method: 'get',
    params:data
  })
}

// createRole
export const createRole = (data) => {
  return axios.request({
    url: 'identity/identityRole',
    method: 'post',
    data
  })
}

//loadRole
export const loadRole = (data) => {
  return axios.request({
    url: 'identity/identityRole/' + data.id,
    method: 'get'
  })
}

// editRole
export const editRole = (data) => {
  return axios.request({
    url: 'identity/identityRole/'+data.id,
    method: 'put',
    data
  })
}

// delete role
export const deleteRole = (ids) => {
  return axios.request({
    url: 'identity/identityRole/' + ids,
    method: 'delete'
  })
}

export const loadSimpleList=()=>{
  return axios.request({
    url: 'identity/identityRole/list',
    method: 'get'
  })
}

export const loadRoleListByUserGuid=(user_guid)=>{
  return axios.request({
    url: 'identity/identityUser/'+user_guid+'/roles',
    method: 'get'
  })
}