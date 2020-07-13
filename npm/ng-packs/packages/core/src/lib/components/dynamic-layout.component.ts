import { Component, Injector, OnDestroy, Optional, SkipSelf, Type } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { eLayoutType } from '../enums/common';
import { ABP } from '../models';
import { ReplaceableComponents } from '../models/replaceable-components';
import { LocalizationService } from '../services/localization.service';
import { RoutesService } from '../services/routes.service';
import { ReplaceableComponentsState } from '../states/replaceable-components.state';
import { findRoute, getRoutePath } from '../utils/route-utils';
import { takeUntilDestroy } from '../utils/rxjs-utils';
import { TreeNode } from '../utils/tree-utils';
import { distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'abp-dynamic-layout',
  template: `
    <ng-container *ngTemplateOutlet="layout ? componentOutlet : routerOutlet"></ng-container>
    <ng-template #routerOutlet><router-outlet></router-outlet></ng-template>
    <ng-template #componentOutlet
      ><ng-container *ngIf="isLayoutVisible" [ngComponentOutlet]="layout"></ng-container
    ></ng-template>
  `,
})
export class DynamicLayoutComponent implements OnDestroy {
  layout: Type<any>;

  isLayoutVisible = true;

  constructor(
    injector: Injector,
    private localizationService: LocalizationService,
    private store: Store,
    @Optional() @SkipSelf() dynamicLayoutComponent: DynamicLayoutComponent,
  ) {
    if (dynamicLayoutComponent) return;
    const route = injector.get(ActivatedRoute);
    const router = injector.get(Router);
    const routes = injector.get(RoutesService);

    const layoutKeys = {
      application: 'Theme.ApplicationLayoutComponent',
      account: 'Theme.AccountLayoutComponent',
      empty: 'Theme.EmptyLayoutComponent'
    };

    const layouts = {
      application: this.getComponent('Theme.ApplicationLayoutComponent'),
      account: this.getComponent('Theme.AccountLayoutComponent'),
      empty: this.getComponent('Theme.EmptyLayoutComponent'),
    };

    let expectedLayout: string;

    for (const key of Object.keys(layoutKeys)) {
      this.store
        .select(ReplaceableComponentsState.getComponent(layoutKeys[key]))
        .pipe(takeUntilDestroy(this), distinctUntilChanged())
        .subscribe((res = {} as ReplaceableComponents.ReplaceableComponent) => {
          (layouts[key] || {}).component = res.component;

          if (expectedLayout === key) {
            this.layout = (layouts[expectedLayout] || {}).component;
          }
        });
    }

    router.events.pipe(takeUntilDestroy(this)).subscribe(event => {
      if (event instanceof NavigationEnd) {
        let newExpectedLayout = (route.snapshot.data || {}).layout;

        if (!newExpectedLayout) {
          let node = findRoute(routes, getRoutePath(router));
          node = { parent: node } as TreeNode<ABP.Route>;

          while (node.parent) {
            node = node.parent;

            if (node.layout) {
              newExpectedLayout = node.layout;
              break;
            }
          }
        }

        if (!newExpectedLayout) newExpectedLayout = eLayoutType.empty;

        expectedLayout = newExpectedLayout;

        this.layout = layouts[expectedLayout].component;
      }
    });

    this.listenToLanguageChange();
  }

  private listenToLanguageChange() {
    this.localizationService.languageChange.pipe(takeUntilDestroy(this)).subscribe(() => {
      this.isLayoutVisible = false;
      setTimeout(() => (this.isLayoutVisible = true), 0);
    });
  }

  private getComponent(key: string): ReplaceableComponents.ReplaceableComponent {
    return this.store.selectSnapshot(ReplaceableComponentsState.getComponent(key));
  }

  ngOnDestroy() {}
}
