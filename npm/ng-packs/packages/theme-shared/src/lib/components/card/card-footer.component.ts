import { Component, HostBinding, Input } from '@angular/core';

@Component({
  selector: 'abp-card-footer',
  template: `
    <div [ngStyle]="cardFooterStyle" [ngClass]="cardFooterClass">
      <ng-content></ng-content>
    </div>
  `,
  styles: [],
})
export class CardFooterComponent {
  @HostBinding('class') componentClass = 'card-footer';
  @Input() cardFooterStyle: string;
  @Input() cardFooterClass: string;
}
