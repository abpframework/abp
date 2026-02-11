import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'abp-router-outlet',
  template: ` <router-outlet></router-outlet> `,
  imports: [RouterOutlet],
})
export class RouterOutletComponent {}
