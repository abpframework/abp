import { Component } from '@angular/core';
import { eLayoutType } from '@abp/ng.core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'abp-layout-empty',
  template: ` <router-outlet></router-outlet> `,
  imports: [RouterOutlet],
})
export class EmptyLayoutComponent {
  static type = eLayoutType.empty;
}
