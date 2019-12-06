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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic3R5bGVzLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29uc3RhbnRzL3N0eWxlcy50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLGVBQWU7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7OztDQXdRZCxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiZXhwb3J0IGRlZmF1bHQgYFxyXG4uaXMtaW52YWxpZCAuZm9ybS1jb250cm9sIHtcclxuICBib3JkZXItY29sb3I6ICNkYzM1NDU7XHJcbiAgYm9yZGVyLXN0eWxlOiBzb2xpZCAhaW1wb3J0YW50O1xyXG59XHJcblxyXG4uaXMtaW52YWxpZCAuaW52YWxpZC1mZWVkYmFjayxcclxuLmlzLWludmFsaWQgKyAqIC5pbnZhbGlkLWZlZWRiYWNrIHtcclxuICBkaXNwbGF5OiBibG9jaztcclxufVxyXG5cclxuLmRhdGEtdGFibGVzLWZpbHRlciB7XHJcbiAgdGV4dC1hbGlnbjogcmlnaHQ7XHJcbn1cclxuXHJcbi5wb2ludGVyIHtcclxuICBjdXJzb3I6IHBvaW50ZXI7XHJcbn1cclxuXHJcbi5uYXZiYXIgLmRyb3Bkb3duLXN1Ym1lbnUgYTo6YWZ0ZXIge1xyXG4gIHRyYW5zZm9ybTogcm90YXRlKC05MGRlZyk7XHJcbiAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gIHJpZ2h0OiAxNnB4O1xyXG4gIHRvcDogMThweDtcclxufVxyXG5cclxuLm5hdmJhciAuZHJvcGRvd24tbWVudSB7XHJcbiAgbWluLXdpZHRoOiAyMTVweDtcclxufVxyXG5cclxuLnVpLXRhYmxlLXNjcm9sbGFibGUtYm9keTo6LXdlYmtpdC1zY3JvbGxiYXIge1xyXG4gIGhlaWdodDogNXB4ICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi51aS10YWJsZS1zY3JvbGxhYmxlLWJvZHk6Oi13ZWJraXQtc2Nyb2xsYmFyLXRyYWNrIHtcclxuICBiYWNrZ3JvdW5kOiAjZGRkO1xyXG59XHJcblxyXG4udWktdGFibGUtc2Nyb2xsYWJsZS1ib2R5Ojotd2Via2l0LXNjcm9sbGJhci10aHVtYiB7XHJcbiAgYmFja2dyb3VuZDogIzhhODY4NjtcclxufVxyXG5cclxuLm1vZGFsLnNob3cge1xyXG4gIGRpc3BsYXk6IGJsb2NrICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi5tb2RhbC1iYWNrZHJvcCB7XHJcbiAgcG9zaXRpb246IGZpeGVkO1xyXG4gIHRvcDogMDtcclxuICBsZWZ0OiAwO1xyXG4gIHdpZHRoOiBjYWxjKDEwMCUgLSA3cHgpO1xyXG4gIGhlaWdodDogMTAwJTtcclxuICBiYWNrZ3JvdW5kLWNvbG9yOiByZ2JhKDAsIDAsIDAsIDAuNik7XHJcbiAgei1pbmRleDogMTA0MDtcclxufVxyXG5cclxuLm1vZGFsOjotd2Via2l0LXNjcm9sbGJhciB7XHJcbiAgd2lkdGg6IDdweDtcclxufVxyXG5cclxuLm1vZGFsOjotd2Via2l0LXNjcm9sbGJhci10cmFjayB7XHJcbiAgYmFja2dyb3VuZDogI2RkZDtcclxufVxyXG5cclxuLm1vZGFsOjotd2Via2l0LXNjcm9sbGJhci10aHVtYiB7XHJcbiAgYmFja2dyb3VuZDogIzhhODY4NjtcclxufVxyXG5cclxuLm1vZGFsLWRpYWxvZyB7XHJcbiAgei1pbmRleDogMTA1MDtcclxufVxyXG5cclxuLmFicC1lbGxpcHNpcy1pbmxpbmUge1xyXG4gIGRpc3BsYXk6IGlubGluZS1ibG9jaztcclxuICBvdmVyZmxvdzogaGlkZGVuO1xyXG4gIHRleHQtb3ZlcmZsb3c6IGVsbGlwc2lzO1xyXG4gIHdoaXRlLXNwYWNlOiBub3dyYXA7XHJcbn1cclxuXHJcbi5hYnAtZWxsaXBzaXMge1xyXG4gIG92ZXJmbG93OiBoaWRkZW4gIWltcG9ydGFudDtcclxuICB0ZXh0LW92ZXJmbG93OiBlbGxpcHNpcztcclxuICB3aGl0ZS1zcGFjZTogbm93cmFwO1xyXG59XHJcblxyXG4uYWJwLXRvYXN0IC51aS10b2FzdC1tZXNzYWdlIHtcclxuICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xyXG4gIGJvcmRlcjogMnB4IHNvbGlkIHRyYW5zcGFyZW50O1xyXG4gIGJvcmRlci1yYWRpdXM6IDRweDtcclxuICBjb2xvcjogIzFiMWQyOTtcclxufVxyXG5cclxuLmFicC10b2FzdCAudWktdG9hc3QtbWVzc2FnZS1jb250ZW50IHtcclxuICBwYWRkaW5nOiAxMHB4O1xyXG59XHJcblxyXG4uYWJwLXRvYXN0IC51aS10b2FzdC1tZXNzYWdlLWNvbnRlbnQgLnVpLXRvYXN0LWljb24ge1xyXG4gIHRvcDogMDtcclxuICBsZWZ0OiAwO1xyXG4gIHBhZGRpbmc6IDEwcHg7XHJcbn1cclxuXHJcbi5hYnAtdG9hc3QgLnVpLXRvYXN0LXN1bW1hcnkge1xyXG4gIG1hcmdpbjogMDtcclxuICBmb250LXdlaWdodDogNzAwO1xyXG59XHJcblxyXG5ib2R5IGFicC10b2FzdCAudWktdG9hc3QgLnVpLXRvYXN0LW1lc3NhZ2UudWktdG9hc3QtbWVzc2FnZS1lcnJvciB7XHJcbiAgYm9yZGVyOiAycHggc29saWQgI2JhMTY1OTtcclxuICBiYWNrZ3JvdW5kLWNvbG9yOiAjZjRmNGY3O1xyXG59XHJcblxyXG5ib2R5IGFicC10b2FzdCAudWktdG9hc3QgLnVpLXRvYXN0LW1lc3NhZ2UudWktdG9hc3QtbWVzc2FnZS1lcnJvciAudWktdG9hc3QtbWVzc2FnZS1jb250ZW50IC51aS10b2FzdC1pY29uIHtcclxuICBjb2xvcjogI2JhMTY1OTtcclxufVxyXG5cclxuYm9keSBhYnAtdG9hc3QgLnVpLXRvYXN0IC51aS10b2FzdC1tZXNzYWdlLnVpLXRvYXN0LW1lc3NhZ2Utd2FybiB7XHJcbiAgYm9yZGVyOiAycHggc29saWQgI2VkNWQ5ODtcclxuICBiYWNrZ3JvdW5kLWNvbG9yOiAjZjRmNGY3O1xyXG59XHJcblxyXG5ib2R5IGFicC10b2FzdCAudWktdG9hc3QgLnVpLXRvYXN0LW1lc3NhZ2UudWktdG9hc3QtbWVzc2FnZS13YXJuIC51aS10b2FzdC1tZXNzYWdlLWNvbnRlbnQgLnVpLXRvYXN0LWljb24ge1xyXG4gIGNvbG9yOiAjZWQ1ZDk4O1xyXG59XHJcblxyXG5ib2R5IGFicC10b2FzdCAudWktdG9hc3QgLnVpLXRvYXN0LW1lc3NhZ2UudWktdG9hc3QtbWVzc2FnZS1zdWNjZXNzIHtcclxuICBib3JkZXI6IDJweCBzb2xpZCAjMWM5MTc0O1xyXG4gIGJhY2tncm91bmQtY29sb3I6ICNmNGY0Zjc7XHJcbn1cclxuXHJcbmJvZHkgYWJwLXRvYXN0IC51aS10b2FzdCAudWktdG9hc3QtbWVzc2FnZS51aS10b2FzdC1tZXNzYWdlLXN1Y2Nlc3MgLnVpLXRvYXN0LW1lc3NhZ2UtY29udGVudCAudWktdG9hc3QtaWNvbiB7XHJcbiAgY29sb3I6ICMxYzkxNzQ7XHJcbn1cclxuXHJcbmJvZHkgYWJwLXRvYXN0IC51aS10b2FzdCAudWktdG9hc3QtbWVzc2FnZS51aS10b2FzdC1tZXNzYWdlLWluZm8ge1xyXG4gIGJvcmRlcjogMnB4IHNvbGlkICNmY2NiMzE7XHJcbiAgYmFja2dyb3VuZC1jb2xvcjogI2Y0ZjRmNztcclxufVxyXG5cclxuYm9keSBhYnAtdG9hc3QgLnVpLXRvYXN0IC51aS10b2FzdC1tZXNzYWdlLnVpLXRvYXN0LW1lc3NhZ2UtaW5mbyAudWktdG9hc3QtbWVzc2FnZS1jb250ZW50IC51aS10b2FzdC1pY29uIHtcclxuICBjb2xvcjogI2ZjY2IzMTtcclxufVxyXG5cclxuLmFicC1jb25maXJtIC51aS10b2FzdC1tZXNzYWdlIHtcclxuICBib3gtc2l6aW5nOiBib3JkZXItYm94O1xyXG4gIHBhZGRpbmc6IDBweDtcclxuICBib3JkZXI6MCBub25lO1xyXG4gIGJvcmRlci1yYWRpdXM6IDRweDtcclxuICBiYWNrZ3JvdW5kLWNvbG9yOiB0cmFuc3BhcmVudCAhaW1wb3J0YW50O1xyXG4gIGZvbnQtZmFtaWx5OiBcIlBvcHBpbnNcIiwgc2Fucy1zZXJpZjtcclxuICB0ZXh0LWFsaWduOiBjZW50ZXI7XHJcbn1cclxuXHJcbi5hYnAtY29uZmlybSAudWktdG9hc3QtbWVzc2FnZS1jb250ZW50IHtcclxuICBwYWRkaW5nOiAwcHg7XHJcbn1cclxuXHJcbi5hYnAtY29uZmlybSAuYWJwLWNvbmZpcm0taWNvbiB7XHJcbiAgbWFyZ2luOiAzMnB4IDUwcHggNXB4ICFpbXBvcnRhbnQ7XHJcbiAgY29sb3I6ICNmOGJiODYgIWltcG9ydGFudDtcclxuICBmb250LXNpemU6IDUycHggIWltcG9ydGFudDtcclxufVxyXG5cclxuLmFicC1jb25maXJtIC51aS10b2FzdC1jbG9zZS1pY29uIHtcclxuICBkaXNwbGF5OiBub25lICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi5hYnAtY29uZmlybSAuYWJwLWNvbmZpcm0tc3VtbWFyeSB7XHJcbiAgZGlzcGxheTogYmxvY2sgIWltcG9ydGFudDtcclxuICBtYXJnaW4tYm90dG9tOiAxM3B4ICFpbXBvcnRhbnQ7XHJcbiAgcGFkZGluZzogMTNweCAxNnB4IDBweCAhaW1wb3J0YW50O1xyXG4gIGZvbnQtd2VpZ2h0OiA2MDAgIWltcG9ydGFudDtcclxuICBmb250LXNpemU6IDE4cHggIWltcG9ydGFudDtcclxufVxyXG5cclxuLmFicC1jb25maXJtIC5hYnAtY29uZmlybS1ib2R5IHtcclxuICBkaXNwbGF5OiBpbmxpbmUtYmxvY2sgIWltcG9ydGFudDtcclxuICBwYWRkaW5nOiAwcHggMTBweCAhaW1wb3J0YW50O1xyXG59XHJcblxyXG4uYWJwLWNvbmZpcm0gLmFicC1jb25maXJtLWZvb3RlciB7XHJcbiAgZGlzcGxheTogYmxvY2s7XHJcbiAgbWFyZ2luLXRvcDogMzBweDtcclxuICBwYWRkaW5nOiAxNnB4O1xyXG4gIHRleHQtYWxpZ246IHJpZ2h0O1xyXG59XHJcblxyXG4uYWJwLWNvbmZpcm0gLmFicC1jb25maXJtLWZvb3RlciAuYnRuIHtcclxuICBtYXJnaW4tbGVmdDogMTBweCAhaW1wb3J0YW50O1xyXG59XHJcblxyXG4udWktd2lkZ2V0LW92ZXJsYXkge1xyXG4gIHotaW5kZXg6IDEwMDA7XHJcbn1cclxuXHJcbi5jb2xvci13aGl0ZSB7XHJcbiAgY29sb3I6ICNGRkYgIWltcG9ydGFudDtcclxufVxyXG5cclxuLmN1c3RvbS1jaGVja2JveCA+IGxhYmVsIHtcclxuICBjdXJzb3I6IHBvaW50ZXI7XHJcbn1cclxuXHJcbi8qIDxhbmltYXRpb25zICovXHJcblxyXG4uZmFkZS1pbi10b3Age1xyXG4gIGFuaW1hdGlvbjogZmFkZUluVG9wIDAuMnMgZWFzZS1pbi1vdXQ7XHJcbn1cclxuXHJcbi5mYWRlLW91dC10b3Age1xyXG4gIGFuaW1hdGlvbjogZmFkZU91dFRvcCAwLjJzIGVhc2UtaW4tb3V0O1xyXG59XHJcblxyXG4uYWJwLWNvbGxhcHNlZC1oZWlnaHQge1xyXG4gIC1tb3otdHJhbnNpdGlvbjogbWF4LWhlaWdodCBsaW5lYXIgMC4zNXM7XHJcbiAgLW1zLXRyYW5zaXRpb246IG1heC1oZWlnaHQgbGluZWFyIDAuMzVzO1xyXG4gIC1vLXRyYW5zaXRpb246IG1heC1oZWlnaHQgbGluZWFyIDAuMzVzO1xyXG4gIC13ZWJraXQtdHJhbnNpdGlvbjogbWF4LWhlaWdodCBsaW5lYXIgMC4zNXM7XHJcbiAgb3ZlcmZsb3c6aGlkZGVuO1xyXG4gIHRyYW5zaXRpb246bWF4LWhlaWdodCAwLjM1cyBsaW5lYXI7XHJcbiAgaGVpZ2h0OmF1dG87XHJcbiAgbWF4LWhlaWdodDogMDtcclxufVxyXG5cclxuLmFicC1taC0yNSB7XHJcbiAgbWF4LWhlaWdodDogMjV2aDtcclxufVxyXG5cclxuLmFicC1taC01MCB7XHJcbiAgdHJhbnNpdGlvbjptYXgtaGVpZ2h0IDAuNjVzIGxpbmVhcjtcclxuICBtYXgtaGVpZ2h0OiA1MHZoO1xyXG59XHJcblxyXG4uYWJwLW1oLTc1IHtcclxuICB0cmFuc2l0aW9uOm1heC1oZWlnaHQgMC44NXMgbGluZWFyO1xyXG4gIG1heC1oZWlnaHQ6IDc1dmg7XHJcbn1cclxuXHJcbi5hYnAtbWgtMTAwIHtcclxuICB0cmFuc2l0aW9uOm1heC1oZWlnaHQgMXMgbGluZWFyO1xyXG4gIG1heC1oZWlnaHQ6IDEwMHZoO1xyXG59XHJcblxyXG5Aa2V5ZnJhbWVzIGZhZGVJblRvcCB7XHJcbiAgZnJvbSB7XHJcbiAgICB0cmFuc2Zvcm06IHRyYW5zbGF0ZVkoLTVweCk7XHJcbiAgICBvcGFjaXR5OiAwO1xyXG4gIH1cclxuXHJcbiAgdG8ge1xyXG4gICAgdHJhbnNmb3JtOiB0cmFuc2xhdGVZKDBweCk7XHJcbiAgICBvcGFjaXR5OiAxO1xyXG4gIH1cclxufVxyXG5cclxuQGtleWZyYW1lcyBmYWRlT3V0VG9wIHtcclxuICB0byB7XHJcbiAgICB0cmFuc2Zvcm06IHRyYW5zbGF0ZVkoLTVweCk7XHJcbiAgICBvcGFjaXR5OiAwO1xyXG4gIH1cclxufVxyXG5cclxuLyogPC9hbmltYXRpb25zICovXHJcblxyXG5gO1xyXG4iXX0=