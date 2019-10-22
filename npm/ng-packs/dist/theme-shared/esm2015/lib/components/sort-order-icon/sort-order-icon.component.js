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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC1vcmRlci1pY29uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc29ydC1vcmRlci1pY29uL3NvcnQtb3JkZXItaWNvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFNdkUsTUFBTSxPQUFPLHNCQUFzQjtJQUpuQztRQWlCcUIsc0JBQWlCLEdBQUcsSUFBSSxZQUFZLEVBQVUsQ0FBQztRQWMvQyxnQkFBVyxHQUFHLElBQUksWUFBWSxFQUFVLENBQUM7SUEyQjlELENBQUM7Ozs7O0lBbERDLElBQ0ksV0FBVyxDQUFDLEtBQWE7UUFDM0IsSUFBSSxDQUFDLFlBQVksR0FBRyxLQUFLLENBQUM7UUFDMUIsSUFBSSxDQUFDLGlCQUFpQixDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztJQUNyQyxDQUFDOzs7O0lBQ0QsSUFBSSxXQUFXO1FBQ2IsT0FBTyxJQUFJLENBQUMsWUFBWSxDQUFDO0lBQzNCLENBQUM7Ozs7O0lBT0QsSUFDSSxLQUFLLENBQUMsS0FBYTtRQUNyQixJQUFJLENBQUMsTUFBTSxHQUFHLEtBQUssQ0FBQztRQUNwQixJQUFJLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztJQUMvQixDQUFDOzs7O0lBQ0QsSUFBSSxLQUFLO1FBQ1AsT0FBTyxJQUFJLENBQUMsTUFBTSxDQUFDO0lBQ3JCLENBQUM7Ozs7SUFPRCxJQUFJLElBQUk7UUFDTixJQUFJLENBQUMsSUFBSSxDQUFDLFdBQVc7WUFBRSxPQUFPLFNBQVMsQ0FBQztRQUN4QyxJQUFJLElBQUksQ0FBQyxXQUFXLEtBQUssSUFBSSxDQUFDLEdBQUc7WUFBRSxPQUFPLFdBQVcsSUFBSSxDQUFDLEtBQUssRUFBRSxDQUFDOztZQUM3RCxPQUFPLEVBQUUsQ0FBQztJQUNqQixDQUFDOzs7OztJQUVELElBQUksQ0FBQyxHQUFXO1FBQ2QsSUFBSSxDQUFDLFdBQVcsR0FBRyxHQUFHLENBQUM7UUFDdkIsUUFBUSxJQUFJLENBQUMsS0FBSyxFQUFFO1lBQ2xCLEtBQUssRUFBRTtnQkFDTCxJQUFJLENBQUMsS0FBSyxHQUFHLEtBQUssQ0FBQztnQkFDbkIsTUFBTTtZQUNSLEtBQUssS0FBSztnQkFDUixJQUFJLENBQUMsS0FBSyxHQUFHLE1BQU0sQ0FBQztnQkFDcEIsSUFBSSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUM7Z0JBQzlCLE1BQU07WUFDUixLQUFLLE1BQU07Z0JBQ1QsSUFBSSxDQUFDLEtBQUssR0FBRyxFQUFFLENBQUM7Z0JBQ2hCLElBQUksQ0FBQyxXQUFXLEdBQUcsRUFBRSxDQUFDO2dCQUN0QixNQUFNO1NBQ1Q7SUFDSCxDQUFDOzs7WUF6REYsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxxQkFBcUI7Z0JBQy9CLDRHQUErQzthQUNoRDs7OzBCQUtFLEtBQUs7Z0NBU0wsTUFBTTtrQkFFTixLQUFLO29CQUdMLEtBQUs7MEJBU0wsTUFBTTt3QkFFTixLQUFLOzs7Ozs7O0lBNUJOLHdDQUF1Qjs7Ozs7SUFDdkIsOENBQTZCOztJQVc3QixtREFBa0U7O0lBRWxFLHFDQUNZOztJQVdaLDZDQUE0RDs7SUFFNUQsMkNBQ2tCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBFdmVudEVtaXR0ZXIsIElucHV0LCBPdXRwdXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLXNvcnQtb3JkZXItaWNvbicsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL3NvcnQtb3JkZXItaWNvbi5jb21wb25lbnQuaHRtbCcsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBTb3J0T3JkZXJJY29uQ29tcG9uZW50IHtcclxuICBwcml2YXRlIF9vcmRlcjogc3RyaW5nO1xyXG4gIHByaXZhdGUgX3NlbGVjdGVkS2V5OiBzdHJpbmc7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgc2V0IHNlbGVjdGVkS2V5KHZhbHVlOiBzdHJpbmcpIHtcclxuICAgIHRoaXMuX3NlbGVjdGVkS2V5ID0gdmFsdWU7XHJcbiAgICB0aGlzLnNlbGVjdGVkS2V5Q2hhbmdlLmVtaXQodmFsdWUpO1xyXG4gIH1cclxuICBnZXQgc2VsZWN0ZWRLZXkoKTogc3RyaW5nIHtcclxuICAgIHJldHVybiB0aGlzLl9zZWxlY3RlZEtleTtcclxuICB9XHJcblxyXG4gIEBPdXRwdXQoKSByZWFkb25seSBzZWxlY3RlZEtleUNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8c3RyaW5nPigpO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGtleTogc3RyaW5nO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIHNldCBvcmRlcih2YWx1ZTogc3RyaW5nKSB7XHJcbiAgICB0aGlzLl9vcmRlciA9IHZhbHVlO1xyXG4gICAgdGhpcy5vcmRlckNoYW5nZS5lbWl0KHZhbHVlKTtcclxuICB9XHJcbiAgZ2V0IG9yZGVyKCk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gdGhpcy5fb3JkZXI7XHJcbiAgfVxyXG5cclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgb3JkZXJDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPHN0cmluZz4oKTtcclxuXHJcbiAgQElucHV0KClcclxuICBpY29uQ2xhc3M6IHN0cmluZztcclxuXHJcbiAgZ2V0IGljb24oKTogc3RyaW5nIHtcclxuICAgIGlmICghdGhpcy5zZWxlY3RlZEtleSkgcmV0dXJuICdmYS1zb3J0JztcclxuICAgIGlmICh0aGlzLnNlbGVjdGVkS2V5ID09PSB0aGlzLmtleSkgcmV0dXJuIGBmYS1zb3J0LSR7dGhpcy5vcmRlcn1gO1xyXG4gICAgZWxzZSByZXR1cm4gJyc7XHJcbiAgfVxyXG5cclxuICBzb3J0KGtleTogc3RyaW5nKSB7XHJcbiAgICB0aGlzLnNlbGVjdGVkS2V5ID0ga2V5O1xyXG4gICAgc3dpdGNoICh0aGlzLm9yZGVyKSB7XHJcbiAgICAgIGNhc2UgJyc6XHJcbiAgICAgICAgdGhpcy5vcmRlciA9ICdhc2MnO1xyXG4gICAgICAgIGJyZWFrO1xyXG4gICAgICBjYXNlICdhc2MnOlxyXG4gICAgICAgIHRoaXMub3JkZXIgPSAnZGVzYyc7XHJcbiAgICAgICAgdGhpcy5vcmRlckNoYW5nZS5lbWl0KCdkZXNjJyk7XHJcbiAgICAgICAgYnJlYWs7XHJcbiAgICAgIGNhc2UgJ2Rlc2MnOlxyXG4gICAgICAgIHRoaXMub3JkZXIgPSAnJztcclxuICAgICAgICB0aGlzLnNlbGVjdGVkS2V5ID0gJyc7XHJcbiAgICAgICAgYnJlYWs7XHJcbiAgICB9XHJcbiAgfVxyXG59XHJcbiJdfQ==