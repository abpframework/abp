## AbpBlobs

AbpBlobs is a table to store blobs.

### Description

This table stores the binary data of BLOBs (binary large objects) in the application. Each record in the table represents a BLOB and allows to manage and track the BLOBs effectively. Each BLOB is related to a container in the "AbpBlobContainers" table, where the container name, tenant id and other properties of the container can be found.

### Module

[`Volo.Abp.BlobStoring`](../../../Blob-Storing.md)

### Uses 

| Table | Column | Description |
| --- | --- | --- |
| [AbpBlobContainers](AbpBlobContainers.md) | Id | To match the blob container. |