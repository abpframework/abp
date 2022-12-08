(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Macedonian
  (function() {
   jQuery.timeago.settings.strings={
      prefixAgo: "пред",
      prefixFromNow: "за",
      suffixAgo: null,
      suffixFromNow: null,
      seconds: "%d секунди",
      minute: "%d минута",
      minutes: "%d минути",
      hour: "%d час",
      hours: "%d часа",
      day: "%d ден",
      days: "%d денови" ,
      month: "%d месец",
      months: "%d месеци",
      year: "%d година",
      years: "%d години"
   };
  })();
}));
