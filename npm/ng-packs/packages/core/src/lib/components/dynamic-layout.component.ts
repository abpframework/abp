import { Component, Injector, Optional, SkipSelf, Type } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { eLayoutType } from '../enums/common';
import { ABP } from '../models';
import { ReplaceableComponents } from '../models/replaceable-components';
import { LocalizationService } from '../services/localization.service';
import { ReplaceableComponentsService } from '../services/replaceable-components.service';
import { RoutesService } from '../services/routes.service';
import { SubscriptionService } from '../services/subscription.service';
import { findRoute, getRoutePath } from '../utils/route-utils';
import { TreeNode } from '../utils/tree-utils';

@Component({
  selector: 'abp-dynamic-layout',
  template: `
    <ng-container *ngTemplateOutlet="layout ? componentOutlet : routerOutlet"></ng-container>
    <ng-template #routerOutlet><router-outlet></router-outlet></ng-template>
    <ng-template #componentOutlet
      ><ng-container *ngIf="isLayoutVisible" [ngComponentOutlet]="layout"></ng-container
    ></ng-template>
  `,
  providers: [SubscriptionService],
})
export class DynamicLayoutComponent {
  layout: Type<any>;

  // TODO: Consider a shared enum (eThemeSharedComponents) for known layouts
  readonly layouts = new Map([
    ['application', 'Theme.ApplicationLayoutComponent'],
    ['account', 'Theme.AccountLayoutComponent'],
    ['empty', 'Theme.EmptyLayoutComponent'],
  ]);

  isLayoutVisible = true;

  constructor(
    injector: Injector,
    private localizationService: LocalizationService,
    private replaceableComponents: ReplaceableComponentsService,
    private subscription: SubscriptionService,
    @Optional() @SkipSelf() dynamicLayoutComponent: DynamicLayoutComponent,
  ) {
    if (dynamicLayoutComponent) return;
    const route = injector.get(ActivatedRoute);
    const router = injector.get(Router);
    const routes = injector.get(RoutesService);

    this.subscription.addOne(router.events, event => {
      if (event instanceof NavigationEnd) {
        let expectedLayout = (route.snapshot.data || {}).layout;

        if (!expectedLayout) {
          let node = findRoute(routes, getRoutePath(router));
          node = { parent: node } as TreeNode<ABP.Route>;

          while (node.parent) {
            node = node.parent;

            if (node.layout) {
              expectedLayout = node.layout;
              break;
            }
          }
        }

        if (!expectedLayout) expectedLayout = eLayoutType.empty;

        const key = this.layouts.get(expectedLayout);
        this.layout = this.getComponent(key)?.component;
      }
    });

    this.listenToLanguageChange();
  }

  private listenToLanguageChange() {
    this.subscription.addOne(this.localizationService.languageChange$, () => {
      this.isLayoutVisible = false;
      setTimeout(() => (this.isLayoutVisible = true), 0);
    });
  }

  private getComponent(key: string): ReplaceableComponents.ReplaceableComponent {
    return this.replaceableComponents.get(key);
  }
}
