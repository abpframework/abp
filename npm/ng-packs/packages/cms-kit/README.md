# @abp/ng.cms-kit

ABP CMS Kit Angular package providing admin and public functionality for content management.

## Structure

This package is organized into two main sub-packages:

- **Admin** (`admin/`) - Admin interface for managing CMS content
- **Public** (`public/`) - Public-facing components for displaying CMS content

## Installation

```bash
npm install @abp/ng.cms-kit
```

## Usage

### Admin

```typescript
import { provideCmsKitAdminConfig } from '@abp/ng.cms-kit/admin/config';

// In your app config
export const appConfig: ApplicationConfig = {
  providers: [
    provideCmsKitAdminConfig(),
    // ... other providers
  ],
};

// In your routes
export const routes: Routes = [
  {
    path: 'cms',
    loadChildren: () => import('@abp/ng.cms-kit/admin').then(m => m.createRoutes()),
  },
];
```

### Public

```typescript
import { provideCmsKitPublicConfig } from '@abp/ng.cms-kit/public/config';

// In your app config
export const appConfig: ApplicationConfig = {
  providers: [
    provideCmsKitPublicConfig(),
    // ... other providers
  ],
};

// In your routes
export const routes: Routes = [
  {
    path: 'cms',
    loadChildren: () => import('@abp/ng.cms-kit/public').then(m => m.createRoutes()),
  },
];
```

## Features

### Admin Features

- Comments management
- Tags management
- Pages management
- Blogs management
- Blog posts management
- Menus management
- Global resources management

### Public Features

- Public page viewing
- Public blog and blog post viewing
- Commenting functionality
- Shared components (MarkedItemToggle, PopularTags, Rating, ReactionSelection, Tags)

## Documentation

For more information, see the [ABP Documentation](https://docs.abp.io).
