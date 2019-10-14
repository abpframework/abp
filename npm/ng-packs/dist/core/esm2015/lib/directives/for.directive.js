/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, Input, IterableDiffers, TemplateRef, ViewContainerRef } from '@angular/core';
import compare from 'just-compare';
import clone from 'just-clone';
class AbpForContext {
  /**
   * @param {?} $implicit
   * @param {?} index
   * @param {?} count
   * @param {?} list
   */
  constructor($implicit, index, count, list) {
    this.$implicit = $implicit;
    this.index = index;
    this.count = count;
    this.list = list;
  }
}
if (false) {
  /** @type {?} */
  AbpForContext.prototype.$implicit;
  /** @type {?} */
  AbpForContext.prototype.index;
  /** @type {?} */
  AbpForContext.prototype.count;
  /** @type {?} */
  AbpForContext.prototype.list;
}
class RecordView {
  /**
   * @param {?} record
   * @param {?} view
   */
  constructor(record, view) {
    this.record = record;
    this.view = view;
  }
}
if (false) {
  /** @type {?} */
  RecordView.prototype.record;
  /** @type {?} */
  RecordView.prototype.view;
}
export class ForDirective {
  /**
   * @param {?} tempRef
   * @param {?} vcRef
   * @param {?} differs
   */
  constructor(tempRef, vcRef, differs) {
    this.tempRef = tempRef;
    this.vcRef = vcRef;
    this.differs = differs;
  }
  /**
   * @return {?}
   */
  get compareFn() {
    return this.compareBy || compare;
  }
  /**
   * @return {?}
   */
  get trackByFn() {
    return (
      this.trackBy
      /**
       * @param {?} index
       * @param {?} item
       * @return {?}
       */ || ((index, item) => /** @type {?} */ (item).id || index)
    );
  }
  /**
   * @private
   * @param {?} changes
   * @return {?}
   */
  iterateOverAppliedOperations(changes) {
    /** @type {?} */
    const rw = [];
    changes.forEachOperation(
      /**
       * @param {?} record
       * @param {?} previousIndex
       * @param {?} currentIndex
       * @return {?}
       */
      (record, previousIndex, currentIndex) => {
        if (record.previousIndex == null) {
          /** @type {?} */
          const view = this.vcRef.createEmbeddedView(
            this.tempRef,
            new AbpForContext(null, -1, -1, this.items),
            currentIndex,
          );
          rw.push(new RecordView(record, view));
        } else if (currentIndex == null) {
          this.vcRef.remove(previousIndex);
        } else {
          /** @type {?} */
          const view = this.vcRef.get(previousIndex);
          this.vcRef.move(view, currentIndex);
          rw.push(new RecordView(record, /** @type {?} */ (view)));
        }
      },
    );
    for (let i = 0, l = rw.length; i < l; i++) {
      rw[i].view.context.$implicit = rw[i].record.item;
    }
  }
  /**
   * @private
   * @param {?} changes
   * @return {?}
   */
  iterateOverAttachedViews(changes) {
    for (let i = 0, l = this.vcRef.length; i < l; i++) {
      /** @type {?} */
      const viewRef = /** @type {?} */ (this.vcRef.get(i));
      viewRef.context.index = i;
      viewRef.context.count = l;
      viewRef.context.list = this.items;
    }
    changes.forEachIdentityChange(
      /**
       * @param {?} record
       * @return {?}
       */
      record => {
        /** @type {?} */
        const viewRef = /** @type {?} */ (this.vcRef.get(record.currentIndex));
        viewRef.context.$implicit = record.item;
      },
    );
  }
  /**
   * @private
   * @param {?} items
   * @return {?}
   */
  projectItems(items) {
    if (!items.length && this.emptyRef) {
      this.vcRef.clear();
      // tslint:disable-next-line: no-unused-expression
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
      /** @type {?} */
      const changes = this.differ.diff(items);
      if (changes) {
        this.iterateOverAppliedOperations(changes);
        this.iterateOverAttachedViews(changes);
      }
    }
  }
  /**
   * @private
   * @param {?} items
   * @return {?}
   */
  sortItems(items) {
    if (this.orderBy) {
      items.sort(
        /**
         * @param {?} a
         * @param {?} b
         * @return {?}
         */
        (a, b) => (a[this.orderBy] > b[this.orderBy] ? 1 : a[this.orderBy] < b[this.orderBy] ? -1 : 0),
      );
    } else {
      items.sort();
    }
  }
  /**
   * @return {?}
   */
  ngOnChanges() {
    /** @type {?} */
    let items = /** @type {?} */ (clone(this.items));
    if (!Array.isArray(items)) return;
    /** @type {?} */
    const compareFn = this.compareFn;
    if (typeof this.filterBy !== 'undefined') {
      items = items.filter(
        /**
         * @param {?} item
         * @return {?}
         */
        item => compareFn(item[this.filterBy], this.filterVal),
      );
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
ForDirective.decorators = [
  {
    type: Directive,
    args: [
      {
        selector: '[abpFor]',
      },
    ],
  },
];
/** @nocollapse */
ForDirective.ctorParameters = () => [{ type: TemplateRef }, { type: ViewContainerRef }, { type: IterableDiffers }];
ForDirective.propDecorators = {
  items: [{ type: Input, args: ['abpForOf'] }],
  orderBy: [{ type: Input, args: ['abpForOrderBy'] }],
  orderDir: [{ type: Input, args: ['abpForOrderDir'] }],
  filterBy: [{ type: Input, args: ['abpForFilterBy'] }],
  filterVal: [{ type: Input, args: ['abpForFilterVal'] }],
  trackBy: [{ type: Input, args: ['abpForTrackBy'] }],
  compareBy: [{ type: Input, args: ['abpForCompareBy'] }],
  emptyRef: [{ type: Input, args: ['abpForEmptyRef'] }],
};
if (false) {
  /** @type {?} */
  ForDirective.prototype.items;
  /** @type {?} */
  ForDirective.prototype.orderBy;
  /** @type {?} */
  ForDirective.prototype.orderDir;
  /** @type {?} */
  ForDirective.prototype.filterBy;
  /** @type {?} */
  ForDirective.prototype.filterVal;
  /** @type {?} */
  ForDirective.prototype.trackBy;
  /** @type {?} */
  ForDirective.prototype.compareBy;
  /** @type {?} */
  ForDirective.prototype.emptyRef;
  /**
   * @type {?}
   * @private
   */
  ForDirective.prototype.differ;
  /**
   * @type {?}
   * @private
   */
  ForDirective.prototype.isShowEmptyRef;
  /**
   * @type {?}
   * @private
   */
  ForDirective.prototype.tempRef;
  /**
   * @type {?}
   * @private
   */
  ForDirective.prototype.vcRef;
  /**
   * @type {?}
   * @private
   */
  ForDirective.prototype.differs;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZm9yLmRpcmVjdGl2ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9kaXJlY3RpdmVzL2Zvci5kaXJlY3RpdmUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFDTCxTQUFTLEVBRVQsS0FBSyxFQUlMLGVBQWUsRUFFZixXQUFXLEVBRVgsZ0JBQWdCLEVBQ2pCLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sT0FBTyxNQUFNLGNBQWMsQ0FBQztBQUNuQyxPQUFPLEtBQUssTUFBTSxZQUFZLENBQUM7QUFJL0IsTUFBTSxhQUFhOzs7Ozs7O0lBQ2pCLFlBQW1CLFNBQWMsRUFBUyxLQUFhLEVBQVMsS0FBYSxFQUFTLElBQVc7UUFBOUUsY0FBUyxHQUFULFNBQVMsQ0FBSztRQUFTLFVBQUssR0FBTCxLQUFLLENBQVE7UUFBUyxVQUFLLEdBQUwsS0FBSyxDQUFRO1FBQVMsU0FBSSxHQUFKLElBQUksQ0FBTztJQUFHLENBQUM7Q0FDdEc7OztJQURhLGtDQUFxQjs7SUFBRSw4QkFBb0I7O0lBQUUsOEJBQW9COztJQUFFLDZCQUFrQjs7QUFHbkcsTUFBTSxVQUFVOzs7OztJQUNkLFlBQW1CLE1BQWlDLEVBQVMsSUFBb0M7UUFBOUUsV0FBTSxHQUFOLE1BQU0sQ0FBMkI7UUFBUyxTQUFJLEdBQUosSUFBSSxDQUFnQztJQUFHLENBQUM7Q0FDdEc7OztJQURhLDRCQUF3Qzs7SUFBRSwwQkFBMkM7O0FBTW5HLE1BQU0sT0FBTyxZQUFZOzs7Ozs7SUFxQ3ZCLFlBQ1UsT0FBbUMsRUFDbkMsS0FBdUIsRUFDdkIsT0FBd0I7UUFGeEIsWUFBTyxHQUFQLE9BQU8sQ0FBNEI7UUFDbkMsVUFBSyxHQUFMLEtBQUssQ0FBa0I7UUFDdkIsWUFBTyxHQUFQLE9BQU8sQ0FBaUI7SUFDL0IsQ0FBQzs7OztJQVpKLElBQUksU0FBUztRQUNYLE9BQU8sSUFBSSxDQUFDLFNBQVMsSUFBSSxPQUFPLENBQUM7SUFDbkMsQ0FBQzs7OztJQUVELElBQUksU0FBUztRQUNYLE9BQU8sSUFBSSxDQUFDLE9BQU8sSUFBSTs7Ozs7UUFBQyxDQUFDLEtBQWEsRUFBRSxJQUFTLEVBQUUsRUFBRSxDQUFDLENBQUMsbUJBQUEsSUFBSSxFQUFPLENBQUMsQ0FBQyxFQUFFLElBQUksS0FBSyxFQUFDLENBQUM7SUFDbkYsQ0FBQzs7Ozs7O0lBUU8sNEJBQTRCLENBQUMsT0FBNkI7O2NBQzFELEVBQUUsR0FBaUIsRUFBRTtRQUUzQixPQUFPLENBQUMsZ0JBQWdCOzs7Ozs7UUFBQyxDQUFDLE1BQWlDLEVBQUUsYUFBcUIsRUFBRSxZQUFvQixFQUFFLEVBQUU7WUFDMUcsSUFBSSxNQUFNLENBQUMsYUFBYSxJQUFJLElBQUksRUFBRTs7c0JBQzFCLElBQUksR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGtCQUFrQixDQUN4QyxJQUFJLENBQUMsT0FBTyxFQUNaLElBQUksYUFBYSxDQUFDLElBQUksRUFBRSxDQUFDLENBQUMsRUFBRSxDQUFDLENBQUMsRUFBRSxJQUFJLENBQUMsS0FBSyxDQUFDLEVBQzNDLFlBQVksQ0FDYjtnQkFFRCxFQUFFLENBQUMsSUFBSSxDQUFDLElBQUksVUFBVSxDQUFDLE1BQU0sRUFBRSxJQUFJLENBQUMsQ0FBQyxDQUFDO2FBQ3ZDO2lCQUFNLElBQUksWUFBWSxJQUFJLElBQUksRUFBRTtnQkFDL0IsSUFBSSxDQUFDLEtBQUssQ0FBQyxNQUFNLENBQUMsYUFBYSxDQUFDLENBQUM7YUFDbEM7aUJBQU07O3NCQUNDLElBQUksR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQyxhQUFhLENBQUM7Z0JBQzFDLElBQUksQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLElBQUksRUFBRSxZQUFZLENBQUMsQ0FBQztnQkFFcEMsRUFBRSxDQUFDLElBQUksQ0FBQyxJQUFJLFVBQVUsQ0FBQyxNQUFNLEVBQUUsbUJBQUEsSUFBSSxFQUFrQyxDQUFDLENBQUMsQ0FBQzthQUN6RTtRQUNILENBQUMsRUFBQyxDQUFDO1FBRUgsS0FBSyxJQUFJLENBQUMsR0FBRyxDQUFDLEVBQUUsQ0FBQyxHQUFHLEVBQUUsQ0FBQyxNQUFNLEVBQUUsQ0FBQyxHQUFHLENBQUMsRUFBRSxDQUFDLEVBQUUsRUFBRTtZQUN6QyxFQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxTQUFTLEdBQUcsRUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUM7U0FDbEQ7SUFDSCxDQUFDOzs7Ozs7SUFFTyx3QkFBd0IsQ0FBQyxPQUE2QjtRQUM1RCxLQUFLLElBQUksQ0FBQyxHQUFHLENBQUMsRUFBRSxDQUFDLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxNQUFNLEVBQUUsQ0FBQyxHQUFHLENBQUMsRUFBRSxDQUFDLEVBQUUsRUFBRTs7a0JBQzNDLE9BQU8sR0FBRyxtQkFBQSxJQUFJLENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsRUFBa0M7WUFDbkUsT0FBTyxDQUFDLE9BQU8sQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDO1lBQzFCLE9BQU8sQ0FBQyxPQUFPLENBQUMsS0FBSyxHQUFHLENBQUMsQ0FBQztZQUMxQixPQUFPLENBQUMsT0FBTyxDQUFDLElBQUksR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDO1NBQ25DO1FBRUQsT0FBTyxDQUFDLHFCQUFxQjs7OztRQUFDLENBQUMsTUFBaUMsRUFBRSxFQUFFOztrQkFDNUQsT0FBTyxHQUFHLG1CQUFBLElBQUksQ0FBQyxLQUFLLENBQUMsR0FBRyxDQUFDLE1BQU0sQ0FBQyxZQUFZLENBQUMsRUFBa0M7WUFDckYsT0FBTyxDQUFDLE9BQU8sQ0FBQyxTQUFTLEdBQUcsTUFBTSxDQUFDLElBQUksQ0FBQztRQUMxQyxDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUM7Ozs7OztJQUVPLFlBQVksQ0FBQyxLQUFZO1FBQy9CLElBQUksQ0FBQyxLQUFLLENBQUMsTUFBTSxJQUFJLElBQUksQ0FBQyxRQUFRLEVBQUU7WUFDbEMsSUFBSSxDQUFDLEtBQUssQ0FBQyxLQUFLLEVBQUUsQ0FBQztZQUNuQixpREFBaUQ7WUFDakQsSUFBSSxDQUFDLEtBQUssQ0FBQyxrQkFBa0IsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUMsU0FBUyxDQUFDO1lBQ3ZELElBQUksQ0FBQyxjQUFjLEdBQUcsSUFBSSxDQUFDO1lBQzNCLElBQUksQ0FBQyxNQUFNLEdBQUcsSUFBSSxDQUFDO1lBRW5CLE9BQU87U0FDUjtRQUVELElBQUksSUFBSSxDQUFDLFFBQVEsSUFBSSxJQUFJLENBQUMsY0FBYyxFQUFFO1lBQ3hDLElBQUksQ0FBQyxLQUFLLENBQUMsS0FBSyxFQUFFLENBQUM7WUFDbkIsSUFBSSxDQUFDLGNBQWMsR0FBRyxLQUFLLENBQUM7U0FDN0I7UUFFRCxJQUFJLENBQUMsSUFBSSxDQUFDLE1BQU0sSUFBSSxLQUFLLEVBQUU7WUFDekIsSUFBSSxDQUFDLE1BQU0sR0FBRyxJQUFJLENBQUMsT0FBTyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQyxNQUFNLENBQUMsSUFBSSxDQUFDLFNBQVMsQ0FBQyxDQUFDO1NBQy9EO1FBRUQsSUFBSSxJQUFJLENBQUMsTUFBTSxFQUFFOztrQkFDVCxPQUFPLEdBQUcsSUFBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDO1lBRXZDLElBQUksT0FBTyxFQUFFO2dCQUNYLElBQUksQ0FBQyw0QkFBNEIsQ0FBQyxPQUFPLENBQUMsQ0FBQztnQkFDM0MsSUFBSSxDQUFDLHdCQUF3QixDQUFDLE9BQU8sQ0FBQyxDQUFDO2FBQ3hDO1NBQ0Y7SUFDSCxDQUFDOzs7Ozs7SUFFTyxTQUFTLENBQUMsS0FBWTtRQUM1QixJQUFJLElBQUksQ0FBQyxPQUFPLEVBQUU7WUFDaEIsS0FBSyxDQUFDLElBQUk7Ozs7O1lBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxFQUFFLEVBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxDQUFDLElBQUksQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDO1NBQzVHO2FBQU07WUFDTCxLQUFLLENBQUMsSUFBSSxFQUFFLENBQUM7U0FDZDtJQUNILENBQUM7Ozs7SUFFRCxXQUFXOztZQUNMLEtBQUssR0FBRyxtQkFBQSxLQUFLLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxFQUFTO1FBQ3RDLElBQUksQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLEtBQUssQ0FBQztZQUFFLE9BQU87O2NBRTVCLFNBQVMsR0FBRyxJQUFJLENBQUMsU0FBUztRQUVoQyxJQUFJLE9BQU8sSUFBSSxDQUFDLFFBQVEsS0FBSyxXQUFXLEVBQUU7WUFDeEMsS0FBSyxHQUFHLEtBQUssQ0FBQyxNQUFNOzs7O1lBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxTQUFTLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsRUFBRSxJQUFJLENBQUMsU0FBUyxDQUFDLEVBQUMsQ0FBQztTQUM5RTtRQUVELFFBQVEsSUFBSSxDQUFDLFFBQVEsRUFBRTtZQUNyQixLQUFLLEtBQUs7Z0JBQ1IsSUFBSSxDQUFDLFNBQVMsQ0FBQyxLQUFLLENBQUMsQ0FBQztnQkFDdEIsSUFBSSxDQUFDLFlBQVksQ0FBQyxLQUFLLENBQUMsQ0FBQztnQkFDekIsTUFBTTtZQUVSLEtBQUssTUFBTTtnQkFDVCxJQUFJLENBQUMsU0FBUyxDQUFDLEtBQUssQ0FBQyxDQUFDO2dCQUN0QixLQUFLLENBQUMsT0FBTyxFQUFFLENBQUM7Z0JBQ2hCLElBQUksQ0FBQyxZQUFZLENBQUMsS0FBSyxDQUFDLENBQUM7Z0JBQ3pCLE1BQU07WUFFUjtnQkFDRSxJQUFJLENBQUMsWUFBWSxDQUFDLEtBQUssQ0FBQyxDQUFDO1NBQzVCO0lBQ0gsQ0FBQzs7O1lBdEpGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsVUFBVTthQUNyQjs7OztZQW5CQyxXQUFXO1lBRVgsZ0JBQWdCO1lBSmhCLGVBQWU7OztvQkF1QmQsS0FBSyxTQUFDLFVBQVU7c0JBR2hCLEtBQUssU0FBQyxlQUFlO3VCQUdyQixLQUFLLFNBQUMsZ0JBQWdCO3VCQUd0QixLQUFLLFNBQUMsZ0JBQWdCO3dCQUd0QixLQUFLLFNBQUMsaUJBQWlCO3NCQUd2QixLQUFLLFNBQUMsZUFBZTt3QkFHckIsS0FBSyxTQUFDLGlCQUFpQjt1QkFHdkIsS0FBSyxTQUFDLGdCQUFnQjs7OztJQXJCdkIsNkJBQ2E7O0lBRWIsK0JBQ2dCOztJQUVoQixnQ0FDeUI7O0lBRXpCLGdDQUNpQjs7SUFFakIsaUNBQ2U7O0lBRWYsK0JBQ1E7O0lBRVIsaUNBQ3FCOztJQUVyQixnQ0FDMkI7Ozs7O0lBRTNCLDhCQUFvQzs7Ozs7SUFFcEMsc0NBQWdDOzs7OztJQVc5QiwrQkFBMkM7Ozs7O0lBQzNDLDZCQUErQjs7Ozs7SUFDL0IsK0JBQWdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcbiAgRGlyZWN0aXZlLFxuICBFbWJlZGRlZFZpZXdSZWYsXG4gIElucHV0LFxuICBJdGVyYWJsZUNoYW5nZVJlY29yZCxcbiAgSXRlcmFibGVDaGFuZ2VzLFxuICBJdGVyYWJsZURpZmZlcixcbiAgSXRlcmFibGVEaWZmZXJzLFxuICBPbkNoYW5nZXMsXG4gIFRlbXBsYXRlUmVmLFxuICBUcmFja0J5RnVuY3Rpb24sXG4gIFZpZXdDb250YWluZXJSZWZcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgY29tcGFyZSBmcm9tICdqdXN0LWNvbXBhcmUnO1xuaW1wb3J0IGNsb25lIGZyb20gJ2p1c3QtY2xvbmUnO1xuXG5leHBvcnQgdHlwZSBDb21wYXJlRm48VCA9IGFueT4gPSAodmFsdWU6IFQsIGNvbXBhcmlzb246IFQpID0+IGJvb2xlYW47XG5cbmNsYXNzIEFicEZvckNvbnRleHQge1xuICBjb25zdHJ1Y3RvcihwdWJsaWMgJGltcGxpY2l0OiBhbnksIHB1YmxpYyBpbmRleDogbnVtYmVyLCBwdWJsaWMgY291bnQ6IG51bWJlciwgcHVibGljIGxpc3Q6IGFueVtdKSB7fVxufVxuXG5jbGFzcyBSZWNvcmRWaWV3IHtcbiAgY29uc3RydWN0b3IocHVibGljIHJlY29yZDogSXRlcmFibGVDaGFuZ2VSZWNvcmQ8YW55PiwgcHVibGljIHZpZXc6IEVtYmVkZGVkVmlld1JlZjxBYnBGb3JDb250ZXh0Pikge31cbn1cblxuQERpcmVjdGl2ZSh7XG4gIHNlbGVjdG9yOiAnW2FicEZvcl0nXG59KVxuZXhwb3J0IGNsYXNzIEZvckRpcmVjdGl2ZSBpbXBsZW1lbnRzIE9uQ2hhbmdlcyB7XG4gIEBJbnB1dCgnYWJwRm9yT2YnKVxuICBpdGVtczogYW55W107XG5cbiAgQElucHV0KCdhYnBGb3JPcmRlckJ5JylcbiAgb3JkZXJCeTogc3RyaW5nO1xuXG4gIEBJbnB1dCgnYWJwRm9yT3JkZXJEaXInKVxuICBvcmRlckRpcjogJ0FTQycgfCAnREVTQyc7XG5cbiAgQElucHV0KCdhYnBGb3JGaWx0ZXJCeScpXG4gIGZpbHRlckJ5OiBzdHJpbmc7XG5cbiAgQElucHV0KCdhYnBGb3JGaWx0ZXJWYWwnKVxuICBmaWx0ZXJWYWw6IGFueTtcblxuICBASW5wdXQoJ2FicEZvclRyYWNrQnknKVxuICB0cmFja0J5O1xuXG4gIEBJbnB1dCgnYWJwRm9yQ29tcGFyZUJ5JylcbiAgY29tcGFyZUJ5OiBDb21wYXJlRm47XG5cbiAgQElucHV0KCdhYnBGb3JFbXB0eVJlZicpXG4gIGVtcHR5UmVmOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIHByaXZhdGUgZGlmZmVyOiBJdGVyYWJsZURpZmZlcjxhbnk+O1xuXG4gIHByaXZhdGUgaXNTaG93RW1wdHlSZWY6IGJvb2xlYW47XG5cbiAgZ2V0IGNvbXBhcmVGbigpOiBDb21wYXJlRm4ge1xuICAgIHJldHVybiB0aGlzLmNvbXBhcmVCeSB8fCBjb21wYXJlO1xuICB9XG5cbiAgZ2V0IHRyYWNrQnlGbigpOiBUcmFja0J5RnVuY3Rpb248YW55PiB7XG4gICAgcmV0dXJuIHRoaXMudHJhY2tCeSB8fCAoKGluZGV4OiBudW1iZXIsIGl0ZW06IGFueSkgPT4gKGl0ZW0gYXMgYW55KS5pZCB8fCBpbmRleCk7XG4gIH1cblxuICBjb25zdHJ1Y3RvcihcbiAgICBwcml2YXRlIHRlbXBSZWY6IFRlbXBsYXRlUmVmPEFicEZvckNvbnRleHQ+LFxuICAgIHByaXZhdGUgdmNSZWY6IFZpZXdDb250YWluZXJSZWYsXG4gICAgcHJpdmF0ZSBkaWZmZXJzOiBJdGVyYWJsZURpZmZlcnNcbiAgKSB7fVxuXG4gIHByaXZhdGUgaXRlcmF0ZU92ZXJBcHBsaWVkT3BlcmF0aW9ucyhjaGFuZ2VzOiBJdGVyYWJsZUNoYW5nZXM8YW55Pikge1xuICAgIGNvbnN0IHJ3OiBSZWNvcmRWaWV3W10gPSBbXTtcblxuICAgIGNoYW5nZXMuZm9yRWFjaE9wZXJhdGlvbigocmVjb3JkOiBJdGVyYWJsZUNoYW5nZVJlY29yZDxhbnk+LCBwcmV2aW91c0luZGV4OiBudW1iZXIsIGN1cnJlbnRJbmRleDogbnVtYmVyKSA9PiB7XG4gICAgICBpZiAocmVjb3JkLnByZXZpb3VzSW5kZXggPT0gbnVsbCkge1xuICAgICAgICBjb25zdCB2aWV3ID0gdGhpcy52Y1JlZi5jcmVhdGVFbWJlZGRlZFZpZXcoXG4gICAgICAgICAgdGhpcy50ZW1wUmVmLFxuICAgICAgICAgIG5ldyBBYnBGb3JDb250ZXh0KG51bGwsIC0xLCAtMSwgdGhpcy5pdGVtcyksXG4gICAgICAgICAgY3VycmVudEluZGV4XG4gICAgICAgICk7XG5cbiAgICAgICAgcncucHVzaChuZXcgUmVjb3JkVmlldyhyZWNvcmQsIHZpZXcpKTtcbiAgICAgIH0gZWxzZSBpZiAoY3VycmVudEluZGV4ID09IG51bGwpIHtcbiAgICAgICAgdGhpcy52Y1JlZi5yZW1vdmUocHJldmlvdXNJbmRleCk7XG4gICAgICB9IGVsc2Uge1xuICAgICAgICBjb25zdCB2aWV3ID0gdGhpcy52Y1JlZi5nZXQocHJldmlvdXNJbmRleCk7XG4gICAgICAgIHRoaXMudmNSZWYubW92ZSh2aWV3LCBjdXJyZW50SW5kZXgpO1xuXG4gICAgICAgIHJ3LnB1c2gobmV3IFJlY29yZFZpZXcocmVjb3JkLCB2aWV3IGFzIEVtYmVkZGVkVmlld1JlZjxBYnBGb3JDb250ZXh0PikpO1xuICAgICAgfVxuICAgIH0pO1xuXG4gICAgZm9yIChsZXQgaSA9IDAsIGwgPSBydy5sZW5ndGg7IGkgPCBsOyBpKyspIHtcbiAgICAgIHJ3W2ldLnZpZXcuY29udGV4dC4kaW1wbGljaXQgPSByd1tpXS5yZWNvcmQuaXRlbTtcbiAgICB9XG4gIH1cblxuICBwcml2YXRlIGl0ZXJhdGVPdmVyQXR0YWNoZWRWaWV3cyhjaGFuZ2VzOiBJdGVyYWJsZUNoYW5nZXM8YW55Pikge1xuICAgIGZvciAobGV0IGkgPSAwLCBsID0gdGhpcy52Y1JlZi5sZW5ndGg7IGkgPCBsOyBpKyspIHtcbiAgICAgIGNvbnN0IHZpZXdSZWYgPSB0aGlzLnZjUmVmLmdldChpKSBhcyBFbWJlZGRlZFZpZXdSZWY8QWJwRm9yQ29udGV4dD47XG4gICAgICB2aWV3UmVmLmNvbnRleHQuaW5kZXggPSBpO1xuICAgICAgdmlld1JlZi5jb250ZXh0LmNvdW50ID0gbDtcbiAgICAgIHZpZXdSZWYuY29udGV4dC5saXN0ID0gdGhpcy5pdGVtcztcbiAgICB9XG5cbiAgICBjaGFuZ2VzLmZvckVhY2hJZGVudGl0eUNoYW5nZSgocmVjb3JkOiBJdGVyYWJsZUNoYW5nZVJlY29yZDxhbnk+KSA9PiB7XG4gICAgICBjb25zdCB2aWV3UmVmID0gdGhpcy52Y1JlZi5nZXQocmVjb3JkLmN1cnJlbnRJbmRleCkgYXMgRW1iZWRkZWRWaWV3UmVmPEFicEZvckNvbnRleHQ+O1xuICAgICAgdmlld1JlZi5jb250ZXh0LiRpbXBsaWNpdCA9IHJlY29yZC5pdGVtO1xuICAgIH0pO1xuICB9XG5cbiAgcHJpdmF0ZSBwcm9qZWN0SXRlbXMoaXRlbXM6IGFueVtdKTogdm9pZCB7XG4gICAgaWYgKCFpdGVtcy5sZW5ndGggJiYgdGhpcy5lbXB0eVJlZikge1xuICAgICAgdGhpcy52Y1JlZi5jbGVhcigpO1xuICAgICAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby11bnVzZWQtZXhwcmVzc2lvblxuICAgICAgdGhpcy52Y1JlZi5jcmVhdGVFbWJlZGRlZFZpZXcodGhpcy5lbXB0eVJlZikucm9vdE5vZGVzO1xuICAgICAgdGhpcy5pc1Nob3dFbXB0eVJlZiA9IHRydWU7XG4gICAgICB0aGlzLmRpZmZlciA9IG51bGw7XG5cbiAgICAgIHJldHVybjtcbiAgICB9XG5cbiAgICBpZiAodGhpcy5lbXB0eVJlZiAmJiB0aGlzLmlzU2hvd0VtcHR5UmVmKSB7XG4gICAgICB0aGlzLnZjUmVmLmNsZWFyKCk7XG4gICAgICB0aGlzLmlzU2hvd0VtcHR5UmVmID0gZmFsc2U7XG4gICAgfVxuXG4gICAgaWYgKCF0aGlzLmRpZmZlciAmJiBpdGVtcykge1xuICAgICAgdGhpcy5kaWZmZXIgPSB0aGlzLmRpZmZlcnMuZmluZChpdGVtcykuY3JlYXRlKHRoaXMudHJhY2tCeUZuKTtcbiAgICB9XG5cbiAgICBpZiAodGhpcy5kaWZmZXIpIHtcbiAgICAgIGNvbnN0IGNoYW5nZXMgPSB0aGlzLmRpZmZlci5kaWZmKGl0ZW1zKTtcblxuICAgICAgaWYgKGNoYW5nZXMpIHtcbiAgICAgICAgdGhpcy5pdGVyYXRlT3ZlckFwcGxpZWRPcGVyYXRpb25zKGNoYW5nZXMpO1xuICAgICAgICB0aGlzLml0ZXJhdGVPdmVyQXR0YWNoZWRWaWV3cyhjaGFuZ2VzKTtcbiAgICAgIH1cbiAgICB9XG4gIH1cblxuICBwcml2YXRlIHNvcnRJdGVtcyhpdGVtczogYW55W10pIHtcbiAgICBpZiAodGhpcy5vcmRlckJ5KSB7XG4gICAgICBpdGVtcy5zb3J0KChhLCBiKSA9PiAoYVt0aGlzLm9yZGVyQnldID4gYlt0aGlzLm9yZGVyQnldID8gMSA6IGFbdGhpcy5vcmRlckJ5XSA8IGJbdGhpcy5vcmRlckJ5XSA/IC0xIDogMCkpO1xuICAgIH0gZWxzZSB7XG4gICAgICBpdGVtcy5zb3J0KCk7XG4gICAgfVxuICB9XG5cbiAgbmdPbkNoYW5nZXMoKSB7XG4gICAgbGV0IGl0ZW1zID0gY2xvbmUodGhpcy5pdGVtcykgYXMgYW55W107XG4gICAgaWYgKCFBcnJheS5pc0FycmF5KGl0ZW1zKSkgcmV0dXJuO1xuXG4gICAgY29uc3QgY29tcGFyZUZuID0gdGhpcy5jb21wYXJlRm47XG5cbiAgICBpZiAodHlwZW9mIHRoaXMuZmlsdGVyQnkgIT09ICd1bmRlZmluZWQnKSB7XG4gICAgICBpdGVtcyA9IGl0ZW1zLmZpbHRlcihpdGVtID0+IGNvbXBhcmVGbihpdGVtW3RoaXMuZmlsdGVyQnldLCB0aGlzLmZpbHRlclZhbCkpO1xuICAgIH1cblxuICAgIHN3aXRjaCAodGhpcy5vcmRlckRpcikge1xuICAgICAgY2FzZSAnQVNDJzpcbiAgICAgICAgdGhpcy5zb3J0SXRlbXMoaXRlbXMpO1xuICAgICAgICB0aGlzLnByb2plY3RJdGVtcyhpdGVtcyk7XG4gICAgICAgIGJyZWFrO1xuXG4gICAgICBjYXNlICdERVNDJzpcbiAgICAgICAgdGhpcy5zb3J0SXRlbXMoaXRlbXMpO1xuICAgICAgICBpdGVtcy5yZXZlcnNlKCk7XG4gICAgICAgIHRoaXMucHJvamVjdEl0ZW1zKGl0ZW1zKTtcbiAgICAgICAgYnJlYWs7XG5cbiAgICAgIGRlZmF1bHQ6XG4gICAgICAgIHRoaXMucHJvamVjdEl0ZW1zKGl0ZW1zKTtcbiAgICB9XG4gIH1cbn1cbiJdfQ==
