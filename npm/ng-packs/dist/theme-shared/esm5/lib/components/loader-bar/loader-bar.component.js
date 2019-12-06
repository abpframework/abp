/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/loader-bar/loader-bar.component.ts
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9hZGVyLWJhci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2xvYWRlci1iYXIvbG9hZGVyLWJhci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsV0FBVyxFQUFFLFVBQVUsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUN2RCxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsU0FBUyxFQUFFLEtBQUssRUFBcUIsTUFBTSxlQUFlLENBQUM7QUFDdkYsT0FBTyxFQUFFLGFBQWEsRUFBRSxlQUFlLEVBQUUsZUFBZSxFQUFFLE1BQU0sRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQzFGLE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBQ3RELE9BQU8sRUFBRSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDMUQsT0FBTyxFQUFFLFFBQVEsRUFBZ0IsS0FBSyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ3JELE9BQU8sRUFBRSxNQUFNLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUV4QztJQTJDRSw0QkFBb0IsT0FBZ0IsRUFBVSxNQUFjLEVBQVUsS0FBd0I7UUFBMUUsWUFBTyxHQUFQLE9BQU8sQ0FBUztRQUFVLFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFtQjtRQXpCOUYsbUJBQWMsR0FBRyxnQkFBZ0IsQ0FBQztRQUdsQyxVQUFLLEdBQUcsU0FBUyxDQUFDO1FBR2xCLGNBQVMsR0FBRyxLQUFLLENBQUM7UUFFbEIsa0JBQWEsR0FBRyxDQUFDLENBQUM7UUFNbEIsbUJBQWMsR0FBRyxHQUFHLENBQUM7UUFFckIsY0FBUyxHQUFHLEdBQUcsQ0FBQztRQUdoQixXQUFNOzs7O1FBQUcsVUFBQyxNQUFnQyxJQUFLLE9BQUEsTUFBTSxDQUFDLE9BQU8s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Q0FBQztZQUN2QixLQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsRUFBRSxDQUFDO1FBQzdCLENBQUMsRUFBQyxDQUFDO0lBQ0wsQ0FBQzs7Z0JBdkdGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsZ0JBQWdCO29CQUMxQixRQUFRLEVBQUUscVVBV1Q7O2lCQUVGOzs7O2dCQW5CUSxPQUFPO2dCQUYwQyxNQUFNO2dCQUR2RCxpQkFBaUI7OztpQ0F3QnZCLEtBQUs7d0JBR0wsS0FBSzs0QkFHTCxLQUFLO3lCQWFMLEtBQUs7O0lBb0VSLHlCQUFDO0NBQUEsQUF4R0QsSUF3R0M7U0F4Rlksa0JBQWtCOzs7SUFDN0IsNENBQ2tDOztJQUVsQyxtQ0FDa0I7O0lBRWxCLHVDQUNrQjs7SUFFbEIsMkNBQWtCOztJQUVsQixzQ0FBdUI7O0lBRXZCLG1DQUFvQjs7SUFFcEIsNENBQXFCOztJQUVyQix1Q0FBZ0I7O0lBRWhCLG9DQUNzRzs7Ozs7SUFNMUYscUNBQXdCOzs7OztJQUFFLG9DQUFzQjs7Ozs7SUFBRSxtQ0FBZ0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBTdGFydExvYWRlciwgU3RvcExvYWRlciB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBDaGFuZ2VEZXRlY3RvclJlZiwgQ29tcG9uZW50LCBJbnB1dCwgT25EZXN0cm95LCBPbkluaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5hdmlnYXRpb25FbmQsIE5hdmlnYXRpb25FcnJvciwgTmF2aWdhdGlvblN0YXJ0LCBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5pbXBvcnQgeyBBY3Rpb25zLCBvZkFjdGlvblN1Y2Nlc3NmdWwgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBpbnRlcnZhbCwgU3Vic2NyaXB0aW9uLCB0aW1lciB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgZmlsdGVyIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtbG9hZGVyLWJhcicsXG4gIHRlbXBsYXRlOiBgXG4gICAgPGRpdiBpZD1cImFicC1sb2FkZXItYmFyXCIgW25nQ2xhc3NdPVwiY29udGFpbmVyQ2xhc3NcIiBbY2xhc3MuaXMtbG9hZGluZ109XCJpc0xvYWRpbmdcIj5cbiAgICAgIDxkaXZcbiAgICAgICAgY2xhc3M9XCJhYnAtcHJvZ3Jlc3NcIlxuICAgICAgICBbc3R5bGUud2lkdGgudnddPVwicHJvZ3Jlc3NMZXZlbFwiXG4gICAgICAgIFtuZ1N0eWxlXT1cIntcbiAgICAgICAgICAnYmFja2dyb3VuZC1jb2xvcic6IGNvbG9yLFxuICAgICAgICAgICdib3gtc2hhZG93JzogYm94U2hhZG93XG4gICAgICAgIH1cIlxuICAgICAgPjwvZGl2PlxuICAgIDwvZGl2PlxuICBgLFxuICBzdHlsZVVybHM6IFsnLi9sb2FkZXItYmFyLmNvbXBvbmVudC5zY3NzJ10sXG59KVxuZXhwb3J0IGNsYXNzIExvYWRlckJhckNvbXBvbmVudCBpbXBsZW1lbnRzIE9uRGVzdHJveSwgT25Jbml0IHtcbiAgQElucHV0KClcbiAgY29udGFpbmVyQ2xhc3MgPSAnYWJwLWxvYWRlci1iYXInO1xuXG4gIEBJbnB1dCgpXG4gIGNvbG9yID0gJyM3N2I2ZmYnO1xuXG4gIEBJbnB1dCgpXG4gIGlzTG9hZGluZyA9IGZhbHNlO1xuXG4gIHByb2dyZXNzTGV2ZWwgPSAwO1xuXG4gIGludGVydmFsOiBTdWJzY3JpcHRpb247XG5cbiAgdGltZXI6IFN1YnNjcmlwdGlvbjtcblxuICBpbnRlcnZhbFBlcmlvZCA9IDM1MDtcblxuICBzdG9wRGVsYXkgPSA4MjA7XG5cbiAgQElucHV0KClcbiAgZmlsdGVyID0gKGFjdGlvbjogU3RhcnRMb2FkZXIgfCBTdG9wTG9hZGVyKSA9PiBhY3Rpb24ucGF5bG9hZC51cmwuaW5kZXhPZignb3BlbmlkLWNvbmZpZ3VyYXRpb24nKSA8IDA7XG5cbiAgZ2V0IGJveFNoYWRvdygpOiBzdHJpbmcge1xuICAgIHJldHVybiBgMCAwIDEwcHggcmdiYSgke3RoaXMuY29sb3J9LCAwLjUpYDtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgYWN0aW9uczogQWN0aW9ucywgcHJpdmF0ZSByb3V0ZXI6IFJvdXRlciwgcHJpdmF0ZSBjZFJlZjogQ2hhbmdlRGV0ZWN0b3JSZWYpIHt9XG5cbiAgbmdPbkluaXQoKSB7XG4gICAgdGhpcy5hY3Rpb25zXG4gICAgICAucGlwZShcbiAgICAgICAgb2ZBY3Rpb25TdWNjZXNzZnVsKFN0YXJ0TG9hZGVyLCBTdG9wTG9hZGVyKSxcbiAgICAgICAgZmlsdGVyKHRoaXMuZmlsdGVyKSxcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoYWN0aW9uID0+IHtcbiAgICAgICAgaWYgKGFjdGlvbiBpbnN0YW5jZW9mIFN0YXJ0TG9hZGVyKSB0aGlzLnN0YXJ0TG9hZGluZygpO1xuICAgICAgICBlbHNlIHRoaXMuc3RvcExvYWRpbmcoKTtcbiAgICAgIH0pO1xuXG4gICAgdGhpcy5yb3V0ZXIuZXZlbnRzXG4gICAgICAucGlwZShcbiAgICAgICAgZmlsdGVyKFxuICAgICAgICAgIGV2ZW50ID0+XG4gICAgICAgICAgICBldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25TdGFydCB8fCBldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25FbmQgfHwgZXZlbnQgaW5zdGFuY2VvZiBOYXZpZ2F0aW9uRXJyb3IsXG4gICAgICAgICksXG4gICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcyksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKGV2ZW50ID0+IHtcbiAgICAgICAgaWYgKGV2ZW50IGluc3RhbmNlb2YgTmF2aWdhdGlvblN0YXJ0KSB0aGlzLnN0YXJ0TG9hZGluZygpO1xuICAgICAgICBlbHNlIHRoaXMuc3RvcExvYWRpbmcoKTtcbiAgICAgIH0pO1xuICB9XG5cbiAgbmdPbkRlc3Ryb3koKSB7XG4gICAgdGhpcy5pbnRlcnZhbC51bnN1YnNjcmliZSgpO1xuICB9XG5cbiAgc3RhcnRMb2FkaW5nKCkge1xuICAgIGlmICh0aGlzLmlzTG9hZGluZyB8fCB0aGlzLnByb2dyZXNzTGV2ZWwgIT09IDApIHJldHVybjtcblxuICAgIHRoaXMuaXNMb2FkaW5nID0gdHJ1ZTtcbiAgICB0aGlzLmludGVydmFsID0gaW50ZXJ2YWwodGhpcy5pbnRlcnZhbFBlcmlvZCkuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCA3NSkge1xuICAgICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgKz0gTWF0aC5yYW5kb20oKSAqIDEwO1xuICAgICAgfSBlbHNlIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCA5MCkge1xuICAgICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgKz0gMC40O1xuICAgICAgfSBlbHNlIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCAxMDApIHtcbiAgICAgICAgdGhpcy5wcm9ncmVzc0xldmVsICs9IDAuMTtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIHRoaXMuaW50ZXJ2YWwudW5zdWJzY3JpYmUoKTtcbiAgICAgIH1cbiAgICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xuICAgIH0pO1xuICB9XG5cbiAgc3RvcExvYWRpbmcoKSB7XG4gICAgdGhpcy5pbnRlcnZhbC51bnN1YnNjcmliZSgpO1xuICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCA9IDEwMDtcbiAgICB0aGlzLmlzTG9hZGluZyA9IGZhbHNlO1xuICAgIGlmICh0aGlzLnRpbWVyICYmICF0aGlzLnRpbWVyLmNsb3NlZCkgcmV0dXJuO1xuXG4gICAgdGhpcy50aW1lciA9IHRpbWVyKHRoaXMuc3RvcERlbGF5KS5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgdGhpcy5wcm9ncmVzc0xldmVsID0gMDtcbiAgICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xuICAgIH0pO1xuICB9XG59XG4iXX0=