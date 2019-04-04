import axios from '@/libs/api.request'

export const getBaseItems = (data) => {
  return axios.request({
    url: 'baseManagement/baseItem',
    method: 'get',
    params:data
  })
}


export const deleteBaseItem = (data) => {
    return axios.request({
      url: 'baseManagement/baseItem?id='+data.id,
      method: 'delete',
    })
  }

export const createBaseItem = (data) => {
    return axios.request({
      url: 'baseManagement/baseItem',
      method: 'post',
      data
    })
  }
  
  export const loadBaseItem = (data) => {
    return axios.request({
      url: 'baseManagement/baseItem/' + data.id,
      method: 'get'
    })
  }
  
  export const editBaseItem = (data) => {
    return axios.request({
      url: 'baseManagement/baseItem/'+data.id,
      method: 'put',
      data
    })
  }


  export const getViewTrees = (data) => {
    return axios.request({
      url: 'baseManagement/baseItem/getViewTrees',
      method: 'get'
      ,params:data
    })
  }