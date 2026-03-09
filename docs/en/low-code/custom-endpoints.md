```json
//[doc-seo]
{
    "Description": "Define custom REST API endpoints with JavaScript handlers in the ABP Low-Code System. Create dynamic APIs without writing C# controllers."
}
```

# Custom Endpoints

Custom Endpoints allow you to define REST API routes with server-side JavaScript handlers directly in `model.json`. Each endpoint is registered as an ASP.NET Core endpoint at startup and supports hot-reload when the model changes.

## Defining Endpoints

Add endpoints to the `endpoints` array in `model.json`:

```json
{
  "endpoints": [
    {
      "name": "GetProductStats",
      "route": "/api/custom/products/stats",
      "method": "GET",
      "description": "Get product statistics",
      "requireAuthentication": false,
      "javascript": "var count = await db.count('LowCodeDemo.Products.Product');\nreturn ok({ totalProducts: count });"
    }
  ]
}
```

### Endpoint Descriptor

| Field | Type | Default | Description |
|-------|------|---------|-------------|
| `name` | string | **Required** | Unique endpoint name |
| `route` | string | **Required** | URL route pattern (supports `{parameters}`) |
| `method` | string | `"GET"` | HTTP method: `GET`, `POST`, `PUT`, `DELETE` |
| `javascript` | string | **Required** | JavaScript handler code |
| `description` | string | null | Description for documentation |
| `requireAuthentication` | bool | `true` | Require authenticated user |
| `requiredPermissions` | string[] | null | Required permission names |

## Route Parameters

Use `{paramName}` syntax in the route. Access values via the `route` object:

```json
{
  "name": "GetProductById",
  "route": "/api/custom/products/{id}",
  "method": "GET",
  "javascript": "var product = await db.get('LowCodeDemo.Products.Product', route.id);\nif (!product) { return notFound('Product not found'); }\nreturn ok({ id: product.Id, name: product.Name, price: product.Price });"
}
```

## JavaScript Context

Inside custom endpoint scripts, you have access to:

### Request Context

| Variable | Description |
|----------|-------------|
| `request` | Full request object |
| `route` | Route parameter values (e.g., `route.id`) |
| `params` | Alias for route parameters |
| `query` | Query string parameters (e.g., `query.q`, `query.page`) |
| `body` | Request body (for POST/PUT) |
| `headers` | Request headers |
| `user` | Current user (same as `context.currentUser` in [Interceptors](interceptors.md)) |
| `email` | Email sender (same as `context.emailSender` in [Interceptors](interceptors.md)) |

### Response Helpers

| Function | HTTP Status | Description |
|----------|-------------|-------------|
| `ok(data)` | 200 | Success response with data |
| `created(data)` | 201 | Created response with data |
| `noContent()` | 204 | No content response |
| `badRequest(message)` | 400 | Bad request response |
| `unauthorized(message)` | 401 | Unauthorized response |
| `forbidden(message)` | 403 | Forbidden response |
| `notFound(message)` | 404 | Not found response |
| `error(message)` | 500 | Internal server error response |
| `response(statusCode, data, error)` | Custom | Custom status code response |

### Logging

| Function | Description |
|----------|-------------|
| `log(message)` | Log an informational message |
| `logWarning(message)` | Log a warning message |
| `logError(message)` | Log an error message |

### Database API

The full [Scripting API](scripting-api.md) (`db` object) is available for querying and mutating data.

## Examples

### Get Statistics

```json
{
  "name": "GetProductStats",
  "route": "/api/custom/products/stats",
  "method": "GET",
  "requireAuthentication": false,
  "javascript": "var totalCount = await db.count('LowCodeDemo.Products.Product');\nvar avgPrice = totalCount > 0 ? await db.query('LowCodeDemo.Products.Product').average(p => p.Price) : 0;\nreturn ok({ totalProducts: totalCount, averagePrice: avgPrice });"
}
```

### Search with Query Parameters

```json
{
  "name": "SearchCustomers",
  "route": "/api/custom/customers/search",
  "method": "GET",
  "requireAuthentication": true,
  "javascript": "var searchTerm = query.q || '';\nvar customers = await db.query('LowCodeDemo.Customers.Customer')\n  .where(c => c.Name.toLowerCase().includes(searchTerm.toLowerCase()))\n  .take(10)\n  .toList();\nreturn ok(customers.map(c => ({ id: c.Id, name: c.Name, email: c.EmailAddress })));"
}
```

### Dashboard Summary

```json
{
  "name": "GetDashboardSummary",
  "route": "/api/custom/dashboard",
  "method": "GET",
  "requireAuthentication": true,
  "javascript": "var productCount = await db.count('LowCodeDemo.Products.Product');\nvar customerCount = await db.count('LowCodeDemo.Customers.Customer');\nvar orderCount = await db.count('LowCodeDemo.Orders.Order');\nreturn ok({ products: productCount, customers: customerCount, orders: orderCount, user: user.isAuthenticated ? user.userName : 'Anonymous' });"
}
```

## Authentication and Authorization

| Setting | Behavior |
|---------|----------|
| `requireAuthentication: false` | Endpoint is publicly accessible |
| `requireAuthentication: true` | User must be authenticated |
| `requiredPermissions: ["MyApp.Products"]` | User must have the specified permissions |

## See Also

* [Scripting API](scripting-api.md)
* [Interceptors](interceptors.md)
* [model.json Structure](model-json.md)
