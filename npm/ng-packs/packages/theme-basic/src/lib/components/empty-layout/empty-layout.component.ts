import { Component, ChangeDetectionStrategy } from '@angular/core';
import { eLayoutType } from '@abp/ng.core';
import { RouterOutlet } from '@angular/router';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-layout-empty',
  template: ` <router-outlet></router-outlet> `,
  imports: [RouterOutlet],
})
export class EmptyLayoutComponent {
  static type = eLayoutType.empty;
}
