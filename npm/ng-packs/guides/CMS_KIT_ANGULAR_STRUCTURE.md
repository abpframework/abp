# CMS Kit - Angular Package Structure Guide

## Overview

This guide provides a comprehensive structure for the Angular package (`@abp/ng.cms-kit`) that mirrors the backend MVC architecture. The structure is organized into two main parts: **Admin** and **Public**, matching the backend `Volo.CmsKit.Admin.Web` and `Volo.CmsKit.Public.Web` modules.

## Backend Structure Reference

Based on the backend MVC structure, CMS Kit has the following features:

### Admin Features (Volo.CmsKit.Admin.Web)

1. **Comments** - Comment approval and management
2. **Tags** - Tag creation and management
3. **Pages** - Page creation and management
4. **Blogs** - Blog management
5. **Blog Posts** - Blog post creation and management
6. **Menus** - Menu item management
7. **Global Resources** - Global script and style resources

### Public Features (Volo.CmsKit.Public.Web)

1. **Pages** - Public page viewing
2. **Blogs** - Public blog and blog post viewing
3. **Comments** - Public commenting functionality
4. **Shared Components** - Reusable public components (Commenting, MarkedItemToggle, PopularTags, Rating, ReactionSelection, Tags, GlobalResources)
5. **Menus** - Public menu items

## Package Structure

