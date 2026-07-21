import { Component, HostBinding, input, ChangeDetectionStrategy } from '@angular/core';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
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
