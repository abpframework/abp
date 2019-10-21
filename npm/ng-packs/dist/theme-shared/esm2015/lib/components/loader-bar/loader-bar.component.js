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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9hZGVyLWJhci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2xvYWRlci1iYXIvbG9hZGVyLWJhci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxXQUFXLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3ZELE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFhLE1BQU0sZUFBZSxDQUFDO0FBQy9FLE9BQU8sRUFBRSxhQUFhLEVBQUUsZUFBZSxFQUFFLGVBQWUsRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUMxRixPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzFELE9BQU8sRUFBRSxRQUFRLEVBQWdCLEtBQUssRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNyRCxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFrQnhDLE1BQU0sT0FBTyxrQkFBa0I7Ozs7OztJQUs3QixZQUFvQixPQUFnQixFQUFVLE1BQWMsRUFBVSxLQUF3QjtRQUExRSxZQUFPLEdBQVAsT0FBTyxDQUFTO1FBQVUsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQW1CO1FBMEI5RixtQkFBYyxHQUFHLGdCQUFnQixDQUFDO1FBR2xDLFVBQUssR0FBRyxTQUFTLENBQUM7UUFHbEIsY0FBUyxHQUFHLEtBQUssQ0FBQztRQUVsQixrQkFBYSxHQUFHLENBQUMsQ0FBQztRQU9sQixXQUFNOzs7O1FBQUcsQ0FBQyxNQUFnQyxFQUFFLEVBQUUsQ0FBQyxNQUFNLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxPQUFPLENBQUMsc0JB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QlEsT0FBTztZQUYwQyxNQUFNO1lBRHZELGlCQUFpQjs7OzZCQXFEdkIsS0FBSztvQkFHTCxLQUFLO3dCQUdMLEtBQUs7cUJBU0wsS0FBSzs7OztJQWZOLDRDQUNrQzs7SUFFbEMsbUNBQ2tCOztJQUVsQix1Q0FDa0I7O0lBRWxCLDJDQUFrQjs7SUFFbEIsc0NBQXVCOztJQUV2QixtQ0FBb0I7O0lBRXBCLG9DQUNzRzs7Ozs7SUF6QzFGLHFDQUF3Qjs7Ozs7SUFBRSxvQ0FBc0I7Ozs7O0lBQUUsbUNBQWdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhcnRMb2FkZXIsIFN0b3BMb2FkZXIgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgQ2hhbmdlRGV0ZWN0b3JSZWYsIENvbXBvbmVudCwgSW5wdXQsIE9uRGVzdHJveSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmF2aWdhdGlvbkVuZCwgTmF2aWdhdGlvbkVycm9yLCBOYXZpZ2F0aW9uU3RhcnQsIFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyB0YWtlVW50aWxEZXN0cm95IH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcbmltcG9ydCB7IEFjdGlvbnMsIG9mQWN0aW9uU3VjY2Vzc2Z1bCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IGludGVydmFsLCBTdWJzY3JpcHRpb24sIHRpbWVyIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBmaWx0ZXIgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1sb2FkZXItYmFyJyxcbiAgdGVtcGxhdGU6IGBcbiAgICA8ZGl2IGlkPVwiYWJwLWxvYWRlci1iYXJcIiBbbmdDbGFzc109XCJjb250YWluZXJDbGFzc1wiIFtjbGFzcy5pcy1sb2FkaW5nXT1cImlzTG9hZGluZ1wiPlxuICAgICAgPGRpdlxuICAgICAgICBjbGFzcz1cImFicC1wcm9ncmVzc1wiXG4gICAgICAgIFtzdHlsZS53aWR0aC52d109XCJwcm9ncmVzc0xldmVsXCJcbiAgICAgICAgW25nU3R5bGVdPVwie1xuICAgICAgICAgICdiYWNrZ3JvdW5kLWNvbG9yJzogY29sb3IsXG4gICAgICAgICAgJ2JveC1zaGFkb3cnOiBib3hTaGFkb3dcbiAgICAgICAgfVwiXG4gICAgICA+PC9kaXY+XG4gICAgPC9kaXY+XG4gIGAsXG4gIHN0eWxlVXJsczogWycuL2xvYWRlci1iYXIuY29tcG9uZW50LnNjc3MnXVxufSlcbmV4cG9ydCBjbGFzcyBMb2FkZXJCYXJDb21wb25lbnQgaW1wbGVtZW50cyBPbkRlc3Ryb3kge1xuICBnZXQgYm94U2hhZG93KCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIGAwIDAgMTBweCByZ2JhKCR7dGhpcy5jb2xvcn0sIDAuNSlgO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBhY3Rpb25zOiBBY3Rpb25zLCBwcml2YXRlIHJvdXRlcjogUm91dGVyLCBwcml2YXRlIGNkUmVmOiBDaGFuZ2VEZXRlY3RvclJlZikge1xuICAgIGFjdGlvbnNcbiAgICAgIC5waXBlKFxuICAgICAgICBvZkFjdGlvblN1Y2Nlc3NmdWwoU3RhcnRMb2FkZXIsIFN0b3BMb2FkZXIpLFxuICAgICAgICBmaWx0ZXIodGhpcy5maWx0ZXIpLFxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKGFjdGlvbiA9PiB7XG4gICAgICAgIGlmIChhY3Rpb24gaW5zdGFuY2VvZiBTdGFydExvYWRlcikgdGhpcy5zdGFydExvYWRpbmcoKTtcbiAgICAgICAgZWxzZSB0aGlzLnN0b3BMb2FkaW5nKCk7XG4gICAgICB9KTtcblxuICAgIHJvdXRlci5ldmVudHNcbiAgICAgIC5waXBlKFxuICAgICAgICBmaWx0ZXIoXG4gICAgICAgICAgZXZlbnQgPT5cbiAgICAgICAgICAgIGV2ZW50IGluc3RhbmNlb2YgTmF2aWdhdGlvblN0YXJ0IHx8IGV2ZW50IGluc3RhbmNlb2YgTmF2aWdhdGlvbkVuZCB8fCBldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25FcnJvclxuICAgICAgICApLFxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKGV2ZW50ID0+IHtcbiAgICAgICAgaWYgKGV2ZW50IGluc3RhbmNlb2YgTmF2aWdhdGlvblN0YXJ0KSB0aGlzLnN0YXJ0TG9hZGluZygpO1xuICAgICAgICBlbHNlIHRoaXMuc3RvcExvYWRpbmcoKTtcbiAgICAgIH0pO1xuICB9XG4gIEBJbnB1dCgpXG4gIGNvbnRhaW5lckNsYXNzID0gJ2FicC1sb2FkZXItYmFyJztcblxuICBASW5wdXQoKVxuICBjb2xvciA9ICcjNzdiNmZmJztcblxuICBASW5wdXQoKVxuICBpc0xvYWRpbmcgPSBmYWxzZTtcblxuICBwcm9ncmVzc0xldmVsID0gMDtcblxuICBpbnRlcnZhbDogU3Vic2NyaXB0aW9uO1xuXG4gIHRpbWVyOiBTdWJzY3JpcHRpb247XG5cbiAgQElucHV0KClcbiAgZmlsdGVyID0gKGFjdGlvbjogU3RhcnRMb2FkZXIgfCBTdG9wTG9hZGVyKSA9PiBhY3Rpb24ucGF5bG9hZC51cmwuaW5kZXhPZignb3BlbmlkLWNvbmZpZ3VyYXRpb24nKSA8IDA7XG5cbiAgbmdPbkRlc3Ryb3koKSB7XG4gICAgdGhpcy5pbnRlcnZhbC51bnN1YnNjcmliZSgpO1xuICB9XG5cbiAgc3RhcnRMb2FkaW5nKCkge1xuICAgIGlmICh0aGlzLmlzTG9hZGluZyB8fCB0aGlzLnByb2dyZXNzTGV2ZWwgIT09IDApIHJldHVybjtcblxuICAgIHRoaXMuaXNMb2FkaW5nID0gdHJ1ZTtcbiAgICB0aGlzLmludGVydmFsID0gaW50ZXJ2YWwoMzUwKS5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgaWYgKHRoaXMucHJvZ3Jlc3NMZXZlbCA8IDc1KSB7XG4gICAgICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCArPSBNYXRoLnJhbmRvbSgpICogMTA7XG4gICAgICB9IGVsc2UgaWYgKHRoaXMucHJvZ3Jlc3NMZXZlbCA8IDkwKSB7XG4gICAgICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCArPSAwLjQ7XG4gICAgICB9IGVsc2UgaWYgKHRoaXMucHJvZ3Jlc3NMZXZlbCA8IDEwMCkge1xuICAgICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgKz0gMC4xO1xuICAgICAgfSBlbHNlIHtcbiAgICAgICAgdGhpcy5pbnRlcnZhbC51bnN1YnNjcmliZSgpO1xuICAgICAgfVxuICAgICAgdGhpcy5jZFJlZi5kZXRlY3RDaGFuZ2VzKCk7XG4gICAgfSk7XG4gIH1cblxuICBzdG9wTG9hZGluZygpIHtcbiAgICB0aGlzLmludGVydmFsLnVuc3Vic2NyaWJlKCk7XG4gICAgdGhpcy5wcm9ncmVzc0xldmVsID0gMTAwO1xuICAgIHRoaXMuaXNMb2FkaW5nID0gZmFsc2U7XG4gICAgaWYgKHRoaXMudGltZXIgJiYgIXRoaXMudGltZXIuY2xvc2VkKSByZXR1cm47XG5cbiAgICB0aGlzLnRpbWVyID0gdGltZXIoODIwKS5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgdGhpcy5wcm9ncmVzc0xldmVsID0gMDtcbiAgICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xuICAgIH0pO1xuICB9XG59XG4iXX0=