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
                if (element && element[0] && dataset) {
                    this.onDataSelect.emit({
                        originalEvent: event,
                        element: element[0],
                        dataset
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
            this.chart = new Chart(this.el.nativeElement.children[0].children[0], {
                type: this.type,
                data: this.data,
                options: this.options,
                plugins: this.plugins
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
            try {
                // tslint:disable-next-line: no-unused-expression
                Chart;
            }
            catch (error) {
                console.error(`Chart is not found. Import the Chart from app.module like shown below:
        import('chart.js');
        `);
                return;
            }
            this.initChart();
            this._initialized = true;
        }));
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
                template: "<div\r\n  style=\"position:relative\"\r\n  [style.width]=\"responsive && !width ? null : width\"\r\n  [style.height]=\"responsive && !height ? null : height\"\r\n>\r\n  <canvas\r\n    [attr.width]=\"responsive && !width ? null : width\"\r\n    [attr.height]=\"responsive && !height ? null : height\"\r\n    (click)=\"onCanvasClick($event)\"\r\n  ></canvas>\r\n</div>\r\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY2hhcnQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9jaGFydC9jaGFydC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFFTCxTQUFTLEVBQ1QsVUFBVSxFQUNWLFlBQVksRUFDWixLQUFLLEVBRUwsTUFBTSxFQUNOLGlCQUFpQixFQUNsQixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ3ZDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSwwQkFBMEIsQ0FBQztBQU8xRCxNQUFNLE9BQU8sY0FBYzs7Ozs7SUF3QnpCLFlBQW1CLEVBQWMsRUFBVSxLQUF3QjtRQUFoRCxPQUFFLEdBQUYsRUFBRSxDQUFZO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBbUI7UUFyQjFELFlBQU8sR0FBUSxFQUFFLENBQUM7UUFFbEIsWUFBTyxHQUFVLEVBQUUsQ0FBQztRQU1wQixlQUFVLEdBQUcsSUFBSSxDQUFDOztRQUdSLGlCQUFZLEdBQXNCLElBQUksWUFBWSxFQUFFLENBQUM7UUFFckQsZ0JBQVcsR0FBRyxJQUFJLGVBQWUsQ0FBQyxJQUFJLENBQUMsQ0FBQztRQTRDM0Qsa0JBQWE7Ozs7UUFBRyxLQUFLLENBQUMsRUFBRTtZQUN0QixJQUFJLElBQUksQ0FBQyxLQUFLLEVBQUU7O3NCQUNSLE9BQU8sR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGlCQUFpQixDQUFDLEtBQUssQ0FBQzs7c0JBQzdDLE9BQU8sR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGlCQUFpQixDQUFDLEtBQUssQ0FBQztnQkFDbkQsSUFBSSxPQUFPLElBQUksT0FBTyxDQUFDLENBQUMsQ0FBQyxJQUFJLE9BQU8sRUFBRTtvQkFDcEMsSUFBSSxDQUFDLFlBQVksQ0FBQyxJQUFJLENBQUM7d0JBQ3JCLGFB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WUFDZCxJQUFJLENBQUMsS0FBSyxDQUFDLE9BQU8sRUFBRSxDQUFDO1lBQ3JCLElBQUksQ0FBQyxZQUFZLEdBQUcsS0FBSyxDQUFDO1lBQzFCLElBQUksQ0FBQyxLQUFLLEdBQUcsSUFBSSxDQUFDO1NBQ25CO0lBQ0gsQ0FBQzs7O1lBM0hGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsV0FBVztnQkFDckIsOFhBQXFDO2FBQ3RDOzs7O1lBZEMsVUFBVTtZQUtWLGlCQUFpQjs7O21CQVdoQixLQUFLO3NCQUVMLEtBQUs7c0JBRUwsS0FBSztvQkFFTCxLQUFLO3FCQUVMLEtBQUs7eUJBRUwsS0FBSzsyQkFHTCxNQUFNOzBCQUVOLE1BQU07bUJBVU4sS0FBSzs7OztJQXpCTiw4QkFBc0I7O0lBRXRCLGlDQUEyQjs7SUFFM0IsaUNBQTZCOztJQUU3QiwrQkFBdUI7O0lBRXZCLGdDQUF3Qjs7SUFFeEIsb0NBQTJCOztJQUczQixzQ0FBd0U7O0lBRXhFLHFDQUEyRDs7Ozs7SUFFM0Qsc0NBQThCOztJQUU5QiwrQkFBVzs7SUFFWCwrQkFBVzs7SUFzQ1gsdUNBWUU7O0lBRUYsbUNBaUJFOztJQUVGLHdDQUlFOztJQUVGLGlDQUtFOztJQUVGLGdDQUtFOztJQXZGVSw0QkFBcUI7Ozs7O0lBQUUsK0JBQWdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcclxuICBBZnRlclZpZXdJbml0LFxyXG4gIENvbXBvbmVudCxcclxuICBFbGVtZW50UmVmLFxyXG4gIEV2ZW50RW1pdHRlcixcclxuICBJbnB1dCxcclxuICBPbkRlc3Ryb3ksXHJcbiAgT3V0cHV0LFxyXG4gIENoYW5nZURldGVjdG9yUmVmXHJcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IEJlaGF2aW9yU3ViamVjdCB9IGZyb20gJ3J4anMnO1xyXG5pbXBvcnQgeyBjaGFydEpzTG9hZGVkJCB9IGZyb20gJy4uLy4uL3V0aWxzL3dpZGdldC11dGlscyc7XHJcbmRlY2xhcmUgY29uc3QgQ2hhcnQ6IGFueTtcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLWNoYXJ0JyxcclxuICB0ZW1wbGF0ZVVybDogJy4vY2hhcnQuY29tcG9uZW50Lmh0bWwnXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBDaGFydENvbXBvbmVudCBpbXBsZW1lbnRzIEFmdGVyVmlld0luaXQsIE9uRGVzdHJveSB7XHJcbiAgQElucHV0KCkgdHlwZTogc3RyaW5nO1xyXG5cclxuICBASW5wdXQoKSBvcHRpb25zOiBhbnkgPSB7fTtcclxuXHJcbiAgQElucHV0KCkgcGx1Z2luczogYW55W10gPSBbXTtcclxuXHJcbiAgQElucHV0KCkgd2lkdGg6IHN0cmluZztcclxuXHJcbiAgQElucHV0KCkgaGVpZ2h0OiBzdHJpbmc7XHJcblxyXG4gIEBJbnB1dCgpIHJlc3BvbnNpdmUgPSB0cnVlO1xyXG5cclxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLW91dHB1dC1vbi1wcmVmaXhcclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgb25EYXRhU2VsZWN0OiBFdmVudEVtaXR0ZXI8YW55PiA9IG5ldyBFdmVudEVtaXR0ZXIoKTtcclxuXHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IGluaXRpYWxpemVkID0gbmV3IEJlaGF2aW9yU3ViamVjdCh0aGlzKTtcclxuXHJcbiAgcHJpdmF0ZSBfaW5pdGlhbGl6ZWQ6IGJvb2xlYW47XHJcblxyXG4gIF9kYXRhOiBhbnk7XHJcblxyXG4gIGNoYXJ0OiBhbnk7XHJcblxyXG4gIGNvbnN0cnVjdG9yKHB1YmxpYyBlbDogRWxlbWVudFJlZiwgcHJpdmF0ZSBjZFJlZjogQ2hhbmdlRGV0ZWN0b3JSZWYpIHt9XHJcblxyXG4gIEBJbnB1dCgpIGdldCBkYXRhKCk6IGFueSB7XHJcbiAgICByZXR1cm4gdGhpcy5fZGF0YTtcclxuICB9XHJcblxyXG4gIHNldCBkYXRhKHZhbDogYW55KSB7XHJcbiAgICB0aGlzLl9kYXRhID0gdmFsO1xyXG4gICAgdGhpcy5yZWluaXQoKTtcclxuICB9XHJcblxyXG4gIGdldCBjYW52YXMoKSB7XHJcbiAgICByZXR1cm4gdGhpcy5lbC5uYXRpdmVFbGVtZW50LmNoaWxkcmVuWzBdLmNoaWxkcmVuWzBdO1xyXG4gIH1cclxuXHJcbiAgZ2V0IGJhc2U2NEltYWdlKCkge1xyXG4gICAgcmV0dXJuIHRoaXMuY2hhcnQudG9CYXNlNjRJbWFnZSgpO1xyXG4gIH1cclxuXHJcbiAgbmdBZnRlclZpZXdJbml0KCkge1xyXG4gICAgY2hhcnRKc0xvYWRlZCQuc3Vic2NyaWJlKCgpID0+IHtcclxuICAgICAgdHJ5IHtcclxuICAgICAgICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLXVudXNlZC1leHByZXNzaW9uXHJcbiAgICAgICAgQ2hhcnQ7XHJcbiAgICAgIH0gY2F0Y2ggKGVycm9yKSB7XHJcbiAgICAgICAgY29uc29sZS5lcnJvcihgQ2hhcnQgaXMgbm90IGZvdW5kLiBJbXBvcnQgdGhlIENoYXJ0IGZyb20gYXBwLm1vZHVsZSBsaWtlIHNob3duIGJlbG93OlxyXG4gICAgICAgIGltcG9ydCgnY2hhcnQuanMnKTtcclxuICAgICAgICBgKTtcclxuICAgICAgICByZXR1cm47XHJcbiAgICAgIH1cclxuXHJcbiAgICAgIHRoaXMuaW5pdENoYXJ0KCk7XHJcbiAgICAgIHRoaXMuX2luaXRpYWxpemVkID0gdHJ1ZTtcclxuICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgb25DYW52YXNDbGljayA9IGV2ZW50ID0+IHtcclxuICAgIGlmICh0aGlzLmNoYXJ0KSB7XHJcbiAgICAgIGNvbnN0IGVsZW1lbnQgPSB0aGlzLmNoYXJ0LmdldEVsZW1lbnRBdEV2ZW50KGV2ZW50KTtcclxuICAgICAgY29uc3QgZGF0YXNldCA9IHRoaXMuY2hhcnQuZ2V0RGF0YXNldEF0RXZlbnQoZXZlbnQpO1xyXG4gICAgICBpZiAoZWxlbWVudCAmJiBlbGVtZW50WzBdICYmIGRhdGFzZXQpIHtcclxuICAgICAgICB0aGlzLm9uRGF0YVNlbGVjdC5lbWl0KHtcclxuICAgICAgICAgIG9yaWdpbmFsRXZlbnQ6IGV2ZW50LFxyXG4gICAgICAgICAgZWxlbWVudDogZWxlbWVudFswXSxcclxuICAgICAgICAgIGRhdGFzZXRcclxuICAgICAgICB9KTtcclxuICAgICAgfVxyXG4gICAgfVxyXG4gIH07XHJcblxyXG4gIGluaXRDaGFydCA9ICgpID0+IHtcclxuICAgIGNvbnN0IG9wdHMgPSB0aGlzLm9wdGlvbnMgfHwge307XHJcbiAgICBvcHRzLnJlc3BvbnNpdmUgPSB0aGlzLnJlc3BvbnNpdmU7XHJcblxyXG4gICAgLy8gYWxsb3dzIGNoYXJ0IHRvIHJlc2l6ZSBpbiByZXNwb25zaXZlIG1vZGVcclxuICAgIGlmIChvcHRzLnJlc3BvbnNpdmUgJiYgKHRoaXMuaGVpZ2h0IHx8IHRoaXMud2lkdGgpKSB7XHJcbiAgICAgIG9wdHMubWFpbnRhaW5Bc3BlY3RSYXRpbyA9IGZhbHNlO1xyXG4gICAgfVxyXG5cclxuICAgIHRoaXMuY2hhcnQgPSBuZXcgQ2hhcnQodGhpcy5lbC5uYXRpdmVFbGVtZW50LmNoaWxkcmVuWzBdLmNoaWxkcmVuWzBdLCB7XHJcbiAgICAgIHR5cGU6IHRoaXMudHlwZSxcclxuICAgICAgZGF0YTogdGhpcy5kYXRhLFxyXG4gICAgICBvcHRpb25zOiB0aGlzLm9wdGlvbnMsXHJcbiAgICAgIHBsdWdpbnM6IHRoaXMucGx1Z2luc1xyXG4gICAgfSk7XHJcblxyXG4gICAgdGhpcy5jZFJlZi5kZXRlY3RDaGFuZ2VzKCk7XHJcbiAgfTtcclxuXHJcbiAgZ2VuZXJhdGVMZWdlbmQgPSAoKSA9PiB7XHJcbiAgICBpZiAodGhpcy5jaGFydCkge1xyXG4gICAgICByZXR1cm4gdGhpcy5jaGFydC5nZW5lcmF0ZUxlZ2VuZCgpO1xyXG4gICAgfVxyXG4gIH07XHJcblxyXG4gIHJlZnJlc2ggPSAoKSA9PiB7XHJcbiAgICBpZiAodGhpcy5jaGFydCkge1xyXG4gICAgICB0aGlzLmNoYXJ0LnVwZGF0ZSgpO1xyXG4gICAgICB0aGlzLmNkUmVmLmRldGVjdENoYW5nZXMoKTtcclxuICAgIH1cclxuICB9O1xyXG5cclxuICByZWluaXQgPSAoKSA9PiB7XHJcbiAgICBpZiAodGhpcy5jaGFydCkge1xyXG4gICAgICB0aGlzLmNoYXJ0LmRlc3Ryb3koKTtcclxuICAgICAgdGhpcy5pbml0Q2hhcnQoKTtcclxuICAgIH1cclxuICB9O1xyXG5cclxuICBuZ09uRGVzdHJveSgpIHtcclxuICAgIGlmICh0aGlzLmNoYXJ0KSB7XHJcbiAgICAgIHRoaXMuY2hhcnQuZGVzdHJveSgpO1xyXG4gICAgICB0aGlzLl9pbml0aWFsaXplZCA9IGZhbHNlO1xyXG4gICAgICB0aGlzLmNoYXJ0ID0gbnVsbDtcclxuICAgIH1cclxuICB9XHJcbn1cclxuIl19