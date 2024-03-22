import { ListService, LocalizationService } from '@abp/ng.core';
import {
  ChangeDetectorRef,
  Directive,
  Inject,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Optional,
  DoCheck,
  SimpleChanges,
} from '@angular/core';
import { DatatableComponent } from '@swimlane/ngx-datatable';
import { Subscription } from 'rxjs';
import {
  defaultNgxDatatableMessages,
  NgxDatatableMessages,
  NGX_DATATABLE_MESSAGES,
} from '../tokens/ngx-datatable-messages.token';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: 'ngx-datatable[list]',
  standalone: true,
  exportAs: 'ngxDatatableList',
})
export class NgxDatatableListDirective implements OnChanges, OnDestroy, OnInit, DoCheck {
  private subscription = new Subscription();
  private querySubscription = new Subscription();

  @Input() list!: ListService;

  constructor(
    private table: DatatableComponent,
    private cdRef: ChangeDetectorRef,
    private localizationService: LocalizationService,
    @Optional() @Inject(NGX_DATATABLE_MESSAGES) private ngxDatatableMessages: NgxDatatableMessages,
  ) {
    this.setInitialValues();
  }

  ngDoCheck(): void {
    this.refreshPageIfDataExist();
  }

  ngOnInit() {
    this.subscribeToPage();
    this.subscribeToSort();
  }

  ngOnChanges({ list }: SimpleChanges) {
    this.subscribeToQuery();

    if (!list.firstChange) return;

    const { maxResultCount, page } = list.currentValue;
    this.table.limit = maxResultCount;
    this.table.offset = page;
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
    this.querySubscription.unsubscribe();
  }

  private setInitialValues() {
    this.table.externalPaging = true;
    this.table.externalSorting = true;

    const { emptyMessage, selectedMessage, totalMessage } =
      this.ngxDatatableMessages || defaultNgxDatatableMessages;

    this.table.messages = {
      emptyMessage: this.localizationService.instant(emptyMessage),
      totalMessage: this.localizationService.instant(totalMessage),
      selectedMessage: this.localizationService.instant(selectedMessage),
    };
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

  private subscribeToPage() {
    const sub = this.table.page.subscribe(({ offset }) => {
      this.setTablePage(offset);
    });
    this.subscription.add(sub);
  }

  private subscribeToQuery() {
    if (!this.querySubscription.closed) this.querySubscription.unsubscribe();

    this.querySubscription = this.list.query$.subscribe(() => {
      const offset = this.list.page;
      if (this.table.offset !== offset) this.table.offset = offset;
    });
  }

  private setTablePage(pageNum: number) {
    this.list.page = pageNum;
    this.table.offset = pageNum;
  }

  private refreshPageIfDataExist() {
    if (this.table.rows.length < 1 && this.list.totalCount > 0) {
      let maxPage = Math.floor(Number(this.list.totalCount / this.list.maxResultCount));

      if (this.list.totalCount < this.list.maxResultCount) {
        this.setTablePage(0);
        return;
      }

      if (this.list.totalCount % this.list.maxResultCount === 0) {
        maxPage -= 1;
      }

      if (this.list.page < maxPage) {
        this.setTablePage(this.list.page);
        return;
      }

      this.setTablePage(maxPage);
    }
  }
}