```
cms-kit/
├── package.json              # Main package metadata
├── ng-package.json           # ng-packagr configuration
├── project.json              # Nx workspace configuration
├── tsconfig.json             # TypeScript root config
├── tsconfig.lib.json         # Library-specific TS config
├── tsconfig.lib.prod.json    # Production build config
├── tsconfig.spec.json        # Test configuration
├── jest.config.ts            # Jest test configuration
├── README.md                 # Package documentation
│
├── admin/                    # Admin sub-package (Admin pages)
│   │── src/
│   │    ├── components/   # Admin-specific components
│   │    │   ├── comments/
│   │    │   │   ├── comment-list/
│   │    │   │   │   ├── comment-list.component.ts
│   │    │   │   │   ├── comment-list.component.html
│   │    │   │   │   └── comment-list.component.spec.ts
│   │    │   │   ├── comment-approve/
│   │    │   │   │   ├── comment-approve.component.ts
│   │    │   │   │   ├── comment-approve.component.html
│   │    │   │   │   └── comment-approve.component.spec.ts
│   │    │   │   ├── comment-details/
│   │    │   │   │   ├── comment-details.component.ts
│   │    │   │   │   ├── comment-details.component.html
│   │    │   │   │   └── comment-details.component.spec.ts
│   │    │   │   └── index.ts
│   │    │   │
│   │    │   ├── tags/
│   │    │   │   ├── tag-list/
│   │    │   │   │   ├── tag-list.component.ts
│   │    │   │   │   ├── tag-list.component.html
│   │    │   │   │   └── tag-list.component.spec.ts
│   │    │   │   ├── tag-modal/ # manages the create/edit
│   │    │   │   │   ├── tag-modal.component.ts
│   │    │   │   │   ├── tag-modal.component.html
│   │    │   │   │   └── tag-modal.component.spec.ts
│   │    │   │   └── index.ts
│   │    │   │
│   │    │   ├── pages/
│   │    │   │   ├── page-list/
│   │    │   │   │   ├── page-list.component.ts
│   │    │   │   │   ├── page-list.component.html
│   │    │   │   │   └── page-list.component.spec.ts
│   │    │   │   ├── page-modal/ # manages the create/edit
│   │    │   │   │   ├── page-modal.component.ts
│   │    │   │   │   ├── page-modal.component.html
│   │    │   │   │   └── page-modal.component.spec.ts
│   │    │   │   └── index.ts
│   │    │   │
│   │    │   ├── blogs/
│   │    │   │   ├── blog-list/
│   │    │   │   │   ├── blog-list.component.ts
│   │    │   │   │   ├── blog-list.component.html
│   │    │   │   │   └── blog-list.component.spec.ts
│   │    │   │   ├── blog-create/
│   │    │   │   │   ├── blog-create.component.ts
│   │    │   │   │   ├── blog-create.component.html
│   │    │   │   │   └── blog-create.component.spec.ts
│   │    │   │   ├── blog-edit/
│   │    │   │   │   ├── blog-edit.component.ts
│   │    │   │   │   ├── blog-edit.component.html
│   │    │   │   │   └── blog-edit.component.spec.ts
│   │    │   │   ├── blog-features/
│   │    │   │   │   ├── blog-features.component.ts
│   │    │   │   │   ├── blog-features.component.html
│   │    │   │   │   └── blog-features.component.spec.ts
│   │    │   │   └── index.ts
│   │    │   │
│   │    │   ├── blog-posts/
│   │    │   │   ├── blog-post-list/
│   │    │   │   │   ├── blog-post-list.component.ts
│   │    │   │   │   ├── blog-post-list.component.html
│   │    │   │   │   └── blog-post-list.component.spec.ts
│   │    │   │   ├── blog-post-create/
│   │    │   │   │   ├── blog-post-create.component.ts
│   │    │   │   │   ├── blog-post-create.component.html
│   │    │   │   │   └── blog-post-create.component.spec.ts
│   │    │   │   ├── blog-post-edit/
│   │    │   │   │   ├── blog-post-edit.component.ts
│   │    │   │   │   ├── blog-post-edit.component.html
│   │    │   │   │   └── blog-post-edit.component.spec.ts
│   │    │   │   └── index.ts
│   │    │   │
│   │    │   ├── menus/
│   │    │   │   ├── menu-item-list/
│   │    │   │   │   ├── menu-item-list.component.ts
│   │    │   │   │   ├── menu-item-list.component.html
│   │    │   │   │   └── menu-item-list.component.spec.ts
│   │    │   │   ├── menu-item-modal/ # manages the create/edit
│   │    │   │   │   ├── menu-item-modal.component.ts
│   │    │   │   │   ├── menu-item-modal.component.html
│   │    │   │   │   └── menu-item-modal.component.spec.ts
│   │    │   │   └── index.ts
│   │    │   │
│   │    │   ├── global-resources/
│   │    │   │   ├── global-resource-list/
│   │    │   │   │   ├── global-resource-list.component.ts
│   │    │   │   │   ├── global-resource-list.component.html
│   │    │   │   │   └── global-resource-list.component.spec.ts
│   │    │   │   └── index.ts
│   │    │   │
│   │    │   └── index.ts
│   │    │
│   │    ├── services/     # Admin-specific services
│   │    │   ├── comments/
│   │    │   │   ├── comment-admin.service.ts
│   │    │   │   └── index.ts
│   │    │   ├── tags/
│   │    │   │   ├── tag-admin.service.ts
│   │    │   │   └── index.ts
│   │    │   ├── pages/
│   │    │   │   ├── page-admin.service.ts
│   │    │   │   └── index.ts
│   │    │   ├── blogs/
│   │    │   │   ├── blog-admin.service.ts
│   │    │   │   └── index.ts
│   │    │   ├── blog-posts/
│   │    │   │   ├── blog-post-admin.service.ts
│   │    │   │   └── index.ts
│   │    │   ├── menus/
│   │    │   │   ├── menu-item-admin.service.ts
│   │    │   │   └── index.ts
│   │    │   ├── global-resources/
│   │    │   │   ├── global-resource-admin.service.ts
│   │    │   │   └── index.ts
│   │    │   └── index.ts
│   │    │
│   │    ├── models/       # Admin-specific models
│   │    │   ├── config-options.ts
│   │    │   └── index.ts
│   │    │
│   │    ├── tokens/        # Admin-specific DI tokens
│   │    │   ├── extensions.token.ts
│   │    │   └── index.ts
│   │    │
│   │    ├── enums/         # Admin-specific enums
│   │    │   ├── components.ts
│   │    │   └── index.ts
│   │    │
│   │    ├── resolvers/         # Admin-specific resolvers
│   │    │   ├── extensions.resolver.ts
│   │    │   └── index.ts
│   │    │
│   │    ├── cms-kit-admin.routes.ts
│   │    │
│   │    └── public-api.ts # Admin public exports
│   └── config/
│       ├── src/
│       │   ├── enums/         # Admin-specific enums
│       │   │   ├── policy-names.ts
│       │   │   ├── route-names.ts
│       │   │   └── index.ts
│       │   │
│       │   ├── providers/         # Admin-specific resolvers
│       │   │   ├── cms-kit-admin-config.provider.ts
│       │   │   ├── route.provider.ts
│       │   │   └── index.ts
│       │   │
│       │   └── public-api.ts # Admin public config exports
│       │
│       └── ng-package.json
│
├── public/                    # Public sub-package (Public pages)
│   └── src/
│   │   ├── lib/
│   │   │   ├── components/   # Public-specific components
│   │   │   │   ├── pages/
│   │   │   │   │   ├── page-view/
│   │   │   │   │   │   ├── page-view.component.ts
│   │   │   │   │   │   ├── page-view.component.html
│   │   │   │   │   │   └── page-view.component.spec.ts
│   │   │   │   │   └── index.ts
│   │   │   │   │
│   │   │   │   ├── blogs/
│   │   │   │   │   ├── blog-list/
│   │   │   │   │   │   ├── blog-list.component.ts
│   │   │   │   │   │   ├── blog-list.component.html
│   │   │   │   │   │   └── blog-list.component.spec.ts
│   │   │   │   │   ├── blog-post-view/
│   │   │   │   │   │   ├── blog-post-view.component.ts
│   │   │   │   │   │   ├── blog-post-view.component.html
│   │   │   │   │   │   └── blog-post-view.component.spec.ts
│   │   │   │   │   └── index.ts
│   │   │   │   │
│   │   │   │   ├── comments/
│   │   │   │   │   ├── commenting/
│   │   │   │   │   │   ├── commenting.component.ts
│   │   │   │   │   │   ├── commenting.component.html
│   │   │   │   │   │   ├── commenting.component.scss
│   │   │   │   │   │   └── commenting.component.spec.ts
│   │   │   │   │   └── index.ts
│   │   │   │   │
│   │   │   │   ├── shared/   # Shared public components
│   │   │   │   │   ├── marked-item-toggle/
│   │   │   │   │   │   ├── marked-item-toggle.component.ts
│   │   │   │   │   │   ├── marked-item-toggle.component.html
│   │   │   │   │   │   ├── marked-item-toggle.component.scss
│   │   │   │   │   │   └── marked-item-toggle.component.spec.ts
│   │   │   │   │   ├── popular-tags/
│   │   │   │   │   │   ├── popular-tags.component.ts
│   │   │   │   │   │   ├── popular-tags.component.html
│   │   │   │   │   │   ├── popular-tags.component.scss
│   │   │   │   │   │   └── popular-tags.component.spec.ts
│   │   │   │   │   ├── rating/
│   │   │   │   │   │   ├── rating.component.ts
│   │   │   │   │   │   ├── rating.component.html
│   │   │   │   │   │   ├── rating.component.scss
│   │   │   │   │   │   └── rating.component.spec.ts
│   │   │   │   │   ├── reaction-selection/
│   │   │   │   │   │   ├── reaction-selection.component.ts
│   │   │   │   │   │   ├── reaction-selection.component.html
│   │   │   │   │   │   ├── reaction-selection.component.scss
│   │   │   │   │   │   └── reaction-selection.component.spec.ts
│   │   │   │   │   ├── tags/
│   │   │   │   │   │   ├── tags.component.ts
│   │   │   │   │   │   ├── tags.component.html
│   │   │   │   │   │   ├── tags.component.scss
│   │   │   │   │   │   └── tags.component.spec.ts
│   │   │   │   │   └── index.ts
│   │   │   │   │
│   │   │   │   └── index.ts
│   │   │   │
│   │   │   ├── services/     # Public-specific services
│   │   │   │   ├── pages/
│   │   │   │   │   ├── page-public.service.ts
│   │   │   │   │   └── index.ts
│   │   │   │   ├── blogs/
│   │   │   │   │   ├── blog-post-public.service.ts
│   │   │   │   │   └── index.ts
│   │   │   │   ├── comments/
│   │   │   │   │   ├── comment-public.service.ts
│   │   │   │   │   └── index.ts
│   │   │   │   ├── menus/
│   │   │   │   │   ├── menu-item-public.service.ts
│   │   │   │   │   └── index.ts
│   │   │   │   ├── global-resources/
│   │   │   │   │   ├── global-resource-public.service.ts
│   │   │   │   │   └── index.ts
│   │   │   │   ├── marked-items/
│   │   │   │   │   ├── marked-item-public.service.ts
│   │   │   │   │   └── index.ts
│   │   │   │   └── index.ts
│   │   │   │
│   │   │   ├── models/       # Public-specific models
│   │   │   │   ├── config-options.ts
│   │   │   │   └── index.ts
│   │   │   │
│   │   │   ├── tokens/        # Public-specific DI tokens
│   │   │   │   ├── extensions.token.ts
│   │   │   │   └── index.ts
│   │   │   │
│   │   │   ├── enums/         # Public-specific enums
│   │   │   │   ├── components.ts
│   │   │   │   └── index.ts
│   │   │   │
│   │   │   ├── resolvers/         # Public-specific resolvers
│   │   │   │   ├── extensions.resolver.ts
│   │   │   │   └── index.ts
│   │   │   │
│   │   │   └── public-api.ts # Public public exports
│   │   │
│   │   └── public-api.ts      # Public root exports
│   └── config/
│       ├── src/
│       │   ├── enums/         # Public-specific enums
│       │   │   ├── policy-names.ts
│       │   │   ├── route-names.ts
│       │   │   └── index.ts
│       │   │
│       │   ├── providers/         # Public-specific resolvers
│       │   │   ├── cms-kit-public-config.provider.ts
│       │   │   ├── route.provider.ts
│       │   │   └── index.ts
│       │   │
│       │   └── public-api.ts # Public public config exports
│       │
│       └── public-api.ts      # Public root exports
└── proxy/                     # API proxy sub-package (Auto-generated - DO NOT TOUCH)
    └── src/
        ├── lib/
        │   └── proxy/
        │       ├── admin/
        │       │   ├── comments/
        │       │   │   └── comment-admin.service.ts
        │       │   ├── tags/
        │       │   │   └── tag-admin.service.ts
        │       │   ├── pages/
        │       │   │   └── page-admin.service.ts
        │       │   ├── blogs/
        │       │   │   └── blog-admin.service.ts
        │       │   ├── blog-posts/
        │       │   │   └── blog-post-admin.service.ts
        │       │   ├── menus/
        │       │   │   └── menu-item-admin.service.ts
        │       │   ├── global-resources/
        │       │   │   └── global-resource-admin.service.ts
        │       │   └── media-descriptors/
        │       │       └── media-descriptor-admin.service.ts
        │       ├── public/
        │       │   ├── pages/
        │       │   │   └── page-public.service.ts
        │       │   ├── blogs/
        │       │   │   └── blog-post-public.service.ts
        │       │   ├── comments/
        │       │   │   └── comment-public.service.ts
        │       │   ├── menus/
        │       │   │   └── menu-item-public.service.ts
        │       │   ├── global-resources/
        │       │   │   └── global-resource-public.service.ts
        │       │   └── marked-items/
        │       │       └── marked-item-public.service.ts
        │       ├── generate-proxy.json
        │       └── README.md
        │
        └── public-api.ts
```

