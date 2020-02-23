import axios from '@/libs/api.request'

export const getUserList = (data) => {
  return axios.request({
    url: 'identity/users',
    method: 'get',
    params: data
  })
}

// createUser
export const createUser = (data) => {
  return axios.request({
    url: 'identity/users',
    method: 'post',
    data
  })
}

//loadUser
export const loadUser = (data) => {
  return axios.request({
    url: 'identity/users/' + data.id,
    method: 'get'
  })
}

// editUser
export const editUser = (data) => {
  return axios.request({
    url: 'identity/users/' + data.id,
    method: 'put',
    data
  })
}

// delete user
export const deleteUser = (ids) => {
  return axios.request({
    url: 'identity/users/' + ids,
    method: 'delete'
  })
}



export const emailConfirmation = (data) => {
  return axios.request({
    url: 'identity/users/emailConfirmation',
    method: 'put',
    data
  })
}
