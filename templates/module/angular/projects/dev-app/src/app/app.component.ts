import { Component } from '@angular/core';
import {LoaderBarComponent} from "@abp/ng.theme.shared";
import {DynamicLayoutComponent} from "@abp/ng.core";

@Component({
  selector: 'app-root',
  template: `
    <abp-loader-bar />
    <abp-dynamic-layout />,
  `,
  imports: [LoaderBarComponent, DynamicLayoutComponent]
})
export class AppComponent {}
