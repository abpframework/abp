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
