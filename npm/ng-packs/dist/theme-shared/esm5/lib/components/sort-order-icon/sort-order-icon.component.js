/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/sort-order-icon/sort-order-icon.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, EventEmitter, Input, Output } from '@angular/core';
var SortOrderIconComponent = /** @class */ (function () {
    function SortOrderIconComponent() {
        this.selectedKeyChange = new EventEmitter();
        this.selectedSortKeyChange = new EventEmitter();
        this.orderChange = new EventEmitter();
    }
    Object.defineProperty(SortOrderIconComponent.prototype, "selectedKey", {
        get: /**
         * @return {?}
         */
        function () {
            return this._selectedSortKey;
        },
        /**
         * @deprecated use selectedSortKey instead.
         */
        set: /**
         * @deprecated use selectedSortKey instead.
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this.selectedSortKey = value;
            this.selectedKeyChange.emit(value);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SortOrderIconComponent.prototype, "selectedSortKey", {
        get: /**
         * @return {?}
         */
        function () {
            return this._selectedSortKey;
        },
        set: /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this._selectedSortKey = value;
            this.selectedSortKeyChange.emit(value);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SortOrderIconComponent.prototype, "key", {
        /**
         * @deprecated use sortKey instead.
         */
        get: /**
         * @deprecated use sortKey instead.
         * @return {?}
         */
        function () {
            return this.sortKey;
        },
        set: /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this.sortKey = value;
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
            if (!this.selectedSortKey)
                return 'fa-sort';
            if (this.selectedSortKey === this.sortKey)
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
        this.selectedKey = key; // TODO: To be removed
        this.selectedSortKey = key;
        switch (this.order) {
            case '':
                this.order = 'asc';
                this.orderChange.emit('asc');
                break;
            case 'asc':
                this.order = 'desc';
                this.orderChange.emit('desc');
                break;
            case 'desc':
                this.order = '';
                this.selectedKey = ''; // TODO: To be removed
                this.orderChange.emit('');
                break;
        }
    };
    SortOrderIconComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-sort-order-icon',
                    template: "<span class=\"float-right {{ iconClass }}\">\n  <i class=\"fa {{ icon }}\"></i>\n</span>\n"
                }] }
    ];
    SortOrderIconComponent.propDecorators = {
        selectedKey: [{ type: Input }],
        selectedSortKey: [{ type: Input }],
        selectedKeyChange: [{ type: Output }],
        selectedSortKeyChange: [{ type: Output }],
        key: [{ type: Input }],
        sortKey: [{ type: Input }],
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
    SortOrderIconComponent.prototype._selectedSortKey;
    /** @type {?} */
    SortOrderIconComponent.prototype.selectedKeyChange;
    /** @type {?} */
    SortOrderIconComponent.prototype.selectedSortKeyChange;
    /** @type {?} */
    SortOrderIconComponent.prototype.sortKey;
    /** @type {?} */
    SortOrderIconComponent.prototype.orderChange;
    /** @type {?} */
    SortOrderIconComponent.prototype.iconClass;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC1vcmRlci1pY29uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc29ydC1vcmRlci1pY29uL3NvcnQtb3JkZXItaWNvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLFlBQVksRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRXZFO0lBQUE7UUE2QnFCLHNCQUFpQixHQUFHLElBQUksWUFBWSxFQUFVLENBQUM7UUFDL0MsMEJBQXFCLEdBQUcsSUFBSSxZQUFZLEVBQVUsQ0FBQztRQXlCbkQsZ0JBQVcsR0FBRyxJQUFJLFlBQVksRUFBVSxDQUFDO0lBOEI5RCxDQUFDO0lBMUVDLHNCQUNJLCtDQUFXOzs7O1FBSWY7WUFDRSxPQUFPLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQztRQUMvQixDQUFDO1FBVkQ7O1dBRUc7Ozs7OztRQUNILFVBQ2dCLEtBQWE7WUFDM0IsSUFBSSxDQUFDLGVBQWUsR0FBRyxLQUFLLENBQUM7WUFDN0IsSUFBSSxDQUFDLGlCQUFpQixDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUNyQyxDQUFDOzs7T0FBQTtJQUtELHNCQUNJLG1EQUFlOzs7O1FBSW5CO1lBQ0UsT0FBTyxJQUFJLENBQUMsZ0JBQWdCLENBQUM7UUFDL0IsQ0FBQzs7Ozs7UUFQRCxVQUNvQixLQUFhO1lBQy9CLElBQUksQ0FBQyxnQkFBZ0IsR0FBRyxLQUFLLENBQUM7WUFDOUIsSUFBSSxDQUFDLHFCQUFxQixDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUN6QyxDQUFDOzs7T0FBQTtJQVdELHNCQUNJLHVDQUFHO1FBSlA7O1dBRUc7Ozs7O1FBQ0g7WUFFRSxPQUFPLElBQUksQ0FBQyxPQUFPLENBQUM7UUFDdEIsQ0FBQzs7Ozs7UUFDRCxVQUFRLEtBQWE7WUFDbkIsSUFBSSxDQUFDLE9BQU8sR0FBRyxLQUFLLENBQUM7UUFDdkIsQ0FBQzs7O09BSEE7SUFRRCxzQkFDSSx5Q0FBSzs7OztRQUlUO1lBQ0UsT0FBTyxJQUFJLENBQUMsTUFBTSxDQUFDO1FBQ3JCLENBQUM7Ozs7O1FBUEQsVUFDVSxLQUEwQjtZQUNsQyxJQUFJLENBQUMsTUFBTSxHQUFHLEtBQUssQ0FBQztZQUNwQixJQUFJLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUMvQixDQUFDOzs7T0FBQTtJQVVELHNCQUFJLHdDQUFJOzs7O1FBQVI7WUFDRSxJQUFJLENBQUMsSUFBSSxDQUFDLGVBQWU7Z0JBQUUsT0FBTyxTQUFTLENBQUM7WUFDNUMsSUFBSSxJQUFJLENBQUMsZUFBZSxLQUFLLElBQUksQ0FBQyxPQUFPO2dCQUFFLE9BQU8sYUFBVyxJQUFJLENBQUMsS0FBTyxDQUFDOztnQkFDckUsT0FBTyxFQUFFLENBQUM7UUFDakIsQ0FBQzs7O09BQUE7Ozs7O0lBRUQscUNBQUk7Ozs7SUFBSixVQUFLLEdBQVc7UUFDZCxJQUFJLENBQUMsV0FBVyxHQUFHLEdBQUcsQ0FBQyxDQUFDLHNCQUFzQjtRQUM5QyxJQUFJLENBQUMsZUFBZSxHQUFHLEdBQUcsQ0FBQztRQUMzQixRQUFRLElBQUksQ0FBQyxLQUFLLEVBQUU7WUFDbEIsS0FBSyxFQUFFO2dCQUNMLElBQUksQ0FBQyxLQUFLLEdBQUcsS0FBSyxDQUFDO2dCQUNuQixJQUFJLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztnQkFDN0IsTUFBTTtZQUNSLEtBQUssS0FBSztnQkFDUixJQUFJLENBQUMsS0FBSyxHQUFHLE1BQU0sQ0FBQztnQkFDcEIsSUFBSSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUM7Z0JBQzlCLE1BQU07WUFDUixLQUFLLE1BQU07Z0JBQ1QsSUFBSSxDQUFDLEtBQUssR0FBRyxFQUFFLENBQUM7Z0JBQ2hCLElBQUksQ0FBQyxXQUFXLEdBQUcsRUFBRSxDQUFDLENBQUMsc0JBQXNCO2dCQUM3QyxJQUFJLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsQ0FBQztnQkFDMUIsTUFBTTtTQUNUO0lBQ0gsQ0FBQzs7Z0JBcEZGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUscUJBQXFCO29CQUMvQixzR0FBK0M7aUJBQ2hEOzs7OEJBUUUsS0FBSztrQ0FTTCxLQUFLO29DQVNMLE1BQU07d0NBQ04sTUFBTTtzQkFLTixLQUFLOzBCQVFMLEtBQUs7d0JBR0wsS0FBSzs4QkFTTCxNQUFNOzRCQUVOLEtBQUs7O0lBNEJSLDZCQUFDO0NBQUEsQUFyRkQsSUFxRkM7U0FqRlksc0JBQXNCOzs7Ozs7SUFDakMsd0NBQW9DOzs7OztJQUNwQyxrREFBaUM7O0lBdUJqQyxtREFBa0U7O0lBQ2xFLHVEQUFzRTs7SUFhdEUseUNBQ2dCOztJQVdoQiw2Q0FBNEQ7O0lBRTVELDJDQUNrQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgRXZlbnRFbWl0dGVyLCBJbnB1dCwgT3V0cHV0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1zb3J0LW9yZGVyLWljb24nLFxuICB0ZW1wbGF0ZVVybDogJy4vc29ydC1vcmRlci1pY29uLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgU29ydE9yZGVySWNvbkNvbXBvbmVudCB7XG4gIHByaXZhdGUgX29yZGVyOiAnYXNjJyB8ICdkZXNjJyB8ICcnO1xuICBwcml2YXRlIF9zZWxlY3RlZFNvcnRLZXk6IHN0cmluZztcblxuICAvKipcbiAgICogQGRlcHJlY2F0ZWQgdXNlIHNlbGVjdGVkU29ydEtleSBpbnN0ZWFkLlxuICAgKi9cbiAgQElucHV0KClcbiAgc2V0IHNlbGVjdGVkS2V5KHZhbHVlOiBzdHJpbmcpIHtcbiAgICB0aGlzLnNlbGVjdGVkU29ydEtleSA9IHZhbHVlO1xuICAgIHRoaXMuc2VsZWN0ZWRLZXlDaGFuZ2UuZW1pdCh2YWx1ZSk7XG4gIH1cbiAgZ2V0IHNlbGVjdGVkS2V5KCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHRoaXMuX3NlbGVjdGVkU29ydEtleTtcbiAgfVxuXG4gIEBJbnB1dCgpXG4gIHNldCBzZWxlY3RlZFNvcnRLZXkodmFsdWU6IHN0cmluZykge1xuICAgIHRoaXMuX3NlbGVjdGVkU29ydEtleSA9IHZhbHVlO1xuICAgIHRoaXMuc2VsZWN0ZWRTb3J0S2V5Q2hhbmdlLmVtaXQodmFsdWUpO1xuICB9XG4gIGdldCBzZWxlY3RlZFNvcnRLZXkoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy5fc2VsZWN0ZWRTb3J0S2V5O1xuICB9XG5cbiAgQE91dHB1dCgpIHJlYWRvbmx5IHNlbGVjdGVkS2V5Q2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxzdHJpbmc+KCk7XG4gIEBPdXRwdXQoKSByZWFkb25seSBzZWxlY3RlZFNvcnRLZXlDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPHN0cmluZz4oKTtcblxuICAvKipcbiAgICogQGRlcHJlY2F0ZWQgdXNlIHNvcnRLZXkgaW5zdGVhZC5cbiAgICovXG4gIEBJbnB1dCgpXG4gIGdldCBrZXkoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy5zb3J0S2V5O1xuICB9XG4gIHNldCBrZXkodmFsdWU6IHN0cmluZykge1xuICAgIHRoaXMuc29ydEtleSA9IHZhbHVlO1xuICB9XG5cbiAgQElucHV0KClcbiAgc29ydEtleTogc3RyaW5nO1xuXG4gIEBJbnB1dCgpXG4gIHNldCBvcmRlcih2YWx1ZTogJ2FzYycgfCAnZGVzYycgfCAnJykge1xuICAgIHRoaXMuX29yZGVyID0gdmFsdWU7XG4gICAgdGhpcy5vcmRlckNoYW5nZS5lbWl0KHZhbHVlKTtcbiAgfVxuICBnZXQgb3JkZXIoKTogJ2FzYycgfCAnZGVzYycgfCAnJyB7XG4gICAgcmV0dXJuIHRoaXMuX29yZGVyO1xuICB9XG5cbiAgQE91dHB1dCgpIHJlYWRvbmx5IG9yZGVyQ2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxzdHJpbmc+KCk7XG5cbiAgQElucHV0KClcbiAgaWNvbkNsYXNzOiBzdHJpbmc7XG5cbiAgZ2V0IGljb24oKTogc3RyaW5nIHtcbiAgICBpZiAoIXRoaXMuc2VsZWN0ZWRTb3J0S2V5KSByZXR1cm4gJ2ZhLXNvcnQnO1xuICAgIGlmICh0aGlzLnNlbGVjdGVkU29ydEtleSA9PT0gdGhpcy5zb3J0S2V5KSByZXR1cm4gYGZhLXNvcnQtJHt0aGlzLm9yZGVyfWA7XG4gICAgZWxzZSByZXR1cm4gJyc7XG4gIH1cblxuICBzb3J0KGtleTogc3RyaW5nKSB7XG4gICAgdGhpcy5zZWxlY3RlZEtleSA9IGtleTsgLy8gVE9ETzogVG8gYmUgcmVtb3ZlZFxuICAgIHRoaXMuc2VsZWN0ZWRTb3J0S2V5ID0ga2V5O1xuICAgIHN3aXRjaCAodGhpcy5vcmRlcikge1xuICAgICAgY2FzZSAnJzpcbiAgICAgICAgdGhpcy5vcmRlciA9ICdhc2MnO1xuICAgICAgICB0aGlzLm9yZGVyQ2hhbmdlLmVtaXQoJ2FzYycpO1xuICAgICAgICBicmVhaztcbiAgICAgIGNhc2UgJ2FzYyc6XG4gICAgICAgIHRoaXMub3JkZXIgPSAnZGVzYyc7XG4gICAgICAgIHRoaXMub3JkZXJDaGFuZ2UuZW1pdCgnZGVzYycpO1xuICAgICAgICBicmVhaztcbiAgICAgIGNhc2UgJ2Rlc2MnOlxuICAgICAgICB0aGlzLm9yZGVyID0gJyc7XG4gICAgICAgIHRoaXMuc2VsZWN0ZWRLZXkgPSAnJzsgLy8gVE9ETzogVG8gYmUgcmVtb3ZlZFxuICAgICAgICB0aGlzLm9yZGVyQ2hhbmdlLmVtaXQoJycpO1xuICAgICAgICBicmVhaztcbiAgICB9XG4gIH1cbn1cbiJdfQ==