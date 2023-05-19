import {
  Directive,
  EmbeddedViewRef,
  Input,
  IterableChangeRecord,
  IterableChanges,
  IterableDiffer,
  IterableDiffers,
  OnChanges,
  TemplateRef,
  TrackByFunction,
  ViewContainerRef,
} from '@angular/core';
import clone from 'just-clone';
import compare from 'just-compare';

export type CompareFn<T = any> = (value: T, comparison: T) => boolean;

class AbpForContext {
  constructor(
    public $implicit: any,
    public index: number,
    public count: number,
    public list: any[],
  ) {}
}

class RecordView {
  constructor(
    public record: IterableChangeRecord<any>,
    public view: EmbeddedViewRef<AbpForContext>,
  ) {}
}

@Directive({
  selector: '[abpFor]',
})
export class ForDirective implements OnChanges {
  // eslint-disable-next-line @angular-eslint/no-input-rename
  @Input('abpForOf')
  items!: any[];

  @Input('abpForOrderBy')
  orderBy?: string;

  @Input('abpForOrderDir')
  orderDir?: 'ASC' | 'DESC';

  @Input('abpForFilterBy')
  filterBy?: string;

  @Input('abpForFilterVal')
  filterVal: any;

  @Input('abpForTrackBy')
  trackBy?: TrackByFunction<any>;

  @Input('abpForCompareBy')
  compareBy?: CompareFn;

  @Input('abpForEmptyRef')
  emptyRef?: TemplateRef<any>;

  private differ!: IterableDiffer<any> | null;

  private isShowEmptyRef!: boolean;

  get compareFn(): CompareFn {
    return this.compareBy || compare;
  }

  get trackByFn(): TrackByFunction<any> {
    return this.trackBy || ((index: number, item: any) => (item as any).id || index);
  }

  constructor(
    private tempRef: TemplateRef<AbpForContext>,
    private vcRef: ViewContainerRef,
    private differs: IterableDiffers,
  ) {}

  private iterateOverAppliedOperations(changes: IterableChanges<any>) {
    const rw: RecordView[] = [];

    changes.forEachOperation(
      (
        record: IterableChangeRecord<any>,
        previousIndex: number | null,
        currentIndex: number | null,
      ) => {
        if (record.previousIndex == null) {
          const view = this.vcRef.createEmbeddedView(
            this.tempRef,
            new AbpForContext(null, -1, -1, this.items),
            currentIndex || 0,
          );

          rw.push(new RecordView(record, view));
        } else if (currentIndex == null && previousIndex !== null) {
          this.vcRef.remove(previousIndex);
        } else {
          if (previousIndex !== null) {
            const view = this.vcRef.get(previousIndex);
            if (view && currentIndex !== null) {
              this.vcRef.move(view, currentIndex);
              rw.push(new RecordView(record, view as EmbeddedViewRef<AbpForContext>));
            }
          }
        }
      },
    );

    for (let i = 0, l = rw.length; i < l; i++) {
      rw[i].view.context.$implicit = rw[i].record.item;
    }
  }

  private iterateOverAttachedViews(changes: IterableChanges<any>) {
    for (let i = 0, l = this.vcRef.length; i < l; i++) {
      const viewRef = this.vcRef.get(i) as EmbeddedViewRef<AbpForContext>;
      viewRef.context.index = i;
      viewRef.context.count = l;
      viewRef.context.list = this.items;
    }

    changes.forEachIdentityChange((record: IterableChangeRecord<any>) => {
      if (record.currentIndex !== null) {
        const viewRef = this.vcRef.get(record.currentIndex) as EmbeddedViewRef<AbpForContext>;
        viewRef.context.$implicit = record.item;
      }
    });
  }

  private projectItems(items: any[]): void {
    if (!items.length && this.emptyRef) {
      this.vcRef.clear();
      this.vcRef.createEmbeddedView(this.emptyRef).rootNodes;
      this.isShowEmptyRef = true;
      this.differ = null;

      return;
    }

    if (this.emptyRef && this.isShowEmptyRef) {
      this.vcRef.clear();
      this.isShowEmptyRef = false;
    }

    if (!this.differ && items) {
      this.differ = this.differs.find(items).create(this.trackByFn);
    }

    if (this.differ) {
      const changes = this.differ.diff(items);

      if (changes) {
        this.iterateOverAppliedOperations(changes);
        this.iterateOverAttachedViews(changes);
      }
    }
  }

  private sortItems(items: any[]) {
    const orderBy = this.orderBy;
    if (orderBy) {
      items.sort((a, b) => (a[orderBy] > b[orderBy] ? 1 : a[orderBy] < b[orderBy] ? -1 : 0));
    } else {
      items.sort();
    }
  }

  ngOnChanges() {
    let items = clone(this.items) as any[];
    if (!Array.isArray(items)) return;

    const compareFn = this.compareFn;
    const filterBy = this.filterBy;
    if (
      typeof filterBy !== 'undefined' &&
      typeof this.filterVal !== 'undefined' &&
      this.filterVal !== ''
    ) {
      items = items.filter(item => compareFn(item[filterBy], this.filterVal));
    }

    switch (this.orderDir) {
      case 'ASC':
        this.sortItems(items);
        this.projectItems(items);
        break;

      case 'DESC':
        this.sortItems(items);
        items.reverse();
        this.projectItems(items);
        break;

      default:
        this.projectItems(items);
    }
  }
}
