import { Component, OnInit, Type, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { distinctUntilChanged } from 'rxjs/operators';
import { ReplaceableComponents } from '../models/replaceable-components';
import { ReplaceableComponentsService } from '../services/replaceable-components.service';
import { SubscriptionService } from '../services/subscription.service';
import { NgComponentOutlet } from '@angular/common';

@Component({
  selector: 'abp-replaceable-route-container',
  template: `
    <ng-container *ngComponentOutlet="externalComponent || defaultComponent"></ng-container>
  `,
  providers: [SubscriptionService],
  imports: [NgComponentOutlet],
})
export class ReplaceableRouteContainerComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private replaceableComponents = inject(ReplaceableComponentsService);
  private subscription = inject(SubscriptionService);

  defaultComponent!: Type<any>;

  componentKey!: string;

  externalComponent?: Type<any>;

  ngOnInit() {
    this.defaultComponent = this.route.snapshot.data.replaceableComponent.defaultComponent;
    this.componentKey = (
      this.route.snapshot.data.replaceableComponent as ReplaceableComponents.RouteData
    ).key;

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
