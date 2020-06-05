import { ABP, ConfigState } from '@abp/ng.core';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';

@Component({
  selector: 'abp-breadcrumb',
  templateUrl: './breadcrumb.component.html',
})
export class BreadcrumbComponent implements OnInit {
  @Input()
  segments: string[] = [];

  show: boolean;

  constructor(private router: Router, private store: Store) {}

  ngOnInit(): void {
    this.show = !!this.store.selectSnapshot(state => state.LeptonLayoutState);

    if (this.show && !this.segments.length) {
      let splittedUrl = this.router.url.split('/').filter(chunk => chunk);

      let currentUrl: ABP.FullRoute = this.store.selectSnapshot(
        ConfigState.getRoute(splittedUrl[0]),
      );

      if (!currentUrl) {
        currentUrl = this.store.selectSnapshot(ConfigState.getRoute(null, null, this.router.url));
        splittedUrl = [this.router.url];
        if (!currentUrl) {
          this.show = false;
          return;
        }
      }

      this.segments.push(currentUrl.name);

      if (splittedUrl.length > 1) {
        const [, ...arr] = splittedUrl;

        let childRoute: ABP.FullRoute = currentUrl;
        for (let i = 0; i < arr.length; i++) {
          const element = String(arr[i])
            .split(/[?#(]/)
            .shift();
          if (!childRoute.children || !childRoute.children.length) return;

          childRoute = childRoute.children.find(child => child.path === element);

          this.segments.push(childRoute.name);
        }
      }
    }
  }
}
