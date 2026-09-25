```json
//[doc-seo]
{
    "Description": "Use ABP Low-Code runtime model history, undo and redo, save points, comparison, change summaries, history extension points, and safe entity deletion."
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

The history list is paged independently from the save-point list. The history panel shows a restore separately from an add. The verified defaults keep up to 200 history batches and 100 save points per runtime layer. Retention can be changed through `LowCodeRuntimeHistoryOptions`.

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

When an action brings back an entity, a property, or a collection that was deleted, it restores the object from its kept data instead of creating an empty table or column. A save point restore reads each object's data from where it is kept now. See [Deleted Data and History](deleted-objects.md#deleted-data-and-history).

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

## Change Summary

`POST /api/lowcode/model/history/change-summary` lists what changed between two history points, one row per changed object: an entity, a property, a page, a form, an API resource, a script, an enum, a permission, and so on. It takes the same `from` and `to` points as history comparison (`Current`, `SavePoint`, `SaveBefore`, `SaveAfter`, or `Checkpoint`).

Each object row reports one of these change kinds:

| Change kind | Meaning |
|-------------|---------|
| `added` | The object did not exist at the first point |
| `changed` | The object exists at both points and some fields differ |
| `removed` | The object no longer exists at the second point |
| `restored` | A deleted object came back with its kept data |
| `replaced` | The name now belongs to a different object, for example one created again after the original was deleted |

A changed object lists each changed field with its value before and after. Set `includeObjectDetails` to also receive each object's JSON before and after and the full text of its changed scripts and expressions, and set `objectPath` (for example `["entities", "Acme.Books.Book", "properties", "Title"]`) to return only one object. When a point has no stored model snapshot, the model is rebuilt from the other point or from the current model.

`restored` and `replaced` are reported when the history between the two points is available. A comparison of two stored snapshots without that history reports such an object as added or changed.

## Extending History

A host application can add its own information to history and keep the save points it depends on.

### History Contributors

Implement `ILowCodeModelHistoryContributor` and register it in DI to set extra properties on each new history batch and save point before it is saved, in the same unit of work. The module stores these properties as they are and returns them in the batch and save point DTOs. Save point creation also accepts `extraProperties` from the caller.

```csharp
public class MyHistoryContributor : ILowCodeModelHistoryContributor, ITransientDependency
{
    public Task ContributeToHistoryBatchAsync(LowCodeModelHistoryBatchContributionContext context)
    {
        context.Batch.SetProperty(LowCodeModelHistoryExtraPropertyNames.SourceLabel, "Import");
        return Task.CompletedTask;
    }
}
```

### Source Labels

The `SourceLabel` extra property (`LowCodeModelHistoryExtraPropertyNames.SourceLabel`) names where a batch or save point came from. The Designer shows it as a badge in **History**. Set it to a short text that is already localized. Batches created by restoring a save point get a **Restore** badge.

### Protected Save Points

The per-layer save point limit (`LowCodeRuntimeHistoryOptions.MaxSavePointsPerLayer`) prunes the oldest save points of an app. Implement `ILowCodeModelSavePointRetentionGuard` to protect save points that your application still needs:

```csharp
public class MySavePointRetentionGuard : ILowCodeModelSavePointRetentionGuard, ITransientDependency
{
    public Task<IReadOnlyCollection<Guid>> GetProtectedSavePointIdsAsync(string layer, string? app)
    {
        // Return the IDs of the save points that must be kept.
        IReadOnlyCollection<Guid> ids = Array.Empty<Guid>();
        return Task.FromResult(ids);
    }
}
```

A protected save point is never pruned and does not count toward the limit. The Designer lists it as protected and refuses to delete it, so the application that protects it is responsible for deleting it when it no longer needs it.

Deleting a save point, or deleting an app's history, removes the rows from the database.

## Deleting Entities and Properties

Entity deletion is a planned operation. Before the Designer deletes an entity, it builds a plan containing:

* Descriptors that will be removed with it
* Relationships that need resolution
* Blocking references
* Affected physical tables, including the collection tables kept with the entity
* Whether each table is Designer-managed or migration-managed
* Current concurrency stamp and a plan fingerprint

For each resolvable relationship, choose one of the actions offered by the plan, such as removing the source property or converting it to a scalar. The apply step rejects stale plans and unreviewed relationship decisions. Migration-managed entities cannot be physically changed by the runtime Designer; their plan requires the migration path to be acknowledged instead.

Deleting an entity, a property, or a primitive collection keeps its data under a tombstone name. The **Deleted objects** page lists the kept data and can restore or purge it, and history actions bring the data back with the object. See [Deleted Objects](deleted-objects.md).

## MCP Automation

The runtime-only [MCP Integration](mcp.md) exposes the same history, comparison, formula/rollup, safe-deletion, and deleted-object restore workflows. Purging deleted data is not available through MCP. MCP clients should follow the feature semantics on this page, preview destructive operations, preserve concurrency stamps, and re-read [Health](health.md) after apply.

## See Also

* [Deleted Objects](deleted-objects.md)
* [Low-Code Designer](designer.md)
* [Health](health.md)
* [MCP Integration](mcp.md)
* [Model Descriptor Files](model-json.md)
* [Calculated and Rollup Properties](formula-properties.md)
