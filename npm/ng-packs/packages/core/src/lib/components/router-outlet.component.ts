import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'abp-router-outlet',
  template: ` <router-outlet></router-outlet> `,
  imports: [RouterModule],
})
export class RouterOutletComponent {}
