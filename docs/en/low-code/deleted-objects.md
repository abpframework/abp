```json
//[doc-seo]
{
    "Description": "Keep, restore, and purge the data of deleted ABP Low-Code entities, properties, and collections, and track the deletion of an app."
}
```

# Deleted Objects

Deleting an entity, a property, or a primitive collection in the Low-Code Designer removes it from the model but keeps its data. The data stays in the database under a tombstone name until you restore it or purge it. This page describes how kept data is listed, restored, and purged, and how deleting a whole app works.

Deleted data is kept for the runtime model (the **Runtime JSON** layer), whose tables the Designer manages. Tables of entities defined in model files or in code belong to your EF Core migrations, so a delete there does not keep or drop anything by itself.

## What a Delete Keeps

A delete never drops data by itself. Depending on what was deleted, the data is kept as follows:

| Deleted object | What is kept |
|----------------|--------------|
| Entity | The entity table, renamed to a tombstone name. The tables of its primitive collections are kept with it. |
| Property mapped to its own column | The column, renamed to a tombstone name. |
| Property stored in the `Data` JSON column | The values, moved into a new column named after the deleted object. See [Known Limitation: Long Strings on SQL Server](#known-limitation-long-strings-on-sql-server). |
| Primitive collection | The collection table, renamed to a tombstone name. |

Tombstone names are unique for each delete, so deleting a name, adding it again, and deleting it again does not collide with the data kept the first time. Generated names stay within the database's identifier limit. Each delete also records the app, the original names, the row count, and who deleted the object and when.

Numbers and booleans moved out of the `Data` column keep their type in the new column; other values are kept as the text the JSON document held. The old key stays in each record with a `null` value, which every reader treats as missing, and disappears the next time the record is saved.

Entity and property delete dialogs no longer ask whether the physical data should be dropped, and a model change that asks for it with the `dropPhysicalObject` option is refused. The entity delete plan still lists the descriptors removed with the entity, the relationships that need a decision, and the collection tables that are kept with it. A migration-managed entity still requires the migration path to be acknowledged.

Keeping deleted data is not a backup strategy. Kept tables and columns live in the same application database and should be covered by its normal backup and recovery process.

### Where a Deleted Property Was Used

Deleting a property also takes it out of the places that show it, so the model stays valid:

* The page columns and page filters that show the property
* The form fields bound to it, together with their layout placements
* The API resource field lists that name it (response, writable, import, export, file, and related-name lists)

These removals are part of the same change: the deleted-object record lists them, and undo puts them back. A form field that the same change binds to another property is kept, for example when a text property is replaced by a lookup and its form field is re-bound to the new key.

Some references still block the delete, and the error names the page or form to change first: a page column or filter whose path goes through the property to a related entity, a page role such as a sort, grouping, calendar, or chart property, and a form rule.

## The Deleted Objects Page

The Designer's **Deleted objects** page lists every entity, property, and collection whose data is still kept for the selected layer and app. For each object it shows the original name, the kind, the row count, when it was deleted, and whether it can be restored. When an object cannot be restored, the page says why, for example:

* The table changed after deletion, so it cannot be restored safely.
* The data is no longer in the database.
* A property was kept with its entity, which is deleted too. Restore the entity first; the property is purged together with it.
* The entity a property belonged to no longer exists. Add or restore the entity first.
* An enum property needs an enum that the model no longer has. Add the enum back, for example by restoring a save point or undoing the change that removed it, then restore the property.
* A purge of the object is running.

When the original name is used by another entity or property, the page says so and offers to restore under a new name.

### Restore

**Restore** brings the object back with its data. An entity comes back on its kept table instead of a new, empty one; the rows are not copied. A property comes back on its entity with the values it had when it was deleted.

If another entity or property already uses the original name, restore the deleted one under a new name. Its data moves with it.

Restore always runs against a fresh read of the object. The request repeats the model concurrency stamp and the object's schema fingerprint, and the server refuses a stale stamp, a changed schema, or an object that is already restored or being purged.

Restoring from **Deleted objects** brings back the object and its data only. It does not put the object back into the places it was used:

* A restored property is not added back to the API resource field lists, page columns, page filters, or form fields it was removed from when it was deleted. The restore result lists the places that still exist, and the page shows them, so you can add the property back where it is still wanted.
* A restored entity does not bring back the pages, forms, and API resources that used it.

To undo a delete together with everything the delete changed, use history instead. See [Deleted Data and History](#deleted-data-and-history).

### Adding a Deleted Name Again

When you add an entity or a property in the Designer under a name whose data is still kept, the Designer offers to restore the deleted object instead. **Restore** brings it back as it was. For a property, the dialog then lists the places it was used before, as the **Deleted objects** page does. Creating a new one leaves the deleted data on the **Deleted objects** page, and the new object starts without that data.

### Purge

**Purge for good** drops the kept data permanently. It requires the **Purge Deleted Objects** permission (`AbpLowCodeDesigner.PurgeDeleted`), which is separate from the Designer edit permission that deletes and restores. Purge is not available through [MCP](mcp.md).

Before a purge, the Designer shows what it will remove, counted at that moment: rows, attachments uploaded to the entity's records, and stored file or image values. To confirm, type the name of the object.

The purge runs as a background job (`LowCode.RetainedPhysicalObjectPurge`), so the application must run ABP background jobs. The object stays on the page as purging until the job finishes, and it cannot be restored meanwhile. The job drops the data together with its files:

* **Entity**: its table and the collection tables kept with it, the properties and collections deleted earlier and kept with the entity, the files its rows point to, and the attachments of its rows
* **File or image property**: the kept column and the files its values point to; the entity and its other data stay
* **Collection of files or images**: its table and the files its values point to

Files are deleted after the table or column is gone, by a cleanup that is picked up again if it cannot finish. If the purge fails, the page shows the reason and offers to purge again.

## Deleted Data and History

[Model history](model-history.md) knows about kept data. **Undo**, **Redo**, **Go to checkpoint**, **Go to save point**, and **Restore save point** bring a deleted entity, property, or collection back from its kept data instead of creating an empty table or column. This also works when:

* An entity was deleted and restored in between.
* An object was restored under a new name.
* A JSON-stored property was renamed. Its values move with the rename and move back on undo.

Undoing a property delete also puts the property back into the API resource field lists, page columns, page filters, and form fields that the delete removed it from, because those changes are part of the same history batch.

Once an object is purged, history can no longer bring its data back.

## Deleting an App

Deleting an [app](model-json.md#apps) removes the app with all of its data: its descriptors, tables and records, files, model history, save points, and access grants, including the data kept from earlier entity, property, and collection deletes. Unlike those deletes, an app delete cannot be restored. To take an app out of use without losing anything, archive it instead (**Archive app** on the app's **App settings** page, or `isArchived: true`).

Deleting an app requires the **Delete Apps** permission (`AbpLowCodeDesigner.DeleteApp`). The Designer edit permission can archive an app but not delete it, and apps cannot be deleted through [MCP](mcp.md). The default app cannot be deleted or archived.

In the Designer, use **Delete app** on the app's **App settings** page. The dialog shows the deletion plan first: the descriptors, the physical tables (including kept tables of earlier deletes), and the permission grants that go with the app, and anything that blocks the deletion. When the app has migration-managed tables, you must acknowledge that Low-Code does not drop them.

An app of the runtime model is deleted in the background, and the deletion is tracked until it completes:

1. The app's descriptors are removed and the app is marked as deleting. From that moment the app is no longer listed, served, or editable.
2. A background job (`LowCode.AppDeletion`) drops the app's tables, starting with the kept tables of earlier deletes, and deletes the files stored with their rows. Each object is handled in its own unit of work.
3. When nothing is left, the app itself is removed.

The app name stays reserved until the deletion completes, so a new app cannot take it while data of the old one may still exist.

A deletion gets three runs; the background job retries it by itself in between. After the third failed run, the deletion is marked as failed and waits until someone retries it. A failed deletion records the step (`data`, `files`, or `app`), the object it stopped at, and a reason code (`busy`, `in-use`, `file-storage`, or `unexpected`), never the exception text. List deletions and retry a failed one through the Designer API (see [Designer API](#designer-api) for the request headers):

| Method | Route | Purpose |
|--------|-------|---------|
| `GET` | `/api/lowcode/model/apps/{app}/delete-impact` | Build the deletion plan, with its `concurrencyStamp` and `planFingerprint` |
| `POST` | `/api/lowcode/model/apps/{app}/delete` | Delete the app, with the plan's `concurrencyStamp` and `planFingerprint`, and `migrationAcknowledged` when the plan requires it |
| `GET` | `/api/lowcode/model/app-deletions` | List app deletions that are still running or failed |
| `POST` | `/api/lowcode/model/apps/{app}/deletion/retry` | Run a deletion again with a fresh set of attempts; this also re-queues a deletion whose job was lost |

Deleting and retrying require the **Delete Apps** permission.

An app kept in model files that the Designer edits during development is deleted at once instead: its descriptors and the app are removed from the files in one write, whatever an earlier delete of the app kept is dropped, and no deletion record or background job is used. Its tables belong to your EF Core migrations. `isDeleting` belongs to the runtime model only: a model file that sets it on an app fails to load with `LowCode:ModelFileRuntimeOnlyProperty`.

A host application that keeps its own data about an app can remove it by handling `LowCodeAppDeletedEventData` on the local event bus. The event is published when the app's data is gone, inside the unit of work that removes the app. A handler that throws keeps the deletion open: it is recorded as failed and the handler runs again on retry, so write handlers that are safe to run more than once. For an app kept in model files, the event is published in the request that deletes the app, and a handler that throws fails that request and keeps the app.

```csharp
public class MyAppDeletedHandler :
    ILocalEventHandler<LowCodeAppDeletedEventData>,
    ITransientDependency
{
    public async Task HandleEventAsync(LowCodeAppDeletedEventData eventData)
    {
        // eventData.Layer and eventData.App identify the deleted app.
        await RemoveMyAppSettingsAsync(eventData.App);
    }
}
```

## Designer API

The **Deleted objects** page uses these Designer endpoints under `/api/lowcode/model`:

| Method | Route | Purpose |
|--------|-------|---------|
| `GET` | `deleted-objects` | List kept objects |
| `GET` | `deleted-objects/lookup?entityName=...&propertyName=...` | Find the kept object for a name |
| `GET` | `deleted-objects/{id}` | Get one kept object |
| `POST` | `deleted-objects/{id}/restore` | Restore, with `concurrencyStamp`, `schemaFingerprint`, and an optional `newName` |
| `GET` | `deleted-objects/{id}/purge-impact` | Count what a purge would remove |
| `POST` | `deleted-objects/{id}/purge` | Start a purge, with `confirmName` |

Like other Designer model endpoints, these requests name the model layer in the `X-LowCode-Layer` header and a non-default app in the `X-LowCode-App` header. The purge endpoints require the **Purge Deleted Objects** permission. Restore requires the Designer edit permission, and the read endpoints require the Designer view permission.

A successful property restore returns `formerUsages`: the API resource, page, and form lists the property was taken out of when it was deleted and that still exist, each with its `kind` (`apiResource`, `page`, or `form`), `name`, and `list`.

[MCP](mcp.md) clients can list, find, and restore deleted objects with the `lowcode_designer_get_deleted_objects`, `lowcode_designer_find_deleted_object`, and `lowcode_designer_restore_deleted_object` tools. There are no MCP tools for purging data or deleting apps.

## Known Limitation: Long Strings on SQL Server

On SQL Server, a string stored in the `Data` JSON column is read through `JSON_VALUE`, which returns no value for strings longer than 4,000 characters. Low-Code cannot move such a value in SQL, so:

* When the property is deleted, the value is not moved to the kept column. It stays in the record under the property's name.
* When the property is restored, there is no kept value to bring back. The old value is only there because it never left the record.
* When the property is renamed, the value is not carried to the new name. It stays under the old one.

As a result, a property added again under the same name after a delete can show such an old value, and after a rename the long value is not shown under the new name.

PostgreSQL and SQLite read strings of any length and are not affected. Properties mapped to their own column (`isMappedToDbField: true`) are not affected either. Map a property to its own column when it can hold text longer than 4,000 characters.

The same limit applies on SQL Server when an existing-data formula fills a property from such a JSON-stored value.

## See Also

* [Model History and Recovery](model-history.md)
* [Data Modeling and Page Behavior](data-modeling.md)
* [Low-Code Designer](designer.md)
* [Model Descriptor Files](model-json.md)
