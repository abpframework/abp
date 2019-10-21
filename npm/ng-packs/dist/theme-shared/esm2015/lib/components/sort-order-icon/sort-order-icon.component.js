/**
 * @fileoverview added by tsickle
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
                template: "<span class=\"float-right {{ iconClass }}\">\n  <i class=\"fa {{ icon }}\"></i>\n</span>\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC1vcmRlci1pY29uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc29ydC1vcmRlci1pY29uL3NvcnQtb3JkZXItaWNvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFNdkUsTUFBTSxPQUFPLHNCQUFzQjtJQUpuQztRQWlCcUIsc0JBQWlCLEdBQUcsSUFBSSxZQUFZLEVBQVUsQ0FBQztRQWMvQyxnQkFBVyxHQUFHLElBQUksWUFBWSxFQUFVLENBQUM7SUEyQjlELENBQUM7Ozs7O0lBbERDLElBQ0ksV0FBVyxDQUFDLEtBQWE7UUFDM0IsSUFBSSxDQUFDLFlBQVksR0FBRyxLQUFLLENBQUM7UUFDMUIsSUFBSSxDQUFDLGlCQUFpQixDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztJQUNyQyxDQUFDOzs7O0lBQ0QsSUFBSSxXQUFXO1FBQ2IsT0FBTyxJQUFJLENBQUMsWUFBWSxDQUFDO0lBQzNCLENBQUM7Ozs7O0lBT0QsSUFDSSxLQUFLLENBQUMsS0FBYTtRQUNyQixJQUFJLENBQUMsTUFBTSxHQUFHLEtBQUssQ0FBQztRQUNwQixJQUFJLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztJQUMvQixDQUFDOzs7O0lBQ0QsSUFBSSxLQUFLO1FBQ1AsT0FBTyxJQUFJLENBQUMsTUFBTSxDQUFDO0lBQ3JCLENBQUM7Ozs7SUFPRCxJQUFJLElBQUk7UUFDTixJQUFJLENBQUMsSUFBSSxDQUFDLFdBQVc7WUFBRSxPQUFPLFNBQVMsQ0FBQztRQUN4QyxJQUFJLElBQUksQ0FBQyxXQUFXLEtBQUssSUFBSSxDQUFDLEdBQUc7WUFBRSxPQUFPLFdBQVcsSUFBSSxDQUFDLEtBQUssRUFBRSxDQUFDOztZQUM3RCxPQUFPLEVBQUUsQ0FBQztJQUNqQixDQUFDOzs7OztJQUVELElBQUksQ0FBQyxHQUFXO1FBQ2QsSUFBSSxDQUFDLFdBQVcsR0FBRyxHQUFHLENBQUM7UUFDdkIsUUFBUSxJQUFJLENBQUMsS0FBSyxFQUFFO1lBQ2xCLEtBQUssRUFBRTtnQkFDTCxJQUFJLENBQUMsS0FBSyxHQUFHLEtBQUssQ0FBQztnQkFDbkIsTUFBTTtZQUNSLEtBQUssS0FBSztnQkFDUixJQUFJLENBQUMsS0FBSyxHQUFHLE1BQU0sQ0FBQztnQkFDcEIsSUFBSSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUM7Z0JBQzlCLE1BQU07WUFDUixLQUFLLE1BQU07Z0JBQ1QsSUFBSSxDQUFDLEtBQUssR0FBRyxFQUFFLENBQUM7Z0JBQ2hCLElBQUksQ0FBQyxXQUFXLEdBQUcsRUFBRSxDQUFDO2dCQUN0QixNQUFNO1NBQ1Q7SUFDSCxDQUFDOzs7WUF6REYsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxxQkFBcUI7Z0JBQy9CLHNHQUErQzthQUNoRDs7OzBCQUtFLEtBQUs7Z0NBU0wsTUFBTTtrQkFFTixLQUFLO29CQUdMLEtBQUs7MEJBU0wsTUFBTTt3QkFFTixLQUFLOzs7Ozs7O0lBNUJOLHdDQUF1Qjs7Ozs7SUFDdkIsOENBQTZCOztJQVc3QixtREFBa0U7O0lBRWxFLHFDQUNZOztJQVdaLDZDQUE0RDs7SUFFNUQsMkNBQ2tCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBFdmVudEVtaXR0ZXIsIElucHV0LCBPdXRwdXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXNvcnQtb3JkZXItaWNvbicsXG4gIHRlbXBsYXRlVXJsOiAnLi9zb3J0LW9yZGVyLWljb24uY29tcG9uZW50Lmh0bWwnLFxufSlcbmV4cG9ydCBjbGFzcyBTb3J0T3JkZXJJY29uQ29tcG9uZW50IHtcbiAgcHJpdmF0ZSBfb3JkZXI6IHN0cmluZztcbiAgcHJpdmF0ZSBfc2VsZWN0ZWRLZXk6IHN0cmluZztcblxuICBASW5wdXQoKVxuICBzZXQgc2VsZWN0ZWRLZXkodmFsdWU6IHN0cmluZykge1xuICAgIHRoaXMuX3NlbGVjdGVkS2V5ID0gdmFsdWU7XG4gICAgdGhpcy5zZWxlY3RlZEtleUNoYW5nZS5lbWl0KHZhbHVlKTtcbiAgfVxuICBnZXQgc2VsZWN0ZWRLZXkoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy5fc2VsZWN0ZWRLZXk7XG4gIH1cblxuICBAT3V0cHV0KCkgcmVhZG9ubHkgc2VsZWN0ZWRLZXlDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPHN0cmluZz4oKTtcblxuICBASW5wdXQoKVxuICBrZXk6IHN0cmluZztcblxuICBASW5wdXQoKVxuICBzZXQgb3JkZXIodmFsdWU6IHN0cmluZykge1xuICAgIHRoaXMuX29yZGVyID0gdmFsdWU7XG4gICAgdGhpcy5vcmRlckNoYW5nZS5lbWl0KHZhbHVlKTtcbiAgfVxuICBnZXQgb3JkZXIoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy5fb3JkZXI7XG4gIH1cblxuICBAT3V0cHV0KCkgcmVhZG9ubHkgb3JkZXJDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPHN0cmluZz4oKTtcblxuICBASW5wdXQoKVxuICBpY29uQ2xhc3M6IHN0cmluZztcblxuICBnZXQgaWNvbigpOiBzdHJpbmcge1xuICAgIGlmICghdGhpcy5zZWxlY3RlZEtleSkgcmV0dXJuICdmYS1zb3J0JztcbiAgICBpZiAodGhpcy5zZWxlY3RlZEtleSA9PT0gdGhpcy5rZXkpIHJldHVybiBgZmEtc29ydC0ke3RoaXMub3JkZXJ9YDtcbiAgICBlbHNlIHJldHVybiAnJztcbiAgfVxuXG4gIHNvcnQoa2V5OiBzdHJpbmcpIHtcbiAgICB0aGlzLnNlbGVjdGVkS2V5ID0ga2V5O1xuICAgIHN3aXRjaCAodGhpcy5vcmRlcikge1xuICAgICAgY2FzZSAnJzpcbiAgICAgICAgdGhpcy5vcmRlciA9ICdhc2MnO1xuICAgICAgICBicmVhaztcbiAgICAgIGNhc2UgJ2FzYyc6XG4gICAgICAgIHRoaXMub3JkZXIgPSAnZGVzYyc7XG4gICAgICAgIHRoaXMub3JkZXJDaGFuZ2UuZW1pdCgnZGVzYycpO1xuICAgICAgICBicmVhaztcbiAgICAgIGNhc2UgJ2Rlc2MnOlxuICAgICAgICB0aGlzLm9yZGVyID0gJyc7XG4gICAgICAgIHRoaXMuc2VsZWN0ZWRLZXkgPSAnJztcbiAgICAgICAgYnJlYWs7XG4gICAgfVxuICB9XG59XG4iXX0=