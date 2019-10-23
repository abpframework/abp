/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { StartLoader, StopLoader } from '@abp/ng.core';
import { ChangeDetectorRef, Component, Input } from '@angular/core';
import { NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router';
import { takeUntilDestroy } from '@ngx-validate/core';
import { Actions, ofActionSuccessful } from '@ngxs/store';
import { interval, timer } from 'rxjs';
import { filter } from 'rxjs/operators';
export class LoaderBarComponent {
    /**
     * @param {?} actions
     * @param {?} router
     * @param {?} cdRef
     */
    constructor(actions, router, cdRef) {
        this.actions = actions;
        this.router = router;
        this.cdRef = cdRef;
        this.containerClass = 'abp-loader-bar';
        this.color = '#77b6ff';
        this.isLoading = false;
        this.progressLevel = 0;
        this.filter = (/**
         * @param {?} action
         * @return {?}
         */
        (action) => action.payload.url.indexOf('openid-configuration') < 0);
        actions
            .pipe(ofActionSuccessful(StartLoader, StopLoader), filter(this.filter), takeUntilDestroy(this))
            .subscribe((/**
         * @param {?} action
         * @return {?}
         */
        action => {
            if (action instanceof StartLoader)
                this.startLoading();
            else
                this.stopLoading();
        }));
        router.events
            .pipe(filter((/**
         * @param {?} event
         * @return {?}
         */
        event => event instanceof NavigationStart || event instanceof NavigationEnd || event instanceof NavigationError)), takeUntilDestroy(this))
            .subscribe((/**
         * @param {?} event
         * @return {?}
         */
        event => {
            if (event instanceof NavigationStart)
                this.startLoading();
            else
                this.stopLoading();
        }));
    }
    /**
     * @return {?}
     */
    get boxShadow() {
        return `0 0 10px rgba(${this.color}, 0.5)`;
    }
    /**
     * @return {?}
     */
    ngOnDestroy() {
        this.interval.unsubscribe();
    }
    /**
     * @return {?}
     */
    startLoading() {
        if (this.isLoading || this.progressLevel !== 0)
            return;
        this.isLoading = true;
        this.interval = interval(350).subscribe((/**
         * @return {?}
         */
        () => {
            if (this.progressLevel < 75) {
                this.progressLevel += Math.random() * 10;
            }
            else if (this.progressLevel < 90) {
                this.progressLevel += 0.4;
            }
            else if (this.progressLevel < 100) {
                this.progressLevel += 0.1;
            }
            else {
                this.interval.unsubscribe();
            }
            this.cdRef.detectChanges();
        }));
    }
    /**
     * @return {?}
     */
    stopLoading() {
        this.interval.unsubscribe();
        this.progressLevel = 100;
        this.isLoading = false;
        if (this.timer && !this.timer.closed)
            return;
        this.timer = timer(820).subscribe((/**
         * @return {?}
         */
        () => {
            this.progressLevel = 0;
            this.cdRef.detectChanges();
        }));
    }
}
LoaderBarComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-loader-bar',
                template: `
    <div id="abp-loader-bar" [ngClass]="containerClass" [class.is-loading]="isLoading">
      <div
        class="abp-progress"
        [style.width.vw]="progressLevel"
        [ngStyle]="{
          'background-color': color,
          'box-shadow': boxShadow
        }"
      ></div>
    </div>
  `,
                styles: [".abp-loader-bar{left:0;opacity:0;position:fixed;top:0;transition:opacity .4s linear .4s;z-index:99999}.abp-loader-bar.is-loading{opacity:1;transition:none}.abp-loader-bar .abp-progress{height:3px;left:0;position:fixed;top:0;transition:width .4s}"]
            }] }
];
/** @nocollapse */
LoaderBarComponent.ctorParameters = () => [
    { type: Actions },
    { type: Router },
    { type: ChangeDetectorRef }
];
LoaderBarComponent.propDecorators = {
    containerClass: [{ type: Input }],
    color: [{ type: Input }],
    isLoading: [{ type: Input }],
    filter: [{ type: Input }]
};
if (false) {
    /** @type {?} */
    LoaderBarComponent.prototype.containerClass;
    /** @type {?} */
    LoaderBarComponent.prototype.color;
    /** @type {?} */
    LoaderBarComponent.prototype.isLoading;
    /** @type {?} */
    LoaderBarComponent.prototype.progressLevel;
    /** @type {?} */
    LoaderBarComponent.prototype.interval;
    /** @type {?} */
    LoaderBarComponent.prototype.timer;
    /** @type {?} */
    LoaderBarComponent.prototype.filter;
    /**
     * @type {?}
     * @private
     */
    LoaderBarComponent.prototype.actions;
    /**
     * @type {?}
     * @private
     */
    LoaderBarComponent.prototype.router;
    /**
     * @type {?}
     * @private
     */
    LoaderBarComponent.prototype.cdRef;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9hZGVyLWJhci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2xvYWRlci1iYXIvbG9hZGVyLWJhci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxXQUFXLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3ZELE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFhLE1BQU0sZUFBZSxDQUFDO0FBQy9FLE9BQU8sRUFBRSxhQUFhLEVBQUUsZUFBZSxFQUFFLGVBQWUsRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUMxRixPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzFELE9BQU8sRUFBRSxRQUFRLEVBQWdCLEtBQUssRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNyRCxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFrQnhDLE1BQU0sT0FBTyxrQkFBa0I7Ozs7OztJQUs3QixZQUFvQixPQUFnQixFQUFVLE1BQWMsRUFBVSxLQUF3QjtRQUExRSxZQUFPLEdBQVAsT0FBTyxDQUFTO1FBQVUsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQW1CO1FBMEI5RixtQkFBYyxHQUFHLGdCQUFnQixDQUFDO1FBR2xDLFVBQUssR0FBRyxTQUFTLENBQUM7UUFHbEIsY0FBUyxHQUFHLEtBQUssQ0FBQztRQUVsQixrQkFBYSxHQUFHLENBQUMsQ0FBQztRQU9sQixXQUFNOzs7O1FBQUcsQ0FBQyxNQUFnQyxFQUFFLEVBQUUsQ0FBQyxNQUFNLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxPQUFPLENBQUMsc0JB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QlEsT0FBTztZQUYwQyxNQUFNO1lBRHZELGlCQUFpQjs7OzZCQXFEdkIsS0FBSztvQkFHTCxLQUFLO3dCQUdMLEtBQUs7cUJBU0wsS0FBSzs7OztJQWZOLDRDQUNrQzs7SUFFbEMsbUNBQ2tCOztJQUVsQix1Q0FDa0I7O0lBRWxCLDJDQUFrQjs7SUFFbEIsc0NBQXVCOztJQUV2QixtQ0FBb0I7O0lBRXBCLG9DQUNzRzs7Ozs7SUF6QzFGLHFDQUF3Qjs7Ozs7SUFBRSxvQ0FBc0I7Ozs7O0lBQUUsbUNBQWdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhcnRMb2FkZXIsIFN0b3BMb2FkZXIgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5pbXBvcnQgeyBDaGFuZ2VEZXRlY3RvclJlZiwgQ29tcG9uZW50LCBJbnB1dCwgT25EZXN0cm95IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IE5hdmlnYXRpb25FbmQsIE5hdmlnYXRpb25FcnJvciwgTmF2aWdhdGlvblN0YXJ0LCBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xyXG5pbXBvcnQgeyB0YWtlVW50aWxEZXN0cm95IH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcclxuaW1wb3J0IHsgQWN0aW9ucywgb2ZBY3Rpb25TdWNjZXNzZnVsIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBpbnRlcnZhbCwgU3Vic2NyaXB0aW9uLCB0aW1lciB9IGZyb20gJ3J4anMnO1xyXG5pbXBvcnQgeyBmaWx0ZXIgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1sb2FkZXItYmFyJyxcclxuICB0ZW1wbGF0ZTogYFxyXG4gICAgPGRpdiBpZD1cImFicC1sb2FkZXItYmFyXCIgW25nQ2xhc3NdPVwiY29udGFpbmVyQ2xhc3NcIiBbY2xhc3MuaXMtbG9hZGluZ109XCJpc0xvYWRpbmdcIj5cclxuICAgICAgPGRpdlxyXG4gICAgICAgIGNsYXNzPVwiYWJwLXByb2dyZXNzXCJcclxuICAgICAgICBbc3R5bGUud2lkdGgudnddPVwicHJvZ3Jlc3NMZXZlbFwiXHJcbiAgICAgICAgW25nU3R5bGVdPVwie1xyXG4gICAgICAgICAgJ2JhY2tncm91bmQtY29sb3InOiBjb2xvcixcclxuICAgICAgICAgICdib3gtc2hhZG93JzogYm94U2hhZG93XHJcbiAgICAgICAgfVwiXHJcbiAgICAgID48L2Rpdj5cclxuICAgIDwvZGl2PlxyXG4gIGAsXHJcbiAgc3R5bGVVcmxzOiBbJy4vbG9hZGVyLWJhci5jb21wb25lbnQuc2NzcyddXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBMb2FkZXJCYXJDb21wb25lbnQgaW1wbGVtZW50cyBPbkRlc3Ryb3kge1xyXG4gIGdldCBib3hTaGFkb3coKTogc3RyaW5nIHtcclxuICAgIHJldHVybiBgMCAwIDEwcHggcmdiYSgke3RoaXMuY29sb3J9LCAwLjUpYDtcclxuICB9XHJcblxyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgYWN0aW9uczogQWN0aW9ucywgcHJpdmF0ZSByb3V0ZXI6IFJvdXRlciwgcHJpdmF0ZSBjZFJlZjogQ2hhbmdlRGV0ZWN0b3JSZWYpIHtcclxuICAgIGFjdGlvbnNcclxuICAgICAgLnBpcGUoXHJcbiAgICAgICAgb2ZBY3Rpb25TdWNjZXNzZnVsKFN0YXJ0TG9hZGVyLCBTdG9wTG9hZGVyKSxcclxuICAgICAgICBmaWx0ZXIodGhpcy5maWx0ZXIpLFxyXG4gICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcylcclxuICAgICAgKVxyXG4gICAgICAuc3Vic2NyaWJlKGFjdGlvbiA9PiB7XHJcbiAgICAgICAgaWYgKGFjdGlvbiBpbnN0YW5jZW9mIFN0YXJ0TG9hZGVyKSB0aGlzLnN0YXJ0TG9hZGluZygpO1xyXG4gICAgICAgIGVsc2UgdGhpcy5zdG9wTG9hZGluZygpO1xyXG4gICAgICB9KTtcclxuXHJcbiAgICByb3V0ZXIuZXZlbnRzXHJcbiAgICAgIC5waXBlKFxyXG4gICAgICAgIGZpbHRlcihcclxuICAgICAgICAgIGV2ZW50ID0+XHJcbiAgICAgICAgICAgIGV2ZW50IGluc3RhbmNlb2YgTmF2aWdhdGlvblN0YXJ0IHx8IGV2ZW50IGluc3RhbmNlb2YgTmF2aWdhdGlvbkVuZCB8fCBldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25FcnJvclxyXG4gICAgICAgICksXHJcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKVxyXG4gICAgICApXHJcbiAgICAgIC5zdWJzY3JpYmUoZXZlbnQgPT4ge1xyXG4gICAgICAgIGlmIChldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25TdGFydCkgdGhpcy5zdGFydExvYWRpbmcoKTtcclxuICAgICAgICBlbHNlIHRoaXMuc3RvcExvYWRpbmcoKTtcclxuICAgICAgfSk7XHJcbiAgfVxyXG4gIEBJbnB1dCgpXHJcbiAgY29udGFpbmVyQ2xhc3MgPSAnYWJwLWxvYWRlci1iYXInO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGNvbG9yID0gJyM3N2I2ZmYnO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGlzTG9hZGluZyA9IGZhbHNlO1xyXG5cclxuICBwcm9ncmVzc0xldmVsID0gMDtcclxuXHJcbiAgaW50ZXJ2YWw6IFN1YnNjcmlwdGlvbjtcclxuXHJcbiAgdGltZXI6IFN1YnNjcmlwdGlvbjtcclxuXHJcbiAgQElucHV0KClcclxuICBmaWx0ZXIgPSAoYWN0aW9uOiBTdGFydExvYWRlciB8IFN0b3BMb2FkZXIpID0+IGFjdGlvbi5wYXlsb2FkLnVybC5pbmRleE9mKCdvcGVuaWQtY29uZmlndXJhdGlvbicpIDwgMDtcclxuXHJcbiAgbmdPbkRlc3Ryb3koKSB7XHJcbiAgICB0aGlzLmludGVydmFsLnVuc3Vic2NyaWJlKCk7XHJcbiAgfVxyXG5cclxuICBzdGFydExvYWRpbmcoKSB7XHJcbiAgICBpZiAodGhpcy5pc0xvYWRpbmcgfHwgdGhpcy5wcm9ncmVzc0xldmVsICE9PSAwKSByZXR1cm47XHJcblxyXG4gICAgdGhpcy5pc0xvYWRpbmcgPSB0cnVlO1xyXG4gICAgdGhpcy5pbnRlcnZhbCA9IGludGVydmFsKDM1MCkuc3Vic2NyaWJlKCgpID0+IHtcclxuICAgICAgaWYgKHRoaXMucHJvZ3Jlc3NMZXZlbCA8IDc1KSB7XHJcbiAgICAgICAgdGhpcy5wcm9ncmVzc0xldmVsICs9IE1hdGgucmFuZG9tKCkgKiAxMDtcclxuICAgICAgfSBlbHNlIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCA5MCkge1xyXG4gICAgICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCArPSAwLjQ7XHJcbiAgICAgIH0gZWxzZSBpZiAodGhpcy5wcm9ncmVzc0xldmVsIDwgMTAwKSB7XHJcbiAgICAgICAgdGhpcy5wcm9ncmVzc0xldmVsICs9IDAuMTtcclxuICAgICAgfSBlbHNlIHtcclxuICAgICAgICB0aGlzLmludGVydmFsLnVuc3Vic2NyaWJlKCk7XHJcbiAgICAgIH1cclxuICAgICAgdGhpcy5jZFJlZi5kZXRlY3RDaGFuZ2VzKCk7XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIHN0b3BMb2FkaW5nKCkge1xyXG4gICAgdGhpcy5pbnRlcnZhbC51bnN1YnNjcmliZSgpO1xyXG4gICAgdGhpcy5wcm9ncmVzc0xldmVsID0gMTAwO1xyXG4gICAgdGhpcy5pc0xvYWRpbmcgPSBmYWxzZTtcclxuICAgIGlmICh0aGlzLnRpbWVyICYmICF0aGlzLnRpbWVyLmNsb3NlZCkgcmV0dXJuO1xyXG5cclxuICAgIHRoaXMudGltZXIgPSB0aW1lcig4MjApLnN1YnNjcmliZSgoKSA9PiB7XHJcbiAgICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCA9IDA7XHJcbiAgICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xyXG4gICAgfSk7XHJcbiAgfVxyXG59XHJcbiJdfQ==