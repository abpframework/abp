# Figuras

`abp-figure` é o contêiner principal para os itens de figura do bootstrap.

Uso básico:

````html
<abp-figure>
  <abp-image src="..." class="img-fluid rounded" alt="Uma imagem de espaço reservado quadrado genérico com cantos arredondados em uma figura.">
  <abp-figcaption class="text-end">Uma legenda para a imagem acima.</abp-figcaption>
</abp-figure>
````

Ele adiciona a classe `figure` ao contêiner principal, também adiciona a classe `figure-img` ao elemento `abp-image` interno e a classe `figure-caption` ao elemento `abp-figcaption` interno.