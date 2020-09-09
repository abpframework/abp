# 栅格

## 介绍

ABP标签助手基于bootstrap栅格系统.

## Demo

参阅[网络demo页面](https://bootstrap-taghelpers.abp.io/Components/Grids)查看示例.

### Sizing

**等宽:** 创建等宽的列.

示例:

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

**列中断:** `abp-column-breaker` 用于中断当前行的自动放置宽度,然后在新行中开始.

示例:

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

**设置列宽度:** size属性用于设置特定列的宽度.

示例:

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

**可变宽度内容:** 根据内容自动调整列的大小.

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

### 响应类

响应式类可以在abp标签中强类型使用.

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

### 对齐

可以使用强类型的abp标签在垂直和水平方向上进行列对齐.

**垂直对齐**: `v-align` 属性值用于对垂直齐列.

示例:

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

**水平对齐**: `h-align` 属性值用于对水平齐列.

示例:

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

**无间隙**: 可以使用 `gutters="false"` 删除预定义栅格类中列之间的栅格线. 这会从 `abp-row` 中消除负边距,并从所有直接子列中消除水平边距.

示例:

```xml
<abp-row gutters="false">
    <abp-column size="_8">One of two columns</abp-column>
    <abp-column size="_4">One of two columns</abp-column>
</abp-row>
```

**列包装**: 如果在一行中放置超过12列,则将每组额外的列作为一个单元包装到新行上.

示例:

```xml
<abp-row>
    <abp-column size="_9">.col-9</abp-column>
    <abp-column size="_4">.col-4<br>Since 9 + 4 = 13 &gt; 12, this 4-column-wide div gets wrapped onto a new line as one contiguous unit.</abp-column>
    <abp-column size="_6">.col-6<br>Subsequent columns continue along the new line.s</abp-column>
</abp-row>
```

### 重新排序

**Order类**:  `order` 属性用于控制内容的视觉顺序.

示例:

```xml
<abp-container>
    <abp-row>
        <abp-column order="_12">First, but Last</abp-column>
        <abp-column>Second, but unordered</abp-column>
        <abp-column order="_6">Third, but Second</abp-column>
    </abp-row>
</abp-container>
```

**偏移列**:  `offset` 属性用于设置栅格列的偏移量.

示例:

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

- **v-align:** 指定包含列的垂直位置. 应为以下值之一:
  * `Default` (默认值)
  * `Start`
  * `Center`
  * `End`

- **h-align**: 指定包含列的水平位置. 应为以下值之一:
  * `Default` (默认值)
  * `Start`
  * `Center`
  * `Around`
  * `Between`
  * `End`
- **gutter**: 指定是否将从所有子列中删除负边距和水平填充. 如果未设置,默认为 `true`. 应为以下值之一:
  * `true`
  * `false`

## abp-column Attributes

- **size:** 指定列的宽度: `_`, `Undefined`, `_1`..`_12`, `Auto`. 或者可以与预定义值一起使用,例如:
  - `size-sm`
  - `size-md`
  - `size-lg`
  - `size-xl`
- **order**: 指定列的顺序: `Undefined`, `_1`..`_12`, `First` 和 `Last`.
- **offset:** 指定列的偏移量: `_`, `Undefined`, `_1`..`_12`, `Auto`. 或者可以与预定义值一起使用,例如:
  - `offset-sm`
  - `offset-md`
  - `offset-lg`
  - `offset-xl`