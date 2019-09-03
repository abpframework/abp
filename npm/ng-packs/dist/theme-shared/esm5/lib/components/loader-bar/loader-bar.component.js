/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { StartLoader, StopLoader } from '@abp/ng.core';
import { Component, Input } from '@angular/core';
import { NavigationEnd, NavigationStart, Router, NavigationError } from '@angular/router';
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9hZGVyLWJhci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2xvYWRlci1iYXIvbG9hZGVyLWJhci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxXQUFXLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3ZELE9BQU8sRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFhLE1BQU0sZUFBZSxDQUFDO0FBQzVELE9BQU8sRUFBRSxhQUFhLEVBQUUsZUFBZSxFQUFFLE1BQU0sRUFBRSxlQUFlLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUMxRixPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzFELE9BQU8sRUFBRSxNQUFNLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUV4QztJQTBCRSw0QkFBb0IsT0FBZ0IsRUFBVSxNQUFjO1FBQTVELGlCQXdCQztRQXhCbUIsWUFBTyxHQUFQLE9BQU8sQ0FBUztRQUFVLFdBQU0sR0FBTixNQUFNLENBQVE7UUFmNUQsbUJBQWMsR0FBVyxnQkFBZ0IsQ0FBQztRQUcxQyxrQkFBYSxHQUFXLGNBQWMsQ0FBQztRQUd2QyxjQUFTLEdBQVksS0FBSyxDQUFDO1FBRzNCLFdBQU07Ozs7UUFBRyxVQUFDLE1BQWdDLElBQUssT0FBQSxNQUFNLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxPQUFPLENBQUMsc0JBQXNCLENBQUMsR0FBRyxDQUFDLEVBQXRELENBQXNELEVBQUM7UUFFdEcsa0JBQWEsR0FBVyxDQUFDLENBQUM7UUFLeEIsT0FBTzthQUNKLElBQUksQ0FDSCxrQkFBa0IsQ0FBQyxXQUFXLEVBQUUsVUFBVSxDQUFDLEVBQzNDLE1BQU0sQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLEVBQ25CLGdCQUFnQixDQUFDLElBQUksQ0FBQyxDQUN2QjthQUNBLFNBQVM7Ozs7UUFBQyxVQUFBLE1BQU07WUFDZixJQUFJLE1BQU0sWUFBWSxXQUFXO2dCQUFFLEtBQUksQ0FBQyxZQUFZLEVBQUUsQ0FBQzs7Z0JBQ2xELEtBQUksQ0FBQyxXQUFXLEVBQUUsQ0FBQztRQUMxQixDQUFDLEVBQUMsQ0FBQztRQUVMLE1BQU0sQ0FBQyxNQUFNO2FBQ1YsSUFBSSxDQUNILE1BQU07Ozs7UUFDSixVQUFBLEtBQUs7WUFDSCxPQUFBLEtBQUssWUFBWSxlQUFlLElBQUksS0FBSyxZQUFZLGFBQWEsSUFBSSxLQUFLLFlBQVksZUFBZTtRQUF0RyxDQUFzRyxFQUN6RyxFQUNELGdCQUFnQixDQUFDLElBQUksQ0FBQyxDQUN2QjthQUNBLFNBQVM7Ozs7UUFBQyxVQUFBLEtBQUs7WUFDZCxJQUFJLEtBQUssWUFBWSxlQUFlO2dCQUFFLEtBQUksQ0FBQyxZQUFZLEVBQUUsQ0FBQzs7Z0JBQ3JELEtBQUksQ0FBQyxXQUFXLEVBQUUsQ0FBQztRQUMxQixDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7Ozs7SUFFRCx3Q0FBVzs7O0lBQVgsY0FBZSxDQUFDOzs7O0lBRWhCLHlDQUFZOzs7SUFBWjtRQUFBLGlCQWVDO1FBZEMsSUFBSSxDQUFDLFNBQVMsR0FBRyxJQUFJLENBQUM7O1lBQ2hCLFFBQVEsR0FBRyxXQUFXOzs7UUFBQztZQUMzQixJQUFJLEtBQUksQ0FBQyxhQUFhLEdBQUcsRUFBRSxFQUFFO2dCQUMzQixLQUFJLENBQUMsYUFBYSxJQUFJLElBQUksQ0FBQyxNQUFNLEVBQUUsR0FBRyxFQUFFLENBQUM7YUFDMUM7aUJBQU0sSUFBSSxLQUFJLENBQUMsYUFBYSxHQUFHLEVBQUUsRUFBRTtnQkFDbEMsS0FBSSxDQUFDLGFBQWEsSUFBSSxHQUFHLENBQUM7YUFDM0I7aUJBQU0sSUFBSSxLQUFJLENBQUMsYUFBYSxHQUFHLEdBQUcsRUFBRTtnQkFDbkMsS0FBSSxDQUFDLGFBQWEsSUFBSSxHQUFHLENBQUM7YUFDM0I7aUJBQU07Z0JBQ0wsYUFBYSxDQUFDLFFBQVEsQ0FBQyxDQUFDO2FBQ3pCO1FBQ0gsQ0FBQyxHQUFFLEdBQUcsQ0FBQztRQUVQLElBQUksQ0FBQyxRQUFRLEdBQUcsUUFBUSxDQUFDO0lBQzNCLENBQUM7Ozs7SUFFRCx3Q0FBVzs7O0lBQVg7UUFBQSxpQkFRQztRQVBDLGFBQWEsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUM7UUFDN0IsSUFBSSxDQUFDLGFBQWEsR0FBRyxHQUFHLENBQUM7UUFDekIsSUFBSSxDQUFDLFNBQVMsR0FBRyxLQUFLLENBQUM7UUFFdkIsVUFBVTs7O1FBQUM7WUFDVCxLQUFJLENBQUMsYUFBYSxHQUFHLENBQUMsQ0FBQztRQUN6QixDQUFDLEdBQUUsR0FBRyxDQUFDLENBQUM7SUFDVixDQUFDOztnQkEvRUYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxnQkFBZ0I7b0JBQzFCLFFBQVEsRUFBRSxtTUFJVDs7aUJBRUY7Ozs7Z0JBWFEsT0FBTztnQkFGeUIsTUFBTTs7O2lDQWU1QyxLQUFLO2dDQUdMLEtBQUs7NEJBR0wsS0FBSzt5QkFHTCxLQUFLOztJQTZEUix5QkFBQztDQUFBLEFBaEZELElBZ0ZDO1NBdkVZLGtCQUFrQjs7O0lBQzdCLDRDQUMwQzs7SUFFMUMsMkNBQ3VDOztJQUV2Qyx1Q0FDMkI7O0lBRTNCLG9DQUNzRzs7SUFFdEcsMkNBQTBCOztJQUUxQixzQ0FBYzs7Ozs7SUFFRixxQ0FBd0I7Ozs7O0lBQUUsb0NBQXNCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhcnRMb2FkZXIsIFN0b3BMb2FkZXIgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgQ29tcG9uZW50LCBJbnB1dCwgT25EZXN0cm95IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBOYXZpZ2F0aW9uRW5kLCBOYXZpZ2F0aW9uU3RhcnQsIFJvdXRlciwgTmF2aWdhdGlvbkVycm9yIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IHRha2VVbnRpbERlc3Ryb3kgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xuaW1wb3J0IHsgQWN0aW9ucywgb2ZBY3Rpb25TdWNjZXNzZnVsIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgZmlsdGVyIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtbG9hZGVyLWJhcicsXG4gIHRlbXBsYXRlOiBgXG4gICAgPGRpdiBpZD1cImFicC1sb2FkZXItYmFyXCIgW25nQ2xhc3NdPVwiY29udGFpbmVyQ2xhc3NcIiBbY2xhc3MuaXMtbG9hZGluZ109XCJpc0xvYWRpbmdcIj5cbiAgICAgIDxkaXYgW25nQ2xhc3NdPVwicHJvZ3Jlc3NDbGFzc1wiIFtzdHlsZS53aWR0aC52d109XCJwcm9ncmVzc0xldmVsXCI+PC9kaXY+XG4gICAgPC9kaXY+XG4gIGAsXG4gIHN0eWxlVXJsczogWycuL2xvYWRlci1iYXIuY29tcG9uZW50LnNjc3MnXSxcbn0pXG5leHBvcnQgY2xhc3MgTG9hZGVyQmFyQ29tcG9uZW50IGltcGxlbWVudHMgT25EZXN0cm95IHtcbiAgQElucHV0KClcbiAgY29udGFpbmVyQ2xhc3M6IHN0cmluZyA9ICdhYnAtbG9hZGVyLWJhcic7XG5cbiAgQElucHV0KClcbiAgcHJvZ3Jlc3NDbGFzczogc3RyaW5nID0gJ2FicC1wcm9ncmVzcyc7XG5cbiAgQElucHV0KClcbiAgaXNMb2FkaW5nOiBib29sZWFuID0gZmFsc2U7XG5cbiAgQElucHV0KClcbiAgZmlsdGVyID0gKGFjdGlvbjogU3RhcnRMb2FkZXIgfCBTdG9wTG9hZGVyKSA9PiBhY3Rpb24ucGF5bG9hZC51cmwuaW5kZXhPZignb3BlbmlkLWNvbmZpZ3VyYXRpb24nKSA8IDA7XG5cbiAgcHJvZ3Jlc3NMZXZlbDogbnVtYmVyID0gMDtcblxuICBpbnRlcnZhbDogYW55O1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgYWN0aW9uczogQWN0aW9ucywgcHJpdmF0ZSByb3V0ZXI6IFJvdXRlcikge1xuICAgIGFjdGlvbnNcbiAgICAgIC5waXBlKFxuICAgICAgICBvZkFjdGlvblN1Y2Nlc3NmdWwoU3RhcnRMb2FkZXIsIFN0b3BMb2FkZXIpLFxuICAgICAgICBmaWx0ZXIodGhpcy5maWx0ZXIpLFxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZShhY3Rpb24gPT4ge1xuICAgICAgICBpZiAoYWN0aW9uIGluc3RhbmNlb2YgU3RhcnRMb2FkZXIpIHRoaXMuc3RhcnRMb2FkaW5nKCk7XG4gICAgICAgIGVsc2UgdGhpcy5zdG9wTG9hZGluZygpO1xuICAgICAgfSk7XG5cbiAgICByb3V0ZXIuZXZlbnRzXG4gICAgICAucGlwZShcbiAgICAgICAgZmlsdGVyKFxuICAgICAgICAgIGV2ZW50ID0+XG4gICAgICAgICAgICBldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25TdGFydCB8fCBldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25FbmQgfHwgZXZlbnQgaW5zdGFuY2VvZiBOYXZpZ2F0aW9uRXJyb3IsXG4gICAgICAgICksXG4gICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcyksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKGV2ZW50ID0+IHtcbiAgICAgICAgaWYgKGV2ZW50IGluc3RhbmNlb2YgTmF2aWdhdGlvblN0YXJ0KSB0aGlzLnN0YXJ0TG9hZGluZygpO1xuICAgICAgICBlbHNlIHRoaXMuc3RvcExvYWRpbmcoKTtcbiAgICAgIH0pO1xuICB9XG5cbiAgbmdPbkRlc3Ryb3koKSB7fVxuXG4gIHN0YXJ0TG9hZGluZygpIHtcbiAgICB0aGlzLmlzTG9hZGluZyA9IHRydWU7XG4gICAgY29uc3QgaW50ZXJ2YWwgPSBzZXRJbnRlcnZhbCgoKSA9PiB7XG4gICAgICBpZiAodGhpcy5wcm9ncmVzc0xldmVsIDwgNzUpIHtcbiAgICAgICAgdGhpcy5wcm9ncmVzc0xldmVsICs9IE1hdGgucmFuZG9tKCkgKiAxMDtcbiAgICAgIH0gZWxzZSBpZiAodGhpcy5wcm9ncmVzc0xldmVsIDwgOTApIHtcbiAgICAgICAgdGhpcy5wcm9ncmVzc0xldmVsICs9IDAuNDtcbiAgICAgIH0gZWxzZSBpZiAodGhpcy5wcm9ncmVzc0xldmVsIDwgMTAwKSB7XG4gICAgICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCArPSAwLjE7XG4gICAgICB9IGVsc2Uge1xuICAgICAgICBjbGVhckludGVydmFsKGludGVydmFsKTtcbiAgICAgIH1cbiAgICB9LCAzMDApO1xuXG4gICAgdGhpcy5pbnRlcnZhbCA9IGludGVydmFsO1xuICB9XG5cbiAgc3RvcExvYWRpbmcoKSB7XG4gICAgY2xlYXJJbnRlcnZhbCh0aGlzLmludGVydmFsKTtcbiAgICB0aGlzLnByb2dyZXNzTGV2ZWwgPSAxMDA7XG4gICAgdGhpcy5pc0xvYWRpbmcgPSBmYWxzZTtcblxuICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgdGhpcy5wcm9ncmVzc0xldmVsID0gMDtcbiAgICB9LCA4MDApO1xuICB9XG59XG4iXX0=