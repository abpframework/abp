/*
    https://select2.org/troubleshooting/common-problems
*/
if ($.fn.modal) {
    $.fn.modal.Constructor.prototype._enforceFocus = function () { };
}
