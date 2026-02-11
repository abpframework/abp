import { Component, HostBinding, Input } from '@angular/core';

@Component({
  selector: 'abp-card-footer',
  template: `
    <div [style]="cardFooterStyle" [class]="cardFooterClass">
      <ng-content></ng-content>
    </div>
  `,
  styles: [],
  imports: [],
})
export class CardFooterComponent {
  @HostBinding('class') componentClass = 'card-footer';
  @Input() cardFooterStyle: string;
  @Input() cardFooterClass: string;
}
