/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
export var ABP;
(function (ABP) {
    /**
     * @record
     */
    function Root() { }
    ABP.Root = Root;
    if (false) {
        /** @type {?} */
        Root.prototype.environment;
        /** @type {?} */
        Root.prototype.requirements;
    }
    /**
     * @record
     * @template T
     */
    function PagedItemsResponse() { }
    ABP.PagedItemsResponse = PagedItemsResponse;
    if (false) {
        /** @type {?} */
        PagedItemsResponse.prototype.items;
    }
    /**
     * @record
     */
    function PageQueryParams() { }
    ABP.PageQueryParams = PageQueryParams;
    if (false) {
        /** @type {?|undefined} */
        PageQueryParams.prototype.filter;
        /** @type {?|undefined} */
        PageQueryParams.prototype.sorting;
        /** @type {?|undefined} */
        PageQueryParams.prototype.skipCount;
        /** @type {?|undefined} */
        PageQueryParams.prototype.maxResultCount;
    }
    /**
     * @record
     */
    function Route() { }
    ABP.Route = Route;
    if (false) {
        /** @type {?|undefined} */
        Route.prototype.children;
        /** @type {?|undefined} */
        Route.prototype.invisible;
        /** @type {?|undefined} */
        Route.prototype.layout;
        /** @type {?} */
        Route.prototype.name;
        /** @type {?|undefined} */
        Route.prototype.order;
        /** @type {?|undefined} */
        Route.prototype.parentName;
        /** @type {?} */
        Route.prototype.path;
        /** @type {?|undefined} */
        Route.prototype.requiredPolicy;
        /** @type {?|undefined} */
        Route.prototype.iconClass;
    }
    /**
     * @record
     */
    function FullRoute() { }
    ABP.FullRoute = FullRoute;
    if (false) {
        /** @type {?|undefined} */
        FullRoute.prototype.url;
        /** @type {?|undefined} */
        FullRoute.prototype.wrapper;
    }
    /**
     * @record
     */
    function BasicItem() { }
    ABP.BasicItem = BasicItem;
    if (false) {
        /** @type {?} */
        BasicItem.prototype.id;
        /** @type {?} */
        BasicItem.prototype.name;
    }
    /**
     * @record
     * @template T
     */
    function Dictionary() { }
    ABP.Dictionary = Dictionary;
})(ABP || (ABP = {}));
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29tbW9uLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL21vZGVscy9jb21tb24udHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUdBLE1BQU0sS0FBVyxHQUFHLENBOENuQjtBQTlDRCxXQUFpQixHQUFHOzs7O0lBQ2xCLG1CQUdDOzs7O1FBRkMsMkJBQXlDOztRQUN6Qyw0QkFBa0M7Ozs7OztJQU9wQyxpQ0FFQzs7OztRQURDLG1DQUFXOzs7OztJQUdiLDhCQUtDOzs7O1FBSkMsaUNBQWdCOztRQUNoQixrQ0FBaUI7O1FBQ2pCLG9DQUFtQjs7UUFDbkIseUNBQXdCOzs7OztJQUcxQixvQkFVQzs7OztRQVRDLHlCQUFtQjs7UUFDbkIsMEJBQW9COztRQUNwQix1QkFBcUI7O1FBQ3JCLHFCQUFhOztRQUNiLHNCQUFlOztRQUNmLDJCQUFvQjs7UUFDcEIscUJBQWE7O1FBQ2IsK0JBQXdCOztRQUN4QiwwQkFBbUI7Ozs7O0lBR3JCLHdCQUdDOzs7O1FBRkMsd0JBQWE7O1FBQ2IsNEJBQWtCOzs7OztJQUdwQix3QkFHQzs7OztRQUZDLHVCQUFXOztRQUNYLHlCQUFhOzs7Ozs7SUFHZix5QkFFQzs7QUFDSCxDQUFDLEVBOUNnQixHQUFHLEtBQUgsR0FBRyxRQThDbkIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb25maWcgfSBmcm9tICcuL2NvbmZpZyc7XHJcbmltcG9ydCB7IGVMYXlvdXRUeXBlIH0gZnJvbSAnLi4vZW51bXMvY29tbW9uJztcclxuXHJcbmV4cG9ydCBuYW1lc3BhY2UgQUJQIHtcclxuICBleHBvcnQgaW50ZXJmYWNlIFJvb3Qge1xyXG4gICAgZW52aXJvbm1lbnQ6IFBhcnRpYWw8Q29uZmlnLkVudmlyb25tZW50PjtcclxuICAgIHJlcXVpcmVtZW50czogQ29uZmlnLlJlcXVpcmVtZW50cztcclxuICB9XHJcblxyXG4gIGV4cG9ydCB0eXBlIFBhZ2VkUmVzcG9uc2U8VD4gPSB7XHJcbiAgICB0b3RhbENvdW50OiBudW1iZXI7XHJcbiAgfSAmIFBhZ2VkSXRlbXNSZXNwb25zZTxUPjtcclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBQYWdlZEl0ZW1zUmVzcG9uc2U8VD4ge1xyXG4gICAgaXRlbXM6IFRbXTtcclxuICB9XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgUGFnZVF1ZXJ5UGFyYW1zIHtcclxuICAgIGZpbHRlcj86IHN0cmluZztcclxuICAgIHNvcnRpbmc/OiBzdHJpbmc7XHJcbiAgICBza2lwQ291bnQ/OiBudW1iZXI7XHJcbiAgICBtYXhSZXN1bHRDb3VudD86IG51bWJlcjtcclxuICB9XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgUm91dGUge1xyXG4gICAgY2hpbGRyZW4/OiBSb3V0ZVtdO1xyXG4gICAgaW52aXNpYmxlPzogYm9vbGVhbjtcclxuICAgIGxheW91dD86IGVMYXlvdXRUeXBlO1xyXG4gICAgbmFtZTogc3RyaW5nO1xyXG4gICAgb3JkZXI/OiBudW1iZXI7XHJcbiAgICBwYXJlbnROYW1lPzogc3RyaW5nO1xyXG4gICAgcGF0aDogc3RyaW5nO1xyXG4gICAgcmVxdWlyZWRQb2xpY3k/OiBzdHJpbmc7XHJcbiAgICBpY29uQ2xhc3M/OiBzdHJpbmc7XHJcbiAgfVxyXG5cclxuICBleHBvcnQgaW50ZXJmYWNlIEZ1bGxSb3V0ZSBleHRlbmRzIFJvdXRlIHtcclxuICAgIHVybD86IHN0cmluZztcclxuICAgIHdyYXBwZXI/OiBib29sZWFuO1xyXG4gIH1cclxuXHJcbiAgZXhwb3J0IGludGVyZmFjZSBCYXNpY0l0ZW0ge1xyXG4gICAgaWQ6IHN0cmluZztcclxuICAgIG5hbWU6IHN0cmluZztcclxuICB9XHJcblxyXG4gIGV4cG9ydCBpbnRlcmZhY2UgRGljdGlvbmFyeTxUID0gYW55PiB7XHJcbiAgICBba2V5OiBzdHJpbmddOiBUO1xyXG4gIH1cclxufVxyXG4iXX0=