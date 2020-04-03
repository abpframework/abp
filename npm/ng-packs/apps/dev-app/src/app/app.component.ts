import { LazyLoadService, LOADING_STRATEGY } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { concat } from 'rxjs';

@Component({
  selector: 'app-root',
  template: `
    <abp-loader-bar></abp-loader-bar>
    <router-outlet></router-outlet>
  `,
})
export class AppComponent implements OnInit {
  constructor(private lazyLoadService: LazyLoadService) {}

  ngOnInit() {
    concat(
      this.lazyLoadService.load(
        LOADING_STRATEGY.PrependAnonymousStyleToHead('fontawesome-all.min.css'),
      ),
      this.lazyLoadService.load(
        LOADING_STRATEGY.PrependAnonymousStyleToHead('fontawesome-v4-shims.min.css'),
      ),
    ).subscribe();
  }
}
