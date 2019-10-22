/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC1vcmRlci1pY29uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc29ydC1vcmRlci1pY29uL3NvcnQtb3JkZXItaWNvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFdkU7SUFBQTtRQWlCcUIsc0JBQWlCLEdBQUcsSUFBSSxZQUFZLEVBQVUsQ0FBQztRQWMvQyxnQkFBVyxHQUFHLElBQUksWUFBWSxFQUFVLENBQUM7SUEyQjlELENBQUM7SUFsREMsc0JBQ0ksK0NBQVc7Ozs7UUFJZjtZQUNFLE9BQU8sSUFBSSxDQUFDLFlBQVksQ0FBQztRQUMzQixDQUFDOzs7OztRQVBELFVBQ2dCLEtBQWE7WUFDM0IsSUFBSSxDQUFDLFlBQVksR0FBRyxLQUFLLENBQUM7WUFDMUIsSUFBSSxDQUFDLGlCQUFpQixDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUNyQyxDQUFDOzs7T0FBQTtJQVVELHNCQUNJLHlDQUFLOzs7O1FBSVQ7WUFDRSxPQUFPLElBQUksQ0FBQyxNQUFNLENBQUM7UUFDckIsQ0FBQzs7Ozs7UUFQRCxVQUNVLEtBQWE7WUFDckIsSUFBSSxDQUFDLE1BQU0sR0FBRyxLQUFLLENBQUM7WUFDcEIsSUFBSSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7UUFDL0IsQ0FBQzs7O09BQUE7SUFVRCxzQkFBSSx3Q0FBSTs7OztRQUFSO1lBQ0UsSUFBSSxDQUFDLElBQUksQ0FBQyxXQUFXO2dCQUFFLE9BQU8sU0FBUyxDQUFDO1lBQ3hDLElBQUksSUFBSSxDQUFDLFdBQVcsS0FBSyxJQUFJLENBQUMsR0FBRztnQkFBRSxPQUFPLGFBQVcsSUFBSSxDQUFDLEtBQU8sQ0FBQzs7Z0JBQzdELE9BQU8sRUFBRSxDQUFDO1FBQ2pCLENBQUM7OztPQUFBOzs7OztJQUVELHFDQUFJOzs7O0lBQUosVUFBSyxHQUFXO1FBQ2QsSUFBSSxDQUFDLFdBQVcsR0FBRyxHQUFHLENBQUM7UUFDdkIsUUFBUSxJQUFJLENBQUMsS0FBSyxFQUFFO1lBQ2xCLEtBQUssRUFBRTtnQkFDTCxJQUFJLENBQUMsS0FBSyxHQUFHLEtBQUssQ0FBQztnQkFDbkIsTUFBTTtZQUNSLEtBQUssS0FBSztnQkFDUixJQUFJLENBQUMsS0FBSyxHQUFHLE1BQU0sQ0FBQztnQkFDcEIsSUFBSSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUM7Z0JBQzlCLE1BQU07WUFDUixLQUFLLE1BQU07Z0JBQ1QsSUFBSSxDQUFDLEtBQUssR0FBRyxFQUFFLENBQUM7Z0JBQ2hCLElBQUksQ0FBQyxXQUFXLEdBQUcsRUFBRSxDQUFDO2dCQUN0QixNQUFNO1NBQ1Q7SUFDSCxDQUFDOztnQkF6REYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxxQkFBcUI7b0JBQy9CLDRHQUErQztpQkFDaEQ7Ozs4QkFLRSxLQUFLO29DQVNMLE1BQU07c0JBRU4sS0FBSzt3QkFHTCxLQUFLOzhCQVNMLE1BQU07NEJBRU4sS0FBSzs7SUF5QlIsNkJBQUM7Q0FBQSxBQTFERCxJQTBEQztTQXREWSxzQkFBc0I7Ozs7OztJQUNqQyx3Q0FBdUI7Ozs7O0lBQ3ZCLDhDQUE2Qjs7SUFXN0IsbURBQWtFOztJQUVsRSxxQ0FDWTs7SUFXWiw2Q0FBNEQ7O0lBRTVELDJDQUNrQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgRXZlbnRFbWl0dGVyLCBJbnB1dCwgT3V0cHV0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1zb3J0LW9yZGVyLWljb24nLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9zb3J0LW9yZGVyLWljb24uY29tcG9uZW50Lmh0bWwnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgU29ydE9yZGVySWNvbkNvbXBvbmVudCB7XHJcbiAgcHJpdmF0ZSBfb3JkZXI6IHN0cmluZztcclxuICBwcml2YXRlIF9zZWxlY3RlZEtleTogc3RyaW5nO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIHNldCBzZWxlY3RlZEtleSh2YWx1ZTogc3RyaW5nKSB7XHJcbiAgICB0aGlzLl9zZWxlY3RlZEtleSA9IHZhbHVlO1xyXG4gICAgdGhpcy5zZWxlY3RlZEtleUNoYW5nZS5lbWl0KHZhbHVlKTtcclxuICB9XHJcbiAgZ2V0IHNlbGVjdGVkS2V5KCk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gdGhpcy5fc2VsZWN0ZWRLZXk7XHJcbiAgfVxyXG5cclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgc2VsZWN0ZWRLZXlDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPHN0cmluZz4oKTtcclxuXHJcbiAgQElucHV0KClcclxuICBrZXk6IHN0cmluZztcclxuXHJcbiAgQElucHV0KClcclxuICBzZXQgb3JkZXIodmFsdWU6IHN0cmluZykge1xyXG4gICAgdGhpcy5fb3JkZXIgPSB2YWx1ZTtcclxuICAgIHRoaXMub3JkZXJDaGFuZ2UuZW1pdCh2YWx1ZSk7XHJcbiAgfVxyXG4gIGdldCBvcmRlcigpOiBzdHJpbmcge1xyXG4gICAgcmV0dXJuIHRoaXMuX29yZGVyO1xyXG4gIH1cclxuXHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IG9yZGVyQ2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxzdHJpbmc+KCk7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgaWNvbkNsYXNzOiBzdHJpbmc7XHJcblxyXG4gIGdldCBpY29uKCk6IHN0cmluZyB7XHJcbiAgICBpZiAoIXRoaXMuc2VsZWN0ZWRLZXkpIHJldHVybiAnZmEtc29ydCc7XHJcbiAgICBpZiAodGhpcy5zZWxlY3RlZEtleSA9PT0gdGhpcy5rZXkpIHJldHVybiBgZmEtc29ydC0ke3RoaXMub3JkZXJ9YDtcclxuICAgIGVsc2UgcmV0dXJuICcnO1xyXG4gIH1cclxuXHJcbiAgc29ydChrZXk6IHN0cmluZykge1xyXG4gICAgdGhpcy5zZWxlY3RlZEtleSA9IGtleTtcclxuICAgIHN3aXRjaCAodGhpcy5vcmRlcikge1xyXG4gICAgICBjYXNlICcnOlxyXG4gICAgICAgIHRoaXMub3JkZXIgPSAnYXNjJztcclxuICAgICAgICBicmVhaztcclxuICAgICAgY2FzZSAnYXNjJzpcclxuICAgICAgICB0aGlzLm9yZGVyID0gJ2Rlc2MnO1xyXG4gICAgICAgIHRoaXMub3JkZXJDaGFuZ2UuZW1pdCgnZGVzYycpO1xyXG4gICAgICAgIGJyZWFrO1xyXG4gICAgICBjYXNlICdkZXNjJzpcclxuICAgICAgICB0aGlzLm9yZGVyID0gJyc7XHJcbiAgICAgICAgdGhpcy5zZWxlY3RlZEtleSA9ICcnO1xyXG4gICAgICAgIGJyZWFrO1xyXG4gICAgfVxyXG4gIH1cclxufVxyXG4iXX0=