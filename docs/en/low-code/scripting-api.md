```json
//[doc-seo]
{
    "Description": "Server-side JavaScript Scripting API for ABP Low-Code System. Query, filter, aggregate data and perform CRUD operations with database-level execution."
}
```

# Scripting API

> **Preview:** The Low-Code scripting API is a preview server-side JavaScript surface. Available globals, helper methods, limits, and sandbox behavior may change before general availability.

The designer and React runtime cover the standard CRUD, form, filter, and export workflows. Use the scripting API when an interceptor, action, or custom endpoint needs server-side JavaScript.

The Low-Code System provides a server-side JavaScript scripting engine for executing custom business logic within [interceptors](interceptors.md), [custom endpoints](custom-endpoints.md), event handlers, background jobs, and background workers. Scripts run in a sandboxed environment with access to a database API backed by EF Core.

Scripts are wrapped in an async function, so `await` and top-level `return` are supported.

## Unified Database API (`db`)

The `db` object is the main entry point for all data operations.

### Key Design Principles

* **Immutable Query Builder** — each query method returns a new builder instance. The original is never modified.
* **Database-Level Execution** — all operations (filters, aggregations, joins, set operations) translate to SQL via EF Core and Dynamic LINQ.
* **No in-memory processing** of large datasets.

> `db.query(entityName)` is asynchronous. Always `await` it before chaining query methods, and `await` the terminal operation such as `toList()`, `count()`, `first()`, or `sum()`.

```javascript
// Immutable pattern — each call creates a new builder
var baseQuery = (await db.query('Entity')).where(x => x.Active);
var cheap = baseQuery.where(x => x.Price < 100);     // baseQuery unchanged
var expensive = baseQuery.where(x => x.Price > 500);  // baseQuery unchanged
```

## Query API

### Basic Queries

```javascript
var productQuery = await db.query('LowCodeDemo.Products.Product');
var products = await productQuery
    .where(x => x.Price > 100)
    .orderBy(x => x.Price)
    .take(10)
    .toList();

var filteredProductQuery = await db.query('LowCodeDemo.Products.Product');
var result = await filteredProductQuery
    .where(x => x.Price > 100 && x.Price < 500)
    .where(x => x.StockCount > 0)
    .orderByDescending(x => x.Price)
    .skip(10)
    .take(20)
    .toList();
```

### Query Methods

| Method | Description | Returns |
|--------|-------------|---------|
| `where(x => condition)` | Filter results | `QueryBuilder` |
| `orderBy(x => x.Property)` | Sort ascending | `QueryBuilder` |
| `orderByDescending(x => x.Property)` | Sort descending | `QueryBuilder` |
| `thenBy(x => x.Property)` | Secondary sort ascending | `QueryBuilder` |
| `thenByDescending(x => x.Property)` | Secondary sort descending | `QueryBuilder` |
| `skip(n)` | Skip n records | `QueryBuilder` |
| `take(n)` | Take n records | `QueryBuilder` |
| `toList()` | Execute and return array | `Promise<object[]>` |
| `count()` | Return count | `Promise<number>` |
| `any()` | Check if any matches exist | `Promise<boolean>` |
| `all(x => condition)` | Check if all records match | `Promise<boolean>` |
| `isEmpty()` | Check if no results | `Promise<boolean>` |
| `isSingle()` | Check if exactly one result | `Promise<boolean>` |
| `first()` / `firstOrDefault()` | Return first match or null | `Promise<object\|null>` |
| `last()` / `lastOrDefault()` | Return last match or null | `Promise<object\|null>` |
| `single()` / `singleOrDefault()` | Return single match or null | `Promise<object\|null>` |
| `elementAt(index)` | Return element at index | `Promise<object\|null>` |
| `select(x => projection)` | Project to custom shape | `QueryBuilder` |
| `join(entity, alias, condition)` | Inner join | `QueryBuilder` |
| `leftJoin(entity, alias, condition)` | Left join | `QueryBuilder` |

