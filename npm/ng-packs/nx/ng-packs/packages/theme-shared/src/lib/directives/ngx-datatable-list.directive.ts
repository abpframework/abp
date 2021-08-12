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
    @Optional() @Inject(NGX_DATATABLE_MESSAGES) private ngxDatatableMessages: NgxDatatableMessages,
  ) {
    this.setInitialValues();
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
    if (!this.querySubscription.closed) this.querySubscription.unsubscribe();

    this.querySubscription = this.list.query$.subscribe(() => {
      const offset = this.list.page;
      if (this.table.offset !== offset) this.table.offset = offset;
    });
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

  ngOnInit() {
    this.subscribeToPage();
    this.subscribeToSort();
  }
}
