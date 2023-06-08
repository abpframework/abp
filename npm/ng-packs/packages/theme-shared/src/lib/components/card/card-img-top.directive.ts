import { Directive, HostBinding } from '@angular/core';

@Directive({
  selector: `abp-card-img-top, [abp-card-img-top], [abpCardImgTop]`,
})
export class CardImgTopDirective {
  @HostBinding('class') directiveClass = 'card-img-top';
}
