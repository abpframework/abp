```json
//[doc-seo]
{
    "Description": "Server-side JavaScript Scripting API for ABP Low-Code System. Query, filter, aggregate data and perform CRUD operations with database-level execution."
}
```

# Scripting API

The Low-Code System provides a server-side JavaScript scripting engine for executing custom business logic within [interceptors](interceptors.md) and [custom endpoints](custom-endpoints.md). Scripts run in a sandboxed environment with access to a database API backed by EF Core.

## Unified Database API (`db`)

The `db` object is the main entry point for all data operations.

### Key Design Principles

* **Immutable Query Builder** — each query method returns a new builder instance. The original is never modified.
* **Database-Level Execution** — all operations (filters, aggregations, joins, set operations) translate to SQL via EF Core and Dynamic LINQ.
* **No in-memory processing** of large datasets.

```javascript
// Immutable pattern — each call creates a new builder
var baseQuery = db.query('Entity').where(x => x.Active);
var cheap = baseQuery.where(x => x.Price < 100);     // baseQuery unchanged
var expensive = baseQuery.where(x => x.Price > 500);  // baseQuery unchanged
```

## Query API

### Basic Queries

```javascript
var products = await db.query('LowCodeDemo.Products.Product')
    .where(x => x.Price > 100)
    .orderBy(x => x.Price)
    .take(10)
    .toList();

var result = await db.query('LowCodeDemo.Products.Product')
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

var result = await db.query('Entity').where(x => x.Price > minPrice).toList();
var result2 = await db.query('Entity').where(x => x.StockCount > config.minStock).toList();
var result3 = await db.query('Entity').where(x => x.Price >= nested.range.min).toList();
```

### Contains / IN Operator

```javascript
var targetPrices = [50, 100, 200];
var products = await db.query('Entity')
    .where(x => targetPrices.includes(x.Price))
    .toList();
```

### Select Projection

```javascript
var projected = await db.query('LowCodeDemo.Products.Product')
    .where(x => x.Price > 0)
    .select(x => ({ ProductName: x.Name, ProductPrice: x.Price }))
    .toList();
```

## Joins

### Explicit Joins

```javascript
var orderLines = await db.query('LowCodeDemo.Orders.OrderLine')
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
var orders = await db.query('LowCodeDemo.Orders.Order')
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
db.query('Order')
  .join('LowCodeDemo.Products.Product',
        o => o.ProductId,
        p => p.Id)
```

### Join with Filtered Query

```javascript
var expensiveProducts = db.query('Product').where(p => p.Price > 100);

var orders = await db.query('OrderLine')
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
var cheap = db.query('Product').where(x => x.Price <= 100);
var popular = db.query('Product').where(x => x.Rating > 4);

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
var totalValue = await db.query('Product').sum(x => x.Price);
var avgPrice = await db.query('Product').where(x => x.InStock).average(x => x.Price);
var cheapest = await db.query('Product').min(x => x.Price);
```

### GroupBy with Select

```javascript
var grouped = await db.query('Product')
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
var grouped = await db.query('Product')
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
var products = await db.query('Product')
    .where(x => Math.round(x.Price) > 100)
    .toList();

var result = await db.query('Product')
    .where(x => Math.abs(x.Balance) < 10 && Math.floor(x.Rating) >= 4)
    .toList();
```

## CRUD API

Direct CRUD methods on the `db` object:

| Method | Description | Returns |
|--------|-------------|---------|
| `db.get(entityName, id)` | Get by ID | `Promise<object\|null>` |
| `db.getCount(entityName)` | Get count | `Promise<number>` |
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

## Context Object

Available in [interceptors](interceptors.md):

