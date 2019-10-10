import { LazyLoadService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';

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
    this.lazyLoadService
      .load(
        ['primeng.min.css', 'primeicons.css', 'primeng-nova-light-theme.css', 'font-awesome.min.css'],
        'style',
        null,
        'head',
      )
      .subscribe();
  }
}
