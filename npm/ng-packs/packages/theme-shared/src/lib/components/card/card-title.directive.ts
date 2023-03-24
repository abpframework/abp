import { Directive } from "@angular/core";

@Directive({
  selector: `abp-card-title, [abp-card-title], [abpCardTitle]`,
  host: {
    class: 'card-title',
  },
})
export class CardTitle {}