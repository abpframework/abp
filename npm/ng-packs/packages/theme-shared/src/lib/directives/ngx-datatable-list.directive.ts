import {
  ChangeDetectorRef,
  Directive,
  OnChanges,
  OnInit,
  DoCheck,
  SimpleChanges,
  inject,
  DestroyRef,
  ViewContainerRef,
  Renderer2,
  input,
} from '@angular/core';
import { outputToObservable, takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { distinctUntilChanged } from 'rxjs';
import { DatatableComponent } from '@swimlane/ngx-datatable';
import { ListService, LocalizationService } from '@abp/ng.core';
import {
  defaultNgxDatatableMessages,
  NGX_DATATABLE_MESSAGES,
} from '../tokens/ngx-datatable-messages.token';
import { SpinnerComponent } from '../components';
import { setNgxDatatableInput } from './set-ngx-datatable-input';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: 'ngx-datatable[list]',
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

  readonly list = input.required<ListService>();

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
    setNgxDatatableInput(this.table.limit, maxResultCount);
    this.table.offset.set(page);
  }

  protected subscribeToRequestStatus() {
    const requestStatus$ = this.list().requestStatus$.pipe(distinctUntilChanged());
    const { emptyMessage, errorMessage } = this.ngxDatatableMessages || defaultNgxDatatableMessages;

    requestStatus$.subscribe(status => {
      setNgxDatatableInput(this.table.loadingIndicator, false);

      if (status === 'idle') {
        return;
      }

      if (status === 'loading') {
        this.setEmptyMessage(undefined);
        setNgxDatatableInput(this.table.loadingIndicator, true);
        this.cdRef.detectChanges();
        this.updateLoadingIndicator();
        return;
      }

      if (status === 'error') {
        this.setEmptyMessage(this.localizationService.instant(errorMessage));
        this.viewContainerRef.clear();
        this.cdRef.markForCheck();
      }

      if (status === 'success') {
        this.setEmptyMessage(this.localizationService.instant(emptyMessage));
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

    this.renderer.insertBefore(parent, spinnerElement, parent.firstChild);

    const placeholderParent = placeholder?.parentNode as Element | null;
    if (placeholderParent) {
      this.renderer.removeChild(placeholderParent, placeholder);
    }
  }

  protected setInitialValues() {
    setNgxDatatableInput(this.table.externalPaging, true);
    setNgxDatatableInput(this.table.externalSorting, true);

    const { selectedMessage, totalMessage } =
      this.ngxDatatableMessages || defaultNgxDatatableMessages;

    setNgxDatatableInput(this.table.messages, {
      totalMessage: this.localizationService.instant(totalMessage),
      selectedMessage: this.localizationService.instant(selectedMessage),
    });
  }

  protected subscribeToSort() {
    outputToObservable(this.table.sorts)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(sorts => {
        if (!sorts.length) {
          return;
        }

        const { prop, dir } = sorts[0];
        const list = this.list();
        if (prop === list.sortKey && list.sortOrder === 'desc') {
          list.sortKey = '';
          list.sortOrder = '';
          this.table.sorts.set([]);
          this.cdRef.detectChanges();
          return;
        }

        if (list.sortKey !== prop || list.sortOrder !== dir) {
          list.sortKey = prop;
          list.sortOrder = dir;
        }
      });
  }

  protected subscribeToPage() {
    outputToObservable(this.table.page)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(({ offset }) => {
        this.setTablePage(offset);
      });
  }

  protected subscribeToQuery() {
    this.list()
      .query$.pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => {
        const offset = this.list().page;
        if (this.table.offset() !== offset) {
          this.table.offset.set(offset);
        }
      });
  }

  protected setTablePage(pageNum: number) {
    this.list().page = pageNum;
    this.table.offset.set(pageNum);
  }

  protected refreshPageIfDataExist() {
    const rows = this.table.rows() ?? [];
    const count = Number(this.table.count());

    if (rows.length < 1 && count > 0) {
      const maxResultCount = this.list().maxResultCount;
      let maxPage = Math.floor(count / maxResultCount);

      if (count < maxResultCount) {
        this.setTablePage(0);
        return;
      }

      if (count % maxResultCount === 0) {
        maxPage -= 1;
      }

      if (this.list().page < maxPage) {
        this.setTablePage(this.list().page);
        return;
      }

      this.setTablePage(maxPage);
    }
  }

  private setEmptyMessage(emptyMessage: string | undefined) {
    setNgxDatatableInput(this.table.messages, {
      ...this.table.messages(),
      emptyMessage,
    });
  }
}
