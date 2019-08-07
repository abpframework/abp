/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
})(ABP || (ABP = {}));
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29tbW9uLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL21vZGVscy9jb21tb24udHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUdBLE1BQU0sS0FBVyxHQUFHLENBeUNuQjtBQXpDRCxXQUFpQixHQUFHOzs7O0lBQ2xCLG1CQUdDOzs7O1FBRkMsMkJBQXlDOztRQUN6Qyw0QkFBa0M7Ozs7OztJQU9wQyxpQ0FFQzs7OztRQURDLG1DQUFXOzs7OztJQUdiLDhCQUtDOzs7O1FBSkMsaUNBQWdCOztRQUNoQixrQ0FBaUI7O1FBQ2pCLG9DQUFtQjs7UUFDbkIseUNBQXdCOzs7OztJQUcxQixvQkFTQzs7OztRQVJDLHlCQUFtQjs7UUFDbkIsMEJBQW9COztRQUNwQix1QkFBcUI7O1FBQ3JCLHFCQUFhOztRQUNiLHNCQUFlOztRQUNmLDJCQUFvQjs7UUFDcEIscUJBQWE7O1FBQ2IsK0JBQXdCOzs7OztJQUcxQix3QkFHQzs7OztRQUZDLHdCQUFhOztRQUNiLDRCQUFrQjs7Ozs7SUFHcEIsd0JBR0M7Ozs7UUFGQyx1QkFBVzs7UUFDWCx5QkFBYTs7QUFFakIsQ0FBQyxFQXpDZ0IsR0FBRyxLQUFILEdBQUcsUUF5Q25CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29uZmlnIH0gZnJvbSAnLi9jb25maWcnO1xuaW1wb3J0IHsgZUxheW91dFR5cGUgfSBmcm9tICcuLi9lbnVtcyc7XG5cbmV4cG9ydCBuYW1lc3BhY2UgQUJQIHtcbiAgZXhwb3J0IGludGVyZmFjZSBSb290IHtcbiAgICBlbnZpcm9ubWVudDogUGFydGlhbDxDb25maWcuRW52aXJvbm1lbnQ+O1xuICAgIHJlcXVpcmVtZW50czogQ29uZmlnLlJlcXVpcmVtZW50cztcbiAgfVxuXG4gIGV4cG9ydCB0eXBlIFBhZ2VkUmVzcG9uc2U8VD4gPSB7XG4gICAgdG90YWxDb3VudDogbnVtYmVyO1xuICB9ICYgUGFnZWRJdGVtc1Jlc3BvbnNlPFQ+O1xuXG4gIGV4cG9ydCBpbnRlcmZhY2UgUGFnZWRJdGVtc1Jlc3BvbnNlPFQ+IHtcbiAgICBpdGVtczogVFtdO1xuICB9XG5cbiAgZXhwb3J0IGludGVyZmFjZSBQYWdlUXVlcnlQYXJhbXMge1xuICAgIGZpbHRlcj86IHN0cmluZztcbiAgICBzb3J0aW5nPzogc3RyaW5nO1xuICAgIHNraXBDb3VudD86IG51bWJlcjtcbiAgICBtYXhSZXN1bHRDb3VudD86IG51bWJlcjtcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgUm91dGUge1xuICAgIGNoaWxkcmVuPzogUm91dGVbXTtcbiAgICBpbnZpc2libGU/OiBib29sZWFuO1xuICAgIGxheW91dD86IGVMYXlvdXRUeXBlO1xuICAgIG5hbWU6IHN0cmluZztcbiAgICBvcmRlcj86IG51bWJlcjtcbiAgICBwYXJlbnROYW1lPzogc3RyaW5nO1xuICAgIHBhdGg6IHN0cmluZztcbiAgICByZXF1aXJlZFBvbGljeT86IHN0cmluZztcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgRnVsbFJvdXRlIGV4dGVuZHMgUm91dGUge1xuICAgIHVybD86IHN0cmluZztcbiAgICB3cmFwcGVyPzogYm9vbGVhbjtcbiAgfVxuXG4gIGV4cG9ydCBpbnRlcmZhY2UgQmFzaWNJdGVtIHtcbiAgICBpZDogc3RyaW5nO1xuICAgIG5hbWU6IHN0cmluZztcbiAgfVxufVxuIl19