| Property | Type | Description |
|----------|------|-------------|
| `context.commandArgs` | object | Command arguments (data, entityId, commandName, entityName) |
| `context.commandArgs.getValue(name)` | function | Get property value |
| `context.commandArgs.setValue(name, value)` | function | Set property value |
| `context.commandArgs.hasValue(name)` | function | Check if a property exists |
| `context.commandArgs.removeValue(name)` | function | Remove a property value |
| `context.currentUser` | object | Current user info (see [Interceptors](interceptors.md) for full list) |
| `context.emailSender` | object | Email sending (`sendAsync`, `sendHtmlAsync`) |
| `context.log(msg)` | function | Log an informational message |
| `context.logWarning(msg)` | function | Log a warning message |
| `context.logError(msg)` | function | Log an error message |

## Configuration

You can configure scripting limits using `AbpLowCodeScriptingOptions` in your module's `ConfigureServices` method:

```csharp
Configure<AbpLowCodeScriptingOptions>(options =>
{
    // Script execution limits (null = no limit)
    options.Script.Timeout = TimeSpan.FromMinutes(1);
    options.Script.MaxStatements = 100_000;
    options.Script.MaxMemoryBytes = 512 * 1024 * 1024; // 512 MB
    options.Script.MaxRecursionDepth = 500;

    // Query API limits (null = no limit)
    options.Query.MaxLimit = 10_000;
    options.Query.DefaultLimit = 1000;
    options.Query.MaxExpressionNodes = 200;
    options.Query.MaxExpressionDepth = 20;
    options.Query.MaxArraySize = 500;
    options.Query.MaxGroupCount = 500;
});
```

All limits default to `null` (no limit). Configure them based on your security requirements and expected workload.

## Security

### Sandbox Constraints

| Constraint | Default | Configurable |
|------------|---------|--------------|
| Script Timeout | No limit | Yes |
| Max Statements | No limit | Yes |
| Memory Limit | No limit | Yes |
| Recursion Depth | No limit | Yes |
| CLR Access | Disabled | No |

### Query Security Limits

| Limit | Default | Description |
|-------|---------|-------------|
| MaxExpressionNodes | No limit | Max AST nodes per expression |
| MaxExpressionDepth | No limit | Max nesting depth |
| MaxLimit (take) | No limit | Max records per query |
| DefaultLimit | No limit | Default if `take()` not specified |
| MaxArraySize (includes) | No limit | Max array size for IN operations |
| MaxGroupCount | No limit | Max groups in GroupBy |

### Property Whitelist

Only properties defined in the entity model can be queried. Accessing undefined properties throws a `SecurityException`.

### SQL Injection Protection

All values are parameterized:

```javascript
var malicious = "'; DROP TABLE Products;--";
// Safely treated as a literal string — no injection
var result = await db.query('Entity').where(x => x.Name.includes(malicious)).count();
```

### Blocked Features

The following are **not allowed** inside lambda expressions: `typeof`, `instanceof`, `in`, bitwise operators, `eval()`, `Function()`, `new RegExp()`, `new Date()`, `console.log()`, `setTimeout()`, `globalThis`, `window`, `__proto__`, `constructor`, `prototype`, `Reflect`, `Proxy`, `Symbol`.

## Error Handling

```javascript
// Abort operation with error
if (!context.commandArgs.getValue('Email').includes('@')) {
    throw new Error('Valid email is required');
}

// Try-catch for safe execution
try {
    var products = await db.query('Entity').where(x => x.Price > 0).toList();
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

var product = await db.query('LowCodeDemo.Products.Product')
    .where(x => x.Id === productId)
    .first();

if (!product) { throw new Error('Product not found'); }
if (product.StockCount < quantity) { throw new Error('Insufficient stock'); }

context.commandArgs.setValue('TotalAmount', product.Price * quantity);
```

### Sales Dashboard (Custom Endpoint)

```javascript
var totalOrders = await db.query('LowCodeDemo.Orders.Order').count();
var delivered = await db.query('LowCodeDemo.Orders.Order')
    .where(x => x.IsDelivered === true).count();
var revenue = await db.query('LowCodeDemo.Orders.Order')
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