## Feature Breakdown

### Admin Features

#### 1. Comments Feature

**Backend DTOs:**

- `CommentDto`
- `CommentWithAuthorDto`
- `GetCommentListInput`
- `CommentApprovalDto`

**Backend Services:**

- `ICommentAdminAppService`

**Angular Components:**

- `CommentListComponent` - List view with filtering
- `CommentApproveComponent` - Comment approval interface
- `CommentDetailsComponent` - Comment details view

**Angular Services:**

- `CommentAdminService` - Extends RestService

#### 2. Tags Feature

**Backend DTOs:**

- `TagDto`
- `CreateTagDto`
- `UpdateTagDto`
- `GetTagListInput`

**Backend Services:**

- `ITagAdminAppService`

**Angular Components:**

- `TagListComponent` - List view with search
- `TagModalComponent` - Create/Edit form

**Angular Services:**

- `TagAdminService` - Extends RestService

#### 3. Pages Feature

**Backend DTOs:**

- `PageDto`
- `GetPagesInputDto`
- `CreatePageInputDto`
- `UpdatePageInputDto`

**Backend Services:**

- `IPageAdminAppService`

**Angular Components:**

- `PageListComponent` - List view with filtering
- `PageModalComponent` - Create/Edit form

**Angular Services:**

- `PageAdminService` - Extends RestService

