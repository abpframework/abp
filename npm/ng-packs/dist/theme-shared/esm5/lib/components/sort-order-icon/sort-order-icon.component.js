/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/sort-order-icon/sort-order-icon.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, EventEmitter, Input, Output } from '@angular/core';
var SortOrderIconComponent = /** @class */ (function () {
    function SortOrderIconComponent() {
        this.selectedKeyChange = new EventEmitter();
        this.orderChange = new EventEmitter();
    }
    Object.defineProperty(SortOrderIconComponent.prototype, "selectedKey", {
        get: /**
         * @return {?}
         */
        function () {
            return this._selectedKey;
        },
        set: /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this._selectedKey = value;
            this.selectedKeyChange.emit(value);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SortOrderIconComponent.prototype, "order", {
        get: /**
         * @return {?}
         */
        function () {
            return this._order;
        },
        set: /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this._order = value;
            this.orderChange.emit(value);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SortOrderIconComponent.prototype, "icon", {
        get: /**
         * @return {?}
         */
        function () {
            if (!this.selectedKey)
                return 'fa-sort';
            if (this.selectedKey === this.key)
                return "fa-sort-" + this.order;
            else
                return '';
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @param {?} key
     * @return {?}
     */
    SortOrderIconComponent.prototype.sort = /**
     * @param {?} key
     * @return {?}
     */
    function (key) {
        this.selectedKey = key;
        switch (this.order) {
            case '':
                this.order = 'asc';
                break;
            case 'asc':
                this.order = 'desc';
                this.orderChange.emit('desc');
                break;
            case 'desc':
                this.order = '';
                this.selectedKey = '';
                break;
        }
    };
    SortOrderIconComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-sort-order-icon',
                    template: "<span class=\"float-right {{ iconClass }}\">\r\n  <i class=\"fa {{ icon }}\"></i>\r\n</span>\r\n"
                }] }
    ];
    SortOrderIconComponent.propDecorators = {
        selectedKey: [{ type: Input }],
        selectedKeyChange: [{ type: Output }],
        key: [{ type: Input }],
        order: [{ type: Input }],
        orderChange: [{ type: Output }],
        iconClass: [{ type: Input }]
    };
    return SortOrderIconComponent;
}());
export { SortOrderIconComponent };
if (false) {
    /**
     * @type {?}
     * @private
     */
    SortOrderIconComponent.prototype._order;
    /**
     * @type {?}
     * @private
     */
    SortOrderIconComponent.prototype._selectedKey;
    /** @type {?} */
    SortOrderIconComponent.prototype.selectedKeyChange;
    /** @type {?} */
    SortOrderIconComponent.prototype.key;
    /** @type {?} */
    SortOrderIconComponent.prototype.orderChange;
    /** @type {?} */
    SortOrderIconComponent.prototype.iconClass;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC1vcmRlci1pY29uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc29ydC1vcmRlci1pY29uL3NvcnQtb3JkZXItaWNvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLFlBQVksRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRXZFO0lBQUE7UUFpQnFCLHNCQUFpQixHQUFHLElBQUksWUFBWSxFQUFVLENBQUM7UUFjL0MsZ0JBQVcsR0FBRyxJQUFJLFlBQVksRUFBVSxDQUFDO0lBMkI5RCxDQUFDO0lBbERDLHNCQUNJLCtDQUFXOzs7O1FBSWY7WUFDRSxPQUFPLElBQUksQ0FBQyxZQUFZLENBQUM7UUFDM0IsQ0FBQzs7Ozs7UUFQRCxVQUNnQixLQUFhO1lBQzNCLElBQUksQ0FBQyxZQUFZLEdBQUcsS0FBSyxDQUFDO1lBQzFCLElBQUksQ0FBQyxpQkFBaUIsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7UUFDckMsQ0FBQzs7O09BQUE7SUFVRCxzQkFDSSx5Q0FBSzs7OztRQUlUO1lBQ0UsT0FBTyxJQUFJLENBQUMsTUFBTSxDQUFDO1FBQ3JCLENBQUM7Ozs7O1FBUEQsVUFDVSxLQUFhO1lBQ3JCLElBQUksQ0FBQyxNQUFNLEdBQUcsS0FBSyxDQUFDO1lBQ3BCLElBQUksQ0FBQyxXQUFXLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO1FBQy9CLENBQUM7OztPQUFBO0lBVUQsc0JBQUksd0NBQUk7Ozs7UUFBUjtZQUNFLElBQUksQ0FBQyxJQUFJLENBQUMsV0FBVztnQkFBRSxPQUFPLFNBQVMsQ0FBQztZQUN4QyxJQUFJLElBQUksQ0FBQyxXQUFXLEtBQUssSUFBSSxDQUFDLEdBQUc7Z0JBQUUsT0FBTyxhQUFXLElBQUksQ0FBQyxLQUFPLENBQUM7O2dCQUM3RCxPQUFPLEVBQUUsQ0FBQztRQUNqQixDQUFDOzs7T0FBQTs7Ozs7SUFFRCxxQ0FBSTs7OztJQUFKLFVBQUssR0FBVztRQUNkLElBQUksQ0FBQyxXQUFXLEdBQUcsR0FBRyxDQUFDO1FBQ3ZCLFFBQVEsSUFBSSxDQUFDLEtBQUssRUFBRTtZQUNsQixLQUFLLEVBQUU7Z0JBQ0wsSUFBSSxDQUFDLEtBQUssR0FBRyxLQUFLLENBQUM7Z0JBQ25CLE1BQU07WUFDUixLQUFLLEtBQUs7Z0JBQ1IsSUFBSSxDQUFDLEtBQUssR0FBRyxNQUFNLENBQUM7Z0JBQ3BCLElBQUksQ0FBQyxXQUFXLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxDQUFDO2dCQUM5QixNQUFNO1lBQ1IsS0FBSyxNQUFNO2dCQUNULElBQUksQ0FBQyxLQUFLLEdBQUcsRUFBRSxDQUFDO2dCQUNoQixJQUFJLENBQUMsV0FBVyxHQUFHLEVBQUUsQ0FBQztnQkFDdEIsTUFBTTtTQUNUO0lBQ0gsQ0FBQzs7Z0JBekRGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUscUJBQXFCO29CQUMvQiw0R0FBK0M7aUJBQ2hEOzs7OEJBS0UsS0FBSztvQ0FTTCxNQUFNO3NCQUVOLEtBQUs7d0JBR0wsS0FBSzs4QkFTTCxNQUFNOzRCQUVOLEtBQUs7O0lBeUJSLDZCQUFDO0NBQUEsQUExREQsSUEwREM7U0F0RFksc0JBQXNCOzs7Ozs7SUFDakMsd0NBQXVCOzs7OztJQUN2Qiw4Q0FBNkI7O0lBVzdCLG1EQUFrRTs7SUFFbEUscUNBQ1k7O0lBV1osNkNBQTREOztJQUU1RCwyQ0FDa0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIEV2ZW50RW1pdHRlciwgSW5wdXQsIE91dHB1dCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdhYnAtc29ydC1vcmRlci1pY29uJyxcclxuICB0ZW1wbGF0ZVVybDogJy4vc29ydC1vcmRlci1pY29uLmNvbXBvbmVudC5odG1sJyxcclxufSlcclxuZXhwb3J0IGNsYXNzIFNvcnRPcmRlckljb25Db21wb25lbnQge1xyXG4gIHByaXZhdGUgX29yZGVyOiBzdHJpbmc7XHJcbiAgcHJpdmF0ZSBfc2VsZWN0ZWRLZXk6IHN0cmluZztcclxuXHJcbiAgQElucHV0KClcclxuICBzZXQgc2VsZWN0ZWRLZXkodmFsdWU6IHN0cmluZykge1xyXG4gICAgdGhpcy5fc2VsZWN0ZWRLZXkgPSB2YWx1ZTtcclxuICAgIHRoaXMuc2VsZWN0ZWRLZXlDaGFuZ2UuZW1pdCh2YWx1ZSk7XHJcbiAgfVxyXG4gIGdldCBzZWxlY3RlZEtleSgpOiBzdHJpbmcge1xyXG4gICAgcmV0dXJuIHRoaXMuX3NlbGVjdGVkS2V5O1xyXG4gIH1cclxuXHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IHNlbGVjdGVkS2V5Q2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxzdHJpbmc+KCk7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAga2V5OiBzdHJpbmc7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgc2V0IG9yZGVyKHZhbHVlOiBzdHJpbmcpIHtcclxuICAgIHRoaXMuX29yZGVyID0gdmFsdWU7XHJcbiAgICB0aGlzLm9yZGVyQ2hhbmdlLmVtaXQodmFsdWUpO1xyXG4gIH1cclxuICBnZXQgb3JkZXIoKTogc3RyaW5nIHtcclxuICAgIHJldHVybiB0aGlzLl9vcmRlcjtcclxuICB9XHJcblxyXG4gIEBPdXRwdXQoKSByZWFkb25seSBvcmRlckNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8c3RyaW5nPigpO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGljb25DbGFzczogc3RyaW5nO1xyXG5cclxuICBnZXQgaWNvbigpOiBzdHJpbmcge1xyXG4gICAgaWYgKCF0aGlzLnNlbGVjdGVkS2V5KSByZXR1cm4gJ2ZhLXNvcnQnO1xyXG4gICAgaWYgKHRoaXMuc2VsZWN0ZWRLZXkgPT09IHRoaXMua2V5KSByZXR1cm4gYGZhLXNvcnQtJHt0aGlzLm9yZGVyfWA7XHJcbiAgICBlbHNlIHJldHVybiAnJztcclxuICB9XHJcblxyXG4gIHNvcnQoa2V5OiBzdHJpbmcpIHtcclxuICAgIHRoaXMuc2VsZWN0ZWRLZXkgPSBrZXk7XHJcbiAgICBzd2l0Y2ggKHRoaXMub3JkZXIpIHtcclxuICAgICAgY2FzZSAnJzpcclxuICAgICAgICB0aGlzLm9yZGVyID0gJ2FzYyc7XHJcbiAgICAgICAgYnJlYWs7XHJcbiAgICAgIGNhc2UgJ2FzYyc6XHJcbiAgICAgICAgdGhpcy5vcmRlciA9ICdkZXNjJztcclxuICAgICAgICB0aGlzLm9yZGVyQ2hhbmdlLmVtaXQoJ2Rlc2MnKTtcclxuICAgICAgICBicmVhaztcclxuICAgICAgY2FzZSAnZGVzYyc6XHJcbiAgICAgICAgdGhpcy5vcmRlciA9ICcnO1xyXG4gICAgICAgIHRoaXMuc2VsZWN0ZWRLZXkgPSAnJztcclxuICAgICAgICBicmVhaztcclxuICAgIH1cclxuICB9XHJcbn1cclxuIl19