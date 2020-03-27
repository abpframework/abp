import { Component, Input, OnDestroy, Type, Injector } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, UrlSegment } from '@angular/router';
import { Select, Store } from '@ngxs/store';
import { Observable, combineLatest } from 'rxjs';
import snq from 'snq';
import { eLayoutType } from '../enums/common';
import { Config } from '../models/config';
import { ABP } from '../models/common';
import { ConfigState } from '../states/config.state';
import { takeUntilDestroy } from '../utils/rxjs-utils';
import { ReplaceableComponentsState } from '../states/replaceable-components.state';
import { ReplaceableComponents } from '../models/replaceable-components';

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
  @Select(ReplaceableComponentsState.getComponent('Theme.ApplicationLayoutComponent'))
  applicationLayout$: Observable<ReplaceableComponents.ReplaceableComponent>;

  @Select(ReplaceableComponentsState.getComponent('Theme.AccountLayoutComponent'))
  accountLayout$: Observable<ReplaceableComponents.ReplaceableComponent>;

  @Select(ReplaceableComponentsState.getComponent('Theme.EmptyLayoutComponent'))
  emptyLayout$: Observable<ReplaceableComponents.ReplaceableComponent>;

  layout: Type<any>;

  layouts = {} as { [key in eLayoutType]: Type<any> };

  expectedLayout: eLayoutType = eLayoutType.empty;

  constructor(private router: Router, private route: ActivatedRoute, private store: Store) {
    this.listenToLayouts();
    const { routes } = this.store.selectSnapshot(ConfigState.getAll);

    router.events.pipe(takeUntilDestroy(this)).subscribe(event => {
      if (event instanceof NavigationEnd) {
        const segments = snq(() => router.parseUrl(event.url).root.children.primary.segments, [
          { path: router.url.replace('/', '') },
        ] as any);

        this.expectedLayout =
          (this.route.snapshot.data || {}).layout || findLayout(segments, routes);

        this.layout = this.layouts[this.expectedLayout];
      }
    });
  }

  ngOnDestroy() {}

  listenToLayouts() {
    combineLatest(this.applicationLayout$, this.accountLayout$, this.emptyLayout$)
      .pipe(takeUntilDestroy(this))
      .subscribe(([application, account, empty]) => {
        this.layouts.application = application.component;
        this.layouts.account = account.component;
        this.layouts.empty = empty.component;
      });
  }
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
