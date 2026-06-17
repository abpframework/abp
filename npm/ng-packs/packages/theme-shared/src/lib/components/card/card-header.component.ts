import {Component, HostBinding, input, ChangeDetectionStrategy,} from '@angular/core';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-card-header',
  template: `
    <div [class]="cardHeaderClass()" [style]="cardHeaderStyle()">
      <ng-content></ng-content>
    </div>
  `,
  styles: [],
  imports: [],
})
export class CardHeaderComponent {
  @HostBinding('class') componentClass = 'card-header';
  readonly cardHeaderClass = input<string>(undefined);
  readonly cardHeaderStyle = input<string>(undefined);
}
