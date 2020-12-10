var abp = abp || {};
(function ($) {

    var datatables = abp.utils.createNamespace(abp, 'libs.datatables');

    var localize = function (key) {
        return abp.localization.getResource('AbpUi')(key);
    };

    /************************************************************************
     * RECORD-ACTIONS extension for datatables                               *
     *************************************************************************/
    (function () {
        if (!$.fn.dataTableExt) {
            return;
        }

        var getVisibilityValue = function (visibilityField, record, tableInstance) {
            if (visibilityField === undefined) {
                return true;
            }

            if (abp.utils.isFunction(visibilityField)) {
                return visibilityField(record, tableInstance);
            } else {
                return visibilityField;
            }
        };

        var htmlEncode = function (html) {
            return $('<div/>').text(html).html();
        }
        
        var _createDropdownItem = function (record, fieldItem, tableInstance) {
            var $li = $('<li/>');
            var $a = $('<a/>');

            if (fieldItem.displayNameHtml) {
                $a.html(fieldItem.text);
            } else {

                if (fieldItem.icon !== undefined && fieldItem.icon) {
                    $a.append($("<i>").addClass("fa fa-" + fieldItem.icon + " mr-1"));
                } else if (fieldItem.iconClass) {
                    $a.append($("<i>").addClass(fieldItem.iconClass + " mr-1"));
                }

                $a.append(htmlEncode(fieldItem.text));
            }

            if (fieldItem.action) {
                $a.click(function (e) {
                    e.preventDefault();

                    if (!$(this).closest('li').hasClass('disabled')) {
                        if (fieldItem.confirmMessage) {
                            abp.message.confirm(fieldItem.confirmMessage({ record: record, table: tableInstance }))
                                .done(function (accepted) {
                                    if (accepted) {
                                        fieldItem.action({ record: record, table: tableInstance });
                                    }
                                });
                        } else {
                            fieldItem.action({ record: record, table: tableInstance });
                        }
                    }
                });
            }

            $a.appendTo($li);
            return $li;
        };

        var _createButtonDropdown = function (record, field, tableInstance) {
            if(field.items.length === 1) {
                var firstItem = field.items[0];
                if (!getVisibilityValue(firstItem.visible, record, tableInstance)) {
                    return "";
                }

                var $button = $('<button type="button" class="btn btn-primary"></button>');

                if (firstItem.displayNameHtml) {
                    $button.html(firstItem.text);
                } else {
                    if (firstItem.icon !== undefined && firstItem.icon) {
                        $button.append($("<i>").addClass("fa fa-" + firstItem.icon + " mr-1"));
                    } else if (firstItem.iconClass) {
                        $button.append($("<i>").addClass(firstItem.iconClass + " mr-1"));
                    }
                    $button.append(htmlEncode(firstItem.text));
                }
                
                if (firstItem.enabled && !firstItem.enabled({ record: record, table: tableInstance })) {
                    $button.addClass('disabled');
                }
                
                if (firstItem.action) {
                    $button.click(function (e) {
                        e.preventDefault();
    
                        if (!$(this).hasClass('disabled')) {
                            if (firstItem.confirmMessage) {
                                abp.message.confirm(firstItem.confirmMessage({ record: record, table: tableInstance }))
                                    .done(function (accepted) {
                                        if (accepted) {
                                            firstItem.action({ record: record, table: tableInstance });
                                        }
                                    });
                            } else {
                                firstItem.action({ record: record, table: tableInstance });
                            }
                        }
                    });
                }

                return $button;
            }

            var $container = $('<div/>')
                .addClass('dropdown')
                .addClass('action-button');

            var $dropdownButton = $('<button/>');

            if (field.icon !== undefined && field.icon) {
                $dropdownButton.append($("<i>").addClass("fa fa-" + field.icon + " mr-1"));
            } else if (field.iconClass) {
                $dropdownButton.append($("<i>").addClass(field.iconClass + " mr-1"));
            } else {
                $dropdownButton.append($("<i>").addClass("fa fa-cog mr-1"));
            }

            if (field.text) {
                $dropdownButton.append(htmlEncode(fieldItem.text));
            } else {
                $dropdownButton.append(htmlEncode(localize("DatatableActionDropdownDefaultText")));
            }

            $dropdownButton
                .addClass('btn btn-primary btn-sm dropdown-toggle')
                .attr('data-toggle', 'dropdown')
                .attr('aria-haspopup', 'true')
                .attr('aria-expanded', 'false');

            if (field.cssClass) {
                $dropdownButton.addClass(field.cssClass);
            }

            var $dropdownItemsContainer = $('<ul/>').addClass('dropdown-menu');

            for (var i = 0; i < field.items.length; i++) {
                var fieldItem = field.items[i];

                var isVisible = getVisibilityValue(fieldItem.visible, record, tableInstance);
                if (!isVisible) {
                    continue;
                }

                var $dropdownItem = _createDropdownItem(record, fieldItem, tableInstance);

                if (fieldItem.enabled && !fieldItem.enabled({ record: record, table: tableInstance })) {
                    $dropdownItem.addClass('disabled');
                }

                $dropdownItem.appendTo($dropdownItemsContainer);
            }

            if ($dropdownItemsContainer.find('li').length > 0) {
                $dropdownItemsContainer.appendTo($container);
                $dropdownButton.appendTo($container);
            }

            if ($dropdownItemsContainer.children().length === 0) {
                return "";
            }

            return $container;
        };

        var _createSingleButton = function (record, field, tableInstance) {
            $(field.element).data(record);

            var isVisible = getVisibilityValue(field.visible, record, tableInstance);

            if (isVisible) {
                return field.element;
            }

            return "";
        };

        var _createRowAction = function (record, field, tableInstance) {
            if (field.items && field.items.length > 0) {
                return _createButtonDropdown(record, field, tableInstance);
            } else if (field.element) {
                var $singleActionButton = _createSingleButton(record, field, tableInstance);
                if ($singleActionButton === "") {
                    return "";
                }

                return $singleActionButton.clone(true);
            }

            throw "DTE#1: Cannot create row action. Either set element or items fields!";
        };

        var hideColumnWithoutRedraw = function (tableInstance, colIndex) {
            tableInstance.fnSetColumnVis(colIndex, false, false);
        };

        var hideEmptyColumn = function (cellContent, tableInstance, colIndex) {
            if (cellContent == "") {
                hideColumnWithoutRedraw(tableInstance, colIndex);
            }
        };

        var renderRowActions = function (tableInstance, nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            var columns;
			
            if (tableInstance.aoColumns) {
                columns = tableInstance.aoColumns;
            } else {
                columns = tableInstance.fnSettings().aoColumns;
            }

            if (!columns) {
                return;
            }

            var cells = $(nRow).children("td");

            for (var colIndex = 0; colIndex < columns.length; colIndex++) {
                var column = columns[colIndex];
                if (column.rowAction) {
                    var $actionContainer = _createRowAction(aData, column.rowAction, tableInstance);
                    hideEmptyColumn($actionContainer, tableInstance, colIndex);

                    var $actionButton = $(cells[colIndex]).find(".action-button");
                    if ($actionButton.length === 0) {
                        $(cells[colIndex]).append($actionContainer);
                    }
                }
            }
        };

        var _existingApiRenderRowActionsFunction = $.fn.dataTableExt.oApi.renderRowActions;
        $.fn.dataTableExt.oApi.renderRowActions =
            function (tableInstance, nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (_existingApiRenderRowActionsFunction) {
                    _existingApiRenderRowActionsFunction(tableInstance, nRow, aData, iDisplayIndex, iDisplayIndexFull);
                }

                renderRowActions(tableInstance, nRow, aData, iDisplayIndex, iDisplayIndexFull);
            };

        if (!$.fn.dataTable) {
            return;
        }

        var _existingDefaultFnRowCallback = $.fn.dataTable.defaults.fnRowCallback;
        $.extend(true,
            $.fn.dataTable.defaults,
            {
                fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    if (_existingDefaultFnRowCallback) {
                        _existingDefaultFnRowCallback(this, nRow, aData, iDisplayIndex, iDisplayIndexFull);
                    }

                    renderRowActions(this, nRow, aData, iDisplayIndex, iDisplayIndexFull);
                }
            });

       //Delay for processing indicator
        var defaultDelayForProcessingIndicator = 500;
        var _existingDefaultFnPreDrawCallback = $.fn.dataTable.defaults.fnPreDrawCallback;
        $.extend(true,
            $.fn.dataTable.defaults,
            {
                fnPreDrawCallback: function (settings) {
                    if (_existingDefaultFnPreDrawCallback) {
                        _existingDefaultFnPreDrawCallback(settings);
                    }

                    var $tableWrapper = $(settings.nTableWrapper);
                    var $processing = $tableWrapper.find(".dataTables_processing");
                    var timeoutHandles = [];
                    var cancelHandles = [];

                    $tableWrapper.on('processing.dt',
                        function (e, settings, processing) {
                            if ((settings.oInit.processingDelay !== undefined && settings.oInit.processingDelay < 1) || defaultDelayForProcessingIndicator < 1) {
                                return;
                            }

                            if (processing) {
                                $processing.hide();

                                var delay = settings.oInit.processingDelay === undefined
                                    ? defaultDelayForProcessingIndicator
                                    : settings.oInit.processingDelay;

                                cancelHandles[settings.nTableWrapper.id] = false;

                                timeoutHandles[settings.nTableWrapper.id] = setTimeout(function () {
                                    if (cancelHandles[settings.nTableWrapper.id] === true) {
                                        return;
                                    }

                                    $processing.show();
                                }, delay);
                            }
                            else {
                                clearTimeout(timeoutHandles[settings.nTableWrapper.id]);
                                cancelHandles[settings.nTableWrapper.id] = true;
                                $processing.hide();
                            }
                        });
                }
            });

    })();

    /************************************************************************
     * AJAX extension for datatables                                         *
     *************************************************************************/
    (function () {
        datatables.createAjax = function (serverMethod, inputAction, responseCallback) {
            responseCallback = responseCallback || function(result) {
                return {
                    recordsTotal: result.totalCount,
                    recordsFiltered: result.totalCount,
                    data: result.items
                };
            }
            return function (requestData, callback, settings) {
                var input = inputAction ? inputAction(requestData, settings) : {};

                //Paging
                if (settings.oInit.paging) {
                    input.maxResultCount = requestData.length;
                    input.skipCount = requestData.start;
                }

                //Sorting
                if (requestData.order && requestData.order.length > 0) {
                    input.sorting = "";

                    for (var i = 0; i < requestData.order.length; i++) {
                        var orderingField = requestData.order[i];

                        if (requestData.columns[orderingField.column].data) {
                            input.sorting += requestData.columns[orderingField.column].data + " " + orderingField.dir;

                            if (i < requestData.order.length - 1) {
                                input.sorting += ",";
                            }
                        }
                    }
                }

                //Text filter
                if (requestData.search && requestData.search.value !== "") {
                    input.filter = requestData.search.value;
                }

                if (callback) {
                    serverMethod(input).then(function (result) {
                        callback(responseCallback(result));
                    });
                }
            };
        };
    })();

    /************************************************************************
     * Configuration/Options normalizer for datatables                       *
     *************************************************************************/
    (function () {

        var customizeRowActionColumn = function (column) {
            column.data = null;
            column.orderable = false;
            column.defaultContent = "";

            if (column.autoWidth === undefined) {
                column.autoWidth = false;
            }
        };

        datatables.normalizeConfiguration = function (configuration) {

            configuration.scrollX = datatables.defaultConfigurations.scrollX;

            for (var i = 0; i < configuration.columnDefs.length; i++) {
                var column = configuration.columnDefs[i];
                if (!column.targets) {
                    column.targets = i;
                }

                if (!column.render && column.dataFormat) {
                    var render = datatables.defaultRenderers[column.dataFormat];
                    if (render) {
                        column.render = render;
                    }
                }

                if (column.rowAction) {
                    customizeRowActionColumn(column);
                }
            }

            configuration.language = datatables.defaultConfigurations.language();

            if(configuration.dom){
                configuration.dom += datatables.defaultConfigurations.dom;
            }else{
                configuration.dom = datatables.defaultConfigurations.dom;
            }

            return configuration;
        };
    })();

    /************************************************************************
     * Default Renderers                                                     *
     *************************************************************************/

    datatables.defaultRenderers = datatables.defaultRenderers || {};

    datatables.defaultRenderers['boolean'] = function(value) {
        if (value) {
            return '<i class="fa fa-check"></i>';
        } else {
            return '<i class="fa fa-times"></i>';
        }
    };

    var ISOStringToDateTimeLocaleString = function (format) {
        return function(data) {
            var date = luxon
                .DateTime
                .fromISO(data, {
                    locale: abp.localization.currentCulture.name
                });
            return format ? date.toLocaleString(format) : date.toLocaleString();
        };
    };

    datatables.defaultRenderers['date'] = function (value) {
        return (ISOStringToDateTimeLocaleString())(value);
    };

    datatables.defaultRenderers['datetime'] = function (value) {
        return (ISOStringToDateTimeLocaleString(luxon.DateTime.DATETIME_SHORT))(value);
    };

    /************************************************************************
     * Default Configurations                                                *
     *************************************************************************/

    datatables.defaultConfigurations = datatables.defaultConfigurations || {};

    datatables.defaultConfigurations.scrollX = true;

    datatables.defaultConfigurations.responsive = true; 

    datatables.defaultConfigurations.language = function () {
        return {
            info: localize("PagerInfo"),
            infoFiltered: localize("PagerInfoFiltered"),
            infoEmpty: localize("PagerInfoEmpty"),
            search: localize("PagerSearch"),
            processing: localize("ProcessingWithThreeDot"),
            loadingRecords: localize("LoadingWithThreeDot"),
            lengthMenu: localize("PagerShowMenuEntries"),
            emptyTable: localize("NoDataAvailableInDatatable"),
            paginate: {
                first: localize("PagerFirst"),
                last: localize("PagerLast"),
                previous: localize("PagerPrevious"),
                next: localize("PagerNext")
            }
        };
    };

    datatables.defaultConfigurations.dom = '<"dataTable_filters"f>rt<"row dataTable_footer"<"col-auto"l><"col-auto"i><"col"p>>';

})(jQuery);
