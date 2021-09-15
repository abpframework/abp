import { Component, Input } from '@angular/core';

@Component({
  // eslint-disable-next-line @angular-eslint/component-selector
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
  localizationResource = 'AbpAccount';

  @Input()
  localizationProp = 'NoDataAvailableInDatatable';

  get emptyMessage(): string {
    return this.message || `${this.localizationResource}::${this.localizationProp}`;
  }
}
