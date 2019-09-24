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
var LoaderBarComponent = /** @class */ (function () {
    function LoaderBarComponent(actions, router, cdRef) {
        var _this = this;
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
        function (action) { return action.payload.url.indexOf('openid-configuration') < 0; });
        this.progressLevel = 0;
        actions
            .pipe(ofActionSuccessful(StartLoader, StopLoader), filter(this.filter), takeUntilDestroy(this))
            .subscribe((/**
         * @param {?} action
         * @return {?}
         */
        function (action) {
            if (action instanceof StartLoader)
                _this.startLoading();
            else
                _this.stopLoading();
        }));
        router.events
            .pipe(filter((/**
         * @param {?} event
         * @return {?}
         */
        function (event) {
            return event instanceof NavigationStart || event instanceof NavigationEnd || event instanceof NavigationError;
        })), takeUntilDestroy(this))
            .subscribe((/**
         * @param {?} event
         * @return {?}
         */
        function (event) {
            if (event instanceof NavigationStart)
                _this.startLoading();
            else
                _this.stopLoading();
        }));
    }
    Object.defineProperty(LoaderBarComponent.prototype, "boxShadow", {
        get: /**
         * @return {?}
         */
        function () {
            return "0 0 10px rgba(" + this.color + ", 0.5)";
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @return {?}
     */
    LoaderBarComponent.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () {
        this.interval.unsubscribe();
    };
    /**
     * @return {?}
     */
    LoaderBarComponent.prototype.startLoading = /**
     * @return {?}
     */
    function () {
        var _this = this;
        if (this.isLoading || this.progressLevel !== 0)
            return;
        this.isLoading = true;
        this.interval = interval(350).subscribe((/**
         * @return {?}
         */
        function () {
            if (_this.progressLevel < 75) {
                _this.progressLevel += Math.random() * 10;
            }
            else if (_this.progressLevel < 90) {
                _this.progressLevel += 0.4;
            }
            else if (_this.progressLevel < 100) {
                _this.progressLevel += 0.1;
            }
            else {
                _this.interval.unsubscribe();
            }
            _this.cdRef.detectChanges();
        }));
    };
    /**
     * @return {?}
     */
    LoaderBarComponent.prototype.stopLoading = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.interval.unsubscribe();
        this.progressLevel = 100;
        this.isLoading = false;
        if (this.timer && !this.timer.closed)
            return;
        this.timer = timer(820).subscribe((/**
         * @return {?}
         */
        function () {
            _this.progressLevel = 0;
            _this.cdRef.detectChanges();
        }));
    };
    LoaderBarComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-loader-bar',
                    template: "\n    <div id=\"abp-loader-bar\" [ngClass]=\"containerClass\" [class.is-loading]=\"isLoading\">\n      <div\n        class=\"abp-progress\"\n        [style.width.vw]=\"progressLevel\"\n        [ngStyle]=\"{\n          'background-color': color,\n          'box-shadow': boxShadow\n        }\"\n      ></div>\n    </div>\n  ",
                    styles: [".abp-loader-bar{left:0;opacity:0;position:fixed;top:0;transition:opacity .4s linear .4s;z-index:99999}.abp-loader-bar.is-loading{opacity:1;transition:none}.abp-loader-bar .abp-progress{height:3px;left:0;position:fixed;top:0;transition:width .4s}"]
                }] }
    ];
    /** @nocollapse */
    LoaderBarComponent.ctorParameters = function () { return [
        { type: Actions },
        { type: Router },
        { type: ChangeDetectorRef }
    ]; };
    LoaderBarComponent.propDecorators = {
        containerClass: [{ type: Input }],
        color: [{ type: Input }],
        isLoading: [{ type: Input }],
        filter: [{ type: Input }]
    };
    return LoaderBarComponent;
}());
export { LoaderBarComponent };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9hZGVyLWJhci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2xvYWRlci1iYXIvbG9hZGVyLWJhci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxXQUFXLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3ZELE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFhLE1BQU0sZUFBZSxDQUFDO0FBQy9FLE9BQU8sRUFBRSxhQUFhLEVBQUUsZUFBZSxFQUFFLGVBQWUsRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUMxRixPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzFELE9BQU8sRUFBRSxRQUFRLEVBQWdCLEtBQUssRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNyRCxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFFeEM7SUF1Q0UsNEJBQW9CLE9BQWdCLEVBQVUsTUFBYyxFQUFVLEtBQXdCO1FBQTlGLGlCQXdCQztRQXhCbUIsWUFBTyxHQUFQLE9BQU8sQ0FBUztRQUFVLFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFtQjtRQXJCOUYsbUJBQWMsR0FBVyxnQkFBZ0IsQ0FBQztRQUcxQyxVQUFLLEdBQVcsU0FBUyxDQUFDO1FBRzFCLGNBQVMsR0FBWSxLQUFLLENBQUM7UUFHM0IsV0FBTTs7OztRQUFHLFVBQUMsTUFBZ0MsSUFBSyxPQUFBLE1BQU0sQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLE9BQU8sQ0FBQyxzQkFBc0IsQ0FBQyxHQUFHLENBQUMsRUFBdEQsQ0FBc0QsRUFBQztRQUV0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QixRQUFRLEVBQUUscVVBV1Q7O2lCQUVGOzs7O2dCQW5CUSxPQUFPO2dCQUYwQyxNQUFNO2dCQUR2RCxpQkFBaUI7OztpQ0F3QnZCLEtBQUs7d0JBR0wsS0FBSzs0QkFHTCxLQUFLO3lCQUdMLEtBQUs7O0lBd0VSLHlCQUFDO0NBQUEsQUFsR0QsSUFrR0M7U0FsRlksa0JBQWtCOzs7SUFDN0IsNENBQzBDOztJQUUxQyxtQ0FDMEI7O0lBRTFCLHVDQUMyQjs7SUFFM0Isb0NBQ3NHOztJQUV0RywyQ0FBMEI7O0lBRTFCLHNDQUF1Qjs7SUFFdkIsbUNBQW9COzs7OztJQU1SLHFDQUF3Qjs7Ozs7SUFBRSxvQ0FBc0I7Ozs7O0lBQUUsbUNBQWdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhcnRMb2FkZXIsIFN0b3BMb2FkZXIgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgQ2hhbmdlRGV0ZWN0b3JSZWYsIENvbXBvbmVudCwgSW5wdXQsIE9uRGVzdHJveSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmF2aWdhdGlvbkVuZCwgTmF2aWdhdGlvbkVycm9yLCBOYXZpZ2F0aW9uU3RhcnQsIFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyB0YWtlVW50aWxEZXN0cm95IH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcbmltcG9ydCB7IEFjdGlvbnMsIG9mQWN0aW9uU3VjY2Vzc2Z1bCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IGludGVydmFsLCBTdWJzY3JpcHRpb24sIHRpbWVyIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBmaWx0ZXIgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1sb2FkZXItYmFyJyxcbiAgdGVtcGxhdGU6IGBcbiAgICA8ZGl2IGlkPVwiYWJwLWxvYWRlci1iYXJcIiBbbmdDbGFzc109XCJjb250YWluZXJDbGFzc1wiIFtjbGFzcy5pcy1sb2FkaW5nXT1cImlzTG9hZGluZ1wiPlxuICAgICAgPGRpdlxuICAgICAgICBjbGFzcz1cImFicC1wcm9ncmVzc1wiXG4gICAgICAgIFtzdHlsZS53aWR0aC52d109XCJwcm9ncmVzc0xldmVsXCJcbiAgICAgICAgW25nU3R5bGVdPVwie1xuICAgICAgICAgICdiYWNrZ3JvdW5kLWNvbG9yJzogY29sb3IsXG4gICAgICAgICAgJ2JveC1zaGFkb3cnOiBib3hTaGFkb3dcbiAgICAgICAgfVwiXG4gICAgICA+PC9kaXY+XG4gICAgPC9kaXY+XG4gIGAsXG4gIHN0eWxlVXJsczogWycuL2xvYWRlci1iYXIuY29tcG9uZW50LnNjc3MnXSxcbn0pXG5leHBvcnQgY2xhc3MgTG9hZGVyQmFyQ29tcG9uZW50IGltcGxlbWVudHMgT25EZXN0cm95IHtcbiAgQElucHV0KClcbiAgY29udGFpbmVyQ2xhc3M6IHN0cmluZyA9ICdhYnAtbG9hZGVyLWJhcic7XG5cbiAgQElucHV0KClcbiAgY29sb3I6IHN0cmluZyA9ICcjNzdiNmZmJztcblxuICBASW5wdXQoKVxuICBpc0xvYWRpbmc6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBASW5wdXQoKVxuICBmaWx0ZXIgPSAoYWN0aW9uOiBTdGFydExvYWRlciB8IFN0b3BMb2FkZXIpID0+IGFjdGlvbi5wYXlsb2FkLnVybC5pbmRleE9mKCdvcGVuaWQtY29uZmlndXJhdGlvbicpIDwgMDtcblxuICBwcm9ncmVzc0xldmVsOiBudW1iZXIgPSAwO1xuXG4gIGludGVydmFsOiBTdWJzY3JpcHRpb247XG5cbiAgdGltZXI6IFN1YnNjcmlwdGlvbjtcblxuICBnZXQgYm94U2hhZG93KCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIGAwIDAgMTBweCByZ2JhKCR7dGhpcy5jb2xvcn0sIDAuNSlgO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBhY3Rpb25zOiBBY3Rpb25zLCBwcml2YXRlIHJvdXRlcjogUm91dGVyLCBwcml2YXRlIGNkUmVmOiBDaGFuZ2VEZXRlY3RvclJlZikge1xuICAgIGFjdGlvbnNcbiAgICAgIC5waXBlKFxuICAgICAgICBvZkFjdGlvblN1Y2Nlc3NmdWwoU3RhcnRMb2FkZXIsIFN0b3BMb2FkZXIpLFxuICAgICAgICBmaWx0ZXIodGhpcy5maWx0ZXIpLFxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZShhY3Rpb24gPT4ge1xuICAgICAgICBpZiAoYWN0aW9uIGluc3RhbmNlb2YgU3RhcnRMb2FkZXIpIHRoaXMuc3RhcnRMb2FkaW5nKCk7XG4gICAgICAgIGVsc2UgdGhpcy5zdG9wTG9hZGluZygpO1xuICAgICAgfSk7XG5cbiAgICByb3V0ZXIuZXZlbnRzXG4gICAgICAucGlwZShcbiAgICAgICAgZmlsdGVyKFxuICAgICAgICAgIGV2ZW50ID0+XG4gICAgICAgICAgICBldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25TdGFydCB8fCBldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25FbmQgfHwgZXZlbnQgaW5zdGFuY2VvZiBOYXZpZ2F0aW9uRXJyb3IsXG4gICAgICAgICksXG4gICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcyksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKGV2ZW50ID0+IHtcbiAgICAgICAgaWYgKGV2ZW50IGluc3RhbmNlb2YgTmF2aWdhdGlvblN0YXJ0KSB0aGlzLnN0YXJ0TG9hZGluZygpO1xuICAgICAgICBlbHNlIHRoaXMuc3RvcExvYWRpbmcoKTtcbiAgICAgIH0pO1xuICB9XG5cbiAgbmdPbkRlc3Ryb3koKSB7XG4gICAgdGhpcy5pbnRlcnZhbC51bnN1YnNjcmliZSgpO1xuICB9XG5cbiAgc3RhcnRMb2FkaW5nKCkge1xuICAgIGlmICh0aGlzLmlzTG9hZGluZyB8fCB0aGlzLnByb2dyZXNzTGV2ZWwgIT09IDApIHJldHVybjtcblxuICAgIHRoaXMuaXNMb2FkaW5nID0gdHJ1ZTtcbiAgICB0aGlzLmludGVydmFsID0gaW50ZXJ2YWwoMzUwKS5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgaWYgKHRoaXMucHJvZ3Jlc3NMZXZlbCA8IDc1KSB7XG4gICAgICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCArPSBNYXRoLnJhbmRvbSgpICogMTA7XG4gICAgICB9IGVsc2UgaWYgKHRoaXMucHJvZ3Jlc3NMZXZlbCA8IDkwKSB7XG4gICAgICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCArPSAwLjQ7XG4gICAgICB9IGVsc2UgaWYgKHRoaXMucHJvZ3Jlc3NMZXZlbCA8IDEwMCkge1xuICAgICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgKz0gMC4xO1xuICAgICAgfSBlbHNlIHtcbiAgICAgICAgdGhpcy5pbnRlcnZhbC51bnN1YnNjcmliZSgpO1xuICAgICAgfVxuICAgICAgdGhpcy5jZFJlZi5kZXRlY3RDaGFuZ2VzKCk7XG4gICAgfSk7XG4gIH1cblxuICBzdG9wTG9hZGluZygpIHtcbiAgICB0aGlzLmludGVydmFsLnVuc3Vic2NyaWJlKCk7XG4gICAgdGhpcy5wcm9ncmVzc0xldmVsID0gMTAwO1xuICAgIHRoaXMuaXNMb2FkaW5nID0gZmFsc2U7XG4gICAgaWYgKHRoaXMudGltZXIgJiYgIXRoaXMudGltZXIuY2xvc2VkKSByZXR1cm47XG5cbiAgICB0aGlzLnRpbWVyID0gdGltZXIoODIwKS5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgdGhpcy5wcm9ncmVzc0xldmVsID0gMDtcbiAgICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xuICAgIH0pO1xuICB9XG59XG4iXX0=