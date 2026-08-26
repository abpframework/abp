```json
//[doc-seo]
{
    "Description": "Use ABP Low-Code runtime model history, undo and redo, save points, comparison, safe entity deletion, and retained-data restoration."
}
```

# Model History and Recovery

The Low-Code Designer records applied mutation batches for history-enabled writable layers. In the standard setup, the history controls operate on the **Runtime JSON** layer. Source-controlled descriptor changes continue to use Git and the normal migration workflow.

The Designer toolbar exposes **Undo history**, **Redo history**, and **History** when the selected layer supports these operations.

## History Batches

Each successful model write is recorded as a batch with:

* Sequence and creation information
* Forward and inverse operations
* Before and after concurrency stamps
* Checkpoints within the operation list
* Status, source kind, and schema-impact metadata

History is model history, not record audit history. It tracks changes to entities, pages, forms, permissions, scripts, and other descriptors; it does not list CRUD changes made to business records.

The history list is paged independently from the save-point list. The verified defaults keep up to 200 history batches and 100 save points per runtime layer. Retention can be changed through `LowCodeRuntimeHistoryOptions`.

## Undo, Redo, and Targeted Actions

Available history actions are:

| Action | Purpose |
|--------|---------|
| Undo | Apply the inverse of the current applied batch |
| Redo | Reapply the next undone batch |
| Go to checkpoint | Move the history cursor to a before/after checkpoint |
| Revert range | Revert a selected range of batches |
| Go to save point | Move through retained history to the save point cursor |
| Restore save point | Restore the full model snapshot saved at that point |
| Apply operation | Apply one selected operation from a history batch |

Use **Go to save point** when its history cursor is still available. Use **Restore save point** when the exact cursor is no longer retained or when the saved snapshot is the intended source of truth.

## Preview Before Apply

Every history action can be previewed before it writes. The preview reports:

* Whether the action is valid
* Current concurrency stamp and conflicts
* Ordered operations
* Schema impact
* Whether destructive schema confirmation is required
* Warnings and structured conflicts

Apply the action with the same current concurrency stamp used by the reviewed preview. If the model changes between preview and apply, refresh history and preview again.

History actions do not drop physical tables or columns by default. When a preview reports destructive schema impact, the apply request requires explicit destructive confirmation. Review the affected operations and data-loss impact before enabling it.

## Save Points and Compare

A save point stores a named model snapshot plus its history cursor. Create one before a coordinated set of runtime changes or before a risky schema edit.

History comparison can compare current, save-point, before-save, after-save, and operation-checkpoint states. The result contains forward and inverse operations and reports both general and destructive schema impact without changing the model.

Comparison and history actions are protected by entry, operation, and serialized-payload budgets. Old history can be pruned while a save point retains its snapshot for later comparison or restore.

## Safe Entity Deletion

Entity deletion is a planned operation. Before the Designer deletes an entity, it builds a plan containing:

* Descriptors that will be removed with it
* Relationships that need resolution
* Blocking references
* Affected physical tables
* Whether each table is Designer-managed or migration-managed
* Current concurrency stamp and a plan fingerprint

For each resolvable relationship, choose one of the actions offered by the plan, such as removing the source property or converting it to a scalar. The apply step rejects stale plans and unreviewed relationship decisions.

Designer-managed tables require an explicit data choice:

* **Keep physical data** removes the model descriptors while retaining the physical table and its file, image, attachment, and collection data.
* **Delete physical data** drops eligible physical tables and deletes their associated stored data.

Migration-managed entities cannot be physically dropped by the runtime Designer. Their plan requires the migration path to be acknowledged instead.

## Restore Retained Entity Data

When an entity was deleted with **Keep physical data**, the Designer can preview and restore its retained table. The preview reports the table, row count, property shape, retained physical object ID, schema fingerprint, and current concurrency stamp.

Restore only from a fresh preview. The apply request must repeat its exact:

* `retainedPhysicalObjectId`
* `schemaFingerprint`
* `concurrencyStamp`

The server rejects a stale concurrency stamp, changed schema, mismatched retained object, or already-restored table. A successful restore recreates the entity descriptor against the retained physical data instead of copying the rows into a new table.

Keeping physical data is not a backup strategy. Retention metadata remains part of the same application database and should be covered by the application's normal backup and recovery process.

## MCP Automation

The runtime-only [MCP Integration](mcp.md) exposes the same history, comparison, formula/rollup, safe-deletion, and retained-restore service workflows. MCP clients should follow the feature semantics on this page, preview destructive operations, preserve concurrency stamps, and re-read [Health](health.md) after apply.

## See Also

* [Low-Code Designer](designer.md)
* [Health](health.md)
* [MCP Integration](mcp.md)
* [Model Descriptor Files](model-json.md)
* [Calculated and Rollup Properties](formula-properties.md)
