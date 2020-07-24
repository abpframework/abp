(function (factory) {
    if (typeof define === 'function' && define.amd) {
      define(['jquery'], factory);
    } else if (typeof module === 'object' && typeof module.exports === 'object') {
      factory(require('jquery'));
    } else {
      factory(jQuery);
    }
  }(function (jQuery) {
    // Belarusian
    function numpf(n, f, s, t) {
      // f - 1, 21, 31, ...
      // s - 2-4, 22-24, 32-34 ...
      // t - 5-20, 25-30, ...
      n = n % 100;
      var n10 = n % 10;
      if ( (n10 === 1) && ( (n === 1) || (n > 20) ) ) {
        return f;
      } else if ( (n10 > 1) && (n10 < 5) && ( (n > 20) || (n < 10) ) ) {
        return s;
      } else {
        return t;
      }
    }
  
    jQuery.timeago.settings.strings = {
        prefixAgo: null,
        prefixFromNow: "праз",
        suffixAgo: "таму",
        suffixFromNow: null,
        seconds: "менш хвіліны",
        minute: "хвіліну",
      minutes: function(value) { return numpf(value, "%d хвіліна", "%d хвіліны", "%d хвілін"); },
      hour: "гадзіну",
      hours: function(value) { return numpf(value, "%d гадзіна", "%d гадзіны", "%d гадзін"); },
      day: "дзень",
      days: function(value) { return numpf(value, "%d дзень", "%d дні", "%d дзён"); },
      month: "месяц",
      months: function(value) { return numpf(value, "%d месяц", "%d месяцы", "%d месяцаў"); },
      year: "год",
      years: function(value) { return numpf(value, "%d год", "%d гады", "%d гадоў"); }
    };
  }));
