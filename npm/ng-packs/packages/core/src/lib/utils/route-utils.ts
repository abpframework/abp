import { PRIMARY_OUTLET, Router, UrlSegmentGroup } from '@angular/router';
import { ABP } from '../models/common';
import { RoutesService } from '../services/routes.service';
import { TreeNode } from './tree-utils';

export function findRoute(routes: RoutesService, path: string): TreeNode<ABP.Route> {
  const node = routes.find(route => route.path === path);

  return node || path === '/'
    ? node
    : findRoute(
        routes,
        path
          .split('/')
          .slice(0, -1)
          .join('/'),
      );
}

export function getRoutePath(router: Router) {
  const emptyGroup = { segments: [] } as UrlSegmentGroup;
  const primaryGroup = router.parseUrl(router.url).root.children[PRIMARY_OUTLET];

  return '/' + (primaryGroup || emptyGroup).segments.map(({ path }) => path).join('/');
}
