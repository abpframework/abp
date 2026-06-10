import { Component, HostBinding, input } from '@angular/core';

@Component({
  selector: 'abp-card-body',
  template: ` <div [class]="cardBodyClass()" [style]="cardBodyStyle()">
    <ng-content></ng-content>
  </div>`,
})
export class CardBodyComponent {
  @HostBinding('class') componentClass = 'card-body';
  readonly cardBodyClass = input<string>(undefined);
  readonly cardBodyStyle = input<string>(undefined);
}
