## DocsProjects

DocsProjects Table is used to store documentation projects.

### Description

This table stores information about the documentation projects. Each record in the table represents a documentation project and allows to manage and track the documentation projects effectively. This table is important for managing and tracking the documentation projects of the application.

### Module

[`Volo.Docs`](../Docs.md)

---

## DocsDocuments

DocsDocuments Table is used to store documents.

### Description

This table stores information about the documents. Each record in the table represents a document and allows to manage and track the documents effectively. This table is important for managing and tracking the documents of the application.

### Module

[`Volo.Docs`](../Docs.md)

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [DocsDocumentContributors](#docsdocumentcontributors) | DocumentId | To match the document. |

---

## DocsDocumentContributors

DocsDocumentContributors Table is used to store document contributors.

### Description

This table stores information about the document contributors. Each record in the table represents a document contributor and allows to manage and track the document contributors effectively. This table is important for managing and tracking the document contributors of the application.

### Module

[`Volo.Docs`](../Docs.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [DocsDocuments](#docsdocuments) | Id | To match the document. |