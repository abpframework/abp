(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  // Galician
  jQuery.timeago.settings.strings = {
     prefixAgo: "hai",
     prefixFromNow: "dentro de",
     suffixAgo: "",
     suffixFromNow: "",
     seconds: "menos dun minuto",
     minute: "un minuto",
     minutes: "uns %d minutos",
     hour: "unha hora",
     hours: "%d horas",
     day: "un día",
     days: "%d días",
     month: "un mes",
     months: "%d meses",
     year: "un ano",
     years: "%d anos"
  };
}));
