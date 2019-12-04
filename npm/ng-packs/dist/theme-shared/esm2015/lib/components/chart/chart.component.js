/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/chart/chart.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, ElementRef, EventEmitter, Input, Output, ChangeDetectorRef, } from '@angular/core';
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
        this.onCanvasClick = (/**
         * @param {?} event
         * @return {?}
         */
        event => {
            if (this.chart) {
                /** @type {?} */
                const element = this.chart.getElementAtEvent(event);
                /** @type {?} */
                const dataset = this.chart.getDatasetAtEvent(event);
                if (element && element.length && dataset) {
                    this.onDataSelect.emit({
                        originalEvent: event,
                        element: element[0],
                        dataset,
                    });
                }
            }
        });
        this.initChart = (/**
         * @return {?}
         */
        () => {
            /** @type {?} */
            const opts = this.options || {};
            opts.responsive = this.responsive;
            // allows chart to resize in responsive mode
            if (opts.responsive && (this.height || this.width)) {
                opts.maintainAspectRatio = false;
            }
            this.chart = new Chart(this.canvas, {
                type: this.type,
                data: this.data,
                options: this.options,
                plugins: this.plugins,
            });
            this.cdRef.detectChanges();
        });
        this.generateLegend = (/**
         * @return {?}
         */
        () => {
            if (this.chart) {
                return this.chart.generateLegend();
            }
        });
        this.refresh = (/**
         * @return {?}
         */
        () => {
            if (this.chart) {
                this.chart.update();
                this.cdRef.detectChanges();
            }
        });
        this.reinit = (/**
         * @return {?}
         */
        () => {
            if (this.chart) {
                this.chart.destroy();
                this.initChart();
            }
        });
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
        chartJsLoaded$.subscribe((/**
         * @return {?}
         */
        () => {
            this.testChartJs();
            this.initChart();
            this._initialized = true;
        }));
    }
    /**
     * @return {?}
     */
    testChartJs() {
        try {
            // tslint:disable-next-line: no-unused-expression
            Chart;
        }
        catch (error) {
            throw new Error(`Chart is not found. Import the Chart from app.module like shown below:
      import('chart.js');
      `);
        }
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
    { type: Component, args: [{
                selector: 'abp-chart',
                template: "<div\n  style=\"position:relative\"\n  [style.width]=\"responsive && !width ? null : width\"\n  [style.height]=\"responsive && !height ? null : height\"\n>\n  <canvas\n    [attr.width]=\"responsive && !width ? null : width\"\n    [attr.height]=\"responsive && !height ? null : height\"\n    (click)=\"onCanvasClick($event)\"\n  ></canvas>\n</div>\n"
            }] }
];
/** @nocollapse */
ChartComponent.ctorParameters = () => [
    { type: ElementRef },
    { type: ChangeDetectorRef }
];
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhcnQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9jaGFydC9jaGFydC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBRUwsU0FBUyxFQUNULFVBQVUsRUFDVixZQUFZLEVBQ1osS0FBSyxFQUVMLE1BQU0sRUFDTixpQkFBaUIsR0FDbEIsTUFBTSxlQUFlLENBQUM7QUFDdkIsT0FBTyxFQUFFLGVBQWUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUN2QyxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sMEJBQTBCLENBQUM7QUFPMUQsTUFBTSxPQUFPLGNBQWM7Ozs7O0lBd0J6QixZQUFtQixFQUFjLEVBQVUsS0FBd0I7UUFBaEQsT0FBRSxHQUFGLEVBQUUsQ0FBWTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQW1CO1FBckIxRCxZQUFPLEdBQVEsRUFBRSxDQUFDO1FBRWxCLFlBQU8sR0FBVSxFQUFFLENBQUM7UUFNcEIsZUFBVSxHQUFHLElBQUksQ0FBQzs7UUFHUixpQkFBWSxHQUFzQixJQUFJLFlBQVksRUFBRSxDQUFDO1FBRXJELGdCQUFXLEdBQUcsSUFBSSxlQUFlLENBQUMsSUFBSSxDQUFDLENBQUM7UUErQzNELGtCQUFhOzs7O1FBQUcsS0FBSyxDQUFDLEVBQUU7WUFDdEIsSUFBSSxJQUFJLENBQUMsS0FBSyxFQUFFOztzQkFDUixPQUFPLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxpQkFBaUIsQ0FBQyxLQUFLLENBQUM7O3NCQUM3QyxPQUFPLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxpQkFBaUIsQ0FBQyxLQUFLLENBQUM7Z0JBQ25ELElBQUksT0FBTyxJQUFJLE9BQU8sQ0FBQyxNQUFNLElBQUksT0FBTyxFQUFFO29CQUN4QyxJQUFJLENBQUMsWUFBWSxDQUFDLElBQUksQ0FBQzt3QkFDckIsYUFBYSxF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LE9BQU8sRUFBRSxDQUFDO1lBQ3JCLElBQUksQ0FBQyxZQUFZLEdBQUcsS0FBSyxDQUFDO1lBQzFCLElBQUksQ0FBQyxLQUFLLEdBQUcsSUFBSSxDQUFDO1NBQ25CO0lBQ0gsQ0FBQzs7O1lBOUhGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsV0FBVztnQkFDckIsd1dBQXFDO2FBQ3RDOzs7O1lBZEMsVUFBVTtZQUtWLGlCQUFpQjs7O21CQVdoQixLQUFLO3NCQUVMLEtBQUs7c0JBRUwsS0FBSztvQkFFTCxLQUFLO3FCQUVMLEtBQUs7eUJBRUwsS0FBSzsyQkFHTCxNQUFNOzBCQUVOLE1BQU07bUJBVU4sS0FBSzs7OztJQXpCTiw4QkFBc0I7O0lBRXRCLGlDQUEyQjs7SUFFM0IsaUNBQTZCOztJQUU3QiwrQkFBdUI7O0lBRXZCLGdDQUF3Qjs7SUFFeEIsb0NBQTJCOztJQUczQixzQ0FBd0U7O0lBRXhFLHFDQUEyRDs7Ozs7SUFFM0Qsc0NBQThCOztJQUU5QiwrQkFBVzs7SUFFWCwrQkFBVzs7SUF5Q1gsdUNBWUU7O0lBRUYsbUNBaUJFOztJQUVGLHdDQUlFOztJQUVGLGlDQUtFOztJQUVGLGdDQUtFOztJQTFGVSw0QkFBcUI7Ozs7O0lBQUUsK0JBQWdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcbiAgQWZ0ZXJWaWV3SW5pdCxcbiAgQ29tcG9uZW50LFxuICBFbGVtZW50UmVmLFxuICBFdmVudEVtaXR0ZXIsXG4gIElucHV0LFxuICBPbkRlc3Ryb3ksXG4gIE91dHB1dCxcbiAgQ2hhbmdlRGV0ZWN0b3JSZWYsXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQmVoYXZpb3JTdWJqZWN0IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBjaGFydEpzTG9hZGVkJCB9IGZyb20gJy4uLy4uL3V0aWxzL3dpZGdldC11dGlscyc7XG5kZWNsYXJlIGNvbnN0IENoYXJ0OiBhbnk7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1jaGFydCcsXG4gIHRlbXBsYXRlVXJsOiAnLi9jaGFydC5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIENoYXJ0Q29tcG9uZW50IGltcGxlbWVudHMgQWZ0ZXJWaWV3SW5pdCwgT25EZXN0cm95IHtcbiAgQElucHV0KCkgdHlwZTogc3RyaW5nO1xuXG4gIEBJbnB1dCgpIG9wdGlvbnM6IGFueSA9IHt9O1xuXG4gIEBJbnB1dCgpIHBsdWdpbnM6IGFueVtdID0gW107XG5cbiAgQElucHV0KCkgd2lkdGg6IHN0cmluZztcblxuICBASW5wdXQoKSBoZWlnaHQ6IHN0cmluZztcblxuICBASW5wdXQoKSByZXNwb25zaXZlID0gdHJ1ZTtcblxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLW91dHB1dC1vbi1wcmVmaXhcbiAgQE91dHB1dCgpIHJlYWRvbmx5IG9uRGF0YVNlbGVjdDogRXZlbnRFbWl0dGVyPGFueT4gPSBuZXcgRXZlbnRFbWl0dGVyKCk7XG5cbiAgQE91dHB1dCgpIHJlYWRvbmx5IGluaXRpYWxpemVkID0gbmV3IEJlaGF2aW9yU3ViamVjdCh0aGlzKTtcblxuICBwcml2YXRlIF9pbml0aWFsaXplZDogYm9vbGVhbjtcblxuICBfZGF0YTogYW55O1xuXG4gIGNoYXJ0OiBhbnk7XG5cbiAgY29uc3RydWN0b3IocHVibGljIGVsOiBFbGVtZW50UmVmLCBwcml2YXRlIGNkUmVmOiBDaGFuZ2VEZXRlY3RvclJlZikge31cblxuICBASW5wdXQoKSBnZXQgZGF0YSgpOiBhbnkge1xuICAgIHJldHVybiB0aGlzLl9kYXRhO1xuICB9XG5cbiAgc2V0IGRhdGEodmFsOiBhbnkpIHtcbiAgICB0aGlzLl9kYXRhID0gdmFsO1xuICAgIHRoaXMucmVpbml0KCk7XG4gIH1cblxuICBnZXQgY2FudmFzKCkge1xuICAgIHJldHVybiB0aGlzLmVsLm5hdGl2ZUVsZW1lbnQuY2hpbGRyZW5bMF0uY2hpbGRyZW5bMF07XG4gIH1cblxuICBnZXQgYmFzZTY0SW1hZ2UoKSB7XG4gICAgcmV0dXJuIHRoaXMuY2hhcnQudG9CYXNlNjRJbWFnZSgpO1xuICB9XG5cbiAgbmdBZnRlclZpZXdJbml0KCkge1xuICAgIGNoYXJ0SnNMb2FkZWQkLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICB0aGlzLnRlc3RDaGFydEpzKCk7XG5cbiAgICAgIHRoaXMuaW5pdENoYXJ0KCk7XG4gICAgICB0aGlzLl9pbml0aWFsaXplZCA9IHRydWU7XG4gICAgfSk7XG4gIH1cblxuICB0ZXN0Q2hhcnRKcygpIHtcbiAgICB0cnkge1xuICAgICAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby11bnVzZWQtZXhwcmVzc2lvblxuICAgICAgQ2hhcnQ7XG4gICAgfSBjYXRjaCAoZXJyb3IpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcihgQ2hhcnQgaXMgbm90IGZvdW5kLiBJbXBvcnQgdGhlIENoYXJ0IGZyb20gYXBwLm1vZHVsZSBsaWtlIHNob3duIGJlbG93OlxuICAgICAgaW1wb3J0KCdjaGFydC5qcycpO1xuICAgICAgYCk7XG4gICAgfVxuICB9XG5cbiAgb25DYW52YXNDbGljayA9IGV2ZW50ID0+IHtcbiAgICBpZiAodGhpcy5jaGFydCkge1xuICAgICAgY29uc3QgZWxlbWVudCA9IHRoaXMuY2hhcnQuZ2V0RWxlbWVudEF0RXZlbnQoZXZlbnQpO1xuICAgICAgY29uc3QgZGF0YXNldCA9IHRoaXMuY2hhcnQuZ2V0RGF0YXNldEF0RXZlbnQoZXZlbnQpO1xuICAgICAgaWYgKGVsZW1lbnQgJiYgZWxlbWVudC5sZW5ndGggJiYgZGF0YXNldCkge1xuICAgICAgICB0aGlzLm9uRGF0YVNlbGVjdC5lbWl0KHtcbiAgICAgICAgICBvcmlnaW5hbEV2ZW50OiBldmVudCxcbiAgICAgICAgICBlbGVtZW50OiBlbGVtZW50WzBdLFxuICAgICAgICAgIGRhdGFzZXQsXG4gICAgICAgIH0pO1xuICAgICAgfVxuICAgIH1cbiAgfTtcblxuICBpbml0Q2hhcnQgPSAoKSA9PiB7XG4gICAgY29uc3Qgb3B0cyA9IHRoaXMub3B0aW9ucyB8fCB7fTtcbiAgICBvcHRzLnJlc3BvbnNpdmUgPSB0aGlzLnJlc3BvbnNpdmU7XG5cbiAgICAvLyBhbGxvd3MgY2hhcnQgdG8gcmVzaXplIGluIHJlc3BvbnNpdmUgbW9kZVxuICAgIGlmIChvcHRzLnJlc3BvbnNpdmUgJiYgKHRoaXMuaGVpZ2h0IHx8IHRoaXMud2lkdGgpKSB7XG4gICAgICBvcHRzLm1haW50YWluQXNwZWN0UmF0aW8gPSBmYWxzZTtcbiAgICB9XG5cbiAgICB0aGlzLmNoYXJ0ID0gbmV3IENoYXJ0KHRoaXMuY2FudmFzLCB7XG4gICAgICB0eXBlOiB0aGlzLnR5cGUsXG4gICAgICBkYXRhOiB0aGlzLmRhdGEsXG4gICAgICBvcHRpb25zOiB0aGlzLm9wdGlvbnMsXG4gICAgICBwbHVnaW5zOiB0aGlzLnBsdWdpbnMsXG4gICAgfSk7XG5cbiAgICB0aGlzLmNkUmVmLmRldGVjdENoYW5nZXMoKTtcbiAgfTtcblxuICBnZW5lcmF0ZUxlZ2VuZCA9ICgpID0+IHtcbiAgICBpZiAodGhpcy5jaGFydCkge1xuICAgICAgcmV0dXJuIHRoaXMuY2hhcnQuZ2VuZXJhdGVMZWdlbmQoKTtcbiAgICB9XG4gIH07XG5cbiAgcmVmcmVzaCA9ICgpID0+IHtcbiAgICBpZiAodGhpcy5jaGFydCkge1xuICAgICAgdGhpcy5jaGFydC51cGRhdGUoKTtcbiAgICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xuICAgIH1cbiAgfTtcblxuICByZWluaXQgPSAoKSA9PiB7XG4gICAgaWYgKHRoaXMuY2hhcnQpIHtcbiAgICAgIHRoaXMuY2hhcnQuZGVzdHJveSgpO1xuICAgICAgdGhpcy5pbml0Q2hhcnQoKTtcbiAgICB9XG4gIH07XG5cbiAgbmdPbkRlc3Ryb3koKSB7XG4gICAgaWYgKHRoaXMuY2hhcnQpIHtcbiAgICAgIHRoaXMuY2hhcnQuZGVzdHJveSgpO1xuICAgICAgdGhpcy5faW5pdGlhbGl6ZWQgPSBmYWxzZTtcbiAgICAgIHRoaXMuY2hhcnQgPSBudWxsO1xuICAgIH1cbiAgfVxufVxuIl19