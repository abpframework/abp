```json
//[doc-seo]
{
    "Description": "Explore LeptonX CSS Variables to customize theming with ease, controlling colors, spacing, and styles for a cohesive application design."
}
```

# LeptonX CSS Variables Documentation

LeptonX uses CSS custom properties (variables) prefixed with `--lpx-*` to provide a flexible theming system. These variables control colors, spacing, shadows, and component-specific styles throughout the application.

## Brand & Semantic Colors

| Variable | Description |
|----------|-------------|
| `--lpx-brand` | Brand-specific accent color |
| `--lpx-brand-text` | Text color used on brand-colored backgrounds |

## Base Colors

| Variable | Description |
|----------|-------------|
| `--lpx-light` | Light shade for subtle backgrounds or text |
| `--lpx-dark` | Dark shade for contrasting elements |

## Layout & Surface Colors

| Variable | Description |
|----------|-------------|
| `--lpx-content-bg` | Main content area background color |
| `--lpx-content-text` | Default text color for content areas |
| `--lpx-card-bg` | Card component background color |
| `--lpx-card-title-text-color` | Card title text color |
| `--lpx-border-color` | Default border color for dividers and outlines |
| `--lpx-shadow` | Box shadow definition for elevated elements |

## Navigation

| Variable | Description |
|----------|-------------|
| `--lpx-navbar-color` | Navbar background color |
| `--lpx-navbar-text-color` | Navbar default text/icon color |
| `--lpx-navbar-active-text-color` | Navbar active/hover text color |
| `--lpx-navbar-active-bg-color` | Navbar active item background color |

## Utility

| Variable | Description |
|----------|-------------|
| `--lpx-radius` | Global border-radius value for rounded corners |

## Global Override

Applies to all themes and pages:

```css
:root {
  /* Brand & Semantic */
  --lpx-brand: #f72585;

  /* Base Colors */
  --lpx-light: #f5f7fb;
  --lpx-dark: #0b0f19;

  /* Layout & Surface */
  --lpx-content-bg: #101018;
  --lpx-content-text: #cfd6e4;
  --lpx-card-bg: #151a2b;
  --lpx-card-title-text-color: #ffffff;
  --lpx-border-color: #242836;
  --lpx-shadow: 0 10px 30px rgba(0, 0, 0, 0.25);

  /* Navigation */
  --lpx-navbar-color: #0d1020;
  --lpx-navbar-text-color: #aab2c8;
  --lpx-navbar-active-text-color: #ffffff;
  --lpx-navbar-active-bg-color: rgba(247, 37, 133, 0.15);

  /* Utility */
  --lpx-radius: 10px;
}

```

## Theme-Scoped Override

Applies only when a specific theme class is active (e.g., `.lpx-theme-dark` on `<html>` or `<body>`):

```css
:root .lpx-theme-dark {
  /* Brand & Semantic */
  --lpx-brand: #4dd0e1;

  /* Base Colors */
  --lpx-light: #e0f7fa;
  --lpx-dark: #020617;

  /* Layout & Surface */
  --lpx-content-bg: #0b1118;
  --lpx-content-text: #c7d0e0;
  --lpx-card-bg: #111a24;
  --lpx-card-title-text-color: #e6f1ff;
  --lpx-border-color: #1e2a3a;
  --lpx-shadow: 0 12px 32px rgba(0, 0, 0, 0.45);

  /* Navigation */
  --lpx-navbar-color: #0f1a22;
  --lpx-navbar-text-color: #9fb3c8;
  --lpx-navbar-active-text-color: #ffffff;
  --lpx-navbar-active-bg-color: rgba(77, 208, 225, 0.18);

  /* Utility */
  --lpx-radius: 12px;
}
```

## Component/Page-Specific Override

For targeted customizations that should only affect a specific section:

```css
.my-custom-page {
  --lpx-brand: #e91e63;
  --lpx-card-bg: #1a1a2e;
}
```