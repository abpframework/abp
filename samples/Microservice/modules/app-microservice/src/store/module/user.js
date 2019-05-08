import {
  login,
  logout,
  getUserInfo,
  getAuthConfig,
  getMessage,
  getContentByMsgId,
  hasRead,
  removeReaded,
  restoreTrash,
} from '@/api/user'
import {
  setToken,
  getToken,
  setTagNavListInLocalstorage,
} from '@/libs/util'

import staticRouters from '@/router/static-routers'

export default {
  state: {
    userName: '',
    userGuid: '',
    user_type: -1,
    avatorImgPath: '',
    token: getToken(),
    access: '',
    hasGetInfo: false,
    unreadCount: 0,
    messageUnreadList: [],
    messageReadedList: [],
    messageTrashList: [],
    messageContentStore: {},
    pages: [],
    permissions: {},
    profile: {}
  },
  mutations: {
    setAvator(state, avatorPath) {
      state.avatorImgPath = avatorPath
    },
    setUserGuid(state, guid) {
      state.userGuid = guid
    },
    setUserType(state, type) {
      state.user_type = type
    },
    setUserName(state, name) {
      state.userName = name
    },
    setAccess(state, access) {
      state.access = access
    },
    //设置用户可以访问的页面编码列表
    setPages(state, pages) {
      state.pages = pages;
    },
    //设置用户可以访问页面的权限集合
    setPermissions(state, permissions) {
      state.permissions = permissions;
    },
    setToken(state, token) {
      state.token = token
      setToken(token)
    },
    setHasGetInfo(state, status) {
      state.hasGetInfo = status
    },
    setMessageCount(state, count) {
      state.unreadCount = count
    },
    setMessageUnreadList(state, list) {
      state.messageUnreadList = list
    },
    setMessageReadedList(state, list) {
      state.messageReadedList = list
    },
    setMessageTrashList(state, list) {
      state.messageTrashList = list
    },
    updateMessageContentStore(state, {
      msg_id,
      content
    }) {
      state.messageContentStore[msg_id] = content
    },
    moveMsg(state, {
      from,
      to,
      msg_id
    }) {
      const index = state[from].findIndex(_ => _.msg_id === msg_id)
      const msgItem = state[from].splice(index, 1)[0]
      msgItem.loading = false
      state[to].unshift(msgItem)
    }
  },
  getters: {
    messageUnreadCount: state => state.messageUnreadList.length,
    messageReadedCount: state => state.messageReadedList.length,
    messageTrashCount: state => state.messageTrashList.length
  },
  actions: {
    // 登录
    handleLogin({
      commit
    }, {
      userName,
      password
    }) {
      userName = userName.trim()
      return new Promise((resolve, reject) => {
        login({
          userName,
          password
        }).then(res => {
          const data = res.data
          if (data.code === 1) {
            commit('setToken', data.data.access_token)
          } else {

          }
          resolve(res)
        }).catch(err => {
          reject(err)
        })
      })
    },
    // 退出登录
    handleLogOut({
      state,
      commit
    }) {
      return new Promise((resolve, reject) => {
        // 如果登录需要请求接口，则使用如下(1)的ajax请求，否则使用(2)的本地退出方式
        // 1.远程接口退出登录方式
        // logout(state.token).then(() => {
        //   commit('setToken', '')
        //   commit('setAccess', [])
        //   commit('setPages', [])
        //   resolve()
        // }).catch(err => {
        //   reject(err)
        // })

        // 2.本地退出登录方式
        // 如果你的退出登录无需请求接口，则可以直接使用下面三行代码而无需使用logout调用接口
        commit('setToken', '')
        commit('setAccess', [])
        commit('setPages', [])
        commit('setPermissions', {})
        setTagNavListInLocalstorage([]);
        resolve()
      })
    },
    // 获取用户相关信息
    getUserInfo({
      state,
      commit
    }) {
      return new Promise((resolve, reject) => {
        try {
          getUserInfo(state.token).then(res => {
            const data = res.data
            // commit('setAvator', data.avator)
            commit('setUserName', data.userName)
            commit('setUserGuid', data.email);

            getAuthConfig(state.token).then(res=>{
                  commit('setPermissions', res.data)
            });
            // commit('setAccess', data.access)
            // commit('setPermissions', data.permissions)
            // commit("setUserType", data.user_type);
            commit('setHasGetInfo', true)
            resolve(data)
          }).catch(err => {
            reject(err)
          })
        } catch (error) {
          reject(error)
        }
      })
    },
    // 此方法用来获取未读消息条数，接口只返回数值，不返回消息列表
    getUnreadMessageCount({
      state,
      commit
    }) {
      getUnreadCount().then(res => {
        const {
          data
        } = res
        commit('setMessageCount', data)
      })
    },
    // 获取消息列表，其中包含未读、已读、回收站三个列表
    getMessageList({
      state,
      commit
    }) {
      return new Promise((resolve, reject) => {
        getMessage().then(res => {
          const {
            unread,
            readed,
            trash
          } = res.data.data
          commit('setMessageUnreadList', unread.sort((a, b) => new Date(b.create_time) - new Date(a.create_time)))
          commit('setMessageReadedList', readed.map(_ => {
            _.loading = false
            return _
          }).sort((a, b) => new Date(b.create_time) - new Date(a.create_time)))
          commit('setMessageTrashList', trash.map(_ => {
            _.loading = false
            return _
          }).sort((a, b) => new Date(b.create_time) - new Date(a.create_time)))
          resolve()
        }).catch(error => {
          reject(error)
        })
      })
    },
    // 根据当前点击的消息的id获取内容
    getContentByMsgId({
      state,
      commit
    }, {
      msg_id
    }) {
      return new Promise((resolve, reject) => {
        let contentItem = state.messageContentStore[msg_id]
        if (contentItem) {
          resolve(contentItem)
        } else {
          getContentByMsgId(msg_id).then(res => {
            const content = res.data.data
            commit('updateMessageContentStore', {
              msg_id,
              content
            })
            resolve(content)
          })
        }
      })
    },
    // 把一个未读消息标记为已读
    hasRead({
      state,
      commit
    }, {
      msg_id
    }) {
      return new Promise((resolve, reject) => {
        hasRead(msg_id).then(() => {
          commit('moveMsg', {
            from: 'messageUnreadList',
            to: 'messageReadedList',
            msg_id
          })
          commit('setMessageCount', state.unreadCount - 1)
          resolve()
        }).catch(error => {
          reject(error)
        })
      })
    },
    // 删除一个已读消息到回收站
    removeReaded({
      commit
    }, {
      msg_id
    }) {
      return new Promise((resolve, reject) => {
        removeReaded(msg_id).then(() => {
          commit('moveMsg', {
            from: 'messageReadedList',
            to: 'messageTrashList',
            msg_id
          })
          resolve()
        }).catch(error => {
          reject(error)
        })
      })
    },
    // 还原一个已删除消息到已读消息
    restoreTrash({
      commit
    }, {
      msg_id
    }) {
      return new Promise((resolve, reject) => {
        restoreTrash(msg_id).then(() => {
          commit('moveMsg', {
            from: 'messageTrashList',
            to: 'messageReadedList',
            msg_id
          })
          resolve()
        }).catch(error => {
          reject(error)
        })
      })
    }
  }
}
