import axios from '@/libs/api.request'

export const getProfile = (data) => {
  return axios.request({
    url: 'identity/profile',
    method: 'get',
    params:data
  })
}

export const editProfile = (data) => {
  return axios.request({
    url: 'identity/profile',
    method: 'put',
    data
  })
}

export const changePassword = (data) => {
  return axios.request({
    url: 'identity/profile/changePassword',
    method: 'post',
    params:data
  })
}