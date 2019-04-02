import axios from '@/libs/api.request'

export const getAuditLogs = (data) => {
  return axios.request({
    url: 'auditLogging/auditLog/all',
    method: 'get',
    params:data
  })
}

export const getAuditLogById = (data) => {
  return axios.request({
    url: 'auditLogging/auditLog/'+data.id,
    method: 'get'
  })
}


export const getauditLogActionById = (data) => {
  return axios.request({
    url: 'auditLogging/auditLogAction/'+data.id,
    method: 'get'
  })
}