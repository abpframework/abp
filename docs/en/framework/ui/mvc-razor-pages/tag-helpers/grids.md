# Grids

## Introduction

Abp tag helpers for bootstrap based grid system.



## Demo

See the [grids demo page](https://bootstrap-taghelpers.abp.io/Components/Grids) to see it in action.



### Sizing

**Equal Width:** Creates columns with equal width.

Sample:

````xml
<abp-container>
    <abp-row>
        <abp-column abp-border="Info">1 of 2</abp-column>
        <abp-column abp-border="Danger">2 of 2</abp-column>
    </abp-row>
    <abp-row>
        <abp-column abp-border="Primary">1 of 3</abp-column>
        <abp-column abp-border="Secondary">2 of 3</abp-column>
        <abp-column abp-border="Dark">3 of 3</abp-column>
    </abp-row>
</abp-container>
````

**Column Breaker:** `abp-column-breaker` is used for breaking the automatic width of placement of the current row and starting in a new row afterwards.

Sample:

````xml
<abp-container>
    <abp-row>
        <abp-column>column</abp-column>
        <abp-column>column</abp-column>
        <abp-column-breaker/>
        <abp-column>column</abp-column>
        <abp-column>column</abp-column>
    </abp-row>
</abp-container>
````

**Setting one column width:** size attribute is used for setting the width for a specific column. 

Sample:

```xml
<abp-container>
    <abp-row>
        <abp-column>1 of 3</abp-column>
        <abp-column size="_6">2 of 3 (wider)</abp-column>
        <abp-column>3 of 3</abp-column>
    </abp-row>
    <abp-row>
        <abp-column>1 of 3</abp-column>
        <abp-column size="_5">2 of 3 (wider)</abp-column>
        <abp-column>3 of 3</abp-column>
    </abp-row>
</abp-container>
```

**Variable width content:** Auto resizing column based on content.

```xml
<abp-container>
    <abp-row h-align="Center">
        <abp-column size-lg="_2" abp-border="Info">1 of 3</abp-column>
        <abp-column size-md="Auto" abp-border="Danger">Contrary to popular belief, Lorem Ipsum is not simply random text.</abp-column>
        <abp-column size-lg="_2" abp-border="Warning">3 of 3</abp-column>
    </abp-row>
    <abp-row>
        <abp-column>1 of 3</abp-column>
        <abp-column size-md="Auto">Variable width content</abp-column>
        <abp-column size-lg="_2">3 of 3</abp-column>
    </abp-row>
</abp-container>
```



### Responsive Classes

Responsive classes can be used strongly typed within abp tags. 

```xml
<abp-row>
    <abp-column size-sm="_8">col-sm-8</abp-column>
    <abp-column size-sm="_4">col-sm-4</abp-column>
</abp-row>
<abp-row>
    <abp-column size-sm="_">col-sm</abp-column>
    <abp-column size-sm="_">col-sm</abp-column>
    <abp-column size-sm="_">col-sm</abp-column>
    <abp-column size-sm="_">col-sm</abp-column>
</abp-row>
<!-- Stack the columns on mobile by making one full-width and the other half-width -->
<abp-row>
    <abp-column size="_12" size-md="_8">.col-12 .col-md-8</abp-column>
    <abp-column size="_6" size-md="_4">.col-6 .col-md-4</abp-column>
</abp-row>

<!-- Columns start at 50% wide on mobile and bump up to 33.3% wide on desktop -->
<abp-row>
    <abp-column size="_6" size-md="_4">.col-6 .col-md-4</abp-column>
    <abp-column size="_6" size-md="_4">.col-6 .col-md-4</abp-column>
    <abp-column size="_6" size-md="_4">.col-6 .col-md-4</abp-column>
</abp-row>

<!-- Columns are always 50% wide, on mobile and desktop -->
<abp-row>
    <abp-column size="_6">.col-6</abp-column>
    <abp-column size="_6">.col-6</abp-column>
</abp-row>
```



### Alignment

Column alignments can be done strongly typed in abp tags with both vertically and horizontally.

**Vertical-alignment**: `v-align` attribute value is used to align the columns vertically. 

Sample:

```xml
<abp-container>
    <abp-row v-align="Start">
        <abp-column>column</abp-column>
        <abp-column>column</abp-column>
        <abp-column>column</abp-column>
    </abp-row>
    <abp-row v-align="Center">
        <abp-column>column</abp-column>
        <abp-column>column</abp-column>
        <abp-column>column</abp-column>
    </abp-row>
    <abp-row v-align="End">
        <abp-column>column</abp-column>
        <abp-column>column</abp-column>
        <abp-column>column</abp-column>
    </abp-row>
</abp-container>
```

**Horizontal-alignment**: `h-align` attribute value is used to align the columns horizontally. 

Sample:

```xml
<abp-container>
    <abp-row h-align="Start">
        <abp-column size="_4">One of two columns</abp-column>
        <abp-column size="_4">One of two columns</abp-column>
    </abp-row>
    <abp-row h-align="Center">
        <abp-column size="_4">One of two columns</abp-column>
        <abp-column size="_4">One of two columns</abp-column>
    </abp-row>
    <abp-row h-align="End">
        <abp-column size="_4">One of two columns</abp-column>
        <abp-column size="_4">One of two columns</abp-column>
    </abp-row>
    <abp-row h-align="Around">
        <abp-column size="_4">One of two columns</abp-column>
        <abp-column size="_4">One of two columns</abp-column>
    </abp-row>
    <abp-row h-align="Between">
        <abp-column size="_4">One of two columns</abp-column>
        <abp-column size="_4">One of two columns</abp-column>
    </abp-row>
</abp-container>
```

**No gutters**: The gutters between columns in predefined grid classes can be removed with `gutters="false"`. This removes the negative `margin`s from `abp-row` and the horizontal `padding` from all immediate children columns. 

Sample:

```xml
<abp-row gutters="false">
    <abp-column size="_8">One of two columns</abp-column>
    <abp-column size="_4">One of two columns</abp-column>
</abp-row>
```

**Column wrapping**: If more than 12 columns are placed within a single row, each group of extra columns will, as one unit, wrap onto a new line.

Sample:

```xml
<abp-row>
    <abp-column size="_9">.col-9</abp-column>
    <abp-column size="_4">.col-4<br>Since 9 + 4 = 13 &gt; 12, this 4-column-wide div gets wrapped onto a new line as one contiguous unit.</abp-column>
    <abp-column size="_6">.col-6<br>Subsequent columns continue along the new line.s</abp-column>
</abp-row>
```



### Reordering

**Order Classes**:  `order` attribute is used for controlling the visual order of the content. 

Sample:

```xml
<abp-container>
    <abp-row>
        <abp-column order="_12">First, but Last</abp-column>
        <abp-column>Second, but unordered</abp-column>
        <abp-column order="_6">Third, but Second</abp-column>
    </abp-row>
</abp-container>
```

**Offsetting columns**:  `offset` attribute is used for setting the offset of the grid columns. 

Sample:

```xml
<abp-container>
    <abp-row>
        <abp-column size-md="_4">.col-md-4</abp-column>
        <abp-column size-md="_4" offset-md="_4">.col-md-4 .offset-md-4</abp-column>
    </abp-row>
    <abp-row>
        <abp-column size-md="_3" offset-md="_3">.col-md-3 .offset-md-3</abp-column>
        <abp-column size-md="_3" offset-md="_3">.col-md-3 .offset-md-3</abp-column>
    </abp-row>
    <abp-row>
        <abp-column size-md="_6" offset-md="_3">.col-md-6 .offset-md-3</abp-column>
    </abp-row>
    <abp-row>
        <abp-column size-sm="_5" size-md="_6">.col-sm-5 .col-md-6</abp-column>
        <abp-column size-sm="_5" offset-sm="_2" size-md="_6" offset-md="_">.col-sm-5 .offset-sm-2 .col-md-6 .offset-md-0</abp-column>
    </abp-row>
    <abp-row>
        <abp-column size-sm="_6" size-md="_5" size-lg="_6">col-sm-6 .col-md-5 .col-lg-6</abp-column>
        <abp-column size-sm="_6" size-md="_5" offset-md="_2" size-lg="_6" offset-lg="_">.col-sm-6 .col-md-5 .offset-md-2 .col-lg-6 .offset-lg-0</abp-column>
    </abp-row>
</abp-container>
```



## abp-row Attributes

- **v-align:** A value indicates the vertical positioning of the containing columns. Should be one of the following values:
  * `Default` (default value)
  * `Start`
  * `Center`
  * `End`

- **h-align**: A value indicates the horizontal positioning of the containing columns. Should be one of the following values: 
  * `Default` (default value)
  * `Start`
  * `Center`
  * `Around`
  * `Between`
  * `End`
- **gutter**: A value indicates if the negative `margin` and horizontal `padding` will be removed from all children columns. Will act as `true` value if this attribute is not set. Should be one of the following values: 
  * `true`
  * `false`

## abp-column Attributes

- **size:** A value indicates the width of the column from `_`, `Undefined`, `_1`..`_12`, `Auto`. Or can be used with predefined values like:
  - `size-sm`
  - `size-md`
  - `size-lg`
  - `size-xl`
- **order**: A value indicates the order of column from `Undefined`, `_1`..`_12`, `First` and `Last`.
- **offset:** A value indicates offset of the column from `_`, `Undefined`, `_1`..`_12`, `Auto`. Or can be used with predefined values like:
  - `offset-sm`
  - `offset-md`
  - `offset-lg`
  - `offset-xl`

