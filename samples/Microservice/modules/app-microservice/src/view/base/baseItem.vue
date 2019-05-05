a<template>
  <Card class="album">
    <Card>
      <tables
        ref="tables"
        editable
        searchable
        :border="false"
        size="small"
        search-place="top"
        v-model="stores.baseItem.data"
        :totalCount="stores.baseItem.query.totalCount"
        :columns="stores.baseItem.columns"
        @on-delete="handleDelete"
        @on-edit="handleEdit"
        @on-refresh="loadbaseItemList"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="search">
          <section class="dnc-toolbar-wrap">
             <Row :gutter="16">
               <Col span="16">
                <Form inline>
                  <FormItem>
                    <Dropdown
                        trigger="click"
                        :transfer="true"
                        placement="bottom-start"
                        style="min-width:80px;"
                        @on-visible-change="handleSearchTypeTreeVisibleChange"
                      >
                        <Button type="primary">
                          <span v-text="stores.baseItem.query.typeName"></span>
                          <Icon type="ios-arrow-down"></Icon>
                        </Button>
                        <div class="text-left pad10" slot="list" style="min-width:360px;">
                          <div>
                            <Button
                              type="primary"
                              size="small"
                              icon="ios-search"
                              @click="handleRefreshSearchTypeTreeData"
                            >刷新菜单</Button>
                            <Button
                              class="ml3"
                              type="primary"
                              size="small"
                              icon="md-close"
                              @click="handleClearSearchTypeTreeSelection"
                            >清空</Button>
                          </div>
                          <Tree
                            class="text-left dropdown-tree"
                            :data="stores.baseItem.sources.typeTree.data"
                            @on-select-change="handleSearchTypeTreeSelectChange"
                          ></Tree>
                        </div>
                      </Dropdown>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button icon="md-refresh" title="刷新" @click="loadbaseItemList"></Button>
                </ButtonGroup>
                <Button
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增字典"
                >新增字典</Button>
              </Col>
            </Row>
          </section>
        </div>
      </tables>
    </Card>

    <Drawer
      title="字典信息"
      v-model="formModel.opened"
      width="400"
      :mask-closable="false"
      :mask="false"
    >
      <Form :model="formModel.fields" ref="form" :rules="formModel.rules" label-position="left">
        <Row>
          <Col span="24">
            <FormItem label-position="left">
              <Input v-model="formModel.parentName" placeholder="请选择字典类别" :readonly="true">
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
                      @on-select-change="handleTypeTreeSelectChange"
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
import Tables from "_c/tables";
import {
  getBaseItems,
  deleteBaseItem,
  createBaseItem,
  loadBaseItem,
  editBaseItem,
  getViewTrees
} from "@/api/base/baseItem";

export default {
  name: "Album",
  components: {
    Tables
  },
  data() {
    return {
      formModel: {
        opened: false,
        mode: "create",
        parentGuid: "",
        parentName: "",
        fields: {
          baseTypeGuid: "",
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
      stores: {
        baseItem: {
          query: {
            totalCount: 0,
            MaxResultCount: 20,
            SkipCount: 0,
            Sorting: "sort",
            typeName: "请选择...",
            BaseTypeGuid:""
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
            ],
             typeTree: {
              inited: false,
              data: []
            },
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "编码名称", key: "code", width: 150 },
            { title: "名称", key: "name", width: 250 },
            { title: "排序码", key: "sort", width: 250 },
            { title: "备注", key: "remark"},
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
                }
              ]
            }
          ],
          data: []
        }
      }
    };
  },
  methods: {
    loadbaseItemList() {
      getBaseItems(this.stores.baseItem.query).then(res => {
        this.stores.baseItem.data = res.data.items;
        this.stores.baseItem.query.totalCount = res.data.totalCount;
      });
    },
    handlePageChanged(page) {
      this.stores.baseItem.query.SkipCount =  (page - 1) * this.stores.baseItem.query.MaxResultCount;
      this.loadbaseItemList();
    },
    handlePageSizeChanged(MaxResultCount) {
      this.stores.baseItem.query.MaxResultCount = MaxResultCount;
      this.loadbaseItemList();
    },
    handleTypeTreeSelectChange(nodes) {
      var node = nodes[0];
      if (node) {
        this.formModel.parentGuid = node.guid;
        this.formModel.parentName = node.title;
      }
    },
    handleDelete(data) {
      var that = this;

      deleteBaseItem({ id: data.row.id }).then(res => {
        this.$Message.success("删除成功!");
        that.loadbaseItemList();
      });
    },
    validatebaseItemForm() {
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
    handleShowCreateWindow() {
      this.$refs["form"].resetFields();
      this.formModel.mode = "create";
      this.formModel.opened = true;
      this.formModel.parentName = "";
      this.formModel.parentGuid=""
      this.getViewTrees(null);
    },
    handleEdit(data) {
      this.formModel.mode = "edit";
      this.formModel.opened = true;
      var that = this;

      loadBaseItem({ id: data.row.id }).then(res => {
        that.formModel.fields = res.data;

        that.formModel.parentName = "";
        that.formModel.parentGuid = res.data.parentId;

        this.getViewTrees(data.row.baseTypeGuid).then(() => {
          var selectNodes = that.$refs.tree.getSelectedNodes();
          if (selectNodes.length > 0) {
            that.formModel.parentName = selectNodes[0].title;
          }
        });
      });
    },
    getViewTrees(id) {
      var that = this;
      return getViewTrees({
        id: id
      }).then(res => {
        that.formModel.data = res.data;
      });
    },
    handleSubmit() {
      let valid = this.validatebaseItemForm();
      var that = this;
      if (valid) {
        if (this.formModel.parentGuid == ""||this.formModel.parentGuid==undefined) {
          this.$Message.error("请选择字典类别!");
          return;
        }
        this.formModel.fields.baseTypeGuid = this.formModel.parentGuid;
        if (this.formModel.mode === "create") {
          createBaseItem(this.formModel.fields).then(res => {
            this.$Message.success("新增字典成功");
            that.loadbaseItemList();
            that.formModel.opened = false;
          });
        }
        if (this.formModel.mode === "edit") {
          editBaseItem(this.formModel.fields).then(res => {
            this.$Message.success("修改字典成功");
            that.loadbaseItemList();
            that.formModel.opened = false;
          });
        }
      }
    },
    handleSearchTypeTreeSelectChange(nodes) {
      var node = nodes[0];
      if (node) {
        this.stores.baseItem.query.BaseTypeGuid = node.guid;
        this.stores.baseItem.query.typeName = node.title;
      }
      this.loadbaseItemList();
    },
    handleSearchTypeTreeVisibleChange(visible) {
      if (visible && !this.stores.baseItem.sources.typeTree.inited) {
        this.stores.baseItem.sources.typeTree.inited = true;
        getViewTrees({}).then(res => {
          this.stores.baseItem.sources.typeTree.data = res.data;
        });
      }
    },
   handleRefreshSearchTypeTreeData() {
     getViewTrees({}).then(res => {
        this.stores.baseItem.sources.typeTree.data = res.data;
      });
    },
    handleClearSearchTypeTreeSelection() {
      this.stores.baseItem.query.BaseTypeGuid = "";
      this.stores.baseItem.query.typeName = "请选择...";
      this.loadbaseItemList();
    }
  },
  mounted() {
    this.loadbaseItemList();
    this.handleRefreshSearchTypeTreeData()
  }
};
</script>

<style scoped>
</style>