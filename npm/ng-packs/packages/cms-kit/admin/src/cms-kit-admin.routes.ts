import { Routes } from '@angular/router';
import { Provider } from '@angular/core';
import {
  RouterOutletComponent,
  authGuard,
  permissionGuard,
  ReplaceableRouteContainerComponent,
} from '@abp/ng.core';
import { eCmsKitAdminComponents } from './enums';
import {
  CommentListComponent,
  CommentDetailsComponent,
  TagListComponent,
  PageListComponent,
  PageFormComponent,
  BlogListComponent,
  BlogPostListComponent,
  BlogPostFormComponent,
  MenuItemListComponent,
  GlobalResourcesComponent,
} from './components';
import {
  CMS_KIT_ADMIN_ENTITY_ACTION_CONTRIBUTORS,
  CMS_KIT_ADMIN_ENTITY_PROP_CONTRIBUTORS,
  CMS_KIT_ADMIN_TOOLBAR_ACTION_CONTRIBUTORS,
  CMS_KIT_ADMIN_CREATE_FORM_PROP_CONTRIBUTORS,
  CMS_KIT_ADMIN_EDIT_FORM_PROP_CONTRIBUTORS,
} from './tokens';
import { cmsKitAdminExtensionsResolver } from './resolvers';
import { CmsKitAdminConfigOptions } from './models';

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
          resolve: {
            extensions: cmsKitAdminExtensionsResolver,
          },
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
          path: 'comments/:id',
          component: ReplaceableRouteContainerComponent,
          resolve: {
            extensions: cmsKitAdminExtensionsResolver,
          },
          data: {
            requiredPolicy: 'CmsKit.Comments',
            replaceableComponent: {
              key: eCmsKitAdminComponents.CommentDetails,
              defaultComponent: CommentDetailsComponent,
            },
          },
          title: 'CmsKit::Comments',
        },
        {
          path: 'tags',
          component: ReplaceableRouteContainerComponent,
          resolve: {
            extensions: cmsKitAdminExtensionsResolver,
          },
          data: {
            requiredPolicy: 'CmsKit.Tags',
            replaceableComponent: {
              key: eCmsKitAdminComponents.Tags,
              defaultComponent: TagListComponent,
            },
          },
          title: 'CmsKit::Tags',
        },
        {
          path: 'pages',
          component: ReplaceableRouteContainerComponent,
          resolve: {
            extensions: cmsKitAdminExtensionsResolver,
          },
          data: {
            requiredPolicy: 'CmsKit.Pages',
            replaceableComponent: {
              key: eCmsKitAdminComponents.Pages,
              defaultComponent: PageListComponent,
            },
          },
          title: 'CmsKit::Pages',
        },
        {
          path: 'pages/create',
          component: ReplaceableRouteContainerComponent,
          resolve: {
            extensions: cmsKitAdminExtensionsResolver,
          },
          data: {
            requiredPolicy: 'CmsKit.Pages.Create',
            replaceableComponent: {
              key: eCmsKitAdminComponents.PageForm,
              defaultComponent: PageFormComponent,
            },
          },
          title: 'CmsKit::Pages',
        },
        {
          path: 'pages/update/:id',
          component: ReplaceableRouteContainerComponent,
          resolve: {
            extensions: cmsKitAdminExtensionsResolver,
          },
          data: {
            requiredPolicy: 'CmsKit.Pages.Update',
            replaceableComponent: {
              key: eCmsKitAdminComponents.PageForm,
              defaultComponent: PageFormComponent,
            },
          },
          title: 'CmsKit::Pages',
        },
        {
          path: 'blogs',
          component: ReplaceableRouteContainerComponent,
          resolve: {
            extensions: cmsKitAdminExtensionsResolver,
          },
          data: {
            requiredPolicy: 'CmsKit.Blogs',
            replaceableComponent: {
              key: eCmsKitAdminComponents.Blogs,
              defaultComponent: BlogListComponent,
            },
          },
          title: 'CmsKit::Blogs',
        },
        {
          path: 'blog-posts',
          component: ReplaceableRouteContainerComponent,
          resolve: {
            extensions: cmsKitAdminExtensionsResolver,
          },
          data: {
            requiredPolicy: 'CmsKit.BlogPosts',
            replaceableComponent: {
              key: eCmsKitAdminComponents.BlogPosts,
              defaultComponent: BlogPostListComponent,
            },
          },
          title: 'CmsKit::BlogPosts',
        },
        {
          path: 'blog-posts/create',
          component: ReplaceableRouteContainerComponent,
          resolve: {
            extensions: cmsKitAdminExtensionsResolver,
          },
          data: {
            requiredPolicy: 'CmsKit.BlogPosts.Create',
            replaceableComponent: {
              key: eCmsKitAdminComponents.BlogPostForm,
              defaultComponent: BlogPostFormComponent,
            },
          },
          title: 'CmsKit::BlogPosts',
        },
        {
          path: 'blog-posts/update/:id',
          component: ReplaceableRouteContainerComponent,
          resolve: {
            extensions: cmsKitAdminExtensionsResolver,
          },
          data: {
            requiredPolicy: 'CmsKit.BlogPosts.Update',
            replaceableComponent: {
              key: eCmsKitAdminComponents.BlogPostForm,
              defaultComponent: BlogPostFormComponent,
            },
          },
          title: 'CmsKit::BlogPosts',
        },
        {
          path: 'menus',
          component: ReplaceableRouteContainerComponent,
          resolve: {
            extensions: cmsKitAdminExtensionsResolver,
          },
          data: {
            requiredPolicy: 'CmsKit.Menus',
            replaceableComponent: {
              key: eCmsKitAdminComponents.Menus,
              defaultComponent: MenuItemListComponent,
            },
          },
          title: 'CmsKit::MenuItems',
        },
        {
          path: 'global-resources',
          component: GlobalResourcesComponent,
          data: {
            requiredPolicy: 'CmsKit.GlobalResources',
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
      provide: CMS_KIT_ADMIN_ENTITY_ACTION_CONTRIBUTORS,
      useValue: options.entityActionContributors,
    },
    {
      provide: CMS_KIT_ADMIN_ENTITY_PROP_CONTRIBUTORS,
      useValue: options.entityPropContributors,
    },
    {
      provide: CMS_KIT_ADMIN_TOOLBAR_ACTION_CONTRIBUTORS,
      useValue: options.toolbarActionContributors,
    },
    {
      provide: CMS_KIT_ADMIN_CREATE_FORM_PROP_CONTRIBUTORS,
      useValue: options.createFormPropContributors,
    },
    {
      provide: CMS_KIT_ADMIN_EDIT_FORM_PROP_CONTRIBUTORS,
      useValue: options.editFormPropContributors,
    },
  ];
}
