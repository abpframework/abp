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
        this.actions = actions;
        this.router = router;
        this.cdRef = cdRef;
        this.containerClass = 'abp-loader-bar';
        this.color = '#77b6ff';
        this.isLoading = false;
        this.progressLevel = 0;
        this.intervalPeriod = 350;
        this.stopDelay = 820;
        this.filter = (/**
         * @param {?} action
         * @return {?}
         */
        function (action) { return action.payload.url.indexOf('openid-configuration') < 0; });
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
    LoaderBarComponent.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.actions
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
        this.router.events
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
    };
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
        this.interval = interval(this.intervalPeriod).subscribe((/**
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
        this.timer = timer(this.stopDelay).subscribe((/**
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
                    styles: [".abp-loader-bar{left:0;opacity:0;position:fixed;top:0;-webkit-transition:opacity .4s linear .4s;transition:opacity .4s linear .4s;z-index:99999}.abp-loader-bar.is-loading{opacity:1;-webkit-transition:none;transition:none}.abp-loader-bar .abp-progress{height:3px;left:0;position:fixed;top:0;-webkit-transition:width .4s;transition:width .4s}"]
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
    LoaderBarComponent.prototype.intervalPeriod;
    /** @type {?} */
    LoaderBarComponent.prototype.stopDelay;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9hZGVyLWJhci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2xvYWRlci1iYXIvbG9hZGVyLWJhci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxXQUFXLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3ZELE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFxQixNQUFNLGVBQWUsQ0FBQztBQUN2RixPQUFPLEVBQUUsYUFBYSxFQUFFLGVBQWUsRUFBRSxlQUFlLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDMUYsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFDdEQsT0FBTyxFQUFFLE9BQU8sRUFBRSxrQkFBa0IsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUMxRCxPQUFPLEVBQUUsUUFBUSxFQUFnQixLQUFLLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDckQsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBRXhDO0lBMkNFLDRCQUFvQixPQUFnQixFQUFVLE1BQWMsRUFBVSxLQUF3QjtRQUExRSxZQUFPLEdBQVAsT0FBTyxDQUFTO1FBQVUsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQW1CO1FBekI5RixtQkFBYyxHQUFHLGdCQUFnQixDQUFDO1FBR2xDLFVBQUssR0FBRyxTQUFTLENBQUM7UUFHbEIsY0FBUyxHQUFHLEtBQUssQ0FBQztRQUVsQixrQkFBYSxHQUFHLENBQUMsQ0FBQztRQU1sQixtQkFBYyxHQUFHLEdBQUcsQ0FBQztRQUVyQixjQUFTLEdBQUcsR0FBRyxDQUFDO1FBR2hCLFdBQU07Ozs7UUFBRyxVQUFDLE1BQWdDLElBQUssT0FBQSxNQUFNLENBQUMsT0FBTyxD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QUFDO1lBQ3ZCLEtBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxFQUFFLENBQUM7UUFDN0IsQ0FBQyxFQUFDLENBQUM7SUFDTCxDQUFDOztnQkF2R0YsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxnQkFBZ0I7b0JBQzFCLFFBQVEsRUFBRSxxVUFXVDs7aUJBRUY7Ozs7Z0JBbkJRLE9BQU87Z0JBRjBDLE1BQU07Z0JBRHZELGlCQUFpQjs7O2lDQXdCdkIsS0FBSzt3QkFHTCxLQUFLOzRCQUdMLEtBQUs7eUJBYUwsS0FBSzs7SUFvRVIseUJBQUM7Q0FBQSxBQXhHRCxJQXdHQztTQXhGWSxrQkFBa0I7OztJQUM3Qiw0Q0FDa0M7O0lBRWxDLG1DQUNrQjs7SUFFbEIsdUNBQ2tCOztJQUVsQiwyQ0FBa0I7O0lBRWxCLHNDQUF1Qjs7SUFFdkIsbUNBQW9COztJQUVwQiw0Q0FBcUI7O0lBRXJCLHVDQUFnQjs7SUFFaEIsb0NBQ3NHOzs7OztJQU0xRixxQ0FBd0I7Ozs7O0lBQUUsb0NBQXNCOzs7OztJQUFFLG1DQUFnQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFN0YXJ0TG9hZGVyLCBTdG9wTG9hZGVyIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IENoYW5nZURldGVjdG9yUmVmLCBDb21wb25lbnQsIElucHV0LCBPbkRlc3Ryb3ksIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmF2aWdhdGlvbkVuZCwgTmF2aWdhdGlvbkVycm9yLCBOYXZpZ2F0aW9uU3RhcnQsIFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyB0YWtlVW50aWxEZXN0cm95IH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcbmltcG9ydCB7IEFjdGlvbnMsIG9mQWN0aW9uU3VjY2Vzc2Z1bCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IGludGVydmFsLCBTdWJzY3JpcHRpb24sIHRpbWVyIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBmaWx0ZXIgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1sb2FkZXItYmFyJyxcbiAgdGVtcGxhdGU6IGBcbiAgICA8ZGl2IGlkPVwiYWJwLWxvYWRlci1iYXJcIiBbbmdDbGFzc109XCJjb250YWluZXJDbGFzc1wiIFtjbGFzcy5pcy1sb2FkaW5nXT1cImlzTG9hZGluZ1wiPlxuICAgICAgPGRpdlxuICAgICAgICBjbGFzcz1cImFicC1wcm9ncmVzc1wiXG4gICAgICAgIFtzdHlsZS53aWR0aC52d109XCJwcm9ncmVzc0xldmVsXCJcbiAgICAgICAgW25nU3R5bGVdPVwie1xuICAgICAgICAgICdiYWNrZ3JvdW5kLWNvbG9yJzogY29sb3IsXG4gICAgICAgICAgJ2JveC1zaGFkb3cnOiBib3hTaGFkb3dcbiAgICAgICAgfVwiXG4gICAgICA+PC9kaXY+XG4gICAgPC9kaXY+XG4gIGAsXG4gIHN0eWxlVXJsczogWycuL2xvYWRlci1iYXIuY29tcG9uZW50LnNjc3MnXSxcbn0pXG5leHBvcnQgY2xhc3MgTG9hZGVyQmFyQ29tcG9uZW50IGltcGxlbWVudHMgT25EZXN0cm95LCBPbkluaXQge1xuICBASW5wdXQoKVxuICBjb250YWluZXJDbGFzcyA9ICdhYnAtbG9hZGVyLWJhcic7XG5cbiAgQElucHV0KClcbiAgY29sb3IgPSAnIzc3YjZmZic7XG5cbiAgQElucHV0KClcbiAgaXNMb2FkaW5nID0gZmFsc2U7XG5cbiAgcHJvZ3Jlc3NMZXZlbCA9IDA7XG5cbiAgaW50ZXJ2YWw6IFN1YnNjcmlwdGlvbjtcblxuICB0aW1lcjogU3Vic2NyaXB0aW9uO1xuXG4gIGludGVydmFsUGVyaW9kID0gMzUwO1xuXG4gIHN0b3BEZWxheSA9IDgyMDtcblxuICBASW5wdXQoKVxuICBmaWx0ZXIgPSAoYWN0aW9uOiBTdGFydExvYWRlciB8IFN0b3BMb2FkZXIpID0+IGFjdGlvbi5wYXlsb2FkLnVybC5pbmRleE9mKCdvcGVuaWQtY29uZmlndXJhdGlvbicpIDwgMDtcblxuICBnZXQgYm94U2hhZG93KCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIGAwIDAgMTBweCByZ2JhKCR7dGhpcy5jb2xvcn0sIDAuNSlgO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBhY3Rpb25zOiBBY3Rpb25zLCBwcml2YXRlIHJvdXRlcjogUm91dGVyLCBwcml2YXRlIGNkUmVmOiBDaGFuZ2VEZXRlY3RvclJlZikge31cblxuICBuZ09uSW5pdCgpIHtcbiAgICB0aGlzLmFjdGlvbnNcbiAgICAgIC5waXBlKFxuICAgICAgICBvZkFjdGlvblN1Y2Nlc3NmdWwoU3RhcnRMb2FkZXIsIFN0b3BMb2FkZXIpLFxuICAgICAgICBmaWx0ZXIodGhpcy5maWx0ZXIpLFxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZShhY3Rpb24gPT4ge1xuICAgICAgICBpZiAoYWN0aW9uIGluc3RhbmNlb2YgU3RhcnRMb2FkZXIpIHRoaXMuc3RhcnRMb2FkaW5nKCk7XG4gICAgICAgIGVsc2UgdGhpcy5zdG9wTG9hZGluZygpO1xuICAgICAgfSk7XG5cbiAgICB0aGlzLnJvdXRlci5ldmVudHNcbiAgICAgIC5waXBlKFxuICAgICAgICBmaWx0ZXIoXG4gICAgICAgICAgZXZlbnQgPT5cbiAgICAgICAgICAgIGV2ZW50IGluc3RhbmNlb2YgTmF2aWdhdGlvblN0YXJ0IHx8IGV2ZW50IGluc3RhbmNlb2YgTmF2aWdhdGlvbkVuZCB8fCBldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25FcnJvcixcbiAgICAgICAgKSxcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoZXZlbnQgPT4ge1xuICAgICAgICBpZiAoZXZlbnQgaW5zdGFuY2VvZiBOYXZpZ2F0aW9uU3RhcnQpIHRoaXMuc3RhcnRMb2FkaW5nKCk7XG4gICAgICAgIGVsc2UgdGhpcy5zdG9wTG9hZGluZygpO1xuICAgICAgfSk7XG4gIH1cblxuICBuZ09uRGVzdHJveSgpIHtcbiAgICB0aGlzLmludGVydmFsLnVuc3Vic2NyaWJlKCk7XG4gIH1cblxuICBzdGFydExvYWRpbmcoKSB7XG4gICAgaWYgKHRoaXMuaXNMb2FkaW5nIHx8IHRoaXMucHJvZ3Jlc3NMZXZlbCAhPT0gMCkgcmV0dXJuO1xuXG4gICAgdGhpcy5pc0xvYWRpbmcgPSB0cnVlO1xuICAgIHRoaXMuaW50ZXJ2YWwgPSBpbnRlcnZhbCh0aGlzLmludGVydmFsUGVyaW9kKS5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgaWYgKHRoaXMucHJvZ3Jlc3NMZXZlbCA8IDc1KSB7XG4gICAgICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCArPSBNYXRoLnJhbmRvbSgpICogMTA7XG4gICAgICB9IGVsc2UgaWYgKHRoaXMucHJvZ3Jlc3NMZXZlbCA8IDkwKSB7XG4gICAgICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCArPSAwLjQ7XG4gICAgICB9IGVsc2UgaWYgKHRoaXMucHJvZ3Jlc3NMZXZlbCA8IDEwMCkge1xuICAgICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgKz0gMC4xO1xuICAgICAgfSBlbHNlIHtcbiAgICAgICAgdGhpcy5pbnRlcnZhbC51bnN1YnNjcmliZSgpO1xuICAgICAgfVxuICAgICAgdGhpcy5jZFJlZi5kZXRlY3RDaGFuZ2VzKCk7XG4gICAgfSk7XG4gIH1cblxuICBzdG9wTG9hZGluZygpIHtcbiAgICB0aGlzLmludGVydmFsLnVuc3Vic2NyaWJlKCk7XG4gICAgdGhpcy5wcm9ncmVzc0xldmVsID0gMTAwO1xuICAgIHRoaXMuaXNMb2FkaW5nID0gZmFsc2U7XG4gICAgaWYgKHRoaXMudGltZXIgJiYgIXRoaXMudGltZXIuY2xvc2VkKSByZXR1cm47XG5cbiAgICB0aGlzLnRpbWVyID0gdGltZXIodGhpcy5zdG9wRGVsYXkpLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgPSAwO1xuICAgICAgdGhpcy5jZFJlZi5kZXRlY3RDaGFuZ2VzKCk7XG4gICAgfSk7XG4gIH1cbn1cbiJdfQ==