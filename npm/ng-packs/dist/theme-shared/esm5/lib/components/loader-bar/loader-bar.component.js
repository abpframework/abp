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
var LoaderBarComponent = /** @class */ (function() {
  function LoaderBarComponent(actions, router, cdRef) {
    var _this = this;
    this.actions = actions;
    this.router = router;
    this.cdRef = cdRef;
    this.containerClass = 'abp-loader-bar';
    this.color = '#77b6ff';
    this.isLoading = false;
    this.progressLevel = 0;
    this.filter
    /**
     * @param {?} action
     * @return {?}
     */ = function(action) {
      return action.payload.url.indexOf('openid-configuration') < 0;
    };
    actions
      .pipe(
        ofActionSuccessful(StartLoader, StopLoader),
        filter(this.filter),
        takeUntilDestroy(this),
      )
      .subscribe(
        /**
         * @param {?} action
         * @return {?}
         */
        function(action) {
          if (action instanceof StartLoader) _this.startLoading();
          else _this.stopLoading();
        },
      );
    router.events
      .pipe(
        filter(
          /**
           * @param {?} event
           * @return {?}
           */
          function(event) {
            return (
              event instanceof NavigationStart || event instanceof NavigationEnd || event instanceof NavigationError
            );
          },
        ),
        takeUntilDestroy(this),
      )
      .subscribe(
        /**
         * @param {?} event
         * @return {?}
         */
        function(event) {
          if (event instanceof NavigationStart) _this.startLoading();
          else _this.stopLoading();
        },
      );
  }
  Object.defineProperty(LoaderBarComponent.prototype, 'boxShadow', {
    /**
     * @return {?}
     */
    get: function() {
      return '0 0 10px rgba(' + this.color + ', 0.5)';
    },
    enumerable: true,
    configurable: true,
  });
  /**
   * @return {?}
   */
  LoaderBarComponent.prototype.ngOnDestroy
  /**
   * @return {?}
   */ = function() {
    this.interval.unsubscribe();
  };
  /**
   * @return {?}
   */
  LoaderBarComponent.prototype.startLoading
  /**
   * @return {?}
   */ = function() {
    var _this = this;
    if (this.isLoading || this.progressLevel !== 0) return;
    this.isLoading = true;
    this.interval = interval(350).subscribe(
      /**
       * @return {?}
       */
      function() {
        if (_this.progressLevel < 75) {
          _this.progressLevel += Math.random() * 10;
        } else if (_this.progressLevel < 90) {
          _this.progressLevel += 0.4;
        } else if (_this.progressLevel < 100) {
          _this.progressLevel += 0.1;
        } else {
          _this.interval.unsubscribe();
        }
        _this.cdRef.detectChanges();
      },
    );
  };
  /**
   * @return {?}
   */
  LoaderBarComponent.prototype.stopLoading
  /**
   * @return {?}
   */ = function() {
    var _this = this;
    this.interval.unsubscribe();
    this.progressLevel = 100;
    this.isLoading = false;
    if (this.timer && !this.timer.closed) return;
    this.timer = timer(820).subscribe(
      /**
       * @return {?}
       */
      function() {
        _this.progressLevel = 0;
        _this.cdRef.detectChanges();
      },
    );
  };
  LoaderBarComponent.decorators = [
    {
      type: Component,
      args: [
        {
          selector: 'abp-loader-bar',
          template:
            '\n    <div id="abp-loader-bar" [ngClass]="containerClass" [class.is-loading]="isLoading">\n      <div\n        class="abp-progress"\n        [style.width.vw]="progressLevel"\n        [ngStyle]="{\n          \'background-color\': color,\n          \'box-shadow\': boxShadow\n        }"\n      ></div>\n    </div>\n  ',
          styles: [
            '.abp-loader-bar{left:0;opacity:0;position:fixed;top:0;transition:opacity .4s linear .4s;z-index:99999}.abp-loader-bar.is-loading{opacity:1;transition:none}.abp-loader-bar .abp-progress{height:3px;left:0;position:fixed;top:0;transition:width .4s}',
          ],
        },
      ],
    },
  ];
  /** @nocollapse */
  LoaderBarComponent.ctorParameters = function() {
    return [{ type: Actions }, { type: Router }, { type: ChangeDetectorRef }];
  };
  LoaderBarComponent.propDecorators = {
    containerClass: [{ type: Input }],
    color: [{ type: Input }],
    isLoading: [{ type: Input }],
    filter: [{ type: Input }],
  };
  return LoaderBarComponent;
})();
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9hZGVyLWJhci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2xvYWRlci1iYXIvbG9hZGVyLWJhci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxXQUFXLEVBQUUsVUFBVSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3ZELE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFhLE1BQU0sZUFBZSxDQUFDO0FBQy9FLE9BQU8sRUFBRSxhQUFhLEVBQUUsZUFBZSxFQUFFLGVBQWUsRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUMxRixPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxvQkFBb0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzFELE9BQU8sRUFBRSxRQUFRLEVBQWdCLEtBQUssRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNyRCxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFFeEM7SUFxQkUsNEJBQW9CLE9BQWdCLEVBQVUsTUFBYyxFQUFVLEtBQXdCO1FBQTlGLGlCQXdCQztRQXhCbUIsWUFBTyxHQUFQLE9BQU8sQ0FBUztRQUFVLFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFtQjtRQTBCOUYsbUJBQWMsR0FBRyxnQkFBZ0IsQ0FBQztRQUdsQyxVQUFLLEdBQUcsU0FBUyxDQUFDO1FBR2xCLGNBQVMsR0FBRyxLQUFLLENBQUM7UUFFbEIsa0JBQWEsR0FBRyxDQUFDLENBQUM7UUFPbEIsV0FBTTs7OztRQUFHLFVBQUMsTUFBZ0MsSUFBSyxPQUFBLE1BQU0sQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLE9BQU8sQ0FBQyxzQkFBc0IsQ0FBQyxHQUFH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MUIsUUFBUSxFQUFFLHFVQVdUOztpQkFFRjs7OztnQkFuQlEsT0FBTztnQkFGMEMsTUFBTTtnQkFEdkQsaUJBQWlCOzs7aUNBcUR2QixLQUFLO3dCQUdMLEtBQUs7NEJBR0wsS0FBSzt5QkFTTCxLQUFLOztJQW9DUix5QkFBQztDQUFBLEFBakdELElBaUdDO1NBakZZLGtCQUFrQjs7O0lBOEI3Qiw0Q0FDa0M7O0lBRWxDLG1DQUNrQjs7SUFFbEIsdUNBQ2tCOztJQUVsQiwyQ0FBa0I7O0lBRWxCLHNDQUF1Qjs7SUFFdkIsbUNBQW9COztJQUVwQixvQ0FDc0c7Ozs7O0lBekMxRixxQ0FBd0I7Ozs7O0lBQUUsb0NBQXNCOzs7OztJQUFFLG1DQUFnQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFN0YXJ0TG9hZGVyLCBTdG9wTG9hZGVyIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IENoYW5nZURldGVjdG9yUmVmLCBDb21wb25lbnQsIElucHV0LCBPbkRlc3Ryb3kgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5hdmlnYXRpb25FbmQsIE5hdmlnYXRpb25FcnJvciwgTmF2aWdhdGlvblN0YXJ0LCBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5pbXBvcnQgeyBBY3Rpb25zLCBvZkFjdGlvblN1Y2Nlc3NmdWwgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBpbnRlcnZhbCwgU3Vic2NyaXB0aW9uLCB0aW1lciB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgZmlsdGVyIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtbG9hZGVyLWJhcicsXG4gIHRlbXBsYXRlOiBgXG4gICAgPGRpdiBpZD1cImFicC1sb2FkZXItYmFyXCIgW25nQ2xhc3NdPVwiY29udGFpbmVyQ2xhc3NcIiBbY2xhc3MuaXMtbG9hZGluZ109XCJpc0xvYWRpbmdcIj5cbiAgICAgIDxkaXZcbiAgICAgICAgY2xhc3M9XCJhYnAtcHJvZ3Jlc3NcIlxuICAgICAgICBbc3R5bGUud2lkdGgudnddPVwicHJvZ3Jlc3NMZXZlbFwiXG4gICAgICAgIFtuZ1N0eWxlXT1cIntcbiAgICAgICAgICAnYmFja2dyb3VuZC1jb2xvcic6IGNvbG9yLFxuICAgICAgICAgICdib3gtc2hhZG93JzogYm94U2hhZG93XG4gICAgICAgIH1cIlxuICAgICAgPjwvZGl2PlxuICAgIDwvZGl2PlxuICBgLFxuICBzdHlsZVVybHM6IFsnLi9sb2FkZXItYmFyLmNvbXBvbmVudC5zY3NzJ11cbn0pXG5leHBvcnQgY2xhc3MgTG9hZGVyQmFyQ29tcG9uZW50IGltcGxlbWVudHMgT25EZXN0cm95IHtcbiAgZ2V0IGJveFNoYWRvdygpOiBzdHJpbmcge1xuICAgIHJldHVybiBgMCAwIDEwcHggcmdiYSgke3RoaXMuY29sb3J9LCAwLjUpYDtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgYWN0aW9uczogQWN0aW9ucywgcHJpdmF0ZSByb3V0ZXI6IFJvdXRlciwgcHJpdmF0ZSBjZFJlZjogQ2hhbmdlRGV0ZWN0b3JSZWYpIHtcbiAgICBhY3Rpb25zXG4gICAgICAucGlwZShcbiAgICAgICAgb2ZBY3Rpb25TdWNjZXNzZnVsKFN0YXJ0TG9hZGVyLCBTdG9wTG9hZGVyKSxcbiAgICAgICAgZmlsdGVyKHRoaXMuZmlsdGVyKSxcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKVxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZShhY3Rpb24gPT4ge1xuICAgICAgICBpZiAoYWN0aW9uIGluc3RhbmNlb2YgU3RhcnRMb2FkZXIpIHRoaXMuc3RhcnRMb2FkaW5nKCk7XG4gICAgICAgIGVsc2UgdGhpcy5zdG9wTG9hZGluZygpO1xuICAgICAgfSk7XG5cbiAgICByb3V0ZXIuZXZlbnRzXG4gICAgICAucGlwZShcbiAgICAgICAgZmlsdGVyKFxuICAgICAgICAgIGV2ZW50ID0+XG4gICAgICAgICAgICBldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25TdGFydCB8fCBldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25FbmQgfHwgZXZlbnQgaW5zdGFuY2VvZiBOYXZpZ2F0aW9uRXJyb3JcbiAgICAgICAgKSxcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKVxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZShldmVudCA9PiB7XG4gICAgICAgIGlmIChldmVudCBpbnN0YW5jZW9mIE5hdmlnYXRpb25TdGFydCkgdGhpcy5zdGFydExvYWRpbmcoKTtcbiAgICAgICAgZWxzZSB0aGlzLnN0b3BMb2FkaW5nKCk7XG4gICAgICB9KTtcbiAgfVxuICBASW5wdXQoKVxuICBjb250YWluZXJDbGFzcyA9ICdhYnAtbG9hZGVyLWJhcic7XG5cbiAgQElucHV0KClcbiAgY29sb3IgPSAnIzc3YjZmZic7XG5cbiAgQElucHV0KClcbiAgaXNMb2FkaW5nID0gZmFsc2U7XG5cbiAgcHJvZ3Jlc3NMZXZlbCA9IDA7XG5cbiAgaW50ZXJ2YWw6IFN1YnNjcmlwdGlvbjtcblxuICB0aW1lcjogU3Vic2NyaXB0aW9uO1xuXG4gIEBJbnB1dCgpXG4gIGZpbHRlciA9IChhY3Rpb246IFN0YXJ0TG9hZGVyIHwgU3RvcExvYWRlcikgPT4gYWN0aW9uLnBheWxvYWQudXJsLmluZGV4T2YoJ29wZW5pZC1jb25maWd1cmF0aW9uJykgPCAwO1xuXG4gIG5nT25EZXN0cm95KCkge1xuICAgIHRoaXMuaW50ZXJ2YWwudW5zdWJzY3JpYmUoKTtcbiAgfVxuXG4gIHN0YXJ0TG9hZGluZygpIHtcbiAgICBpZiAodGhpcy5pc0xvYWRpbmcgfHwgdGhpcy5wcm9ncmVzc0xldmVsICE9PSAwKSByZXR1cm47XG5cbiAgICB0aGlzLmlzTG9hZGluZyA9IHRydWU7XG4gICAgdGhpcy5pbnRlcnZhbCA9IGludGVydmFsKDM1MCkuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCA3NSkge1xuICAgICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgKz0gTWF0aC5yYW5kb20oKSAqIDEwO1xuICAgICAgfSBlbHNlIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCA5MCkge1xuICAgICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgKz0gMC40O1xuICAgICAgfSBlbHNlIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCAxMDApIHtcbiAgICAgICAgdGhpcy5wcm9ncmVzc0xldmVsICs9IDAuMTtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIHRoaXMuaW50ZXJ2YWwudW5zdWJzY3JpYmUoKTtcbiAgICAgIH1cbiAgICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xuICAgIH0pO1xuICB9XG5cbiAgc3RvcExvYWRpbmcoKSB7XG4gICAgdGhpcy5pbnRlcnZhbC51bnN1YnNjcmliZSgpO1xuICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCA9IDEwMDtcbiAgICB0aGlzLmlzTG9hZGluZyA9IGZhbHNlO1xuICAgIGlmICh0aGlzLnRpbWVyICYmICF0aGlzLnRpbWVyLmNsb3NlZCkgcmV0dXJuO1xuXG4gICAgdGhpcy50aW1lciA9IHRpbWVyKDgyMCkuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCA9IDA7XG4gICAgICB0aGlzLmNkUmVmLmRldGVjdENoYW5nZXMoKTtcbiAgICB9KTtcbiAgfVxufVxuIl19
