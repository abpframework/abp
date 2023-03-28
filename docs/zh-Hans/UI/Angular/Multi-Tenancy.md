# 在 Angular UI 中的多租户支持

ABP Angular UI 支持多租户，以下是与多租户相关的一些特性：

![Tenants Page](./images/tenants-page.png)

<p style="font-size:small;text-align:center;">租户页面</p>

在上面的页面中，您可以：

- 查看所有租户。
- 创建新的租户。
- 编辑现有租户。
- 删除租户。

![Tenant Switching Component](./images/tenant-switching-box.png)

<p style="font-size:small;text-align:center;">租户切换组件</p>

您可以在 MVC Account Public Module 的子页面（如登录页面）中使用租户切换框来在现有租户之间切换。Angular UI 从 `application-configuration` 响应中获取选定的租户，并在每个请求中将租户 ID 作为 `__tenant` header 发送到后端。

## 域名/子域名租户解析器

> **注意：** 如果要执行下面的步骤，您还应该实现后端的域名/子域名租户解析器功能。请参阅[多租户文档中的域名/子域名租户解析器](../../Multi-Tenancy#domain-subdomain-tenant-resolver)以了解后端实现。

Angular UI 可以从运行 URL 中获取租户名称。您可以通过子域名（如 mytenant1.mydomain.com）或整个域名（如 mytenant.com）来确定当前租户。要做到这一点，您需要在环境中设置 `application.baseUrl` 属性：

子域名解析器：

```js
// environment.prod.ts

export const environment = {
  //...
  application: {
    baseUrl: "https://{0}.mydomain.com/",
  },
  //...
};
```

**{0}** 是用于确定当前租户唯一名称的占位符。

在上述配置完成后，如果您的应用程序运行在 `mytenant1.mydomain.com` 上，应用程序将获取租户名称为 **mytenant1**。接下来，应用程序将调用 `/api/abp/multi-tenancy/tenants/by-name/mytenant1` 端点来检查租户是否存在。如果租户（mytenant1）存在，则应用程序将保留此租户数据，并在每个请求中将其 `id` 作为 `__tenant` header 发送到后端。如果租户不存在，则应用程序不会将 `__tenant` header 发送到后端。

> **重要提示：** 如果在 `baseUrl` 中使用了占位符（**{0}**），则子页面（如登录页面）中 `AccountLayoutComponent` 中的租户切换组件将被隐藏。

域名解析器:

```js
// environment.prod.ts

export const environment = {
  //...
  application: {
    baseUrl: "https://{0}.com/",
  },
  //...
};
```

配置完成后，如果您的应用程序运行在 `mytenant.com` 上，应用程序将获取租户名称为 **mytenant**。

### 租户特定的入口

在环境中，可以将占位符 **{0}** 放入 API URL 中以确定租户特定的入口。

```js
// environment.prod.ts

export const environment = {
  //...
  application: {
    baseUrl: "https://{0}.mydomain.com/",
    //...
  },
  oAuthConfig: {
    issuer: "https://{0}.ids.mydomain.com",
    //...
  },
  apis: {
    default: {
      url: "https://{0}.api.mydomain.com",
    },
    AbpIdentity: {
      url: "https://{0}.identity.mydomain.com",
    },
  },
};
```

> **重要提示:**  `application.baseUrl`和`baseUrl`属性中的 `{0}`  占位符是必需的，以便从运行 URL 中获取租户。API URL 中的其他占位符是可选的。

在上述配置完成后，如果您的应用程序运行在 `mytenant1.mydomain.com`上，应用程序将获取租户名称为 **mytenant1** ，并在应用程序初始化时将环境对象替换为  `EnvironmentService` 中的以下内容:

```js
// environment object in EnvironmentService

{
  //...
  application: {
    baseUrl: 'https://mytenant1.mydomain.com/',
    //...
  },
  oAuthConfig: {
    issuer: 'https://mytenant1.ids.mydomain.com',
    //...
  },
  apis: {
    default: {
      url: 'https://mytenant1.api.mydomain.com',
    },
    AbpIdentity: {
      url: 'https://mytenant1.identity.mydomain.com',
    },
  },
}
```

替换后，应用程序将使用以下 URL:

- `https://mytenant1.ids.mydomain.com` 作为 AuthServer URL。
- `https://mytenant1.api.mydomain.com`  作为默认 URL。
- `https://mytenant1.identity.mydomain.com` 作为 `AbpIdentity` 入点口 URL.

应用程序在每个请求中发送包含当前租户 id 的 `__tenant` header.

## 参见

- [ABP 中的多租户](../../Multi-Tenancy.md)
