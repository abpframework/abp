/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/sort-order-icon/sort-order-icon.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, EventEmitter, Input, Output } from '@angular/core';
export class SortOrderIconComponent {
    constructor() {
        this.selectedKeyChange = new EventEmitter();
        this.orderChange = new EventEmitter();
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set selectedKey(value) {
        this._selectedKey = value;
        this.selectedKeyChange.emit(value);
    }
    /**
     * @return {?}
     */
    get selectedKey() {
        return this._selectedKey;
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set order(value) {
        this._order = value;
        this.orderChange.emit(value);
    }
    /**
     * @return {?}
     */
    get order() {
        return this._order;
    }
    /**
     * @return {?}
     */
    get icon() {
        if (!this.selectedKey)
            return 'fa-sort';
        if (this.selectedKey === this.key)
            return `fa-sort-${this.order}`;
        else
            return '';
    }
    /**
     * @param {?} key
     * @return {?}
     */
    sort(key) {
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
    }
}
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC1vcmRlci1pY29uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc29ydC1vcmRlci1pY29uL3NvcnQtb3JkZXItaWNvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLFlBQVksRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBTXZFLE1BQU0sT0FBTyxzQkFBc0I7SUFKbkM7UUFpQnFCLHNCQUFpQixHQUFHLElBQUksWUFBWSxFQUFVLENBQUM7UUFjL0MsZ0JBQVcsR0FBRyxJQUFJLFlBQVksRUFBVSxDQUFDO0lBMkI5RCxDQUFDOzs7OztJQWxEQyxJQUNJLFdBQVcsQ0FBQyxLQUFhO1FBQzNCLElBQUksQ0FBQyxZQUFZLEdBQUcsS0FBSyxDQUFDO1FBQzFCLElBQUksQ0FBQyxpQkFBaUIsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7SUFDckMsQ0FBQzs7OztJQUNELElBQUksV0FBVztRQUNiLE9BQU8sSUFBSSxDQUFDLFlBQVksQ0FBQztJQUMzQixDQUFDOzs7OztJQU9ELElBQ0ksS0FBSyxDQUFDLEtBQWE7UUFDckIsSUFBSSxDQUFDLE1BQU0sR0FBRyxLQUFLLENBQUM7UUFDcEIsSUFBSSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7SUFDL0IsQ0FBQzs7OztJQUNELElBQUksS0FBSztRQUNQLE9BQU8sSUFBSSxDQUFDLE1BQU0sQ0FBQztJQUNyQixDQUFDOzs7O0lBT0QsSUFBSSxJQUFJO1FBQ04sSUFBSSxDQUFDLElBQUksQ0FBQyxXQUFXO1lBQUUsT0FBTyxTQUFTLENBQUM7UUFDeEMsSUFBSSxJQUFJLENBQUMsV0FBVyxLQUFLLElBQUksQ0FBQyxHQUFHO1lBQUUsT0FBTyxXQUFXLElBQUksQ0FBQyxLQUFLLEVBQUUsQ0FBQzs7WUFDN0QsT0FBTyxFQUFFLENBQUM7SUFDakIsQ0FBQzs7Ozs7SUFFRCxJQUFJLENBQUMsR0FBVztRQUNkLElBQUksQ0FBQyxXQUFXLEdBQUcsR0FBRyxDQUFDO1FBQ3ZCLFFBQVEsSUFBSSxDQUFDLEtBQUssRUFBRTtZQUNsQixLQUFLLEVBQUU7Z0JBQ0wsSUFBSSxDQUFDLEtBQUssR0FBRyxLQUFLLENBQUM7Z0JBQ25CLE1BQU07WUFDUixLQUFLLEtBQUs7Z0JBQ1IsSUFBSSxDQUFDLEtBQUssR0FBRyxNQUFNLENBQUM7Z0JBQ3BCLElBQUksQ0FBQyxXQUFXLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxDQUFDO2dCQUM5QixNQUFNO1lBQ1IsS0FBSyxNQUFNO2dCQUNULElBQUksQ0FBQyxLQUFLLEdBQUcsRUFBRSxDQUFDO2dCQUNoQixJQUFJLENBQUMsV0FBVyxHQUFHLEVBQUUsQ0FBQztnQkFDdEIsTUFBTTtTQUNUO0lBQ0gsQ0FBQzs7O1lBekRGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUscUJBQXFCO2dCQUMvQiw0R0FBK0M7YUFDaEQ7OzswQkFLRSxLQUFLO2dDQVNMLE1BQU07a0JBRU4sS0FBSztvQkFHTCxLQUFLOzBCQVNMLE1BQU07d0JBRU4sS0FBSzs7Ozs7OztJQTVCTix3Q0FBdUI7Ozs7O0lBQ3ZCLDhDQUE2Qjs7SUFXN0IsbURBQWtFOztJQUVsRSxxQ0FDWTs7SUFXWiw2Q0FBNEQ7O0lBRTVELDJDQUNrQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgRXZlbnRFbWl0dGVyLCBJbnB1dCwgT3V0cHV0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1zb3J0LW9yZGVyLWljb24nLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9zb3J0LW9yZGVyLWljb24uY29tcG9uZW50Lmh0bWwnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgU29ydE9yZGVySWNvbkNvbXBvbmVudCB7XHJcbiAgcHJpdmF0ZSBfb3JkZXI6IHN0cmluZztcclxuICBwcml2YXRlIF9zZWxlY3RlZEtleTogc3RyaW5nO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIHNldCBzZWxlY3RlZEtleSh2YWx1ZTogc3RyaW5nKSB7XHJcbiAgICB0aGlzLl9zZWxlY3RlZEtleSA9IHZhbHVlO1xyXG4gICAgdGhpcy5zZWxlY3RlZEtleUNoYW5nZS5lbWl0KHZhbHVlKTtcclxuICB9XHJcbiAgZ2V0IHNlbGVjdGVkS2V5KCk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gdGhpcy5fc2VsZWN0ZWRLZXk7XHJcbiAgfVxyXG5cclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgc2VsZWN0ZWRLZXlDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPHN0cmluZz4oKTtcclxuXHJcbiAgQElucHV0KClcclxuICBrZXk6IHN0cmluZztcclxuXHJcbiAgQElucHV0KClcclxuICBzZXQgb3JkZXIodmFsdWU6IHN0cmluZykge1xyXG4gICAgdGhpcy5fb3JkZXIgPSB2YWx1ZTtcclxuICAgIHRoaXMub3JkZXJDaGFuZ2UuZW1pdCh2YWx1ZSk7XHJcbiAgfVxyXG4gIGdldCBvcmRlcigpOiBzdHJpbmcge1xyXG4gICAgcmV0dXJuIHRoaXMuX29yZGVyO1xyXG4gIH1cclxuXHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IG9yZGVyQ2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxzdHJpbmc+KCk7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgaWNvbkNsYXNzOiBzdHJpbmc7XHJcblxyXG4gIGdldCBpY29uKCk6IHN0cmluZyB7XHJcbiAgICBpZiAoIXRoaXMuc2VsZWN0ZWRLZXkpIHJldHVybiAnZmEtc29ydCc7XHJcbiAgICBpZiAodGhpcy5zZWxlY3RlZEtleSA9PT0gdGhpcy5rZXkpIHJldHVybiBgZmEtc29ydC0ke3RoaXMub3JkZXJ9YDtcclxuICAgIGVsc2UgcmV0dXJuICcnO1xyXG4gIH1cclxuXHJcbiAgc29ydChrZXk6IHN0cmluZykge1xyXG4gICAgdGhpcy5zZWxlY3RlZEtleSA9IGtleTtcclxuICAgIHN3aXRjaCAodGhpcy5vcmRlcikge1xyXG4gICAgICBjYXNlICcnOlxyXG4gICAgICAgIHRoaXMub3JkZXIgPSAnYXNjJztcclxuICAgICAgICBicmVhaztcclxuICAgICAgY2FzZSAnYXNjJzpcclxuICAgICAgICB0aGlzLm9yZGVyID0gJ2Rlc2MnO1xyXG4gICAgICAgIHRoaXMub3JkZXJDaGFuZ2UuZW1pdCgnZGVzYycpO1xyXG4gICAgICAgIGJyZWFrO1xyXG4gICAgICBjYXNlICdkZXNjJzpcclxuICAgICAgICB0aGlzLm9yZGVyID0gJyc7XHJcbiAgICAgICAgdGhpcy5zZWxlY3RlZEtleSA9ICcnO1xyXG4gICAgICAgIGJyZWFrO1xyXG4gICAgfVxyXG4gIH1cclxufVxyXG4iXX0=