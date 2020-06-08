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
import { DatatableComponent } from '@swimlane/ngx-datatable';
import { Subscription } from 'rxjs';

@Directive({
  exportAs: 'abpList',
  selector: 'ngx-datatable[abpList]',
})
export class NgxDatatableListDirective implements OnChanges, OnDestroy, OnInit {
  private subscription = new Subscription();

  /* tslint:disable-next-line:no-input-rename */
  @Input('abpList') list: ListService;
  @Input() columnMode: 'standard' | 'flex' | 'force' = 'force';
  @Input() externalPaging = true;
  @Input() externalSorting = true;
  @Input() footerHeight = 50;
  @Input() headerHeight = 50;
  @Input() rowHeight: Function | number | 'auto' | undefined = 'auto';

  @HostBinding() @Input() class = 'ngx-datatable material bordered';

  constructor(
    @Host() private cdRef: ChangeDetectorRef,
    @Host() private table: DatatableComponent,
  ) {}

  ngOnChanges({ list, ...changes }: SimpleChanges) {
    if (!(list && list.firstChange)) return;

    ['columnMode', 'externalPaging', 'externalSorting', 'footerHeight', 'headerHeight', 'rowHeight']
      .filter(key => Object.keys(changes).indexOf(key) < 0)
      .forEach(key => (this.table[key] = this[key]));

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
