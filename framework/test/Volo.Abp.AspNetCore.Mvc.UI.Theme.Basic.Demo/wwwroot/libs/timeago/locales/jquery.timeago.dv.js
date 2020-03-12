(function (factory) {
  if (typeof define === 'function' && define.amd) {
    define(['jquery'], factory);
  } else if (typeof module === 'object' && typeof module.exports === 'object') {
    factory(require('jquery'));
  } else {
    factory(jQuery);
  }
}(function (jQuery) {
  /** 
   * Dhivehi time in Thaana for timeago.js
   **/
  jQuery.timeago.settings.strings = {
    prefixAgo: null,
    prefixFromNow: null,
    suffixAgo: "ކުރިން",
    suffixFromNow: "ފަހުން",
    seconds: "ސިކުންތުކޮޅެއް",
    minute: "މިނިޓެއްވަރު",
    minutes: "%d މިނިޓު",
    hour: "ގަޑިއެއްވަރު",
    hours: "ގާތްގަނޑަކަށް %d ގަޑިއިރު",
    day: "އެއް ދުވަސް",
    days: "މީގެ %d ދުވަސް",
    month: "މަހެއްވަރު",
    months: "މީގެ %d މަސް",
    year: "އަހަރެއްވަރު",
    years: "މީގެ %d އަހަރު",
    wordSeparator: " ",
    numbers: []
  };
}));
