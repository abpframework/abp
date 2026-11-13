import { Directive, HostBinding, inject, input } from '@angular/core';
import { ColumnMode, DatatableComponent } from '@swimlane/ngx-datatable';
import { setNgxDatatableInput } from './set-ngx-datatable-input';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: 'ngx-datatable[default]',
  exportAs: 'ngxDatatableDefault',
})
export class NgxDatatableDefaultDirective {
  private table = inject(DatatableComponent);

  readonly class = input('material bordered');

  @HostBinding('class')
  get classes(): string {
    return `ngx-datatable ${this.class()}`;
  }

  constructor() {
    setNgxDatatableInput(this.table.columnMode, ColumnMode.force);
    setNgxDatatableInput(this.table.footerHeight, 50);
    setNgxDatatableInput(this.table.headerHeight, 50);
    setNgxDatatableInput(this.table.rowHeight, 'auto');
    setNgxDatatableInput(this.table.scrollbarH, true);
    setNgxDatatableInput(this.table.virtualization, false);
  }
}
