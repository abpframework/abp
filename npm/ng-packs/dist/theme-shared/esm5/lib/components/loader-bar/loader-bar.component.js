/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, Input } from '@angular/core';
import { Actions, ofActionSuccessful } from '@ngxs/store';
import { LoaderStart, LoaderStop } from '@abp/ng.core';
import { filter } from 'rxjs/operators';
var LoaderBarComponent = /** @class */ (function () {
    function LoaderBarComponent(actions) {
        var _this = this;
        this.actions = actions;
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
            .pipe(ofActionSuccessful(LoaderStart, LoaderStop), filter(this.filter))
            .subscribe((/**
         * @param {?} action
         * @return {?}
         */
        function (action) {
            if (action instanceof LoaderStart) {
                _this.startLoading();
            }
            else {
                _this.stopLoading();
            }
        }));
    }
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
        { type: Actions }
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
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9hZGVyLWJhci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2xvYWRlci1iYXIvbG9hZGVyLWJhci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQVUsS0FBSyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3pELE9BQU8sRUFBRSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDMUQsT0FBTyxFQUFFLFdBQVcsRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDdkQsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBRXhDO0lBMEJFLDRCQUFvQixPQUFnQjtRQUFwQyxpQkFhQztRQWJtQixZQUFPLEdBQVAsT0FBTyxDQUFTO1FBZnBDLG1CQUFjLEdBQVcsZ0JBQWdCLENBQUM7UUFHMUMsa0JBQWEsR0FBVyxjQUFjLENBQUM7UUFHdkMsY0FBUyxHQUFZLEtBQUssQ0FBQztRQUczQixXQUFNOzs7O1FBQUcsVUFBQyxNQUFnQyxJQUFLLE9BQUEsTUFBTSxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsT0FBTyxDQUFDLHNCQUFzQixDQUFDLEdBQUcsQ0FBQyxFQUF0RCxDQUFzRCxFQUFDO1FBRXRHLGtCQUFhLEdBQVcsQ0FBQyxDQUFDO1FBS3hCLE9BQU87YUFDSixJQUFJLENBQ0gsa0JBQWtCLENBQUMsV0FBVyxFQUFFLFVBQVUsQ0FBQyxFQUMzQyxNQUFNLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxDQUNwQjthQUNBLFNBQVM7Ozs7UUFBQyxVQUFBLE1BQU07WUFDZixJQUFJLE1BQU0sWUFBWSxXQUFXLEVBQUU7Z0JBQ2pDLEtBQUksQ0FBQyxZQUFZLEVBQUUsQ0FBQzthQUNyQjtpQkFBTTtnQkFDTCxLQUFJLENBQUMsV0FBVyxFQUFFLENBQUM7YUFDcEI7UUFDSCxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7Ozs7SUFFRCx5Q0FBWTs7O0lBQVo7UUFBQSxpQkFlQztRQWRDLElBQUksQ0FBQyxTQUFTLEdBQUcsSUFBSSxDQUFDOztZQUNoQixRQUFRLEdBQUcsV0FBVzs7O1FBQUM7WUFDM0IsSUFBSSxLQUFJLENBQUMsYUFBYSxHQUFHLEVBQUUsRUFBRTtnQkFDM0IsS0FBSSxDQUFDLGFBQWEsSUFBSSxJQUFJLENBQUMsTUFBTSxFQUFFLEdBQUcsRUFBRSxDQUFDO2FBQzFDO2lCQUFNLElBQUksS0FBSSxDQUFDLGFBQWEsR0FBRyxFQUFFLEVBQUU7Z0JBQ2xDLEtBQUksQ0FBQyxhQUFhLElBQUksR0FBRyxDQUFDO2FBQzNCO2lCQUFNLElBQUksS0FBSSxDQUFDLGFBQWEsR0FBRyxHQUFHLEVBQUU7Z0JBQ25DLEtBQUksQ0FBQyxhQUFhLElBQUksR0FBRyxDQUFDO2FBQzNCO2lCQUFNO2dCQUNMLGFBQWEsQ0FBQyxRQUFRLENBQUMsQ0FBQzthQUN6QjtRQUNILENBQUMsR0FBRSxHQUFHLENBQUM7UUFFUCxJQUFJLENBQUMsUUFBUSxHQUFHLFFBQVEsQ0FBQztJQUMzQixDQUFDOzs7O0lBRUQsd0NBQVc7OztJQUFYO1FBQUEsaUJBUUM7UUFQQyxhQUFhLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDO1FBQzdCLElBQUksQ0FBQyxhQUFhLEdBQUcsR0FBRyxDQUFDO1FBQ3pCLElBQUksQ0FBQyxTQUFTLEdBQUcsS0FBSyxDQUFDO1FBRXZCLFVBQVU7OztRQUFDO1lBQ1QsS0FBSSxDQUFDLGFBQWEsR0FBRyxDQUFDLENBQUM7UUFDekIsQ0FBQyxHQUFFLEdBQUcsQ0FBQyxDQUFDO0lBQ1YsQ0FBQzs7Z0JBbEVGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsZ0JBQWdCO29CQUMxQixRQUFRLEVBQUUsbU1BSVQ7O2lCQUVGOzs7O2dCQVpRLE9BQU87OztpQ0FjYixLQUFLO2dDQUdMLEtBQUs7NEJBR0wsS0FBSzt5QkFHTCxLQUFLOztJQWdEUix5QkFBQztDQUFBLEFBbkVELElBbUVDO1NBMURZLGtCQUFrQjs7O0lBQzdCLDRDQUMwQzs7SUFFMUMsMkNBQ3VDOztJQUV2Qyx1Q0FDMkI7O0lBRTNCLG9DQUNzRzs7SUFFdEcsMkNBQTBCOztJQUUxQixzQ0FBYzs7Ozs7SUFFRixxQ0FBd0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIE9uSW5pdCwgSW5wdXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEFjdGlvbnMsIG9mQWN0aW9uU3VjY2Vzc2Z1bCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IExvYWRlclN0YXJ0LCBMb2FkZXJTdG9wIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IGZpbHRlciB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWxvYWRlci1iYXInLFxuICB0ZW1wbGF0ZTogYFxuICAgIDxkaXYgaWQ9XCJhYnAtbG9hZGVyLWJhclwiIFtuZ0NsYXNzXT1cImNvbnRhaW5lckNsYXNzXCIgW2NsYXNzLmlzLWxvYWRpbmddPVwiaXNMb2FkaW5nXCI+XG4gICAgICA8ZGl2IFtuZ0NsYXNzXT1cInByb2dyZXNzQ2xhc3NcIiBbc3R5bGUud2lkdGgudnddPVwicHJvZ3Jlc3NMZXZlbFwiPjwvZGl2PlxuICAgIDwvZGl2PlxuICBgLFxuICBzdHlsZVVybHM6IFsnLi9sb2FkZXItYmFyLmNvbXBvbmVudC5zY3NzJ10sXG59KVxuZXhwb3J0IGNsYXNzIExvYWRlckJhckNvbXBvbmVudCB7XG4gIEBJbnB1dCgpXG4gIGNvbnRhaW5lckNsYXNzOiBzdHJpbmcgPSAnYWJwLWxvYWRlci1iYXInO1xuXG4gIEBJbnB1dCgpXG4gIHByb2dyZXNzQ2xhc3M6IHN0cmluZyA9ICdhYnAtcHJvZ3Jlc3MnO1xuXG4gIEBJbnB1dCgpXG4gIGlzTG9hZGluZzogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIEBJbnB1dCgpXG4gIGZpbHRlciA9IChhY3Rpb246IExvYWRlclN0YXJ0IHwgTG9hZGVyU3RvcCkgPT4gYWN0aW9uLnBheWxvYWQudXJsLmluZGV4T2YoJ29wZW5pZC1jb25maWd1cmF0aW9uJykgPCAwO1xuXG4gIHByb2dyZXNzTGV2ZWw6IG51bWJlciA9IDA7XG5cbiAgaW50ZXJ2YWw6IGFueTtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGFjdGlvbnM6IEFjdGlvbnMpIHtcbiAgICBhY3Rpb25zXG4gICAgICAucGlwZShcbiAgICAgICAgb2ZBY3Rpb25TdWNjZXNzZnVsKExvYWRlclN0YXJ0LCBMb2FkZXJTdG9wKSxcbiAgICAgICAgZmlsdGVyKHRoaXMuZmlsdGVyKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoYWN0aW9uID0+IHtcbiAgICAgICAgaWYgKGFjdGlvbiBpbnN0YW5jZW9mIExvYWRlclN0YXJ0KSB7XG4gICAgICAgICAgdGhpcy5zdGFydExvYWRpbmcoKTtcbiAgICAgICAgfSBlbHNlIHtcbiAgICAgICAgICB0aGlzLnN0b3BMb2FkaW5nKCk7XG4gICAgICAgIH1cbiAgICAgIH0pO1xuICB9XG5cbiAgc3RhcnRMb2FkaW5nKCkge1xuICAgIHRoaXMuaXNMb2FkaW5nID0gdHJ1ZTtcbiAgICBjb25zdCBpbnRlcnZhbCA9IHNldEludGVydmFsKCgpID0+IHtcbiAgICAgIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCA3NSkge1xuICAgICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgKz0gTWF0aC5yYW5kb20oKSAqIDEwO1xuICAgICAgfSBlbHNlIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCA5MCkge1xuICAgICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgKz0gMC40O1xuICAgICAgfSBlbHNlIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCAxMDApIHtcbiAgICAgICAgdGhpcy5wcm9ncmVzc0xldmVsICs9IDAuMTtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIGNsZWFySW50ZXJ2YWwoaW50ZXJ2YWwpO1xuICAgICAgfVxuICAgIH0sIDMwMCk7XG5cbiAgICB0aGlzLmludGVydmFsID0gaW50ZXJ2YWw7XG4gIH1cblxuICBzdG9wTG9hZGluZygpIHtcbiAgICBjbGVhckludGVydmFsKHRoaXMuaW50ZXJ2YWwpO1xuICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCA9IDEwMDtcbiAgICB0aGlzLmlzTG9hZGluZyA9IGZhbHNlO1xuXG4gICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgPSAwO1xuICAgIH0sIDgwMCk7XG4gIH1cbn1cbiJdfQ==