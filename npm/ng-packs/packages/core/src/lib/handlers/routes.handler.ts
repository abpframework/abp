import { Injectable, Optional } from '@angular/core';
import { Route, Router } from '@angular/router';
import { ABP } from '../models';
import { RoutesService } from '../services/routes.service';

@Injectable({
  providedIn: 'root',
})
export class RoutesHandler {
  constructor(private routes: RoutesService, @Optional() private router: Router) {
    this.addRoutes();
  }

  addRoutes() {
    (this.router?.config as RouteData[])?.forEach(({ path = '', data }: RouteData) => {
      const routes = data?.routes;
      if (!routes) return;

      if (Array.isArray(routes)) {
        this.routes.add(routes);
      } else {
        const routesFlatten = flatRoutes([{ path, ...routes }], { path: '' });
        this.routes.add(routesFlatten);
      }
    });
  }
}

function flatRoutes(routes: RouteDef[], parent: any) {
  if (!routes) return [];

  return routes.reduce((acc, route) => {
    const { children, ...current } = {
      ...route,
      parentName: parent.name,
      path: (parent.path + '/' + route.path).replace(/\/\//g, '/'),
    };

    acc.push(current, ...flatRoutes(children, current));

    return acc;
  }, [] as ABP.Route[]);
}

type RouteDef = ABP.Route & { children: RouteDef[] };
type RouteData = Route & { data: { routes: RouteDef | Array<RouteDef> } };
