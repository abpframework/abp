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
        v-model="stores.menu.data"
        :totalCount="stores.menu.query.totalCount"
        :columns="stores.menu.columns"
        @on-delete="handleDelete"
        @on-edit="handleEdit"
        @on-select="handleSelect"
        @on-selection-change="handleSelectionChange"
        @on-refresh="handleRefresh"
        :row-class-name="rowClsRender"
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
                      v-model="stores.menu.query.kw"
                      placeholder="输入关键字搜索..."
                      @on-search="handleSearchMenu()"
                    >
                      <Select
                        slot="prepend"
                        v-model="stores.menu.query.isDeleted"
                        @on-change="handleSearchMenu"
                        placeholder="删除状态"
                        style="width:60px;"
                      >
                        <Option
                          v-for="item in stores.menu.sources.isDeletedSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                      <Select
                        slot="prepend"
                        v-model="stores.menu.query.status"
                        @on-change="handleSearchMenu"
                        placeholder="菜单状态"
                        style="width:60px;"
                      >
                        <Option
                          v-for="item in stores.menu.sources.statusSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                      <Dropdown
                        slot="prepend"
                        trigger="click"
                        :transfer="true"
                        placement="bottom-start"
                        style="min-width:80px;"
                        @on-visible-change="handleSearchMenuTreeVisibleChange"
                      >
                        <Button type="primary">
                          <span v-text="stores.menu.query.parentName"></span>
                          <Icon type="ios-arrow-down"></Icon>
                        </Button>
                        <div class="text-left" slot="list" style="min-width:390px;">
                          <div>
                            <Button
                              type="primary"
                              icon="ios-search"
                              @click="handleRefreshSearchMenuTreeData"
                            >刷新菜单</Button>
                            <Button
                              class="ml3"
                              type="primary"
                              icon="md-close"
                              @click="handleClearSearchMenuTreeSelection"
                            >清空</Button>
                          </div>
                          <Tree
                            class="text-left dropdown-tree"
                            :data="stores.menu.sources.menuTree.data"
                            @on-select-change="handleSearchMenuTreeSelectChange"
                          ></Tree>
                        </div>
                      </Dropdown>
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
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增菜单"
                >新增菜单</Button>
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
      <Form :model="formModel.fields" ref="formMenu" :rules="formModel.rules" label-position="left">
        <FormItem label="菜单名称" prop="name" label-position="left">
          <Input v-model="formModel.fields.name" placeholder="请输入菜单名称"/>
        </FormItem>
        <FormItem label="路由名称" prop="alias" label-position="left">
          <Input v-model="formModel.fields.alias" placeholder="请输入路由名称"/>
        </FormItem>
        <FormItem label="URL地址" prop="url" label-position="left">
          <Input v-model="formModel.fields.url" placeholder="请输入URL地址"/>
        </FormItem>
        <Row :gutter="8">
          <Col span="12">
            <FormItem>
              <Select
                v-model="formModel.fields.icon"
                filterable
                remote
                :remote-method="handleLoadIconDataSource"
                :loading="stores.menu.sources.iconSources.loading"
                placeholder="请选择图标..."
              >
                <Option
                  v-for="(icon, index) in stores.menu.sources.iconSources.data"
                  :value="icon.code"
                  :key="index"
                >
                  <Icon :type="icon.code" :color="icon.color" :size="24"/>
                  <span v-text="icon.code" style="margin-left:10px;"></span>
                </Option>
              </Select>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem>
              <Icon :type="formModel.fields.icon" :size="32"/>
            </FormItem>
          </Col>
        </Row>
        <Row>
          <Col span="24">
            <FormItem label-position="left">
              <Input v-model="formModel.fields.parentName" placeholder="请选择上级菜单" :readonly="true">
                <Dropdown slot="append" trigger="click" :transfer="true" placement="bottom-end">
                  <Button type="primary">选择...
                    <Icon type="ios-arrow-down"></Icon>
                  </Button>
                  <div class="text-left pad10" slot="list" style="min-width:360px;">
                    <div>
                      <Button
                        type="primary"
                        icon="ios-search"
                        @click="handleRefreshMenuTreeData"
                      >刷新菜单</Button>
                    </div>
                    <Tree
                      class="text-left dropdown-tree"
                      :data="stores.menuTree.data"
                      @on-select-change="handleMenuTreeSelectChange"
                    ></Tree>
                  </div>
                </Dropdown>
              </Input>
            </FormItem>
          </Col>
        </Row>
        <Row>
          <Col span="12">
            <FormItem label="菜单状态" label-position="left">
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
          <Col span="12">
            <FormItem label="默认路由" label-position="left">
              <i-switch size="large" v-model="formModel.fields.isDefaultRouter" :true-value="1" :false-value="0">
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch>
            </FormItem>
          </Col>
        </Row>
        <Row>
          <Col span="12">
            <FormItem label="排序" label-position="left">
              <InputNumber :min="0" v-model="formModel.fields.sort"></InputNumber>
            </FormItem>
          </Col>
        </Row>

        <FormItem label="备注" label-position="top">
          <Input
            type="textarea"
            v-model="formModel.fields.description"
            :rows="4"
            placeholder="菜单备注信息"
          />
        </FormItem>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitMenu">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>

