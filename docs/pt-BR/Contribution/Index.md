## Guia de Contribuição

O ABP é um projeto de [código aberto](https://github.com/abpframework) e orientado à comunidade. Este guia tem como objetivo ajudar alguém que queira contribuir com o projeto.

### Contribuição de código

Você sempre pode enviar solicitações pull ao repositório do Github.

- Clone o [repositório ABP](https://github.com/abpframework/abp/) do Github.
- Faça as alterações necessárias.
- Envie uma solicitação de recebimento.

Antes de fazer qualquer alteração, discuta-a sobre os [problemas](https://github.com/abpframework/abp/issues) do [Github](https://github.com/abpframework/abp/issues) . Dessa forma, nenhum outro desenvolvedor trabalhará no mesmo problema e seu PR terá uma chance melhor de ser aceito.

#### Correções de bugs e aprimoramentos

Você pode corrigir um bug conhecido ou trabalhar em uma melhoria planejada. Veja [a lista de problemas](https://github.com/abpframework/abp/issues) no Github.

#### Solicitações de recursos

Se você tem uma ideia de recurso para a estrutura ou módulos, [crie um problema](https://github.com/abpframework/abp/issues/new) no Github ou participe de uma discussão existente. Então você pode implementá-lo se for adotado pela comunidade.

### Tradução de documentos

Você pode traduzir a [documentação](https://abp.io/documents/) completa (incluindo esta) para o idioma materno. Nesse caso, siga estas etapas:

- Clone o [repositório ABP](https://github.com/abpframework/abp/) do Github.
- Para adicionar um novo idioma, crie uma nova pasta dentro da pasta [docs](https://github.com/abpframework/abp/tree/master/docs) . Os nomes das pastas podem ser "en", "es", "fr", "tr" e assim por diante, com base no idioma (consulte [todos os códigos de cultura](https://msdn.microsoft.com/en-us/library/hh441729.aspx) ).
- Obtenha a [pasta "en"](https://github.com/abpframework/abp/tree/master/docs/en) como uma referência para os nomes de arquivos e a estrutura de pastas. Mantenha o mesmo nome se estiver traduzindo a mesma documentação.
- Envie uma solicitação de recebimento (PR) depois de traduzir qualquer documento. Traduza documentos e envie PRs um por um. Não espere para terminar as traduções de todos os documentos.

Alguns documentos fundamentais precisam ser traduzidos antes da publicação de um idioma no [site de documentação](https://docs.abp.io/) da [ABP](https://docs.abp.io/) :

- Documentos de introdução
- Tutoriais
- CLI

Um novo idioma é publicado após a conclusão dessas traduções mínimas.

### Localização de Recursos

A estrutura ABP possui um [sistema de localização](../Localization.md) flexível . Você pode criar interfaces de usuário localizadas para seu próprio aplicativo.

Além disso, os módulos de estrutura e pré-construção já localizaram textos. Como exemplo, veja [os textos de localização para o pacote Volo.Abp.UI](https://github.com/abpframework/abp/blob/master/framework/src/Volo.Abp.UI/Localization/Resources/AbpUi/en.json) . Você pode criar um novo arquivo na [mesma pasta](https://github.com/abpframework/abp/tree/master/framework/src/Volo.Abp.UI/Localization/Resources/AbpUi) para traduzi-lo.

- Clone o [repositório ABP](https://github.com/abpframework/abp/) do Github.
- Crie um novo arquivo para o idioma de destino para um arquivo de texto de localização (json) (próximo ao arquivo en.json).
- Copie todos os textos do arquivo en.json.
- Traduzir os textos.
- Enviar solicitação de recebimento no Github.

ABP é uma estrutura modular. Portanto, existem muitos recursos de texto de localização, um por módulo. Para encontrar todos os arquivos .json, você pode procurar por "en.json" após clonar o repositório. Você também pode verificar [esta lista](https://docs.abp.io/en/abp/latest/Contribution/Localization-Text-Files) para obter uma lista de arquivos de texto de localização.

### Posts e tutoriais do blog

Se você decidir criar alguns tutoriais ou postagens de blog no ABP, informe-nos (criando um [problema no Github](https://github.com/abpframework/abp/issues) ), para que possamos adicionar um link ao seu tutorial / publicação na documentação oficial e podemos anunciá-lo em nossa [conta do Twitter](https://twitter.com/abpframework) .

### Relatório de erro

Se você encontrar algum erro, [crie um problema no repositório do Github](https://github.com/abpframework/abp/issues/new) .