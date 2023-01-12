## AbpBlobContainers

AbpBlobContainers is a table that stores the information of the blob container.

### Description

This table stores information about the BLOB (binary large object) containers in the application. Each record in the table represents a blob container and allows to manage and organize the blobs effectively. For example, you can use the `ContainerId` column to link each blob with its corresponding container in the [`AbpBlobs`](AbpBlobs.md) table, so that you can easily manage and organize the blobs.

## Module

[`Volo.Abp.BlobStoring`](../../../Blob-Storing.md)

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [AbpBlobs](AbpBlobs.md) | ContainerId | To match the blob. |