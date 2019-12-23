import { Type, Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'abp-replaceable-route-container',
  template: `
    <ng-container *ngComponentOutlet="defaultComponent"></ng-container>
  `,
})
export class ReplaceableRouteContainerComponent implements OnInit {
  defaultComponent: Type<any>;

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    this.defaultComponent = this.route.snapshot.data.component.default;
  }
}