### Supported Operators in Lambda

| Category | Operators |
|----------|-----------|
| Comparison | `===`, `!==`, `>`, `>=`, `<`, `<=` |
| Logical | `&&`, `\|\|`, `!` |
| Arithmetic | `+`, `-`, `*`, `/`, `%` |
| String | `startsWith()`, `endsWith()`, `includes()`, `trim()`, `toLowerCase()`, `toUpperCase()` |
| Array | `array.includes(x.Property)` — translates to SQL `IN` |
| Math | `Math.round()`, `Math.floor()`, `Math.ceil()`, `Math.abs()`, `Math.sqrt()`, `Math.pow()`, `Math.sign()`, `Math.truncate()` |
| Null | `!= null`, `=== null` |

### Variable Capture

External variables are captured and passed as parameters:

```javascript
var minPrice = 100;
var config = { minStock: 10 };
var nested = { range: { min: 50, max: 200 } };

var query = await db.query('Entity');

var result = await query.where(x => x.Price > minPrice).toList();
var result2 = await query.where(x => x.StockCount > config.minStock).toList();
var result3 = await query.where(x => x.Price >= nested.range.min).toList();
```

### Contains / IN Operator

```javascript
var targetPrices = [50, 100, 200];
var query = await db.query('Entity');
var products = await query
    .where(x => targetPrices.includes(x.Price))
    .toList();
```

### Select Projection

```javascript
var productQuery = await db.query('LowCodeDemo.Products.Product');
var projected = await productQuery
    .where(x => x.Price > 0)
    .select(x => ({ ProductName: x.Name, ProductPrice: x.Price }))
    .toList();
```

## Joins

### Explicit Joins

```javascript
var orderLineQuery = await db.query('LowCodeDemo.Orders.OrderLine');
var orderLines = await orderLineQuery
    .join('LowCodeDemo.Products.Product', 'p', (ol, p) => ol.ProductId === p.Id)
    .take(10)
    .toList();

// Access joined data via alias
orderLines.forEach(line => {
    var product = line.p;
    context.log(product.Name + ': $' + line.Amount);
});
```

### Left Join

```javascript
var orderQuery = await db.query('LowCodeDemo.Orders.Order');
var orders = await orderQuery
    .leftJoin('LowCodeDemo.Products.Product', 'p', (o, p) => o.CustomerId === p.Id)
    .toList();

orders.forEach(order => {
    if (order.p) {
        context.log('Has match: ' + order.p.Name);
    }
});
```

### LINQ-Style Join

```javascript
var orderQuery = await db.query('Order');
orderQuery
  .join('LowCodeDemo.Products.Product',
        o => o.ProductId,
        p => p.Id)
```

### Join with Filtered Query

```javascript
var productQuery = await db.query('Product');
var expensiveProducts = productQuery.where(p => p.Price > 100);

var orderLineQuery = await db.query('OrderLine');
var orders = await orderLineQuery
  .join(expensiveProducts,
        ol => ol.ProductId,
        p => p.Id)
  .toList();
```

## Set Operations

Set operations execute at the database level using SQL:

| Method | SQL Equivalent | Description |
|--------|---------------|-------------|
| `union(query)` | `UNION` | Combine, remove duplicates |
| `concat(query)` | `UNION ALL` | Combine, keep duplicates |
| `intersect(query)` | `INTERSECT` | Elements in both |
| `except(query)` | `EXCEPT` | Elements in first, not second |

```javascript
var productQuery = await db.query('Product');
var cheap = productQuery.where(x => x.Price <= 100);
var popular = productQuery.where(x => x.Rating > 4);

var bestDeals = await cheap.intersect(popular).toList();
var underrated = await cheap.except(popular).toList();
```

## Aggregation Methods

All aggregations execute as SQL statements:

