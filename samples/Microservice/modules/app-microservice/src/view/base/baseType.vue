<template>
  <Card class="album">
    <Row :gutter="16">
      <Col span="24" class="dnc-toolbar-btns">
        <Button icon="md-create" type="primary" @click="add" title="新增字典类别">新增字典类别</Button>
      </Col>
    </Row>
    <Tree :data="treeData" :load-data="loadData" :render="renderContent"></Tree>
    <Drawer
      title="字典类别"
      v-model="formModel.opened"
      width="400"
      :mask-closable="false"
      :mask="false"
    >
      <Form :model="formModel.fields" ref="form" :rules="formModel.rules" label-position="left">
        <Row>
          <Col span="24">
            <FormItem label-position="left">
              <Input v-model="formModel.parentName" placeholder="请选择上级字典类别" :readonly="true">
                <Dropdown slot="append" trigger="click" :transfer="true" placement="bottom-end">
                  <Button type="primary">
                    选择...
                    <Icon type="ios-arrow-down"></Icon>
                  </Button>
                  <div class="text-left pad10" slot="list" style="min-width:360px;">
                    <Tree
                      ref="tree"
                      class="text-left dropdown-tree"
                      :data="formModel.data"
                      @on-select-change="handleMenuTreeSelectChange"
                    ></Tree>
                  </div>
                </Dropdown>
              </Input>
            </FormItem>
          </Col>
        </Row>
        <FormItem label="编码" prop="code" label-position="left">
          <Input v-model="formModel.fields.code" placeholder="请输入编码"/>
        </FormItem>
        <FormItem label="名称" prop="name" label-position="left">
          <Input v-model="formModel.fields.name" placeholder="请输入名称"/>
        </FormItem>
        <FormItem label="排序码" prop="sort" label-position="left">
          <Input v-model="formModel.fields.sort" placeholder="请输入排序码"/>
        </FormItem>
        <FormItem label="备注" prop="remark" label-position="left">
          <Input v-model="formModel.fields.remark" placeholder="请输入备注"/>
        </FormItem>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmit">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
  </Card>
</template>


<script>
import { mapActions } from "vuex";

import {
  getBaseTypes,
  deleteBaseType,
  createBaseType,
  loadBaseType,
  editBaseType,
  getBaseTypeViewTrees
} from "@/api/base/baseType";

export default {
  name: "Album",
  data() {
    return {
      formModel: {
        opened: false,
        mode: "create",
        parentGuid: "",
        parentName: "",
        fields: {
          parentId: "",
          code: "",
          name: "",
          sort: 0,
          remark: ""
        },
        data: [],
        rules: {
          code: [
            {
              type: "string",
              required: true,
              message: "请输入编码"
            }
          ],
          name: [
            {
              type: "string",
              required: true,
              message: "请输入名称"
            }
          ]
        }
      },
      treeData: []
    };
  },
  methods: {
    handleMenuTreeSelectChange(nodes) {
      var node = nodes[0];
      if (node) {
        this.formModel.parentGuid = node.guid;
        this.formModel.parentName = node.title;
      }
    },
    loadData(item, callback) {
      let parentId = item.id || null;
      let data = [];
      getBaseTypes({
        Sorting: "sort",
        ParentId: parentId
      }).then(res => {
        data = this.getTree(res.data.items);
        callback(data);
      });
    },
    getTree(tree = []) {
      let arr = [];
      if (tree.length !== 0) {
        tree.forEach(item => {
          let obj = {};
          obj.name = item.name;
          obj.id = item.id;
          obj.sort = item.sort;
          obj.remark = item.remark;
          obj.code = item.code;
          obj.parentId = item.parentId;
          if (item.hasChildren === true) {
            obj.children = [];
            obj.loading = false;
          }
          arr.push(obj);
        });
      }
      return arr;
    },
    renderContent(h, { root, node, data }) {
      return h(
        "span",
        {
          style: {
            display: "inline-block",
            width: "100%"
          }
        },
        [
          h("span", [
            h("Icon", {
              style: {
                marginRight: "8px"
              }
            }),
            h("span", data.name)
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
                "span",
                {
                  style: {
                    display: "inline-block",
                    "text-align": "center",
                    width: "205px",
                    marginRight: "40px"
                  }
                },
                data.code
              ),
              h(
                "span",
                {
                  style: {
                    display: "inline-block",
                    width: "280px",
                    marginRight: "20px"
                  }
                },
                data.remark
              ),
              h(
                "span",
                {
                  style: {
                    display: "inline-block",
                    width: "280px",
                    marginRight: "20px"
                  }
                },
                data.sort
              ),
              h(
                "Button",
                {
                  props: Object.assign(
                    {},
                    {
                      type: "primary",
                      size: "small"
                    }
                  ),
                  style: {
                    marginRight: "8px"
                  },
                  on: {
                    click: () => {
                      this.edit(data);
                    }
                  }
                },
                "编辑"
              ),
              h(
                "Button",
                {
                  props: Object.assign(
                    {},
                    {
                      type: "error",
                      size: "small"
                    }
                  ),
                  on: {
                    click: () => {
                      this.deleted(data.id);
                    }
                  }
                },
                "删除"
              )
            ]
          )
        ]
      );
    },
    queryCategoryList() {
      getBaseTypes({
        Sorting: "sort",
        ParentId: null
      }).then(res => {
        this.treeData = this.getTree(res.data.items);
      });
    },
    deleted(id) {
      var that = this;
      this.$Modal.confirm({
        title: "提示",
        content: "你确认要删除这条记录吗",
        onOk: function() {
          deleteBaseType({ id: id }).then(res => {
            this.$Message.success("删除成功!");
            that.queryCategoryList();
          });
        }
      });
    },
    validateRoleForm() {
      let _valid = false;
      this.$refs["form"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
          _valid = false;
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    add() {
      this.$refs["form"].resetFields();
      this.formModel.mode = "create";
      this.formModel.opened = true;
      this.formModel.parentName = "";
      this.formModel.parentGuid=""
      this.getBaseTypeViewTrees(null);
    },
    edit(data) {
      this.formModel.mode = "edit";
      this.formModel.opened = true;
      var that = this;

      loadBaseType({ id: data.id }).then(res => {
        that.formModel.fields = res.data;

        that.formModel.parentName='';
        that.formModel.parentGuid=res.data.parentId;

        this.getBaseTypeViewTrees(data.id).then(() => {
          var selectNodes = that.$refs.tree.getSelectedNodes();
          if (selectNodes.length > 0) {
            that.formModel.parentName = selectNodes[0].title;
          }
        });
      });
    },
    getBaseTypeViewTrees(id) {
      var that = this;
      return getBaseTypeViewTrees({
        id: id
      }).then(res => {
        that.formModel.data = res.data;
      });
    },
    
    handleSubmit() {
      let valid = this.validateRoleForm();
      var that = this;
      if (valid) {
        if (this.formModel.parentGuid != "") {
          this.formModel.fields.parentId = this.formModel.parentGuid;
        }
        if (this.formModel.mode === "create") {
          createBaseType(this.formModel.fields).then(res => {
            this.$Message.success("新增字典类别成功");
            that.queryCategoryList();
            that.formModel.opened = false;
          });
        }
        if (this.formModel.mode === "edit") {
          editBaseType(this.formModel.fields).then(res => {
            this.$Message.success("修改字典类别成功");
            that.queryCategoryList();
            that.formModel.opened = false;
          });
        }
      }
    }
  },
  mounted() {
    this.queryCategoryList();
  }
};
</script>

<style scoped>
</style>