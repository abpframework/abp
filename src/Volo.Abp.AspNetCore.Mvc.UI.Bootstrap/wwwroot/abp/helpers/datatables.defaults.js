/************************************************************************
* Overrides default settings for datatables                             *
*************************************************************************/
(function ($) {
    if (!$.fn.dataTable) {
        return;
    }

    var translationUrl = abp.appPath + 'libs/datatables/localizations/';
    var translationsUrl = translationUrl + abp.localization.currentCulture.displayNameEnglish + '.json';
    var defaultLanguage = "English.json";

    if (abp.localization.currentCulture.displayNameEnglish === defaultLanguage) {
        $.fn.dataTable.defaults.languageFileReady = true;
    } else {
        $.ajax(translationsUrl)
            .fail(function () {
                translationsUrl = translationUrl + 'English.json';
                console.log('DataTables.Language.Fallback to English, cannot retrieve ' + abp.localization.currentCulture.displayNameEnglish + ' is not found!');
            }).always(function () {
                $.extend(true, $.fn.dataTable.defaults, {
                    language: {
                        url: translationsUrl
                    },
                    lengthMenu: [5, 10, 25, 50, 100, 250, 500],
                    pageLength: 10,
                    responsive: true,
                    searching: false,
                    pagingType: "bootstrap_full_number",
                    dom: 'rt<"bottom"ilp><"clear">',
                    order: []
                });

                $.fn.dataTable.defaults.languageFileReady = true;
            });
    }


    /* Set the defaults for DataTables initialisation */
    $.extend(true, $.fn.dataTable.defaults, {
        "dom": "<'row'<'col-md-6 col-sm-6'l><'col-md-6 col-sm-6'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-5'i><'col-md-7 col-sm-7'p>>",
        "language": {
            "lengthMenu": " _MENU_ records ",
            "paginate": {
                "previous": 'Prev',
                "next": 'Next',
                "page": "Page",
                "pageOf": "of"
            }
        },
        "pagingType": "bootstrap_full_number"
    });

    /* Default class modification */
    $.extend($.fn.dataTableExt.oStdClasses, {
        "sWrapper": "dataTables_wrapper",
        "sFilterInput": "form-control m-input form-control-sm",
        "sLengthSelect": "form-control m-input form-control-sm"
    });

    //// In 1.10 we use the pagination renderers to draw the Bootstrap paging,
    //// rather than  custom plug-in
    //$.fn.dataTable.defaults.renderer = 'bootstrap';
    $.fn.dataTable.ext.renderer.pageButton.bootstrap = function (settings, host, idx, buttons, page, pages) {
        var api = new $.fn.dataTable.Api(settings);
        var classes = settings.oClasses;
        var lang = settings.oLanguage.oPaginate;
        var btnDisplay, btnClass;

        var attach = function (container, buttons) {
            var i, ien, node, button;
            var clickHandler = function (e) {
                e.preventDefault();
                if (e.data.action !== 'ellipsis') {
                    api.page(e.data.action).draw(false);
                }
            };

            for (i = 0, ien = buttons.length; i < ien; i++) {
                button = buttons[i];

                if ($.isArray(button)) {
                    attach(container, button);
                } else {
                    btnDisplay = '';
                    btnClass = '';

                    switch (button) {
                        case 'ellipsis':
                            btnDisplay = '&hellip;';
                            btnClass = 'disabled';
                            break;

                        case 'first':
                            btnDisplay = lang.sFirst;
                            btnClass = button + (page > 0 ?
                                '' : ' disabled');
                            break;

                        case 'previous':
                            btnDisplay = lang.sPrevious;
                            btnClass = button + (page > 0 ?
                                '' : ' disabled');
                            break;

                        case 'next':
                            btnDisplay = lang.sNext;
                            btnClass = button + (page < pages - 1 ?
                                '' : ' disabled');
                            break;

                        case 'last':
                            btnDisplay = lang.sLast;
                            btnClass = button + (page < pages - 1 ?
                                '' : ' disabled');
                            break;

                        default:
                            btnDisplay = button + 1;
                            btnClass = page === button ?
                                'active' : '';
                            break;
                    }

                    if (btnDisplay) {
                        node = $('<li>', {
                            'class': classes.sPageButton + ' ' + btnClass,
                            'aria-controls': settings.sTableId,
                            'tabindex': settings.iTabIndex,
                            'id': idx === 0 && typeof button === 'string' ?
                                settings.sTableId + '_' + button : null
                        })
                            .append($('<a>', {
                                'href': '#'
                            })
                                .html(btnDisplay)
                            )
                            .appendTo(container);

                        settings.oApi._fnBindAction(
                            node, {
                                action: button
                            }, clickHandler
                        );
                    }
                }
            }
        };

        attach(
            $(host).empty().html('<ul class="pagination"/>').children('ul'),
            buttons
        );
    }


    /* API method to get paging information */
    $.fn.dataTableExt.oApi.fnPagingInfo = function (oSettings) {
        return {
            "iStart": oSettings._iDisplayStart,
            "iEnd": oSettings.fnDisplayEnd(),
            "iLength": oSettings._iDisplayLength,
            "iTotal": oSettings.fnRecordsTotal(),
            "iFilteredTotal": oSettings.fnRecordsDisplay(),
            "iPage": oSettings._iDisplayLength === -1 ?
                0 : Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength),
            "iTotalPages": oSettings._iDisplayLength === -1 ?
                0 : Math.ceil(oSettings.fnRecordsDisplay() / oSettings._iDisplayLength)
        };
    };

    /* Bootstrap style full number pagination control */
    $.extend($.fn.dataTableExt.oPagination, {
        "bootstrap_full_number": {
            "fnInit": function (oSettings, nPaging, fnDraw) {
                var oLang = oSettings.oLanguage.oPaginate;
                var fnClickHandler = function (e) {
                    e.preventDefault();
                    if (oSettings.oApi._fnPageChange(oSettings, e.data.action)) {
                        fnDraw(oSettings);
                    }
                };

                $(nPaging).append(
                    '<ul class="pagination">' +
                    '<li class="prev disabled"><a href="#" title="' + oLang.sFirst + '"><i class="fa fa-angle-double-left"></i></a></li>' +
                    '<li class="prev disabled"><a href="#" title="' + oLang.sPrevious + '"><i class="fa fa-angle-left"></i></a></li>' +
                    '<li class="next disabled"><a href="#" title="' + oLang.sNext + '"><i class="fa fa-angle-right"></i></a></li>' +
                    '<li class="next disabled"><a href="#" title="' + oLang.sLast + '"><i class="fa fa-angle-double-right"></i></a></li>' +
                    '</ul>'
                );
                var els = $('a', nPaging);
                $(els[0]).bind('click.DT', {
                    action: "first"
                }, fnClickHandler);
                $(els[1]).bind('click.DT', {
                    action: "previous"
                }, fnClickHandler);
                $(els[2]).bind('click.DT', {
                    action: "next"
                }, fnClickHandler);
                $(els[3]).bind('click.DT', {
                    action: "last"
                }, fnClickHandler);
            },

            "fnUpdate": function (oSettings, fnDraw) {
                var iListLength = 5;
                var oPaging = oSettings.oInstance.fnPagingInfo();
                var an = oSettings.aanFeatures.p;
                var i, j, sClass, iStart, iEnd, iHalf = Math.floor(iListLength / 2);

                if (oPaging.iTotalPages < iListLength) {
                    iStart = 1;
                    iEnd = oPaging.iTotalPages;
                } else if (oPaging.iPage <= iHalf) {
                    iStart = 1;
                    iEnd = iListLength;
                } else if (oPaging.iPage >= (oPaging.iTotalPages - iHalf)) {
                    iStart = oPaging.iTotalPages - iListLength + 1;
                    iEnd = oPaging.iTotalPages;
                } else {
                    iStart = oPaging.iPage - iHalf + 1;
                    iEnd = iStart + iListLength - 1;
                }



                for (i = 0, iLen = an.length; i < iLen; i++) {
                    if (oPaging.iTotalPages <= 0) {
                        $('.pagination', an[i]).css('visibility', 'hidden');
                    } else {
                        $('.pagination', an[i]).css('visibility', 'visible');
                    }

                    // Remove the middle elements
                    $('li:gt(1)', an[i]).filter(':not(.next)').remove();

                    // Add the new list items and their event handlers
                    for (j = iStart; j <= iEnd; j++) {
                        sClass = (j == oPaging.iPage + 1) ? 'class="active"' : '';
                        $('<li ' + sClass + '><a href="#">' + j + '</a></li>')
                            .insertBefore($('li.next:first', an[i])[0])
                            .bind('click', function (e) {
                                e.preventDefault();
                                oSettings._iDisplayStart = (parseInt($('a', this).text(), 10) - 1) * oPaging.iLength;
                                fnDraw(oSettings);
                            });
                    }

                    // Add / remove disabled classes from the static elements
                    if (oPaging.iPage === 0) {
                        $('li.prev', an[i]).addClass('disabled');
                    } else {
                        $('li.prev', an[i]).removeClass('disabled');
                    }

                    if (oPaging.iPage === oPaging.iTotalPages - 1 || oPaging.iTotalPages === 0) {
                        $('li.next', an[i]).addClass('disabled');
                    } else {
                        $('li.next', an[i]).removeClass('disabled');
                    }
                }
            }
        }
    });

    $.extend($.fn.dataTableExt.oPagination, {
        "bootstrap_number": {
            "fnInit": function (oSettings, nPaging, fnDraw) {
                var oLang = oSettings.oLanguage.oPaginate;
                var fnClickHandler = function (e) {
                    e.preventDefault();
                    if (oSettings.oApi._fnPageChange(oSettings, e.data.action)) {
                        fnDraw(oSettings);
                    }
                };

                $(nPaging).append(
                    '<ul class="pagination">' +
                    '<li class="prev disabled"><a href="#" title="' + oLang.sPrevious + '"><i class="fa fa-angle-left"></i></a></li>' +
                    '<li class="next disabled"><a href="#" title="' + oLang.sNext + '"><i class="fa fa-angle-right"></i></a></li>' +
                    '</ul>'
                );
                var els = $('a', nPaging);
                $(els[0]).bind('click.DT', {
                    action: "previous"
                }, fnClickHandler);
                $(els[1]).bind('click.DT', {
                    action: "next"
                }, fnClickHandler);
            },

            "fnUpdate": function (oSettings, fnDraw) {
                var iListLength = 5;
                var oPaging = oSettings.oInstance.fnPagingInfo();
                var an = oSettings.aanFeatures.p;
                var i, j, sClass, iStart, iEnd, iHalf = Math.floor(iListLength / 2);

                if (oPaging.iTotalPages < iListLength) {
                    iStart = 1;
                    iEnd = oPaging.iTotalPages;
                } else if (oPaging.iPage <= iHalf) {
                    iStart = 1;
                    iEnd = iListLength;
                } else if (oPaging.iPage >= (oPaging.iTotalPages - iHalf)) {
                    iStart = oPaging.iTotalPages - iListLength + 1;
                    iEnd = oPaging.iTotalPages;
                } else {
                    iStart = oPaging.iPage - iHalf + 1;
                    iEnd = iStart + iListLength - 1;
                }

                for (i = 0, iLen = an.length; i < iLen; i++) {
                    if (oPaging.iTotalPages <= 0) {
                        $('.pagination', an[i]).css('visibility', 'hidden');
                    } else {
                        $('.pagination', an[i]).css('visibility', 'visible');
                    }

                    // Remove the middle elements
                    $('li:gt(0)', an[i]).filter(':not(.next)').remove();

                    // Add the new list items and their event handlers
                    for (j = iStart; j <= iEnd; j++) {
                        sClass = (j == oPaging.iPage + 1) ? 'class="active"' : '';
                        $('<li ' + sClass + '><a href="#">' + j + '</a></li>')
                            .insertBefore($('li.next:first', an[i])[0])
                            .bind('click', function (e) {
                                e.preventDefault();
                                oSettings._iDisplayStart = (parseInt($('a', this).text(), 10) - 1) * oPaging.iLength;
                                fnDraw(oSettings);
                            });
                    }

                    // Add / remove disabled classes from the static elements
                    if (oPaging.iPage === 0) {
                        $('li.prev', an[i]).addClass('disabled');
                    } else {
                        $('li.prev', an[i]).removeClass('disabled');
                    }

                    if (oPaging.iPage === oPaging.iTotalPages - 1 || oPaging.iTotalPages === 0) {
                        $('li.next', an[i]).addClass('disabled');
                    } else {
                        $('li.next', an[i]).removeClass('disabled');
                    }
                }
            }
        }
    });


    /* Bootstrap style full number pagination control */
    $.extend($.fn.dataTableExt.oPagination, {
        "bootstrap_extended": {
            "fnInit": function (oSettings, nPaging, fnDraw) {
                var oLang = oSettings.oLanguage.oPaginate;
                var oPaging = oSettings.oInstance.fnPagingInfo();

                var fnClickHandler = function (e) {
                    e.preventDefault();
                    if (oSettings.oApi._fnPageChange(oSettings, e.data.action)) {
                        fnDraw(oSettings);
                    }
                };

                $(nPaging).append(
                    '<div class="pagination-panel"> ' + (oLang.page ? oLang.page : '') + ' ' +
                    '<a href="#" class="btn btn-sm default prev disabled"><i class="fa fa-angle-left"></i></a>' +
                    '<input type="text" class="pagination-panel-input form-control input-sm input-inline input-mini" maxlenght="5" style="text-align:center; margin: 0 5px;">' +
                    '<a href="#" class="btn btn-sm default next disabled"><i class="fa fa-angle-right"></i></a> ' +
                    (oLang.pageOf ? oLang.pageOf + ' <span class="pagination-panel-total"></span>' : '') +
                    '</div>'
                );

                var els = $('a', nPaging);

                $(els[0]).bind('click.DT', {
                    action: "previous"
                }, fnClickHandler);
                $(els[1]).bind('click.DT', {
                    action: "next"
                }, fnClickHandler);

                $('.pagination-panel-input', nPaging).bind('change.DT', function (e) {
                    var oPaging = oSettings.oInstance.fnPagingInfo();
                    e.preventDefault();
                    var page = parseInt($(this).val());
                    if (page > 0 && page <= oPaging.iTotalPages) {
                        if (oSettings.oApi._fnPageChange(oSettings, page - 1)) {
                            fnDraw(oSettings);
                        }
                    } else {
                        $(this).val(oPaging.iPage + 1);
                    }
                });

                $('.pagination-panel-input', nPaging).bind('keypress.DT', function (e) {
                    var oPaging = oSettings.oInstance.fnPagingInfo();
                    if (e.which == 13) {
                        var page = parseInt($(this).val());
                        if (page > 0 && page <= oSettings.oInstance.fnPagingInfo().iTotalPages) {
                            if (oSettings.oApi._fnPageChange(oSettings, page - 1)) {
                                fnDraw(oSettings);
                            }
                        } else {
                            $(this).val(oPaging.iPage + 1);
                        }
                        e.preventDefault();
                    }
                });
            },

            "fnUpdate": function (oSettings, fnDraw) {
                var iListLength = 5;
                var oPaging = oSettings.oInstance.fnPagingInfo();
                var an = oSettings.aanFeatures.p;
                var i, j, sClass, iStart, iEnd, iHalf = Math.floor(iListLength / 2);

                if (oPaging.iTotalPages < iListLength) {
                    iStart = 1;
                    iEnd = oPaging.iTotalPages;
                } else if (oPaging.iPage <= iHalf) {
                    iStart = 1;
                    iEnd = iListLength;
                } else if (oPaging.iPage >= (oPaging.iTotalPages - iHalf)) {
                    iStart = oPaging.iTotalPages - iListLength + 1;
                    iEnd = oPaging.iTotalPages;
                } else {
                    iStart = oPaging.iPage - iHalf + 1;
                    iEnd = iStart + iListLength - 1;
                }

                for (i = 0, iLen = an.length; i < iLen; i++) {
                    var wrapper = $(an[i]).parents(".dataTables_wrapper");

                    if (oPaging.iTotal <= 0) {
                        $('.dataTables_paginate, .dataTables_length', wrapper).hide();
                    } else {
                        $('.dataTables_paginate, .dataTables_length', wrapper).show();
                    }

                    if (oPaging.iTotalPages <= 0) {
                        $('.dataTables_paginate, .dataTables_length .seperator', wrapper).hide();
                    } else {
                        $('.dataTables_paginate, .dataTables_length .seperator', wrapper).show();
                    }

                    $('.pagination-panel-total', an[i]).html(oPaging.iTotalPages);
                    $('.pagination-panel-input', an[i]).val(oPaging.iPage + 1);

                    // Remove the middle elements
                    $('li:gt(1)', an[i]).filter(':not(.next)').remove();

                    // Add the new list items and their event handlers
                    for (j = iStart; j <= iEnd; j++) {
                        sClass = (j == oPaging.iPage + 1) ? 'class="active"' : '';
                        $('<li ' + sClass + '><a href="#">' + j + '</a></li>')
                            .insertBefore($('li.next:first', an[i])[0])
                            .bind('click', function (e) {
                                e.preventDefault();
                                oSettings._iDisplayStart = (parseInt($('a', this).text(), 10) - 1) * oPaging.iLength;
                                fnDraw(oSettings);
                            });
                    }

                    // Add / remove disabled classes from the static elements
                    if (oPaging.iPage === 0) {
                        $('a.prev', an[i]).addClass('disabled');
                    } else {
                        $('a.prev', an[i]).removeClass('disabled');
                    }

                    if (oPaging.iPage === oPaging.iTotalPages - 1 || oPaging.iTotalPages === 0) {
                        $('a.next', an[i]).addClass('disabled');
                    } else {
                        $('a.next', an[i]).removeClass('disabled');
                    }
                }
            }
        }
    });

})(jQuery);