| Method | SQL | Returns |
|--------|-----|---------|
| `sum(x => x.Property)` | `SELECT SUM(...)` | `Promise<number>` |
| `average(x => x.Property)` | `SELECT AVG(...)` | `Promise<number>` |
| `min(x => x.Property)` | `SELECT MIN(...)` | `Promise<any>` |
| `max(x => x.Property)` | `SELECT MAX(...)` | `Promise<any>` |
| `distinct(x => x.Property)` | `SELECT DISTINCT ...` | `Promise<any[]>` |
| `groupBy(x => x.Property)` | `GROUP BY ...` | `Promise<GroupResult[]>` |

```javascript
var productQuery = await db.query('Product');

var totalValue = await productQuery.sum(x => x.Price);
var avgPrice = await productQuery.where(x => x.InStock).average(x => x.Price);
var cheapest = await productQuery.min(x => x.Price);
```

### GroupBy with Select

```javascript
var productQuery = await db.query('Product');
var grouped = await productQuery
    .groupBy(x => x.Category)
    .select(g => ({
        Category: g.Key,
        Count: g.count(),
        TotalPrice: g.sum(x => x.Price),
        AvgPrice: g.average(x => x.Price),
        MinPrice: g.min(x => x.Price),
        MaxPrice: g.max(x => x.Price)
    }))
    .toList();
```

### GroupBy Aggregation Methods

| Method | SQL |
|--------|-----|
| `g.Key` | Group key value |
| `g.count()` | `COUNT(*)` |
| `g.sum(x => x.Prop)` | `SUM(prop)` |
| `g.average(x => x.Prop)` | `AVG(prop)` |
| `g.min(x => x.Prop)` | `MIN(prop)` |
| `g.max(x => x.Prop)` | `MAX(prop)` |
| `g.toList()` | Get group items |
| `g.take(n).toList()` | Get first n items |

### GroupBy with Items

```javascript
var productQuery = await db.query('Product');
var grouped = await productQuery
    .groupBy(x => x.Category)
    .select(g => ({
        Category: g.Key,
        Count: g.count(),
        Items: g.take(10).toList()
    }))
    .toList();
```

### GroupBy Security Limits

| Limit | Default | Description |
|-------|---------|-------------|
| `MaxGroupCount` | No limit | Maximum groups |

## Math Functions

Math functions translate to SQL functions (ROUND, FLOOR, CEILING, ABS, etc.):

```javascript
var productQuery = await db.query('Product');
var products = await productQuery
    .where(x => Math.round(x.Price) > 100)
    .toList();

var result = await productQuery
    .where(x => Math.abs(x.Balance) < 10 && Math.floor(x.Rating) >= 4)
    .toList();
```

## CRUD API

Direct CRUD methods on the `db` object:

| Method | Description | Returns |
|--------|-------------|---------|
| `db.get(entityName, id)` | Get by ID | `Promise<object\|null>` |
| `db.getList(entityName, take?)` | Get a list with an optional limit | `Promise<object[]>` |
| `db.getCount(entityName)` | Get count | `Promise<number>` |
| `db.count(entityName)` | Alias for `db.getCount(entityName)` | `Promise<number>` |
| `db.exists(entityName)` | Check if any records exist | `Promise<boolean>` |
| `db.insert(entityName, entity)` | Insert new | `Promise<object>` |
| `db.update(entityName, entity)` | Update existing | `Promise<object>` |
| `db.delete(entityName, id)` | Delete by ID | `Promise<void>` |

> **Note:** The `entityName` parameter can be either a **dynamic entity** (e.g., `"LowCodeDemo.Products.Product"`) or a **[reference entity](reference-entities.md)** (e.g., `"Volo.Abp.Identity.IdentityUser"`). However, `insert`, `update`, and `delete` operations only work on dynamic entities — reference entities are read-only.