#### 4. Blogs Feature

**Backend DTOs:**

- `BlogDto`
- `CreateBlogDto`
- `UpdateBlogDto`
- `GetBlogListInput`

**Backend Services:**

- `IBlogAdminAppService`

**Angular Components:**

- `BlogListComponent` - List view
- `BlogCreateComponent` - Create form
- `BlogEditComponent` - Edit form
- `BlogFeaturesComponent` - Feature management

**Angular Services:**

- `BlogAdminService` - Extends RestService

#### 5. Blog Posts Feature

**Backend DTOs:**

- `BlogPostDto`
- `BlogPostListDto`
- `BlogPostGetListInput`
- `CreateBlogPostDto`
- `UpdateBlogPostDto`

**Backend Services:**

- `IBlogPostAdminAppService`

**Angular Components:**

- `BlogPostListComponent` - List view with filtering
- `BlogPostCreateComponent` - Create form
- `BlogPostEditComponent` - Edit form

**Angular Services:**

- `BlogPostAdminService` - Extends RestService

#### 6. Menus Feature

**Backend DTOs:**

- `MenuItemDto`
- `MenuItemWithDetailsDto`
- `MenuItemCreateInput`
- `MenuItemUpdateInput`
- `MenuItemMoveInput`
- `PageLookupDto`
- `PageLookupInputDto`
- `PermissionLookupDto`
- `PermissionLookupInputDto`

**Backend Services:**

- `IMenuItemAdminAppService`

**Angular Components:**

- `MenuItemListComponent` - Tree/list view
- `MenuItemModalComponent` - Create/Edit form

**Angular Services:**

- `MenuItemAdminService` - Extends RestService

#### 7. Global Resources Feature

**Backend DTOs:**

- `GlobalResourceDto`
- `CreateGlobalResourceDto`
- `UpdateGlobalResourceDto`
- `GetGlobalResourceListInput`

**Backend Services:**

- `IGlobalResourceAdminAppService`

**Angular Components:**

- `GlobalResourceListComponent` - List view

**Angular Services:**

- `GlobalResourceAdminService` - Extends RestService

### Public Features

#### 1. Pages Feature

**Backend DTOs:**

- `PageDto`

**Backend Services:**

- `IPagePublicAppService`

**Angular Components:**

- `PageViewComponent` - Public page display

**Angular Services:**

- `PagePublicService` - Extends RestService

#### 2. Blogs Feature

**Backend DTOs:**

- `BlogDto`
- `BlogPostDto`
- `BlogPostGetListInput`

**Backend Services:**

- `IBlogPostPublicAppService`

**Angular Components:**

- `BlogListComponent` - Public blog list
- `BlogPostViewComponent` - Public blog post display

**Angular Services:**

- `BlogPostPublicService` - Extends RestService

#### 3. Comments Feature

**Backend DTOs:**

- `CommentDto`
- `CommentWithAuthorDto`
- `CreateCommentInput`
- `UpdateCommentInput`

**Backend Services:**

- `ICommentPublicAppService`

**Angular Components:**

- `CommentingComponent` - Public commenting interface

