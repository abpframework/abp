import { ABP } from '../models/common';

export function organizeRoutes(
  routes: ABP.FullRoute[],
  wrappers: ABP.FullRoute[] = [],
  parentNameArr = [] as ABP.FullRoute[],
  parentName: string = null,
): ABP.FullRoute[] {
  const filter = route => {
    if (route.children && route.children.length) {
      route.children = organizeRoutes(route.children, wrappers, parentNameArr, route.name);
    }

    if (route.parentName && route.parentName !== parentName) {
      parentNameArr.push(route);
      return false;
    }

    return true;
  };

  if (parentName) {
    // recursive block
    return routes.filter(filter);
  }

  const filteredRoutes = routes.filter(filter);

  if (parentNameArr.length) {
    return sortRoutes(setChildRoute([...filteredRoutes, ...wrappers], parentNameArr));
  }

  return filteredRoutes;
}

export function setChildRoute(
  routes: ABP.FullRoute[],
  parentNameArr: ABP.FullRoute[],
): ABP.FullRoute[] {
  return routes.map(route => {
    if (route.children && route.children.length) {
      route.children = setChildRoute(route.children, parentNameArr);
    }

    const foundedChildren = parentNameArr.filter(parent => parent.parentName === route.name);
    if (foundedChildren && foundedChildren.length) {
      route.children = [...(route.children || []), ...foundedChildren];
    }

    return route;
  });
}

export function sortRoutes(routes: ABP.FullRoute[] = []): ABP.FullRoute[] {
  if (!routes.length) return [];
  return routes
    .map((route, index) => {
      return {
        ...route,
        order: typeof route.order === 'undefined' ? index + 1 : route.order,
      };
    })
    .sort((a, b) => a.order - b.order)
    .map(route => {
      if (route.children && route.children.length) {
        route.children = sortRoutes(route.children);
      }

      return route;
    });
}

const ABP_ROUTES = [] as ABP.FullRoute[];

export function addAbpRoutes(routes: ABP.FullRoute | ABP.FullRoute[]): void {
  if (!Array.isArray(routes)) {
    routes = [routes];
  }

  ABP_ROUTES.push(...routes);
}

export function getAbpRoutes(): ABP.FullRoute[] {
  return ABP_ROUTES;
}
