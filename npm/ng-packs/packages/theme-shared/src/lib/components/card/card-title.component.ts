import { Component } from '@angular/core';

@Component({
  selector: 'abp-card-title',
  template: `<div class="card-title">
    <ng-content></ng-content>
  </div>`,
})
export class CardTitleComponent {}
