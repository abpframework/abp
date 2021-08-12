import { Component, OnInit, Type } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { distinctUntilChanged } from 'rxjs/operators';
import { ReplaceableComponents } from '../models/replaceable-components';
import { ReplaceableComponentsService } from '../services/replaceable-components.service';
import { SubscriptionService } from '../services/subscription.service';

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
    private replaceableComponents: ReplaceableComponentsService,
    private subscription: SubscriptionService,
  ) {}

  ngOnInit() {
    this.defaultComponent = this.route.snapshot.data.replaceableComponent.defaultComponent;
    this.componentKey = (this.route.snapshot.data
      .replaceableComponent as ReplaceableComponents.RouteData).key;

    const component$ = this.replaceableComponents
      .get$(this.componentKey)
      .pipe(distinctUntilChanged());

    this.subscription.addOne(
      component$,
      (res = {} as ReplaceableComponents.ReplaceableComponent) => {
        this.externalComponent = res.component;
      },
    );
  }
}
