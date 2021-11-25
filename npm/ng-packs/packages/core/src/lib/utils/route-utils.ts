import { NgZone } from '@angular/core';
import { PRIMARY_OUTLET, Router, UrlSegmentGroup } from '@angular/router';
import { ABP } from '../models/common';
import { RoutesService } from '../services/routes.service';
import { noop } from './common-utils';
import { TreeNode } from './tree-utils';

export function findRoute(routesService: RoutesService, path: string): TreeNode<ABP.Route> {
  const node = routesService.find(route => route.path === path);

  return node || path === '/'
    ? node
    : findRoute(routesService, path.split('/').slice(0, -1).join('/') || '/');
}

export function getRoutePath(router: Router, url = router.url) {
  const emptyGroup = { segments: [] } as UrlSegmentGroup;
  const primaryGroup = router.parseUrl(url).root.children[PRIMARY_OUTLET];

  return '/' + (primaryGroup || emptyGroup).segments.map(({ path }) => path).join('/');
}

export function reloadRoute(router: Router, ngZone: NgZone) {
  const { shouldReuseRoute } = router.routeReuseStrategy;
  const setRouteReuse = (reuse: typeof shouldReuseRoute) => {
    router.routeReuseStrategy.shouldReuseRoute = reuse;
  };

  setRouteReuse(() => false);
  router.navigated = false;

  ngZone.run(async () => {
    await router.navigateByUrl(router.url).catch(noop);
    setRouteReuse(shouldReuseRoute);
  });
}
