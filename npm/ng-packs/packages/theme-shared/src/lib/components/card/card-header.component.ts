import { Component, HostBinding, Input } from '@angular/core';

@Component({
  selector: 'abp-card-header',
  template: `
    <div [class]="cardHeaderClass" [style]="cardHeaderStyle">
      <ng-content></ng-content>
    </div>
  `,
  styles: [],
  imports: [],
})
export class CardHeaderComponent {
  @HostBinding('class') componentClass = 'card-header';
  @Input() cardHeaderClass: string;
  @Input() cardHeaderStyle: string;
}
