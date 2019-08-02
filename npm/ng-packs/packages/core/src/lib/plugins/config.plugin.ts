import { Inject, Injectable, InjectionToken } from '@angular/core';
import { Router, Routes } from '@angular/router';
import { actionMatcher, InitState, NgxsNextPluginFn, NgxsPlugin, setValue, UpdateState } from '@ngxs/store';
import snq from 'snq';
import { ABP } from '../models';
import { organizeRoutes } from '../utils/route-utils';

export const NGXS_CONFIG_PLUGIN_OPTIONS = new InjectionToken('NGXS_CONFIG_PLUGIN_OPTIONS');

@Injectable()
export class ConfigPlugin implements NgxsPlugin {
  private initialized: boolean = false;

  constructor(@Inject(NGXS_CONFIG_PLUGIN_OPTIONS) private options: ABP.Root, private router: Router) {}

  handle(state: any, event: any, next: NgxsNextPluginFn) {
    const matches = actionMatcher(event);
    const isInitAction = matches(InitState) || matches(UpdateState);

    // const layouts = snq(() => this.options.requirements.layouts.filter(layout => layout instanceof Type), []);
    if (isInitAction && !this.initialized) {
      let { routes, wrappers } = transformRoutes(this.router.config);
      routes = organizeRoutes(routes, wrappers);

      state = setValue(state, 'ConfigState', {
        ...(state.ConfigState && { ...state.ConfigState }),
        ...this.options,
        routes,
      });

      this.initialized = true;
    }

    return next(state, event);
  }
}

function transformRoutes(routes: Routes = [], wrappers: ABP.FullRoute[] = []): any {
  const abpRoutes: ABP.FullRoute[] = routes
    .filter(route => {
      return snq(() => route.data.routes.find(r => r.path === route.path), false);
    })
    .reduce((acc, val) => [...acc, ...val.data.routes], []);

  wrappers = abpRoutes.filter(ar => ar.wrapper);
  const transformed = [] as ABP.FullRoute[];
  routes
    .filter(route => route.component || route.loadChildren || (route.data || {}).routes)
    .forEach(route => {
      const abpPackage = abpRoutes.find(abp => abp.path.toLowerCase() === route.path.toLowerCase());
      const { length } = transformed;

      if (abpPackage) {
        transformed.push(abpPackage);
      }

      if (transformed.length === length) {
        transformed.push({
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
