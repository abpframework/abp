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
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
        @on-permission="handlePermission"
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
                      v-model="stores.user.query.Filter"
                      placeholder="输入关键字搜索..."
                      @on-search="loadUserList()"
                    ></Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
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
            <FormItem label="登录名" prop="userName">
              <Input v-model="formModel.fields.userName" placeholder="请输入登录名"/>
            </FormItem>
          </Col>
          <Col span="12" v-show="formModel.fields.id==''">
            <FormItem label="登录密码" prop="password">
              <Input type="password" v-model="formModel.fields.password" placeholder="请输入登录密码"/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="姓" prop="surname">
              <Input v-model="formModel.fields.surname" placeholder="请输入姓"/>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="名" prop="name">
              <Input v-model="formModel.fields.name" placeholder="请输入名"/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="email" prop="email">
              <Input type="text" v-model="formModel.fields.email" placeholder="请输入email"/>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="手机号" prop="phoneNumber">
              <Input type="text" v-model="formModel.fields.phoneNumber" placeholder="请输入手机号"/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="8" v-show="formModel.fields.id==''">
            <FormItem label="是否发送激活邮件" label-position="left">
              <i-switch
                size="large"
                v-model="formModel.fields.sendActivationEmail"
                :true-value="true"
                :false-value="false"
              >
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch>
            </FormItem>
          </Col>
          <Col span="8">
            <FormItem label="是否启用禁用" label-position="left">
              <i-switch
                size="large"
                v-model="formModel.fields.lockoutEnabled"
                :true-value="true"
                :false-value="false"
              >
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch>
            </FormItem>
          </Col>
          <Col span="8">
            <FormItem label="双身份验证" label-position="left">
              <i-switch
                size="large"
                v-model="formModel.fields.twoFactorEnabled"
                :true-value="true"
                :false-value="false"
              >
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch>
            </FormItem>
          </Col>
        </Row>
        <Row>
          <Col span="24">
            <FormItem label-position="left">
              <Input v-model="formModel.parentName" placeholder="请选择部门" :readonly="true">
                <Dropdown
                  slot="append"
                  trigger="custom"
                  :visible="formModel.visible"
                  placement="bottom-end"
                  @on-clickoutside="clickoutside"
                >
                  <Button type="primary" @click="formModel.visible=true">
                    选择...
                    <Icon type="ios-arrow-down"></Icon>
                  </Button>
                  <div class="text-left pad10" slot="list" style="min-width:540px;">
                    <Tree
                      ref="depTree"
                      show-checkbox
                      class="text-left dropdown-tree"
                      :data="formModel.organizationData"
                      @on-check-change="handleMenuTreeSelectChange"
                    ></Tree>
                  </div>
                </Dropdown>
              </Input>
            </FormItem>
          </Col>
        </Row>
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
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitUser">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>

    <Drawer
      title="角色权限配置"
      v-model="permissionModal.opened"
      width="600"
      :mask-closable="false"
      :mask="false"
      :styles="styles"
    >
      <Form ref="permissionRole" :model="permissionModal" label-position="left">
        <FormItem label="用户Id" prop="entityDisplayName" label-position="left">
          <Input v-model="permissionModal.entityDisplayName" disabled/>
        </FormItem>
        <FormItem>
          <Row :gutter="16">
            <Tabs type="card">
              <template v-for="item in permissionModal.groups">
                <TabPane :label="item.displayName" :name="item.name">
                  <Tree ref="tree" show-checkbox check-directly :data="item.permissions"></Tree>
                </TabPane>
              </template>
            </Tabs>
          </Row>
        </FormItem>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitPermission">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="permissionModal.opened = false">取 消</Button>
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
  saveUserRoles
} from "@/api/rbac/user";
import { loadRoleListByUserGuid, loadSimpleList } from "@/api/rbac/role";
import { editPermission, loadPermissionTree } from "@/api/rbac/permission";
import { getUserViewTrees, setOrganizations } from "@/api/rbac/organization";

