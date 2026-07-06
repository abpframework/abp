```json
//[doc-seo]
{
    "Description": "Use the ABP Low-Code Designer MCP integration to inspect, validate, and apply structured runtime mutations to the database-backed low-code model."
}
```

# MCP Integration

> **Preview:** The low-code MCP surface is part of the preview Low-Code System. Tool names, mutation metadata, and validation advice may change before general availability.

The low-code designer exposes a Model Context Protocol (MCP) surface for automation. It is intended for agents and scripted tooling that need to inspect or mutate the same **runtime database-backed** model that the Designer edits in the **Runtime JSON** layer.

## Scope

The MCP surface is **runtime-only**:

* It always works against the database-backed runtime model.
* It does not choose between layers at call time.
* It is not the right surface for source-controlled `_Dynamic/model/**/*.json` files.

Use [Model Descriptor Files](model-json.md) when you need source-controlled descriptors. Use MCP when an agent or automation needs safe, structured changes to the runtime model that the Designer can immediately show.

## Tool Reference

All tools below work against the **runtime database-backed** model only.

### Core Workflow Tools

| Tool | Use it for |
|------|------------|
| `lowcode_designer_get_capabilities` | Read the server's runtime-only scope, write rules, recommended workflow, and example mutation targets |
| `lowcode_designer_get_mutation_metadata` | Fetch the current `concurrencyStamp` and the valid semantic target tree before planning a write |
| `lowcode_designer_validate_mutations` | Dry-run a mutation batch and get structured validation feedback without writing |
| `lowcode_designer_apply_mutations` | Apply a validated mutation batch to the runtime model; this is the **only write tool** |
| `lowcode_designer_get_health_snapshot` | Re-check the full runtime model after changes to catch broken references or invalid layouts |

### Entity and Enum Tools

| Tool | Use it for |
|------|------------|
| `lowcode_designer_get_entities` | List runtime entities; optionally include configured reference entities |
| `lowcode_designer_get_entity` | Read one entity with properties, validators, interceptors, and child metadata |
| `lowcode_designer_get_entity_delete_impact` | Review references that would break or change if an entity is removed |
| `lowcode_designer_get_enum_types` | List available enum types |
| `lowcode_designer_get_enum` | Read one enum and its values |
| `lowcode_designer_get_enum_delete_impact` | Review what depends on an enum before removing it |

### Page, Form, and Permission Tools

| Tool | Use it for |
|------|------------|
| `lowcode_designer_get_page_groups` | List page groups used for runtime navigation folders |
| `lowcode_designer_get_pages` | List pages across page types, including dashboard pages |
| `lowcode_designer_get_page` | Read one page descriptor, including columns, filters, dashboard config, and form links |
| `lowcode_designer_get_page_permission_config` | Read the page's permission bindings before changing access rules |
| `lowcode_designer_get_forms` | List forms |
| `lowcode_designer_get_form` | Read one form with fields, flat layout placements, validations, and rules |
| `lowcode_designer_get_form_delete_impact` | Review what depends on a form before removing it |
| `lowcode_designer_get_permissions` | List runtime low-code permission definitions |
| `lowcode_designer_get_permission_delete_impact` | Review where a permission is referenced before removing it |

### Script and Action Tools

| Tool | Use it for |
|------|------------|
| `lowcode_designer_get_endpoints` | List scripted custom endpoints |
| `lowcode_designer_get_endpoint` | Read one endpoint with route, method, JavaScript, and analyzed references |
| `lowcode_designer_get_script_autocomplete_metadata` | Discover the script globals, helpers, entities, and metadata available for a script type |
| `lowcode_designer_test_script` | Dry-run endpoint, interceptor, event handler, background job, or background worker JavaScript |
| `lowcode_designer_get_script_event_handlers` | List script event handlers |
| `lowcode_designer_get_script_event_handler` | Read one event handler |
| `lowcode_designer_get_script_background_jobs` | List script background jobs |
| `lowcode_designer_get_script_background_job` | Read one background job |
| `lowcode_designer_get_script_background_worker_scheduler_capabilities` | Read whether the runtime supports dynamic worker registration and cron scheduling |
| `lowcode_designer_get_script_background_workers` | List script background workers |
| `lowcode_designer_get_script_background_worker` | Read one background worker with schedule and JavaScript |

## Common Sequences

### Change an Entity or Form

1. Read the current item with `lowcode_designer_get_entity` or `lowcode_designer_get_form`.
2. Fetch `lowcode_designer_get_mutation_metadata`.
3. Build a small mutation batch against the returned target tree.
4. Run `lowcode_designer_validate_mutations`.
5. Apply with `lowcode_designer_apply_mutations`.
6. Re-read the item and run `lowcode_designer_get_health_snapshot`.

### Change a Dashboard or Page Group

1. Read the page with `lowcode_designer_get_page` or list groups with `lowcode_designer_get_page_groups`.
2. Fetch mutation metadata and validate the exact batch.
3. Apply the change.
4. Re-open the page via `lowcode_designer_get_page` and confirm health.

### Remove a Shared Item Safely

