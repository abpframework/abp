/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
                    _this.onDataSelect.emit({ originalEvent: event, element: element[0], dataset: dataset });
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
            try {
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhcnQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9jaGFydC9jaGFydC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFFTCxTQUFTLEVBQ1QsVUFBVSxFQUNWLFlBQVksRUFDWixLQUFLLEVBRUwsTUFBTSxFQUNOLGlCQUFpQixHQUNsQixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ3ZDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSwwQkFBMEIsQ0FBQztBQUcxRDtJQTJCRSx3QkFBbUIsRUFBYyxFQUFVLEtBQXdCO1FBQW5FLGlCQUF1RTtRQUFwRCxPQUFFLEdBQUYsRUFBRSxDQUFZO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBbUI7UUFwQjFELFlBQU8sR0FBUSxFQUFFLENBQUM7UUFFbEIsWUFBTyxHQUFVLEVBQUUsQ0FBQztRQU1wQixlQUFVLEdBQVksSUFBSSxDQUFDO1FBRTFCLGlCQUFZLEdBQXNCLElBQUksWUFBWSxFQUFFLENBQUM7UUFFckQsZ0JBQVcsR0FBRyxJQUFJLGVBQWUsQ0FBQyxJQUFJLENBQUMsQ0FBQztRQTJDbEQsa0JBQWE7Ozs7UUFBRyxVQUFBLEtBQUs7WUFDbkIsSUFBSSxLQUFJLENBQUMsS0FBSyxFQUFFOztvQkFDVixPQUFPLEdBQUcsS0FBSSxDQUFDLEtBQUssQ0FBQyxpQkFBaUIsQ0FBQyxLQUFLLENBQUM7O29CQUM3QyxPQUFPLEdBQUcsS0FBSSxDQUFDLEtBQUssQ0FBQyxpQkFBaUIsQ0FBQyxLQUFLLENBQUM7Z0JBQ2pELElBQUksT0FBTyxJQUFJLE9BQU8sQ0FBQyxDQUFDLENBQUMsSUFBSSxPQUFPLEVBQUU7b0JBQ3BDLEtBQUksQ0FBQyxZQUFZLENBQUMsSUFBSSxDQUFDLEVBQUUsYUFBYSxFQUFFLEtBQUss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QUMsS0FBSyxDQUFDLE9BQU8sRUFBRSxDQUFDO1lBQ3JCLElBQUksQ0FBQyxZQUFZLEdBQUcsS0FBSyxDQUFDO1lBQzFCLElBQUksQ0FBQyxLQUFLLEdBQUcsSUFBSSxDQUFDO1NBQ25CO0lBQ0gsQ0FBQzs7Z0JBckhGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsV0FBVztvQkFDckIsd1dBQXFDO2lCQUN0Qzs7OztnQkFkQyxVQUFVO2dCQUtWLGlCQUFpQjs7O3VCQVdoQixLQUFLOzBCQUVMLEtBQUs7MEJBRUwsS0FBSzt3QkFFTCxLQUFLO3lCQUVMLEtBQUs7NkJBRUwsS0FBSzsrQkFFTCxNQUFNOzhCQUVOLE1BQU07dUJBVU4sS0FBSzs7SUF5RlIscUJBQUM7Q0FBQSxBQXRIRCxJQXNIQztTQWxIWSxjQUFjOzs7SUFDekIsOEJBQXNCOztJQUV0QixpQ0FBMkI7O0lBRTNCLGlDQUE2Qjs7SUFFN0IsK0JBQXVCOztJQUV2QixnQ0FBd0I7O0lBRXhCLG9DQUFvQzs7SUFFcEMsc0NBQStEOztJQUUvRCxxQ0FBa0Q7Ozs7O0lBRWxELHNDQUE4Qjs7SUFFOUIsK0JBQVc7O0lBRVgsK0JBQVc7O0lBcUNYLHVDQVFFOztJQUVGLG1DQWlCRTs7SUFFRix3Q0FJRTs7SUFFRixpQ0FLRTs7SUFFRixnQ0FLRTs7SUFsRlUsNEJBQXFCOzs7OztJQUFFLCtCQUFnQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7XG4gIEFmdGVyVmlld0luaXQsXG4gIENvbXBvbmVudCxcbiAgRWxlbWVudFJlZixcbiAgRXZlbnRFbWl0dGVyLFxuICBJbnB1dCxcbiAgT25EZXN0cm95LFxuICBPdXRwdXQsXG4gIENoYW5nZURldGVjdG9yUmVmLFxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEJlaGF2aW9yU3ViamVjdCB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgY2hhcnRKc0xvYWRlZCQgfSBmcm9tICcuLi8uLi91dGlscy93aWRnZXQtdXRpbHMnO1xuZGVjbGFyZSBjb25zdCBDaGFydDogYW55O1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtY2hhcnQnLFxuICB0ZW1wbGF0ZVVybDogJy4vY2hhcnQuY29tcG9uZW50Lmh0bWwnLFxufSlcbmV4cG9ydCBjbGFzcyBDaGFydENvbXBvbmVudCBpbXBsZW1lbnRzIEFmdGVyVmlld0luaXQsIE9uRGVzdHJveSB7XG4gIEBJbnB1dCgpIHR5cGU6IHN0cmluZztcblxuICBASW5wdXQoKSBvcHRpb25zOiBhbnkgPSB7fTtcblxuICBASW5wdXQoKSBwbHVnaW5zOiBhbnlbXSA9IFtdO1xuXG4gIEBJbnB1dCgpIHdpZHRoOiBzdHJpbmc7XG5cbiAgQElucHV0KCkgaGVpZ2h0OiBzdHJpbmc7XG5cbiAgQElucHV0KCkgcmVzcG9uc2l2ZTogYm9vbGVhbiA9IHRydWU7XG5cbiAgQE91dHB1dCgpIG9uRGF0YVNlbGVjdDogRXZlbnRFbWl0dGVyPGFueT4gPSBuZXcgRXZlbnRFbWl0dGVyKCk7XG5cbiAgQE91dHB1dCgpIGluaXRpYWxpemVkID0gbmV3IEJlaGF2aW9yU3ViamVjdCh0aGlzKTtcblxuICBwcml2YXRlIF9pbml0aWFsaXplZDogYm9vbGVhbjtcblxuICBfZGF0YTogYW55O1xuXG4gIGNoYXJ0OiBhbnk7XG5cbiAgY29uc3RydWN0b3IocHVibGljIGVsOiBFbGVtZW50UmVmLCBwcml2YXRlIGNkUmVmOiBDaGFuZ2VEZXRlY3RvclJlZikge31cblxuICBASW5wdXQoKSBnZXQgZGF0YSgpOiBhbnkge1xuICAgIHJldHVybiB0aGlzLl9kYXRhO1xuICB9XG5cbiAgc2V0IGRhdGEodmFsOiBhbnkpIHtcbiAgICB0aGlzLl9kYXRhID0gdmFsO1xuICAgIHRoaXMucmVpbml0KCk7XG4gIH1cblxuICBnZXQgY2FudmFzKCkge1xuICAgIHJldHVybiB0aGlzLmVsLm5hdGl2ZUVsZW1lbnQuY2hpbGRyZW5bMF0uY2hpbGRyZW5bMF07XG4gIH1cblxuICBnZXQgYmFzZTY0SW1hZ2UoKSB7XG4gICAgcmV0dXJuIHRoaXMuY2hhcnQudG9CYXNlNjRJbWFnZSgpO1xuICB9XG5cbiAgbmdBZnRlclZpZXdJbml0KCkge1xuICAgIGNoYXJ0SnNMb2FkZWQkLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICB0cnkge1xuICAgICAgICBDaGFydDtcbiAgICAgIH0gY2F0Y2ggKGVycm9yKSB7XG4gICAgICAgIGNvbnNvbGUuZXJyb3IoYENoYXJ0IGlzIG5vdCBmb3VuZC4gSW1wb3J0IHRoZSBDaGFydCBmcm9tIGFwcC5tb2R1bGUgbGlrZSBzaG93biBiZWxvdzpcbiAgICAgICAgaW1wb3J0KCdjaGFydC5qcycpO1xuICAgICAgICBgKTtcbiAgICAgICAgcmV0dXJuO1xuICAgICAgfVxuXG4gICAgICB0aGlzLmluaXRDaGFydCgpO1xuICAgICAgdGhpcy5faW5pdGlhbGl6ZWQgPSB0cnVlO1xuICAgIH0pO1xuICB9XG5cbiAgb25DYW52YXNDbGljayA9IGV2ZW50ID0+IHtcbiAgICBpZiAodGhpcy5jaGFydCkge1xuICAgICAgbGV0IGVsZW1lbnQgPSB0aGlzLmNoYXJ0LmdldEVsZW1lbnRBdEV2ZW50KGV2ZW50KTtcbiAgICAgIGxldCBkYXRhc2V0ID0gdGhpcy5jaGFydC5nZXREYXRhc2V0QXRFdmVudChldmVudCk7XG4gICAgICBpZiAoZWxlbWVudCAmJiBlbGVtZW50WzBdICYmIGRhdGFzZXQpIHtcbiAgICAgICAgdGhpcy5vbkRhdGFTZWxlY3QuZW1pdCh7IG9yaWdpbmFsRXZlbnQ6IGV2ZW50LCBlbGVtZW50OiBlbGVtZW50WzBdLCBkYXRhc2V0OiBkYXRhc2V0IH0pO1xuICAgICAgfVxuICAgIH1cbiAgfTtcblxuICBpbml0Q2hhcnQgPSAoKSA9PiB7XG4gICAgbGV0IG9wdHMgPSB0aGlzLm9wdGlvbnMgfHwge307XG4gICAgb3B0cy5yZXNwb25zaXZlID0gdGhpcy5yZXNwb25zaXZlO1xuXG4gICAgLy8gYWxsb3dzIGNoYXJ0IHRvIHJlc2l6ZSBpbiByZXNwb25zaXZlIG1vZGVcbiAgICBpZiAob3B0cy5yZXNwb25zaXZlICYmICh0aGlzLmhlaWdodCB8fCB0aGlzLndpZHRoKSkge1xuICAgICAgb3B0cy5tYWludGFpbkFzcGVjdFJhdGlvID0gZmFsc2U7XG4gICAgfVxuXG4gICAgdGhpcy5jaGFydCA9IG5ldyBDaGFydCh0aGlzLmVsLm5hdGl2ZUVsZW1lbnQuY2hpbGRyZW5bMF0uY2hpbGRyZW5bMF0sIHtcbiAgICAgIHR5cGU6IHRoaXMudHlwZSxcbiAgICAgIGRhdGE6IHRoaXMuZGF0YSxcbiAgICAgIG9wdGlvbnM6IHRoaXMub3B0aW9ucyxcbiAgICAgIHBsdWdpbnM6IHRoaXMucGx1Z2lucyxcbiAgICB9KTtcblxuICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xuICB9O1xuXG4gIGdlbmVyYXRlTGVnZW5kID0gKCkgPT4ge1xuICAgIGlmICh0aGlzLmNoYXJ0KSB7XG4gICAgICByZXR1cm4gdGhpcy5jaGFydC5nZW5lcmF0ZUxlZ2VuZCgpO1xuICAgIH1cbiAgfTtcblxuICByZWZyZXNoID0gKCkgPT4ge1xuICAgIGlmICh0aGlzLmNoYXJ0KSB7XG4gICAgICB0aGlzLmNoYXJ0LnVwZGF0ZSgpO1xuICAgICAgdGhpcy5jZFJlZi5kZXRlY3RDaGFuZ2VzKCk7XG4gICAgfVxuICB9O1xuXG4gIHJlaW5pdCA9ICgpID0+IHtcbiAgICBpZiAodGhpcy5jaGFydCkge1xuICAgICAgdGhpcy5jaGFydC5kZXN0cm95KCk7XG4gICAgICB0aGlzLmluaXRDaGFydCgpO1xuICAgIH1cbiAgfTtcblxuICBuZ09uRGVzdHJveSgpIHtcbiAgICBpZiAodGhpcy5jaGFydCkge1xuICAgICAgdGhpcy5jaGFydC5kZXN0cm95KCk7XG4gICAgICB0aGlzLl9pbml0aWFsaXplZCA9IGZhbHNlO1xuICAgICAgdGhpcy5jaGFydCA9IG51bGw7XG4gICAgfVxuICB9XG59XG4iXX0=