export default {
  name: "rbac_user_page",
  components: {
    Tables
  },
  data() {
    return {
      providerName:"User",
      permissionModal: {
        opened: false,
        rolePermission: [],
        name: "",
        entityDisplayName: "",
        groups: []
      },
      formModel: {
        opened: false,
        title: "创建用户",
        mode: "create",
        selection: [],
        organizationIds: [],
        parentName: "",
        organizationData: [],
        visible: false,
        fields: {
          sendActivationEmail:false,
          id: "",
          password: "",
          userName: "",
          name: "",
          surname: "",
          email: "",
          phoneNumber: "",
          twoFactorEnabled: false,
          lockoutEnabled: true,
          roleNames: []
        },
        rules: {
          userName: [
            {
              type: "string",
              required: true,
              message: "请输入登录名,大于三个字符!",
              min: 3
            }
          ],
          email: [
            {
              type: "email",
              required: true,
              message: "请输入email格式,大于三个字符",
              min: 3
            }
          ],
          password: [
            {
              type: "string",
              required: true,
              message: "请输入密码!"
            }
          ]
        }
      },
      formAssignRole: {
        id: "",
        opened: false,
        ownedRoles: [],
        inited: false,
        roles: []
      },
      stores: {
        user: {
          query: {
            totalCount: 0,
            MaxResultCount: 20,
            SkipCount: 0,
            Filter: "",
            Sorting: "Id"
          },
          sources: {
            statusFormSources: [
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" }
            ]
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "登录名", key: "userName", width: 250, sortable: true },
            { title: "姓", key: "surname", width: 100 },
            { title: "名", key: "name", width: 100 },
            { title: "email", key: "email", width: 250 },
            { title: "手机号", key: "phoneNumber", width: 150 },
            {
              title: "创建时间",
              ellipsis: true,
              tooltip: true,
              key: "creationTime"
            },
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
                          icon: "ios-cog",
                          type: "primary"
                        },
                        on: {
                          click: () => {
                            vm.$emit("on-permission", params);
                            vm.$emit("input", params.tableData);
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
                        "配置权限"
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
      return this.formModel.selection.map(x => x.id);
    }
  },
  methods: {
    loadUserList() {
      getUserList(this.stores.user.query).then(res => {
        this.stores.user.data = res.data.items;
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
      var id = params.row.id;
      this.handleSwitchFormModeToEdit();
      this.handleResetFormUser();
      this.doLoadUser(id);
      this.loadUserRoleList(id);
      this.getUserViewTrees({ id: id }).then(res => {
        var nodes = this.$refs.depTree.getCheckedNodes();
        this.handleMenuTreeSelectChange(nodes);
      });
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadUserList();
    },
    handleShowCreateWindow() {
      this.formModel.fields.id = "";
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormUser();
      this.loadUserRoleList();
      this.getUserViewTrees(null);
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
      this.formModel.fields.roleNames = this.formAssignRole.ownedRoles;
      createUser(this.formModel.fields).then(res => {
        setOrganizations({
          userId: res.id,
          organizationIds: this.formModel.organizationIds
        }).then(r => {
          this.$Message.success("新增用户成功");
          this.handleCloseFormWindow();
          this.loadUserList();
        });
      });
    },
    doEditUser() {
      this.formModel.fields.roleNames = this.formAssignRole.ownedRoles;
      var that = this;
      editUser(this.formModel.fields).then(res => {
        setOrganizations({
          userId: res.data.id,
          organizationIds: that.formModel.organizationIds
        }).then(r => {
          this.$Message.success("修改成功");
          this.handleCloseFormWindow();
          this.loadUserList();
        });
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
    doLoadUser(id) {
      loadUser({ id: id }).then(res => {
        this.formModel.fields = res.data;
        this.formModel.fields.password = "t";
      });
    },
    handleDelete(params) {
      var that = this;
      deleteUser(params.row.id).then(res => {
        that.$Message.success("删除成功!");
        that.loadUserList();
        that.formModel.selection = [];
      });
    },
    handlePageChanged(page) {
      this.stores.user.query.SkipCount =
        (page - 1) * this.stores.user.query.MaxResultCount;
      this.loadUserList();
    },
    handlePageSizeChanged(MaxResultCount) {
      this.stores.user.query.MaxResultCount = MaxResultCount;
      this.loadUserList();
    },
    renderOwnedRoles(item) {
      return item.label;
    },
    handleChangeOwnedRolesChanged(newTargetKeys, direction, moveKeys) {
      this.formAssignRole.ownedRoles = newTargetKeys;
    },
    loadUserRoleList(id) {
      this.formAssignRole.roles = [];
      this.formAssignRole.ownedRoles = [];
      var defaults = [];
      loadSimpleList().then(res => {
        var result = [];
        for (var i = 0; i < res.data.length; i++) {
          result.push({
            key: res.data[i].name,
            label: res.data[i].name
          });
          if (res.data[i].isDefault == true) {
            defaults.push(res.data[i].name);
          }
        }
        this.formAssignRole.roles = result;
      });
      if (id != "" && id != undefined) {
        loadRoleListByUserGuid(id).then(res => {
          var roleIds = res.data.items.map(item => item.name);
          this.formAssignRole.ownedRoles = roleIds;
        });
      } else {
        this.formAssignRole.ownedRoles = defaults;
      }
    },
    handleSubmitPermission() {
      var that = this;
      var permissions = [];
      that.permissionModal.groups.forEach(r => {
        r.permissions.forEach(i => {
          permissions.push({
            name: i.name,
            isGranted: i.checked
          });
          i.children.forEach(j => {
            permissions.push({
              name: j.name,
              isGranted: j.checked
            });
          });
        });
      });

      editPermission(this.providerName, that.permissionModal.name, {
        permissions: permissions
      }).then(res => {
        this.$Message.success("配置权限成功!");
        this.permissionModal.opened = false;
      });
    },
    handlePermission(params) {
      this.permissionModal.opened = true;
      var that = this;
      loadPermissionTree({
        providerName:this.providerName,
        providerKey: params.row.id
      }).then(res => {
        that.permissionModal.name = params.row.id;
        that.permissionModal.entityDisplayName = res.data.entityDisplayName;
        that.permissionModal.rolePermission.length = 0;
        // that.permissionModal.rolePermission = that.dfsTreeData(res.data.groups);
        var newObj = [];
        res.data.groups.forEach(element => {
          element.permissions = that.dfsTreeData(element.permissions);
          newObj.push(element);
        });
        that.permissionModal.groups = newObj;
      });
    },
    isDisabled(isGranted, grantedProviders) {
      return (
        isGranted &&
        grantedProviders.filter(it => it.providerName != this.providerName).length > 0
      );
    },
    getShownName(item) {
      if (!this.isDisabled(item.isGranted,item.grantedProviders)) {
         return item.displayName;
      }
      return (
        item.displayName +
        "(" +
        item.grantedProviders
          .filter(it => it.providerName != this.providerName).map(r=>{return r.providerName})
          .join(",") +
        ")"
      );
    },
    dfsTreeData(permissions) {
      var newTrees = [];
      var that = this;
      var parentNames = permissions.filter(item => item.parentName == null);
      parentNames.forEach(item => {
        var treeData = {
          title: this.getShownName(item),
          expand: true,
          name: item.name,
          checked: item.isGranted,
          children: [],
          disabled: that.isDisabled(item.isGranted, item.grantedProviders)
        };
        var childrens = permissions.filter(it => it.parentName == item.name);
        childrens.forEach(r => {
          treeData.children.push({
            title: this.getShownName(r),
            name: r.name,
            checked: r.isGranted,
            expand: true,
            disabled: that.isDisabled(r.isGranted, r.grantedProviders)
          });
        });
        newTrees.push(treeData);
      });
      return newTrees;
    },
    getUserViewTrees(id) {
      var that = this;
      return getUserViewTrees(id).then(res => {
        that.formModel.organizationData = res.data;
      });
    },
    handleMenuTreeSelectChange(nodes) {
      var name = [];
      var ids = [];
      this.formModel.organizationIds.length = 0;
      nodes.forEach(r => {
        name.push(r.title);
        ids.push(r.guid);
      });
      this.formModel.organizationIds = ids;
      this.formModel.parentName = name.join(";");
    },
    clickoutside(event) {
      this.formModel.visible = false;
    }
  },
  mounted() {
    this.loadUserList();
  }
};
</script>

<style>
</style>
