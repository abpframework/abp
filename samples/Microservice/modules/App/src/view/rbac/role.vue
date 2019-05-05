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
        v-model="stores.role.data"
        :totalCount="stores.role.query.totalCount"
        :columns="stores.role.columns"
        @on-delete="handleDelete"
        @on-permission="handlePermission"
        @on-edit="handleEdit"
        @on-refresh="loadRoleList"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
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
                      v-model="stores.role.query.Filter"
                      placeholder="输入关键字搜索..."
                      @on-search="handleSearchRole()"
                    ></Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button icon="md-refresh" title="刷新" @click="loadRoleList"></Button>
                </ButtonGroup>
                <Button
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增角色"
                >新增角色</Button>
              </Col>
            </Row>
          </section>
        </div>
      </tables>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="400"
      :mask-closable="false"
      :mask="false"
      :styles="styles"
    >
      <Form :model="formModel.fields" ref="formRole" :rules="formModel.rules" label-position="left">
        <FormItem label="角色名称" prop="name" label-position="left">
          <Input
            v-model="formModel.fields.name"
            :disabled="formModel.fields.isStatic"
            placeholder="请输入角色名称"
          />
        </FormItem>
        <FormItem label="是否默认" label-position="left">
          <i-switch
            size="large"
            v-model="formModel.fields.isDefault"
            :true-value="true"
            :false-value="false"
          >
            <span slot="open">是</span>
            <span slot="close">否</span>
          </i-switch>
        </FormItem>
        <FormItem label="是否公用" label-position="left">
          <i-switch
            size="large"
            v-model="formModel.fields.isPublic"
            :true-value="true"
            :false-value="false"
          >
            <span slot="open">是</span>
            <span slot="close">否</span>
          </i-switch>
        </FormItem>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitRole">保 存</Button>
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
        <FormItem label="角色名称" prop="entityDisplayName" label-position="left">
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
  getRoleList,
  createRole,
  loadRole,
  editRole,
  deleteRole
} from "@/api/rbac/role";

import { editPermission, loadPermissionTree } from "@/api/rbac/permission";

export default {
  name: "rbac_role_page",
  components: {
    Tables
  },
  data() {
    return {
      formModel: {
        opened: false,
        title: "创建角色",
        mode: "create",
        fields: {
          isStatic: false,
          concurrencyStamp: "",
          name: "",
          isDefault: false,
          isPublic: false
        },
        rules: {
          name: [
            {
              type: "string",
              required: true,
              message: "请输入角色名称",
              min: 2
            }
          ]
        }
      },
      permissionModal: {
        opened: false,
        rolePermission: [],
        name: "",
        entityDisplayName: "",
        groups: []
      },
      stores: {
        role: {
          query: {
            totalCount: 0,
            MaxResultCount: 20,
            SkipCount: 0,
            Filter: "",
            Sorting: "Id"
          },
          sources: {
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
            { title: "角色名称", key: "name", width: 250, sortable: true },
            {
              title: "静态角色",
              key: "isStatic",
              align: "center",
              width: 120,
              render: (h, params) => {
                let status = params.row.isStatic;
                let statusColor = "success";
                let statusText = "是";
                if (status == false) {
                  statusText = "否";
                  statusColor = "default";
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
                    h(
                      "Tag",
                      {
                        props: {
                          //type: "dot",
                          color: statusColor
                        }
                      },
                      statusText
                    ),
                    h(
                      "p",
                      {
                        slot: "content",
                        style: {
                          whiteSpace: "normal"
                        }
                      },
                      statusText
                    )
                  ]
                );
              }
            },
            {
              title: "默认角色",
              key: "isDefault",
              align: "center",
              width: 80,
              render: (h, params) => {
                let status = params.row.isDefault;
                let statusColor = "success";
                let statusText = "是";
                switch (status) {
                  case false:
                    statusText = "否";
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
                    h(
                      "Tag",
                      {
                        props: {
                          //type: "dot",
                          color: statusColor
                        }
                      },
                      statusText
                    ),
                    h(
                      "p",
                      {
                        slot: "content",
                        style: {
                          whiteSpace: "normal"
                        }
                      },
                      statusText
                    )
                  ]
                );
              }
            },
            {
              title: "公用角色",
              key: "isPublic",
              align: "center",
              render: (h, params) => {
                let status = params.row.isPublic;
                let statusColor = "success";
                let statusText = "是";
                switch (status) {
                  case false:
                    statusText = "否";
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
              title: "操作",
              align: "center",
              key: "handle",
              width: 180,
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
        return "创建角色";
      }
      if (this.formModel.mode === "edit") {
        return "编辑角色";
      }
      return "";
    }
  },
  methods: {
    loadRoleList() {
      getRoleList(this.stores.role.query).then(res => {
        this.stores.role.data = res.data.items;
        this.stores.role.query.totalCount = res.data.totalCount;
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
      this.handleResetFormRole();
      this.doLoadRole(params.row.id);
    },
    handleShowCreateWindow() {
      this.formModel.fields.isStatic = false;
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormRole();
    },
    handleSubmitRole() {
      let valid = this.validateRoleForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateRole();
        }
        if (this.formModel.mode === "edit") {
          this.doEditRole();
        }
      }
    },
    handleResetFormRole() {
      this.$refs["formRole"].resetFields();
    },
    doCreateRole() {
      createRole(this.formModel.fields).then(res => {
        this.$Message.success("新增角色成功");
        this.loadRoleList();

        this.handleCloseFormWindow();
      });
    },
    doEditRole() {
      editRole(this.formModel.fields).then(res => {
        this.$Message.success("修改角色成功");
        this.loadRoleList();
        this.handleCloseFormWindow();
      });
    },
    validateRoleForm() {
      let _valid = false;
      this.$refs["formRole"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
          _valid = false;
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadRole(id) {
      loadRole({ id: id }).then(res => {
        this.formModel.fields = res.data;
      });
    },
    handleDelete(params) {
      this.doDelete(params.row.id);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteRole(ids).then(res => {
        this.$Message.success("删除成功");
        this.loadRoleList();
      });
    },
    handleSearchRole() {
      this.stores.role.query.SkipCount = 0;
      this.loadRoleList();
    },
    handlePageChanged(page) {
      this.stores.role.query.SkipCount =
        (page - 1) * this.stores.role.query.MaxResultCount;
      this.loadRoleList();
    },
    handlePageSizeChanged(MaxResultCount) {
      this.stores.role.query.MaxResultCount = MaxResultCount;
      this.loadRoleList();
    },
    dfsTreeData(permissions) {
      var newTrees = [];
      var that = this;
      var parentNames = permissions.filter(item => item.parentName == null);
      parentNames.forEach(item => {
        var treeData = {
          title: item.displayName,
          expand: true,
          name: item.name,
          checked: item.isGranted,
          children: []
        };
        var childrens = permissions.filter(it => it.parentName == item.name);
        childrens.forEach(r => {
          treeData.children.push({
            title: r.displayName,
            name: r.name,
            checked: r.isGranted,
            expand: true
          });
        });
        newTrees.push(treeData);
      });
      return newTrees;
    },
    handlePermission(params) {
      this.permissionModal.opened = true;
      var that = this;
      loadPermissionTree({
        providerName: "Role",
        providerKey: params.row.name
      }).then(res => {
        that.permissionModal.name = params.row.name;
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

      editPermission("Role", that.permissionModal.name, {
        permissions: permissions
      }).then(res => {
        this.$Message.success("配置权限成功!");
      });
    }
  },
  mounted() {
    this.loadRoleList();
  }
};
</script>
