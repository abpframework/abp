/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, ElementRef, EventEmitter, Input, Output, ChangeDetectorRef } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { chartJsLoaded$ } from '../../utils/widget-utils';
var ChartComponent = /** @class */ (function() {
  function ChartComponent(el, cdRef) {
    var _this = this;
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
     */ = function(event) {
      if (_this.chart) {
        /** @type {?} */
        var element = _this.chart.getElementAtEvent(event);
        /** @type {?} */
        var dataset = _this.chart.getDatasetAtEvent(event);
        if (element && element[0] && dataset) {
          _this.onDataSelect.emit({
            originalEvent: event,
            element: element[0],
            dataset: dataset,
          });
        }
      }
    };
    this.initChart
    /**
     * @return {?}
     */ = function() {
      /** @type {?} */
      var opts = _this.options || {};
      opts.responsive = _this.responsive;
      // allows chart to resize in responsive mode
      if (opts.responsive && (_this.height || _this.width)) {
        opts.maintainAspectRatio = false;
      }
      _this.chart = new Chart(_this.el.nativeElement.children[0].children[0], {
        type: _this.type,
        data: _this.data,
        options: _this.options,
        plugins: _this.plugins,
      });
      _this.cdRef.detectChanges();
    };
    this.generateLegend
    /**
     * @return {?}
     */ = function() {
      if (_this.chart) {
        return _this.chart.generateLegend();
      }
    };
    this.refresh
    /**
     * @return {?}
     */ = function() {
      if (_this.chart) {
        _this.chart.update();
        _this.cdRef.detectChanges();
      }
    };
    this.reinit
    /**
     * @return {?}
     */ = function() {
      if (_this.chart) {
        _this.chart.destroy();
        _this.initChart();
      }
    };
  }
  Object.defineProperty(ChartComponent.prototype, 'data', {
    /**
     * @return {?}
     */
    get: function() {
      return this._data;
    },
    /**
     * @param {?} val
     * @return {?}
     */
    set: function(val) {
      this._data = val;
      this.reinit();
    },
    enumerable: true,
    configurable: true,
  });
  Object.defineProperty(ChartComponent.prototype, 'canvas', {
    /**
     * @return {?}
     */
    get: function() {
      return this.el.nativeElement.children[0].children[0];
    },
    enumerable: true,
    configurable: true,
  });
  Object.defineProperty(ChartComponent.prototype, 'base64Image', {
    /**
     * @return {?}
     */
    get: function() {
      return this.chart.toBase64Image();
    },
    enumerable: true,
    configurable: true,
  });
  /**
   * @return {?}
   */
  ChartComponent.prototype.ngAfterViewInit
  /**
   * @return {?}
   */ = function() {
    var _this = this;
    chartJsLoaded$.subscribe(
      /**
       * @return {?}
       */
      function() {
        try {
          // tslint:disable-next-line: no-unused-expression
          Chart;
        } catch (error) {
          console.error(
            "Chart is not found. Import the Chart from app.module like shown below:\n        import('chart.js');\n        ",
          );
          return;
        }
        _this.initChart();
        _this._initialized = true;
      },
    );
  };
  /**
   * @return {?}
   */
  ChartComponent.prototype.ngOnDestroy
  /**
   * @return {?}
   */ = function() {
    if (this.chart) {
      this.chart.destroy();
      this._initialized = false;
      this.chart = null;
    }
  };
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
  ChartComponent.ctorParameters = function() {
    return [{ type: ElementRef }, { type: ChangeDetectorRef }];
  };
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
  return ChartComponent;
})();
export { ChartComponent };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhcnQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9jaGFydC9jaGFydC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFFTCxTQUFTLEVBQ1QsVUFBVSxFQUNWLFlBQVksRUFDWixLQUFLLEVBRUwsTUFBTSxFQUNOLGlCQUFpQixFQUNsQixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ3ZDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSwwQkFBMEIsQ0FBQztBQUcxRDtJQTRCRSx3QkFBbUIsRUFBYyxFQUFVLEtBQXdCO1FBQW5FLGlCQUF1RTtRQUFwRCxPQUFFLEdBQUYsRUFBRSxDQUFZO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBbUI7UUFyQjFELFlBQU8sR0FBUSxFQUFFLENBQUM7UUFFbEIsWUFBTyxHQUFVLEVBQUUsQ0FBQztRQU1wQixlQUFVLEdBQUcsSUFBSSxDQUFDOztRQUdSLGlCQUFZLEdBQXNCLElBQUksWUFBWSxFQUFFLENBQUM7UUFFckQsZ0JBQVcsR0FBRyxJQUFJLGVBQWUsQ0FBQyxJQUFJLENBQUMsQ0FBQztRQTRDM0Qsa0JBQWE7Ozs7UUFBRyxVQUFBLEtBQUs7WUFDbkIsSUFBSSxLQUFJLENBQUMsS0FBSyxFQUFFOztvQkFDUixPQUFPLEdBQUcsS0FBSSxDQUFDLEtBQUssQ0FBQyxpQkFBaUIsQ0FBQyxLQUFLLENBQUM7O29CQUM3QyxPQUFPLEdBQUcsS0FBSSxDQUFDLEtBQUssQ0FBQyxpQkFBaUIsQ0FBQyxLQUFLLENBQUM7Z0JBQ25ELElBQUksT0FBTyxJQUFJLE9BQU8sQ0FBQyxDQUFDLENBQUMsSUFBSSxPQUFPLEVBQUU7b0JBQ3BDLEtBQUksQ0FBQyxZQUFZLENBQUMsSUFBSSxDQUFDO3dCQUNyQixhQUFhLEVBQUUsS0FB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QUU7WUFDZCxJQUFJLENBQUMsS0FBSyxDQUFDLE9BQU8sRUFBRSxDQUFDO1lBQ3JCLElBQUksQ0FBQyxZQUFZLEdBQUcsS0FBSyxDQUFDO1lBQzFCLElBQUksQ0FBQyxLQUFLLEdBQUcsSUFBSSxDQUFDO1NBQ25CO0lBQ0gsQ0FBQzs7Z0JBM0hGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsV0FBVztvQkFDckIsd1dBQXFDO2lCQUN0Qzs7OztnQkFkQyxVQUFVO2dCQUtWLGlCQUFpQjs7O3VCQVdoQixLQUFLOzBCQUVMLEtBQUs7MEJBRUwsS0FBSzt3QkFFTCxLQUFLO3lCQUVMLEtBQUs7NkJBRUwsS0FBSzsrQkFHTCxNQUFNOzhCQUVOLE1BQU07dUJBVU4sS0FBSzs7SUE4RlIscUJBQUM7Q0FBQSxBQTVIRCxJQTRIQztTQXhIWSxjQUFjOzs7SUFDekIsOEJBQXNCOztJQUV0QixpQ0FBMkI7O0lBRTNCLGlDQUE2Qjs7SUFFN0IsK0JBQXVCOztJQUV2QixnQ0FBd0I7O0lBRXhCLG9DQUEyQjs7SUFHM0Isc0NBQXdFOztJQUV4RSxxQ0FBMkQ7Ozs7O0lBRTNELHNDQUE4Qjs7SUFFOUIsK0JBQVc7O0lBRVgsK0JBQVc7O0lBc0NYLHVDQVlFOztJQUVGLG1DQWlCRTs7SUFFRix3Q0FJRTs7SUFFRixpQ0FLRTs7SUFFRixnQ0FLRTs7SUF2RlUsNEJBQXFCOzs7OztJQUFFLCtCQUFnQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7XG4gIEFmdGVyVmlld0luaXQsXG4gIENvbXBvbmVudCxcbiAgRWxlbWVudFJlZixcbiAgRXZlbnRFbWl0dGVyLFxuICBJbnB1dCxcbiAgT25EZXN0cm95LFxuICBPdXRwdXQsXG4gIENoYW5nZURldGVjdG9yUmVmXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQmVoYXZpb3JTdWJqZWN0IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBjaGFydEpzTG9hZGVkJCB9IGZyb20gJy4uLy4uL3V0aWxzL3dpZGdldC11dGlscyc7XG5kZWNsYXJlIGNvbnN0IENoYXJ0OiBhbnk7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1jaGFydCcsXG4gIHRlbXBsYXRlVXJsOiAnLi9jaGFydC5jb21wb25lbnQuaHRtbCdcbn0pXG5leHBvcnQgY2xhc3MgQ2hhcnRDb21wb25lbnQgaW1wbGVtZW50cyBBZnRlclZpZXdJbml0LCBPbkRlc3Ryb3kge1xuICBASW5wdXQoKSB0eXBlOiBzdHJpbmc7XG5cbiAgQElucHV0KCkgb3B0aW9uczogYW55ID0ge307XG5cbiAgQElucHV0KCkgcGx1Z2luczogYW55W10gPSBbXTtcblxuICBASW5wdXQoKSB3aWR0aDogc3RyaW5nO1xuXG4gIEBJbnB1dCgpIGhlaWdodDogc3RyaW5nO1xuXG4gIEBJbnB1dCgpIHJlc3BvbnNpdmUgPSB0cnVlO1xuXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW9uLXByZWZpeFxuICBAT3V0cHV0KCkgcmVhZG9ubHkgb25EYXRhU2VsZWN0OiBFdmVudEVtaXR0ZXI8YW55PiA9IG5ldyBFdmVudEVtaXR0ZXIoKTtcblxuICBAT3V0cHV0KCkgcmVhZG9ubHkgaW5pdGlhbGl6ZWQgPSBuZXcgQmVoYXZpb3JTdWJqZWN0KHRoaXMpO1xuXG4gIHByaXZhdGUgX2luaXRpYWxpemVkOiBib29sZWFuO1xuXG4gIF9kYXRhOiBhbnk7XG5cbiAgY2hhcnQ6IGFueTtcblxuICBjb25zdHJ1Y3RvcihwdWJsaWMgZWw6IEVsZW1lbnRSZWYsIHByaXZhdGUgY2RSZWY6IENoYW5nZURldGVjdG9yUmVmKSB7fVxuXG4gIEBJbnB1dCgpIGdldCBkYXRhKCk6IGFueSB7XG4gICAgcmV0dXJuIHRoaXMuX2RhdGE7XG4gIH1cblxuICBzZXQgZGF0YSh2YWw6IGFueSkge1xuICAgIHRoaXMuX2RhdGEgPSB2YWw7XG4gICAgdGhpcy5yZWluaXQoKTtcbiAgfVxuXG4gIGdldCBjYW52YXMoKSB7XG4gICAgcmV0dXJuIHRoaXMuZWwubmF0aXZlRWxlbWVudC5jaGlsZHJlblswXS5jaGlsZHJlblswXTtcbiAgfVxuXG4gIGdldCBiYXNlNjRJbWFnZSgpIHtcbiAgICByZXR1cm4gdGhpcy5jaGFydC50b0Jhc2U2NEltYWdlKCk7XG4gIH1cblxuICBuZ0FmdGVyVmlld0luaXQoKSB7XG4gICAgY2hhcnRKc0xvYWRlZCQuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgIHRyeSB7XG4gICAgICAgIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tdW51c2VkLWV4cHJlc3Npb25cbiAgICAgICAgQ2hhcnQ7XG4gICAgICB9IGNhdGNoIChlcnJvcikge1xuICAgICAgICBjb25zb2xlLmVycm9yKGBDaGFydCBpcyBub3QgZm91bmQuIEltcG9ydCB0aGUgQ2hhcnQgZnJvbSBhcHAubW9kdWxlIGxpa2Ugc2hvd24gYmVsb3c6XG4gICAgICAgIGltcG9ydCgnY2hhcnQuanMnKTtcbiAgICAgICAgYCk7XG4gICAgICAgIHJldHVybjtcbiAgICAgIH1cblxuICAgICAgdGhpcy5pbml0Q2hhcnQoKTtcbiAgICAgIHRoaXMuX2luaXRpYWxpemVkID0gdHJ1ZTtcbiAgICB9KTtcbiAgfVxuXG4gIG9uQ2FudmFzQ2xpY2sgPSBldmVudCA9PiB7XG4gICAgaWYgKHRoaXMuY2hhcnQpIHtcbiAgICAgIGNvbnN0IGVsZW1lbnQgPSB0aGlzLmNoYXJ0LmdldEVsZW1lbnRBdEV2ZW50KGV2ZW50KTtcbiAgICAgIGNvbnN0IGRhdGFzZXQgPSB0aGlzLmNoYXJ0LmdldERhdGFzZXRBdEV2ZW50KGV2ZW50KTtcbiAgICAgIGlmIChlbGVtZW50ICYmIGVsZW1lbnRbMF0gJiYgZGF0YXNldCkge1xuICAgICAgICB0aGlzLm9uRGF0YVNlbGVjdC5lbWl0KHtcbiAgICAgICAgICBvcmlnaW5hbEV2ZW50OiBldmVudCxcbiAgICAgICAgICBlbGVtZW50OiBlbGVtZW50WzBdLFxuICAgICAgICAgIGRhdGFzZXRcbiAgICAgICAgfSk7XG4gICAgICB9XG4gICAgfVxuICB9O1xuXG4gIGluaXRDaGFydCA9ICgpID0+IHtcbiAgICBjb25zdCBvcHRzID0gdGhpcy5vcHRpb25zIHx8IHt9O1xuICAgIG9wdHMucmVzcG9uc2l2ZSA9IHRoaXMucmVzcG9uc2l2ZTtcblxuICAgIC8vIGFsbG93cyBjaGFydCB0byByZXNpemUgaW4gcmVzcG9uc2l2ZSBtb2RlXG4gICAgaWYgKG9wdHMucmVzcG9uc2l2ZSAmJiAodGhpcy5oZWlnaHQgfHwgdGhpcy53aWR0aCkpIHtcbiAgICAgIG9wdHMubWFpbnRhaW5Bc3BlY3RSYXRpbyA9IGZhbHNlO1xuICAgIH1cblxuICAgIHRoaXMuY2hhcnQgPSBuZXcgQ2hhcnQodGhpcy5lbC5uYXRpdmVFbGVtZW50LmNoaWxkcmVuWzBdLmNoaWxkcmVuWzBdLCB7XG4gICAgICB0eXBlOiB0aGlzLnR5cGUsXG4gICAgICBkYXRhOiB0aGlzLmRhdGEsXG4gICAgICBvcHRpb25zOiB0aGlzLm9wdGlvbnMsXG4gICAgICBwbHVnaW5zOiB0aGlzLnBsdWdpbnNcbiAgICB9KTtcblxuICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xuICB9O1xuXG4gIGdlbmVyYXRlTGVnZW5kID0gKCkgPT4ge1xuICAgIGlmICh0aGlzLmNoYXJ0KSB7XG4gICAgICByZXR1cm4gdGhpcy5jaGFydC5nZW5lcmF0ZUxlZ2VuZCgpO1xuICAgIH1cbiAgfTtcblxuICByZWZyZXNoID0gKCkgPT4ge1xuICAgIGlmICh0aGlzLmNoYXJ0KSB7XG4gICAgICB0aGlzLmNoYXJ0LnVwZGF0ZSgpO1xuICAgICAgdGhpcy5jZFJlZi5kZXRlY3RDaGFuZ2VzKCk7XG4gICAgfVxuICB9O1xuXG4gIHJlaW5pdCA9ICgpID0+IHtcbiAgICBpZiAodGhpcy5jaGFydCkge1xuICAgICAgdGhpcy5jaGFydC5kZXN0cm95KCk7XG4gICAgICB0aGlzLmluaXRDaGFydCgpO1xuICAgIH1cbiAgfTtcblxuICBuZ09uRGVzdHJveSgpIHtcbiAgICBpZiAodGhpcy5jaGFydCkge1xuICAgICAgdGhpcy5jaGFydC5kZXN0cm95KCk7XG4gICAgICB0aGlzLl9pbml0aWFsaXplZCA9IGZhbHNlO1xuICAgICAgdGhpcy5jaGFydCA9IG51bGw7XG4gICAgfVxuICB9XG59XG4iXX0=
