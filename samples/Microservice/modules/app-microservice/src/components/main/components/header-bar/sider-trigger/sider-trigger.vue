<template>
  <a @click="handleChange" type="text" :class="['sider-trigger-a', collapsed ? 'collapsed' : '']">
    <Icon :type="icon" :size="size"/>
  </a>
</template>
<script>
export default {
  name: "siderTrigger",
  props: {
    collapsed: Boolean,
    icon: {
      type: String,
      default: "navicon-round"
    },
    size: {
      type: Number,
      default: 26
    }
  },
  methods: {
    handleChange() {
      this.$emit("on-change", !this.collapsed);
      setTimeout(function(){
        var evt = window.document.createEvent("UIEvents");
        evt.initUIEvent("resize", true, false, window, 0);
        window.dispatchEvent(evt);
      },200);
    }
  }
};
</script>
<style lang="less">
@import "./sider-trigger.less";
</style>
