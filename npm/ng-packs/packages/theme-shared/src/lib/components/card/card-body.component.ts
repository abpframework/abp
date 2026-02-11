import { Component, HostBinding, Input } from '@angular/core';

@Component({
  selector: 'abp-card-body',
  template: ` <div [class]="cardBodyClass" [style]="cardBodyStyle">
    <ng-content></ng-content>
  </div>`,
})
export class CardBodyComponent {
  @HostBinding('class') componentClass = 'card-body';
  @Input() cardBodyClass: string;
  @Input() cardBodyStyle: string;
}
