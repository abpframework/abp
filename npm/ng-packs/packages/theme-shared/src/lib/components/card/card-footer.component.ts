import {Component, HostBinding, input, ChangeDetectionStrategy,} from '@angular/core';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-card-footer',
  template: `
    <div [style]="cardFooterStyle()" [class]="cardFooterClass()">
      <ng-content></ng-content>
    </div>
  `,
  styles: [],
  imports: [],
})
export class CardFooterComponent {
  @HostBinding('class') componentClass = 'card-footer';
  readonly cardFooterStyle = input<string>(undefined);
  readonly cardFooterClass = input<string>(undefined);
}