```javascript
// Get by ID
var product = await db.get('LowCodeDemo.Products.Product', id);

// Insert
var newProduct = await db.insert('LowCodeDemo.Products.Product', {
    Name: 'New Product',
    Price: 99.99,
    StockCount: 100
});

// Update
var updated = await db.update('LowCodeDemo.Products.Product', {
    Id: existingId,
    Name: 'Updated Name',
    Price: 149.99
});

// Delete
await db.delete('LowCodeDemo.Products.Product', id);
```

## Script Context

Scripts receive a `context` object and common global shortcuts. Available services can be enabled or disabled per script type with capability profiles.

### Common Services

| Global | Context property | Description |
|--------|------------------|-------------|
| `db` | `context.db` | Query and CRUD API |
| `user`, `currentUser` | `context.currentUser` | Current user information and claims |
| `tenant`, `currentTenant` | `context.currentTenant` | Current tenant information |
| `email`, `emailSender` | `context.emailSender` | Email send and queue helpers |
| `config` | `context.config` | Filtered application configuration reader |
| `http` | `context.http` | Hardened outbound HTTP client |
| `auth`, `authorization` | `context.authorization` | Permission checks |
| `settings` | `context.settings` | Filtered setting provider |
| `features` | `context.features` | Feature checks |
| `events` | `context.events` | Distributed event publishing |
| `jobs` | `context.jobs` | Dynamic background job enqueueing |
| `encryption` | `context.encryption` | String encryption and decryption |
| `textTemplating` | `context.textTemplating` | ABP text template rendering |
| `blob` | `context.blob` | Base64 blob storage wrapper |
| `files` | `context.files` | Low-code file field helper |
| `images` | `context.images` | Low-code image field helper |
| `attachments` | `context.attachments` | Record attachment helper |
| `fields` | `context.fields` | Field selector helpers for file operations |
| `log`, `logWarning`, `logError` | logging methods | Script logging |

Global helpers are also available:

| Helper | Description |
|--------|-------------|
| `guid()` | Generates a GUID string |
| `userFriendlyError(message)` | Throws a `UserFriendlyException` |
| `businessError(message, code?)` | Throws a `BusinessException` |

### Interceptor Context

Interceptors add `args` and `commandArgs`:

| Property / Method | Description |
|-------------------|-------------|
| `commandArgs.data` | Entity data dictionary for create/update |
| `commandArgs.entityId` | Entity ID for update/delete |
| `commandArgs.commandName` | `Create`, `Update`, or `Delete` |
| `commandArgs.entityName` | Full entity name |
| `commandArgs.getValue(name)` | Get a property value |
| `commandArgs.setValue(name, value)` | Set a property value |
| `commandArgs.hasValue(name)` | Check whether the input contains a property |
| `commandArgs.removeValue(name)` | Remove a property from the input |

Set `globalError` to abort an operation with a user-facing error:

```javascript
if (!args.getValue('Name')) {
    globalError = 'Name is required.';
}
```

### Custom Endpoint Context

Custom endpoints add request globals and response helpers. See [Custom Endpoints](custom-endpoints.md) for details.

| Variable | Description |
|----------|-------------|
| `request` | Full request object |
| `route`, `params` | Route values |
| `query` | Query string values |
| `body` | Request body |
| `headers` | Selected safe request headers |

### Event, Job, and Worker Context

| Script type | Additional globals |
|-------------|--------------------|
| Event handler | `handler`, `event`, `eventName`, `eventData` |
| Background job | `job`, `jobName`, `jobData`, `jobJsonData` |
| Background worker | `worker`, `workerName` |

## Service Helpers

### HTTP

The `http` helper supports outbound requests with timeout, response-size, host, and HTTPS policy checks.

| Method | Description |
|--------|-------------|
| `http.getAsync(url, options?)` | GET |
| `http.postAsync(url, body?, options?)` | POST |
| `http.putAsync(url, body?, options?)` | PUT |
| `http.patchAsync(url, body?, options?)` | PATCH |
| `http.deleteAsync(url, options?)` | DELETE |
| `http.requestAsync(method, url, bodyOrOptions?, options?)` | Custom method |

