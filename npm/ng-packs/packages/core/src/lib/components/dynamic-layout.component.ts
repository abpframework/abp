import { Component, Injector, OnDestroy, Type } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { eLayoutType } from '../enums/common';
import { ABP } from '../models';
import { ReplaceableComponents } from '../models/replaceable-components';
import { RoutesService } from '../services/routes.service';
import { ReplaceableComponentsState } from '../states/replaceable-components.state';
import { getRoutePath } from '../utils/route-utils';
import { takeUntilDestroy } from '../utils/rxjs-utils';
import { TreeNode } from '../utils/tree-utils';

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

  constructor(
    injector: Injector,
    private route: ActivatedRoute,
    private routes: RoutesService,
    private store: Store,
  ) {
    const router = injector.get(Router);
    const layouts = {
      application: this.getComponent('Theme.ApplicationLayoutComponent'),
      account: this.getComponent('Theme.AccountLayoutComponent'),
      empty: this.getComponent('Theme.EmptyLayoutComponent'),
    };

    router.events.pipe(takeUntilDestroy(this)).subscribe(event => {
      if (event instanceof NavigationEnd) {
        let expectedLayout = (this.route.snapshot.data || {}).layout;
        const path = getRoutePath(router);

        if (!expectedLayout) {
          let node = { parent: this.routes.search({ path }) } as TreeNode<ABP.Route>;
          while (node.parent) {
            node = node.parent;

            if (node.layout) {
              expectedLayout = node.layout;
              break;
            }
          }
        }

        if (!expectedLayout) expectedLayout = eLayoutType.empty;

        this.layout = layouts[expectedLayout].component;
      }
    });
  }

  private getComponent(key: string): ReplaceableComponents.ReplaceableComponent {
    return this.store.selectSnapshot(ReplaceableComponentsState.getComponent(key));
  }

  ngOnDestroy() {}
}
