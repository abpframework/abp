<template>
  <div>
    <Card>
      <tables
        ref="tables"
        editable
        searchable
        :border="false"
        size="small"
        search-place="top"
        v-model="stores.user.data"
        :totalCount="stores.user.query.totalCount"
        :columns="stores.user.columns"
        @on-delete="handleDelete"
        @on-edit="handleEdit"
        @on-select="handleSelect"
        @on-selection-change="handleSelectionChange"
        @on-refresh="handleRefresh"
        :row-class-name="rowClsRender"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
        @on-assign="handleAssignRole"
      >
        <div slot="search">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.user.query.kw"
                      placeholder="输入关键字搜索..."
                      @on-search="handleSearchUser()"
                    >
                      <Select
                        slot="prepend"
                        v-model="stores.user.query.isDeleted"
                        @on-change="handleSearchUser"
                        placeholder="删除状态"
                        style="width:60px;"
                      >
                        <Option
                          v-for="item in stores.user.sources.isDeletedSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                      <Select
                        slot="prepend"
                        v-model="stores.user.query.status"
                        @on-change="handleSearchUser"
                        placeholder="用户状态"
                        style="width:60px;"
                      >
                        <Option
                          v-for="item in stores.user.sources.statusSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                    </Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <Button
                    class="txt-success"
                    icon="md-redo"
                    title="恢复"
                    @click="handleBatchCommand('recover')"
                  ></Button>
                  <Button
                    class="txt-danger"
                    icon="md-hand"
                    title="禁用"
                    @click="handleBatchCommand('forbidden')"
                  ></Button>
                  <Button
                    class="txt-success"
                    icon="md-checkmark"
                    title="启用"
                    @click="handleBatchCommand('normal')"
                  ></Button>
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增用户"
                >新增用户</Button>
              </Col>
            </Row>
          </section>
        </div>
      </tables>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="600"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form :model="formModel.fields" ref="formUser" :rules="formModel.rules" label-position="top">
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="登录名" prop="loginName">
              <Input v-model="formModel.fields.loginName" placeholder="请输入登录名"/>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="显示名" prop="displayName">
              <Input v-model="formModel.fields.displayName" placeholder="请输入显示名"/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="密码" prop="password">
              <Input type="password" v-model="formModel.fields.password" placeholder="请输入登录密码"/>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="用户类型">
              <Select v-model="formModel.fields.userType">
                <Option
                  v-for="item in stores.user.sources.userTypes"
                  :value="item.value"
                  :key="item.value"
                >{{ item.text }}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="用户状态" label-position="left">
              <!-- <RadioGroup v-model="formModel.fields.status" type="button">
                              <Radio v-for="item in stores.user.sources.statusFormSources" :label="item.text" :key="item.value"></Radio>
              </RadioGroup>-->
              <i-switch
                size="large"
                v-model="formModel.fields.status"
                :true-value="1"
                :false-value="0"
              >
                <span slot="open">正常</span>
                <span slot="close">禁用</span>
              </i-switch>
            </FormItem>
          </Col>
        </Row>
        <FormItem label="备注" label-position="top">
          <Input type="textarea" v-model="formModel.fields.desc" :rows="4" placeholder="用户备注信息"/>
        </FormItem>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitUser">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer
      title="用户角色分配"
      v-model="formAssignRole.opened"
      width="500"
      :mask-closable="true"
      :mask="true"
    >
      <Form>
        <FormItem>
          <Transfer
            :data="formAssignRole.roles"
            :target-keys="formAssignRole.ownedRoles"
            :render-format="renderOwnedRoles"
            :titles="['未获得的角色','已获得的角色']"
            @on-change="handleChangeOwnedRolesChanged"
          ></Transfer>
        </FormItem>
      </Form>
      <div class="demo-drawer-footer" style="margin-top:15px;">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSaveUserRoles">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formAssignRole.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>

