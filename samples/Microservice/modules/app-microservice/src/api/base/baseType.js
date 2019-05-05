import axios from '@/libs/api.request'

export const getBaseTypes = (data) => {
  return axios.request({
    url: 'baseManagement/baseType',
    method: 'get',
    params:data
  })
}


export const deleteBaseType = (data) => {
    return axios.request({
      url: 'baseManagement/baseType?id='+data.id,
      method: 'delete',
    })
  }

export const createBaseType = (data) => {
    return axios.request({
      url: 'baseManagement/baseType',
      method: 'post',
      data
    })
  }
  
  export const loadBaseType = (data) => {
    return axios.request({
      url: 'baseManagement/baseType/' + data.id,
      method: 'get'
    })
  }
  
  export const editBaseType = (data) => {
    return axios.request({
      url: 'baseManagement/baseType/'+data.id,
      method: 'put',
      data
    })
  }


  export const getBaseTypeViewTrees = (data) => {
    return axios.request({
      url: 'baseManagement/baseType/getViewTrees',
      method: 'get'
      ,params:data
    })
  }