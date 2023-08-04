(function () {
    [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]')).map(function (popoverTriggerEl) {
      return new bootstrap.Popover(popoverTriggerEl)
    })
})();
