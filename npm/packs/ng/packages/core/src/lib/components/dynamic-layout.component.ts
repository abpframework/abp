import { Component, OnDestroy, Type } from '@angular/core';
import { NavigationEnd, Router, UrlSegment } from '@angular/router';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { eLayoutType } from '../enums';
import { ABP, Config } from '../models';
import { ConfigState } from '../states';
import { takeUntilDestroy } from '../utils';
import snq from 'snq';

@Component({
  selector: 'abp-dynamic-layout',
  template: `
    <ng-container *ngTemplateOutlet="layout ? componentOutlet : routerOutlet"></ng-container>

    <ng-template #routerOutlet><router-outlet></router-outlet></ng-template>
    <ng-template #componentOutlet><ng-container *ngComponentOutlet="layout"></ng-container></ng-template>
  `,
})
export class DynamicLayoutComponent implements OnDestroy {
  @Select(ConfigState.getOne('requirements'))
  requirements$: Observable<Config.Requirements>;

  layout: Type<any>;

  constructor(private router: Router, private store: Store) {
    this.router.events.pipe(takeUntilDestroy(this)).subscribe(event => {
      if (event instanceof NavigationEnd) {
        const { segments } = this.router.parseUrl(event.url).root.children.primary;
        const {
          requirements: { layouts },
          routes,
        } = this.store.selectSnapshot(ConfigState.getAll);

        const layout = findLayout(segments, routes);

        this.layout = layouts.filter(l => !!l).find(l => snq(() => l.type.toLowerCase().indexOf(layout), -1) > -1);
      }
    });
  }

  ngOnDestroy() {}
}

function findLayout(segments: UrlSegment[], routes: ABP.FullRoute[]): eLayoutType {
  let layout = eLayoutType.empty;

  const route = routes
    .reduce((acc, val) => (val.wrapper ? [...acc, ...val.children] : [...acc, val]), [])
    .find(r => r.path === segments[0].path);

  if (route) {
    if (route.layout) {
      layout = route.layout;
    }

    if (route.children && route.children.length) {
      const child = route.children.find(c => c.path === segments[1].path);

      if (child.layout) {
        layout = child.layout;
      }
    }
  }

  return layout;
}
