(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  jQuery.timeago.settings.strings = {
     prefixAgo: "duela",
     prefixFromNow: "hemendik",
     suffixAgo: "",
     suffixFromNow: "barru",
     seconds: "minutu bat bainu gutxiago",
     minute: "minutu bat",
     minutes: "%d minutu inguru",
     hour: "ordu bat",
     hours: "%d ordu",
     day: "egun bat",
     days: "%d egun",
     month: "hilabete bat",
     months: "%d hilabete",
     year: "urte bat",
     years: "%d urte"
  };
}));

