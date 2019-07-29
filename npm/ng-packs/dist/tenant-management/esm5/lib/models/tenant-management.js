/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
export var TenantManagement;
(function (TenantManagement) {
    /**
     * @record
     */
    function State() { }
    TenantManagement.State = State;
    if (false) {
        /** @type {?} */
        State.prototype.result;
        /** @type {?} */
        State.prototype.selectedItem;
    }
    /**
     * @record
     */
    function Item() { }
    TenantManagement.Item = Item;
    if (false) {
        /** @type {?} */
        Item.prototype.id;
        /** @type {?} */
        Item.prototype.name;
    }
    /**
     * @record
     */
    function AddRequest() { }
    TenantManagement.AddRequest = AddRequest;
    if (false) {
        /** @type {?} */
        AddRequest.prototype.name;
    }
    /**
     * @record
     */
    function UpdateRequest() { }
    TenantManagement.UpdateRequest = UpdateRequest;
    if (false) {
        /** @type {?} */
        UpdateRequest.prototype.id;
    }
    /**
     * @record
     */
    function DefaultConnectionStringRequest() { }
    TenantManagement.DefaultConnectionStringRequest = DefaultConnectionStringRequest;
    if (false) {
        /** @type {?} */
        DefaultConnectionStringRequest.prototype.id;
        /** @type {?} */
        DefaultConnectionStringRequest.prototype.defaultConnectionString;
    }
})(TenantManagement || (TenantManagement = {}));
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL21vZGVscy90ZW5hbnQtbWFuYWdlbWVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBRUEsTUFBTSxLQUFXLGdCQUFnQixDQXlCaEM7QUF6QkQsV0FBaUIsZ0JBQWdCOzs7O0lBQy9CLG9CQUdDOzs7O1FBRkMsdUJBQWlCOztRQUNqQiw2QkFBbUI7Ozs7O0lBS3JCLG1CQUdDOzs7O1FBRkMsa0JBQVc7O1FBQ1gsb0JBQWE7Ozs7O0lBR2YseUJBRUM7Ozs7UUFEQywwQkFBYTs7Ozs7SUFHZiw0QkFFQzs7OztRQURDLDJCQUFXOzs7OztJQUdiLDZDQUdDOzs7O1FBRkMsNENBQVc7O1FBQ1gsaUVBQWdDOztBQUVwQyxDQUFDLEVBekJnQixnQkFBZ0IsS0FBaEIsZ0JBQWdCLFFBeUJoQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5cbmV4cG9ydCBuYW1lc3BhY2UgVGVuYW50TWFuYWdlbWVudCB7XG4gIGV4cG9ydCBpbnRlcmZhY2UgU3RhdGUge1xuICAgIHJlc3VsdDogUmVzcG9uc2U7XG4gICAgc2VsZWN0ZWRJdGVtOiBJdGVtO1xuICB9XG5cbiAgZXhwb3J0IHR5cGUgUmVzcG9uc2UgPSBBQlAuUGFnZWRSZXNwb25zZTxJdGVtPjtcblxuICBleHBvcnQgaW50ZXJmYWNlIEl0ZW0ge1xuICAgIGlkOiBzdHJpbmc7XG4gICAgbmFtZTogc3RyaW5nO1xuICB9XG5cbiAgZXhwb3J0IGludGVyZmFjZSBBZGRSZXF1ZXN0IHtcbiAgICBuYW1lOiBzdHJpbmc7XG4gIH1cblxuICBleHBvcnQgaW50ZXJmYWNlIFVwZGF0ZVJlcXVlc3QgZXh0ZW5kcyBBZGRSZXF1ZXN0IHtcbiAgICBpZDogc3RyaW5nO1xuICB9XG5cbiAgZXhwb3J0IGludGVyZmFjZSBEZWZhdWx0Q29ubmVjdGlvblN0cmluZ1JlcXVlc3Qge1xuICAgIGlkOiBzdHJpbmc7XG4gICAgZGVmYXVsdENvbm5lY3Rpb25TdHJpbmc6IHN0cmluZztcbiAgfVxufVxuIl19