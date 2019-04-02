import axios from '@/libs/api.request'

export const getUserList = (data) => {
  return axios.request({
    url: 'identity/identityUser',
    method: 'get',
    params:data
  })
}