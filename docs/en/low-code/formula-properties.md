```json
//[doc-seo]
{
    "Description": "Create virtual formula and rollup properties in the ABP Low-Code System with provider-translated expressions and related-record aggregates."
}
```

# Calculated and Rollup Properties

Calculated properties are server-authoritative, virtual properties in the Low-Code entity model. They are evaluated as part of the database query and do not create physical database columns.

The Designer supports two kinds of calculated property:

* **Calculated Property** evaluates a scalar formula from fields on the current record, other calculated properties, or fields reached through a foreign key.
* **Rollup Property** aggregates records from an entity that has a foreign key to the current entity.

Both kinds can participate in filtering, sorting, paging, count, projection, grouping, and supported aggregates while the work remains in the database provider. The complete entity set is not loaded into application memory to calculate the values.

> **Preview:** Calculated properties, rollups, and the expression profile are preview features. Supported operations and provider translation may change before general availability.

## Availability

Calculated and rollup properties can be authored in a writable Runtime JSON-backed layer that supports direct model changes. The corresponding commands are disabled when the selected Designer layer does not support them.

## Create a calculated property

In the Low-Code Designer:

1. Open **Data**, select an entity, and open its **Properties** tab.
2. Open **Add Property** and select **Calculated Property**.
3. Enter the property name and an optional display name.
4. Enter an expression such as `Round(UnitPrice * Quantity, 2)`.
5. Review the inferred result type and validation result, then select **Create Calculated Property**.

The result type is inferred from the expression. Supported property types are String, Int, Long, Decimal, Money, Boolean, Date, and DateTime. Decimal and Money results can also define display precision, and Money results can define a currency symbol.

Formula properties may use both JSON-backed and database-mapped scalar fields. Use dot notation to read a scalar field through a foreign key:

```text
CustomerId.CreditLimit
Round(CustomerId.CreditLimit - CurrentBalance, 2)
```

The formula editor offers fields, related fields, local values, and supported functions as suggestions. See the [Low-Code Expression Language](expression-language.md) reference for the complete scalar syntax.

Client applications cannot set a calculated property. Enable **Server only** when the result must also be omitted from client-facing metadata and responses. A client-visible formula cannot expose a server-only dependency; a server-only formula may use server-only fields.

## Create a rollup property

A rollup evaluates a correlated aggregate over related records. For example, an `Order` can expose the sum of `OrderItem.LineTotal` values when `OrderItem.OrderId` is a foreign key to `Order`.

1. Open **Add Property** and select **Rollup Property**.
2. Select the **Source Entity** that contains the related records.
3. Select the **Relation Field** whose foreign key points to the current entity.
4. Select an operation: `Count`, `Sum`, `Average`, `Min`, or `Max`.
5. For every operation except `Count`, select the **Value Field**.
6. Review validation and select **Create Rollup Property**.

`Count` returns a Long value and does not use a value field. `Sum` and `Average` require a numeric value field. `Min` and `Max` preserve a compatible scalar type. A rollup value may be a normal field or a formula property, but it cannot be another rollup.

## Storage and query behavior

Calculated and rollup properties always remain virtual:

* `isMappedToDbField` is false.
* Values are not stored in JSON or in a physical column.
* No schema migration, synchronization job, or existing-data backfill is required.
* Required, unique, default-value, and client-write settings do not apply.

At query time, the EF Core provider expands formulas into SQL-translatable expressions and rollups into correlated aggregate expressions. Calculated dependencies are expanded recursively. Calculations that are not needed by search, filtering, or sorting can be evaluated after page selection while still remaining provider-side.

This is different from the one-time **Formula** option used to backfill an ordinary property while mapping it to a database field. That workflow writes existing rows; a calculated property remains virtual and is evaluated from current data whenever it is queried.

## Validation and dependency safety

Before a calculated property is saved, the Designer validates:

* syntax, field paths, functions, and argument types
* inferred result type and display metadata
* direct and transitive dependencies
* circular dependencies across formulas and rollups
* server-only dependency exposure
* translation by the active database provider

Saving publishes the calculated metadata only after the complete affected dependency closure passes provider validation. Renaming or deleting fields that are still referenced is guarded so an existing calculation is not silently broken.

Provider-specific translation remains authoritative. An expression that is syntactically valid but cannot be translated by the active provider is rejected instead of falling back to full-table client-side evaluation.

## Current limitations

Formula expressions are scalar. They do not contain arbitrary aggregate subqueries; use a Rollup Property for a supported related-record aggregate. Arbitrary SQL, JavaScript, network calls, browser APIs, side effects, and unsupported Power Fx table or record operations are not allowed.

Related-field access must follow configured foreign keys and stay within the query capability exposed by the backend. Rollups require a source-side Guid foreign key that points to the entity receiving the rollup.
