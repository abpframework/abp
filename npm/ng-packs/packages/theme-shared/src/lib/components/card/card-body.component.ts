import { Component, HostBinding, Input } from '@angular/core';

@Component({
  selector: 'abp-card-body',
  template: ` <div [ngClass]="cardBodyClass" [ngStyle]="cardBodyStyle">
    <ng-content></ng-content>
  </div>`,
})
export class CardBodyComponent {
  @HostBinding('class') class = 'card-body';
  @Input() cardBodyClass: string;
  @Input() cardBodyStyle: string;
}