**Angular Services:**

- `CommentPublicService` - Extends RestService

#### 4. Shared Public Components

**Components:**

- `MarkedItemToggleComponent` - Favorite/Bookmark/Star/Flag toggle
- `PopularTagsComponent` - Popular tags display
- `RatingComponent` - Rating interface
- `ReactionSelectionComponent` - Reaction selection interface
- `TagsComponent` - Tags display

#### 5. Menus Feature

**Backend DTOs:**

- `MenuItemDto`

**Backend Services:**

- `IMenuItemPublicAppService`

**Angular Services:**

- `MenuItemPublicService` - Extends RestService

## Component Structure Examples

### Admin List Component Pattern

```typescript
// admin/src/lib/components/pages/page-list/page-list.component.ts

import { Component, OnInit, inject } from '@angular/core';
import { ListService } from '@abp/ng.core';
import { PageAdminService } from '../../services';
import { PageDto } from '../../models';

@Component({
  selector: 'abp-page-list',
  templateUrl: './page-list.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eCmsKitAdminComponents.PageList,
    },
  ],
  imports: [],
})
export class PageListComponent implements OnInit {
  data = this.list.getGrid();

  public readonly list = inject(ListService);
  private pageService = inject(PageAdminService);

  ngOnInit() {
    this.hookToQuery();
  }

  private hookToQuery() {
    this.list
      .hookToQuery(query => this.pageService.getList({ ...query, ...this.filters }))
      .subscribe(res => (this.data = res));
  }
}
```

### Public View Component Pattern

```typescript
// public/src/lib/components/pages/page-view/page-view.component.ts

import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PagePublicService } from '../../services';
import { PageDto } from '../../models';

@Component({
  selector: 'abp-page-view',
  templateUrl: './page-view.component.html',
  imports: [],
})
export class PageViewComponent implements OnInit {
  page: PageDto;
  slug: string;

  private route = inject(ActivatedRoute);
  private pageService = inject(PagePublicService);

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.slug = params['slug'];
      this.loadPage();
    });
  }

  private loadPage() {
    this.pageService.findBySlug(this.slug).subscribe(page => {
      this.page = page;
    });
  }
}
```

### Service Pattern

```typescript
// admin/src/lib/services/pages/page-admin.service.ts

import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';
import { PageDto, GetPagesInputDto, CreatePageInputDto, UpdatePageInputDto } from '../models';

@Injectable({
  providedIn: 'root',
})
export class PageAdminService extends RestService {
  protected get url() {
    return '/api/cms-kit-admin/pages';
  }

  getList(input?: GetPagesInputDto) {
    return this.request<ListResultDto<PageDto>>({
      method: 'GET',
      url: this.url,
      params: input,
    });
  }

  getById(id: string) {
    return this.request<PageDto>({
      method: 'GET',
      url: `${this.url}/${id}`,
    });
  }

  create(input: CreatePageInputDto) {
    return this.request<PageDto>({
      method: 'POST',
      url: this.url,
      body: input,
    });
  }

  update(id: string, input: UpdatePageInputDto) {
    return this.request<PageDto>({
      method: 'PUT',
      url: `${this.url}/${id}`,
      body: input,
    });
  }

  delete(id: string) {
    return this.request<void>({
      method: 'DELETE',
      url: `${this.url}/${id}`,
    });
  }

  setAsHomePage(id: string) {
    return this.request<void>({
      method: 'POST',
      url: `${this.url}/${id}/set-as-home-page`,
    });
  }
}
```

## Route Configuration

### Admin Route Definition

