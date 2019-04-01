import axios from 'axios'
import store from '@/store'
import {
  getToken
} from '@/libs/util'
import {
  Message,
  Modal
} from 'iview'
import iView from 'iview'

// let token = getToken()
// axios.defaults.headers.common['Authorization'] = 'Bearer ' + token
// import { Spin } from 'iview'

const addErrorLog = errorInfo => {
  const {
    statusText,
    status,
    request: {
      responseURL
    }
  } = errorInfo
  let info = {
    type: 'ajax',
    code: status,
    mes: statusText,
    url: responseURL
  }
  // if (!responseURL.includes('save_error_logger'))
  //   store.dispatch('addErrorLog', info)
}

class HttpRequest {
  constructor(baseUrl = baseURL, defaultPrefix = defaultPrefix) {
    this.baseUrl = baseUrl
    this.defaultPrefix = defaultPrefix
    this.queue = {}
  }
  getInsideConfig() {
    const config = {
      baseURL: this.baseUrl,
      headers: {
        "Authorization": "Bearer " + getToken()
      }
    }
    return config
  }
  destroy(url) {
    delete this.queue[url]
    if (!Object.keys(this.queue).length) {
      // Spin.hide()
    }
  }

  showError(error, errorInfo) {
    let message = "接口服务错误,请稍候再试.";

    let statusCode = -1;
    if (error.response && error.response.status) {
      statusCode = error.response.status;
    }
    switch (statusCode) {
      case 401:
        message = "接口服务错误,原因:未授权的访问(没有权限或者登录已超时)";
        break;
      case 403:
        message = "接口服务错误,原因:[" + error.response.data.error.code+'('+error.response.data.error.message+')' + "]";
        break;
      case 500:
        message = "接口服务错误,原因:[" + error.response.statusText + "]";
        break;
      case -1:
        message = "网络出错,请检查你的网络或者服务是否可用<br />请求地址:[" + error.config.url + "]";
        break;
    }
    Modal.error({
      title: "错误提示",
      content: message,
      duration: 15,
      closable: false
    });
    iView.LoadingBar.finish();
  }

  interceptors(instance, url) {
    // 请求拦截
    instance.interceptors.request.use(config => {
      // 添加全局的loading...
      if (!Object.keys(this.queue).length) {
        // Spin.show() // 不建议开启，因为界面不友好
        iView.LoadingBar.start();
      }
      this.queue[url] = true
      return config
    }, error => {
      return Promise.reject(error)
    })
    // 响应拦截
    instance.interceptors.response.use(res => {
      iView.LoadingBar.finish();
      this.destroy(url)
      const {
        data,
        status
      } = res
      return {
        data,
        status
      }
    }, error => {
      this.destroy(url)
      let errorInfo = error.response
      if (!errorInfo) {
        const {
          request: {
            statusText,
            status
          },
          config
        } = JSON.parse(JSON.stringify(error))
        errorInfo = {
          statusText,
          status,
          request: {
            responseURL: config.url
          }
        }
      }
      addErrorLog(errorInfo)
      if (error.config.hideError == null || error.config.hideError == false) {
        this.showError(error);
      }


      iView.LoadingBar.finish();

      return Promise.reject(error)
    })
  }
  request(options) {
    const instance = axios.create()
    let withPrefix = true
    if (options.withPrefix !== undefined && options.withPrefix == false) {
      withPrefix = false
    }
    let url = options.url
    if (options.prefix !== undefined && options.prefix.length > 0) {
      url = options.prefix + options.url
    } else if (withPrefix) {
      url = this.defaultPrefix + options.url
    }
    options.url = url
    options = Object.assign(this.getInsideConfig(), options)
    this.interceptors(instance, options.url)
    return instance(options)
  }

}
export default HttpRequest
