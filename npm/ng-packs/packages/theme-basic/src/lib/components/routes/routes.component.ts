import { Component, OnInit, TrackByFunction, Input, Renderer2 } from '@angular/core';
import { Observable } from 'rxjs';
import { ABP, ConfigState } from '@abp/ng.core';
import { map } from 'rxjs/operators';
import { Select } from '@ngxs/store';

@Component({
  selector: 'abp-routes',
  templateUrl: 'routes.component.html',
})
export class RoutesComponent {
  @Select(ConfigState.getOne('routes'))
  routes$: Observable<ABP.FullRoute[]>;

  @Input()
  smallScreen: boolean;

  @Input()
  isDropdownChildDynamic: boolean;

  get visibleRoutes$(): Observable<ABP.FullRoute[]> {
    return this.routes$.pipe(map(routes => getVisibleRoutes(routes)));
  }

  trackByFn: TrackByFunction<ABP.FullRoute> = (_, item) => item.name;

  constructor(private renderer: Renderer2) {}

  openChange(event: boolean, childrenContainer: HTMLDivElement) {
    if (!event) {
      Object.keys(childrenContainer.style)
        .filter(key => Number.isInteger(+key))
        .forEach(key => {
          this.renderer.removeStyle(childrenContainer, childrenContainer.style[key]);
        });
      this.renderer.removeStyle(childrenContainer, 'left');
    }
  }
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