Options include `headers`, `query`, `timeoutMs`, `contentType`, and `responseType` (`json`, `text`, or `base64`).

### Authorization, Settings, Features, and Config

```javascript
if (await auth.isGrantedAsync('Acme.Campaigns.Create')) {
    var enabled = await features.isEnabledAsync('Acme.Campaigns');
    var threshold = await settings.getIntAsync('Acme.Campaigns.Threshold', 10);
    var baseUrl = config.get('ExternalApi:BaseUrl');
}
```

### Events and Background Jobs

```javascript
await events.publishAsync('Acme.Campaigns.CampaignCompleted', { id: campaignId });

await jobs.enqueueAsync('SendCampaignSummary', { campaignId: campaignId }, {
    priority: 'Normal',
    delayMs: 60000
});
```

### Files, Images, and Attachments

The file helpers use low-code page services so permissions, file validation, linked-blob checks, and foreign access stay consistent with the runtime.

| Helper | Purpose |
|--------|---------|
| `files.parse(value)` | Parse a stored file value |
| `files.format(value, includeSize?)` | Format a file display value |
| `files.save(...)` / `images.save(...)` | Save file or image content |
| `files.get(...)` / `images.get(...)` | Read file or image content |
| `files.upload(entityName, fieldName, fileInput, options?)` | Upload field content |
| `attachments.list(...)` | List record attachments |
| `attachments.upload(...)` / `attachments.save(...)` | Upload a record attachment |
| `attachments.get(...)` / `attachments.download(...)` | Download a record attachment |
| `attachments.delete(...)` | Delete a record attachment |

File content is passed as base64 data. File operations are subject to configured read/write size limits.

## Configuration

Configure scripting limits with the `LowCode:Scripting` configuration section or `AbpLowCodeScriptingOptions`.

```csharp
Configure<AbpLowCodeScriptingOptions>(options =>
{
    options.Script.Timeout = TimeSpan.FromMinutes(1);
    options.Script.MaxStatements = 100_000;
    options.Script.MaxMemoryBytes = 128 * 1024 * 1024;
    options.Script.MaxRecursionDepth = 64;

    options.Query.MaxLimit = 10_000;
    options.Query.DefaultLimit = 1000;
    options.Query.MaxExpressionNodes = 200;
    options.Query.MaxExpressionDepth = 10;
    options.Query.MaxArraySize = 500;
    options.Query.MaxGroupCount = 500;

    options.Capabilities.Endpoint.EnableHttp = false;
});
```

Most numeric limits can be set to `null` to explicitly disable that limit. Keep the defaults for untrusted or tenant-authored scripts.

### Capability Profiles

The same services are not required in every script type. Capability profiles let you disable services per execution type:

```json
{
  "LowCode": {
    "Scripting": {
      "Capabilities": {
        "Interception": {
          "EnableDb": true,
          "EnableHttp": false
        },
        "Endpoint": {
          "EnableDb": true,
          "EnableHttp": true
        },
        "EventHandler": {
          "EnableDb": true
        },
        "BackgroundJob": {
          "EnableDb": true
        },
        "BackgroundWorker": {
          "EnableDb": true
        }
      }
    }
  }
}
```

Each profile supports flags such as `EnableDb`, `EnableCurrentUser`, `EnableCurrentTenant`, `EnableEmail`, `EnableConfig`, `EnableHttp`, `EnableAuthorization`, `EnableSettings`, `EnableFeatures`, `EnableEvents`, `EnableBackgroundJobs`, `EnableEncryption`, `EnableTextTemplating`, `EnableBlob`, and `EnableFiles`.

## Security

### Sandbox Constraints

| Constraint | Default | Configurable |
|------------|---------|--------------|
| Script Timeout | 30 seconds | Yes |
| Max Statements | 100,000 | Yes |
| Memory Limit | 128 MB | Yes |
| Recursion Depth | 64 | Yes |
| Max Script Length | 500,000 characters | Yes |
| CLR Access | Disabled | No |

