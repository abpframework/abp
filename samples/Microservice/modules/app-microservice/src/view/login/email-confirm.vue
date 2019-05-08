<style lang="less">
@import "./login.less";
</style>

<template>
  <div>
    <div class="bg bg-blur"></div>
    <div class="content content-front">
      <div class="login">
        <div class="login-con">
          <Card icon="log-in" title="欢迎激活" :bordered="false">
            <div class="form-con">
              <login-form
                @on-success-valid="handleSubmit"
                :processing="processing"
                :loading="loading"
              ></login-form>
            </div>
          </Card>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import LoginForm from "_c/login-form";
import { mapActions } from "vuex";
import {emailConfirmation} from '@/api/rbac/user.js';

export default {
  components: {
    LoginForm
  },
  data () {
    return {
      processing: false,
      loading: false
    };
  },
  mounted:function(){
    var query=this.$route.query
      emailConfirmation({
          userId:query.userId,
          confirmationCode:query.confirmationCode
      }).then(res=>{
             this.$Message.loading({
              duration: 0,
              closable: false,
              content: '激活成功！您可使用邮件或登录名进行登录!'
            });
           setTimeout(() => {
                this.$router.push({
                  name: "login"
                });
                setTimeout(() => {
                  this.$Message.destroy();
                }, 1000);
              }, 1500);
      });
  },
  methods: {
    ...mapActions(["handleLogin", "getUserInfo"]),
    handleSubmit ({ userName, password }) {
      var target = this;
      this.loading = true;
      this.handleLogin({ userName, password })
        .then(res => {
          if (res.data.code == 1) {
            this.processing = true;
            this.$Message.loading({
              duration: 0,
              closable: false,
              content: "用户信息验证成功,正在登录系统..."
            });
            this.getUserInfo().then(res => {
              setTimeout(() => {
                this.$router.push({
                  name: "home"
                });
                setTimeout(() => {
                  this.$Message.destroy();
                }, 1000);
              }, 1500);
            });
          } else {
            this.processing = false;
            this.loading = false;
            this.$Message.error(res.data.data);
          }
        })
    }
  }
};
</script>

<style>
.demo-spin-icon-load {
  animation: ani-demo-spin 1s linear infinite;
}
.content {
  color: #ffffff;
  font-size: 40px;
}
.bg {
  background: url('../../assets/images/login-bg.jpg');
  height: 100%;
  text-align: center;
  line-height: 100%;
  position: absolute;
}
.bg-blur {
  float: left;
  width: 100%;
  background-repeat: no-repeat;
  background-position: center;
  background-size: cover;
  -webkit-filter: blur(1px);
  -moz-filter: blur(1px);
  -o-filter: blur(1px);
  -ms-filter: blur(1px);
  filter: blur(1px);
}
.content-front {
  position: absolute;
  left: 10px;
  right: 10px;
  height: 100%;
  line-height: 100%;
}
</style>
