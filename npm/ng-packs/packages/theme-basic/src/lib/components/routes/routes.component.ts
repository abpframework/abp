import { ABP, ConfigState } from '@abp/ng.core';
import { Component, Input, Renderer2, TrackByFunction } from '@angular/core';
import { Select } from '@ngxs/store';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'abp-routes',
  templateUrl: 'routes.component.html',
})
export class RoutesComponent {
  @Select(ConfigState.getOne('routes'))
  routes$: Observable<ABP.FullRoute[]>;

  @Input()
  smallScreen: boolean;

  get visibleRoutes$(): Observable<ABP.FullRoute[]> {
    return this.routes$.pipe(map(routes => getVisibleRoutes(routes)));
  }

  trackByFn: TrackByFunction<ABP.FullRoute> = (_, item) => item.name;

  constructor(private renderer: Renderer2) {}
}

function getVisibleRoutes(routes: ABP.FullRoute[]) {
  return routes.reduce((acc, val) => {
    if (val.invisible) return acc;

    if (val.children && val.children.length) {
      val.children = getVisibleRoutes(val.children);
    }

    return [...acc, val];
  }, []);
}
