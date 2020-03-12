(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Hungarian
  jQuery.timeago.settings.strings = {
    prefixAgo: null,
    prefixFromNow: null,
    suffixAgo: null,
    suffixFromNow: null,
    seconds: "kevesebb mint egy perce",
    minute: "körülbelül egy perce",
    minutes: "%d perce",
    hour: "körülbelül egy órája",
    hours: "körülbelül %d órája",
    day: "körülbelül egy napja",
    days: "%d napja",
    month: "körülbelül egy hónapja",
    months: "%d hónapja",
    year: "körülbelül egy éve",
    years: "%d éve"
  };
}));
