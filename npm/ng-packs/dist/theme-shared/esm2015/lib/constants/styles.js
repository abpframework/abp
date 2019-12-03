/**
 * @fileoverview added by tsickle
 * Generated from: lib/constants/styles.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
export default `
.is-invalid .form-control {
  border-color: #dc3545;
  border-style: solid !important;
}

.is-invalid .invalid-feedback,
.is-invalid + * .invalid-feedback {
  display: block;
}

.data-tables-filter {
  text-align: right;
}

.pointer {
  cursor: pointer;
}

.navbar .dropdown-submenu a::after {
  transform: rotate(-90deg);
  position: absolute;
  right: 16px;
  top: 18px;
}

.navbar .dropdown-menu {
  min-width: 215px;
}

.ui-table-scrollable-body::-webkit-scrollbar {
  height: 5px !important;
}

.ui-table-scrollable-body::-webkit-scrollbar-track {
  background: #ddd;
}

.ui-table-scrollable-body::-webkit-scrollbar-thumb {
  background: #8a8686;
}

.modal.show {
  display: block !important;
}

.modal-backdrop {
  position: fixed;
  top: 0;
  left: 0;
  width: calc(100% - 7px);
  height: 100%;
  background-color: rgba(0, 0, 0, 0.6);
  z-index: 1040;
}

.modal::-webkit-scrollbar {
  width: 7px;
}

.modal::-webkit-scrollbar-track {
  background: #ddd;
}

.modal::-webkit-scrollbar-thumb {
  background: #8a8686;
}

.modal-dialog {
  z-index: 1050;
}

.abp-ellipsis-inline {
  display: inline-block;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.abp-ellipsis {
  overflow: hidden !important;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.abp-toast .ui-toast-message {
  box-sizing: border-box;
  border: 2px solid transparent;
  border-radius: 4px;
  color: #1b1d29;
}

.abp-toast .ui-toast-message-content {
  padding: 10px;
}

.abp-toast .ui-toast-message-content .ui-toast-icon {
  top: 0;
  left: 0;
  padding: 10px;
}

.abp-toast .ui-toast-summary {
  margin: 0;
  font-weight: 700;
}

body abp-toast .ui-toast .ui-toast-message.ui-toast-message-error {
  border: 2px solid #ba1659;
  background-color: #f4f4f7;
}

body abp-toast .ui-toast .ui-toast-message.ui-toast-message-error .ui-toast-message-content .ui-toast-icon {
  color: #ba1659;
}

body abp-toast .ui-toast .ui-toast-message.ui-toast-message-warn {
  border: 2px solid #ed5d98;
  background-color: #f4f4f7;
}

body abp-toast .ui-toast .ui-toast-message.ui-toast-message-warn .ui-toast-message-content .ui-toast-icon {
  color: #ed5d98;
}

body abp-toast .ui-toast .ui-toast-message.ui-toast-message-success {
  border: 2px solid #1c9174;
  background-color: #f4f4f7;
}

body abp-toast .ui-toast .ui-toast-message.ui-toast-message-success .ui-toast-message-content .ui-toast-icon {
  color: #1c9174;
}

body abp-toast .ui-toast .ui-toast-message.ui-toast-message-info {
  border: 2px solid #fccb31;
  background-color: #f4f4f7;
}

body abp-toast .ui-toast .ui-toast-message.ui-toast-message-info .ui-toast-message-content .ui-toast-icon {
  color: #fccb31;
}

.abp-confirm .ui-toast-message {
  box-sizing: border-box;
  padding: 0px;
  border:0 none;
  border-radius: 4px;
  background-color: transparent !important;
  font-family: "Poppins", sans-serif;
  text-align: center;
}

.abp-confirm .ui-toast-message-content {
  padding: 0px;
}

.abp-confirm .abp-confirm-icon {
  margin: 32px 50px 5px !important;
  color: #f8bb86 !important;
  font-size: 52px !important;
}

.abp-confirm .ui-toast-close-icon {
  display: none !important;
}

.abp-confirm .abp-confirm-summary {
  display: block !important;
  margin-bottom: 13px !important;
  padding: 13px 16px 0px !important;
  font-weight: 600 !important;
  font-size: 18px !important;
}

.abp-confirm .abp-confirm-body {
  display: inline-block !important;
  padding: 0px 10px !important;
}

.abp-confirm .abp-confirm-footer {
  display: block;
  margin-top: 30px;
  padding: 16px;
  text-align: right;
}

.abp-confirm .abp-confirm-footer .btn {
  margin-left: 10px !important;
}

.ui-widget-overlay {
  z-index: 1000;
}

.color-white {
  color: #FFF !important;
}

.custom-checkbox > label {
  cursor: pointer;
}

/* <animations */

