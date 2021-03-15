/*
    https://jquery.com/upgrade-guide/3.5/#jquery-htmlprefilter-changes
*/
var rxhtmlTag = /<(?!area|br|col|embed|hr|img|input|link|meta|param)(([a-z][^\/\0>\x20\t\r\n\f]*)[^>]*)\/>/gi;
jQuery.htmlPrefilter = function (html) {
    return html.replace(rxhtmlTag, "<$1></$2>");
};