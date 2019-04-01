<template>
  <div>
    <h1>这是一个用于演示的页面</h1>
    <Button v-can="'create'" icon="md-create" type="primary" title="新增用户">新增(有权限才显示)</Button>
  </div>
</template>

<script>
export default {
  name: 'demo_page',
  data () {
    return {

    }
  }
}
</script>
