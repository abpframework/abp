<template>
  <Row :gutter="32">
    <Col span="16" class="tabs-card-primary" style="background: #fff;padding:16px;">
      <Tabs value="user_info" type="card">
        <TabPane label="用户基本信息" name="user_info">
          <Form
            :model="formModel"
            ref="formUser"
            :rules="formModelRules"
            label-position="top"
          >
            <Row :gutter="16">
              <Col span="24">
                <FormItem label="登录名" prop="userName">
                  <Input v-model="formModel.userName" placeholder="请输入登录名" :disabled="true"/>
                </FormItem>
              </Col>
            </Row>
            <Row :gutter="16">
              <Col span="12">
                <FormItem label="姓" prop="surname">
                  <Input v-model="formModel.surname" placeholder="请输入姓"/>
                </FormItem>
              </Col>
              <Col span="12">
                <FormItem label="名" prop="name">
                  <Input v-model="formModel.name" placeholder="请输入名"/>
                </FormItem>
              </Col>
            </Row>
            <Row :gutter="16">
              <Col span="12">
                <FormItem label="email" prop="email">
                  <Input type="text" v-model="formModel.email" placeholder="请输入email"/>
                </FormItem>
              </Col>
              <Col span="12">
                <FormItem label="手机号" prop="phoneNumber">
                  <Input type="text" v-model="formModel.phoneNumber" placeholder="请输入手机号"/>
                </FormItem>
              </Col>
            </Row>
          </Form>
          <div class="demo-drawer-footer">
            <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitUser">保 存</Button>
          </div>
        </TabPane>
        <TabPane label="修改密码" name="reset_pwd">
            <Form
            :model="formPassword"
            ref="formPassword"
            :rules="formPasswordRules"
            label-position="top"
          >
            <Row :gutter="16">
              <Col span="24">
                <FormItem label="旧密码" prop="currentPassword">
                  <Input v-model="formPassword.currentPassword" placeholder="请输入旧密码" type="password"/>
                </FormItem>
              </Col>
            </Row>
               <Row :gutter="16">
              <Col span="24">
                <FormItem label="新密码" prop="newPassword">
                  <Input v-model="formPassword.newPassword" placeholder="请输入新密码"  type="password"/>
                </FormItem>
              </Col>
            </Row>
          </Form>
          <div class="demo-drawer-footer">
            <Button icon="md-checkmark-circle" type="primary" @click="changePassword">保 存</Button>
          </div>
        </TabPane>
        <!-- <TabPane label="设置头像" name="set_profile">设置头像</TabPane> -->
      </Tabs>
    </Col>
  </Row>
</template>
<script>
import { getProfile, editProfile, changePassword } from "@/api/rbac/profile";

export default {
  data() {
    return {
      formModel: {
          userName: "",
          name: "",
          surname: "",
          email: "",
          phoneNumber: ""
      },
      formPassword:{
          currentPassword:"",
          newPassword:""
      },
      formPasswordRules:{
        currentPassword: [
                {
                    required: true,
                    message: "请输入旧密码",
                }
         ],  
        newPassword: [
                {
                     required: true,
                     message: "请输入新密码",
                }
         ],
      },
      formModelRules: {
        email: [
          {
            type: "email",
            required: true,
            message: "请输入email格式,大于三个字符",
            min: 3
          }
        ]
      }
    };
  },
  mounted: function() {
    getProfile().then(res => {
      this.formModel = res.data;
    });
  },
  methods: {
    handleSubmitUser() {
     this.$refs["formUser"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          editProfile(this.formModel).then(res=>{
            this.$Message.success("修改成功!");
        });
        }
      });
    },
    changePassword(){
        this.$refs["formPassword"].validate(valid => {
        if (valid) {
            changePassword(this.formPassword).then(res=>{
                this.$Message.success("修改密码成功!");
            });
        }
      });
    }
  }
};
</script>

<style>
.tabs-card-default > .ivu-tabs-card > .ivu-tabs-content {
  height: 120px;
  margin-top: -16px;
}

.tabs-card-default > .ivu-tabs-card > .ivu-tabs-content > .ivu-tabs-tabpane {
  background: #fff;
  padding: 16px;
}

.tabs-card-default > .ivu-tabs.ivu-tabs-card > .ivu-tabs-bar .ivu-tabs-tab {
  border-color: transparent;
}

.tabs-card-default > .ivu-tabs-card > .ivu-tabs-bar .ivu-tabs-tab-active {
  border-color: #fff;
}
.tabs-card-primary > .ivu-tabs.ivu-tabs-card > .ivu-tabs-bar .ivu-tabs-tab {
  border-radius: 0;
  background: #fff;
}
.tabs-card-primary
  > .ivu-tabs.ivu-tabs-card
  > .ivu-tabs-bar
  .ivu-tabs-tab-active {
  border-top: 1px solid #3399ff;
}
.tabs-card-primary
  > .ivu-tabs.ivu-tabs-card
  > .ivu-tabs-bar
  .ivu-tabs-tab-active:before {
  content: "";
  display: block;
  width: 100%;
  height: 1px;
  background: #3399ff;
  position: absolute;
  top: 0;
  left: 0;
}
</style>