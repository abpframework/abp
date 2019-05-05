<template>
  <div>
    <Row>
      <i-col span="8">
        <Card title="Microservice/.NET Core交流群" shadow>
          <img class="qq-group-img" :src="qqFans" alt="">
          <p class="qq-join">QQ扫一扫二维码加群</p>
          <p class="qq-group-intro">本群为使用Microservice/.NET Core或者对Microservice/.NET Core感兴趣的开发者提供交流平台。在这里，解决你开发中的疑惑，共同进步。</p>
          <p class="addtional">
            本框架 <a href="https://github.com/lampo1024/Microservice" target="_blank">[Microservice]</a> 由 <a href="https://codedefault.com" target="_blank">码友网</a> 创建者<b>Rector</b>主导开发和维护。
          </p>
        </Card>
      </i-col>
    </Row>
  </div>
</template>

<script>
import qqFans from '@/assets/images/qq-group1.jpg'
export default {
  name: 'join_page',
  data () {
    return {
      qqFans
    }
  }
}
</script>

<style>
.qq-join{color: #999; text-align: center;}
.qq-group-img{
  display: block;
  margin: 0 auto;
  width: 240px;
}
.qq-group-intro{
  padding: 20px;
  font-size: 16px;
}
.addtional{padding: 20px; color: #777;}
</style>
