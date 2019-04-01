import { Modal } from 'iview'

const beforeClose = {
  before_close_normal: (resolve) => {
    Modal.confirm({
      title: '确定要关闭这一页吗',
      onOk: () => {
        console.log('close tab...')
        resolve(true)
        console.log('tab closed.')
      },
      onCancel: () => {
        resolve(false)
      }
    })
  }
}

export default beforeClose