1. Call the matching delete-impact tool first, such as `lowcode_designer_get_entity_delete_impact`, `lowcode_designer_get_enum_delete_impact`, `lowcode_designer_get_form_delete_impact`, or `lowcode_designer_get_permission_delete_impact`.
2. Fix or remove dependent references.
3. Validate the removal batch.
4. Apply and then re-check health.

### Review or Debug a Script

1. Read the action or endpoint with its `get_*` tool.
2. Fetch `lowcode_designer_get_script_autocomplete_metadata` for the relevant script type.
3. Run `lowcode_designer_test_script`.
4. Persist the change through mutations only after the dry run looks correct.

## Mental Model

MCP is not a raw JSON document editor. It is a semantic mutation layer over low-code concepts such as:

* Entities and properties
* Pages, page groups, and dashboards
* Forms and form layout
* Permissions
* Custom endpoints and script actions

The MCP surface includes read/query capabilities, mutation metadata, dry-run validation, a single mutation apply path, and a health snapshot. The normal workflow is:

1. Read the current runtime model state for the item you want to change.
2. Fetch the latest mutation metadata and keep its `concurrencyStamp`.
3. Build a small ordered mutation batch that touches only the changed paths.
4. Dry-run the batch with validation.
5. Apply the batch.
6. Re-read the changed item and review [Health](health.md).

## Concurrency and Validation

Every runtime write depends on the latest `concurrencyStamp`. If another write changes the runtime model first, your apply attempt becomes stale and must be rebuilt from the refreshed model state.

Validation happens before apply and returns structured feedback instead of requiring a client to guess from generic failures. The validation layer checks:

* Stale concurrency stamps
* Target path syntax
* Missing required input such as `data` or move destinations
* Primitive-only `Set` data
* Final descriptor shape after the batch is simulated
* Entity property type and foreign key rules
* Form layout consistency
* Dashboard layout consistency
* Runtime override rules for descriptors inherited from lower layers

Treat validation as part of the normal write flow, not as an optional extra.

## Mutation Model

Runtime writes are intentionally narrow:

* `Add` creates a keyed descriptor or collection member.
* `Set` changes a scalar value or a supported primitive list.
* `Remove` deletes the semantic target.
* `Move` relocates an existing node without resending the whole object.

Important behavior:

* `Set` data must stay primitive: string, number, boolean, `null`, or a supported primitive string list.
* Do not replace whole descriptor trees when only one field changed.
* Keep batches small and semantic. Send changed paths, not full model payloads.
* If a mapped entity or property is removed with drop behavior, the runtime model can also remove the physical table or column in the runtime layer.

## Runtime Override and Schema Rules

Runtime MCP writes can extend descriptors that originate from lower layers, but they do not have unlimited control over inherited schema.

For entity properties inherited from a lower layer, runtime edits are intentionally restricted. Typical runtime-safe changes are display labels and runtime-owned validators. Inherited storage details such as type, database mapping, default value, required or unique behavior, foreign key settings, and file or image storage options remain immutable in runtime.

This separation matches the low-code layer model:

* Runtime database-backed changes use **direct** schema mutation.
* Source-controlled JSON layer changes use the normal **migration** workflow.

If you are working in `_Dynamic/model`, MCP is the wrong tool. Use the source-controlled descriptor flow and migrations instead.

## Layout and Type Conventions

Several conventions matter when MCP clients generate mutations:

* Use canonical lowercase entity property types: `string`, `int`, `long`, `decimal`, `datetime`, `boolean`, `guid`, `enum`, `date`, `time`, `file`, `image`, `money`.
* Use canonical lowercase form field types: `text`, `textarea`, `number`, `checkbox`, `date`, `datetime`, `select`, `lookup`, `guid`, `computed`, `time`, `file`, `image`, `money`.
* Form layout is flat and id-keyed under `layout.tabs[].groups[].fields[]`. Each placement carries `row`, `colSpan`, and optional `colStart`.
* Dashboard layout is flat and name-keyed under `dashboard.visualizations[]`. Each visualization carries `row`, `order`, and `width`.
* Page and page-group icons are CSS class strings, not URLs or image file paths.
* For foreign access, `foreignKey.access` values are `none`, `view`, or `edit`.

These rules matter because validation and runtime rendering assume them. For example, a form can define fields correctly but still render an empty group if no valid placements point to those field IDs.

## Designer and MCP Together

Use the Designer when you want:

* Interactive editing
* Visual context for entities, pages, forms, and permissions
* Manual review before publishing

Use MCP when you want:

* Agent-driven or scripted changes
* Repeatable runtime mutation workflows
* Structured validation before write
* Safe, incremental edits instead of raw JSON replacement

The two surfaces are complementary. A common workflow is to inspect or prototype in the Designer, automate a repeatable mutation flow through MCP, then reopen the Designer and [Health](health.md) to confirm the result.

## See Also

* [Low-Code Designer](designer.md)
* [Health](health.md)
* [Dashboards](dashboards.md)
* [Page Groups](page-groups.md)
* [Model Descriptor Files](model-json.md)
