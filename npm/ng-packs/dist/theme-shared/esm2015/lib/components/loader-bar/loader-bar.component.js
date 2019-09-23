/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
        this.filter = (/**
         * @param {?} action
         * @return {?}
         */
        (action) => action.payload.url.indexOf('openid-configuration') < 0);
        this.progressLevel = 0;
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
    LoaderBarComponent.prototype.filter;
    /** @type {?} */
    LoaderBarComponent.prototype.progressLevel;
    /** @type {?} */
    LoaderBarComponent.prototype.interval;
    /** @type {?} */
    LoaderBarComponent.prototype.timer;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9hZGVyLWJhci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2xvYWRlci1iYXIvbG9hZGVyLWJhci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxXQUFXLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3ZELE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFhLE1BQU0sZUFBZSxDQUFDO0FBQy9FLE9BQU8sRUFBRSxhQUFhLEVBQUUsZUFBZSxFQUFFLGVBQWUsRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUMxRixPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzFELE9BQU8sRUFBRSxRQUFRLEVBQWdCLEtBQUssRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNyRCxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFrQnhDLE1BQU0sT0FBTyxrQkFBa0I7Ozs7OztJQXVCN0IsWUFBb0IsT0FBZ0IsRUFBVSxNQUFjLEVBQVUsS0FBd0I7UUFBMUUsWUFBTyxHQUFQLE9BQU8sQ0FBUztRQUFVLFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFtQjtRQXJCOUYsbUJBQWMsR0FBVyxnQkFBZ0IsQ0FBQztRQUcxQyxVQUFLLEdBQVcsU0FBUyxDQUFDO1FBRzFCLGNBQVMsR0FBWSxLQUFLLENBQUM7UUFHM0IsV0FBTTs7OztRQUFHLENBQUMsTUFBZ0MsRUFBRSxFQUFFLENBQUMsTUFBTSxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsT0FBTyxDQUFDLHNCQUFzQixDQUFDLEdBQUcsQ0FBQyxFQUFDO1FB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QlEsT0FBTztZQUYwQyxNQUFNO1lBRHZELGlCQUFpQjs7OzZCQXdCdkIsS0FBSztvQkFHTCxLQUFLO3dCQUdMLEtBQUs7cUJBR0wsS0FBSzs7OztJQVROLDRDQUMwQzs7SUFFMUMsbUNBQzBCOztJQUUxQix1Q0FDMkI7O0lBRTNCLG9DQUNzRzs7SUFFdEcsMkNBQTBCOztJQUUxQixzQ0FBdUI7O0lBRXZCLG1DQUFvQjs7Ozs7SUFNUixxQ0FBd0I7Ozs7O0lBQUUsb0NBQXNCOzs7OztJQUFFLG1DQUFnQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFN0YXJ0TG9hZGVyLCBTdG9wTG9hZGVyIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IENoYW5nZURldGVjdG9yUmVmLCBDb21wb25lbnQsIElucHV0LCBPbkRlc3Ryb3kgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5hdmlnYXRpb25FbmQsIE5hdmlnYXRpb25FcnJvciwgTmF2aWdhdGlvblN0YXJ0LCBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5pbXBvcnQgeyBBY3Rpb25zLCBvZkFjdGlvblN1Y2Nlc3NmdWwgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBpbnRlcnZhbCwgU3Vic2NyaXB0aW9uLCB0aW1lciB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgZmlsdGVyIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtbG9hZGVyLWJhcicsXG4gIHRlbXBsYXRlOiBgXG4gICAgPGRpdiBpZD1cImFicC1sb2FkZXItYmFyXCIgW25nQ2xhc3NdPVwiY29udGFpbmVyQ2xhc3NcIiBbY2xhc3MuaXMtbG9hZGluZ109XCJpc0xvYWRpbmdcIj5cbiAgICAgIDxkaXZcbiAgICAgICAgY2xhc3M9XCJhYnAtcHJvZ3Jlc3NcIlxuICAgICAgICBbc3R5bGUud2lkdGgudnddPVwicHJvZ3Jlc3NMZXZlbFwiXG4gICAgICAgIFtuZ1N0eWxlXT1cIntcbiAgICAgICAgICAnYmFja2dyb3VuZC1jb2xvcic6IGNvbG9yLFxuICAgICAgICAgICdib3gtc2hhZG93JzogYm94U2hhZG93XG4gICAgICAgIH1cIlxuICAgICAgPjwvZGl2PlxuICAgIDwvZGl2PlxuICBgLFxuICBzdHlsZVVybHM6IFsnLi9sb2FkZXItYmFyLmNvbXBvbmVudC5zY3NzJ10sXG59KVxuZXhwb3J0IGNsYXNzIExvYWRlckJhckNvbXBvbmVudCBpbXBsZW1lbnRzIE9uRGVzdHJveSB7XG4gIEBJbnB1dCgpXG4gIGNvbnRhaW5lckNsYXNzOiBzdHJpbmcgPSAnYWJwLWxvYWRlci1iYXInO1xuXG4gIEBJbnB1dCgpXG4gIGNvbG9yOiBzdHJpbmcgPSAnIzc3YjZmZic7XG5cbiAgQElucHV0KClcbiAgaXNMb2FkaW5nOiBib29sZWFuID0gZmFsc2U7XG5cbiAgQElucHV0KClcbiAgZmlsdGVyID0gKGFjdGlvbjogU3RhcnRMb2FkZXIgfCBTdG9wTG9hZGVyKSA9PiBhY3Rpb24ucGF5bG9hZC51cmwuaW5kZXhPZignb3BlbmlkLWNvbmZpZ3VyYXRpb24nKSA8IDA7XG5cbiAgcHJvZ3Jlc3NMZXZlbDogbnVtYmVyID0gMDtcblxuICBpbnRlcnZhbDogU3Vic2NyaXB0aW9uO1xuXG4gIHRpbWVyOiBTdWJzY3JpcHRpb247XG5cbiAgZ2V0IGJveFNoYWRvdygpOiBzdHJpbmcge1xuICAgIHJldHVybiBgMCAwIDEwcHggcmdiYSgke3RoaXMuY29sb3J9LCAwLjUpYDtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgYWN0aW9uczogQWN0aW9ucywgcHJpdmF0ZSByb3V0ZXI6IFJvdXRlciwgcHJpdmF0ZSBjZFJlZjogQ2hhbmdlRGV0ZWN0b3JSZWYpIHtcbiAgICBhY3Rpb25zXG4gICAgICAucGlwZShcbiAgICAgICAgb2ZBY3Rpb25TdWNjZXNzZnVsKFN0YXJ0TG9hZGVyLCBTdG9wTG9hZGVyKSxcbiAgICAgICAgZmlsdGVyKHRoaXMuZmlsdGVyKSxcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoYWN0aW9uID0+IHtcbiAgICAgICAgaWYgKGFjdGlvbiBpbnN0YW5jZW9mIFN0YXJ0TG9hZGVyKSB0aGlzLnN0YXJ0TG9hZGluZygpO1xuICAgICAgICBlbHNlIHRoaXMuc3RvcExvYWRpbmcoKTtcbiAgICAgIH0pO1xuXG4gICAgcm91dGVyLmV2ZW50c1xuICAgICAgLnBpcGUoXG4gICAgICAgIGZpbHRlcihcbiAgICAgICAgICBldmVudCA9PlxuICAgICAgICAgICAgZXZlbnQgaW5zdGFuY2VvZiBOYXZpZ2F0aW9uU3RhcnQgfHwgZXZlbnQgaW5zdGFuY2VvZiBOYXZpZ2F0aW9uRW5kIHx8IGV2ZW50IGluc3RhbmNlb2YgTmF2aWdhdGlvbkVycm9yLFxuICAgICAgICApLFxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZShldmVudCA9PiB7XG4gICAgICAgIGlmIChldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25TdGFydCkgdGhpcy5zdGFydExvYWRpbmcoKTtcbiAgICAgICAgZWxzZSB0aGlzLnN0b3BMb2FkaW5nKCk7XG4gICAgICB9KTtcbiAgfVxuXG4gIG5nT25EZXN0cm95KCkge1xuICAgIHRoaXMuaW50ZXJ2YWwudW5zdWJzY3JpYmUoKTtcbiAgfVxuXG4gIHN0YXJ0TG9hZGluZygpIHtcbiAgICBpZiAodGhpcy5pc0xvYWRpbmcgfHwgdGhpcy5wcm9ncmVzc0xldmVsICE9PSAwKSByZXR1cm47XG5cbiAgICB0aGlzLmlzTG9hZGluZyA9IHRydWU7XG4gICAgdGhpcy5pbnRlcnZhbCA9IGludGVydmFsKDM1MCkuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCA3NSkge1xuICAgICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgKz0gTWF0aC5yYW5kb20oKSAqIDEwO1xuICAgICAgfSBlbHNlIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCA5MCkge1xuICAgICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgKz0gMC40O1xuICAgICAgfSBlbHNlIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCAxMDApIHtcbiAgICAgICAgdGhpcy5wcm9ncmVzc0xldmVsICs9IDAuMTtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIHRoaXMuaW50ZXJ2YWwudW5zdWJzY3JpYmUoKTtcbiAgICAgIH1cbiAgICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xuICAgIH0pO1xuICB9XG5cbiAgc3RvcExvYWRpbmcoKSB7XG4gICAgdGhpcy5pbnRlcnZhbC51bnN1YnNjcmliZSgpO1xuICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCA9IDEwMDtcbiAgICB0aGlzLmlzTG9hZGluZyA9IGZhbHNlO1xuICAgIGlmICh0aGlzLnRpbWVyICYmICF0aGlzLnRpbWVyLmNsb3NlZCkgcmV0dXJuO1xuXG4gICAgdGhpcy50aW1lciA9IHRpbWVyKDgyMCkuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCA9IDA7XG4gICAgICB0aGlzLmNkUmVmLmRldGVjdENoYW5nZXMoKTtcbiAgICB9KTtcbiAgfVxufVxuIl19