```typescript
// admin/src/lib/routes/cms-kit-admin.routes.ts

import { Routes } from '@angular/router';
import { Provider } from '@angular/core';
import {
  RouterOutletComponent,
  authGuard,
  permissionGuard,
  ReplaceableRouteContainerComponent,
} from '@abp/ng.core';
import { eCmsKitAdminComponents } from '../enums';
import {
  CommentListComponent,
  TagListComponent,
  PageListComponent,
  BlogListComponent,
  BlogPostListComponent,
  MenuItemListComponent,
  GlobalResourceListComponent,
} from '../components';

export interface CmsKitAdminConfigOptions {
  // Extension point contributors
  commentEntityActionContributors?: any;
  tagEntityActionContributors?: any;
  // ... other contributors
}

export function createRoutes(config: CmsKitAdminConfigOptions = {}): Routes {
  return [
    {
      path: '',
      component: RouterOutletComponent,
      providers: provideCmsKitAdminContributors(config),
      canActivate: [authGuard, permissionGuard],
      children: [
        {
          path: 'comments',
          component: ReplaceableRouteContainerComponent,
          data: {
            requiredPolicy: 'CmsKit.Comments',
            replaceableComponent: {
              key: eCmsKitAdminComponents.CommentList,
              defaultComponent: CommentListComponent,
            },
          },
          title: 'CmsKit::Comments',
        },
        {
          path: 'tags',
          component: ReplaceableRouteContainerComponent,
          data: {
            requiredPolicy: 'CmsKit.Tags',
            replaceableComponent: {
              key: eCmsKitAdminComponents.TagList,
              defaultComponent: TagListComponent,
            },
          },
          title: 'CmsKit::Tags',
        },
        {
          path: 'pages',
          component: ReplaceableRouteContainerComponent,
          data: {
            requiredPolicy: 'CmsKit.Pages',
            replaceableComponent: {
              key: eCmsKitAdminComponents.PageList,
              defaultComponent: PageListComponent,
            },
          },
          title: 'CmsKit::Pages',
        },
        {
          path: 'pages/create',
          component: ReplaceableRouteContainerComponent,
          data: {
            requiredPolicy: 'CmsKit.Pages.Create',
            replaceableComponent: {
              key: eCmsKitAdminComponents.PageCreate,
              defaultComponent: PageCreateComponent,
            },
          },
          title: 'CmsKit::CreatePage',
        },
        {
          path: 'pages/:id/edit',
          component: ReplaceableRouteContainerComponent,
          data: {
            requiredPolicy: 'CmsKit.Pages.Update',
            replaceableComponent: {
              key: eCmsKitAdminComponents.PageEdit,
              defaultComponent: PageEditComponent,
            },
          },
          title: 'CmsKit::EditPage',
        },
        {
          path: 'blogs',
          component: ReplaceableRouteContainerComponent,
          data: {
            requiredPolicy: 'CmsKit.Blogs',
            replaceableComponent: {
              key: eCmsKitAdminComponents.BlogList,
              defaultComponent: BlogListComponent,
            },
          },
          title: 'CmsKit::Blogs',
        },
        {
          path: 'blog-posts',
          component: ReplaceableRouteContainerComponent,
          data: {
            requiredPolicy: 'CmsKit.BlogPosts',
            replaceableComponent: {
              key: eCmsKitAdminComponents.BlogPostList,
              defaultComponent: BlogPostListComponent,
            },
          },
          title: 'CmsKit::BlogPosts',
        },
        {
          path: 'menus',
          component: ReplaceableRouteContainerComponent,
          data: {
            requiredPolicy: 'CmsKit.Menus',
            replaceableComponent: {
              key: eCmsKitAdminComponents.MenuItemList,
              defaultComponent: MenuItemListComponent,
            },
          },
          title: 'CmsKit::Menus',
        },
        {
          path: 'global-resources',
          component: ReplaceableRouteContainerComponent,
          data: {
            requiredPolicy: 'CmsKit.GlobalResources',
            replaceableComponent: {
              key: eCmsKitAdminComponents.GlobalResourceList,
              defaultComponent: GlobalResourceListComponent,
            },
          },
          title: 'CmsKit::GlobalResources',
        },
      ],
    },
  ];
}

function provideCmsKitAdminContributors(options: CmsKitAdminConfigOptions = {}): Provider[] {
  return [
    {
      provide: COMMENT_ENTITY_ACTION_CONTRIBUTORS,
      useValue: options.commentEntityActionContributors,
    },
    {
      provide: TAG_ENTITY_ACTION_CONTRIBUTORS,
      useValue: options.tagEntityActionContributors,
    },
    // ... other contributors
  ];
}
```

### Admin Route Provider Configuration

```typescript
// config/src/lib/providers/cms-kit-admin-route-providers.ts

import { Provider } from '@angular/core';
import { RoutesService, provideAppInitializer } from '@abp/ng.core';
import { eCmsKitAdminRouteNames } from '@abp/cms-kit/admin';

export const CMS_KIT_ADMIN_ROUTE_PROVIDERS: Provider[] = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

export function configureRoutes() {
  const routes = inject(RoutesService);
  routes.add([
    {
      path: '/cms-kit/comments',
      name: eCmsKitAdminRouteNames.Comments,
      parentName: eThemeSharedRouteNames.Administration,
      order: 100,
      layout: eLayoutType.application,
      iconClass: 'fa fa-comments',
      requiredPolicy: 'CmsKit.Comments',
    },
    {
      path: '/cms-kit/tags',
      name: eCmsKitAdminRouteNames.Tags,
      parentName: eThemeSharedRouteNames.Administration,
      order: 101,
      layout: eLayoutType.application,
      iconClass: 'fa fa-tags',
      requiredPolicy: 'CmsKit.Tags',
    },
    {
      path: '/cms-kit/pages',
      name: eCmsKitAdminRouteNames.Pages,
      parentName: eThemeSharedRouteNames.Administration,
      order: 102,
      layout: eLayoutType.application,
      iconClass: 'fa fa-file',
      requiredPolicy: 'CmsKit.Pages',
    },
    {
      path: '/cms-kit/blogs',
      name: eCmsKitAdminRouteNames.Blogs,
      parentName: eThemeSharedRouteNames.Administration,
      order: 103,
      layout: eLayoutType.application,
      iconClass: 'fa fa-blog',
      requiredPolicy: 'CmsKit.Blogs',
    },
    {
      path: '/cms-kit/blog-posts',
      name: eCmsKitAdminRouteNames.BlogPosts,
      parentName: eThemeSharedRouteNames.Administration,
      order: 104,
      layout: eLayoutType.application,
      iconClass: 'fa fa-file-alt',
      requiredPolicy: 'CmsKit.BlogPosts',
    },
    {
      path: '/cms-kit/menus',
      name: eCmsKitAdminRouteNames.Menus,
      parentName: eThemeSharedRouteNames.Administration,
      order: 105,
      layout: eLayoutType.application,
      iconClass: 'fa fa-bars',
      requiredPolicy: 'CmsKit.Menus',
    },
    {
      path: '/cms-kit/global-resources',
      name: eCmsKitAdminRouteNames.GlobalResources,
      parentName: eThemeSharedRouteNames.Administration,
      order: 106,
      layout: eLayoutType.application,
      iconClass: 'fa fa-globe',
      requiredPolicy: 'CmsKit.GlobalResources',
    },
  ]);
}
```

