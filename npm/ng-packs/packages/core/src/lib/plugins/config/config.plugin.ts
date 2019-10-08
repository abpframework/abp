import { Inject, Injectable, InjectionToken } from '@angular/core';
import { Router, Routes } from '@angular/router';
import { actionMatcher, InitState, NgxsNextPluginFn, NgxsPlugin, setValue, UpdateState } from '@ngxs/store';
import snq from 'snq';
import { ABP } from '../../models';
import { organizeRoutes } from '../../utils/route-utils';
import clone from 'just-clone';

export const NGXS_CONFIG_PLUGIN_OPTIONS = new InjectionToken('NGXS_CONFIG_PLUGIN_OPTIONS');

let ABP_ROUTES = [] as ABP.FullRoute[];

export function addAbpRoutes(routes: ABP.FullRoute | ABP.FullRoute[]): void {
  if (!Array.isArray(routes)) {
    routes = [routes];
  }

  ABP_ROUTES.push(...routes);
}

export function getAbpRoutes(): ABP.FullRoute[] {
  return [];
}

@Injectable()
export class ConfigPlugin implements NgxsPlugin {
  private initialized = false;

  constructor(@Inject(NGXS_CONFIG_PLUGIN_OPTIONS) private options: ABP.Root, private router: Router) {}

  handle(state: any, event: any, next: NgxsNextPluginFn) {
    const matches = actionMatcher(event);
    const isInitAction = matches(InitState) || matches(UpdateState);

    if (isInitAction && !this.initialized) {
      const transformedRoutes = transformRoutes(this.router.config);
      let { routes } = transformedRoutes;
      const { wrappers } = transformedRoutes;

      routes = organizeRoutes(routes, wrappers);
      const flattedRoutes = flatRoutes(clone(routes));
      state = setValue(state, 'ConfigState', {
        ...(state.ConfigState && { ...state.ConfigState }),
        ...this.options,
        routes,
        flattedRoutes,
      });

      this.initialized = true;
    }

    return next(state, event);
  }
}

function transformRoutes(routes: Routes = [], wrappers: ABP.FullRoute[] = []): any {
  /**
   *
   * @deprecated since version 0.9.0
   */
  const abpRoutes: ABP.FullRoute[] = routes
    .filter(route => {
      return snq(() => route.data.routes.routes.find(r => r.path === route.path), false);
    })
    .reduce((acc, val) => [...acc, ...val.data.routes.routes], []);
  // tslint:disable-next-line: deprecation
  ABP_ROUTES = [...ABP_ROUTES, ...abpRoutes];

  wrappers = ABP_ROUTES.filter(ar => ar.wrapper);
  const transformed = [] as ABP.FullRoute[];
  routes
    .filter(route => route.component || route.loadChildren)
    .forEach(route => {
      const abpPackage = ABP_ROUTES.find(abp => abp.path.toLowerCase() === route.path.toLowerCase() && !abp.wrapper);

      const { length } = transformed;

      if (abpPackage) {
        transformed.push(abpPackage);
      }

      if (transformed.length === length && (route.data || {}).routes) {
        transformed.push({
          ...route.data.routes,
          path: route.path,
          name: snq(() => route.data.routes.name, route.path),
          children: route.data.routes.children || [],
        } as ABP.FullRoute);
      }
    });

  return { routes: setUrls(transformed), wrappers };
}

function setUrls(routes: ABP.FullRoute[], parentUrl?: string): ABP.FullRoute[] {
  if (parentUrl) {
    // this if block using for only recursive call

    return routes.map(route => ({
      ...route,
      url: `${parentUrl}/${route.path}`,
      ...(route.children &&
        route.children.length && {
          children: setUrls(route.children, `${parentUrl}/${route.path}`),
        }),
    }));
  }

  return routes.map(route => ({
    ...route,
    url: `/${route.path}`,
    ...(route.children &&
      route.children.length && {
        children: setUrls(route.children, `/${route.path}`),
      }),
  }));
}

function flatRoutes(routes: ABP.FullRoute[]): ABP.FullRoute[] {
  const flat = (r: ABP.FullRoute[]) => {
    return r.reduce((acc, val) => {
      let value: ABP.FullRoute[] = [val];
      if (val.children) {
        value = [val, ...flat(val.children)];
      }

      return [...acc, ...value];
    }, []);
  };

  return flat(routes);
}
