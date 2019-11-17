/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/chart/chart.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, ElementRef, EventEmitter, Input, Output, ChangeDetectorRef, } from '@angular/core';
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
                if (element && element.length && dataset) {
                    _this.onDataSelect.emit({
                        originalEvent: event,
                        element: element[0],
                        dataset: dataset,
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
            _this.chart = new Chart(_this.canvas, {
                type: _this.type,
                data: _this.data,
                options: _this.options,
                plugins: _this.plugins,
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
            _this.testChartJs();
            _this.initChart();
            _this._initialized = true;
        }));
    };
    /**
     * @return {?}
     */
    ChartComponent.prototype.testChartJs = /**
     * @return {?}
     */
    function () {
        try {
            // tslint:disable-next-line: no-unused-expression
            Chart;
        }
        catch (error) {
            throw new Error("Chart is not found. Import the Chart from app.module like shown below:\n      import('chart.js');\n      ");
        }
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhcnQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9jaGFydC9jaGFydC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBRUwsU0FBUyxFQUNULFVBQVUsRUFDVixZQUFZLEVBQ1osS0FBSyxFQUVMLE1BQU0sRUFDTixpQkFBaUIsR0FDbEIsTUFBTSxlQUFlLENBQUM7QUFDdkIsT0FBTyxFQUFFLGVBQWUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUN2QyxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sMEJBQTBCLENBQUM7QUFHMUQ7SUE0QkUsd0JBQW1CLEVBQWMsRUFBVSxLQUF3QjtRQUFuRSxpQkFBdUU7UUFBcEQsT0FBRSxHQUFGLEVBQUUsQ0FBWTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQW1CO1FBckIxRCxZQUFPLEdBQVEsRUFBRSxDQUFDO1FBRWxCLFlBQU8sR0FBVSxFQUFFLENBQUM7UUFNcEIsZUFBVSxHQUFHLElBQUksQ0FBQzs7UUFHUixpQkFBWSxHQUFzQixJQUFJLFlBQVksRUFBRSxDQUFDO1FBRXJELGdCQUFXLEdBQUcsSUFBSSxlQUFlLENBQUMsSUFBSSxDQUFDLENBQUM7UUErQzNELGtCQUFhOzs7O1FBQUcsVUFBQSxLQUFLO1lBQ25CLElBQUksS0FBSSxDQUFDLEtBQUssRUFBRTs7b0JBQ1IsT0FBTyxHQUFHLEtBQUksQ0FBQyxLQUFLLENBQUMsaUJBQWlCLENBQUMsS0FBSyxDQUFDOztvQkFDN0MsT0FBTyxHQUFHLEtBQUksQ0FBQyxLQUFLLENBQUMsaUJBQWlCLENBQUMsS0FBSyxDQUFDO2dCQUNuRCxJQUFJLE9BQU8sSUFBSSxPQUFPLENBQUMsTUFBTSxJQUFJLE9BQU8sRUFBRTtvQkFDeEMsS0FBSSxDQUFDLFlBQVksQ0FBQyxJQUFJLENBQUM7d0JBQ3JCLGFBQWEsRUFBRSxLQUFLO3dC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QyxLQUFLLENBQUMsT0FBTyxFQUFFLENBQUM7WUFDckIsSUFBSSxDQUFDLFlBQVksR0FBRyxLQUFLLENBQUM7WUFDMUIsSUFBSSxDQUFDLEtBQUssR0FBRyxJQUFJLENBQUM7U0FDbkI7SUFDSCxDQUFDOztnQkE5SEYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxXQUFXO29CQUNyQiw4WEFBcUM7aUJBQ3RDOzs7O2dCQWRDLFVBQVU7Z0JBS1YsaUJBQWlCOzs7dUJBV2hCLEtBQUs7MEJBRUwsS0FBSzswQkFFTCxLQUFLO3dCQUVMLEtBQUs7eUJBRUwsS0FBSzs2QkFFTCxLQUFLOytCQUdMLE1BQU07OEJBRU4sTUFBTTt1QkFVTixLQUFLOztJQWlHUixxQkFBQztDQUFBLEFBL0hELElBK0hDO1NBM0hZLGNBQWM7OztJQUN6Qiw4QkFBc0I7O0lBRXRCLGlDQUEyQjs7SUFFM0IsaUNBQTZCOztJQUU3QiwrQkFBdUI7O0lBRXZCLGdDQUF3Qjs7SUFFeEIsb0NBQTJCOztJQUczQixzQ0FBd0U7O0lBRXhFLHFDQUEyRDs7Ozs7SUFFM0Qsc0NBQThCOztJQUU5QiwrQkFBVzs7SUFFWCwrQkFBVzs7SUF5Q1gsdUNBWUU7O0lBRUYsbUNBaUJFOztJQUVGLHdDQUlFOztJQUVGLGlDQUtFOztJQUVGLGdDQUtFOztJQTFGVSw0QkFBcUI7Ozs7O0lBQUUsK0JBQWdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcclxuICBBZnRlclZpZXdJbml0LFxyXG4gIENvbXBvbmVudCxcclxuICBFbGVtZW50UmVmLFxyXG4gIEV2ZW50RW1pdHRlcixcclxuICBJbnB1dCxcclxuICBPbkRlc3Ryb3ksXHJcbiAgT3V0cHV0LFxyXG4gIENoYW5nZURldGVjdG9yUmVmLFxyXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBCZWhhdmlvclN1YmplY3QgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgY2hhcnRKc0xvYWRlZCQgfSBmcm9tICcuLi8uLi91dGlscy93aWRnZXQtdXRpbHMnO1xyXG5kZWNsYXJlIGNvbnN0IENoYXJ0OiBhbnk7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1jaGFydCcsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL2NoYXJ0LmNvbXBvbmVudC5odG1sJyxcclxufSlcclxuZXhwb3J0IGNsYXNzIENoYXJ0Q29tcG9uZW50IGltcGxlbWVudHMgQWZ0ZXJWaWV3SW5pdCwgT25EZXN0cm95IHtcclxuICBASW5wdXQoKSB0eXBlOiBzdHJpbmc7XHJcblxyXG4gIEBJbnB1dCgpIG9wdGlvbnM6IGFueSA9IHt9O1xyXG5cclxuICBASW5wdXQoKSBwbHVnaW5zOiBhbnlbXSA9IFtdO1xyXG5cclxuICBASW5wdXQoKSB3aWR0aDogc3RyaW5nO1xyXG5cclxuICBASW5wdXQoKSBoZWlnaHQ6IHN0cmluZztcclxuXHJcbiAgQElucHV0KCkgcmVzcG9uc2l2ZSA9IHRydWU7XHJcblxyXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW9uLXByZWZpeFxyXG4gIEBPdXRwdXQoKSByZWFkb25seSBvbkRhdGFTZWxlY3Q6IEV2ZW50RW1pdHRlcjxhbnk+ID0gbmV3IEV2ZW50RW1pdHRlcigpO1xyXG5cclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgaW5pdGlhbGl6ZWQgPSBuZXcgQmVoYXZpb3JTdWJqZWN0KHRoaXMpO1xyXG5cclxuICBwcml2YXRlIF9pbml0aWFsaXplZDogYm9vbGVhbjtcclxuXHJcbiAgX2RhdGE6IGFueTtcclxuXHJcbiAgY2hhcnQ6IGFueTtcclxuXHJcbiAgY29uc3RydWN0b3IocHVibGljIGVsOiBFbGVtZW50UmVmLCBwcml2YXRlIGNkUmVmOiBDaGFuZ2VEZXRlY3RvclJlZikge31cclxuXHJcbiAgQElucHV0KCkgZ2V0IGRhdGEoKTogYW55IHtcclxuICAgIHJldHVybiB0aGlzLl9kYXRhO1xyXG4gIH1cclxuXHJcbiAgc2V0IGRhdGEodmFsOiBhbnkpIHtcclxuICAgIHRoaXMuX2RhdGEgPSB2YWw7XHJcbiAgICB0aGlzLnJlaW5pdCgpO1xyXG4gIH1cclxuXHJcbiAgZ2V0IGNhbnZhcygpIHtcclxuICAgIHJldHVybiB0aGlzLmVsLm5hdGl2ZUVsZW1lbnQuY2hpbGRyZW5bMF0uY2hpbGRyZW5bMF07XHJcbiAgfVxyXG5cclxuICBnZXQgYmFzZTY0SW1hZ2UoKSB7XHJcbiAgICByZXR1cm4gdGhpcy5jaGFydC50b0Jhc2U2NEltYWdlKCk7XHJcbiAgfVxyXG5cclxuICBuZ0FmdGVyVmlld0luaXQoKSB7XHJcbiAgICBjaGFydEpzTG9hZGVkJC5zdWJzY3JpYmUoKCkgPT4ge1xyXG4gICAgICB0aGlzLnRlc3RDaGFydEpzKCk7XHJcblxyXG4gICAgICB0aGlzLmluaXRDaGFydCgpO1xyXG4gICAgICB0aGlzLl9pbml0aWFsaXplZCA9IHRydWU7XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIHRlc3RDaGFydEpzKCkge1xyXG4gICAgdHJ5IHtcclxuICAgICAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby11bnVzZWQtZXhwcmVzc2lvblxyXG4gICAgICBDaGFydDtcclxuICAgIH0gY2F0Y2ggKGVycm9yKSB7XHJcbiAgICAgIHRocm93IG5ldyBFcnJvcihgQ2hhcnQgaXMgbm90IGZvdW5kLiBJbXBvcnQgdGhlIENoYXJ0IGZyb20gYXBwLm1vZHVsZSBsaWtlIHNob3duIGJlbG93OlxyXG4gICAgICBpbXBvcnQoJ2NoYXJ0LmpzJyk7XHJcbiAgICAgIGApO1xyXG4gICAgfVxyXG4gIH1cclxuXHJcbiAgb25DYW52YXNDbGljayA9IGV2ZW50ID0+IHtcclxuICAgIGlmICh0aGlzLmNoYXJ0KSB7XHJcbiAgICAgIGNvbnN0IGVsZW1lbnQgPSB0aGlzLmNoYXJ0LmdldEVsZW1lbnRBdEV2ZW50KGV2ZW50KTtcclxuICAgICAgY29uc3QgZGF0YXNldCA9IHRoaXMuY2hhcnQuZ2V0RGF0YXNldEF0RXZlbnQoZXZlbnQpO1xyXG4gICAgICBpZiAoZWxlbWVudCAmJiBlbGVtZW50Lmxlbmd0aCAmJiBkYXRhc2V0KSB7XHJcbiAgICAgICAgdGhpcy5vbkRhdGFTZWxlY3QuZW1pdCh7XHJcbiAgICAgICAgICBvcmlnaW5hbEV2ZW50OiBldmVudCxcclxuICAgICAgICAgIGVsZW1lbnQ6IGVsZW1lbnRbMF0sXHJcbiAgICAgICAgICBkYXRhc2V0LFxyXG4gICAgICAgIH0pO1xyXG4gICAgICB9XHJcbiAgICB9XHJcbiAgfTtcclxuXHJcbiAgaW5pdENoYXJ0ID0gKCkgPT4ge1xyXG4gICAgY29uc3Qgb3B0cyA9IHRoaXMub3B0aW9ucyB8fCB7fTtcclxuICAgIG9wdHMucmVzcG9uc2l2ZSA9IHRoaXMucmVzcG9uc2l2ZTtcclxuXHJcbiAgICAvLyBhbGxvd3MgY2hhcnQgdG8gcmVzaXplIGluIHJlc3BvbnNpdmUgbW9kZVxyXG4gICAgaWYgKG9wdHMucmVzcG9uc2l2ZSAmJiAodGhpcy5oZWlnaHQgfHwgdGhpcy53aWR0aCkpIHtcclxuICAgICAgb3B0cy5tYWludGFpbkFzcGVjdFJhdGlvID0gZmFsc2U7XHJcbiAgICB9XHJcblxyXG4gICAgdGhpcy5jaGFydCA9IG5ldyBDaGFydCh0aGlzLmNhbnZhcywge1xyXG4gICAgICB0eXBlOiB0aGlzLnR5cGUsXHJcbiAgICAgIGRhdGE6IHRoaXMuZGF0YSxcclxuICAgICAgb3B0aW9uczogdGhpcy5vcHRpb25zLFxyXG4gICAgICBwbHVnaW5zOiB0aGlzLnBsdWdpbnMsXHJcbiAgICB9KTtcclxuXHJcbiAgICB0aGlzLmNkUmVmLmRldGVjdENoYW5nZXMoKTtcclxuICB9O1xyXG5cclxuICBnZW5lcmF0ZUxlZ2VuZCA9ICgpID0+IHtcclxuICAgIGlmICh0aGlzLmNoYXJ0KSB7XHJcbiAgICAgIHJldHVybiB0aGlzLmNoYXJ0LmdlbmVyYXRlTGVnZW5kKCk7XHJcbiAgICB9XHJcbiAgfTtcclxuXHJcbiAgcmVmcmVzaCA9ICgpID0+IHtcclxuICAgIGlmICh0aGlzLmNoYXJ0KSB7XHJcbiAgICAgIHRoaXMuY2hhcnQudXBkYXRlKCk7XHJcbiAgICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xyXG4gICAgfVxyXG4gIH07XHJcblxyXG4gIHJlaW5pdCA9ICgpID0+IHtcclxuICAgIGlmICh0aGlzLmNoYXJ0KSB7XHJcbiAgICAgIHRoaXMuY2hhcnQuZGVzdHJveSgpO1xyXG4gICAgICB0aGlzLmluaXRDaGFydCgpO1xyXG4gICAgfVxyXG4gIH07XHJcblxyXG4gIG5nT25EZXN0cm95KCkge1xyXG4gICAgaWYgKHRoaXMuY2hhcnQpIHtcclxuICAgICAgdGhpcy5jaGFydC5kZXN0cm95KCk7XHJcbiAgICAgIHRoaXMuX2luaXRpYWxpemVkID0gZmFsc2U7XHJcbiAgICAgIHRoaXMuY2hhcnQgPSBudWxsO1xyXG4gICAgfVxyXG4gIH1cclxufVxyXG4iXX0=