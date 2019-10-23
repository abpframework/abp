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
var LoaderBarComponent = /** @class */ (function () {
    function LoaderBarComponent(actions, router, cdRef) {
        var _this = this;
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
        function (action) { return action.payload.url.indexOf('openid-configuration') < 0; });
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9hZGVyLWJhci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2xvYWRlci1iYXIvbG9hZGVyLWJhci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxXQUFXLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3ZELE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFhLE1BQU0sZUFBZSxDQUFDO0FBQy9FLE9BQU8sRUFBRSxhQUFhLEVBQUUsZUFBZSxFQUFFLGVBQWUsRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUMxRixPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzFELE9BQU8sRUFBRSxRQUFRLEVBQWdCLEtBQUssRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNyRCxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFFeEM7SUFxQkUsNEJBQW9CLE9BQWdCLEVBQVUsTUFBYyxFQUFVLEtBQXdCO1FBQTlGLGlCQXdCQztRQXhCbUIsWUFBTyxHQUFQLE9BQU8sQ0FBUztRQUFVLFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFtQjtRQTBCOUYsbUJBQWMsR0FBRyxnQkFBZ0IsQ0FBQztRQUdsQyxVQUFLLEdBQUcsU0FBUyxDQUFDO1FBR2xCLGNBQVMsR0FBRyxLQUFLLENBQUM7UUFFbEIsa0JBQWEsR0FBRyxDQUFDLENBQUM7UUFPbEIsV0FBTTs7OztRQUFHLFVBQUMsTUFBZ0MsSUFBSyxPQUFBLE1BQU0sQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLE9BQU8sQ0FBQyxzQkFBc0IsQ0FBQyxHQUFH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MUIsUUFBUSxFQUFFLHFVQVdUOztpQkFFRjs7OztnQkFuQlEsT0FBTztnQkFGMEMsTUFBTTtnQkFEdkQsaUJBQWlCOzs7aUNBcUR2QixLQUFLO3dCQUdMLEtBQUs7NEJBR0wsS0FBSzt5QkFTTCxLQUFLOztJQW9DUix5QkFBQztDQUFBLEFBakdELElBaUdDO1NBakZZLGtCQUFrQjs7O0lBOEI3Qiw0Q0FDa0M7O0lBRWxDLG1DQUNrQjs7SUFFbEIsdUNBQ2tCOztJQUVsQiwyQ0FBa0I7O0lBRWxCLHNDQUF1Qjs7SUFFdkIsbUNBQW9COztJQUVwQixvQ0FDc0c7Ozs7O0lBekMxRixxQ0FBd0I7Ozs7O0lBQUUsb0NBQXNCOzs7OztJQUFFLG1DQUFnQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFN0YXJ0TG9hZGVyLCBTdG9wTG9hZGVyIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgQ2hhbmdlRGV0ZWN0b3JSZWYsIENvbXBvbmVudCwgSW5wdXQsIE9uRGVzdHJveSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBOYXZpZ2F0aW9uRW5kLCBOYXZpZ2F0aW9uRXJyb3IsIE5hdmlnYXRpb25TdGFydCwgUm91dGVyIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcclxuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XHJcbmltcG9ydCB7IEFjdGlvbnMsIG9mQWN0aW9uU3VjY2Vzc2Z1bCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgaW50ZXJ2YWwsIFN1YnNjcmlwdGlvbiwgdGltZXIgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgZmlsdGVyIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdhYnAtbG9hZGVyLWJhcicsXHJcbiAgdGVtcGxhdGU6IGBcclxuICAgIDxkaXYgaWQ9XCJhYnAtbG9hZGVyLWJhclwiIFtuZ0NsYXNzXT1cImNvbnRhaW5lckNsYXNzXCIgW2NsYXNzLmlzLWxvYWRpbmddPVwiaXNMb2FkaW5nXCI+XHJcbiAgICAgIDxkaXZcclxuICAgICAgICBjbGFzcz1cImFicC1wcm9ncmVzc1wiXHJcbiAgICAgICAgW3N0eWxlLndpZHRoLnZ3XT1cInByb2dyZXNzTGV2ZWxcIlxyXG4gICAgICAgIFtuZ1N0eWxlXT1cIntcclxuICAgICAgICAgICdiYWNrZ3JvdW5kLWNvbG9yJzogY29sb3IsXHJcbiAgICAgICAgICAnYm94LXNoYWRvdyc6IGJveFNoYWRvd1xyXG4gICAgICAgIH1cIlxyXG4gICAgICA+PC9kaXY+XHJcbiAgICA8L2Rpdj5cclxuICBgLFxyXG4gIHN0eWxlVXJsczogWycuL2xvYWRlci1iYXIuY29tcG9uZW50LnNjc3MnXVxyXG59KVxyXG5leHBvcnQgY2xhc3MgTG9hZGVyQmFyQ29tcG9uZW50IGltcGxlbWVudHMgT25EZXN0cm95IHtcclxuICBnZXQgYm94U2hhZG93KCk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gYDAgMCAxMHB4IHJnYmEoJHt0aGlzLmNvbG9yfSwgMC41KWA7XHJcbiAgfVxyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGFjdGlvbnM6IEFjdGlvbnMsIHByaXZhdGUgcm91dGVyOiBSb3V0ZXIsIHByaXZhdGUgY2RSZWY6IENoYW5nZURldGVjdG9yUmVmKSB7XHJcbiAgICBhY3Rpb25zXHJcbiAgICAgIC5waXBlKFxyXG4gICAgICAgIG9mQWN0aW9uU3VjY2Vzc2Z1bChTdGFydExvYWRlciwgU3RvcExvYWRlciksXHJcbiAgICAgICAgZmlsdGVyKHRoaXMuZmlsdGVyKSxcclxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpXHJcbiAgICAgIClcclxuICAgICAgLnN1YnNjcmliZShhY3Rpb24gPT4ge1xyXG4gICAgICAgIGlmIChhY3Rpb24gaW5zdGFuY2VvZiBTdGFydExvYWRlcikgdGhpcy5zdGFydExvYWRpbmcoKTtcclxuICAgICAgICBlbHNlIHRoaXMuc3RvcExvYWRpbmcoKTtcclxuICAgICAgfSk7XHJcblxyXG4gICAgcm91dGVyLmV2ZW50c1xyXG4gICAgICAucGlwZShcclxuICAgICAgICBmaWx0ZXIoXHJcbiAgICAgICAgICBldmVudCA9PlxyXG4gICAgICAgICAgICBldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25TdGFydCB8fCBldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25FbmQgfHwgZXZlbnQgaW5zdGFuY2VvZiBOYXZpZ2F0aW9uRXJyb3JcclxuICAgICAgICApLFxyXG4gICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcylcclxuICAgICAgKVxyXG4gICAgICAuc3Vic2NyaWJlKGV2ZW50ID0+IHtcclxuICAgICAgICBpZiAoZXZlbnQgaW5zdGFuY2VvZiBOYXZpZ2F0aW9uU3RhcnQpIHRoaXMuc3RhcnRMb2FkaW5nKCk7XHJcbiAgICAgICAgZWxzZSB0aGlzLnN0b3BMb2FkaW5nKCk7XHJcbiAgICAgIH0pO1xyXG4gIH1cclxuICBASW5wdXQoKVxyXG4gIGNvbnRhaW5lckNsYXNzID0gJ2FicC1sb2FkZXItYmFyJztcclxuXHJcbiAgQElucHV0KClcclxuICBjb2xvciA9ICcjNzdiNmZmJztcclxuXHJcbiAgQElucHV0KClcclxuICBpc0xvYWRpbmcgPSBmYWxzZTtcclxuXHJcbiAgcHJvZ3Jlc3NMZXZlbCA9IDA7XHJcblxyXG4gIGludGVydmFsOiBTdWJzY3JpcHRpb247XHJcblxyXG4gIHRpbWVyOiBTdWJzY3JpcHRpb247XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgZmlsdGVyID0gKGFjdGlvbjogU3RhcnRMb2FkZXIgfCBTdG9wTG9hZGVyKSA9PiBhY3Rpb24ucGF5bG9hZC51cmwuaW5kZXhPZignb3BlbmlkLWNvbmZpZ3VyYXRpb24nKSA8IDA7XHJcblxyXG4gIG5nT25EZXN0cm95KCkge1xyXG4gICAgdGhpcy5pbnRlcnZhbC51bnN1YnNjcmliZSgpO1xyXG4gIH1cclxuXHJcbiAgc3RhcnRMb2FkaW5nKCkge1xyXG4gICAgaWYgKHRoaXMuaXNMb2FkaW5nIHx8IHRoaXMucHJvZ3Jlc3NMZXZlbCAhPT0gMCkgcmV0dXJuO1xyXG5cclxuICAgIHRoaXMuaXNMb2FkaW5nID0gdHJ1ZTtcclxuICAgIHRoaXMuaW50ZXJ2YWwgPSBpbnRlcnZhbCgzNTApLnN1YnNjcmliZSgoKSA9PiB7XHJcbiAgICAgIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCA3NSkge1xyXG4gICAgICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCArPSBNYXRoLnJhbmRvbSgpICogMTA7XHJcbiAgICAgIH0gZWxzZSBpZiAodGhpcy5wcm9ncmVzc0xldmVsIDwgOTApIHtcclxuICAgICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgKz0gMC40O1xyXG4gICAgICB9IGVsc2UgaWYgKHRoaXMucHJvZ3Jlc3NMZXZlbCA8IDEwMCkge1xyXG4gICAgICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCArPSAwLjE7XHJcbiAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgdGhpcy5pbnRlcnZhbC51bnN1YnNjcmliZSgpO1xyXG4gICAgICB9XHJcbiAgICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBzdG9wTG9hZGluZygpIHtcclxuICAgIHRoaXMuaW50ZXJ2YWwudW5zdWJzY3JpYmUoKTtcclxuICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCA9IDEwMDtcclxuICAgIHRoaXMuaXNMb2FkaW5nID0gZmFsc2U7XHJcbiAgICBpZiAodGhpcy50aW1lciAmJiAhdGhpcy50aW1lci5jbG9zZWQpIHJldHVybjtcclxuXHJcbiAgICB0aGlzLnRpbWVyID0gdGltZXIoODIwKS5zdWJzY3JpYmUoKCkgPT4ge1xyXG4gICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgPSAwO1xyXG4gICAgICB0aGlzLmNkUmVmLmRldGVjdENoYW5nZXMoKTtcclxuICAgIH0pO1xyXG4gIH1cclxufVxyXG4iXX0=