<script>
import Tables from "_c/tables";
import {
  getUserList,
  createUser,
  loadUser,
  editUser,
  deleteUser,
  batchCommand,
  saveUserRoles
} from "@/api/rbac/user";
import { loadRoleListByUserGuid } from "@/api/rbac/role";
export default {
  name: "rbac_user_page",
  components: {
    Tables
  },
  data() {
    return {
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
        forbidden: { name: "forbidden", title: "禁用" },
        normal: { name: "normal", title: "启用" }
      },
      formModel: {
        opened: false,
        title: "创建用户",
        mode: "create",
        selection: [],
        fields: {
          guid: "",
          loginName: "",
          displayName: "",
          password: "",
          avatar: "",
          userType: 0,
          isLocked: 0,
          status: 1,
          isDeleted: 0,
          createdOn: null,
          createdByUserGuid: "",
          createdByUserName: "",
          modifiedOn: null,
          modifiedByUserGuid: "",
          modifiedByUserName: ""
        },
        rules: {
          loginName: [
            { type: "string", required: true, message: "请输入登录名", min: 3 }
          ],
          displayName: [],
          password: []
        }
      },
      formAssignRole: {
        userGuid: "",
        opened: false,
        ownedRoles: [],
        inited: false,
        roles: []
      },
      stores: {
        user: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            isDeleted: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: ""
              }
            ]
          },
          sources: {
            userTypes: [
              { value: 0, text: "超级管理员" },
              { value: 1, text: "管理员" },
              { value: 2, text: "普通用户" }
            ],
            isDeletedSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "正常" },
              { value: 1, text: "已删" }
            ],
            statusSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" }
            ],
            statusFormSources: [
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" }
            ]
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "登录名", key: "loginName", width: 250, sortable: true },
            { title: "显示名", key: "displayName", width: 250 },
            {
              title: "用户类型",
              key: "userType",
              render: (h, params) => {
                var userTypeText = "未知";
                switch (params.row.userType) {
                  case 0:
                    userTypeText = "超级管理员";
                    break;
                  case 1:
                    userTypeText = "管理员";
                    break;
                  case 2:
                    userTypeText = "普通用户";
                    break;
                }
                return h("p", userTypeText);
              }
            },
            {
              title: "状态",
              key: "status",
              align: "center",
              width: 120,
              render: (h, params) => {
                let status = params.row.status;
                let statusColor = "success";
                let statusText = "正常";
                switch (status) {
                  case 0:
                    statusText = "禁用";
                    statusColor = "default";
                    break;
                }
                return h(
                  "Tooltip",
                  {
                    props: {
                      placement: "top",
                      transfer: true,
                      delay: 500
                    }
                  },
                  [
                    //这个中括号表示是Tooltip标签的子标签
                    h(
                      "Tag",
                      {
                        props: {
                          //type: "dot",
                          color: statusColor
                        }
                      },
                      statusText
                    ), //表格列显示文字
                    h(
                      "p",
                      {
                        slot: "content",
                        style: {
                          whiteSpace: "normal"
                        }
                      },
                      statusText //整个的信息即气泡内文字
                    )
                  ]
                );
              }
            },
            {
              title: "创建时间",
              width: 120,
              ellipsis: true,
              tooltip: true,
              key: "createdOn"
            },
            { title: "创建者", key: "createdByUserName" },
            {
              title: "操作",
              align: "center",
              key: "handle",
              width: 150,
              className: "table-command-column",
              options: ["edit"],
              button: [
                (h, params, vm) => {
                  return h(
                    "Poptip",
                    {
                      props: {
                        confirm: true,
                        title: "你确定要删除吗?"
                      },
                      on: {
                        "on-ok": () => {
                          vm.$emit("on-delete", params);
                        }
                      }
                    },
                    [
                      h(
                        "Tooltip",
                        {
                          props: {
                            placement: "left",
                            transfer: true,
                            delay: 1000
                          }
                        },
                        [
                          h("Button", {
                            props: {
                              shape: "circle",
                              size: "small",
                              icon: "md-trash",
                              type: "error"
                            }
                          }),
                          h(
                            "p",
                            {
                              slot: "content",
                              style: {
                                whiteSpace: "normal"
                              }
                            },
                            "删除"
                          )
                        ]
                      )
                    ]
                  );
                },
                (h, params, vm) => {
                  return h(
                    "Tooltip",
                    {
                      props: {
                        placement: "left",
                        transfer: true,
                        delay: 1000
                      }
                    },
                    [
                      h("Button", {
                        props: {
                          shape: "circle",
                          size: "small",
                          icon: "md-create",
                          type: "primary"
                        },
                        on: {
                          click: () => {
                            vm.$emit("on-edit", params);
                            //vm.$emit("input", params.tableData);
                          }
                        }
                      }),
                      h(
                        "p",
                        {
                          slot: "content",
                          style: {
                            whiteSpace: "normal"
                          }
                        },
                        "编辑"
                      )
                    ]
                  );
                },
                (h, params, vm) => {
                  return h(
                    "Tooltip",
                    {
                      props: {
                        placement: "left",
                        transfer: true,
                        delay: 1000
                      }
                    },
                    [
                      h("Button", {
                        props: {
                          shape: "circle",
                          size: "small",
                          icon: "md-contacts",
                          type: "success"
                        },
                        on: {
                          click: () => {
                            vm.$emit("on-assign", params);
                          }
                        }
                      }),
                      h(
                        "p",
                        {
                          slot: "content",
                          style: {
                            whiteSpace: "normal"
                          }
                        },
                        "分配角色"
                      )
                    ]
                  );
                }
              ]
            }
          ],
          data: []
        }
      },
      styles: {
        height: "calc(100% - 55px)",
        overflow: "auto",
        paddingBottom: "53px",
        position: "static"
      }
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "创建用户";
      }
      if (this.formModel.mode === "edit") {
        return "编辑用户";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.guid);
    }
  },
  methods: {
    loadUserList() {
      getUserList(this.stores.user.query).then(res => {
        this.stores.user.data = res.data.data;
        this.stores.user.query.totalCount = res.data.totalCount;
      });
    },
    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    handleCloseFormWindow() {
      this.formModel.opened = false;
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
    },
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
      this.handleOpenFormWindow();
    },
    handleEdit(params) {
      this.handleSwitchFormModeToEdit();
      this.handleResetFormUser();
      this.doLoadUser(params.row.guid);
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadUserList();
    },
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormUser();
    },
    handleSubmitUser() {
      let valid = this.validateUserForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateUser();
        }
        if (this.formModel.mode === "edit") {
          this.doEditUser();
        }
      }
    },
    handleResetFormUser() {
      this.$refs["formUser"].resetFields();
    },
    doCreateUser() {
      createUser(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadUserList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditUser() {
      editUser(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadUserList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    validateUserForm() {
      let _valid = false;
      this.$refs["formUser"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadUser(guid) {
      loadUser({ guid: guid }).then(res => {
        this.formModel.fields = res.data.data;
      });
    },
    handleDelete(params) {
      this.doDelete(params.row.guid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteUser(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadUserList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    handleBatchCommand(command) {
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand(command);
        }
      });
    },
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(",")
      }).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadUserList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleSearchUser() {
      this.loadUserList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handlePageChanged(page) {
      this.stores.user.query.currentPage = page;
      this.loadUserList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.user.query.pageSize = pageSize;
      this.loadUserList();
    },
    renderOwnedRoles(item) {
      return item.label;
    },
    handleChangeOwnedRolesChanged(newTargetKeys, direction, moveKeys) {
      this.formAssignRole.ownedRoles = newTargetKeys;
    },
    loadUserRoleList(guid) {
      this.formAssignRole.roles = [];
      this.formAssignRole.ownedRoles = [];
      loadRoleListByUserGuid(guid).then(res => {
        var result = res.data.data;
        this.formAssignRole.roles = result.roles;
        this.formAssignRole.ownedRoles = result.assignedRoles;
      });
    },
    handleAssignRole(params) {
      this.formAssignRole.opened = true;
      this.formAssignRole.userGuid = params.row.guid;
      this.loadUserRoleList(params.row.guid);
    },
    handleSaveUserRoles() {
      var data = {
        userGuid: this.formAssignRole.userGuid,
        assignedRoles: this.formAssignRole.ownedRoles
      };
      saveUserRoles(data).then(res => {
        this.formAssignRole.opened = false;
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    }
  },
  mounted() {
    this.loadUserList();
  }
};
</script>

<style>
</style>
