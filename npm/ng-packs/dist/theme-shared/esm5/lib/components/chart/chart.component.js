/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, ElementRef, EventEmitter, Input, Output, ChangeDetectorRef } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { chartJsLoaded$ } from '../../utils/widget-utils';
var ChartComponent = /** @class */ (function () {
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
        this.onCanvasClick = (/**
         * @param {?} event
         * @return {?}
         */
        function (event) {
            if (_this.chart) {
                /** @type {?} */
                var element = _this.chart.getElementAtEvent(event);
                /** @type {?} */
                var dataset = _this.chart.getDatasetAtEvent(event);
                if (element && element[0] && dataset) {
                    _this.onDataSelect.emit({
                        originalEvent: event,
                        element: element[0],
                        dataset: dataset
                    });
                }
            }
        });
        this.initChart = (/**
         * @return {?}
         */
        function () {
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
                plugins: _this.plugins
            });
            _this.cdRef.detectChanges();
        });
        this.generateLegend = (/**
         * @return {?}
         */
        function () {
            if (_this.chart) {
                return _this.chart.generateLegend();
            }
        });
        this.refresh = (/**
         * @return {?}
         */
        function () {
            if (_this.chart) {
                _this.chart.update();
                _this.cdRef.detectChanges();
            }
        });
        this.reinit = (/**
         * @return {?}
         */
        function () {
            if (_this.chart) {
                _this.chart.destroy();
                _this.initChart();
            }
        });
    }
    Object.defineProperty(ChartComponent.prototype, "data", {
        get: /**
         * @return {?}
         */
        function () {
            return this._data;
        },
        set: /**
         * @param {?} val
         * @return {?}
         */
        function (val) {
            this._data = val;
            this.reinit();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChartComponent.prototype, "canvas", {
        get: /**
         * @return {?}
         */
        function () {
            return this.el.nativeElement.children[0].children[0];
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChartComponent.prototype, "base64Image", {
        get: /**
         * @return {?}
         */
        function () {
            return this.chart.toBase64Image();
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @return {?}
     */
    ChartComponent.prototype.ngAfterViewInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        chartJsLoaded$.subscribe((/**
         * @return {?}
         */
        function () {
            try {
                // tslint:disable-next-line: no-unused-expression
                Chart;
            }
            catch (error) {
                console.error("Chart is not found. Import the Chart from app.module like shown below:\n        import('chart.js');\n        ");
                return;
            }
            _this.initChart();
            _this._initialized = true;
        }));
    };
    /**
     * @return {?}
     */
    ChartComponent.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () {
        if (this.chart) {
            this.chart.destroy();
            this._initialized = false;
            this.chart = null;
        }
    };
    ChartComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-chart',
                    template: "<div\r\n  style=\"position:relative\"\r\n  [style.width]=\"responsive && !width ? null : width\"\r\n  [style.height]=\"responsive && !height ? null : height\"\r\n>\r\n  <canvas\r\n    [attr.width]=\"responsive && !width ? null : width\"\r\n    [attr.height]=\"responsive && !height ? null : height\"\r\n    (click)=\"onCanvasClick($event)\"\r\n  ></canvas>\r\n</div>\r\n"
                }] }
    ];
    /** @nocollapse */
    ChartComponent.ctorParameters = function () { return [
        { type: ElementRef },
        { type: ChangeDetectorRef }
    ]; };
    ChartComponent.propDecorators = {
        type: [{ type: Input }],
        options: [{ type: Input }],
        plugins: [{ type: Input }],
        width: [{ type: Input }],
        height: [{ type: Input }],
        responsive: [{ type: Input }],
        onDataSelect: [{ type: Output }],
        initialized: [{ type: Output }],
        data: [{ type: Input }]
    };
    return ChartComponent;
}());
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhcnQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9jaGFydC9jaGFydC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFFTCxTQUFTLEVBQ1QsVUFBVSxFQUNWLFlBQVksRUFDWixLQUFLLEVBRUwsTUFBTSxFQUNOLGlCQUFpQixFQUNsQixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ3ZDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSwwQkFBMEIsQ0FBQztBQUcxRDtJQTRCRSx3QkFBbUIsRUFBYyxFQUFVLEtBQXdCO1FBQW5FLGlCQUF1RTtRQUFwRCxPQUFFLEdBQUYsRUFBRSxDQUFZO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBbUI7UUFyQjFELFlBQU8sR0FBUSxFQUFFLENBQUM7UUFFbEIsWUFBTyxHQUFVLEVBQUUsQ0FBQztRQU1wQixlQUFVLEdBQUcsSUFBSSxDQUFDOztRQUdSLGlCQUFZLEdBQXNCLElBQUksWUFBWSxFQUFFLENBQUM7UUFFckQsZ0JBQVcsR0FBRyxJQUFJLGVBQWUsQ0FBQyxJQUFJLENBQUMsQ0FBQztRQTRDM0Qsa0JBQWE7Ozs7UUFBRyxVQUFBLEtBQUs7WUFDbkIsSUFBSSxLQUFJLENBQUMsS0FBSyxFQUFFOztvQkFDUixPQUFPLEdBQUcsS0FBSSxDQUFDLEtBQUssQ0FBQyxpQkFBaUIsQ0FBQyxLQUFLLENBQUM7O29CQUM3QyxPQUFPLEdBQUcsS0FBSSxDQUFDLEtBQUssQ0FBQyxpQkFBaUIsQ0FBQyxLQUFLLENBQUM7Z0JBQ25ELElBQUksT0FBTyxJQUFJLE9BQU8sQ0FBQyxDQUFDLENBQUMsSUFBSSxPQUFPLEVBQUU7b0JBQ3BDLEtBQUksQ0FBQyxZQUFZLENBQUMsSUFBSSxDQUFDO3dCQUNyQixhQUFhLEVBQUUsS0FB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QUU7WUFDZCxJQUFJLENBQUMsS0FBSyxDQUFDLE9BQU8sRUFBRSxDQUFDO1lBQ3JCLElBQUksQ0FBQyxZQUFZLEdBQUcsS0FBSyxDQUFDO1lBQzFCLElBQUksQ0FBQyxLQUFLLEdBQUcsSUFBSSxDQUFDO1NBQ25CO0lBQ0gsQ0FBQzs7Z0JBM0hGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsV0FBVztvQkFDckIsOFhBQXFDO2lCQUN0Qzs7OztnQkFkQyxVQUFVO2dCQUtWLGlCQUFpQjs7O3VCQVdoQixLQUFLOzBCQUVMLEtBQUs7MEJBRUwsS0FBSzt3QkFFTCxLQUFLO3lCQUVMLEtBQUs7NkJBRUwsS0FBSzsrQkFHTCxNQUFNOzhCQUVOLE1BQU07dUJBVU4sS0FBSzs7SUE4RlIscUJBQUM7Q0FBQSxBQTVIRCxJQTRIQztTQXhIWSxjQUFjOzs7SUFDekIsOEJBQXNCOztJQUV0QixpQ0FBMkI7O0lBRTNCLGlDQUE2Qjs7SUFFN0IsK0JBQXVCOztJQUV2QixnQ0FBd0I7O0lBRXhCLG9DQUEyQjs7SUFHM0Isc0NBQXdFOztJQUV4RSxxQ0FBMkQ7Ozs7O0lBRTNELHNDQUE4Qjs7SUFFOUIsK0JBQVc7O0lBRVgsK0JBQVc7O0lBc0NYLHVDQVlFOztJQUVGLG1DQWlCRTs7SUFFRix3Q0FJRTs7SUFFRixpQ0FLRTs7SUFFRixnQ0FLRTs7SUF2RlUsNEJBQXFCOzs7OztJQUFFLCtCQUFnQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7XHJcbiAgQWZ0ZXJWaWV3SW5pdCxcclxuICBDb21wb25lbnQsXHJcbiAgRWxlbWVudFJlZixcclxuICBFdmVudEVtaXR0ZXIsXHJcbiAgSW5wdXQsXHJcbiAgT25EZXN0cm95LFxyXG4gIE91dHB1dCxcclxuICBDaGFuZ2VEZXRlY3RvclJlZlxyXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBCZWhhdmlvclN1YmplY3QgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgY2hhcnRKc0xvYWRlZCQgfSBmcm9tICcuLi8uLi91dGlscy93aWRnZXQtdXRpbHMnO1xyXG5kZWNsYXJlIGNvbnN0IENoYXJ0OiBhbnk7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1jaGFydCcsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL2NoYXJ0LmNvbXBvbmVudC5odG1sJ1xyXG59KVxyXG5leHBvcnQgY2xhc3MgQ2hhcnRDb21wb25lbnQgaW1wbGVtZW50cyBBZnRlclZpZXdJbml0LCBPbkRlc3Ryb3kge1xyXG4gIEBJbnB1dCgpIHR5cGU6IHN0cmluZztcclxuXHJcbiAgQElucHV0KCkgb3B0aW9uczogYW55ID0ge307XHJcblxyXG4gIEBJbnB1dCgpIHBsdWdpbnM6IGFueVtdID0gW107XHJcblxyXG4gIEBJbnB1dCgpIHdpZHRoOiBzdHJpbmc7XHJcblxyXG4gIEBJbnB1dCgpIGhlaWdodDogc3RyaW5nO1xyXG5cclxuICBASW5wdXQoKSByZXNwb25zaXZlID0gdHJ1ZTtcclxuXHJcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby1vdXRwdXQtb24tcHJlZml4XHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IG9uRGF0YVNlbGVjdDogRXZlbnRFbWl0dGVyPGFueT4gPSBuZXcgRXZlbnRFbWl0dGVyKCk7XHJcblxyXG4gIEBPdXRwdXQoKSByZWFkb25seSBpbml0aWFsaXplZCA9IG5ldyBCZWhhdmlvclN1YmplY3QodGhpcyk7XHJcblxyXG4gIHByaXZhdGUgX2luaXRpYWxpemVkOiBib29sZWFuO1xyXG5cclxuICBfZGF0YTogYW55O1xyXG5cclxuICBjaGFydDogYW55O1xyXG5cclxuICBjb25zdHJ1Y3RvcihwdWJsaWMgZWw6IEVsZW1lbnRSZWYsIHByaXZhdGUgY2RSZWY6IENoYW5nZURldGVjdG9yUmVmKSB7fVxyXG5cclxuICBASW5wdXQoKSBnZXQgZGF0YSgpOiBhbnkge1xyXG4gICAgcmV0dXJuIHRoaXMuX2RhdGE7XHJcbiAgfVxyXG5cclxuICBzZXQgZGF0YSh2YWw6IGFueSkge1xyXG4gICAgdGhpcy5fZGF0YSA9IHZhbDtcclxuICAgIHRoaXMucmVpbml0KCk7XHJcbiAgfVxyXG5cclxuICBnZXQgY2FudmFzKCkge1xyXG4gICAgcmV0dXJuIHRoaXMuZWwubmF0aXZlRWxlbWVudC5jaGlsZHJlblswXS5jaGlsZHJlblswXTtcclxuICB9XHJcblxyXG4gIGdldCBiYXNlNjRJbWFnZSgpIHtcclxuICAgIHJldHVybiB0aGlzLmNoYXJ0LnRvQmFzZTY0SW1hZ2UoKTtcclxuICB9XHJcblxyXG4gIG5nQWZ0ZXJWaWV3SW5pdCgpIHtcclxuICAgIGNoYXJ0SnNMb2FkZWQkLnN1YnNjcmliZSgoKSA9PiB7XHJcbiAgICAgIHRyeSB7XHJcbiAgICAgICAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby11bnVzZWQtZXhwcmVzc2lvblxyXG4gICAgICAgIENoYXJ0O1xyXG4gICAgICB9IGNhdGNoIChlcnJvcikge1xyXG4gICAgICAgIGNvbnNvbGUuZXJyb3IoYENoYXJ0IGlzIG5vdCBmb3VuZC4gSW1wb3J0IHRoZSBDaGFydCBmcm9tIGFwcC5tb2R1bGUgbGlrZSBzaG93biBiZWxvdzpcclxuICAgICAgICBpbXBvcnQoJ2NoYXJ0LmpzJyk7XHJcbiAgICAgICAgYCk7XHJcbiAgICAgICAgcmV0dXJuO1xyXG4gICAgICB9XHJcblxyXG4gICAgICB0aGlzLmluaXRDaGFydCgpO1xyXG4gICAgICB0aGlzLl9pbml0aWFsaXplZCA9IHRydWU7XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIG9uQ2FudmFzQ2xpY2sgPSBldmVudCA9PiB7XHJcbiAgICBpZiAodGhpcy5jaGFydCkge1xyXG4gICAgICBjb25zdCBlbGVtZW50ID0gdGhpcy5jaGFydC5nZXRFbGVtZW50QXRFdmVudChldmVudCk7XHJcbiAgICAgIGNvbnN0IGRhdGFzZXQgPSB0aGlzLmNoYXJ0LmdldERhdGFzZXRBdEV2ZW50KGV2ZW50KTtcclxuICAgICAgaWYgKGVsZW1lbnQgJiYgZWxlbWVudFswXSAmJiBkYXRhc2V0KSB7XHJcbiAgICAgICAgdGhpcy5vbkRhdGFTZWxlY3QuZW1pdCh7XHJcbiAgICAgICAgICBvcmlnaW5hbEV2ZW50OiBldmVudCxcclxuICAgICAgICAgIGVsZW1lbnQ6IGVsZW1lbnRbMF0sXHJcbiAgICAgICAgICBkYXRhc2V0XHJcbiAgICAgICAgfSk7XHJcbiAgICAgIH1cclxuICAgIH1cclxuICB9O1xyXG5cclxuICBpbml0Q2hhcnQgPSAoKSA9PiB7XHJcbiAgICBjb25zdCBvcHRzID0gdGhpcy5vcHRpb25zIHx8IHt9O1xyXG4gICAgb3B0cy5yZXNwb25zaXZlID0gdGhpcy5yZXNwb25zaXZlO1xyXG5cclxuICAgIC8vIGFsbG93cyBjaGFydCB0byByZXNpemUgaW4gcmVzcG9uc2l2ZSBtb2RlXHJcbiAgICBpZiAob3B0cy5yZXNwb25zaXZlICYmICh0aGlzLmhlaWdodCB8fCB0aGlzLndpZHRoKSkge1xyXG4gICAgICBvcHRzLm1haW50YWluQXNwZWN0UmF0aW8gPSBmYWxzZTtcclxuICAgIH1cclxuXHJcbiAgICB0aGlzLmNoYXJ0ID0gbmV3IENoYXJ0KHRoaXMuZWwubmF0aXZlRWxlbWVudC5jaGlsZHJlblswXS5jaGlsZHJlblswXSwge1xyXG4gICAgICB0eXBlOiB0aGlzLnR5cGUsXHJcbiAgICAgIGRhdGE6IHRoaXMuZGF0YSxcclxuICAgICAgb3B0aW9uczogdGhpcy5vcHRpb25zLFxyXG4gICAgICBwbHVnaW5zOiB0aGlzLnBsdWdpbnNcclxuICAgIH0pO1xyXG5cclxuICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xyXG4gIH07XHJcblxyXG4gIGdlbmVyYXRlTGVnZW5kID0gKCkgPT4ge1xyXG4gICAgaWYgKHRoaXMuY2hhcnQpIHtcclxuICAgICAgcmV0dXJuIHRoaXMuY2hhcnQuZ2VuZXJhdGVMZWdlbmQoKTtcclxuICAgIH1cclxuICB9O1xyXG5cclxuICByZWZyZXNoID0gKCkgPT4ge1xyXG4gICAgaWYgKHRoaXMuY2hhcnQpIHtcclxuICAgICAgdGhpcy5jaGFydC51cGRhdGUoKTtcclxuICAgICAgdGhpcy5jZFJlZi5kZXRlY3RDaGFuZ2VzKCk7XHJcbiAgICB9XHJcbiAgfTtcclxuXHJcbiAgcmVpbml0ID0gKCkgPT4ge1xyXG4gICAgaWYgKHRoaXMuY2hhcnQpIHtcclxuICAgICAgdGhpcy5jaGFydC5kZXN0cm95KCk7XHJcbiAgICAgIHRoaXMuaW5pdENoYXJ0KCk7XHJcbiAgICB9XHJcbiAgfTtcclxuXHJcbiAgbmdPbkRlc3Ryb3koKSB7XHJcbiAgICBpZiAodGhpcy5jaGFydCkge1xyXG4gICAgICB0aGlzLmNoYXJ0LmRlc3Ryb3koKTtcclxuICAgICAgdGhpcy5faW5pdGlhbGl6ZWQgPSBmYWxzZTtcclxuICAgICAgdGhpcy5jaGFydCA9IG51bGw7XHJcbiAgICB9XHJcbiAgfVxyXG59XHJcbiJdfQ==