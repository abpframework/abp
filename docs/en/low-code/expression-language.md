```json
//[doc-seo]
{
    "Description": "Reference for the provider-safe ABP Low-Code expression language used by virtual calculated properties and formula backfills."
}
```

# Low-Code Expression Language

The Low-Code expression language is a provider-safe scalar profile used by virtual calculated properties and by the one-time **Formula** option for existing-data backfill. Its syntax is intentionally familiar to Power Fx users, but it is a smaller language designed for server validation and database-provider translation.

Expressions are not JavaScript. They cannot contain arbitrary code, SQL, network calls, browser APIs, side effects, or unsupported Power Fx table and record operations.

An expression can contain up to 4096 characters.

> **Preview:** The language profile is preview functionality. The supported syntax and function set may change before general availability.

## Values and field references

Use invariant numeric literals, quoted strings, Boolean values, and date constructors. Reference a property by name:

```text
UnitPrice * Quantity
If(IsActive, "Enabled", "Disabled")
Date(2026, 8, 4)
```

Property and local names that are not simple identifiers can be enclosed in single quotes. Escape a single quote by doubling it:

```text
'Unit Price' * Quantity
'Manager''s Price' * Quantity
```

Names and function names are case-insensitive. Fields may be JSON-backed or mapped to database columns.

### Related fields

Use dot notation to traverse a configured foreign key and read a scalar field from the related record:

```text
CustomerId.CreditLimit
If(CustomerId.IsPreferred, Amount * 90%, Amount)
```

Related paths may also contain quoted identifiers. The Designer loads the available fields for each relationship level and applies the backend's configured maximum traversal depth. Missing related values produce a blank result where the expression is nullable.

## Operators

| Purpose | Operators and forms |
| --- | --- |
| Arithmetic | `+`, `-`, `*`, `/` |
| Comparison | `=`, `==`, `<>`, `!=`, `<`, `<=`, `>`, `>=` |
| Logical | `And(a, b, ...)`, `Or(a, b, ...)`, `Not(a)`, `&&`, `||`, `!` |
| Text concatenation | `&` |
| Percentage | `10%` (equivalent to `10 / 100`) |

`<>` and `!=` are equivalent not-equal operators. `=` and `==` are equivalent equality operators. `%` is the postfix percentage operator, not a modulo operator. Use parentheses when combining operations so the intended precedence is explicit.

Division returns a nullable Decimal result because division by zero produces blank rather than forcing client-side evaluation.

## Functions

The current scalar profile supports these functions:

| Category | Functions |
| --- | --- |
| Conditional and blank values | `If(condition, trueValue, falseValue)`, `Coalesce(value, fallback)`, `IsBlank(value)` |
| Logical | `And(condition1, condition2, ...)`, `Or(condition1, condition2, ...)`, `Not(condition)` |
| Numeric | `Abs(number)`, `Round(number, places)`, `Min(left, right)`, `Max(left, right)` |
| Text | `Lower(text)`, `Upper(text)`, `Trim(text)`, `Len(text)`, `Left(text, length)`, `Right(text, length)`, `Mid(text, start[, length])` |
| Date and time | `Year(value)`, `Month(value)`, `Day(value)`, `Date(year, month, day)`, `DateTime(year, month, day, hour, minute, second[, millisecond])` |

Examples:

```text
If(Len(Name) > 5, "Long", "Short")
Coalesce(Discount, 0)
Round(UnitPrice * Quantity, 2)
FirstName & " " & LastName
Mid(ProductCode, 2, 3)
```

`Round` uses midpoint-away-from-zero semantics. `Mid` uses a one-based start position. `Date` and `DateTime` require literal numeric components in the provider-neutral profile. Numeric and date literals use invariant syntax; browser and database locale settings do not change their meaning.

Functions from the full Power Fx language that are not listed here are rejected. For example, `Floor`, `Ceiling`, `Concat`, and `Substring` are not aliases for the supported scalar functions.

## Local values with `With`

Use `With` to define immutable local values and avoid repeating an expression:

```text
With(
  {
    subtotal: UnitPrice * Quantity,
    rebate: Coalesce(Discount, 0)
  },
  If(subtotal > 100, Round(subtotal - rebate, 2), subtotal)
)
```

A `With` record supports up to 16 bindings, and `With` expressions can be nested up to 8 levels. A local name must not collide with a property on the current entity. Bindings in the same record do not see one another; nest another `With` when a later value must use an earlier local.

## Where expressions run

For a calculated property, the expression is expanded into the EF Core query. It can therefore participate in provider-side filtering, sorting, paging, count, projection, grouping, and supported aggregates without creating a physical column or loading the complete table into memory.

For an ordinary property mapping that uses **Formula** existing-data backfill, the same scalar profile is compiled into a provider-side update that initializes existing rows once. The mapped property then stores the result; this is separate from a virtual calculated property.

During JSON-to-database mapping, use `Self` to read the property's current JSON value before it is moved to the database column:

```text
Coalesce(Self, "Unknown")
Self & " migrated"
```

Related-record aggregates are not written inside a formula expression. Create a [Rollup Property](formula-properties.md#create-a-rollup-property) for `Count`, `Sum`, `Average`, `Min`, or `Max` over related records.

## Validation errors

The Designer validates syntax, field and related-field references, function arity and argument types, inferred result type, dependency cycles, server-only exposure, and translation by the active database provider. Validation covers transitive calculated dependencies, not only the expression currently being edited.

Common errors include:

* unknown fields or functions
* incompatible branch or result types
* a local name that collides with an entity property
* a circular formula or rollup dependency
* a related path that exceeds backend query capabilities
* an operation that the active provider cannot translate

Provider translation failure is a validation error. The runtime does not fall back to evaluating the entire entity set in application memory.

See [Calculated and Rollup Properties](formula-properties.md) for the Designer workflows and query behavior.
