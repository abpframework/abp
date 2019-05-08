import _axios from 'axios'
import config from '@/config'
import axios from '@/libs/api.request'

const authUrl = process.env.NODE_ENV === 'development' ? config.authUrl.dev : config.authUrl.pro

export const login = ({
  userName,
  password
}) => {
  // return _axios.get(authUrl + '?username=' + userName + '&password=' + password)
  // return _axios.post('http://localhost:64999/api/account/token',{
  //   "userNameOrEmailAddress": userName,
  //   "password": password,
  //   "rememberMe": true,
  //   "tenanId": ""
  // });
 return axios.request({
    url: 'http://localhost:64999/api/account/token',
    method: 'post',
    data:{
      "userNameOrEmailAddress": userName,
      "password": password,
      "rememberMe": true,
      "tenanId": ""
    },
    withPrefix: false
  })
}

export const getUserInfo = (token) => {
  return axios.request({
    url: 'identity/profile',
    method: 'get',
    //是否在请求资源中添加资源的前缀
    withPrefix: false,  //设置为true或者不设置此属性，将默认添加配置文件config.baseUrl.defaultPrefix的前缀，如果设置下面这个属性[prefix]，默认配置文件中的默认前缀将不生效
    //请求资源的前缀重写
    prefix:"api/"    //设此属性权重最高，将覆盖配置文件[baseUrl.defaultPrefix]中的前缀，withPrefix对此属性不起作用(也就是说只要设置了此属性，都将在请求中添加设置的前缀)
  })
}

export const getAuthConfig=(token)=>{
  return axios.request({
    url: 'identity/abpApplication/getAuthConfig',
    method: 'get',
  })
}


export const logout = (token) => {
  return new Promise((resolve, reject) => {
    resolve()
  })
}

export const getUnreadCount = () => {
  return axios.request({
    url: 'message/count',
    hideError: false,
    method: 'get'
  })
}

export const getMessage = () => {
  return axios.request({
    url: 'message/init',
    method: 'get'
  })
}

export const getContentByMsgId = msg_id => {
  return axios.request({
    url: 'message/content/' + msg_id,
    method: 'get'
  })
}

export const hasRead = msg_id => {
  return axios.request({
    url: 'message/has_read/' + msg_id,
    method: 'get',
  })
}

export const removeReaded = msg_id => {
  return axios.request({
    url: 'message/remove_readed/' + msg_id,
    method: 'get'
  })
}

export const restoreTrash = msg_id => {
  return axios.request({
    url: 'message/restore/' + msg_id,
    method: 'get'
  })
}
