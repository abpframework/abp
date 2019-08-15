/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { StartLoader, StopLoader } from '@abp/ng.core';
import { Component, Input } from '@angular/core';
import { NavigationEnd, NavigationStart, Router } from '@angular/router';
import { takeUntilDestroy } from '@ngx-validate/core';
import { Actions, ofActionSuccessful } from '@ngxs/store';
import { filter } from 'rxjs/operators';
var LoaderBarComponent = /** @class */ (function () {
    function LoaderBarComponent(actions, router) {
        var _this = this;
        this.actions = actions;
        this.router = router;
        this.containerClass = 'abp-loader-bar';
        this.progressClass = 'abp-progress';
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
        function (event) { return event instanceof NavigationStart || event instanceof NavigationEnd; })), takeUntilDestroy(this))
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
    /**
     * @return {?}
     */
    LoaderBarComponent.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () { };
    /**
     * @return {?}
     */
    LoaderBarComponent.prototype.startLoading = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.isLoading = true;
        /** @type {?} */
        var interval = setInterval((/**
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
                clearInterval(interval);
            }
        }), 300);
        this.interval = interval;
    };
    /**
     * @return {?}
     */
    LoaderBarComponent.prototype.stopLoading = /**
     * @return {?}
     */
    function () {
        var _this = this;
        clearInterval(this.interval);
        this.progressLevel = 100;
        this.isLoading = false;
        setTimeout((/**
         * @return {?}
         */
        function () {
            _this.progressLevel = 0;
        }), 800);
    };
    LoaderBarComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-loader-bar',
                    template: "\n    <div id=\"abp-loader-bar\" [ngClass]=\"containerClass\" [class.is-loading]=\"isLoading\">\n      <div [ngClass]=\"progressClass\" [style.width.vw]=\"progressLevel\"></div>\n    </div>\n  ",
                    styles: [".abp-loader-bar{left:0;opacity:0;position:fixed;top:0;transition:opacity .4s linear .4s;z-index:99999}.abp-loader-bar.is-loading{opacity:1;transition:none}.abp-loader-bar .abp-progress{background:#77b6ff;box-shadow:0 0 10px rgba(119,182,255,.7);height:2px;left:0;position:fixed;top:0;transition:width .4s}"]
                }] }
    ];
    /** @nocollapse */
    LoaderBarComponent.ctorParameters = function () { return [
        { type: Actions },
        { type: Router }
    ]; };
    LoaderBarComponent.propDecorators = {
        containerClass: [{ type: Input }],
        progressClass: [{ type: Input }],
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
    LoaderBarComponent.prototype.progressClass;
    /** @type {?} */
    LoaderBarComponent.prototype.isLoading;
    /** @type {?} */
    LoaderBarComponent.prototype.filter;
    /** @type {?} */
    LoaderBarComponent.prototype.progressLevel;
    /** @type {?} */
    LoaderBarComponent.prototype.interval;
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
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9hZGVyLWJhci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2xvYWRlci1iYXIvbG9hZGVyLWJhci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxXQUFXLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3ZELE9BQU8sRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFhLE1BQU0sZUFBZSxDQUFDO0FBQzVELE9BQU8sRUFBRSxhQUFhLEVBQUUsZUFBZSxFQUFFLE1BQU0sRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQ3pFLE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBQ3RELE9BQU8sRUFBRSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDMUQsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBRXhDO0lBMEJFLDRCQUFvQixPQUFnQixFQUFVLE1BQWM7UUFBNUQsaUJBcUJDO1FBckJtQixZQUFPLEdBQVAsT0FBTyxDQUFTO1FBQVUsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQWY1RCxtQkFBYyxHQUFXLGdCQUFnQixDQUFDO1FBRzFDLGtCQUFhLEdBQVcsY0FBYyxDQUFDO1FBR3ZDLGNBQVMsR0FBWSxLQUFLLENBQUM7UUFHM0IsV0FBTTs7OztRQUFHLFVBQUMsTUFBZ0MsSUFBSyxPQUFBLE1BQU0sQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLE9BQU8sQ0FBQyxzQkFBc0IsQ0FBQyxHQUFHLENBQUMsRUFBdEQsQ0FBc0QsRUFBQztRQUV0RyxrQkFBYSxHQUFXLENBQUMsQ0FBQztRQUt4QixPQUFPO2FBQ0osSUFBSSxDQUNILGtCQUFrQixDQUFDLFdBQVcsRUFBRSxVQUFVLENBQUMsRUFDM0MsTUFBTSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsRUFDbkIsZ0JBQWdCLENBQUMsSUFBSSxDQUFDLENBQ3ZCO2FBQ0EsU0FBUzs7OztRQUFDLFVBQUEsTUFBTTtZQUNmLElBQUksTUFBTSxZQUFZLFdBQVc7Z0JBQUUsS0FBSSxDQUFDLFlBQVksRUFBRSxDQUFDOztnQkFDbEQsS0FBSSxDQUFDLFdBQVcsRUFBRSxDQUFDO1FBQzFCLENBQUMsRUFBQyxDQUFDO1FBRUwsTUFBTSxDQUFDLE1BQU07YUFDVixJQUFJLENBQ0gsTUFBTTs7OztRQUFDLFVBQUEsS0FBSyxJQUFJLE9BQUEsS0FBSyxZQUFZLGVBQWUsSUFBSSxLQUFLLFlBQVksYUFBYSxFQUFsRSxDQUFrRSxFQUFDLEVBQ25GLGdCQUFnQixDQUFDLElBQUksQ0FBQyxDQUN2QjthQUNBLFNBQVM7Ozs7UUFBQyxVQUFBLEtBQUs7WUFDZCxJQUFJLEtBQUssWUFBWSxlQUFlO2dCQUFFLEtBQUksQ0FBQyxZQUFZLEVBQUUsQ0FBQzs7Z0JBQ3JELEtBQUksQ0FBQyxXQUFXLEVBQUUsQ0FBQztRQUMxQixDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7Ozs7SUFFRCx3Q0FBVzs7O0lBQVgsY0FBZSxDQUFDOzs7O0lBRWhCLHlDQUFZOzs7SUFBWjtRQUFBLGlCQWVDO1FBZEMsSUFBSSxDQUFDLFNBQVMsR0FBRyxJQUFJLENBQUM7O1lBQ2hCLFFBQVEsR0FBRyxXQUFXOzs7UUFBQztZQUMzQixJQUFJLEtBQUksQ0FBQyxhQUFhLEdBQUcsRUFBRSxFQUFFO2dCQUMzQixLQUFJLENBQUMsYUFBYSxJQUFJLElBQUksQ0FBQyxNQUFNLEVBQUUsR0FBRyxFQUFFLENBQUM7YUFDMUM7aUJBQU0sSUFBSSxLQUFJLENBQUMsYUFBYSxHQUFHLEVBQUUsRUFBRTtnQkFDbEMsS0FBSSxDQUFDLGFBQWEsSUFBSSxHQUFHLENBQUM7YUFDM0I7aUJBQU0sSUFBSSxLQUFJLENBQUMsYUFBYSxHQUFHLEdBQUcsRUFBRTtnQkFDbkMsS0FBSSxDQUFDLGFBQWEsSUFBSSxHQUFHLENBQUM7YUFDM0I7aUJBQU07Z0JBQ0wsYUFBYSxDQUFDLFFBQVEsQ0FBQyxDQUFDO2FBQ3pCO1FBQ0gsQ0FBQyxHQUFFLEdBQUcsQ0FBQztRQUVQLElBQUksQ0FBQyxRQUFRLEdBQUcsUUFBUSxDQUFDO0lBQzNCLENBQUM7Ozs7SUFFRCx3Q0FBVzs7O0lBQVg7UUFBQSxpQkFRQztRQVBDLGFBQWEsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUM7UUFDN0IsSUFBSSxDQUFDLGFBQWEsR0FBRyxHQUFHLENBQUM7UUFDekIsSUFBSSxDQUFDLFNBQVMsR0FBRyxLQUFLLENBQUM7UUFFdkIsVUFBVTs7O1FBQUM7WUFDVCxLQUFJLENBQUMsYUFBYSxHQUFHLENBQUMsQ0FBQztRQUN6QixDQUFDLEdBQUUsR0FBRyxDQUFDLENBQUM7SUFDVixDQUFDOztnQkE1RUYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxnQkFBZ0I7b0JBQzFCLFFBQVEsRUFBRSxtTUFJVDs7aUJBRUY7Ozs7Z0JBWFEsT0FBTztnQkFGeUIsTUFBTTs7O2lDQWU1QyxLQUFLO2dDQUdMLEtBQUs7NEJBR0wsS0FBSzt5QkFHTCxLQUFLOztJQTBEUix5QkFBQztDQUFBLEFBN0VELElBNkVDO1NBcEVZLGtCQUFrQjs7O0lBQzdCLDRDQUMwQzs7SUFFMUMsMkNBQ3VDOztJQUV2Qyx1Q0FDMkI7O0lBRTNCLG9DQUNzRzs7SUFFdEcsMkNBQTBCOztJQUUxQixzQ0FBYzs7Ozs7SUFFRixxQ0FBd0I7Ozs7O0lBQUUsb0NBQXNCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhcnRMb2FkZXIsIFN0b3BMb2FkZXIgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgQ29tcG9uZW50LCBJbnB1dCwgT25EZXN0cm95IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBOYXZpZ2F0aW9uRW5kLCBOYXZpZ2F0aW9uU3RhcnQsIFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyB0YWtlVW50aWxEZXN0cm95IH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcbmltcG9ydCB7IEFjdGlvbnMsIG9mQWN0aW9uU3VjY2Vzc2Z1bCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IGZpbHRlciB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWxvYWRlci1iYXInLFxuICB0ZW1wbGF0ZTogYFxuICAgIDxkaXYgaWQ9XCJhYnAtbG9hZGVyLWJhclwiIFtuZ0NsYXNzXT1cImNvbnRhaW5lckNsYXNzXCIgW2NsYXNzLmlzLWxvYWRpbmddPVwiaXNMb2FkaW5nXCI+XG4gICAgICA8ZGl2IFtuZ0NsYXNzXT1cInByb2dyZXNzQ2xhc3NcIiBbc3R5bGUud2lkdGgudnddPVwicHJvZ3Jlc3NMZXZlbFwiPjwvZGl2PlxuICAgIDwvZGl2PlxuICBgLFxuICBzdHlsZVVybHM6IFsnLi9sb2FkZXItYmFyLmNvbXBvbmVudC5zY3NzJ10sXG59KVxuZXhwb3J0IGNsYXNzIExvYWRlckJhckNvbXBvbmVudCBpbXBsZW1lbnRzIE9uRGVzdHJveSB7XG4gIEBJbnB1dCgpXG4gIGNvbnRhaW5lckNsYXNzOiBzdHJpbmcgPSAnYWJwLWxvYWRlci1iYXInO1xuXG4gIEBJbnB1dCgpXG4gIHByb2dyZXNzQ2xhc3M6IHN0cmluZyA9ICdhYnAtcHJvZ3Jlc3MnO1xuXG4gIEBJbnB1dCgpXG4gIGlzTG9hZGluZzogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIEBJbnB1dCgpXG4gIGZpbHRlciA9IChhY3Rpb246IFN0YXJ0TG9hZGVyIHwgU3RvcExvYWRlcikgPT4gYWN0aW9uLnBheWxvYWQudXJsLmluZGV4T2YoJ29wZW5pZC1jb25maWd1cmF0aW9uJykgPCAwO1xuXG4gIHByb2dyZXNzTGV2ZWw6IG51bWJlciA9IDA7XG5cbiAgaW50ZXJ2YWw6IGFueTtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGFjdGlvbnM6IEFjdGlvbnMsIHByaXZhdGUgcm91dGVyOiBSb3V0ZXIpIHtcbiAgICBhY3Rpb25zXG4gICAgICAucGlwZShcbiAgICAgICAgb2ZBY3Rpb25TdWNjZXNzZnVsKFN0YXJ0TG9hZGVyLCBTdG9wTG9hZGVyKSxcbiAgICAgICAgZmlsdGVyKHRoaXMuZmlsdGVyKSxcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoYWN0aW9uID0+IHtcbiAgICAgICAgaWYgKGFjdGlvbiBpbnN0YW5jZW9mIFN0YXJ0TG9hZGVyKSB0aGlzLnN0YXJ0TG9hZGluZygpO1xuICAgICAgICBlbHNlIHRoaXMuc3RvcExvYWRpbmcoKTtcbiAgICAgIH0pO1xuXG4gICAgcm91dGVyLmV2ZW50c1xuICAgICAgLnBpcGUoXG4gICAgICAgIGZpbHRlcihldmVudCA9PiBldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25TdGFydCB8fCBldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25FbmQpLFxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZShldmVudCA9PiB7XG4gICAgICAgIGlmIChldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25TdGFydCkgdGhpcy5zdGFydExvYWRpbmcoKTtcbiAgICAgICAgZWxzZSB0aGlzLnN0b3BMb2FkaW5nKCk7XG4gICAgICB9KTtcbiAgfVxuXG4gIG5nT25EZXN0cm95KCkge31cblxuICBzdGFydExvYWRpbmcoKSB7XG4gICAgdGhpcy5pc0xvYWRpbmcgPSB0cnVlO1xuICAgIGNvbnN0IGludGVydmFsID0gc2V0SW50ZXJ2YWwoKCkgPT4ge1xuICAgICAgaWYgKHRoaXMucHJvZ3Jlc3NMZXZlbCA8IDc1KSB7XG4gICAgICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCArPSBNYXRoLnJhbmRvbSgpICogMTA7XG4gICAgICB9IGVsc2UgaWYgKHRoaXMucHJvZ3Jlc3NMZXZlbCA8IDkwKSB7XG4gICAgICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCArPSAwLjQ7XG4gICAgICB9IGVsc2UgaWYgKHRoaXMucHJvZ3Jlc3NMZXZlbCA8IDEwMCkge1xuICAgICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgKz0gMC4xO1xuICAgICAgfSBlbHNlIHtcbiAgICAgICAgY2xlYXJJbnRlcnZhbChpbnRlcnZhbCk7XG4gICAgICB9XG4gICAgfSwgMzAwKTtcblxuICAgIHRoaXMuaW50ZXJ2YWwgPSBpbnRlcnZhbDtcbiAgfVxuXG4gIHN0b3BMb2FkaW5nKCkge1xuICAgIGNsZWFySW50ZXJ2YWwodGhpcy5pbnRlcnZhbCk7XG4gICAgdGhpcy5wcm9ncmVzc0xldmVsID0gMTAwO1xuICAgIHRoaXMuaXNMb2FkaW5nID0gZmFsc2U7XG5cbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCA9IDA7XG4gICAgfSwgODAwKTtcbiAgfVxufVxuIl19