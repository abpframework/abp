import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { ConfigState, ABP } from '@abp/ng.core';

@Component({
  selector: 'abp-breadcrumb',
  template: `
    <ol *ngIf="show" class="breadcrumb">
      <li class="breadcrumb-item">
        <a routerLink="/"><i class="fa fa-home"></i> </a>
      </li>
      <li
        *ngFor="let segment of segments; let last = last"
        class="breadcrumb-item"
        [class.active]="last"
        aria-current="page"
      >
        {{ segment | abpLocalization }}
      </li>
    </ol>
  `,
})
export class BreadcrumbComponent implements OnInit {
  show: boolean;

  segments: string[] = [];

  constructor(private router: Router, private store: Store) {
    this.show = !!this.store.selectSnapshot(state => state.LeptonLayoutState);
  }

  ngOnInit(): void {
    const splittedUrl = this.router.url.split('/').filter(chunk => chunk);

    const currentUrl: ABP.FullRoute = this.store.selectSnapshot(ConfigState.getRoute(splittedUrl[0]));
    this.segments.push(currentUrl.name);

    if (splittedUrl.length > 1) {
      const [, ...arr] = splittedUrl;

      let childRoute: ABP.FullRoute = currentUrl;
      for (let i = 0; i < arr.length; i++) {
        const element = arr[i];
        childRoute = childRoute.children.find(child => child.path === element);

        this.segments.push(childRoute.name);
      }
    }
  }
}
