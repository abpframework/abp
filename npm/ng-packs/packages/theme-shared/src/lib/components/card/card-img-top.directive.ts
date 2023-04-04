import { Directive } from "@angular/core";

@Directive({
  selector: `abp-card-img-top, [abp-card-img-top], [abpCardImgTop]`,
  host: {
    class: 'card-img-top',
  },
})
export class CardImgTop {}
