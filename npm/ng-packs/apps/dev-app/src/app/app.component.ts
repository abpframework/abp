import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <abp-loader-bar></abp-loader-bar>
    <abp-dynamic-layout></abp-dynamic-layout>
  `,
})
export class AppComponent {
  constructor() {
    // Hello
    if (false) {
      console.log('hi');
      console.log('hi 2');
    }
  }
}
