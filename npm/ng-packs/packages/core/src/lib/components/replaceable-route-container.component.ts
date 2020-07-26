import { Component, OnInit, Type } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngxs/store';
import { distinctUntilChanged } from 'rxjs/operators';
import { ReplaceableComponents } from '../models/replaceable-components';
import { SubscriptionService } from '../services/subscription.service';
import { ReplaceableComponentsState } from '../states/replaceable-components.state';

@Component({
  selector: 'abp-replaceable-route-container',
  template: `
    <ng-container *ngComponentOutlet="externalComponent || defaultComponent"></ng-container>
  `,
  providers: [SubscriptionService],
})
export class ReplaceableRouteContainerComponent implements OnInit {
  defaultComponent: Type<any>;

  componentKey: string;

  externalComponent: Type<any>;

  constructor(
    private route: ActivatedRoute,
    private store: Store,
    private subscription: SubscriptionService,
  ) {}

  ngOnInit() {
    this.defaultComponent = this.route.snapshot.data.replaceableComponent.defaultComponent;
    this.componentKey = (this.route.snapshot.data
      .replaceableComponent as ReplaceableComponents.RouteData).key;

    const component$ = this.store
      .select(ReplaceableComponentsState.getComponent(this.componentKey))
      .pipe(distinctUntilChanged());

    this.subscription.addOne(
      component$,
      (res = {} as ReplaceableComponents.ReplaceableComponent) => {
        this.externalComponent = res.component;
      },
    );
  }
}
