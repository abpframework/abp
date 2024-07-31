# Lepton X SCSS Files

The LeptonX scss file structure is divided into different bundles. The purpose of this is to exclude unnecessary styles (for example, the top menu layer styles when using side menu layout) in the project. To reload only relevant files on theme changes.

## Bundle Files

Folders containing bundle files in source files do not contain underscores. Those containing underscores are files used by the bundle files.

**Theme bundle files**: dim.scss dark.scss light.scss placed under themes directory.

**Bootstrap bundle files**: dark/booststrap-dark.scss, light/booststrap-light.scss and dim/booststrap-dim.scss placed under the frameworks/bootstrap directory.

**Layout bundle files**: side-menu/layout-bundle.scss and top-menu/layout-bundle.scss placed under the pro directory

**Ui spescific bundles**: The Angular UI bundle is pro/libraries/ng-bundle.scss, the Blazor UI bundle is pro/libraries/blazor-bundle.scss, the MVC UI bundle is pro/libraries/js-bundle.scss

**ABP bundle file**: Styles of ABP UI elements is pro/abp/abp-bundle.scss

**Font bundle**: pro/libraries/font-bundle.scss

## Theme Map

Theme maps are defined in theme color files. The position of the files is listed as following:
dark: \_colors/dark/colors.scss
light: \_colors/light/colors.scss
dim: \_colors/dim/colors.scss

Possible properties are listed below

```
border-color,
brand,
brand-text,
card-bg,
card-title-text-color,
container-active-text,
content-bg,
content-text,
danger,
dark
info,
light
logo,
logo-icon,
logo-reverse,
navbar-active-bg-color
navbar-active-text-color
navbar-color
navbar-text-color
primary,
radius
secondary,
shadow,
success,
text-white,
warning
```

## Theme Builder

The build-theme mixin reads the theme-map and writes its values to the :root selector as CSS variables by using defined builder functions.

The global-theme-builder converts the rgb colors of specific property values in the theme map

## Compiling to CSS

> Please make sure that the dependencies are installed. You can install with the `yarn install` or `npm install` command.

For building source files please run the command below

```bash
yarn build
```

CSS files will be created in the built folder.

## Adding new theme bundle to source file

Create a new file under \_colors/your-theme/colors.scss and replace the content below

```scss
$theme-map: (
  light: #f0f4f7,
  dark: #062a44,
  navbar-color: #fff,
  navbar-text-color: #445f72,
  navbar-active-text-color: #124163,
  navbar-active-bg-color: #f3f6f9,
);
```

Create a new file \_colors/your-theme/index.scss and paste content below in it

```scss
@import "colors";
@import "../common";
```

Create a new file frameworks/bootstrap/your-theme/bootstrap-your-theme.scss and paste content below in it

```scss
@import "_colors/your-theme";
@import "../common";
```

Finally create a new file themes/your-theme.scss and paste content below in it

```scss
@import "_colors/your-theme";
@import "builders/_builder";
```

## Other Files

- build.js: Builds scss bundle files to css files.Also creates rtl.css files.
- package.json: Includes dependencies and build command
- postcss.config.js: Used by postcss, needed for ltr to rtl
