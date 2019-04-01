import HttpRequest from '@/libs/axios'
import config from '@/config'
const baseUrl = process.env.NODE_ENV === 'development' ? config.baseUrl.dev : config.baseUrl.pro
const defaultPrefix = config.baseUrl.defaultPrefix;
const axios = new HttpRequest(baseUrl,defaultPrefix)
export default axios
