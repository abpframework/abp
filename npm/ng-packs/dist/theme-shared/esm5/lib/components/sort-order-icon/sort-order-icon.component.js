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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic29ydC1vcmRlci1pY29uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc29ydC1vcmRlci1pY29uL3NvcnQtb3JkZXItaWNvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFdkU7SUFBQTtRQWlCcUIsc0JBQWlCLEdBQUcsSUFBSSxZQUFZLEVBQVUsQ0FBQztRQWMvQyxnQkFBVyxHQUFHLElBQUksWUFBWSxFQUFVLENBQUM7SUEyQjlELENBQUM7SUFsREMsc0JBQ0ksK0NBQVc7Ozs7UUFJZjtZQUNFLE9BQU8sSUFBSSxDQUFDLFlBQVksQ0FBQztRQUMzQixDQUFDOzs7OztRQVBELFVBQ2dCLEtBQWE7WUFDM0IsSUFBSSxDQUFDLFlBQVksR0FBRyxLQUFLLENBQUM7WUFDMUIsSUFBSSxDQUFDLGlCQUFpQixDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUNyQyxDQUFDOzs7T0FBQTtJQVVELHNCQUNJLHlDQUFLOzs7O1FBSVQ7WUFDRSxPQUFPLElBQUksQ0FBQyxNQUFNLENBQUM7UUFDckIsQ0FBQzs7Ozs7UUFQRCxVQUNVLEtBQWE7WUFDckIsSUFBSSxDQUFDLE1BQU0sR0FBRyxLQUFLLENBQUM7WUFDcEIsSUFBSSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7UUFDL0IsQ0FBQzs7O09BQUE7SUFVRCxzQkFBSSx3Q0FBSTs7OztRQUFSO1lBQ0UsSUFBSSxDQUFDLElBQUksQ0FBQyxXQUFXO2dCQUFFLE9BQU8sU0FBUyxDQUFDO1lBQ3hDLElBQUksSUFBSSxDQUFDLFdBQVcsS0FBSyxJQUFJLENBQUMsR0FBRztnQkFBRSxPQUFPLGFBQVcsSUFBSSxDQUFDLEtBQU8sQ0FBQzs7Z0JBQzdELE9BQU8sRUFBRSxDQUFDO1FBQ2pCLENBQUM7OztPQUFBOzs7OztJQUVELHFDQUFJOzs7O0lBQUosVUFBSyxHQUFXO1FBQ2QsSUFBSSxDQUFDLFdBQVcsR0FBRyxHQUFHLENBQUM7UUFDdkIsUUFBUSxJQUFJLENBQUMsS0FBSyxFQUFFO1lBQ2xCLEtBQUssRUFBRTtnQkFDTCxJQUFJLENBQUMsS0FBSyxHQUFHLEtBQUssQ0FBQztnQkFDbkIsTUFBTTtZQUNSLEtBQUssS0FBSztnQkFDUixJQUFJLENBQUMsS0FBSyxHQUFHLE1BQU0sQ0FBQztnQkFDcEIsSUFBSSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUM7Z0JBQzlCLE1BQU07WUFDUixLQUFLLE1BQU07Z0JBQ1QsSUFBSSxDQUFDLEtBQUssR0FBRyxFQUFFLENBQUM7Z0JBQ2hCLElBQUksQ0FBQyxXQUFXLEdBQUcsRUFBRSxDQUFDO2dCQUN0QixNQUFNO1NBQ1Q7SUFDSCxDQUFDOztnQkF6REYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxxQkFBcUI7b0JBQy9CLHNHQUErQztpQkFDaEQ7Ozs4QkFLRSxLQUFLO29DQVNMLE1BQU07c0JBRU4sS0FBSzt3QkFHTCxLQUFLOzhCQVNMLE1BQU07NEJBRU4sS0FBSzs7SUF5QlIsNkJBQUM7Q0FBQSxBQTFERCxJQTBEQztTQXREWSxzQkFBc0I7Ozs7OztJQUNqQyx3Q0FBdUI7Ozs7O0lBQ3ZCLDhDQUE2Qjs7SUFXN0IsbURBQWtFOztJQUVsRSxxQ0FDWTs7SUFXWiw2Q0FBNEQ7O0lBRTVELDJDQUNrQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgRXZlbnRFbWl0dGVyLCBJbnB1dCwgT3V0cHV0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1zb3J0LW9yZGVyLWljb24nLFxuICB0ZW1wbGF0ZVVybDogJy4vc29ydC1vcmRlci1pY29uLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgU29ydE9yZGVySWNvbkNvbXBvbmVudCB7XG4gIHByaXZhdGUgX29yZGVyOiBzdHJpbmc7XG4gIHByaXZhdGUgX3NlbGVjdGVkS2V5OiBzdHJpbmc7XG5cbiAgQElucHV0KClcbiAgc2V0IHNlbGVjdGVkS2V5KHZhbHVlOiBzdHJpbmcpIHtcbiAgICB0aGlzLl9zZWxlY3RlZEtleSA9IHZhbHVlO1xuICAgIHRoaXMuc2VsZWN0ZWRLZXlDaGFuZ2UuZW1pdCh2YWx1ZSk7XG4gIH1cbiAgZ2V0IHNlbGVjdGVkS2V5KCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHRoaXMuX3NlbGVjdGVkS2V5O1xuICB9XG5cbiAgQE91dHB1dCgpIHJlYWRvbmx5IHNlbGVjdGVkS2V5Q2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxzdHJpbmc+KCk7XG5cbiAgQElucHV0KClcbiAga2V5OiBzdHJpbmc7XG5cbiAgQElucHV0KClcbiAgc2V0IG9yZGVyKHZhbHVlOiBzdHJpbmcpIHtcbiAgICB0aGlzLl9vcmRlciA9IHZhbHVlO1xuICAgIHRoaXMub3JkZXJDaGFuZ2UuZW1pdCh2YWx1ZSk7XG4gIH1cbiAgZ2V0IG9yZGVyKCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHRoaXMuX29yZGVyO1xuICB9XG5cbiAgQE91dHB1dCgpIHJlYWRvbmx5IG9yZGVyQ2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxzdHJpbmc+KCk7XG5cbiAgQElucHV0KClcbiAgaWNvbkNsYXNzOiBzdHJpbmc7XG5cbiAgZ2V0IGljb24oKTogc3RyaW5nIHtcbiAgICBpZiAoIXRoaXMuc2VsZWN0ZWRLZXkpIHJldHVybiAnZmEtc29ydCc7XG4gICAgaWYgKHRoaXMuc2VsZWN0ZWRLZXkgPT09IHRoaXMua2V5KSByZXR1cm4gYGZhLXNvcnQtJHt0aGlzLm9yZGVyfWA7XG4gICAgZWxzZSByZXR1cm4gJyc7XG4gIH1cblxuICBzb3J0KGtleTogc3RyaW5nKSB7XG4gICAgdGhpcy5zZWxlY3RlZEtleSA9IGtleTtcbiAgICBzd2l0Y2ggKHRoaXMub3JkZXIpIHtcbiAgICAgIGNhc2UgJyc6XG4gICAgICAgIHRoaXMub3JkZXIgPSAnYXNjJztcbiAgICAgICAgYnJlYWs7XG4gICAgICBjYXNlICdhc2MnOlxuICAgICAgICB0aGlzLm9yZGVyID0gJ2Rlc2MnO1xuICAgICAgICB0aGlzLm9yZGVyQ2hhbmdlLmVtaXQoJ2Rlc2MnKTtcbiAgICAgICAgYnJlYWs7XG4gICAgICBjYXNlICdkZXNjJzpcbiAgICAgICAgdGhpcy5vcmRlciA9ICcnO1xuICAgICAgICB0aGlzLnNlbGVjdGVkS2V5ID0gJyc7XG4gICAgICAgIGJyZWFrO1xuICAgIH1cbiAgfVxufVxuIl19