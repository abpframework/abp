/**
 * Create comment node
 *
 * @private
 * @author https://stackoverflow.com/questions/43003976/a-custom-directive-similar-to-v-if-in-vuejs#43543814
 */
function commentNode(el, vnode) {
  const comment = document.createComment(' ')

  Object.defineProperty(comment, 'setAttribute', {
    value: () => undefined
  })

  vnode.text = ' '
  vnode.elm = comment
  vnode.isComment = true
  vnode.context = undefined
  vnode.tag = undefined
  vnode.data.directives = undefined

  if (vnode.componentInstance) {
    vnode.componentInstance.$el = comment
  }

  if (el.parentNode) {
    el.parentNode.replaceChild(comment, el)
  }
}

const hasPermission = {
  install(Vue, options) {
    Vue.directive('can', {
      bind(el, binding, vnode) {
        let checkPermission = vnode.context.$route.meta.checkPermission;
        if (!checkPermission) {
          return;
        }
        let permissionList = vnode.context.$route.meta.permissions;
        if(permissionList===undefined){
          // if(el.parentNode){
          //   el.parentNode.removeChild(el);
          // }
          el.disabled = true;
          commentNode(el,vnode)
        }
        if (permissionList && permissionList.length && !permissionList.includes(binding.value)) {
          // if(el.parentNode){
          //   console.log("run here 3...");
          //   el.parentNode.removeChild(el);
          // }
          el.disabled = true;
          commentNode(el,vnode)
        }
      }
    });
  }
};

export default hasPermission;
