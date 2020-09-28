import { ListService, LocalizationService } from '@abp/ng.core';
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
  private querySubscription = new Subscription();

  @Input() list: ListService;

  constructor(
    private table: DatatableComponent,
    private cdRef: ChangeDetectorRef,
    private localizationService: LocalizationService,
  ) {
    this.setInitialValues();
  }

  private setInitialValues() {
    this.table.externalPaging = true;
    this.table.externalSorting = true;
    this.table.messages = {
      emptyMessage: this.localizationService.localizeSync(
        'AbpUi',
        'NoDataAvailableInDatatable',
        'No data available',
      ),
      totalMessage: this.localizationService.localizeSync('AbpUi', 'Total', 'total'),
      selectedMessage: this.localizationService.localizeSync('AbpUi', 'Selected', 'selected'),
    };
  }

  private subscribeToPage() {
    const sub = this.table.page.subscribe(({ offset }) => {
      this.list.page = offset;
      this.table.offset = offset;
    });
    this.subscription.add(sub);
  }

  private subscribeToSort() {
    const sub = this.table.sort.subscribe(({ sorts: [{ prop, dir }] }) => {
      if (prop === this.list.sortKey && this.list.sortOrder === 'desc') {
        this.list.sortKey = '';
        this.list.sortOrder = '';
        this.table.sorts = [];
        this.cdRef.detectChanges();
      } else {
        this.list.sortKey = prop;
        this.list.sortOrder = dir;
      }
    });
    this.subscription.add(sub);
  }

  private subscribeToQuery() {
    this.querySubscription.add(
      this.list.query$.subscribe(() => {
        if (this.list.page !== this.table.offset) this.table.offset = this.list.page;
      }),
    );
  }

  ngOnChanges({ list }: SimpleChanges) {
    if (!list.firstChange) return;

    const { maxResultCount, page } = list.currentValue;
    this.table.limit = maxResultCount;
    this.table.offset = page;

    this.querySubscription.unsubscribe();
    this.subscribeToQuery();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
    this.querySubscription.unsubscribe();
  }

  ngOnInit() {
    this.subscribeToPage();
    this.subscribeToSort();
  }
}
