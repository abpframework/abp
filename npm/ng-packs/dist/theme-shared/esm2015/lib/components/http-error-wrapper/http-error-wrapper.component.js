/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/http-error-wrapper/http-error-wrapper.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { takeUntilDestroy } from '@abp/ng.core';
import { Component, ElementRef, ViewChild, } from '@angular/core';
import { fromEvent } from 'rxjs';
import { debounceTime, filter } from 'rxjs/operators';
import snq from 'snq';
export class HttpErrorWrapperComponent {
    constructor() {
        this.status = 0;
        this.title = 'Oops!';
        this.details = 'Sorry, an error has occured.';
        this.customComponent = null;
        this.hideCloseIcon = false;
    }
    /**
     * @return {?}
     */
    get statusText() {
        return this.status ? `[${this.status}]` : '';
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        this.backgroundColor =
            snq((/**
             * @return {?}
             */
            () => window.getComputedStyle(document.body).getPropertyValue('background-color'))) || '#fff';
    }
    /**
     * @return {?}
     */
    ngAfterViewInit() {
        if (this.customComponent) {
            /** @type {?} */
            const customComponentRef = this.cfRes.resolveComponentFactory(this.customComponent).create(this.injector);
            customComponentRef.instance.errorStatus = this.status;
            customComponentRef.instance.destroy$ = this.destroy$;
            this.appRef.attachView(customComponentRef.hostView);
            this.containerRef.nativeElement.appendChild(((/** @type {?} */ (customComponentRef.hostView))).rootNodes[0]);
            customComponentRef.changeDetectorRef.detectChanges();
        }
        fromEvent(document, 'keyup')
            .pipe(takeUntilDestroy(this), debounceTime(150), filter((/**
         * @param {?} key
         * @return {?}
         */
        (key) => key && key.key === 'Escape')))
            .subscribe((/**
         * @return {?}
         */
        () => {
            this.destroy();
        }));
    }
    /**
     * @return {?}
     */
    ngOnDestroy() { }
    /**
     * @return {?}
     */
    destroy() {
        this.destroy$.next();
        this.destroy$.complete();
    }
}
HttpErrorWrapperComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-http-error-wrapper',
                template: "<div #container id=\"abp-http-error-container\" class=\"error\" [style.backgroundColor]=\"backgroundColor\">\n  <button *ngIf=\"!hideCloseIcon\" id=\"abp-close-button\" type=\"button\" class=\"close mr-2\" (click)=\"destroy()\">\n    <span aria-hidden=\"true\">&times;</span>\n  </button>\n\n  <div *ngIf=\"!customComponent\" class=\"row centered\">\n    <div class=\"col-md-12\">\n      <div class=\"error-template\">\n        <h1>{{ statusText }} {{ title | abpLocalization }}</h1>\n        <div class=\"error-details\">\n          {{ details | abpLocalization }}\n        </div>\n        <div class=\"error-actions\">\n          <a (click)=\"destroy()\" routerLink=\"/\" class=\"btn btn-primary btn-md mt-2\"\n            ><span class=\"glyphicon glyphicon-home\"></span>\n            {{ { key: '::Menu:Home', defaultValue: 'Home' } | abpLocalization }}\n          </a>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n",
                styles: [".error{position:fixed;top:0;width:100vw;height:100vh;z-index:999999}.centered{position:fixed;top:50%;left:50%;-webkit-transform:translate(-50%,-50%);transform:translate(-50%,-50%)}"]
            }] }
];
HttpErrorWrapperComponent.propDecorators = {
    containerRef: [{ type: ViewChild, args: ['container', { static: false },] }]
};
if (false) {
    /** @type {?} */
    HttpErrorWrapperComponent.prototype.appRef;
    /** @type {?} */
    HttpErrorWrapperComponent.prototype.cfRes;
    /** @type {?} */
    HttpErrorWrapperComponent.prototype.injector;
    /** @type {?} */
    HttpErrorWrapperComponent.prototype.status;
    /** @type {?} */
    HttpErrorWrapperComponent.prototype.title;
    /** @type {?} */
    HttpErrorWrapperComponent.prototype.details;
    /** @type {?} */
    HttpErrorWrapperComponent.prototype.customComponent;
    /** @type {?} */
    HttpErrorWrapperComponent.prototype.destroy$;
    /** @type {?} */
    HttpErrorWrapperComponent.prototype.hideCloseIcon;
    /** @type {?} */
    HttpErrorWrapperComponent.prototype.backgroundColor;
    /** @type {?} */
    HttpErrorWrapperComponent.prototype.containerRef;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaHR0cC1lcnJvci13cmFwcGVyLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvaHR0cC1lcnJvci13cmFwcGVyL2h0dHAtZXJyb3Itd3JhcHBlci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQVUsZ0JBQWdCLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDeEQsT0FBTyxFQUdMLFNBQVMsRUFFVCxVQUFVLEVBTVYsU0FBUyxHQUNWLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBRSxTQUFTLEVBQVcsTUFBTSxNQUFNLENBQUM7QUFDMUMsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUN0RCxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFPdEIsTUFBTSxPQUFPLHlCQUF5QjtJQUx0QztRQVlFLFdBQU0sR0FBRyxDQUFDLENBQUM7UUFFWCxVQUFLLEdBQTZCLE9BQU8sQ0FBQztRQUUxQyxZQUFPLEdBQTZCLDhCQUE4QixDQUFDO1FBRW5FLG9CQUFlLEdBQWMsSUFBSSxDQUFDO1FBSWxDLGtCQUFhLEdBQUcsS0FBSyxDQUFDO0lBMkN4QixDQUFDOzs7O0lBcENDLElBQUksVUFBVTtRQUNaLE9BQU8sSUFBSSxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsSUFBSSxJQUFJLENBQUMsTUFBTSxHQUFHLENBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQztJQUMvQyxDQUFDOzs7O0lBRUQsUUFBUTtRQUNOLElBQUksQ0FBQyxlQUFlO1lBQ2xCLEdBQUc7OztZQUFDLEdBQUcsRUFBRSxDQUFDLE1BQU0sQ0FBQyxnQkFBZ0IsQ0FBQyxRQUFRLENBQUMsSUFBSSxDQUFDLENBQUMsZ0JBQWdCLENBQUMsa0JBQWtCLENBQUMsRUFBQyxJQUFJLE1BQU0sQ0FBQztJQUNyRyxDQUFDOzs7O0lBRUQsZUFBZTtRQUNiLElBQUksSUFBSSxDQUFDLGVBQWUsRUFBRTs7a0JBQ2xCLGtCQUFrQixHQUFHLElBQUksQ0FBQyxLQUFLLENBQUMsdUJBQXVCLENBQUMsSUFBSSxDQUFDLGVBQWUsQ0FBQyxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDO1lBQ3pHLGtCQUFrQixDQUFDLFFBQVEsQ0FBQyxXQUFXLEdBQUcsSUFBSSxDQUFDLE1BQU0sQ0FBQztZQUN0RCxrQkFBa0IsQ0FBQyxRQUFRLENBQUMsUUFBUSxHQUFHLElBQUksQ0FBQyxRQUFRLENBQUM7WUFDckQsSUFBSSxDQUFDLE1BQU0sQ0FBQyxVQUFVLENBQUMsa0JBQWtCLENBQUMsUUFBUSxDQUFDLENBQUM7WUFDcEQsSUFBSSxDQUFDLFlBQVksQ0FBQyxhQUFhLENBQUMsV0FBVyxDQUFDLENBQUMsbUJBQUEsa0JBQWtCLENBQUMsUUFBUSxFQUF3QixDQUFDLENBQUMsU0FBUyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7WUFDaEgsa0JBQWtCLENBQUMsaUJBQWlCLENBQUMsYUFBYSxFQUFFLENBQUM7U0FDdEQ7UUFFRCxTQUFTLENBQUMsUUFBUSxFQUFFLE9BQU8sQ0FBQzthQUN6QixJQUFJLENBQ0gsZ0JBQWdCLENBQUMsSUFBSSxDQUFDLEVBQ3RCLFlBQVksQ0FBQyxHQUFHLENBQUMsRUFDakIsTUFBTTs7OztRQUFDLENBQUMsR0FBa0IsRUFBRSxFQUFFLENBQUMsR0FBRyxJQUFJLEdBQUcsQ0FBQyxHQUFHLEtBQUssUUFBUSxFQUFDLENBQzVEO2FBQ0EsU0FBUzs7O1FBQUMsR0FBRyxFQUFFO1lBQ2QsSUFBSSxDQUFDLE9BQU8sRUFBRSxDQUFDO1FBQ2pCLENBQUMsRUFBQyxDQUFDO0lBQ1AsQ0FBQzs7OztJQUVELFdBQVcsS0FBSSxDQUFDOzs7O0lBRWhCLE9BQU87UUFDTCxJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksRUFBRSxDQUFDO1FBQ3JCLElBQUksQ0FBQyxRQUFRLENBQUMsUUFBUSxFQUFFLENBQUM7SUFDM0IsQ0FBQzs7O1lBaEVGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsd0JBQXdCO2dCQUNsQyxrN0JBQWtEOzthQUVuRDs7OzJCQXNCRSxTQUFTLFNBQUMsV0FBVyxFQUFFLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRTs7OztJQXBCekMsMkNBQXVCOztJQUV2QiwwQ0FBZ0M7O0lBRWhDLDZDQUFtQjs7SUFFbkIsMkNBQVc7O0lBRVgsMENBQTBDOztJQUUxQyw0Q0FBbUU7O0lBRW5FLG9EQUFrQzs7SUFFbEMsNkNBQXdCOztJQUV4QixrREFBc0I7O0lBRXRCLG9EQUF3Qjs7SUFFeEIsaURBQ3lDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29uZmlnLCB0YWtlVW50aWxEZXN0cm95IH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7XG4gIEFmdGVyVmlld0luaXQsXG4gIEFwcGxpY2F0aW9uUmVmLFxuICBDb21wb25lbnQsXG4gIENvbXBvbmVudEZhY3RvcnlSZXNvbHZlcixcbiAgRWxlbWVudFJlZixcbiAgRW1iZWRkZWRWaWV3UmVmLFxuICBJbmplY3RvcixcbiAgT25EZXN0cm95LFxuICBPbkluaXQsXG4gIFR5cGUsXG4gIFZpZXdDaGlsZCxcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBmcm9tRXZlbnQsIFN1YmplY3QgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGRlYm91bmNlVGltZSwgZmlsdGVyIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtaHR0cC1lcnJvci13cmFwcGVyJyxcbiAgdGVtcGxhdGVVcmw6ICcuL2h0dHAtZXJyb3Itd3JhcHBlci5jb21wb25lbnQuaHRtbCcsXG4gIHN0eWxlVXJsczogWydodHRwLWVycm9yLXdyYXBwZXIuY29tcG9uZW50LnNjc3MnXSxcbn0pXG5leHBvcnQgY2xhc3MgSHR0cEVycm9yV3JhcHBlckNvbXBvbmVudCBpbXBsZW1lbnRzIEFmdGVyVmlld0luaXQsIE9uRGVzdHJveSwgT25Jbml0IHtcbiAgYXBwUmVmOiBBcHBsaWNhdGlvblJlZjtcblxuICBjZlJlczogQ29tcG9uZW50RmFjdG9yeVJlc29sdmVyO1xuXG4gIGluamVjdG9yOiBJbmplY3RvcjtcblxuICBzdGF0dXMgPSAwO1xuXG4gIHRpdGxlOiBDb25maWcuTG9jYWxpemF0aW9uUGFyYW0gPSAnT29wcyEnO1xuXG4gIGRldGFpbHM6IENvbmZpZy5Mb2NhbGl6YXRpb25QYXJhbSA9ICdTb3JyeSwgYW4gZXJyb3IgaGFzIG9jY3VyZWQuJztcblxuICBjdXN0b21Db21wb25lbnQ6IFR5cGU8YW55PiA9IG51bGw7XG5cbiAgZGVzdHJveSQ6IFN1YmplY3Q8dm9pZD47XG5cbiAgaGlkZUNsb3NlSWNvbiA9IGZhbHNlO1xuXG4gIGJhY2tncm91bmRDb2xvcjogc3RyaW5nO1xuXG4gIEBWaWV3Q2hpbGQoJ2NvbnRhaW5lcicsIHsgc3RhdGljOiBmYWxzZSB9KVxuICBjb250YWluZXJSZWY6IEVsZW1lbnRSZWY8SFRNTERpdkVsZW1lbnQ+O1xuXG4gIGdldCBzdGF0dXNUZXh0KCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHRoaXMuc3RhdHVzID8gYFske3RoaXMuc3RhdHVzfV1gIDogJyc7XG4gIH1cblxuICBuZ09uSW5pdCgpIHtcbiAgICB0aGlzLmJhY2tncm91bmRDb2xvciA9XG4gICAgICBzbnEoKCkgPT4gd2luZG93LmdldENvbXB1dGVkU3R5bGUoZG9jdW1lbnQuYm9keSkuZ2V0UHJvcGVydHlWYWx1ZSgnYmFja2dyb3VuZC1jb2xvcicpKSB8fCAnI2ZmZic7XG4gIH1cblxuICBuZ0FmdGVyVmlld0luaXQoKSB7XG4gICAgaWYgKHRoaXMuY3VzdG9tQ29tcG9uZW50KSB7XG4gICAgICBjb25zdCBjdXN0b21Db21wb25lbnRSZWYgPSB0aGlzLmNmUmVzLnJlc29sdmVDb21wb25lbnRGYWN0b3J5KHRoaXMuY3VzdG9tQ29tcG9uZW50KS5jcmVhdGUodGhpcy5pbmplY3Rvcik7XG4gICAgICBjdXN0b21Db21wb25lbnRSZWYuaW5zdGFuY2UuZXJyb3JTdGF0dXMgPSB0aGlzLnN0YXR1cztcbiAgICAgIGN1c3RvbUNvbXBvbmVudFJlZi5pbnN0YW5jZS5kZXN0cm95JCA9IHRoaXMuZGVzdHJveSQ7XG4gICAgICB0aGlzLmFwcFJlZi5hdHRhY2hWaWV3KGN1c3RvbUNvbXBvbmVudFJlZi5ob3N0Vmlldyk7XG4gICAgICB0aGlzLmNvbnRhaW5lclJlZi5uYXRpdmVFbGVtZW50LmFwcGVuZENoaWxkKChjdXN0b21Db21wb25lbnRSZWYuaG9zdFZpZXcgYXMgRW1iZWRkZWRWaWV3UmVmPGFueT4pLnJvb3ROb2Rlc1swXSk7XG4gICAgICBjdXN0b21Db21wb25lbnRSZWYuY2hhbmdlRGV0ZWN0b3JSZWYuZGV0ZWN0Q2hhbmdlcygpO1xuICAgIH1cblxuICAgIGZyb21FdmVudChkb2N1bWVudCwgJ2tleXVwJylcbiAgICAgIC5waXBlKFxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxuICAgICAgICBkZWJvdW5jZVRpbWUoMTUwKSxcbiAgICAgICAgZmlsdGVyKChrZXk6IEtleWJvYXJkRXZlbnQpID0+IGtleSAmJiBrZXkua2V5ID09PSAnRXNjYXBlJyksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgdGhpcy5kZXN0cm95KCk7XG4gICAgICB9KTtcbiAgfVxuXG4gIG5nT25EZXN0cm95KCkge31cblxuICBkZXN0cm95KCkge1xuICAgIHRoaXMuZGVzdHJveSQubmV4dCgpO1xuICAgIHRoaXMuZGVzdHJveSQuY29tcGxldGUoKTtcbiAgfVxufVxuIl19