.fade-in-top {
  animation: fadeInTop 0.2s ease-in-out;
}

.fade-out-top {
  animation: fadeOutTop 0.2s ease-in-out;
}

.abp-collapsed-height {
  -moz-transition: max-height linear 0.35s;
  -ms-transition: max-height linear 0.35s;
  -o-transition: max-height linear 0.35s;
  -webkit-transition: max-height linear 0.35s;
  overflow:hidden;
  transition:max-height 0.35s linear;
  height:auto;
  max-height: 0;
}

.abp-mh-25 {
  max-height: 25vh;
}

.abp-mh-50 {
  transition:max-height 0.65s linear;
  max-height: 50vh;
}

.abp-mh-75 {
  transition:max-height 0.85s linear;
  max-height: 75vh;
}

.abp-mh-100 {
  transition:max-height 1s linear;
  max-height: 100vh;
}

@keyframes fadeInTop {
  from {
    transform: translateY(-5px);
    opacity: 0;
  }

  to {
    transform: translateY(0px);
    opacity: 1;
  }
}

@keyframes fadeOutTop {
  to {
    transform: translateY(-5px);
    opacity: 0;
  }
}

/* </animations */

`;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic3R5bGVzLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29uc3RhbnRzL3N0eWxlcy50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLGVBQWU7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7OztDQXdRZCxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiZXhwb3J0IGRlZmF1bHQgYFxuLmlzLWludmFsaWQgLmZvcm0tY29udHJvbCB7XG4gIGJvcmRlci1jb2xvcjogI2RjMzU0NTtcbiAgYm9yZGVyLXN0eWxlOiBzb2xpZCAhaW1wb3J0YW50O1xufVxuXG4uaXMtaW52YWxpZCAuaW52YWxpZC1mZWVkYmFjayxcbi5pcy1pbnZhbGlkICsgKiAuaW52YWxpZC1mZWVkYmFjayB7XG4gIGRpc3BsYXk6IGJsb2NrO1xufVxuXG4uZGF0YS10YWJsZXMtZmlsdGVyIHtcbiAgdGV4dC1hbGlnbjogcmlnaHQ7XG59XG5cbi5wb2ludGVyIHtcbiAgY3Vyc29yOiBwb2ludGVyO1xufVxuXG4ubmF2YmFyIC5kcm9wZG93bi1zdWJtZW51IGE6OmFmdGVyIHtcbiAgdHJhbnNmb3JtOiByb3RhdGUoLTkwZGVnKTtcbiAgcG9zaXRpb246IGFic29sdXRlO1xuICByaWdodDogMTZweDtcbiAgdG9wOiAxOHB4O1xufVxuXG4ubmF2YmFyIC5kcm9wZG93bi1tZW51IHtcbiAgbWluLXdpZHRoOiAyMTVweDtcbn1cblxuLnVpLXRhYmxlLXNjcm9sbGFibGUtYm9keTo6LXdlYmtpdC1zY3JvbGxiYXIge1xuICBoZWlnaHQ6IDVweCAhaW1wb3J0YW50O1xufVxuXG4udWktdGFibGUtc2Nyb2xsYWJsZS1ib2R5Ojotd2Via2l0LXNjcm9sbGJhci10cmFjayB7XG4gIGJhY2tncm91bmQ6ICNkZGQ7XG59XG5cbi51aS10YWJsZS1zY3JvbGxhYmxlLWJvZHk6Oi13ZWJraXQtc2Nyb2xsYmFyLXRodW1iIHtcbiAgYmFja2dyb3VuZDogIzhhODY4Njtcbn1cblxuLm1vZGFsLnNob3cge1xuICBkaXNwbGF5OiBibG9jayAhaW1wb3J0YW50O1xufVxuXG4ubW9kYWwtYmFja2Ryb3Age1xuICBwb3NpdGlvbjogZml4ZWQ7XG4gIHRvcDogMDtcbiAgbGVmdDogMDtcbiAgd2lkdGg6IGNhbGMoMTAwJSAtIDdweCk7XG4gIGhlaWdodDogMTAwJTtcbiAgYmFja2dyb3VuZC1jb2xvcjogcmdiYSgwLCAwLCAwLCAwLjYpO1xuICB6LWluZGV4OiAxMDQwO1xufVxuXG4ubW9kYWw6Oi13ZWJraXQtc2Nyb2xsYmFyIHtcbiAgd2lkdGg6IDdweDtcbn1cblxuLm1vZGFsOjotd2Via2l0LXNjcm9sbGJhci10cmFjayB7XG4gIGJhY2tncm91bmQ6ICNkZGQ7XG59XG5cbi5tb2RhbDo6LXdlYmtpdC1zY3JvbGxiYXItdGh1bWIge1xuICBiYWNrZ3JvdW5kOiAjOGE4Njg2O1xufVxuXG4ubW9kYWwtZGlhbG9nIHtcbiAgei1pbmRleDogMTA1MDtcbn1cblxuLmFicC1lbGxpcHNpcy1pbmxpbmUge1xuICBkaXNwbGF5OiBpbmxpbmUtYmxvY2s7XG4gIG92ZXJmbG93OiBoaWRkZW47XG4gIHRleHQtb3ZlcmZsb3c6IGVsbGlwc2lzO1xuICB3aGl0ZS1zcGFjZTogbm93cmFwO1xufVxuXG4uYWJwLWVsbGlwc2lzIHtcbiAgb3ZlcmZsb3c6IGhpZGRlbiAhaW1wb3J0YW50O1xuICB0ZXh0LW92ZXJmbG93OiBlbGxpcHNpcztcbiAgd2hpdGUtc3BhY2U6IG5vd3JhcDtcbn1cblxuLmFicC10b2FzdCAudWktdG9hc3QtbWVzc2FnZSB7XG4gIGJveC1zaXppbmc6IGJvcmRlci1ib3g7XG4gIGJvcmRlcjogMnB4IHNvbGlkIHRyYW5zcGFyZW50O1xuICBib3JkZXItcmFkaXVzOiA0cHg7XG4gIGNvbG9yOiAjMWIxZDI5O1xufVxuXG4uYWJwLXRvYXN0IC51aS10b2FzdC1tZXNzYWdlLWNvbnRlbnQge1xuICBwYWRkaW5nOiAxMHB4O1xufVxuXG4uYWJwLXRvYXN0IC51aS10b2FzdC1tZXNzYWdlLWNvbnRlbnQgLnVpLXRvYXN0LWljb24ge1xuICB0b3A6IDA7XG4gIGxlZnQ6IDA7XG4gIHBhZGRpbmc6IDEwcHg7XG59XG5cbi5hYnAtdG9hc3QgLnVpLXRvYXN0LXN1bW1hcnkge1xuICBtYXJnaW46IDA7XG4gIGZvbnQtd2VpZ2h0OiA3MDA7XG59XG5cbmJvZHkgYWJwLXRvYXN0IC51aS10b2FzdCAudWktdG9hc3QtbWVzc2FnZS51aS10b2FzdC1tZXNzYWdlLWVycm9yIHtcbiAgYm9yZGVyOiAycHggc29saWQgI2JhMTY1OTtcbiAgYmFja2dyb3VuZC1jb2xvcjogI2Y0ZjRmNztcbn1cblxuYm9keSBhYnAtdG9hc3QgLnVpLXRvYXN0IC51aS10b2FzdC1tZXNzYWdlLnVpLXRvYXN0LW1lc3NhZ2UtZXJyb3IgLnVpLXRvYXN0LW1lc3NhZ2UtY29udGVudCAudWktdG9hc3QtaWNvbiB7XG4gIGNvbG9yOiAjYmExNjU5O1xufVxuXG5ib2R5IGFicC10b2FzdCAudWktdG9hc3QgLnVpLXRvYXN0LW1lc3NhZ2UudWktdG9hc3QtbWVzc2FnZS13YXJuIHtcbiAgYm9yZGVyOiAycHggc29saWQgI2VkNWQ5ODtcbiAgYmFja2dyb3VuZC1jb2xvcjogI2Y0ZjRmNztcbn1cblxuYm9keSBhYnAtdG9hc3QgLnVpLXRvYXN0IC51aS10b2FzdC1tZXNzYWdlLnVpLXRvYXN0LW1lc3NhZ2Utd2FybiAudWktdG9hc3QtbWVzc2FnZS1jb250ZW50IC51aS10b2FzdC1pY29uIHtcbiAgY29sb3I6ICNlZDVkOTg7XG59XG5cbmJvZHkgYWJwLXRvYXN0IC51aS10b2FzdCAudWktdG9hc3QtbWVzc2FnZS51aS10b2FzdC1tZXNzYWdlLXN1Y2Nlc3Mge1xuICBib3JkZXI6IDJweCBzb2xpZCAjMWM5MTc0O1xuICBiYWNrZ3JvdW5kLWNvbG9yOiAjZjRmNGY3O1xufVxuXG5ib2R5IGFicC10b2FzdCAudWktdG9hc3QgLnVpLXRvYXN0LW1lc3NhZ2UudWktdG9hc3QtbWVzc2FnZS1zdWNjZXNzIC51aS10b2FzdC1tZXNzYWdlLWNvbnRlbnQgLnVpLXRvYXN0LWljb24ge1xuICBjb2xvcjogIzFjOTE3NDtcbn1cblxuYm9keSBhYnAtdG9hc3QgLnVpLXRvYXN0IC51aS10b2FzdC1tZXNzYWdlLnVpLXRvYXN0LW1lc3NhZ2UtaW5mbyB7XG4gIGJvcmRlcjogMnB4IHNvbGlkICNmY2NiMzE7XG4gIGJhY2tncm91bmQtY29sb3I6ICNmNGY0Zjc7XG59XG5cbmJvZHkgYWJwLXRvYXN0IC51aS10b2FzdCAudWktdG9hc3QtbWVzc2FnZS51aS10b2FzdC1tZXNzYWdlLWluZm8gLnVpLXRvYXN0LW1lc3NhZ2UtY29udGVudCAudWktdG9hc3QtaWNvbiB7XG4gIGNvbG9yOiAjZmNjYjMxO1xufVxuXG4uYWJwLWNvbmZpcm0gLnVpLXRvYXN0LW1lc3NhZ2Uge1xuICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xuICBwYWRkaW5nOiAwcHg7XG4gIGJvcmRlcjowIG5vbmU7XG4gIGJvcmRlci1yYWRpdXM6IDRweDtcbiAgYmFja2dyb3VuZC1jb2xvcjogdHJhbnNwYXJlbnQgIWltcG9ydGFudDtcbiAgZm9udC1mYW1pbHk6IFwiUG9wcGluc1wiLCBzYW5zLXNlcmlmO1xuICB0ZXh0LWFsaWduOiBjZW50ZXI7XG59XG5cbi5hYnAtY29uZmlybSAudWktdG9hc3QtbWVzc2FnZS1jb250ZW50IHtcbiAgcGFkZGluZzogMHB4O1xufVxuXG4uYWJwLWNvbmZpcm0gLmFicC1jb25maXJtLWljb24ge1xuICBtYXJnaW46IDMycHggNTBweCA1cHggIWltcG9ydGFudDtcbiAgY29sb3I6ICNmOGJiODYgIWltcG9ydGFudDtcbiAgZm9udC1zaXplOiA1MnB4ICFpbXBvcnRhbnQ7XG59XG5cbi5hYnAtY29uZmlybSAudWktdG9hc3QtY2xvc2UtaWNvbiB7XG4gIGRpc3BsYXk6IG5vbmUgIWltcG9ydGFudDtcbn1cblxuLmFicC1jb25maXJtIC5hYnAtY29uZmlybS1zdW1tYXJ5IHtcbiAgZGlzcGxheTogYmxvY2sgIWltcG9ydGFudDtcbiAgbWFyZ2luLWJvdHRvbTogMTNweCAhaW1wb3J0YW50O1xuICBwYWRkaW5nOiAxM3B4IDE2cHggMHB4ICFpbXBvcnRhbnQ7XG4gIGZvbnQtd2VpZ2h0OiA2MDAgIWltcG9ydGFudDtcbiAgZm9udC1zaXplOiAxOHB4ICFpbXBvcnRhbnQ7XG59XG5cbi5hYnAtY29uZmlybSAuYWJwLWNvbmZpcm0tYm9keSB7XG4gIGRpc3BsYXk6IGlubGluZS1ibG9jayAhaW1wb3J0YW50O1xuICBwYWRkaW5nOiAwcHggMTBweCAhaW1wb3J0YW50O1xufVxuXG4uYWJwLWNvbmZpcm0gLmFicC1jb25maXJtLWZvb3RlciB7XG4gIGRpc3BsYXk6IGJsb2NrO1xuICBtYXJnaW4tdG9wOiAzMHB4O1xuICBwYWRkaW5nOiAxNnB4O1xuICB0ZXh0LWFsaWduOiByaWdodDtcbn1cblxuLmFicC1jb25maXJtIC5hYnAtY29uZmlybS1mb290ZXIgLmJ0biB7XG4gIG1hcmdpbi1sZWZ0OiAxMHB4ICFpbXBvcnRhbnQ7XG59XG5cbi51aS13aWRnZXQtb3ZlcmxheSB7XG4gIHotaW5kZXg6IDEwMDA7XG59XG5cbi5jb2xvci13aGl0ZSB7XG4gIGNvbG9yOiAjRkZGICFpbXBvcnRhbnQ7XG59XG5cbi5jdXN0b20tY2hlY2tib3ggPiBsYWJlbCB7XG4gIGN1cnNvcjogcG9pbnRlcjtcbn1cblxuLyogPGFuaW1hdGlvbnMgKi9cblxuLmZhZGUtaW4tdG9wIHtcbiAgYW5pbWF0aW9uOiBmYWRlSW5Ub3AgMC4ycyBlYXNlLWluLW91dDtcbn1cblxuLmZhZGUtb3V0LXRvcCB7XG4gIGFuaW1hdGlvbjogZmFkZU91dFRvcCAwLjJzIGVhc2UtaW4tb3V0O1xufVxuXG4uYWJwLWNvbGxhcHNlZC1oZWlnaHQge1xuICAtbW96LXRyYW5zaXRpb246IG1heC1oZWlnaHQgbGluZWFyIDAuMzVzO1xuICAtbXMtdHJhbnNpdGlvbjogbWF4LWhlaWdodCBsaW5lYXIgMC4zNXM7XG4gIC1vLXRyYW5zaXRpb246IG1heC1oZWlnaHQgbGluZWFyIDAuMzVzO1xuICAtd2Via2l0LXRyYW5zaXRpb246IG1heC1oZWlnaHQgbGluZWFyIDAuMzVzO1xuICBvdmVyZmxvdzpoaWRkZW47XG4gIHRyYW5zaXRpb246bWF4LWhlaWdodCAwLjM1cyBsaW5lYXI7XG4gIGhlaWdodDphdXRvO1xuICBtYXgtaGVpZ2h0OiAwO1xufVxuXG4uYWJwLW1oLTI1IHtcbiAgbWF4LWhlaWdodDogMjV2aDtcbn1cblxuLmFicC1taC01MCB7XG4gIHRyYW5zaXRpb246bWF4LWhlaWdodCAwLjY1cyBsaW5lYXI7XG4gIG1heC1oZWlnaHQ6IDUwdmg7XG59XG5cbi5hYnAtbWgtNzUge1xuICB0cmFuc2l0aW9uOm1heC1oZWlnaHQgMC44NXMgbGluZWFyO1xuICBtYXgtaGVpZ2h0OiA3NXZoO1xufVxuXG4uYWJwLW1oLTEwMCB7XG4gIHRyYW5zaXRpb246bWF4LWhlaWdodCAxcyBsaW5lYXI7XG4gIG1heC1oZWlnaHQ6IDEwMHZoO1xufVxuXG5Aa2V5ZnJhbWVzIGZhZGVJblRvcCB7XG4gIGZyb20ge1xuICAgIHRyYW5zZm9ybTogdHJhbnNsYXRlWSgtNXB4KTtcbiAgICBvcGFjaXR5OiAwO1xuICB9XG5cbiAgdG8ge1xuICAgIHRyYW5zZm9ybTogdHJhbnNsYXRlWSgwcHgpO1xuICAgIG9wYWNpdHk6IDE7XG4gIH1cbn1cblxuQGtleWZyYW1lcyBmYWRlT3V0VG9wIHtcbiAgdG8ge1xuICAgIHRyYW5zZm9ybTogdHJhbnNsYXRlWSgtNXB4KTtcbiAgICBvcGFjaXR5OiAwO1xuICB9XG59XG5cbi8qIDwvYW5pbWF0aW9ucyAqL1xuXG5gO1xuIl19