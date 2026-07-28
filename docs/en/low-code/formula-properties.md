```json
//[doc-seo]
{
    "Description": "Define provider-side calculated properties in the ABP Low-Code System with Power Fx-like expressions."
}
```

# Formula Properties

Formula properties are calculated properties in the Low-Code property model. Their values are materialized by the server, so filtering, sorting, paging, and projections continue to use the database provider instead of loading the whole entity set into application memory.

> **Preview:** Formula properties and the expression profile are preview features. Supported functions and provider translation may change before general availability.

## Create a formula property

In the Low-Code Designer, open the normal **Add Property** flow, select the result type, and enable **Calculate with a formula**. The formula result uses the selected property type:

* String
* Int or Long
* Decimal or Money
* Boolean
* Date or DateTime

Formula values are server-authoritative. Values supplied by the client are ignored for the calculated property.

## Expression syntax

See the [Low-Code Expression Language](expression-language.md) reference for operators, functions, literals, `With`, and provider translation rules.

## Storage and mapping

Formula storage is independent of the expression language. A formula property can be JSON-backed or mapped to a physical database column. Mapped properties use the public property name as their column identity and are recalculated on create, update, and deployment backfill.

Formula properties may reference both mapped and JSON-backed fields. For existing-data mapping, choose a fixed value or **Formula** backfill. Required database columns remain nullable during schema evolution unless an explicit backfill is requested.

## Query and performance behavior

Formula updates are set-based and provider-side. The server recalculates affected rows and refreshes only the bounded saved-key batch needed by the current operation. It does not fetch the entire table into memory.

Materialized results can be used by normal database filtering, sorting, paging, count, projection, and supported aggregates. Cross-row lookups, arbitrary SQL, network calls, and aggregate queries inside a row formula are outside the v1 profile.

## Validation and deployment

The Designer validates syntax, referenced fields, result type, dependencies, and provider-translatable operations before deployment. A syntactically valid expression is not active until deployment/backfill succeeds. Failed deployments can be retried from the Designer.

Common validation errors include unknown fields or functions, result type mismatches, a `With` variable colliding with a property name, unsupported provider operations, and a missing expression when Formula backfill is selected.
