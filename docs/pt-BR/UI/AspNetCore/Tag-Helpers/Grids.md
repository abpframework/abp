# Grids

## Introdução

Abp tag helpers para o sistema de grid baseado no bootstrap.

## Demonstração

Veja a página de demonstração [grids demo page](https://bootstrap-taghelpers.abp.io/Components/Grids) para ver em ação.

### Dimensionamento

**Largura igual:** Cria colunas com largura igual.

Exemplo:

````xml
<abp-container>
    <abp-row>
        <abp-column abp-border="Info">1 de 2</abp-column>
        <abp-column abp-border="Danger">2 de 2</abp-column>
    </abp-row>
    <abp-row>
        <abp-column abp-border="Primary">1 de 3</abp-column>
        <abp-column abp-border="Secondary">2 de 3</abp-column>
        <abp-column abp-border="Dark">3 de 3</abp-column>
    </abp-row>
</abp-container>
````

**Quebra de coluna:** `abp-column-breaker` é usado para quebrar a largura automática de colocação da linha atual e iniciar em uma nova linha em seguida.

Exemplo:

````xml
<abp-container>
    <abp-row>
        <abp-column>coluna</abp-column>
        <abp-column>coluna</abp-column>
        <abp-column-breaker/>
        <abp-column>coluna</abp-column>
        <abp-column>coluna</abp-column>
    </abp-row>
</abp-container>
````

**Definindo a largura de uma coluna:** o atributo size é usado para definir a largura de uma coluna específica.

Exemplo:

```xml
<abp-container>
    <abp-row>
        <abp-column>1 de 3</abp-column>
        <abp-column size="_6">2 de 3 (mais larga)</abp-column>
        <abp-column>3 de 3</abp-column>
    </abp-row>
    <abp-row>
        <abp-column>1 de 3</abp-column>
        <abp-column size="_5">2 de 3 (mais larga)</abp-column>
        <abp-column>3 de 3</abp-column>
    </abp-row>
</abp-container>
```

**Conteúdo de largura variável:** Coluna de redimensionamento automático com base no conteúdo.

```xml
<abp-container>
    <abp-row h-align="Center">
        <abp-column size-lg="_2" abp-border="Info">1 de 3</abp-column>
        <abp-column size-md="Auto" abp-border="Danger">Contrary to popular belief, Lorem Ipsum is not simply random text.</abp-column>
        <abp-column size-lg="_2" abp-border="Warning">3 de 3</abp-column>
    </abp-row>
    <abp-row>
        <abp-column>1 de 3</abp-column>
        <abp-column size-md="Auto">Conteúdo de largura variável</abp-column>
        <abp-column size-lg="_2">3 de 3</abp-column>
    </abp-row>
</abp-container>
```

### Classes Responsivas

As classes responsivas podem ser usadas de forma fortemente tipada dentro das tags abp.

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
<!-- Empilhe as colunas em dispositivos móveis, tornando uma em largura total e a outra em meia largura -->
<abp-row>
    <abp-column size="_12" size-md="_8">.col-12 .col-md-8</abp-column>
    <abp-column size="_6" size-md="_4">.col-6 .col-md-4</abp-column>
</abp-row>

<!-- As colunas começam com 50% de largura em dispositivos móveis e aumentam para 33,3% de largura em desktop -->
<abp-row>
    <abp-column size="_6" size-md="_4">.col-6 .col-md-4</abp-column>
    <abp-column size="_6" size-md="_4">.col-6 .col-md-4</abp-column>
    <abp-column size="_6" size-md="_4">.col-6 .col-md-4</abp-column>
</abp-row>

<!-- As colunas têm sempre 50% de largura, em dispositivos móveis e desktop -->
<abp-row>
    <abp-column size="_6">.col-6</abp-column>
    <abp-column size="_6">.col-6</abp-column>
</abp-row>
```

### Alinhamento

Os alinhamentos de coluna podem ser feitos de forma fortemente tipada nas tags abp, tanto vertical quanto horizontalmente.

**Alinhamento vertical**: o valor do atributo `v-align` é usado para alinhar as colunas verticalmente.

Exemplo:

```xml
<abp-container>
    <abp-row v-align="Start">
        <abp-column>coluna</abp-column>
        <abp-column>coluna</abp-column>
        <abp-column>coluna</abp-column>
    </abp-row>
    <abp-row v-align="Center">
        <abp-column>coluna</abp-column>
        <abp-column>coluna</abp-column>
        <abp-column>coluna</abp-column>
    </abp-row>
    <abp-row v-align="End">
        <abp-column>coluna</abp-column>
        <abp-column>coluna</abp-column>
        <abp-column>coluna</abp-column>
    </abp-row>
</abp-container>
```

**Alinhamento horizontal**: o valor do atributo `h-align` é usado para alinhar as colunas horizontalmente.

Exemplo:

```xml
<abp-container>
    <abp-row h-align="Start">
        <abp-column size="_4">Uma de duas colunas</abp-column>
        <abp-column size="_4">Uma de duas colunas</abp-column>
    </abp-row>
    <abp-row h-align="Center">
        <abp-column size="_4">Uma de duas colunas</abp-column>
        <abp-column size="_4">Uma de duas colunas</abp-column>
    </abp-row>
    <abp-row h-align="End">
        <abp-column size="_4">Uma de duas colunas</abp-column>
        <abp-column size="_4">Uma de duas colunas</abp-column>
    </abp-row>
    <abp-row h-align="Around">
        <abp-column size="_4">Uma de duas colunas</abp-column>
        <abp-column size="_4">Uma de duas colunas</abp-column>
    </abp-row>
    <abp-row h-align="Between">
        <abp-column size="_4">Uma de duas colunas</abp-column>
        <abp-column size="_4">Uma de duas colunas</abp-column>
    </abp-row>
</abp-container>
```

**Sem margens**: as margens entre as colunas nas classes de grade predefinidas podem ser removidas com `gutters="false"`. Isso remove as margens negativas de `abp-row` e o preenchimento horizontal de todas as colunas filhas imediatas.

Exemplo:

```xml
<abp-row gutters="false">
    <abp-column size="_8">Uma de duas colunas</abp-column>
    <abp-column size="_4">Uma de duas colunas</abp-column>
</abp-row>
```

**Quebra de coluna**: se mais de 12 colunas forem colocadas em uma única linha, cada grupo de colunas extras será, como uma unidade, quebrado em uma nova linha.

Exemplo:

```xml
<abp-row>
    <abp-column size="_9">.col-9</abp-column>
    <abp-column size="_4">.col-4<br>Since 9 + 4 = 13 &gt; 12, this 4-column-wide div gets wrapped onto a new line as one contiguous unit.</abp-column>
    <abp-column size="_6">.col-6<br>Subsequent columns continue along the new line.s</abp-column>
</abp-row>
```

### Reordenamento

**Classes de ordem**: o atributo `order` é usado para controlar a ordem visual do conteúdo.

Exemplo:

```xml
<abp-container>
    <abp-row>
        <abp-column order="_12">Primeiro, mas último</abp-column>
        <abp-column>Segundo, mas não ordenado</abp-column>
        <abp-column order="_6">Terceiro, mas segundo</abp-column>
    </abp-row>
</abp-container>
```

**Deslocamento de colunas**: o atributo `offset` é usado para definir o deslocamento das colunas da grade.

Exemplo:

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

## Atributos de abp-row

- **v-align:** Um valor que indica o posicionamento vertical das colunas contidas. Deve ser um dos seguintes valores:
  * `Default` (valor padrão)
  * `Start`
  * `Center`
  * `End`

- **h-align**: Um valor que indica o posicionamento horizontal das colunas contidas. Deve ser um dos seguintes valores:
  * `Default` (valor padrão)
  * `Start`
  * `Center`
  * `Around`
  * `Between`
  * `End`
- **gutter**: Um valor que indica se as margens negativas e o preenchimento horizontal serão removidos de todas as colunas filhas. Atuará como valor `true` se esse atributo não for definido. Deve ser um dos seguintes valores:
  * `true`
  * `false`

## Atributos de abp-column

- **size:** Um valor que indica a largura da coluna de `_`, `Undefined`, `_1`..`_12`, `Auto`. Ou pode ser usado com valores predefinidos como:
  - `size-sm`
  - `size-md`
  - `size-lg`
  - `size-xl`
- **order**: Um valor que indica a ordem da coluna de `Undefined`, `_1`..`_12`, `First` e `Last`.
- **offset:** Um valor que indica o deslocamento da coluna de `_`, `Undefined`, `_1`..`_12`, `Auto`. Ou pode ser usado com valores predefinidos como:
  - `offset-sm`
  - `offset-md`
  - `offset-lg`
  - `offset-xl`