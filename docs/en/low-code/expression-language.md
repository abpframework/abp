```json
//[doc-seo]
{
    "Description": "Reference for the ABP Low-Code expression language used by calculated properties, mappings, defaults, and validations."
}
```

# Low-Code Expression Language

The Low-Code expression language is a small, provider-safe language used in more than one designer workflow. It is used by formula properties and can also be used where the designer asks for an expression-backed value, such as mapped-property initial values or existing-data backfills.

The v1 syntax is intentionally familiar to Excel, Airtable, and Power Fx users while remaining translatable to a server-side expression tree. Expressions are evaluated by the server; they are not JavaScript and must not contain arbitrary code, SQL, network calls, or browser APIs.

> **Preview:** The language profile is preview functionality. The supported function set is deliberately smaller than the complete Excel or Power Fx languages.

## Values and field references

Use numbers, quoted strings, Boolean values, and date constructors as literals. Reference a property by its name:

```text
UnitPrice * Quantity
If(IsActive, "Enabled", "Disabled")
Date(2026, 7, 28)
```

Property names containing spaces can be enclosed in single quotes:

```text
'Unit Price' * Quantity
```

Names and function names are case-insensitive. A field reference is resolved against the current entity, including fields backed by JSON and fields mapped to database columns.

## Operators

| Purpose | Operators and forms |
| --- | --- |
| Arithmetic | `+`, `-`, `*`, `/`, `%` |
| Comparison | `=`, `<>`, `!=`, `<`, `<=`, `>`, `>=` |
| Logical | `And(a, b)`, `Or(a, b)`, `Not(a)`, `&&`, `||`, `!` |
| Text concatenation | `&` |
| Percentage | `10%` (equivalent to `10 / 100`) |

`<>` and `!=` are equivalent not-equal operators. Use parentheses when combining arithmetic, comparison, and logical operators so the intended order is clear.

## Functions

The v1 profile supports the following functions:

```text
If, Coalesce, IsBlank,
Abs, Round, Floor, Ceiling, Min, Max,
Concat, Lower, Upper, Trim, Len,
Left, Right, Mid, Substring,
Year, Month, Day, Date, DateTime
```

Examples:

```text
If(Len(Name) > 5, "Long", "Short")
Coalesce(Discount, 0)
Round(UnitPrice * Quantity, 2)
Concat(FirstName, " ", LastName)
```

`Round(value, places)` uses midpoint-away-from-zero semantics. Numeric and date literals use invariant syntax; the browser or database locale does not change their meaning.

## Local values with `With`

Use `With` to name intermediate values and avoid repeating calculations:

```text
With(
  {
    subtotal: UnitPrice * Quantity,
    rebate: Coalesce(Discount, 0)
  },
  If(subtotal > 100, Round(subtotal - rebate, 2), subtotal)
)
```

Local variable names must not match properties on the current entity. This rule prevents an ambiguous reference during server-side translation.

## Where expressions run

The expression is compiled and validated on the server. For materialized values, writes are performed as provider-side operations so filtering, sorting, paging, and projections remain database operations. The server does not load the complete table into application memory to calculate a column.

The language does not support cross-row lookups, arbitrary SQL, JavaScript, network calls, or aggregate queries inside a row expression. Use a relation/query endpoint or a custom server endpoint for those scenarios.

## Validation errors

The designer checks syntax, field references, function names, result type, dependencies, and provider translation before publishing an expression. Common errors are:

* unknown property or function
* incompatible result type
* a `With` variable that collides with an entity property
* an operation that the active provider cannot translate
* a missing expression in a Formula backfill configuration

See [Formula Properties](formula-properties.md) for the materialized-column workflow and storage behavior.
