import { Component, inject, isDevMode, OnInit, Optional, SkipSelf, Type } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { eLayoutType } from '../enums/common';
import { ABP } from '../models';
import { ReplaceableComponents } from '../models/replaceable-components';
import { LocalizationService } from '../services/localization.service';
import { ReplaceableComponentsService } from '../services/replaceable-components.service';
import { RouterEvents } from '../services/router-events.service';
import { RoutesService } from '../services/routes.service';
import { SubscriptionService } from '../services/subscription.service';
import { findRoute, getRoutePath } from '../utils/route-utils';
import { TreeNode } from '../utils/tree-utils';
import { DYNAMIC_LAYOUTS_TOKEN } from '../tokens/dynamic-layout.token';
import { EnvironmentService } from '../services';

@Component({
  selector: 'abp-dynamic-layout',
  template: `
    @if (isLayoutVisible) {
      <ng-container [ngComponentOutlet]="layout"></ng-container>
    }
  `,
  providers: [SubscriptionService],
})
export class DynamicLayoutComponent implements OnInit {
  layout?: Type<any>;
  layoutKey?: eLayoutType;
  readonly layouts = inject(DYNAMIC_LAYOUTS_TOKEN);
  isLayoutVisible = true;


  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly routes = inject(RoutesService);
  protected readonly localizationService = inject(LocalizationService);
  protected readonly replaceableComponents = inject(ReplaceableComponentsService);
  protected readonly subscription = inject(SubscriptionService);
  protected readonly routerEvents = inject(RouterEvents);
  protected readonly environment = inject(EnvironmentService);

  constructor(@Optional() @SkipSelf() dynamicLayoutComponent: DynamicLayoutComponent) {
    if (dynamicLayoutComponent) {
      if (isDevMode()) console.warn('DynamicLayoutComponent must be used only in AppComponent.');
      return;
    }
    this.checkLayoutOnNavigationEnd();
    this.listenToLanguageChange();
  }
  
  ngOnInit(): void {
    if (this.layout) {
      return;
    }

    const { oAuthConfig } = this.environment.getEnvironment();
    if (oAuthConfig.responseType === 'code') {
      this.getLayout();
    }
  }

  private checkLayoutOnNavigationEnd() {
    const navigationEnd$ = this.routerEvents.getNavigationEvents('End');
    this.subscription.addOne(navigationEnd$, () => this.getLayout());
  }

  private getLayout() {
    let expectedLayout = this.getExtractedLayout();

    if (!expectedLayout) expectedLayout = eLayoutType.empty;

    if (this.layoutKey === expectedLayout) return;

    const key = this.layouts.get(expectedLayout);
    if (key) {
      this.layout = this.getComponent(key)?.component;
      this.layoutKey = expectedLayout;
    }
    if (!this.layout) {
      this.showLayoutNotFoundError(expectedLayout);
    }
  }

  private getExtractedLayout() {
    const routeData = this.route.snapshot.data || {};
    let expectedLayout = routeData['layout'] as eLayoutType;

    let node = findRoute(this.routes, getRoutePath(this.router));
    node = { parent: node } as TreeNode<ABP.Route>;

    while (node.parent) {
      node = node.parent;

      if (node.layout) {
        expectedLayout = node.layout;
        break;
      }
    }
    return expectedLayout;
  }

  showLayoutNotFoundError(layoutName: string) {
    let message = `Layout ${layoutName} not found.`;
    if (layoutName === 'account') {
      message =
        'Account layout not found. Please check your configuration. If you are using LeptonX, please make sure you have added "AccountLayoutModule.forRoot()" to your app.module configuration.';
    }
    console.warn(message);
  }

  private listenToLanguageChange() {
    this.subscription.addOne(this.localizationService.languageChange$, () => {
      this.isLayoutVisible = false;
      setTimeout(() => (this.isLayoutVisible = true), 0);
    });
  }

  private getComponent(key: string): ReplaceableComponents.ReplaceableComponent | undefined {
    return this.replaceableComponents.get(key);
  }
}
