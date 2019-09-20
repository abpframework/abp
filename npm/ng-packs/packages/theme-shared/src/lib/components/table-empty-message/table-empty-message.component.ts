import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: '[abp-table-empty-message]',
  template: `
    <td class="text-center" [attr.colspan]="colspan">
      {{ emptyMessage | abpLocalization }}
    </td>
  `,
})
export class TableEmptyMessageComponent {
  @Input()
  colspan = 2;

  @Input()
  message: string;

  @Input()
  localizationResource: string = 'AbpAccount';

  @Input()
  localizationProp: string = 'NoDataAvailableInDatatable';

  get emptyMessage(): string {
    return this.message || `${this.localizationResource}::${this.localizationProp}`;
  }
}
