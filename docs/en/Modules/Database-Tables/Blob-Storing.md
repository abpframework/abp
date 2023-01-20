## AbpBlobContainers

AbpBlobContainers is a table that stores the information of the blob container.

### Description

This table stores information about the BLOB (binary large object) containers in the application. Each record in the table represents a blob container and allows to manage and organize the blobs effectively. For example, you can use the `ContainerId` column to link each blob with its corresponding container in the [`AbpBlobs`](#abpblobs) table, so that you can easily manage and organize the blobs.

## Module

[`Volo.Abp.BlobStoring`](../../Blob-Storing.md)

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [AbpBlobs](#abpblobs) | ContainerId | To match the blob. |

---

## AbpBlobs

AbpBlobs is a table to store blobs.

### Description

This table stores the binary data of BLOBs (binary large objects) in the application. Each record in the table represents a BLOB and allows to manage and track the BLOBs effectively. Each BLOB is related to a container in the "AbpBlobContainers" table, where the container name, tenant id and other properties of the container can be found.

### Module

[`Volo.Abp.BlobStoring`](../../Blob-Storing.md)

### Uses 

| Table | Column | Description |
| --- | --- | --- |
| [AbpBlobContainers](#abpblobcontainers) | Id | To match the blob container. |

