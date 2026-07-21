import { eLayoutType, RoutesService } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';
import { eCmsKitAdminPolicyNames } from '../enums/policy-names';
import { eCmsKitAdminRouteNames } from '../enums/route-names';

export const CMS_KIT_ADMIN_ROUTE_PROVIDERS = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

export function configureRoutes() {
  const routesService = inject(RoutesService);
  routesService.add([
    {
      path: '/cms/blog-posts',
      name: eCmsKitAdminRouteNames.BlogPosts,
      parentName: eCmsKitAdminRouteNames.Cms,
      order: 1,
      layout: eLayoutType.application,
      iconClass: 'fa fa-file-alt',
      requiredPolicy: eCmsKitAdminPolicyNames.BlogPosts,
    },
    {
      path: '/cms/blogs',
      name: eCmsKitAdminRouteNames.Blogs,
      parentName: eCmsKitAdminRouteNames.Cms,
      order: 2,
      layout: eLayoutType.application,
      iconClass: 'fa fa-blog',
      requiredPolicy: eCmsKitAdminPolicyNames.Blogs,
    },
    {
      path: '/cms/comments',
      name: eCmsKitAdminRouteNames.Comments,
      parentName: eCmsKitAdminRouteNames.Cms,
      order: 3,
      layout: eLayoutType.application,
      iconClass: 'fa fa-comments',
      requiredPolicy: eCmsKitAdminPolicyNames.Comments,
    },
    {
      path: '/cms/global-resources',
      name: eCmsKitAdminRouteNames.GlobalResources,
      parentName: eCmsKitAdminRouteNames.Cms,
      order: 5,
      layout: eLayoutType.application,
      iconClass: 'fa fa-globe',
      requiredPolicy: eCmsKitAdminPolicyNames.GlobalResources,
    },
    {
      path: '/cms/menus',
      name: eCmsKitAdminRouteNames.Menus,
      parentName: eCmsKitAdminRouteNames.Cms,
      order: 6,
      layout: eLayoutType.application,
      iconClass: 'fa fa-bars',
      requiredPolicy: eCmsKitAdminPolicyNames.Menus,
    },
    {
      path: '/cms/pages',
      name: eCmsKitAdminRouteNames.Pages,
      parentName: eCmsKitAdminRouteNames.Cms,
      order: 9,
      layout: eLayoutType.application,
      iconClass: 'fa fa-file',
      requiredPolicy: eCmsKitAdminPolicyNames.Pages,
    },
    {
      path: '/cms/tags',
      name: eCmsKitAdminRouteNames.Tags,
      parentName: eCmsKitAdminRouteNames.Cms,
      order: 11,
      layout: eLayoutType.application,
      iconClass: 'fa fa-tags',
      requiredPolicy: eCmsKitAdminPolicyNames.Tags,
    },
  ]);
}
