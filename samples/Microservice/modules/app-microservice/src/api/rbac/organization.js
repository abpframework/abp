import axios from '@/libs/api.request'

export const getOrganizations = (data) => {
  return axios.request({
    url: 'organization/abpOrganization',
    method: 'get',
    params:data
  })
}


export const deleteOrganization = (data) => {
    return axios.request({
      url: 'organization/abpOrganization?id='+data.id,
      method: 'delete',
    })
  }

export const createOrganization = (data) => {
    return axios.request({
      url: 'organization/abpOrganization',
      method: 'post',
      data
    })
  }
  
  export const loadOrganization = (data) => {
    return axios.request({
      url: 'organization/abpOrganization/' + data.id,
      method: 'get'
    })
  }
  
  export const editOrganization = (data) => {
    return axios.request({
      url: 'organization/abpOrganization/'+data.id,
      method: 'put',
      data
    })
  }


  export const getViewTrees = (data) => {
    return axios.request({
      url: 'organization/abpOrganization/getViewTrees',
      method: 'get'
      ,params:data
    })
  }

  export const getUserViewTrees = (data) => {
    return axios.request({
      url: 'organization/abpOrganization/getUserViewTrees',
      method: 'get'
      ,params:data
    })
  }

  export const setOrganizations = (data) => {
    return axios.request({
      url: 'organization/abpOrganization/setOrganizations',
      method: 'post',
      data
    })
  }