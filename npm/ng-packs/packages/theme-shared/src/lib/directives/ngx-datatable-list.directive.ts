import { ListService } from '@abp/ng.core';
import {
  ChangeDetectorRef,
  Directive,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { DatatableComponent } from '@swimlane/ngx-datatable';
import { Subscription } from 'rxjs';

@Directive({
  // tslint:disable-next-line
  selector: 'ngx-datatable[list]',
  exportAs: 'ngxDatatableList',
})
export class NgxDatatableListDirective implements OnChanges, OnDestroy, OnInit {
  private subscription = new Subscription();

  @Input() list: ListService;

  constructor(private table: DatatableComponent, private cdRef: ChangeDetectorRef) {
    this.table.externalPaging = true;
    this.table.externalSorting = true;
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
