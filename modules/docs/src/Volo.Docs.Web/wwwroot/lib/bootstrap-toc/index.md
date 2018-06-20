---
layout: default
permalink: /
---

# Table of Contents plugin for Bootstrap
{: .page-header}

[![Build Status](https://travis-ci.org/afeld/bootstrap-toc.svg?branch=gh-pages)](https://travis-ci.org/afeld/bootstrap-toc)

This [Bootstrap](http://getbootstrap.com/) plugin allows you to generate a table of contents for any page, based on the heading elements (`<h1>`, `<h2>`, etc.). It is meant to emulate the sidebar you see on [the Bootstrap documentation site](http://getbootstrap.com/css/).

This page is an example of the plugin in action – the table of contents you see on the left (or top, on mobile) was automatically generated, without having to manually keep all of the navigation items in sync with the headings.

## Usage

On top of the normal Bootstrap setup (see their [Getting Started](http://getbootstrap.com/getting-started/) guide), you will need to include the Bootstrap Table of Contents stylesheet and JavaScript file.

```html
<!-- add after bootstrap.min.css -->
<link rel="stylesheet" href="https://cdn.rawgit.com/afeld/bootstrap-toc/v0.4.1/dist/bootstrap-toc.min.css">
<!-- add after bootstrap.min.js -->
<script src="https://cdn.rawgit.com/afeld/bootstrap-toc/v0.4.1/dist/bootstrap-toc.min.js"></script>
```

[Unminified versions](https://github.com/afeld/bootstrap-toc/tree/gh-pages/dist) are also available.

Next, pick one of the two options below.

### Via data attributes

*Simplest.*

Create a `<nav>` element with a `data-toggle="toc"` attribute.

```html
<nav id="toc" data-toggle="toc"></nav>
```

You can put this wherever on the page you like. Since this plugin leverages Bootstrap's [Scrollspy](http://getbootstrap.com/javascript/#scrollspy) plugin, you will also need to add a couple attributes to the `<body>`:

```html
<body data-spy="scroll" data-target="#toc">
```

### Via JavaScript

*If you need customization.*

If you prefer to create your navigation element another way (e.g. within single-page apps), you can pass a jQuery object into `Toc.init()`.

```html
<nav id="toc"></nav>
```

```javascript
$(function() {
  var navSelector = '#toc';
  var $myNav = $(navSelector);
  Toc.init($myNav);
  $('body').scrollspy({
    target: navSelector
  });
});
```

See the [Scrollspy](http://getbootstrap.com/javascript/#scrollspy) documentation for more information about initializing that plugin.

#### Options

When calling `Toc.init()`, you can either pass in the jQuery object for the `<nav>` element (as seen above), or an options object:

```javascript
Toc.init({
  $nav: $('#myNav'),
  // ...
});
```

All options are optional, unless otherwise indicated.

option | type | notes
--- | --- | ---
`$nav` | jQuery Object | (required) The element that the navigation will be created in.
`$scope` | jQuery Object | The element where the search for headings will be limited to, or the list of headings that will be used in the navigation. Defaults to `$(document.body)`.
{: .table }

## Customization

The following options can be specified at the heading level via `data-toc-*` attributes.

### Displayed text

By default, Bootstrap TOC will use the text from the heading element in the table of contents. If you want to customize what is displayed, add a `data-toc-text` attribute with the desired text. For example:

```html
<h2 data-toc-text="Short text">Longer text</h2>
```

displays "Longer text" as the heading, but "Short text" in the sidebar.

### Skipping

To prevent a particular heading from being added to the table of contents, add a `data-toc-skip` [boolean attribute](https://www.w3.org/TR/2008/WD-html5-20080610/semantics.html#boolean).

```html
<h2 data-toc-skip>Some heading you don't want in the nav</h2>
```

## Layout

This plugin isn't opinionated about where it should be placed on the page, but a common use case is to have the table of contents created as a "sticky" sidebar. We will leverage the [Affix](http://getbootstrap.com/javascript/#affix) plugin for this, and wrap the `<nav>` element in a `<div>` with a Bootstrap column class (see information about the [Grid](http://getbootstrap.com/css/#grid)). As an example putting it all together (similar to this page):

```html
<body data-spy="scroll" data-target="#toc">
  <div class="container">
    <div class="row">
      <!-- sidebar, which will move to the top on a small screen -->
      <div class="col-sm-3">
        <nav id="toc" data-spy="affix" data-toggle="toc"></nav>
      </div>
      <!-- main content area -->
      <div class="col-sm-9">
        ...
      </div>
    </div>
  </div>
</body>
```

You may also want to include this in your stylesheet:

```css
nav[data-toggle='toc'] {
  margin-top: 30px;
}

/* small screens */
@media (max-width: 768px) {
  /* override the Affix plugin so that the navigation isn't sticky */
  nav.affix[data-toggle='toc'] {
    position: static;
  }

  /* PICK ONE */
  /* don't expand nested items, which pushes down the rest of the page when navigating */
  nav[data-toggle='toc'] .nav .active .nav {
    display: none;
  }
  /* alternatively, if you *do* want the second-level navigation to be shown (as seen on this page on mobile), use this */
  nav[data-toggle='toc'] .nav .nav {
    display: block;
  }
}
```

## See also

This plugin was heavily inspired by:

* [Bootstrap Docs Sidebar example](https://jsfiddle.net/gableroux/S2SMK/)
* [Tocify plugin](http://gregfranko.com/jquery.tocify.js/)
* [TOC plugin](http://projects.jga.me/toc/)

## Contributing

Questions, feature suggestions, and bug reports/fixes welcome!

### Manual testing

1. Run `bundle`.
1. Run `bundle exec jekyll serve`.
1. Open the various test templates:
    * [H2's](http://localhost:4000/bootstrap-toc/test/templates/h2s.html)
    * [Markdown](http://localhost:4000/bootstrap-toc/test/templates/markdown.html)
    * [No IDs](http://localhost:4000/bootstrap-toc/test/templates/no-ids.html)

### Automated testing

1. Run `npm install`.
1. Run `gulp test`/`gulp watch` (command-line), or `open test/index.html` (browser).

You can find the tests in [`test/toc-test.js`](test/toc-test.js).
