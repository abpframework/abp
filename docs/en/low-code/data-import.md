```json
//[doc-seo]
{
    "Description": "Import Excel or CSV data into ABP Low-Code pages with guided mapping, append or merge behavior, foreign-key matching, remote files, and invalid-row downloads."
}
```

# Data Import

The React Low-Code runtime can import Excel and CSV files into a dynamic page. The import wizard previews the file in the browser, maps source columns to entity properties, reviews the operation, and sends the file plus the confirmed mapping to the backend.

Import is enabled by default for pages and can be disabled per page with `importEnabled: false`.

## Import Workflow

1. Open a dynamic data page and select **Import**.
2. Upload an Excel or CSV file, or download a sample file for the page.
3. Review the detected columns and map each source column to one target property.
4. Choose **Append** or **Merge**.
5. Review required fields, conversions, relation matching, and file/image sources.
6. Run the import and download the invalid-row file if any rows fail.

Column names are matched automatically when a source header equals a property name or display label after normalizing spaces, underscores, hyphens, dots, and letter casing. Review every automatic mapping before import.

The wizard reports mappings as compatible, convertible, warning, or incompatible. The backend remains authoritative and validates each converted value, entity rule, and mapped property.

## Append and Merge

**Append** creates a new record for every valid row. The `Id` column is not accepted in append mode.

**Merge** uses one mapped property as the match key:

* A matching record is updated.
* A non-matching row creates a record.
* The match property must be included in the column mapping.
* When the selected property can match multiple records, choose either **Error** or **Use first**. **Use first** also requires a deterministic sort property and direction.

Use an ID or unique business key whenever possible. A non-unique merge key makes the result depend on the selected multiple-match rule.

## Foreign-Key Mapping

A foreign-key column can contain the related record ID or another allowed match property such as a unique username or code. The wizard exposes the supported related-entity match fields for that property.

If a foreign-key lookup can return multiple records, the same rules apply:

* **Error** rejects the row.
* **Use first** requires an explicit sort property and direction.

This decision is per mapped foreign-key column, independent of the main append or merge mode.

## Value Extraction

Use a source-value regular expression when a cell contains extra text around the value that should be imported. The expression must contain a named `value` capture group:

```text
Order: (?<value>[A-Z]+-\d+)
```

The server compiles and executes the expression with bounded input, evaluation count, and elapsed-time budgets. A structurally valid client preview does not replace server validation.

## File and Image URLs

File and image properties can import remote files referenced by spreadsheet text. Source modes are:

| Mode | Use it when |
|------|-------------|
| `auto` | The runtime should detect a supported URL shape |
| `fullUrl` | The whole cell is the URL |
| `fileNameAndUrl` | The cell contains a file name and URL |
| `extractUrlFromText` | The URL is embedded in surrounding text |
| `customRegex` | A custom expression extracts a named `url` group |

Remote downloads are performed by the backend, not by the browser. The verified defaults require HTTPS on port `443`, allow only public network destinations, follow at most three redirects, and reject private, link-local, multicast, and loopback destinations. Development loopback HTTP is available only when both the environment is Development and `AllowLoopbackHttpInDevelopment` is enabled.

Restrict production destinations with `LowCode:Import:RemoteFiles:AllowedHosts` when imports should fetch only from known hosts. Download count, per-file bytes, aggregate bytes, concurrency, redirects, and timeout are all bounded by `LowCode:Import:RemoteFiles` options.

Remote files are staged before row persistence. A failed row or failed merge does not replace an existing stored file with an incomplete download.

## Partial Failures

Import continues after row-scoped validation or conversion failures. The result reports:

* Total rows
* Succeeded and failed rows
* Created and updated records
* A short-lived invalid-row download token when failures exist

The invalid-row file contains the original row values plus failure details so the rows can be corrected and imported again. The verified default token lifetime is 600 seconds.

Failures that make the whole request unsafe, such as an invalid archive, exceeded global file budget, or blocked remote destination, stop the import before normal row processing.

## Limits and Configuration

Important verified defaults are:

| Setting | Default |
|---------|---------|
| `LowCode:Import:MaxRows` | `10000` |
| `LowCode:Import:MaxColumns` | `256` |
| `LowCode:Import:InvalidRowsTokenLifetimeSeconds` | `600` |
| `LowCode:Import:RemoteFiles:Enabled` | `true` |
| `LowCode:Import:RemoteFiles:RequestTimeoutSeconds` | `15` |
| `LowCode:Import:RemoteFiles:MaxConcurrentDownloads` | `4` |
| `LowCode:Import:RemoteFiles:MaxFilesPerImport` | `500` |
| `LowCode:Import:RemoteFiles:MaxFileBytes` | `10485760` |
| `LowCode:Import:RemoteFiles:MaxTotalBytesPerImport` | `104857600` |
| `LowCode:Import:RemoteFiles:AllowedPorts` | `[443]` |

The options validators reject non-positive, inconsistent, or above-ceiling values. Keep imports page-scoped and use the existing limits instead of accepting arbitrary workbook sizes.

## See Also

* [React Runtime](react-runtime.md)
* [Data Modeling and Page Behavior](data-modeling.md)
* [Model Descriptor Files](model-json.md)
* [Foreign Access](foreign-access.md)
