import axios from '@/libs/api.request'


// edit submit
export const editPermission = (providerName,providerKey,data) => {
  return axios.request({
    url: 'abp/permissions?providerName='+providerName+'&providerKey='+providerKey,
    method: 'put',
    data
  })
}

//load role-permission tree
export const loadPermissionTree = (data) => {
  return axios.request({
    url: 'abp/permissions',
    method: 'get'
    ,params:data
  })
}