## Default Extension Points

### Entity Actions Example

```typescript
// admin/src/lib/defaults/pages/default-page-entity-actions.ts

import { EntityAction } from '@abp/ng.components/extensible';
import { PageDto } from '../../models';
import { eCmsKitAdminComponents } from '../../enums';

export const DEFAULT_PAGE_ENTITY_ACTIONS = EntityAction.createMany<PageDto>([
  {
    text: 'CmsKit::Edit',
    action: data => {
      const router = data.getInjected(Router);
      router.navigate(['/cms-kit/pages', data.record.id, 'edit']);
    },
    permission: 'CmsKit.Pages.Update',
  },
  {
    text: 'CmsKit::Delete',
    action: data => {
      const pageService = data.getInjected(PageAdminService);
      const confirmation = data.getInjected(ConfirmationService);

      confirmation
        .warn('CmsKit::PageDeletionConfirmationMessage', 'CmsKit::AreYouSure', {
          yesText: 'AbpUi::Yes',
          cancelText: 'AbpUi::Cancel',
        })
        .subscribe(status => {
          if (status === Confirmation.Status.confirm) {
            pageService.delete(data.record.id).subscribe(() => {
              data.list.get();
            });
          }
        });
    },
    permission: 'CmsKit.Pages.Delete',
  },
]);
```

### Entity Props Example

```typescript
// admin/src/lib/defaults/pages/default-page-entity-props.ts

import { EntityProp } from '@abp/ng.components/extensible';
import { ePropType } from '@abp/ng.theme.shared/extensions';
import { PageDto } from '../../models';

export const DEFAULT_PAGE_ENTITY_PROPS = EntityProp.createMany<PageDto>([
  {
    type: ePropType.String,
    name: 'title',
    displayName: 'CmsKit::Title',
    sortable: true,
    columnWidth: 300,
  },
  {
    type: ePropType.String,
    name: 'slug',
    displayName: 'CmsKit::Slug',
    sortable: true,
    columnWidth: 200,
  },
  {
    type: ePropType.Date,
    name: 'creationTime',
    displayName: 'AbpIdentity::CreationTime',
    sortable: true,
    columnWidth: 200,
  },
]);
```

## Model Definitions

### DTO Models

```typescript
// admin/src/lib/models/pages/page-dto.model.ts

export interface PageDto {
  id: string;
  title: string;
  slug: string;
  content: string;
  script: string;
  style: string;
  layout: string;
  seoTitle: string;
  seoDescription: string;
  seoKeywords: string;
  isHomePage: boolean;
  creationTime: string;
  [key: string]: any; // For extensibility
}

export interface GetPagesInputDto {
  filter?: string;
  status?: string;
  sorting?: string;
  skipCount?: number;
  maxResultCount?: number;
}

export interface CreatePageInputDto {
  title: string;
  slug: string;
  content: string;
  script?: string;
  style?: string;
  layout?: string;
  seoTitle?: string;
  seoDescription?: string;
  seoKeywords?: string;
}

export interface UpdatePageInputDto extends Partial<CreatePageInputDto> {
  id: string;
}
```

## Enums

