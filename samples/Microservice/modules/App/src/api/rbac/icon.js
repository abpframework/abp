import axios from '@/libs/api.request'

export const getIconList = (data) => {
  return axios.request({
    url: 'rbac/icon/list',
    method: 'post',
    data
  })
}

// create icon
export const createIcon = (data) => {
  return axios.request({
    url: 'rbac/icon/create',
    method: 'post',
    data
  })
}

//load icon
export const loadIcon = (data) => {
  return axios.request({
    url: 'rbac/icon/edit/' + data.id,
    method: 'get'
  })
}

// edit icon
export const editIcon = (data) => {
  return axios.request({
    url: 'rbac/icon/edit',
    method: 'post',
    data
  })
}

// delete icon
export const deleteIcon = (ids) => {
  return axios.request({
    url: 'rbac/icon/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'rbac/icon/batch',
    method: 'get',
    params: data
  })
}


// batch import
export const batchImportIcon = (data) => {
  return axios.request({
    url: 'rbac/icon/import',
    method: 'post',
    data
  })
}


// loadIconDataSource

// find icon data source by keyword
export const findIconDataSourceByKeyword = (data) => {
  return axios.request({
    url: 'rbac/icon/find_list_by_kw/' + data.keyword,
    method: 'get'
  })
}
