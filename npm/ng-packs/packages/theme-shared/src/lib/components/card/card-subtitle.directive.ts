import { Directive, HostBinding } from '@angular/core';

@Directive({
  selector: `abp-card-subtitle, [abp-card-subtitle], [abpCardSubtitle]`,
})
export class CardSubtitleDirective {
  @HostBinding('class') directiveClass = 'card-subtitle';
}