```typescript
// admin/src/lib/enums/e-cms-kit-admin-components.enum.ts

export enum eCmsKitAdminComponents {
  CommentList = 'CmsKit.Admin.CommentList',
  CommentApprove = 'CmsKit.Admin.CommentApprove',
  CommentDetails = 'CmsKit.Admin.CommentDetails',

  TagList = 'CmsKit.Admin.TagList',
  TagCreate = 'CmsKit.Admin.TagCreate',
  TagEdit = 'CmsKit.Admin.TagEdit',

  PageList = 'CmsKit.Admin.PageList',
  PageCreate = 'CmsKit.Admin.PageCreate',
  PageEdit = 'CmsKit.Admin.PageEdit',

  BlogList = 'CmsKit.Admin.BlogList',
  BlogCreate = 'CmsKit.Admin.BlogCreate',
  BlogEdit = 'CmsKit.Admin.BlogEdit',
  BlogFeatures = 'CmsKit.Admin.BlogFeatures',

  BlogPostList = 'CmsKit.Admin.BlogPostList',
  BlogPostCreate = 'CmsKit.Admin.BlogPostCreate',
  BlogPostEdit = 'CmsKit.Admin.BlogPostEdit',

  MenuItemList = 'CmsKit.Admin.MenuItemList',
  MenuItemCreate = 'CmsKit.Admin.MenuItemCreate',
  MenuItemEdit = 'CmsKit.Admin.MenuItemEdit',

  GlobalResourceList = 'CmsKit.Admin.GlobalResourceList',
}

// admin/src/lib/enums/e-cms-kit-admin-route-names.enum.ts

export enum eCmsKitAdminRouteNames {
  Comments = 'CmsKit::Menu:Comments',
  Tags = 'CmsKit::Menu:Tags',
  Pages = 'CmsKit::Menu:Pages',
  Blogs = 'CmsKit::Menu:Blogs',
  BlogPosts = 'CmsKit::Menu:BlogPosts',
  Menus = 'CmsKit::Menu:Menus',
  GlobalResources = 'CmsKit::Menu:GlobalResources',
}
```

## Implementation Checklist

### Phase 1: Foundation

- [x] Create package structure (admin, public, proxy)
- [x] Set up TypeScript configurations
- [x] Set up ng-package.json files
- [x] Create base models and enums
- [x] Set up service base classes

### Phase 2: Admin - Comments Feature

- [x] Create CommentListComponent
- [x] Create CommentApproveComponent
- [x] Create CommentDetailsComponent
- [x] Create default extension points
- [x] Add routes and providers

### Phase 3: Admin - Tags Feature

- [x] Create Tag components
- [x] Create default extension points
- [x] Add routes and providers

### Phase 4: Admin - Pages Feature

- [x] Create Page components
- [x] Create default extension points
- [x] Add routes and providers

### Phase 5: Admin - Blogs Feature

- [x] Create Blog components
- [x] Create default extension points
- [x] Add routes and providers

### Phase 6: Admin - Blog Posts Feature

- [x] Create BlogPost components
- [x] Create default extension points
- [x] Add routes and providers

### Phase 7: Admin - Menus Feature

- [x] Create MenuItem components
- [x] Create default extension points
- [x] Add routes and providers

### Phase 8: Admin - Global Resources Feature

- [x] Create GlobalResourceListComponent
- [x] Create default extension points
- [x] Add routes and providers

### Phase 9: Public - Pages Feature

- [ ] Create PageViewComponent
- [ ] Add routes

### Phase 10: Public - Blogs Feature

- [ ] Create BlogListComponent
- [ ] Create BlogPostViewComponent
- [ ] Add routes

### Phase 11: Public - Comments Feature

- [ ] Create CommentingComponent
- [ ] Add routes

### Phase 12: Public - Shared Components

- [ ] Create MarkedItemToggleComponent
- [ ] Create PopularTagsComponent
- [ ] Create RatingComponent
- [ ] Create ReactionSelectionComponent
- [ ] Create TagsComponent

### Phase 13: Testing & Documentation

- [ ] Write unit tests for services
- [ ] Write unit tests for components
- [ ] Write integration tests

## Best Practices

1. **Naming Conventions**
   - Files: kebab-case (`page-list.component.ts`)
   - Classes: PascalCase (`PageListComponent`)
   - Variables/Methods: camelCase (`pageService`, `getList()`)
   - Constants: UPPER_SNAKE_CASE (`DEFAULT_PAGE_ENTITY_ACTIONS`)
   - Enums: PascalCase with 'e' prefix (`eCmsKitAdminComponents`)

2. **Service Layer**
   - Always extend `RestService` from `@abp/ng.core`
   - Use TypeScript interfaces for all DTOs
   - Handle errors consistently
   - Use observables properly with RxJS

3. **Component Layer**
   - Use `ListService` for list components
   - Implement `OnInit` and `OnDestroy` when needed
   - Use reactive forms for all forms
   - Implement proper validation

4. **Extension Points**
   - Provide default implementations
   - Allow customization through contributors
   - Use tokens for dependency injection

5. **Routes**
   - Use lazy loading
   - Implement guards (auth, permission)
   - Use replaceable components
   - Follow ABP route naming conventions

## Dependencies

### Core Dependencies

- `@abp/ng.core` - Core ABP Angular functionality
- `@abp/ng.theme.shared` - Shared theme components
- `@abp/ng.components/extensible` - Extensible components

### Development Dependencies

- `@angular/` - Angular framework
- `@nx/` - Nx workspace tools
- `ng-packagr` - Package building
- `jest` - Testing framework

## Notes

- The `proxy` sub-package is auto-generated and should NOT be manually edited
- All models should match the backend DTOs exactly
- Use the localization keys from the backend (`CmsKit::*`)
- Follow the ABP permission naming convention (`CmsKit.Feature.Action`)
- All components should be replaceable using the extension system
- Admin and Public packages are separate to maintain clear separation of concerns
- The proxy package contains both admin and public API proxies in a common location
