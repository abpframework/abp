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
        v-model="stores.audit.data"
        :totalCount="stores.audit.query.totalCount"
        :columns="stores.audit.columns"
        @on-details="handleDetail"
        @on-refresh="loadAuditList"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="search">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <!-- <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.audit.query.Filter"
                      placeholder="输入关键字搜索..."
                      @on-search="loadAuditList()"
                    ></Input>
                  </FormItem>
                </Form> -->
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button icon="md-refresh" title="刷新" @click="loadAuditList"></Button>
                </ButtonGroup>
              </Col>
            </Row>
          </section>
        </div>
      </tables>
    </Card>
    <Drawer
      title="日志详情"
      v-model="formModel.opened"
      width="800"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form :model="formModel.fields" label-position="top">
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="应用名" prop="applicationName">
              <Input v-model="formModel.fields.applicationName"/>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="登录名" prop="userName">
              <Input type="text" v-model="formModel.fields.userName" placeholder/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="租户名称" prop="tenantName">
              <Input v-model="formModel.fields.tenantName" placeholder/>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="客户端ip" prop="clientIpAddress">
              <Input v-model="formModel.fields.clientIpAddress" placeholder/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="客户端名称" prop="clientName">
              <Input type="text" v-model="formModel.fields.clientName" placeholder/>
            </FormItem>
          </Col>

          <Col span="12">
            <FormItem label="请求方式" prop="httpMethod">
              <Input type="text" v-model="formModel.fields.httpMethod" placeholder/>
            </FormItem>
          </Col>
        </Row>
               <Row :gutter="16">
          <Col span="12">
            <FormItem label="执行时间" prop="executionTime">
              <Input type="text" v-model="formModel.fields.executionTime" placeholder/>
            </FormItem>
          </Col>

          <Col span="12">
            <FormItem label="时长" prop="executionDuration">
              <Input type="text" v-model="formModel.fields.executionDuration" placeholder/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="24">
            <FormItem label="浏览器信息" prop="browserInfo">
              <Input type="text" v-model="formModel.fields.browserInfo" placeholder/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="24">
            <FormItem label="url" prop="url">
              <Input type="text" v-model="formModel.fields.url" placeholder/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="异常信息" prop="exceptions">
              <Input type="text" v-model="formModel.fields.exceptions" placeholder/>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="状态码" prop="httpStatusCode">
              <Input type="text" v-model="formModel.fields.httpStatusCode" placeholder/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="24">
            <Card>
              <Table 
                :border="false"
                size="small"
                :data="formModel.auditLogAction"
                :totalCount="formModel.totalCount"
                :columns="formModel.columns"
              >
              </Table >
            </Card>
          </Col>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">关闭</Button>
      </div>
    </Drawer>
  </div>
</template>

<script>
import Tables from "_c/tables";
import {
  getAuditLogs,
  getAuditLogById,
  getauditLogActionById
} from "@/api/auditlog/auditlog";
export default {
  name: "auditlog",
  components: {
    Tables
  },
  data() {
    return {
      formModel: {
        opened: false,
        fields: {},
        auditLogAction: [],
        totalCount:0,
        columns:[
            { title: "服务名", key: "serviceName", width: 200 },
            { title: "方法名", key: "methodName", width: 100 },
            { title: "参数", key: "parameters", width: 200 },
            { title: "执行时间", key: "executionTime", width: 130 },
            { title: "时长", key: "executionDuration", width: 100 },
        ]
      },
      stores: {
        audit: {
          query: {
            totalCount: 0,
            MaxResultCount: 20,
            SkipCount: 0,
            Filter: "",
            // Sorting: "Id"
          },
          sources: {
            statusFormSources: [
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" }
            ]
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            {
              title: "应用名",
              key: "applicationName",
              width: 140,
              sortable: true
            },
            { title: "登录名", key: "userName", width: 100 },
            { title: "租户名", key: "tenantName", width: 100 },
            { title: "执行时间", key: "executionTime", width: 140 },
            { title: "时长", key: "executionDuration", width: 80 },
            { title: "请求方式", key: "httpMethod", width: 100 },
            { title: "url", key: "url", width: 280 },
            { title: "浏览器信息", key: "browserInfo", width: 200 },
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
                            vm.$emit("on-details", params);
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
                        "查看"
                      )
                    ]
                  );
                },
                (h, params, vm) => {
                  return h("Tooltip", {
                    props: {
                      placement: "left",
                      transfer: true,
                      delay: 1000
                    }
                  });
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
  computed: {},
  methods: {
    loadAuditList() {
      getAuditLogs(this.stores.audit.query).then(res => {
        this.stores.audit.data = res.data.items;
        this.stores.audit.query.totalCount = res.data.totalCount;
      });
    },
    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    handleCloseFormWindow() {
      this.formModel.opened = false;
    },
    handleShowCreateWindow() {
      this.formModel.fields.id = "";
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormAudit();
    },
    handleDetail(params) {
      getAuditLogById({ id: params.row.id }).then(res => {
        this.formModel.fields = res.data;
      });
      getauditLogActionById({ id: params.row.id }).then(res => {
        this.formModel.auditLogAction = res.data.items;
        this.formModel.totalCount = res.data.items.length;
      });
      this.formModel.opened = true;
    },
    handlePageChanged(page) {
      this.stores.audit.query.SkipCount =
        (page - 1) * this.stores.audit.query.MaxResultCount;
      this.loadAuditList();
    },
    handlePageSizeChanged(MaxResultCount) {
      this.stores.audit.query.MaxResultCount = MaxResultCount;
      this.loadAuditList();
    }
  },
  mounted() {
    this.loadAuditList();
  }
};
</script>

<style>
</style>
