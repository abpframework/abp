import { Component, HostBinding, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'abp-card-body',
  template: ` <div [ngClass]="cardBodyClass" [ngStyle]="cardBodyStyle">
    <ng-content></ng-content>
  </div>`,
  imports: [CommonModule],
})
export class CardBodyComponent {
  @HostBinding('class') componentClass = 'card-body';
  @Input() cardBodyClass: string;
  @Input() cardBodyStyle: string;
}
