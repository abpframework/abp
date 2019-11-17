/**
 * @fileoverview added by tsickle
 * Generated from: lib/contants/styles.ts
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
  position: absolute !important;
  top: 0 !important;
  left: 0 !important;
  width: 100% !important;
  height: 100% !important;
  background-color: rgba(0, 0, 0, 0.6) !important;
  z-index: 1040 !important;
}

.modal-dialog {
  z-index: 1050 !important;
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
  box-sizing: border-box !important;
  border: 2px solid transparent !important;
  border-radius: 4px !important;
  background-color: #f4f4f7 !important;
  color: #1b1d29 !important;
}

.abp-toast .ui-toast-message-content {
  padding: 10px !important;
}

.abp-toast .ui-toast-message-content .ui-toast-icon {
  top: 0 !important;
  left: 0 !important;
  padding: 10px !important;
}

.abp-toast .ui-toast-summary {
  margin: 0 !important;
  font-weight: 700 !important;
}

.abp-toast .ui-toast-message.ui-toast-message-error {
  border-color: #ba1659 !important;
}

.abp-toast .ui-toast-message.ui-toast-message-error .ui-toast-message-content .ui-toast-icon {
  color: #ba1659 !important;
}

.abp-toast .ui-toast-message.ui-toast-message-warning {
  border-color: #ed5d98 !important;
}

.abp-toast .ui-toast-message.ui-toast-message-warning .ui-toast-message-content .ui-toast-icon {
  color: #ed5d98 !important;
}

.abp-toast .ui-toast-message.ui-toast-message-success {
  border-color: #1c9174 !important;
}

.abp-toast .ui-toast-message.ui-toast-message-success .ui-toast-message-content .ui-toast-icon {
  color: #1c9174 !important;
}

.abp-toast .ui-toast-message.ui-toast-message-info {
  border-color: #fccb31 !important;
}

.abp-toast .ui-toast-message.ui-toast-message-info .ui-toast-message-content .ui-toast-icon {
  color: #fccb31 !important;
}

.abp-confirm .ui-toast-message {
  box-sizing: border-box !important;
  padding: 0px !important;
  border:0 none !important;
  border-radius: 4px !important;
  background-color: #fff !important;
  color: rgba(0, 0, 0, .65) !important;
  font-family: "Poppins", sans-serif;
  text-align: center !important;
}

.abp-confirm .ui-toast-message-content {
  padding: 0px !important;
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
  display: block !important;
  margin-top: 30px !important;
  padding: 16px !important;
  background-color: #f4f4f7 !important;
  text-align: right !important;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic3R5bGVzLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29udGFudHMvc3R5bGVzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsZUFBZTs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7OztDQXVQZCxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiZXhwb3J0IGRlZmF1bHQgYFxyXG4uaXMtaW52YWxpZCAuZm9ybS1jb250cm9sIHtcclxuICBib3JkZXItY29sb3I6ICNkYzM1NDU7XHJcbiAgYm9yZGVyLXN0eWxlOiBzb2xpZCAhaW1wb3J0YW50O1xyXG59XHJcblxyXG4uaXMtaW52YWxpZCAuaW52YWxpZC1mZWVkYmFjayxcclxuLmlzLWludmFsaWQgKyAqIC5pbnZhbGlkLWZlZWRiYWNrIHtcclxuICBkaXNwbGF5OiBibG9jaztcclxufVxyXG5cclxuLmRhdGEtdGFibGVzLWZpbHRlciB7XHJcbiAgdGV4dC1hbGlnbjogcmlnaHQ7XHJcbn1cclxuXHJcbi5wb2ludGVyIHtcclxuICBjdXJzb3I6IHBvaW50ZXI7XHJcbn1cclxuXHJcbi5uYXZiYXIgLmRyb3Bkb3duLXN1Ym1lbnUgYTo6YWZ0ZXIge1xyXG4gIHRyYW5zZm9ybTogcm90YXRlKC05MGRlZyk7XHJcbiAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gIHJpZ2h0OiAxNnB4O1xyXG4gIHRvcDogMThweDtcclxufVxyXG5cclxuLm5hdmJhciAuZHJvcGRvd24tbWVudSB7XHJcbiAgbWluLXdpZHRoOiAyMTVweDtcclxufVxyXG5cclxuLnVpLXRhYmxlLXNjcm9sbGFibGUtYm9keTo6LXdlYmtpdC1zY3JvbGxiYXIge1xyXG4gIGhlaWdodDogNXB4ICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi51aS10YWJsZS1zY3JvbGxhYmxlLWJvZHk6Oi13ZWJraXQtc2Nyb2xsYmFyLXRyYWNrIHtcclxuICBiYWNrZ3JvdW5kOiAjZGRkO1xyXG59XHJcblxyXG4udWktdGFibGUtc2Nyb2xsYWJsZS1ib2R5Ojotd2Via2l0LXNjcm9sbGJhci10aHVtYiB7XHJcbiAgYmFja2dyb3VuZDogIzhhODY4NjtcclxufVxyXG5cclxuLm1vZGFsLnNob3cge1xyXG4gIGRpc3BsYXk6IGJsb2NrICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi5tb2RhbC1iYWNrZHJvcCB7XHJcbiAgcG9zaXRpb246IGFic29sdXRlICFpbXBvcnRhbnQ7XHJcbiAgdG9wOiAwICFpbXBvcnRhbnQ7XHJcbiAgbGVmdDogMCAhaW1wb3J0YW50O1xyXG4gIHdpZHRoOiAxMDAlICFpbXBvcnRhbnQ7XHJcbiAgaGVpZ2h0OiAxMDAlICFpbXBvcnRhbnQ7XHJcbiAgYmFja2dyb3VuZC1jb2xvcjogcmdiYSgwLCAwLCAwLCAwLjYpICFpbXBvcnRhbnQ7XHJcbiAgei1pbmRleDogMTA0MCAhaW1wb3J0YW50O1xyXG59XHJcblxyXG4ubW9kYWwtZGlhbG9nIHtcclxuICB6LWluZGV4OiAxMDUwICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi5hYnAtZWxsaXBzaXMtaW5saW5lIHtcclxuICBkaXNwbGF5OiBpbmxpbmUtYmxvY2s7XHJcbiAgb3ZlcmZsb3c6IGhpZGRlbjtcclxuICB0ZXh0LW92ZXJmbG93OiBlbGxpcHNpcztcclxuICB3aGl0ZS1zcGFjZTogbm93cmFwO1xyXG59XHJcblxyXG4uYWJwLWVsbGlwc2lzIHtcclxuICBvdmVyZmxvdzogaGlkZGVuICFpbXBvcnRhbnQ7XHJcbiAgdGV4dC1vdmVyZmxvdzogZWxsaXBzaXM7XHJcbiAgd2hpdGUtc3BhY2U6IG5vd3JhcDtcclxufVxyXG5cclxuLmFicC10b2FzdCAudWktdG9hc3QtbWVzc2FnZSB7XHJcbiAgYm94LXNpemluZzogYm9yZGVyLWJveCAhaW1wb3J0YW50O1xyXG4gIGJvcmRlcjogMnB4IHNvbGlkIHRyYW5zcGFyZW50ICFpbXBvcnRhbnQ7XHJcbiAgYm9yZGVyLXJhZGl1czogNHB4ICFpbXBvcnRhbnQ7XHJcbiAgYmFja2dyb3VuZC1jb2xvcjogI2Y0ZjRmNyAhaW1wb3J0YW50O1xyXG4gIGNvbG9yOiAjMWIxZDI5ICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi5hYnAtdG9hc3QgLnVpLXRvYXN0LW1lc3NhZ2UtY29udGVudCB7XHJcbiAgcGFkZGluZzogMTBweCAhaW1wb3J0YW50O1xyXG59XHJcblxyXG4uYWJwLXRvYXN0IC51aS10b2FzdC1tZXNzYWdlLWNvbnRlbnQgLnVpLXRvYXN0LWljb24ge1xyXG4gIHRvcDogMCAhaW1wb3J0YW50O1xyXG4gIGxlZnQ6IDAgIWltcG9ydGFudDtcclxuICBwYWRkaW5nOiAxMHB4ICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi5hYnAtdG9hc3QgLnVpLXRvYXN0LXN1bW1hcnkge1xyXG4gIG1hcmdpbjogMCAhaW1wb3J0YW50O1xyXG4gIGZvbnQtd2VpZ2h0OiA3MDAgIWltcG9ydGFudDtcclxufVxyXG5cclxuLmFicC10b2FzdCAudWktdG9hc3QtbWVzc2FnZS51aS10b2FzdC1tZXNzYWdlLWVycm9yIHtcclxuICBib3JkZXItY29sb3I6ICNiYTE2NTkgIWltcG9ydGFudDtcclxufVxyXG5cclxuLmFicC10b2FzdCAudWktdG9hc3QtbWVzc2FnZS51aS10b2FzdC1tZXNzYWdlLWVycm9yIC51aS10b2FzdC1tZXNzYWdlLWNvbnRlbnQgLnVpLXRvYXN0LWljb24ge1xyXG4gIGNvbG9yOiAjYmExNjU5ICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi5hYnAtdG9hc3QgLnVpLXRvYXN0LW1lc3NhZ2UudWktdG9hc3QtbWVzc2FnZS13YXJuaW5nIHtcclxuICBib3JkZXItY29sb3I6ICNlZDVkOTggIWltcG9ydGFudDtcclxufVxyXG5cclxuLmFicC10b2FzdCAudWktdG9hc3QtbWVzc2FnZS51aS10b2FzdC1tZXNzYWdlLXdhcm5pbmcgLnVpLXRvYXN0LW1lc3NhZ2UtY29udGVudCAudWktdG9hc3QtaWNvbiB7XHJcbiAgY29sb3I6ICNlZDVkOTggIWltcG9ydGFudDtcclxufVxyXG5cclxuLmFicC10b2FzdCAudWktdG9hc3QtbWVzc2FnZS51aS10b2FzdC1tZXNzYWdlLXN1Y2Nlc3Mge1xyXG4gIGJvcmRlci1jb2xvcjogIzFjOTE3NCAhaW1wb3J0YW50O1xyXG59XHJcblxyXG4uYWJwLXRvYXN0IC51aS10b2FzdC1tZXNzYWdlLnVpLXRvYXN0LW1lc3NhZ2Utc3VjY2VzcyAudWktdG9hc3QtbWVzc2FnZS1jb250ZW50IC51aS10b2FzdC1pY29uIHtcclxuICBjb2xvcjogIzFjOTE3NCAhaW1wb3J0YW50O1xyXG59XHJcblxyXG4uYWJwLXRvYXN0IC51aS10b2FzdC1tZXNzYWdlLnVpLXRvYXN0LW1lc3NhZ2UtaW5mbyB7XHJcbiAgYm9yZGVyLWNvbG9yOiAjZmNjYjMxICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi5hYnAtdG9hc3QgLnVpLXRvYXN0LW1lc3NhZ2UudWktdG9hc3QtbWVzc2FnZS1pbmZvIC51aS10b2FzdC1tZXNzYWdlLWNvbnRlbnQgLnVpLXRvYXN0LWljb24ge1xyXG4gIGNvbG9yOiAjZmNjYjMxICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi5hYnAtY29uZmlybSAudWktdG9hc3QtbWVzc2FnZSB7XHJcbiAgYm94LXNpemluZzogYm9yZGVyLWJveCAhaW1wb3J0YW50O1xyXG4gIHBhZGRpbmc6IDBweCAhaW1wb3J0YW50O1xyXG4gIGJvcmRlcjowIG5vbmUgIWltcG9ydGFudDtcclxuICBib3JkZXItcmFkaXVzOiA0cHggIWltcG9ydGFudDtcclxuICBiYWNrZ3JvdW5kLWNvbG9yOiAjZmZmICFpbXBvcnRhbnQ7XHJcbiAgY29sb3I6IHJnYmEoMCwgMCwgMCwgLjY1KSAhaW1wb3J0YW50O1xyXG4gIGZvbnQtZmFtaWx5OiBcIlBvcHBpbnNcIiwgc2Fucy1zZXJpZjtcclxuICB0ZXh0LWFsaWduOiBjZW50ZXIgIWltcG9ydGFudDtcclxufVxyXG5cclxuLmFicC1jb25maXJtIC51aS10b2FzdC1tZXNzYWdlLWNvbnRlbnQge1xyXG4gIHBhZGRpbmc6IDBweCAhaW1wb3J0YW50O1xyXG59XHJcblxyXG4uYWJwLWNvbmZpcm0gLmFicC1jb25maXJtLWljb24ge1xyXG4gIG1hcmdpbjogMzJweCA1MHB4IDVweCAhaW1wb3J0YW50O1xyXG4gIGNvbG9yOiAjZjhiYjg2ICFpbXBvcnRhbnQ7XHJcbiAgZm9udC1zaXplOiA1MnB4ICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi5hYnAtY29uZmlybSAudWktdG9hc3QtY2xvc2UtaWNvbiB7XHJcbiAgZGlzcGxheTogbm9uZSAhaW1wb3J0YW50O1xyXG59XHJcblxyXG4uYWJwLWNvbmZpcm0gLmFicC1jb25maXJtLXN1bW1hcnkge1xyXG4gIGRpc3BsYXk6IGJsb2NrICFpbXBvcnRhbnQ7XHJcbiAgbWFyZ2luLWJvdHRvbTogMTNweCAhaW1wb3J0YW50O1xyXG4gIHBhZGRpbmc6IDEzcHggMTZweCAwcHggIWltcG9ydGFudDtcclxuICBmb250LXdlaWdodDogNjAwICFpbXBvcnRhbnQ7XHJcbiAgZm9udC1zaXplOiAxOHB4ICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi5hYnAtY29uZmlybSAuYWJwLWNvbmZpcm0tYm9keSB7XHJcbiAgZGlzcGxheTogaW5saW5lLWJsb2NrICFpbXBvcnRhbnQ7XHJcbiAgcGFkZGluZzogMHB4IDEwcHggIWltcG9ydGFudDtcclxufVxyXG5cclxuLmFicC1jb25maXJtIC5hYnAtY29uZmlybS1mb290ZXIge1xyXG4gIGRpc3BsYXk6IGJsb2NrICFpbXBvcnRhbnQ7XHJcbiAgbWFyZ2luLXRvcDogMzBweCAhaW1wb3J0YW50O1xyXG4gIHBhZGRpbmc6IDE2cHggIWltcG9ydGFudDtcclxuICBiYWNrZ3JvdW5kLWNvbG9yOiAjZjRmNGY3ICFpbXBvcnRhbnQ7XHJcbiAgdGV4dC1hbGlnbjogcmlnaHQgIWltcG9ydGFudDtcclxufVxyXG5cclxuLmFicC1jb25maXJtIC5hYnAtY29uZmlybS1mb290ZXIgLmJ0biB7XHJcbiAgbWFyZ2luLWxlZnQ6IDEwcHggIWltcG9ydGFudDtcclxufVxyXG5cclxuLnVpLXdpZGdldC1vdmVybGF5IHtcclxuICB6LWluZGV4OiAxMDAwO1xyXG59XHJcblxyXG4uY29sb3Itd2hpdGUge1xyXG4gIGNvbG9yOiAjRkZGICFpbXBvcnRhbnQ7XHJcbn1cclxuXHJcbi8qIDxhbmltYXRpb25zICovXHJcblxyXG4uZmFkZS1pbi10b3Age1xyXG4gIGFuaW1hdGlvbjogZmFkZUluVG9wIDAuMnMgZWFzZS1pbi1vdXQ7XHJcbn1cclxuXHJcbi5mYWRlLW91dC10b3Age1xyXG4gIGFuaW1hdGlvbjogZmFkZU91dFRvcCAwLjJzIGVhc2UtaW4tb3V0O1xyXG59XHJcblxyXG4uYWJwLWNvbGxhcHNlZC1oZWlnaHQge1xyXG4gIC1tb3otdHJhbnNpdGlvbjogbWF4LWhlaWdodCBsaW5lYXIgMC4zNXM7XHJcbiAgLW1zLXRyYW5zaXRpb246IG1heC1oZWlnaHQgbGluZWFyIDAuMzVzO1xyXG4gIC1vLXRyYW5zaXRpb246IG1heC1oZWlnaHQgbGluZWFyIDAuMzVzO1xyXG4gIC13ZWJraXQtdHJhbnNpdGlvbjogbWF4LWhlaWdodCBsaW5lYXIgMC4zNXM7XHJcbiAgb3ZlcmZsb3c6aGlkZGVuO1xyXG4gIHRyYW5zaXRpb246bWF4LWhlaWdodCAwLjM1cyBsaW5lYXI7XHJcbiAgaGVpZ2h0OmF1dG87XHJcbiAgbWF4LWhlaWdodDogMDtcclxufVxyXG5cclxuLmFicC1taC0yNSB7XHJcbiAgbWF4LWhlaWdodDogMjV2aDtcclxufVxyXG5cclxuLmFicC1taC01MCB7XHJcbiAgdHJhbnNpdGlvbjptYXgtaGVpZ2h0IDAuNjVzIGxpbmVhcjtcclxuICBtYXgtaGVpZ2h0OiA1MHZoO1xyXG59XHJcblxyXG4uYWJwLW1oLTc1IHtcclxuICB0cmFuc2l0aW9uOm1heC1oZWlnaHQgMC44NXMgbGluZWFyO1xyXG4gIG1heC1oZWlnaHQ6IDc1dmg7XHJcbn1cclxuXHJcbi5hYnAtbWgtMTAwIHtcclxuICB0cmFuc2l0aW9uOm1heC1oZWlnaHQgMXMgbGluZWFyO1xyXG4gIG1heC1oZWlnaHQ6IDEwMHZoO1xyXG59XHJcblxyXG5Aa2V5ZnJhbWVzIGZhZGVJblRvcCB7XHJcbiAgZnJvbSB7XHJcbiAgICB0cmFuc2Zvcm06IHRyYW5zbGF0ZVkoLTVweCk7XHJcbiAgICBvcGFjaXR5OiAwO1xyXG4gIH1cclxuXHJcbiAgdG8ge1xyXG4gICAgdHJhbnNmb3JtOiB0cmFuc2xhdGVZKDBweCk7XHJcbiAgICBvcGFjaXR5OiAxO1xyXG4gIH1cclxufVxyXG5cclxuQGtleWZyYW1lcyBmYWRlT3V0VG9wIHtcclxuICB0byB7XHJcbiAgICB0cmFuc2Zvcm06IHRyYW5zbGF0ZVkoLTVweCk7XHJcbiAgICBvcGFjaXR5OiAwO1xyXG4gIH1cclxufVxyXG5cclxuLyogPC9hbmltYXRpb25zICovXHJcblxyXG5gO1xyXG4iXX0=