import {
  ChangeDetectorRef,
  Directive,
  Input,
  OnChanges,
  OnInit,
  DoCheck,
  SimpleChanges,
  inject,
  DestroyRef,
  ViewContainerRef,
  Renderer2,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { distinctUntilChanged } from 'rxjs';
import { DatatableComponent } from '@swimlane/ngx-datatable';
import { ListService, LocalizationService } from '@abp/ng.core';
import {
  defaultNgxDatatableMessages,
  NGX_DATATABLE_MESSAGES,
} from '../tokens/ngx-datatable-messages.token';
import { SpinnerComponent } from '../components';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: 'ngx-datatable[list]',
  standalone: true,
  exportAs: 'ngxDatatableList',
})
export class NgxDatatableListDirective implements OnChanges, OnInit, DoCheck {
  protected readonly table = inject(DatatableComponent);
  protected readonly cdRef = inject(ChangeDetectorRef);
  protected readonly destroyRef = inject(DestroyRef);
  protected readonly localizationService = inject(LocalizationService);
  protected readonly ngxDatatableMessages = inject(NGX_DATATABLE_MESSAGES, { optional: true });
  protected readonly viewContainerRef = inject(ViewContainerRef);
  protected readonly renderer = inject(Renderer2);

  @Input() list!: ListService;

  constructor() {
    this.setInitialValues();
  }

  ngDoCheck(): void {
    this.refreshPageIfDataExist();
  }

  ngOnInit() {
    this.subscribeToPage();
    this.subscribeToSort();
    this.subscribeToRequestStatus();
  }

  ngOnChanges({ list }: SimpleChanges) {
    this.subscribeToQuery();

    if (!list.firstChange) return;

    const { maxResultCount, page } = list.currentValue;
    this.table.limit = maxResultCount;
    this.table.offset = page;
  }

  protected subscribeToRequestStatus() {
    const requestStatus$ = this.list.requestStatus$.pipe(distinctUntilChanged());
    const { emptyMessage, errorMessage } = this.ngxDatatableMessages || defaultNgxDatatableMessages;

    requestStatus$.subscribe(status => {
      this.table.loadingIndicator = false;

      if (status === 'idle') {
        return;
      }

      if (status === 'loading') {
        this.table.messages.emptyMessage = undefined;
        this.table.loadingIndicator = true;
        this.cdRef.detectChanges();
        this.updateLoadingIndicator();
        return;
      }

      if (status === 'error') {
        this.table.messages.emptyMessage = this.localizationService.instant(errorMessage);
        this.viewContainerRef.clear();
        this.cdRef.markForCheck();
      }

      if (status === 'success') {
        this.table.messages.emptyMessage = this.localizationService.instant(emptyMessage);
        this.viewContainerRef.clear();
      }
    });
  }

  protected updateLoadingIndicator() {
    const body = this.table.element.querySelector('datatable-body');
    const progress = this.table.element.querySelector('datatable-progress');

    if (!body) {
      return;
    }

    if (progress) {
      this.replaceLoadingIndicator(body, progress);
    }
  }

  protected replaceLoadingIndicator(parent: Element, placeholder: Element) {
    this.viewContainerRef.clear();

    const spinnerRef = this.viewContainerRef.createComponent(SpinnerComponent);
    const spinnerElement = spinnerRef.location.nativeElement;

    this.renderer.insertBefore(parent, spinnerElement, placeholder);
    this.renderer.removeChild(parent, placeholder);
  }

  protected setInitialValues() {
    this.table.externalPaging = true;
    this.table.externalSorting = true;

    const { selectedMessage, totalMessage } =
      this.ngxDatatableMessages || defaultNgxDatatableMessages;

    this.table.messages = {
      totalMessage: this.localizationService.instant(totalMessage),
      selectedMessage: this.localizationService.instant(selectedMessage),
    };
  }

  protected subscribeToSort() {
    this.table.sort
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(({ sorts: [{ prop, dir }] }) => {
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
  }

  protected subscribeToPage() {
    this.table.page.pipe(takeUntilDestroyed(this.destroyRef)).subscribe(({ offset }) => {
      this.setTablePage(offset);
    });
  }

  protected subscribeToQuery() {
    this.list.query$.pipe(takeUntilDestroyed(this.destroyRef)).subscribe(() => {
      const offset = this.list.page;
      if (this.table.offset !== offset) this.table.offset = offset;
    });
  }

  protected setTablePage(pageNum: number) {
    this.list.page = pageNum;
    this.table.offset = pageNum;
  }

  protected refreshPageIfDataExist() {
    if (this.table.rows?.length < 1 && this.table.count > 0) {
      let maxPage = Math.floor(Number(this.table.count / this.list.maxResultCount));

      if (this.table.count < this.list.maxResultCount) {
        this.setTablePage(0);
        return;
      }

      if (this.table.count % this.list.maxResultCount === 0) {
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