### Query Security Limits

| Limit | Default | Description |
|-------|---------|-------------|
| MaxExpressionNodes | 200 | Max AST nodes per expression |
| MaxExpressionDepth | 10 | Max nesting depth |
| MaxLimit (take) | 10,000 | Max records per query |
| DefaultLimit | 1,000 | Default if `take()` is not specified |
| MaxArraySize (includes) | 500 | Max array size for IN operations |
| MaxGroupCount | 500 | Max groups in GroupBy |

### Integration Limits

| Area | Default |
|------|---------|
| HTTP timeout | 30 seconds |
| HTTP response size | 5 MB |
| HTTP requests per execution | 50 |
| HTTP blocked hosts | `localhost` and private IP ranges |
| Email sends per execution | 5 |
| Blob read/write size | 10 MB read, 5 MB write |
| Low-code file read/write size | 10 MB read, 5 MB write |
| Event publishes per execution | 10 |
| Background jobs per execution | 10 |
| Endpoint response body | 1 MB |

### Property Whitelist

Only properties defined in the entity model can be queried. Accessing undefined properties throws a `SecurityException`.

### SQL Injection Protection

All values are parameterized:

```javascript
var malicious = "'; DROP TABLE Products;--";
// Safely treated as a literal string — no injection
var query = await db.query('Entity');
var result = await query.where(x => x.Name.includes(malicious)).count();
```

### Blocked Features

The following are **not allowed** inside lambda expressions: `typeof`, `instanceof`, `in`, bitwise operators, `eval()`, `Function()`, `new RegExp()`, `new Date()`, `console.log()`, `setTimeout()`, `globalThis`, `window`, `__proto__`, `constructor`, `prototype`, `Reflect`, `Proxy`, `Symbol`.

## Error Handling

```javascript
// Abort operation with error
if (!context.commandArgs.getValue('Email').includes('@')) {
    throw new Error('Valid email is required');
}

// User-friendly ABP exception
userFriendlyError('The campaign is not ready to publish.');

// Business exception with a code
businessError('Budget is exceeded.', 'Acme.Campaigns:BudgetExceeded');

// Try-catch for safe execution
try {
    var query = await db.query('Entity');
    var products = await query.where(x => x.Price > 0).toList();
} catch (error) {
    context.log('Query failed: ' + error.message);
}
```

## Best Practices

1. **Use specific filters** — avoid querying all records without `where()`
2. **Set limits** — always use `take()` to limit results
3. **Validate early** — check inputs at the start of scripts
4. **Use `first()` for single results** — instead of `toList()[0]`
5. **Keep scripts focused** — one responsibility per interceptor
6. **Use `context.log()`** — never `console.log()`
7. **Handle nulls** — check for null before property access

## Examples

### Inventory Check on Order Creation

```javascript
// Pre-create interceptor for Order
var productId = context.commandArgs.getValue('ProductId');
var quantity = context.commandArgs.getValue('Quantity');

var productQuery = await db.query('LowCodeDemo.Products.Product');
var product = await productQuery
    .where(x => x.Id === productId)
    .first();

if (!product) { throw new Error('Product not found'); }
if (product.StockCount < quantity) { throw new Error('Insufficient stock'); }

context.commandArgs.setValue('TotalAmount', product.Price * quantity);
```

### Sales Dashboard (Custom Endpoint)

```javascript
var orderQuery = await db.query('LowCodeDemo.Orders.Order');

var totalOrders = await orderQuery.count();
var delivered = await orderQuery
    .where(x => x.IsDelivered === true).count();
var revenue = await orderQuery
    .where(x => x.IsDelivered === true).sum(x => x.TotalAmount);

return ok({
    orders: totalOrders,
    delivered: delivered,
    revenue: revenue
});
```

## See Also

* [Interceptors](interceptors.md)
* [Custom Endpoints](custom-endpoints.md)
* [model.json Structure](model-json.md)
