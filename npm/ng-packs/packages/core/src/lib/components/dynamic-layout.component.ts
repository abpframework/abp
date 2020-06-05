import { Component, OnDestroy, Type } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, UrlSegment } from '@angular/router';
import { Store } from '@ngxs/store';
import snq from 'snq';
import { eLayoutType } from '../enums/common';
import { ABP } from '../models/common';
import { ReplaceableComponents } from '../models/replaceable-components';
import { ConfigState } from '../states/config.state';
import { ReplaceableComponentsState } from '../states/replaceable-components.state';
import { takeUntilDestroy } from '../utils/rxjs-utils';

@Component({
  selector: 'abp-dynamic-layout',
  template: `
    <ng-container *ngTemplateOutlet="layout ? componentOutlet : routerOutlet"></ng-container>
    <ng-template #routerOutlet><router-outlet></router-outlet></ng-template>
    <ng-template #componentOutlet
      ><ng-container *ngComponentOutlet="layout"></ng-container
    ></ng-template>
  `,
})
export class DynamicLayoutComponent implements OnDestroy {
  layout: Type<any>;

  constructor(private router: Router, private route: ActivatedRoute, private store: Store) {
    const { routes } = this.store.selectSnapshot(ConfigState.getAll);

    router.events.pipe(takeUntilDestroy(this)).subscribe(event => {
      if (event instanceof NavigationEnd) {
        const segments = snq(() => router.parseUrl(event.url).root.children.primary.segments, [
          { path: router.url.replace('/', '') },
        ] as any);

        const layouts = {
          application: this.getComponent('Theme.ApplicationLayoutComponent'),
          account: this.getComponent('Theme.AccountLayoutComponent'),
          empty: this.getComponent('Theme.EmptyLayoutComponent'),
        };

        const expectedLayout =
          (this.route.snapshot.data || {}).layout || findLayout(segments, routes);

        this.layout = layouts[expectedLayout].component;
      }
    });
  }

  private getComponent(key: string): ReplaceableComponents.ReplaceableComponent {
    return this.store.selectSnapshot(ReplaceableComponentsState.getComponent(key));
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
