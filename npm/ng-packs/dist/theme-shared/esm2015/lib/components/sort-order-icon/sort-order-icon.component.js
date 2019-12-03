/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, EventEmitter, Input, Output } from '@angular/core';
export class SortOrderIconComponent {
    constructor() {
        this.selectedKeyChange = new EventEmitter();
        this.selectedSortKeyChange = new EventEmitter();
        this.orderChange = new EventEmitter();
    }
    /**
     * @deprecated use selectedSortKey instead.
     * @param {?} value
     * @return {?}
     */
    set selectedKey(value) {
        this.selectedSortKey = value;
        this.selectedKeyChange.emit(value);
    }
    /**
     * @return {?}
     */
    get selectedKey() {
        return this._selectedSortKey;
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set selectedSortKey(value) {
        this._selectedSortKey = value;
        this.selectedSortKeyChange.emit(value);
    }
    /**
     * @return {?}
     */
    get selectedSortKey() {
        return this._selectedSortKey;
    }
    /**
     * @deprecated use sortKey instead.
     * @return {?}
     */
    get key() {
        return this.sortKey;
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set key(value) {
        this.sortKey = value;
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
        if (!this.selectedSortKey)
            return 'fa-sort';
        if (this.selectedSortKey === this.sortKey)
            return `fa-sort-${this.order}`;
        else
            return '';
    }
    /**
     * @param {?} key
     * @return {?}
     */
    sort(key) {
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
    }
}
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC1vcmRlci1pY29uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc29ydC1vcmRlci1pY29uL3NvcnQtb3JkZXItaWNvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFNdkUsTUFBTSxPQUFPLHNCQUFzQjtJQUpuQztRQTZCcUIsc0JBQWlCLEdBQUcsSUFBSSxZQUFZLEVBQVUsQ0FBQztRQUMvQywwQkFBcUIsR0FBRyxJQUFJLFlBQVksRUFBVSxDQUFDO1FBeUJuRCxnQkFBVyxHQUFHLElBQUksWUFBWSxFQUFVLENBQUM7SUE4QjlELENBQUM7Ozs7OztJQTFFQyxJQUNJLFdBQVcsQ0FBQyxLQUFhO1FBQzNCLElBQUksQ0FBQyxlQUFlLEdBQUcsS0FBSyxDQUFDO1FBQzdCLElBQUksQ0FBQyxpQkFBaUIsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7SUFDckMsQ0FBQzs7OztJQUNELElBQUksV0FBVztRQUNiLE9BQU8sSUFBSSxDQUFDLGdCQUFnQixDQUFDO0lBQy9CLENBQUM7Ozs7O0lBRUQsSUFDSSxlQUFlLENBQUMsS0FBYTtRQUMvQixJQUFJLENBQUMsZ0JBQWdCLEdBQUcsS0FBSyxDQUFDO1FBQzlCLElBQUksQ0FBQyxxQkFBcUIsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7SUFDekMsQ0FBQzs7OztJQUNELElBQUksZUFBZTtRQUNqQixPQUFPLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQztJQUMvQixDQUFDOzs7OztJQVFELElBQ0ksR0FBRztRQUNMLE9BQU8sSUFBSSxDQUFDLE9BQU8sQ0FBQztJQUN0QixDQUFDOzs7OztJQUNELElBQUksR0FBRyxDQUFDLEtBQWE7UUFDbkIsSUFBSSxDQUFDLE9BQU8sR0FBRyxLQUFLLENBQUM7SUFDdkIsQ0FBQzs7Ozs7SUFLRCxJQUNJLEtBQUssQ0FBQyxLQUEwQjtRQUNsQyxJQUFJLENBQUMsTUFBTSxHQUFHLEtBQUssQ0FBQztRQUNwQixJQUFJLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztJQUMvQixDQUFDOzs7O0lBQ0QsSUFBSSxLQUFLO1FBQ1AsT0FBTyxJQUFJLENBQUMsTUFBTSxDQUFDO0lBQ3JCLENBQUM7Ozs7SUFPRCxJQUFJLElBQUk7UUFDTixJQUFJLENBQUMsSUFBSSxDQUFDLGVBQWU7WUFBRSxPQUFPLFNBQVMsQ0FBQztRQUM1QyxJQUFJLElBQUksQ0FBQyxlQUFlLEtBQUssSUFBSSxDQUFDLE9BQU87WUFBRSxPQUFPLFdBQVcsSUFBSSxDQUFDLEtBQUssRUFBRSxDQUFDOztZQUNyRSxPQUFPLEVBQUUsQ0FBQztJQUNqQixDQUFDOzs7OztJQUVELElBQUksQ0FBQyxHQUFXO1FBQ2QsSUFBSSxDQUFDLFdBQVcsR0FBRyxHQUFHLENBQUMsQ0FBQyxzQkFBc0I7UUFDOUMsSUFBSSxDQUFDLGVBQWUsR0FBRyxHQUFHLENBQUM7UUFDM0IsUUFBUSxJQUFJLENBQUMsS0FBSyxFQUFFO1lBQ2xCLEtBQUssRUFBRTtnQkFDTCxJQUFJLENBQUMsS0FBSyxHQUFHLEtBQUssQ0FBQztnQkFDbkIsSUFBSSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7Z0JBQzdCLE1BQU07WUFDUixLQUFLLEtBQUs7Z0JBQ1IsSUFBSSxDQUFDLEtBQUssR0FBRyxNQUFNLENBQUM7Z0JBQ3BCLElBQUksQ0FBQyxXQUFXLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxDQUFDO2dCQUM5QixNQUFNO1lBQ1IsS0FBSyxNQUFNO2dCQUNULElBQUksQ0FBQyxLQUFLLEdBQUcsRUFBRSxDQUFDO2dCQUNoQixJQUFJLENBQUMsV0FBVyxHQUFHLEVBQUUsQ0FBQyxDQUFDLHNCQUFzQjtnQkFDN0MsSUFBSSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsRUFBRSxDQUFDLENBQUM7Z0JBQzFCLE1BQU07U0FDVDtJQUNILENBQUM7OztZQXBGRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLHFCQUFxQjtnQkFDL0Isc0dBQStDO2FBQ2hEOzs7MEJBUUUsS0FBSzs4QkFTTCxLQUFLO2dDQVNMLE1BQU07b0NBQ04sTUFBTTtrQkFLTixLQUFLO3NCQVFMLEtBQUs7b0JBR0wsS0FBSzswQkFTTCxNQUFNO3dCQUVOLEtBQUs7Ozs7Ozs7SUFwRE4sd0NBQW9DOzs7OztJQUNwQyxrREFBaUM7O0lBdUJqQyxtREFBa0U7O0lBQ2xFLHVEQUFzRTs7SUFhdEUseUNBQ2dCOztJQVdoQiw2Q0FBNEQ7O0lBRTVELDJDQUNrQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgRXZlbnRFbWl0dGVyLCBJbnB1dCwgT3V0cHV0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1zb3J0LW9yZGVyLWljb24nLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9zb3J0LW9yZGVyLWljb24uY29tcG9uZW50Lmh0bWwnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgU29ydE9yZGVySWNvbkNvbXBvbmVudCB7XHJcbiAgcHJpdmF0ZSBfb3JkZXI6ICdhc2MnIHwgJ2Rlc2MnIHwgJyc7XHJcbiAgcHJpdmF0ZSBfc2VsZWN0ZWRTb3J0S2V5OiBzdHJpbmc7XHJcblxyXG4gIC8qKlxyXG4gICAqIEBkZXByZWNhdGVkIHVzZSBzZWxlY3RlZFNvcnRLZXkgaW5zdGVhZC5cclxuICAgKi9cclxuICBASW5wdXQoKVxyXG4gIHNldCBzZWxlY3RlZEtleSh2YWx1ZTogc3RyaW5nKSB7XHJcbiAgICB0aGlzLnNlbGVjdGVkU29ydEtleSA9IHZhbHVlO1xyXG4gICAgdGhpcy5zZWxlY3RlZEtleUNoYW5nZS5lbWl0KHZhbHVlKTtcclxuICB9XHJcbiAgZ2V0IHNlbGVjdGVkS2V5KCk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gdGhpcy5fc2VsZWN0ZWRTb3J0S2V5O1xyXG4gIH1cclxuXHJcbiAgQElucHV0KClcclxuICBzZXQgc2VsZWN0ZWRTb3J0S2V5KHZhbHVlOiBzdHJpbmcpIHtcclxuICAgIHRoaXMuX3NlbGVjdGVkU29ydEtleSA9IHZhbHVlO1xyXG4gICAgdGhpcy5zZWxlY3RlZFNvcnRLZXlDaGFuZ2UuZW1pdCh2YWx1ZSk7XHJcbiAgfVxyXG4gIGdldCBzZWxlY3RlZFNvcnRLZXkoKTogc3RyaW5nIHtcclxuICAgIHJldHVybiB0aGlzLl9zZWxlY3RlZFNvcnRLZXk7XHJcbiAgfVxyXG5cclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgc2VsZWN0ZWRLZXlDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPHN0cmluZz4oKTtcclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgc2VsZWN0ZWRTb3J0S2V5Q2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxzdHJpbmc+KCk7XHJcblxyXG4gIC8qKlxyXG4gICAqIEBkZXByZWNhdGVkIHVzZSBzb3J0S2V5IGluc3RlYWQuXHJcbiAgICovXHJcbiAgQElucHV0KClcclxuICBnZXQga2V5KCk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gdGhpcy5zb3J0S2V5O1xyXG4gIH1cclxuICBzZXQga2V5KHZhbHVlOiBzdHJpbmcpIHtcclxuICAgIHRoaXMuc29ydEtleSA9IHZhbHVlO1xyXG4gIH1cclxuXHJcbiAgQElucHV0KClcclxuICBzb3J0S2V5OiBzdHJpbmc7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgc2V0IG9yZGVyKHZhbHVlOiAnYXNjJyB8ICdkZXNjJyB8ICcnKSB7XHJcbiAgICB0aGlzLl9vcmRlciA9IHZhbHVlO1xyXG4gICAgdGhpcy5vcmRlckNoYW5nZS5lbWl0KHZhbHVlKTtcclxuICB9XHJcbiAgZ2V0IG9yZGVyKCk6ICdhc2MnIHwgJ2Rlc2MnIHwgJycge1xyXG4gICAgcmV0dXJuIHRoaXMuX29yZGVyO1xyXG4gIH1cclxuXHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IG9yZGVyQ2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxzdHJpbmc+KCk7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgaWNvbkNsYXNzOiBzdHJpbmc7XHJcblxyXG4gIGdldCBpY29uKCk6IHN0cmluZyB7XHJcbiAgICBpZiAoIXRoaXMuc2VsZWN0ZWRTb3J0S2V5KSByZXR1cm4gJ2ZhLXNvcnQnO1xyXG4gICAgaWYgKHRoaXMuc2VsZWN0ZWRTb3J0S2V5ID09PSB0aGlzLnNvcnRLZXkpIHJldHVybiBgZmEtc29ydC0ke3RoaXMub3JkZXJ9YDtcclxuICAgIGVsc2UgcmV0dXJuICcnO1xyXG4gIH1cclxuXHJcbiAgc29ydChrZXk6IHN0cmluZykge1xyXG4gICAgdGhpcy5zZWxlY3RlZEtleSA9IGtleTsgLy8gVE9ETzogVG8gYmUgcmVtb3ZlZFxyXG4gICAgdGhpcy5zZWxlY3RlZFNvcnRLZXkgPSBrZXk7XHJcbiAgICBzd2l0Y2ggKHRoaXMub3JkZXIpIHtcclxuICAgICAgY2FzZSAnJzpcclxuICAgICAgICB0aGlzLm9yZGVyID0gJ2FzYyc7XHJcbiAgICAgICAgdGhpcy5vcmRlckNoYW5nZS5lbWl0KCdhc2MnKTtcclxuICAgICAgICBicmVhaztcclxuICAgICAgY2FzZSAnYXNjJzpcclxuICAgICAgICB0aGlzLm9yZGVyID0gJ2Rlc2MnO1xyXG4gICAgICAgIHRoaXMub3JkZXJDaGFuZ2UuZW1pdCgnZGVzYycpO1xyXG4gICAgICAgIGJyZWFrO1xyXG4gICAgICBjYXNlICdkZXNjJzpcclxuICAgICAgICB0aGlzLm9yZGVyID0gJyc7XHJcbiAgICAgICAgdGhpcy5zZWxlY3RlZEtleSA9ICcnOyAvLyBUT0RPOiBUbyBiZSByZW1vdmVkXHJcbiAgICAgICAgdGhpcy5vcmRlckNoYW5nZS5lbWl0KCcnKTtcclxuICAgICAgICBicmVhaztcclxuICAgIH1cclxuICB9XHJcbn1cclxuIl19