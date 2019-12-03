/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhcnQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9jaGFydC9jaGFydC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFFTCxTQUFTLEVBQ1QsVUFBVSxFQUNWLFlBQVksRUFDWixLQUFLLEVBRUwsTUFBTSxFQUNOLGlCQUFpQixHQUNsQixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ3ZDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSwwQkFBMEIsQ0FBQztBQUcxRDtJQTRCRSx3QkFBbUIsRUFBYyxFQUFVLEtBQXdCO1FBQW5FLGlCQUF1RTtRQUFwRCxPQUFFLEdBQUYsRUFBRSxDQUFZO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBbUI7UUFyQjFELFlBQU8sR0FBUSxFQUFFLENBQUM7UUFFbEIsWUFBTyxHQUFVLEVBQUUsQ0FBQztRQU1wQixlQUFVLEdBQUcsSUFBSSxDQUFDOztRQUdSLGlCQUFZLEdBQXNCLElBQUksWUFBWSxFQUFFLENBQUM7UUFFckQsZ0JBQVcsR0FBRyxJQUFJLGVBQWUsQ0FBQyxJQUFJLENBQUMsQ0FBQztRQStDM0Qsa0JBQWE7Ozs7UUFBRyxVQUFBLEtBQUs7WUFDbkIsSUFBSSxLQUFJLENBQUMsS0FBSyxFQUFFOztvQkFDUixPQUFPLEdBQUcsS0FBSSxDQUFDLEtBQUssQ0FBQyxpQkFBaUIsQ0FBQyxLQUFLLENBQUM7O29CQUM3QyxPQUFPLEdBQUcsS0FBSSxDQUFDLEtBQUssQ0FBQyxpQkFBaUIsQ0FBQyxLQUFLLENBQUM7Z0JBQ25ELElBQUksT0FBTyxJQUFJLE9BQU8sQ0FBQyxNQUFNLElBQUksT0FBTyxFQUFFO29CQUN4QyxLQUFJLENBQUMsWUFBWSxDQUFDLElBQUksQ0FBQzt3QkFDckIsYUFBYSxFQUFFLEtBQUs7d0JB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LEtBQUssQ0FBQyxPQUFPLEVBQUUsQ0FBQztZQUNyQixJQUFJLENBQUMsWUFBWSxHQUFHLEtBQUssQ0FBQztZQUMxQixJQUFJLENBQUMsS0FBSyxHQUFHLElBQUksQ0FBQztTQUNuQjtJQUNILENBQUM7O2dCQTlIRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLFdBQVc7b0JBQ3JCLHdXQUFxQztpQkFDdEM7Ozs7Z0JBZEMsVUFBVTtnQkFLVixpQkFBaUI7Ozt1QkFXaEIsS0FBSzswQkFFTCxLQUFLOzBCQUVMLEtBQUs7d0JBRUwsS0FBSzt5QkFFTCxLQUFLOzZCQUVMLEtBQUs7K0JBR0wsTUFBTTs4QkFFTixNQUFNO3VCQVVOLEtBQUs7O0lBaUdSLHFCQUFDO0NBQUEsQUEvSEQsSUErSEM7U0EzSFksY0FBYzs7O0lBQ3pCLDhCQUFzQjs7SUFFdEIsaUNBQTJCOztJQUUzQixpQ0FBNkI7O0lBRTdCLCtCQUF1Qjs7SUFFdkIsZ0NBQXdCOztJQUV4QixvQ0FBMkI7O0lBRzNCLHNDQUF3RTs7SUFFeEUscUNBQTJEOzs7OztJQUUzRCxzQ0FBOEI7O0lBRTlCLCtCQUFXOztJQUVYLCtCQUFXOztJQXlDWCx1Q0FZRTs7SUFFRixtQ0FpQkU7O0lBRUYsd0NBSUU7O0lBRUYsaUNBS0U7O0lBRUYsZ0NBS0U7O0lBMUZVLDRCQUFxQjs7Ozs7SUFBRSwrQkFBZ0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xuICBBZnRlclZpZXdJbml0LFxuICBDb21wb25lbnQsXG4gIEVsZW1lbnRSZWYsXG4gIEV2ZW50RW1pdHRlcixcbiAgSW5wdXQsXG4gIE9uRGVzdHJveSxcbiAgT3V0cHV0LFxuICBDaGFuZ2VEZXRlY3RvclJlZixcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBCZWhhdmlvclN1YmplY3QgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGNoYXJ0SnNMb2FkZWQkIH0gZnJvbSAnLi4vLi4vdXRpbHMvd2lkZ2V0LXV0aWxzJztcbmRlY2xhcmUgY29uc3QgQ2hhcnQ6IGFueTtcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWNoYXJ0JyxcbiAgdGVtcGxhdGVVcmw6ICcuL2NoYXJ0LmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgQ2hhcnRDb21wb25lbnQgaW1wbGVtZW50cyBBZnRlclZpZXdJbml0LCBPbkRlc3Ryb3kge1xuICBASW5wdXQoKSB0eXBlOiBzdHJpbmc7XG5cbiAgQElucHV0KCkgb3B0aW9uczogYW55ID0ge307XG5cbiAgQElucHV0KCkgcGx1Z2luczogYW55W10gPSBbXTtcblxuICBASW5wdXQoKSB3aWR0aDogc3RyaW5nO1xuXG4gIEBJbnB1dCgpIGhlaWdodDogc3RyaW5nO1xuXG4gIEBJbnB1dCgpIHJlc3BvbnNpdmUgPSB0cnVlO1xuXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW9uLXByZWZpeFxuICBAT3V0cHV0KCkgcmVhZG9ubHkgb25EYXRhU2VsZWN0OiBFdmVudEVtaXR0ZXI8YW55PiA9IG5ldyBFdmVudEVtaXR0ZXIoKTtcblxuICBAT3V0cHV0KCkgcmVhZG9ubHkgaW5pdGlhbGl6ZWQgPSBuZXcgQmVoYXZpb3JTdWJqZWN0KHRoaXMpO1xuXG4gIHByaXZhdGUgX2luaXRpYWxpemVkOiBib29sZWFuO1xuXG4gIF9kYXRhOiBhbnk7XG5cbiAgY2hhcnQ6IGFueTtcblxuICBjb25zdHJ1Y3RvcihwdWJsaWMgZWw6IEVsZW1lbnRSZWYsIHByaXZhdGUgY2RSZWY6IENoYW5nZURldGVjdG9yUmVmKSB7fVxuXG4gIEBJbnB1dCgpIGdldCBkYXRhKCk6IGFueSB7XG4gICAgcmV0dXJuIHRoaXMuX2RhdGE7XG4gIH1cblxuICBzZXQgZGF0YSh2YWw6IGFueSkge1xuICAgIHRoaXMuX2RhdGEgPSB2YWw7XG4gICAgdGhpcy5yZWluaXQoKTtcbiAgfVxuXG4gIGdldCBjYW52YXMoKSB7XG4gICAgcmV0dXJuIHRoaXMuZWwubmF0aXZlRWxlbWVudC5jaGlsZHJlblswXS5jaGlsZHJlblswXTtcbiAgfVxuXG4gIGdldCBiYXNlNjRJbWFnZSgpIHtcbiAgICByZXR1cm4gdGhpcy5jaGFydC50b0Jhc2U2NEltYWdlKCk7XG4gIH1cblxuICBuZ0FmdGVyVmlld0luaXQoKSB7XG4gICAgY2hhcnRKc0xvYWRlZCQuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgIHRoaXMudGVzdENoYXJ0SnMoKTtcblxuICAgICAgdGhpcy5pbml0Q2hhcnQoKTtcbiAgICAgIHRoaXMuX2luaXRpYWxpemVkID0gdHJ1ZTtcbiAgICB9KTtcbiAgfVxuXG4gIHRlc3RDaGFydEpzKCkge1xuICAgIHRyeSB7XG4gICAgICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLXVudXNlZC1leHByZXNzaW9uXG4gICAgICBDaGFydDtcbiAgICB9IGNhdGNoIChlcnJvcikge1xuICAgICAgdGhyb3cgbmV3IEVycm9yKGBDaGFydCBpcyBub3QgZm91bmQuIEltcG9ydCB0aGUgQ2hhcnQgZnJvbSBhcHAubW9kdWxlIGxpa2Ugc2hvd24gYmVsb3c6XG4gICAgICBpbXBvcnQoJ2NoYXJ0LmpzJyk7XG4gICAgICBgKTtcbiAgICB9XG4gIH1cblxuICBvbkNhbnZhc0NsaWNrID0gZXZlbnQgPT4ge1xuICAgIGlmICh0aGlzLmNoYXJ0KSB7XG4gICAgICBjb25zdCBlbGVtZW50ID0gdGhpcy5jaGFydC5nZXRFbGVtZW50QXRFdmVudChldmVudCk7XG4gICAgICBjb25zdCBkYXRhc2V0ID0gdGhpcy5jaGFydC5nZXREYXRhc2V0QXRFdmVudChldmVudCk7XG4gICAgICBpZiAoZWxlbWVudCAmJiBlbGVtZW50Lmxlbmd0aCAmJiBkYXRhc2V0KSB7XG4gICAgICAgIHRoaXMub25EYXRhU2VsZWN0LmVtaXQoe1xuICAgICAgICAgIG9yaWdpbmFsRXZlbnQ6IGV2ZW50LFxuICAgICAgICAgIGVsZW1lbnQ6IGVsZW1lbnRbMF0sXG4gICAgICAgICAgZGF0YXNldCxcbiAgICAgICAgfSk7XG4gICAgICB9XG4gICAgfVxuICB9O1xuXG4gIGluaXRDaGFydCA9ICgpID0+IHtcbiAgICBjb25zdCBvcHRzID0gdGhpcy5vcHRpb25zIHx8IHt9O1xuICAgIG9wdHMucmVzcG9uc2l2ZSA9IHRoaXMucmVzcG9uc2l2ZTtcblxuICAgIC8vIGFsbG93cyBjaGFydCB0byByZXNpemUgaW4gcmVzcG9uc2l2ZSBtb2RlXG4gICAgaWYgKG9wdHMucmVzcG9uc2l2ZSAmJiAodGhpcy5oZWlnaHQgfHwgdGhpcy53aWR0aCkpIHtcbiAgICAgIG9wdHMubWFpbnRhaW5Bc3BlY3RSYXRpbyA9IGZhbHNlO1xuICAgIH1cblxuICAgIHRoaXMuY2hhcnQgPSBuZXcgQ2hhcnQodGhpcy5jYW52YXMsIHtcbiAgICAgIHR5cGU6IHRoaXMudHlwZSxcbiAgICAgIGRhdGE6IHRoaXMuZGF0YSxcbiAgICAgIG9wdGlvbnM6IHRoaXMub3B0aW9ucyxcbiAgICAgIHBsdWdpbnM6IHRoaXMucGx1Z2lucyxcbiAgICB9KTtcblxuICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xuICB9O1xuXG4gIGdlbmVyYXRlTGVnZW5kID0gKCkgPT4ge1xuICAgIGlmICh0aGlzLmNoYXJ0KSB7XG4gICAgICByZXR1cm4gdGhpcy5jaGFydC5nZW5lcmF0ZUxlZ2VuZCgpO1xuICAgIH1cbiAgfTtcblxuICByZWZyZXNoID0gKCkgPT4ge1xuICAgIGlmICh0aGlzLmNoYXJ0KSB7XG4gICAgICB0aGlzLmNoYXJ0LnVwZGF0ZSgpO1xuICAgICAgdGhpcy5jZFJlZi5kZXRlY3RDaGFuZ2VzKCk7XG4gICAgfVxuICB9O1xuXG4gIHJlaW5pdCA9ICgpID0+IHtcbiAgICBpZiAodGhpcy5jaGFydCkge1xuICAgICAgdGhpcy5jaGFydC5kZXN0cm95KCk7XG4gICAgICB0aGlzLmluaXRDaGFydCgpO1xuICAgIH1cbiAgfTtcblxuICBuZ09uRGVzdHJveSgpIHtcbiAgICBpZiAodGhpcy5jaGFydCkge1xuICAgICAgdGhpcy5jaGFydC5kZXN0cm95KCk7XG4gICAgICB0aGlzLl9pbml0aWFsaXplZCA9IGZhbHNlO1xuICAgICAgdGhpcy5jaGFydCA9IG51bGw7XG4gICAgfVxuICB9XG59XG4iXX0=