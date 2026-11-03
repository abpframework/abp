```json
//[doc-seo]
{
    "Description": "Review the ABP Low-Code model health snapshot to inspect entities, pages, forms, page groups, permissions, and script assets before publishing runtime changes."
}
```

# Health

The **Health** section in the Low-Code Designer is a selected-layer readiness view. It helps you review the resolved low-code model for the currently selected layer before publishing changes or relying on the generated runtime pages.

Health works on the **resolved view of the selected layer**, not on raw JSON files. In practice, this means it shows the selected layer after lower-layer inheritance and descriptor materialization have been applied. It does not silently merge unrelated edits from another layer. To review runtime-only overrides, switch the Designer to **Runtime JSON** or use the runtime-focused [MCP Integration](mcp.md) workflow.

## What Health Reads

The health snapshot is a combined view of the current low-code model in the selected layer. It includes:

* Entities
* Enums
* Page groups
* Pages
* Forms
* Custom permissions
* Effective page permission configuration
* Custom endpoints
* Script event handlers
* Script background jobs
* Script background workers

This makes Health the best place to review cross-descriptor relationships instead of checking each designer tab in isolation.

## What It Helps You Catch

Health is most useful for selected-layer consistency problems that often appear only after several descriptors start referencing each other.

Typical problem classes include:

* Page to form mismatches, where a page references a form that does not exist or belongs to another entity
* Page-type configuration errors, such as a kanban page missing `groupByProperty`, a calendar page using invalid date or time fields, or a gallery page pointing to a non-image property
* Page-group structure problems such as missing parents, circular references, or nesting deeper than the supported depth
* Form layout issues where fields exist but valid placements do not
* Permission and page configuration mismatches that would affect runtime visibility or access
* Script assets that exist in the model but still need review in the broader context of entities, pages, and permissions
* Orphaned upper-layer descriptors whose lower-layer base was removed or disabled

Some of these issues are also enforced by runtime validation and mutation rules. Health gives you a selected-layer review point before users discover the problem in the runtime UI.

When orphan warnings are present, preview the matching inactive-override or disabled-override cleanup before applying it. Cleanup removes only the affected authored deltas from the selected writable layer; review the preview because enabled sibling deltas on the same descriptor are preserved.

## Use It When

Use Health in these moments:

* After changing entities, pages, forms, page groups, or permissions in the Designer
* After applying MCP-driven runtime mutations
* After undo, redo, save-point restore, retained-entity restore, or override cleanup
* Before publishing a set of runtime changes
* After copying or importing source-controlled descriptors into an application

If you automate low-code changes through [MCP Integration](mcp.md), re-read the health snapshot after apply and treat that review as part of the success criteria.

For runtime history, safe entity deletion, and retained-data restoration, see [Model History and Recovery](model-history.md).

## Health and Source-Controlled Models

Health is not a replacement for source-controlled file validation.

When you edit `_Dynamic/model/**/*.json` directly:

1. Run the generated model file checker:

```bash
dotnet run --project <startup-project> -- --check-lowcode-model-files
```

2. Run the required migration workflow for schema-affecting entity changes.
3. Open the Designer and review **Health** after the model has loaded successfully.

The file checker catches JSON and category-level problems. Health complements it by helping you inspect the resolved model after descriptors, references, permissions, pages, and forms are all combined.

A useful example is form layout drift: a descriptor can still pass `--check-lowcode-model-files` while a source-controlled form renders with no visible fields because its layout placements are stale. Health and runtime preview are the places to catch that class of issue.

## See Also

* [Low-Code Designer](designer.md)
* [MCP Integration](mcp.md)
* [Model History and Recovery](model-history.md)
* [Model Descriptor Files](model-json.md)
* [Dashboards](dashboards.md)
* [Page Groups](page-groups.md)
