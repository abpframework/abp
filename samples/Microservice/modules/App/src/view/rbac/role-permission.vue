<template>
  <div>
    <Card>
      <Row :gutter="15">
        <Col span="4">
          <h3 style="background-color:#f3f3f3;margin-bottom:0; padding:5px 5px 5px 10px;position:relative;">角色列表
            <span style="display:inline-block;position:absolute;right:6px;top:4px;">
              <Tooltip content="刷新角色列表">
                  <Button shape="circle" size="small" icon="md-refresh" @click="handleReloadRoleListClick"></Button>
              </Tooltip>
            </span>
          </h3>
          <ul class="role-list-box">
            <li :class="{'active': r.selected}" v-for="r in stores.role.data" @click="handleRoleClicked(r)">{{r.name}}</li>
          </ul>
        </Col>
        <Col span="20">
          <div style="margin-bottom:10px;">
            <h3 style="background-color:#f3f3f3;margin-bottom:0;padding:5px 5px 5px 10px;">权限列表</h3>
            <span style="display:inline-block;position:absolute;right:0;top:1px;">
              <Button icon="md-checkmark-circle" type="primary" @click="handleSaveRolePermissions">保 存</Button>
            </span>
          </div>
          <Tree :data="stores.permissionTree.data" :render="renderContent" class="permission-tree-box"></Tree>
        </Col>
      </Row>
    </Card>
  </div>
</template>

<script>
import { loadSimpleList, assignPermission } from "@/api/rbac/role";
import { loadPermissionTree } from "@/api/rbac/permission";

export default {
  name: "rbac_role_permission_page",
  data() {
    return {
      selectedPermissions: [],
      currentRoleCode: "",
      stores: {
        role: {
          data: []
        },
        permissionTree: {
          data: []
        }
      },
      buttonProps: {
        type: "default",
        size: "small"
      }
    };
  },
  mounted() {
    this.loadRoles();
  },
  methods: {
    loadRoles() {
      loadSimpleList().then(response => {
        let data = response.data.data;
        for (var i = 0, len = data.length; i < len; i++) {
          data[i].selected = false;
        }
        this.stores.role.data = data;
      });
    },
    loadRolePermissionTree() {
      loadPermissionTree(this.currentRoleCode).then(response => {
        var data = response.data.data;
        this.stores.permissionTree.data = data.tree;
        this.selectedPermissions = data.selectedPermissions;
      });
    },
    handleRoleClicked(role) {
      for (var i = 0, len = this.stores.role.data.length; i < len; i++) {
        this.stores.role.data[i].selected = false;
      }
      this.currentRoleCode = role.code;
      role.selected = true;
      this.loadRolePermissionTree();
    },
    handleSaveRolePermissions() {
      var data = {
        roleCode: this.currentRoleCode,
        permissions: this.selectedPermissions
      };
      assignPermission(data).then(response => {
        var result = response.data;
        if (result.code == 200) {
          this.$Message.success(result.message);
        } else {
          this.$Message.warning(result.message);
        }
      });
    },
    handleReloadRoleListClick() {
      this.currentRoleCode = "";
      this.selectedPermissions = [];
      this.stores.permissionTree.data = [];
      this.loadRoles();
    },
    renderContent(h, { root, node, data }) {
      let showCheckAll = false;
      if (data.permissions && data.permissions.length > 0) {
        showCheckAll = true;
      }
      return h(
        "span",
        {
          style: {
            display: "inline-block",
            width: "100%"
          },
          class: "permission-tree-node"
        },
        [
          h("span", [
            h("Icon", {
              props: {
                type: "ios-paper-outline"
              },
              style: {
                marginRight: "5px"
              }
            }),
            h("span", data.title)
          ]),
          h(
            "span",
            {
              style: {
                display: "inline-block",
                float: "right",
                marginRight: "32px"
              }
            },
            [
              h(
                "div",
                {
                  style: {
                    display: "inline-block",
                    marginRight: "10px"
                  }
                },
                [
                  h(
                    "CheckboxGroup",
                    {
                      props: {
                        value: this.selectedPermissions,
                        style: {
                          display: "inline-block"
                        },
                        class: "permission-box"
                      },
                      on: {
                        "on-change": event => {
                          this.selectedPermissions = event;
                        }
                      }
                    },
                    (data.permissions || []).map(obj => {
                      return h(
                        "Checkbox",
                        {
                          props: {
                            label: obj.code
                          }
                        },
                        obj.name
                      );
                    })
                  )
                ]
              ),
              h(
                "Checkbox",
                {
                  props: {
                    label: "全选",
                    value: data.allAssigned
                  },
                  style: {
                    marginLeft: "15px",
                    color: "333",
                    fontWeight: "600",
                    visibility: showCheckAll ? "visible" : "hidden"
                  },
                  class: "permission-check-all",
                  on: {
                    "on-change": val => {
                      this.checkNodePermission(val, data.permissions, node);
                    }
                  }
                },
                "全选"
              )
            ]
          )
        ]
      );
    },
    checkNodePermission(checked, permissions, node) {
      node.allAssigned = checked;
      if (!permissions || permissions.length <= 0) {
        return;
      }
      if (checked) {
        for (let i = 0, len = permissions.length; i < len; i++) {
          var d = permissions[i];
          var index = this.selectedPermissions.indexOf(d.code);
          if (index == -1) {
            this.selectedPermissions.push(d.code);
          }
        }
      } else {
        for (let i = 0, len = permissions.length; i < len; i++) {
          var d = permissions[i];
          var index = this.selectedPermissions.indexOf(d.code);
          if (index !== -1) {
            this.selectedPermissions.splice(index, 1);
          }
        }
      }
    }
  }
};
</script>
<style>
.permission-tree-box ul.ivu-tree-children > li {
  margin: 0;
}
.permission-tree-node {
  padding: 10px 5px 10px 0;
  border-bottom: 1px solid #f6f6f6;
}
.permission-tree-node:hover {
  background-color: #f7f7f7;
}
.permission-box .ivu-checkbox-group {
  display: inline-block;
}
.role-list-box {
  list-style: none;
  margin-top: 0px;
  border: 1px solid #f7f7f7;
}
.role-list-box li {
  padding: 5px 15px;
}
.role-list-box li:hover,
.role-list-box li.active {
  background-color: #f7f7f7;
  cursor: pointer;
}
</style>
