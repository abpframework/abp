# Figures

`abp-figure` is the main container for bootstrap figure items. 

Basic usage:

````html
<abp-figure>
  <abp-image src="..." class="img-fluid rounded" alt="A generic square placeholder image with rounded corners in a figure.">
  <abp-figcaption class="text-right">A caption for the above image.</abp-figcaption>
</abp-figure>
````

It adds `figure` class to main container, also adds `figure-img` class to inner `abp-image` element and `figure-caption` class to inner `abp-figcaption` element.