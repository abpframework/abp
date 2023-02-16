import { Component } from '@angular/core';

@Component({
  selector: 'abp-card-body',
  template: `<div class="card-body">
    <ng-content></ng-content>
  </div>`,
})
export class CardBodyComponent {}
