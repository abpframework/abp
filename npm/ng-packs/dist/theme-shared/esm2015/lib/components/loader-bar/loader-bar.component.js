/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, Input } from '@angular/core';
import { Actions, ofActionSuccessful } from '@ngxs/store';
import { LoaderStart, LoaderStop } from '@abp/ng.core';
import { filter } from 'rxjs/operators';
export class LoaderBarComponent {
    /**
     * @param {?} actions
     */
    constructor(actions) {
        this.actions = actions;
        this.containerClass = 'abp-loader-bar';
        this.progressClass = 'abp-progress';
        this.isLoading = false;
        this.filter = (/**
         * @param {?} action
         * @return {?}
         */
        (action) => action.payload.url.indexOf('openid-configuration') < 0);
        this.progressLevel = 0;
        actions
            .pipe(ofActionSuccessful(LoaderStart, LoaderStop), filter(this.filter))
            .subscribe((/**
         * @param {?} action
         * @return {?}
         */
        action => {
            if (action instanceof LoaderStart) {
                this.startLoading();
            }
            else {
                this.stopLoading();
            }
        }));
    }
    /**
     * @return {?}
     */
    startLoading() {
        this.isLoading = true;
        /** @type {?} */
        const interval = setInterval((/**
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
                clearInterval(interval);
            }
        }), 300);
        this.interval = interval;
    }
    /**
     * @return {?}
     */
    stopLoading() {
        clearInterval(this.interval);
        this.progressLevel = 100;
        this.isLoading = false;
        setTimeout((/**
         * @return {?}
         */
        () => {
            this.progressLevel = 0;
        }), 800);
    }
}
LoaderBarComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-loader-bar',
                template: `
    <div id="abp-loader-bar" [ngClass]="containerClass" [class.is-loading]="isLoading">
      <div [ngClass]="progressClass" [style.width.vw]="progressLevel"></div>
    </div>
  `,
                styles: [".abp-loader-bar{left:0;opacity:0;position:fixed;top:0;transition:opacity .4s linear .4s;z-index:99999}.abp-loader-bar.is-loading{opacity:1;transition:none}.abp-loader-bar .abp-progress{background:#77b6ff;box-shadow:0 0 10px rgba(119,182,255,.7);height:2px;left:0;position:fixed;top:0;transition:width .4s}"]
            }] }
];
/** @nocollapse */
LoaderBarComponent.ctorParameters = () => [
    { type: Actions }
];
LoaderBarComponent.propDecorators = {
    containerClass: [{ type: Input }],
    progressClass: [{ type: Input }],
    isLoading: [{ type: Input }],
    filter: [{ type: Input }]
};
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9hZGVyLWJhci5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2xvYWRlci1iYXIvbG9hZGVyLWJhci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQVUsS0FBSyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3pELE9BQU8sRUFBRSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDMUQsT0FBTyxFQUFFLFdBQVcsRUFBRSxVQUFVLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDdkQsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBV3hDLE1BQU0sT0FBTyxrQkFBa0I7Ozs7SUFpQjdCLFlBQW9CLE9BQWdCO1FBQWhCLFlBQU8sR0FBUCxPQUFPLENBQVM7UUFmcEMsbUJBQWMsR0FBVyxnQkFBZ0IsQ0FBQztRQUcxQyxrQkFBYSxHQUFXLGNBQWMsQ0FBQztRQUd2QyxjQUFTLEdBQVksS0FBSyxDQUFDO1FBRzNCLFdBQU07Ozs7UUFBRyxDQUFDLE1BQWdDLEVBQUUsRUFBRSxDQUFDLE1BQU0sQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLE9BQU8sQ0FBQyxzQkFBc0IsQ0FBQyxHQUFHLENBQUMsRUFBQztRQUV0RyxrQkFBYSxHQUFXLENBQUMsQ0FBQztRQUt4QixPQUFPO2FBQ0osSUFBSSxDQUNILGtCQUFrQixDQUFDLFdBQVcsRUFBRSxVQUFVLENBQUMsRUFDM0MsTUFBTSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsQ0FDcEI7YUFDQSxTQUFTOzs7O1FBQUMsTUFBTSxDQUFDLEVBQUU7WUFDbEIsSUFBSSxNQUFNLFlBQVksV0FBVyxFQUFFO2dCQUNqQyxJQUFJLENBQUMsWUFBWSxFQUFFLENBQUM7YUFDckI7aUJBQU07Z0JBQ0wsSUFBSSxDQUFDLFdBQVcsRUFBRSxDQUFDO2FBQ3BCO1FBQ0gsQ0FBQyxFQUFDLENBQUM7SUFDUCxDQUFDOzs7O0lBRUQsWUFBWTtRQUNWLElBQUksQ0FBQyxTQUFTLEdBQUcsSUFBSSxDQUFDOztjQUNoQixRQUFRLEdBQUcsV0FBVzs7O1FBQUMsR0FBRyxFQUFFO1lBQ2hDLElBQUksSUFBSSxDQUFDLGFBQWEsR0FBRyxFQUFFLEVBQUU7Z0JBQzNCLElBQUksQ0FBQyxhQUFhLElBQUksSUFBSSxDQUFDLE1BQU0sRUFBRSxHQUFHLEVBQUUsQ0FBQzthQUMxQztpQkFBTSxJQUFJLElBQUksQ0FBQyxhQUFhLEdBQUcsRUFBRSxFQUFFO2dCQUNsQyxJQUFJLENBQUMsYUFBYSxJQUFJLEdBQUcsQ0FBQzthQUMzQjtpQkFBTSxJQUFJLElBQUksQ0FBQyxhQUFhLEdBQUcsR0FBRyxFQUFFO2dCQUNuQyxJQUFJLENBQUMsYUFBYSxJQUFJLEdBQUcsQ0FBQzthQUMzQjtpQkFBTTtnQkFDTCxhQUFhLENBQUMsUUFBUSxDQUFDLENBQUM7YUFDekI7UUFDSCxDQUFDLEdBQUUsR0FBRyxDQUFDO1FBRVAsSUFBSSxDQUFDLFFBQVEsR0FBRyxRQUFRLENBQUM7SUFDM0IsQ0FBQzs7OztJQUVELFdBQVc7UUFDVCxhQUFhLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDO1FBQzdCLElBQUksQ0FBQyxhQUFhLEdBQUcsR0FBRyxDQUFDO1FBQ3pCLElBQUksQ0FBQyxTQUFTLEdBQUcsS0FBSyxDQUFDO1FBRXZCLFVBQVU7OztRQUFDLEdBQUcsRUFBRTtZQUNkLElBQUksQ0FBQyxhQUFhLEdBQUcsQ0FBQyxDQUFDO1FBQ3pCLENBQUMsR0FBRSxHQUFHLENBQUMsQ0FBQztJQUNWLENBQUM7OztZQWxFRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGdCQUFnQjtnQkFDMUIsUUFBUSxFQUFFOzs7O0dBSVQ7O2FBRUY7Ozs7WUFaUSxPQUFPOzs7NkJBY2IsS0FBSzs0QkFHTCxLQUFLO3dCQUdMLEtBQUs7cUJBR0wsS0FBSzs7OztJQVROLDRDQUMwQzs7SUFFMUMsMkNBQ3VDOztJQUV2Qyx1Q0FDMkI7O0lBRTNCLG9DQUNzRzs7SUFFdEcsMkNBQTBCOztJQUUxQixzQ0FBYzs7Ozs7SUFFRixxQ0FBd0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIE9uSW5pdCwgSW5wdXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEFjdGlvbnMsIG9mQWN0aW9uU3VjY2Vzc2Z1bCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IExvYWRlclN0YXJ0LCBMb2FkZXJTdG9wIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IGZpbHRlciB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWxvYWRlci1iYXInLFxuICB0ZW1wbGF0ZTogYFxuICAgIDxkaXYgaWQ9XCJhYnAtbG9hZGVyLWJhclwiIFtuZ0NsYXNzXT1cImNvbnRhaW5lckNsYXNzXCIgW2NsYXNzLmlzLWxvYWRpbmddPVwiaXNMb2FkaW5nXCI+XG4gICAgICA8ZGl2IFtuZ0NsYXNzXT1cInByb2dyZXNzQ2xhc3NcIiBbc3R5bGUud2lkdGgudnddPVwicHJvZ3Jlc3NMZXZlbFwiPjwvZGl2PlxuICAgIDwvZGl2PlxuICBgLFxuICBzdHlsZVVybHM6IFsnLi9sb2FkZXItYmFyLmNvbXBvbmVudC5zY3NzJ10sXG59KVxuZXhwb3J0IGNsYXNzIExvYWRlckJhckNvbXBvbmVudCB7XG4gIEBJbnB1dCgpXG4gIGNvbnRhaW5lckNsYXNzOiBzdHJpbmcgPSAnYWJwLWxvYWRlci1iYXInO1xuXG4gIEBJbnB1dCgpXG4gIHByb2dyZXNzQ2xhc3M6IHN0cmluZyA9ICdhYnAtcHJvZ3Jlc3MnO1xuXG4gIEBJbnB1dCgpXG4gIGlzTG9hZGluZzogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIEBJbnB1dCgpXG4gIGZpbHRlciA9IChhY3Rpb246IExvYWRlclN0YXJ0IHwgTG9hZGVyU3RvcCkgPT4gYWN0aW9uLnBheWxvYWQudXJsLmluZGV4T2YoJ29wZW5pZC1jb25maWd1cmF0aW9uJykgPCAwO1xuXG4gIHByb2dyZXNzTGV2ZWw6IG51bWJlciA9IDA7XG5cbiAgaW50ZXJ2YWw6IGFueTtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGFjdGlvbnM6IEFjdGlvbnMpIHtcbiAgICBhY3Rpb25zXG4gICAgICAucGlwZShcbiAgICAgICAgb2ZBY3Rpb25TdWNjZXNzZnVsKExvYWRlclN0YXJ0LCBMb2FkZXJTdG9wKSxcbiAgICAgICAgZmlsdGVyKHRoaXMuZmlsdGVyKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoYWN0aW9uID0+IHtcbiAgICAgICAgaWYgKGFjdGlvbiBpbnN0YW5jZW9mIExvYWRlclN0YXJ0KSB7XG4gICAgICAgICAgdGhpcy5zdGFydExvYWRpbmcoKTtcbiAgICAgICAgfSBlbHNlIHtcbiAgICAgICAgICB0aGlzLnN0b3BMb2FkaW5nKCk7XG4gICAgICAgIH1cbiAgICAgIH0pO1xuICB9XG5cbiAgc3RhcnRMb2FkaW5nKCkge1xuICAgIHRoaXMuaXNMb2FkaW5nID0gdHJ1ZTtcbiAgICBjb25zdCBpbnRlcnZhbCA9IHNldEludGVydmFsKCgpID0+IHtcbiAgICAgIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCA3NSkge1xuICAgICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgKz0gTWF0aC5yYW5kb20oKSAqIDEwO1xuICAgICAgfSBlbHNlIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCA5MCkge1xuICAgICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgKz0gMC40O1xuICAgICAgfSBlbHNlIGlmICh0aGlzLnByb2dyZXNzTGV2ZWwgPCAxMDApIHtcbiAgICAgICAgdGhpcy5wcm9ncmVzc0xldmVsICs9IDAuMTtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIGNsZWFySW50ZXJ2YWwoaW50ZXJ2YWwpO1xuICAgICAgfVxuICAgIH0sIDMwMCk7XG5cbiAgICB0aGlzLmludGVydmFsID0gaW50ZXJ2YWw7XG4gIH1cblxuICBzdG9wTG9hZGluZygpIHtcbiAgICBjbGVhckludGVydmFsKHRoaXMuaW50ZXJ2YWwpO1xuICAgIHRoaXMucHJvZ3Jlc3NMZXZlbCA9IDEwMDtcbiAgICB0aGlzLmlzTG9hZGluZyA9IGZhbHNlO1xuXG4gICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICB0aGlzLnByb2dyZXNzTGV2ZWwgPSAwO1xuICAgIH0sIDgwMCk7XG4gIH1cbn1cbiJdfQ==