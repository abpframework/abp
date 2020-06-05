import { Component, OnDestroy, OnInit, Type } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngxs/store';
import { distinctUntilChanged } from 'rxjs/operators';
import { ABP } from '../models/common';
import { ReplaceableComponents } from '../models/replaceable-components';
import { ReplaceableComponentsState } from '../states/replaceable-components.state';
import { takeUntilDestroy } from '../utils/rxjs-utils';

@Component({
  selector: 'abp-replaceable-route-container',
  template: `
    <ng-container *ngComponentOutlet="externalComponent || defaultComponent"></ng-container>
  `,
})
export class ReplaceableRouteContainerComponent implements OnInit, OnDestroy {
  defaultComponent: Type<any>;

  componentKey: string;

  externalComponent: Type<any>;

  constructor(private route: ActivatedRoute, private store: Store) {}

  ngOnInit() {
    this.defaultComponent = this.route.snapshot.data.replaceableComponent.defaultComponent;
    this.componentKey = (this.route.snapshot.data
      .replaceableComponent as ReplaceableComponents.RouteData).key;

    this.store
      .select(ReplaceableComponentsState.getComponent(this.componentKey))
      .pipe(takeUntilDestroy(this), distinctUntilChanged())
      .subscribe((res = {} as ReplaceableComponents.ReplaceableComponent) => {
        this.externalComponent = res.component;
      });
  }

  ngOnDestroy() {}
}