<script>
import Tables from "_c/tables";
import {
  getMenuList,
  createMenu,
  loadMenu,
  editMenu,
  deleteMenu,
  batchCommand,
  loadMenuTree
} from "@/api/rbac/menu";
import { findIconDataSourceByKeyword } from "@/api/rbac/icon";
export default {
  name: "rbac_menu_page",
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
        title: "创建菜单",
        mode: "create",
        selection: [],
        selectOption: {
          icon: {}
        },
        fields: {
          guid: "",
          name: "",
          icon: "",
          url: "",
          alias: "",
          parentGuid: "",
          parentName: "",
          level: 0,
          sort: 0,
          status: 1,
          isDeleted: 0,
          isDefaultRouter: false,
          description: ""
        },
        rules: {
          name: [
            {
              type: "string",
              required: true,
              message: "请输入菜单名称",
              min: 2
            }
          ],
          alias: [
            {
              type: "string",
              required: true,
              message: "请输入菜单名称",
              min: 2
            }
          ]
        }
      },
      stores: {
        menu: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            isDeleted: 0,
            status: -1,
            parentGuid: "",
            parentName: "请选择...",
            sort: [
              {
                direct: "DESC",
                field: "id"
              }
            ]
          },
          sources: {
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
            ],
            menuTree: {
              inited: false,
              data: []
            },
            iconSources: {
              loading: false,
              data: []
            }
          },
          columns: [
            { type: "selection", width: 30, key: "handle" },
            {
              title: "图标",
              key: "icon",
              width: 60,
              align: "center",
              render: (h, params) => {
                return h("Icon", {
                  props: {
                    type: params.row.icon,
                    size: 24
                  }
                });
              }
            },
            { title: "菜单名称", key: "name", sortable: true,minWidth:200 },
            {
              title: "请求地址",
              key: "url",
              width: 250,
              sortable: false,
              ellipsis: true,
              tooltip: true
            },
            { title: "路由名称", key: "alias", width: 200 },
            { title: "上级菜单", key: "parentName", width: 150 },
            { title: "排序", key: "sort", width: 60, align: "center" },
            {
              title: "状态",
              key: "status",
              align: "center",
              width: 60,
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
              title: "默认路由",
              key: "isDefaultRouter",
              align: "center",
              width: 90,
              render: (h, params) => {
                let status = params.row.isDefaultRouter;
                let statusColor = "default";
                let statusText = "否";
                switch (status) {
                  case 1:
                    statusText = "是";
                    statusColor = "success";
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
              width: 150,
              ellipsis: true,
              tooltip: true,
              key: "createdOn"
            },
            {
              title: "创建者",
              key: "createdByUserName",
              ellipsis: true,
              tooltip: true,
              width: 80
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
        },
        menuTree: {
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
        return "创建菜单";
      }
      if (this.formModel.mode === "edit") {
        return "编辑菜单";
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
    loadMenuList() {
      getMenuList(this.stores.menu.query).then(res => {
        this.stores.menu.data = res.data.data;
        this.stores.menu.query.totalCount = res.data.totalCount;
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
      this.handleResetFormMenu();
      this.doLoadMenu(params.row.guid);
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadMenuList();
    },
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormMenu();
    },
    handleSubmitMenu() {
      let valid = this.validateMenuForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateMenu();
        }
        if (this.formModel.mode === "edit") {
          this.doEditMenu();
        }
      }
    },
    handleResetFormMenu() {
      this.$refs["formMenu"].resetFields();
    },
    doCreateMenu() {
      createMenu(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadMenuList();
          this.handleRefreshMenuTreeData();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditMenu() {
      editMenu(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadMenuList();
          this.handleRefreshMenuTreeData();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    validateMenuForm() {
      let _valid = false;
      this.$refs["formMenu"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
          _valid = false;
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadMenu(guid) {
      loadMenu({ guid: guid }).then(res => {
        this.formModel.fields = res.data.data.model;
        this.stores.menuTree.data = res.data.data.tree;
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
      deleteMenu(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadMenuList();
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
          this.loadMenuList();
          this.formModel.selection=[];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleSearchMenu() {
      this.loadMenuList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    doLoadMenuTree() {
      loadMenuTree(null).then(res => {
        this.stores.menuTree.data = res.data.data;
      });
    },
    handleMenuTreeSelectChange(nodes) {
      var node = nodes[0];
      if (node) {
        this.formModel.fields.parentGuid = node.guid;
        this.formModel.fields.parentName = node.title;
      }
    },
    handleRefreshMenuTreeData() {
      this.doLoadMenuTree();
    },
    doLoadSearchMenuTree() {
      loadMenuTree(null).then(res => {
        this.stores.menu.sources.menuTree.data = res.data.data;
      });
    },
    handleSearchMenuTreeSelectChange(nodes) {
      var node = nodes[0];
      if (node) {
        this.stores.menu.query.parentGuid = node.guid;
        this.stores.menu.query.parentName = node.title;
      }
      this.loadMenuList();
    },
    handleRefreshSearchMenuTreeData() {
      this.doLoadSearchMenuTree();
    },
    handleSearchMenuTreeVisibleChange(visible) {
      if (visible && !this.stores.menu.sources.menuTree.inited) {
        this.stores.menu.sources.menuTree.inited = true;
        this.handleRefreshSearchMenuTreeData();
      }
    },
    handleClearSearchMenuTreeSelection() {
      this.stores.menu.query.parentGuid = "";
      this.stores.menu.query.parentName = "请选择...";
      this.loadMenuList();
    },
    handlePageChanged(page) {
      this.stores.menu.query.currentPage = page;
      this.loadMenuList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.menu.query.pageSize = pageSize;
      this.loadMenuList();
    },
    handleLoadIconDataSource(keyword) {
      this.stores.menu.sources.iconSources.loading = true;
      let query = { keyword: keyword };
      findIconDataSourceByKeyword(query).then(res => {
        this.stores.menu.sources.iconSources.data = res.data.data;
        this.stores.menu.sources.iconSources.loading = false;
      });
    }
  },
  mounted() {
    this.loadMenuList();
    this.doLoadMenuTree();
    this.doLoadSearchMenuTree();
  }
};
</script>
