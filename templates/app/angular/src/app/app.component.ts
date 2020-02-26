import { LazyLoadService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <abp-loader-bar></abp-loader-bar>
    <router-outlet></router-outlet>
  `
})
export class AppComponent implements OnInit {
  constructor(private lazyLoadService: LazyLoadService) {}
  async ngOnInit() {
    this.lazyLoadService
      .load(
        ['fontawesome-all.min.css', 'fontawesome-v4-shims.min.css'],
        'style',
        null,
        'head',
        'afterbegin'
      )
      .subscribe();
  }
}
