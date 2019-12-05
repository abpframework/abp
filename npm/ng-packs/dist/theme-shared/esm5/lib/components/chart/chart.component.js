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
                    template: "<div\n  style=\"position:relative\"\n  [style.width]=\"responsive && !width ? null : width\"\n  [style.height]=\"responsive && !height ? null : height\"\n>\n  <canvas\n    [attr.width]=\"responsive && !width ? null : width\"\n    [attr.height]=\"responsive && !height ? null : height\"\n    (click)=\"onCanvasClick($event)\"\n  ></canvas>\n</div>\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhcnQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9jaGFydC9jaGFydC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBRUwsU0FBUyxFQUNULFVBQVUsRUFDVixZQUFZLEVBQ1osS0FBSyxFQUVMLE1BQU0sRUFDTixpQkFBaUIsR0FDbEIsTUFBTSxlQUFlLENBQUM7QUFDdkIsT0FBTyxFQUFFLGVBQWUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUN2QyxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sMEJBQTBCLENBQUM7QUFHMUQ7SUE0QkUsd0JBQW1CLEVBQWMsRUFBVSxLQUF3QjtRQUFuRSxpQkFBdUU7UUFBcEQsT0FBRSxHQUFGLEVBQUUsQ0FBWTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQW1CO1FBckIxRCxZQUFPLEdBQVEsRUFBRSxDQUFDO1FBRWxCLFlBQU8sR0FBVSxFQUFFLENBQUM7UUFNcEIsZUFBVSxHQUFHLElBQUksQ0FBQzs7UUFHUixpQkFBWSxHQUFzQixJQUFJLFlBQVksRUFBRSxDQUFDO1FBRXJELGdCQUFXLEdBQUcsSUFBSSxlQUFlLENBQUMsSUFBSSxDQUFDLENBQUM7UUErQzNELGtCQUFhOzs7O1FBQUcsVUFBQSxLQUFLO1lBQ25CLElBQUksS0FBSSxDQUFDLEtBQUssRUFBRTs7b0JBQ1IsT0FBTyxHQUFHLEtBQUksQ0FBQyxLQUFLLENBQUMsaUJBQWlCLENBQUMsS0FBSyxDQUFDOztvQkFDN0MsT0FBTyxHQUFHLEtBQUksQ0FBQyxLQUFLLENBQUMsaUJBQWlCLENBQUMsS0FBSyxDQUFDO2dCQUNuRCxJQUFJLE9BQU8sSUFBSSxPQUFPLENBQUMsTUFBTSxJQUFJLE9BQU8sRUFBRTtvQkFDeEMsS0FBSSxDQUFDLFlBQVksQ0FBQyxJQUFJLENBQUM7d0JBQ3JCLGFBQWEsRUFBRSxLQUFLO3dC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QyxLQUFLLENBQUMsT0FBTyxFQUFFLENBQUM7WUFDckIsSUFBSSxDQUFDLFlBQVksR0FBRyxLQUFLLENBQUM7WUFDMUIsSUFBSSxDQUFDLEtBQUssR0FBRyxJQUFJLENBQUM7U0FDbkI7SUFDSCxDQUFDOztnQkE5SEYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxXQUFXO29CQUNyQix3V0FBcUM7aUJBQ3RDOzs7O2dCQWRDLFVBQVU7Z0JBS1YsaUJBQWlCOzs7dUJBV2hCLEtBQUs7MEJBRUwsS0FBSzswQkFFTCxLQUFLO3dCQUVMLEtBQUs7eUJBRUwsS0FBSzs2QkFFTCxLQUFLOytCQUdMLE1BQU07OEJBRU4sTUFBTTt1QkFVTixLQUFLOztJQWlHUixxQkFBQztDQUFBLEFBL0hELElBK0hDO1NBM0hZLGNBQWM7OztJQUN6Qiw4QkFBc0I7O0lBRXRCLGlDQUEyQjs7SUFFM0IsaUNBQTZCOztJQUU3QiwrQkFBdUI7O0lBRXZCLGdDQUF3Qjs7SUFFeEIsb0NBQTJCOztJQUczQixzQ0FBd0U7O0lBRXhFLHFDQUEyRDs7Ozs7SUFFM0Qsc0NBQThCOztJQUU5QiwrQkFBVzs7SUFFWCwrQkFBVzs7SUF5Q1gsdUNBWUU7O0lBRUYsbUNBaUJFOztJQUVGLHdDQUlFOztJQUVGLGlDQUtFOztJQUVGLGdDQUtFOztJQTFGVSw0QkFBcUI7Ozs7O0lBQUUsK0JBQWdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcbiAgQWZ0ZXJWaWV3SW5pdCxcbiAgQ29tcG9uZW50LFxuICBFbGVtZW50UmVmLFxuICBFdmVudEVtaXR0ZXIsXG4gIElucHV0LFxuICBPbkRlc3Ryb3ksXG4gIE91dHB1dCxcbiAgQ2hhbmdlRGV0ZWN0b3JSZWYsXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQmVoYXZpb3JTdWJqZWN0IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBjaGFydEpzTG9hZGVkJCB9IGZyb20gJy4uLy4uL3V0aWxzL3dpZGdldC11dGlscyc7XG5kZWNsYXJlIGNvbnN0IENoYXJ0OiBhbnk7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1jaGFydCcsXG4gIHRlbXBsYXRlVXJsOiAnLi9jaGFydC5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIENoYXJ0Q29tcG9uZW50IGltcGxlbWVudHMgQWZ0ZXJWaWV3SW5pdCwgT25EZXN0cm95IHtcbiAgQElucHV0KCkgdHlwZTogc3RyaW5nO1xuXG4gIEBJbnB1dCgpIG9wdGlvbnM6IGFueSA9IHt9O1xuXG4gIEBJbnB1dCgpIHBsdWdpbnM6IGFueVtdID0gW107XG5cbiAgQElucHV0KCkgd2lkdGg6IHN0cmluZztcblxuICBASW5wdXQoKSBoZWlnaHQ6IHN0cmluZztcblxuICBASW5wdXQoKSByZXNwb25zaXZlID0gdHJ1ZTtcblxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLW91dHB1dC1vbi1wcmVmaXhcbiAgQE91dHB1dCgpIHJlYWRvbmx5IG9uRGF0YVNlbGVjdDogRXZlbnRFbWl0dGVyPGFueT4gPSBuZXcgRXZlbnRFbWl0dGVyKCk7XG5cbiAgQE91dHB1dCgpIHJlYWRvbmx5IGluaXRpYWxpemVkID0gbmV3IEJlaGF2aW9yU3ViamVjdCh0aGlzKTtcblxuICBwcml2YXRlIF9pbml0aWFsaXplZDogYm9vbGVhbjtcblxuICBfZGF0YTogYW55O1xuXG4gIGNoYXJ0OiBhbnk7XG5cbiAgY29uc3RydWN0b3IocHVibGljIGVsOiBFbGVtZW50UmVmLCBwcml2YXRlIGNkUmVmOiBDaGFuZ2VEZXRlY3RvclJlZikge31cblxuICBASW5wdXQoKSBnZXQgZGF0YSgpOiBhbnkge1xuICAgIHJldHVybiB0aGlzLl9kYXRhO1xuICB9XG5cbiAgc2V0IGRhdGEodmFsOiBhbnkpIHtcbiAgICB0aGlzLl9kYXRhID0gdmFsO1xuICAgIHRoaXMucmVpbml0KCk7XG4gIH1cblxuICBnZXQgY2FudmFzKCkge1xuICAgIHJldHVybiB0aGlzLmVsLm5hdGl2ZUVsZW1lbnQuY2hpbGRyZW5bMF0uY2hpbGRyZW5bMF07XG4gIH1cblxuICBnZXQgYmFzZTY0SW1hZ2UoKSB7XG4gICAgcmV0dXJuIHRoaXMuY2hhcnQudG9CYXNlNjRJbWFnZSgpO1xuICB9XG5cbiAgbmdBZnRlclZpZXdJbml0KCkge1xuICAgIGNoYXJ0SnNMb2FkZWQkLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICB0aGlzLnRlc3RDaGFydEpzKCk7XG5cbiAgICAgIHRoaXMuaW5pdENoYXJ0KCk7XG4gICAgICB0aGlzLl9pbml0aWFsaXplZCA9IHRydWU7XG4gICAgfSk7XG4gIH1cblxuICB0ZXN0Q2hhcnRKcygpIHtcbiAgICB0cnkge1xuICAgICAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby11bnVzZWQtZXhwcmVzc2lvblxuICAgICAgQ2hhcnQ7XG4gICAgfSBjYXRjaCAoZXJyb3IpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcihgQ2hhcnQgaXMgbm90IGZvdW5kLiBJbXBvcnQgdGhlIENoYXJ0IGZyb20gYXBwLm1vZHVsZSBsaWtlIHNob3duIGJlbG93OlxuICAgICAgaW1wb3J0KCdjaGFydC5qcycpO1xuICAgICAgYCk7XG4gICAgfVxuICB9XG5cbiAgb25DYW52YXNDbGljayA9IGV2ZW50ID0+IHtcbiAgICBpZiAodGhpcy5jaGFydCkge1xuICAgICAgY29uc3QgZWxlbWVudCA9IHRoaXMuY2hhcnQuZ2V0RWxlbWVudEF0RXZlbnQoZXZlbnQpO1xuICAgICAgY29uc3QgZGF0YXNldCA9IHRoaXMuY2hhcnQuZ2V0RGF0YXNldEF0RXZlbnQoZXZlbnQpO1xuICAgICAgaWYgKGVsZW1lbnQgJiYgZWxlbWVudC5sZW5ndGggJiYgZGF0YXNldCkge1xuICAgICAgICB0aGlzLm9uRGF0YVNlbGVjdC5lbWl0KHtcbiAgICAgICAgICBvcmlnaW5hbEV2ZW50OiBldmVudCxcbiAgICAgICAgICBlbGVtZW50OiBlbGVtZW50WzBdLFxuICAgICAgICAgIGRhdGFzZXQsXG4gICAgICAgIH0pO1xuICAgICAgfVxuICAgIH1cbiAgfTtcblxuICBpbml0Q2hhcnQgPSAoKSA9PiB7XG4gICAgY29uc3Qgb3B0cyA9IHRoaXMub3B0aW9ucyB8fCB7fTtcbiAgICBvcHRzLnJlc3BvbnNpdmUgPSB0aGlzLnJlc3BvbnNpdmU7XG5cbiAgICAvLyBhbGxvd3MgY2hhcnQgdG8gcmVzaXplIGluIHJlc3BvbnNpdmUgbW9kZVxuICAgIGlmIChvcHRzLnJlc3BvbnNpdmUgJiYgKHRoaXMuaGVpZ2h0IHx8IHRoaXMud2lkdGgpKSB7XG4gICAgICBvcHRzLm1haW50YWluQXNwZWN0UmF0aW8gPSBmYWxzZTtcbiAgICB9XG5cbiAgICB0aGlzLmNoYXJ0ID0gbmV3IENoYXJ0KHRoaXMuY2FudmFzLCB7XG4gICAgICB0eXBlOiB0aGlzLnR5cGUsXG4gICAgICBkYXRhOiB0aGlzLmRhdGEsXG4gICAgICBvcHRpb25zOiB0aGlzLm9wdGlvbnMsXG4gICAgICBwbHVnaW5zOiB0aGlzLnBsdWdpbnMsXG4gICAgfSk7XG5cbiAgICB0aGlzLmNkUmVmLmRldGVjdENoYW5nZXMoKTtcbiAgfTtcblxuICBnZW5lcmF0ZUxlZ2VuZCA9ICgpID0+IHtcbiAgICBpZiAodGhpcy5jaGFydCkge1xuICAgICAgcmV0dXJuIHRoaXMuY2hhcnQuZ2VuZXJhdGVMZWdlbmQoKTtcbiAgICB9XG4gIH07XG5cbiAgcmVmcmVzaCA9ICgpID0+IHtcbiAgICBpZiAodGhpcy5jaGFydCkge1xuICAgICAgdGhpcy5jaGFydC51cGRhdGUoKTtcbiAgICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xuICAgIH1cbiAgfTtcblxuICByZWluaXQgPSAoKSA9PiB7XG4gICAgaWYgKHRoaXMuY2hhcnQpIHtcbiAgICAgIHRoaXMuY2hhcnQuZGVzdHJveSgpO1xuICAgICAgdGhpcy5pbml0Q2hhcnQoKTtcbiAgICB9XG4gIH07XG5cbiAgbmdPbkRlc3Ryb3koKSB7XG4gICAgaWYgKHRoaXMuY2hhcnQpIHtcbiAgICAgIHRoaXMuY2hhcnQuZGVzdHJveSgpO1xuICAgICAgdGhpcy5faW5pdGlhbGl6ZWQgPSBmYWxzZTtcbiAgICAgIHRoaXMuY2hhcnQgPSBudWxsO1xuICAgIH1cbiAgfVxufVxuIl19