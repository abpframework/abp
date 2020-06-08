import { ListService } from '@abp/ng.core';
import {
  ChangeDetectorRef,
  Directive,
  Host,
  HostBinding,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { ColumnMode, DatatableComponent } from '@swimlane/ngx-datatable';
import { Subscription } from 'rxjs';

@Directive({
  exportAs: 'abpList',
  selector: 'ngx-datatable[abpList]',
})
export class NgxDatatableListDirective implements OnChanges, OnDestroy, OnInit {
  private subscription = new Subscription();

  /* tslint:disable-next-line:no-input-rename */
  @Input('abpList') list: ListService;

  @Input() class = 'material bordered';

  @HostBinding('class')
  get classes(): string {
    return `ngx-datatable ${this.class}`;
  }

  constructor(@Host() private cdRef: ChangeDetectorRef, @Host() private table: DatatableComponent) {
    this.table.columnMode = ColumnMode.force;
    this.table.externalPaging = true;
    this.table.externalSorting = true;
    this.table.footerHeight = 50;
    this.table.headerHeight = 50;
    this.table.rowHeight = 'auto';
  }

  ngOnChanges({ list }: SimpleChanges) {
    if (!(list && list.firstChange)) return;

    const { maxResultCount, page } = list.currentValue;
    this.table.limit = maxResultCount;
    this.table.offset = page;
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  ngOnInit() {
    const subPage = this.table.page.subscribe(({ offset }) => {
      this.list.page = offset;
      this.table.offset = offset;
    });
    this.subscription.add(subPage);

    const subSort = this.table.sort.subscribe(({ sorts: [{ prop, dir }] }) => {
      this.list.sortKey = prop;
      this.list.sortOrder = dir;
    });
    this.subscription.add(subSort);

    const subIsLoading = this.list.isLoading$.subscribe(loading => {
      this.table.loadingIndicator = loading;
      this.cdRef.markForCheck();
    });
    this.subscription.add(subIsLoading);
  }
}
