import { Component } from '@angular/core';
import { eLayoutType } from '@abp/ng.core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'abp-layout-empty',
  template: ` <router-outlet></router-outlet> `,
  imports: [RouterModule],
})
export class EmptyLayoutComponent {
  static type = eLayoutType.empty;
}
