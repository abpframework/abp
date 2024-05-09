# Tabelas

## Introdução

`abp-table` é o componente básico de tag para tabelas no abp.

Uso básico:

````html
<abp-table hoverable-rows="true" responsive-sm="true">
        <thead>
            <tr>
                <th scope="Column">#</th>
                <th scope="Column">Primeiro</th>
                <th scope="Column">Último</th>
                <th scope="Column">Identificador</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <th scope="Row">1</th>
                <td>Mark</td>
                <td>Otto</td>
                <td table-style="Danger">mdo</td>
            </tr>
            <tr table-style="Warning">
                <th scope="Row">2</th>
                <td>Jacob</td>
                <td>Thornton</td>
                <td>fat</td>
            </tr>
            <tr>
                <th scope="Row">3</th>
                <td table-style="Success">Larry</td>
                <td>the Bird</td>
                <td>twitter</td>
            </tr>
        </tbody>
    </abp-table>
````



## Demonstração

Veja a página de demonstração de [tabelas](https://bootstrap-taghelpers.abp.io/Components/Tables) para vê-la em ação.

## Atributos do abp-table

- **responsive**: Usado para criar tabelas responsivas até um determinado ponto de interrupção. Veja [específico do ponto de interrupção](https://getbootstrap.com/docs/4.1/content/tables/#breakpoint-specific) para mais informações.
- **responsive-sm**: Se não for definido como false, define a responsividade da tabela para dispositivos de tela pequena.
- **responsive-md**: Se não for definido como false, define a responsividade da tabela para dispositivos de tela média.
- **responsive-lg**: Se não for definido como false, define a responsividade da tabela para dispositivos de tela grande.
- **responsive-xl**: Se não for definido como false, define a responsividade da tabela para dispositivos de tela extra grande.
- **dark-theme**: Se definido como true, define o tema de cor da tabela como escuro.
- **striped-rows**: Se definido como true, adiciona listras zebradas às linhas da tabela.
- **hoverable-rows**: Se definido como true, adiciona estado de hover às linhas da tabela.
- **border-style**: Define o estilo da borda da tabela. Deve ser um dos seguintes valores:
  - `Default` (padrão)
  - `Bordered`
  - `Borderless`