import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-router-outlet',
  template: ` <router-outlet></router-outlet> `,
  imports: [RouterOutlet],
})
export class RouterOutletComponent {}
