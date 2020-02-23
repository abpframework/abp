import axios from '@/libs/api.request'

export const getProfile = (data) => {
  return axios.request({
    url: 'identity/my-profile',
    method: 'get',
    params: data
  })
}

export const editProfile = (data) => {
  return axios.request({
    url: 'identity/my-profile',
    method: 'put',
    data
  })
}

export const changePassword = (data) => {
  return axios.request({
    url: 'identity/my-profile/changePassword',
    method: 'post',
    params: data
  })
}