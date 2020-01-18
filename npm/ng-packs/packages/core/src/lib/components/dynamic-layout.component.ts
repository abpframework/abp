import { Component, Input, OnDestroy, Type } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, UrlSegment } from '@angular/router';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import snq from 'snq';
import { eLayoutType } from '../enums/common';
import { Config } from '../models/config';
import { ABP } from '../models/common';
import { ConfigState } from '../states/config.state';
import { takeUntilDestroy } from '../utils/rxjs-utils';

@Component({
  selector: 'abp-dynamic-layout',
  template: `
    <ng-container *ngTemplateOutlet="layout ? componentOutlet : routerOutlet"></ng-container>
    <ng-template #routerOutlet><router-outlet></router-outlet></ng-template>
    <ng-template #componentOutlet><ng-container *ngComponentOutlet="layout"></ng-container></ng-template>
  `
})
export class DynamicLayoutComponent implements OnDestroy {
  @Select(ConfigState.getOne('requirements')) requirements$: Observable<Config.Requirements>;

  layout: Type<any>;

  constructor(private router: Router, private route: ActivatedRoute, private store: Store) {
    const {
      requirements: { layouts },
      routes
    } = this.store.selectSnapshot(ConfigState.getAll);

    if ((this.route.snapshot.data || {}).layout) {
      this.layout = layouts
        .filter(l => !!l)
        .find((l: any) => snq(() => l.type.toLowerCase().indexOf(this.route.snapshot.data.layout), -1) > -1);
    }

    this.router.events.pipe(takeUntilDestroy(this)).subscribe(event => {
      if (event instanceof NavigationEnd) {
        const { segments } = this.router.parseUrl(event.url).root.children.primary;

        const layout = (this.route.snapshot.data || {}).layout || findLayout(segments, routes);

        this.layout = layouts
          .filter(l => !!l)
          .find((l: any) => snq(() => l.type.toLowerCase().indexOf(layout), -1) > -1);
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

    if (route.children && route.children.length && segments.length > 1) {
      const child = route.children.find(c => c.path === segments[1].path);

      if (child && child.layout) {
        layout = child.layout;
      }
    }
  }

  return layout;
}
