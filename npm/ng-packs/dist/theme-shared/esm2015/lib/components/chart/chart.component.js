/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, ElementRef, EventEmitter, Input, Output, ChangeDetectorRef } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { chartJsLoaded$ } from '../../utils/widget-utils';
export class ChartComponent {
  /**
   * @param {?} el
   * @param {?} cdRef
   */
  constructor(el, cdRef) {
    this.el = el;
    this.cdRef = cdRef;
    this.options = {};
    this.plugins = [];
    this.responsive = true;
    // tslint:disable-next-line: no-output-on-prefix
    this.onDataSelect = new EventEmitter();
    this.initialized = new BehaviorSubject(this);
    this.onCanvasClick
    /**
     * @param {?} event
     * @return {?}
     */ = event => {
      if (this.chart) {
        /** @type {?} */
        const element = this.chart.getElementAtEvent(event);
        /** @type {?} */
        const dataset = this.chart.getDatasetAtEvent(event);
        if (element && element[0] && dataset) {
          this.onDataSelect.emit({
            originalEvent: event,
            element: element[0],
            dataset,
          });
        }
      }
    };
    this.initChart
    /**
     * @return {?}
     */ = () => {
      /** @type {?} */
      const opts = this.options || {};
      opts.responsive = this.responsive;
      // allows chart to resize in responsive mode
      if (opts.responsive && (this.height || this.width)) {
        opts.maintainAspectRatio = false;
      }
      this.chart = new Chart(this.el.nativeElement.children[0].children[0], {
        type: this.type,
        data: this.data,
        options: this.options,
        plugins: this.plugins,
      });
      this.cdRef.detectChanges();
    };
    this.generateLegend
    /**
     * @return {?}
     */ = () => {
      if (this.chart) {
        return this.chart.generateLegend();
      }
    };
    this.refresh
    /**
     * @return {?}
     */ = () => {
      if (this.chart) {
        this.chart.update();
        this.cdRef.detectChanges();
      }
    };
    this.reinit
    /**
     * @return {?}
     */ = () => {
      if (this.chart) {
        this.chart.destroy();
        this.initChart();
      }
    };
  }
  /**
   * @return {?}
   */
  get data() {
    return this._data;
  }
  /**
   * @param {?} val
   * @return {?}
   */
  set data(val) {
    this._data = val;
    this.reinit();
  }
  /**
   * @return {?}
   */
  get canvas() {
    return this.el.nativeElement.children[0].children[0];
  }
  /**
   * @return {?}
   */
  get base64Image() {
    return this.chart.toBase64Image();
  }
  /**
   * @return {?}
   */
  ngAfterViewInit() {
    chartJsLoaded$.subscribe(
      /**
       * @return {?}
       */
      () => {
        try {
          // tslint:disable-next-line: no-unused-expression
          Chart;
        } catch (error) {
          console.error(`Chart is not found. Import the Chart from app.module like shown below:
        import('chart.js');
        `);
          return;
        }
        this.initChart();
        this._initialized = true;
      },
    );
  }
  /**
   * @return {?}
   */
  ngOnDestroy() {
    if (this.chart) {
      this.chart.destroy();
      this._initialized = false;
      this.chart = null;
    }
  }
}
ChartComponent.decorators = [
  {
    type: Component,
    args: [
      {
        selector: 'abp-chart',
        template:
          '<div\n  style="position:relative"\n  [style.width]="responsive && !width ? null : width"\n  [style.height]="responsive && !height ? null : height"\n>\n  <canvas\n    [attr.width]="responsive && !width ? null : width"\n    [attr.height]="responsive && !height ? null : height"\n    (click)="onCanvasClick($event)"\n  ></canvas>\n</div>\n',
      },
    ],
  },
];
/** @nocollapse */
ChartComponent.ctorParameters = () => [{ type: ElementRef }, { type: ChangeDetectorRef }];
ChartComponent.propDecorators = {
  type: [{ type: Input }],
  options: [{ type: Input }],
  plugins: [{ type: Input }],
  width: [{ type: Input }],
  height: [{ type: Input }],
  responsive: [{ type: Input }],
  onDataSelect: [{ type: Output }],
  initialized: [{ type: Output }],
  data: [{ type: Input }],
};
if (false) {
  /** @type {?} */
  ChartComponent.prototype.type;
  /** @type {?} */
  ChartComponent.prototype.options;
  /** @type {?} */
  ChartComponent.prototype.plugins;
  /** @type {?} */
  ChartComponent.prototype.width;
  /** @type {?} */
  ChartComponent.prototype.height;
  /** @type {?} */
  ChartComponent.prototype.responsive;
  /** @type {?} */
  ChartComponent.prototype.onDataSelect;
  /** @type {?} */
  ChartComponent.prototype.initialized;
  /**
   * @type {?}
   * @private
   */
  ChartComponent.prototype._initialized;
  /** @type {?} */
  ChartComponent.prototype._data;
  /** @type {?} */
  ChartComponent.prototype.chart;
  /** @type {?} */
  ChartComponent.prototype.onCanvasClick;
  /** @type {?} */
  ChartComponent.prototype.initChart;
  /** @type {?} */
  ChartComponent.prototype.generateLegend;
  /** @type {?} */
  ChartComponent.prototype.refresh;
  /** @type {?} */
  ChartComponent.prototype.reinit;
  /** @type {?} */
  ChartComponent.prototype.el;
  /**
   * @type {?}
   * @private
   */
  ChartComponent.prototype.cdRef;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhcnQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9jaGFydC9jaGFydC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFFTCxTQUFTLEVBQ1QsVUFBVSxFQUNWLFlBQVksRUFDWixLQUFLLEVBRUwsTUFBTSxFQUNOLGlCQUFpQixFQUNsQixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ3ZDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSwwQkFBMEIsQ0FBQztBQU8xRCxNQUFNLE9BQU8sY0FBYzs7Ozs7SUF3QnpCLFlBQW1CLEVBQWMsRUFBVSxLQUF3QjtRQUFoRCxPQUFFLEdBQUYsRUFBRSxDQUFZO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBbUI7UUFyQjFELFlBQU8sR0FBUSxFQUFFLENBQUM7UUFFbEIsWUFBTyxHQUFVLEVBQUUsQ0FBQztRQU1wQixlQUFVLEdBQUcsSUFBSSxDQUFDOztRQUdSLGlCQUFZLEdBQXNCLElBQUksWUFBWSxFQUFFLENBQUM7UUFFckQsZ0JBQVcsR0FBRyxJQUFJLGVBQWUsQ0FBQyxJQUFJLENBQUMsQ0FBQztRQTRDM0Qsa0JBQWE7Ozs7UUFBRyxLQUFLLENBQUMsRUFBRTtZQUN0QixJQUFJLElBQUksQ0FBQyxLQUFLLEVBQUU7O3NCQUNSLE9BQU8sR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGlCQUFpQixDQUFDLEtBQUssQ0FBQzs7c0JBQzdDLE9BQU8sR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGlCQUFpQixDQUFDLEtBQUssQ0FBQztnQkFDbkQsSUFBSSxPQUFPLElBQUksT0FBTyxDQUFDLENBQUMsQ0FBQyxJQUFJLE9BQU8sRUFBRTtvQkFDcEMsSUFBSSxDQUFDLFlBQVksQ0FBQyxJQUFJLENBQUM7d0JBQ3JCLGFB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WUFDZCxJQUFJLENBQUMsS0FBSyxDQUFDLE9BQU8sRUFBRSxDQUFDO1lBQ3JCLElBQUksQ0FBQyxZQUFZLEdBQUcsS0FBSyxDQUFDO1lBQzFCLElBQUksQ0FBQyxLQUFLLEdBQUcsSUFBSSxDQUFDO1NBQ25CO0lBQ0gsQ0FBQzs7O1lBM0hGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsV0FBVztnQkFDckIsd1dBQXFDO2FBQ3RDOzs7O1lBZEMsVUFBVTtZQUtWLGlCQUFpQjs7O21CQVdoQixLQUFLO3NCQUVMLEtBQUs7c0JBRUwsS0FBSztvQkFFTCxLQUFLO3FCQUVMLEtBQUs7eUJBRUwsS0FBSzsyQkFHTCxNQUFNOzBCQUVOLE1BQU07bUJBVU4sS0FBSzs7OztJQXpCTiw4QkFBc0I7O0lBRXRCLGlDQUEyQjs7SUFFM0IsaUNBQTZCOztJQUU3QiwrQkFBdUI7O0lBRXZCLGdDQUF3Qjs7SUFFeEIsb0NBQTJCOztJQUczQixzQ0FBd0U7O0lBRXhFLHFDQUEyRDs7Ozs7SUFFM0Qsc0NBQThCOztJQUU5QiwrQkFBVzs7SUFFWCwrQkFBVzs7SUFzQ1gsdUNBWUU7O0lBRUYsbUNBaUJFOztJQUVGLHdDQUlFOztJQUVGLGlDQUtFOztJQUVGLGdDQUtFOztJQXZGVSw0QkFBcUI7Ozs7O0lBQUUsK0JBQWdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcbiAgQWZ0ZXJWaWV3SW5pdCxcbiAgQ29tcG9uZW50LFxuICBFbGVtZW50UmVmLFxuICBFdmVudEVtaXR0ZXIsXG4gIElucHV0LFxuICBPbkRlc3Ryb3ksXG4gIE91dHB1dCxcbiAgQ2hhbmdlRGV0ZWN0b3JSZWZcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBCZWhhdmlvclN1YmplY3QgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGNoYXJ0SnNMb2FkZWQkIH0gZnJvbSAnLi4vLi4vdXRpbHMvd2lkZ2V0LXV0aWxzJztcbmRlY2xhcmUgY29uc3QgQ2hhcnQ6IGFueTtcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWNoYXJ0JyxcbiAgdGVtcGxhdGVVcmw6ICcuL2NoYXJ0LmNvbXBvbmVudC5odG1sJ1xufSlcbmV4cG9ydCBjbGFzcyBDaGFydENvbXBvbmVudCBpbXBsZW1lbnRzIEFmdGVyVmlld0luaXQsIE9uRGVzdHJveSB7XG4gIEBJbnB1dCgpIHR5cGU6IHN0cmluZztcblxuICBASW5wdXQoKSBvcHRpb25zOiBhbnkgPSB7fTtcblxuICBASW5wdXQoKSBwbHVnaW5zOiBhbnlbXSA9IFtdO1xuXG4gIEBJbnB1dCgpIHdpZHRoOiBzdHJpbmc7XG5cbiAgQElucHV0KCkgaGVpZ2h0OiBzdHJpbmc7XG5cbiAgQElucHV0KCkgcmVzcG9uc2l2ZSA9IHRydWU7XG5cbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby1vdXRwdXQtb24tcHJlZml4XG4gIEBPdXRwdXQoKSByZWFkb25seSBvbkRhdGFTZWxlY3Q6IEV2ZW50RW1pdHRlcjxhbnk+ID0gbmV3IEV2ZW50RW1pdHRlcigpO1xuXG4gIEBPdXRwdXQoKSByZWFkb25seSBpbml0aWFsaXplZCA9IG5ldyBCZWhhdmlvclN1YmplY3QodGhpcyk7XG5cbiAgcHJpdmF0ZSBfaW5pdGlhbGl6ZWQ6IGJvb2xlYW47XG5cbiAgX2RhdGE6IGFueTtcblxuICBjaGFydDogYW55O1xuXG4gIGNvbnN0cnVjdG9yKHB1YmxpYyBlbDogRWxlbWVudFJlZiwgcHJpdmF0ZSBjZFJlZjogQ2hhbmdlRGV0ZWN0b3JSZWYpIHt9XG5cbiAgQElucHV0KCkgZ2V0IGRhdGEoKTogYW55IHtcbiAgICByZXR1cm4gdGhpcy5fZGF0YTtcbiAgfVxuXG4gIHNldCBkYXRhKHZhbDogYW55KSB7XG4gICAgdGhpcy5fZGF0YSA9IHZhbDtcbiAgICB0aGlzLnJlaW5pdCgpO1xuICB9XG5cbiAgZ2V0IGNhbnZhcygpIHtcbiAgICByZXR1cm4gdGhpcy5lbC5uYXRpdmVFbGVtZW50LmNoaWxkcmVuWzBdLmNoaWxkcmVuWzBdO1xuICB9XG5cbiAgZ2V0IGJhc2U2NEltYWdlKCkge1xuICAgIHJldHVybiB0aGlzLmNoYXJ0LnRvQmFzZTY0SW1hZ2UoKTtcbiAgfVxuXG4gIG5nQWZ0ZXJWaWV3SW5pdCgpIHtcbiAgICBjaGFydEpzTG9hZGVkJC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgdHJ5IHtcbiAgICAgICAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby11bnVzZWQtZXhwcmVzc2lvblxuICAgICAgICBDaGFydDtcbiAgICAgIH0gY2F0Y2ggKGVycm9yKSB7XG4gICAgICAgIGNvbnNvbGUuZXJyb3IoYENoYXJ0IGlzIG5vdCBmb3VuZC4gSW1wb3J0IHRoZSBDaGFydCBmcm9tIGFwcC5tb2R1bGUgbGlrZSBzaG93biBiZWxvdzpcbiAgICAgICAgaW1wb3J0KCdjaGFydC5qcycpO1xuICAgICAgICBgKTtcbiAgICAgICAgcmV0dXJuO1xuICAgICAgfVxuXG4gICAgICB0aGlzLmluaXRDaGFydCgpO1xuICAgICAgdGhpcy5faW5pdGlhbGl6ZWQgPSB0cnVlO1xuICAgIH0pO1xuICB9XG5cbiAgb25DYW52YXNDbGljayA9IGV2ZW50ID0+IHtcbiAgICBpZiAodGhpcy5jaGFydCkge1xuICAgICAgY29uc3QgZWxlbWVudCA9IHRoaXMuY2hhcnQuZ2V0RWxlbWVudEF0RXZlbnQoZXZlbnQpO1xuICAgICAgY29uc3QgZGF0YXNldCA9IHRoaXMuY2hhcnQuZ2V0RGF0YXNldEF0RXZlbnQoZXZlbnQpO1xuICAgICAgaWYgKGVsZW1lbnQgJiYgZWxlbWVudFswXSAmJiBkYXRhc2V0KSB7XG4gICAgICAgIHRoaXMub25EYXRhU2VsZWN0LmVtaXQoe1xuICAgICAgICAgIG9yaWdpbmFsRXZlbnQ6IGV2ZW50LFxuICAgICAgICAgIGVsZW1lbnQ6IGVsZW1lbnRbMF0sXG4gICAgICAgICAgZGF0YXNldFxuICAgICAgICB9KTtcbiAgICAgIH1cbiAgICB9XG4gIH07XG5cbiAgaW5pdENoYXJ0ID0gKCkgPT4ge1xuICAgIGNvbnN0IG9wdHMgPSB0aGlzLm9wdGlvbnMgfHwge307XG4gICAgb3B0cy5yZXNwb25zaXZlID0gdGhpcy5yZXNwb25zaXZlO1xuXG4gICAgLy8gYWxsb3dzIGNoYXJ0IHRvIHJlc2l6ZSBpbiByZXNwb25zaXZlIG1vZGVcbiAgICBpZiAob3B0cy5yZXNwb25zaXZlICYmICh0aGlzLmhlaWdodCB8fCB0aGlzLndpZHRoKSkge1xuICAgICAgb3B0cy5tYWludGFpbkFzcGVjdFJhdGlvID0gZmFsc2U7XG4gICAgfVxuXG4gICAgdGhpcy5jaGFydCA9IG5ldyBDaGFydCh0aGlzLmVsLm5hdGl2ZUVsZW1lbnQuY2hpbGRyZW5bMF0uY2hpbGRyZW5bMF0sIHtcbiAgICAgIHR5cGU6IHRoaXMudHlwZSxcbiAgICAgIGRhdGE6IHRoaXMuZGF0YSxcbiAgICAgIG9wdGlvbnM6IHRoaXMub3B0aW9ucyxcbiAgICAgIHBsdWdpbnM6IHRoaXMucGx1Z2luc1xuICAgIH0pO1xuXG4gICAgdGhpcy5jZFJlZi5kZXRlY3RDaGFuZ2VzKCk7XG4gIH07XG5cbiAgZ2VuZXJhdGVMZWdlbmQgPSAoKSA9PiB7XG4gICAgaWYgKHRoaXMuY2hhcnQpIHtcbiAgICAgIHJldHVybiB0aGlzLmNoYXJ0LmdlbmVyYXRlTGVnZW5kKCk7XG4gICAgfVxuICB9O1xuXG4gIHJlZnJlc2ggPSAoKSA9PiB7XG4gICAgaWYgKHRoaXMuY2hhcnQpIHtcbiAgICAgIHRoaXMuY2hhcnQudXBkYXRlKCk7XG4gICAgICB0aGlzLmNkUmVmLmRldGVjdENoYW5nZXMoKTtcbiAgICB9XG4gIH07XG5cbiAgcmVpbml0ID0gKCkgPT4ge1xuICAgIGlmICh0aGlzLmNoYXJ0KSB7XG4gICAgICB0aGlzLmNoYXJ0LmRlc3Ryb3koKTtcbiAgICAgIHRoaXMuaW5pdENoYXJ0KCk7XG4gICAgfVxuICB9O1xuXG4gIG5nT25EZXN0cm95KCkge1xuICAgIGlmICh0aGlzLmNoYXJ0KSB7XG4gICAgICB0aGlzLmNoYXJ0LmRlc3Ryb3koKTtcbiAgICAgIHRoaXMuX2luaXRpYWxpemVkID0gZmFsc2U7XG4gICAgICB0aGlzLmNoYXJ0ID0gbnVsbDtcbiAgICB9XG4gIH1cbn1cbiJdfQ==
