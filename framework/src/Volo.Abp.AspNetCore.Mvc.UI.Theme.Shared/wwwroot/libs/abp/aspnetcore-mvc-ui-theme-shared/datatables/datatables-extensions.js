var abp = abp;

(function ($) {

    /************************************************************************
    * RECORD-ACTIONS extension for datatables                               
     ---------------------------------------------------------------
    * SINGLE BUTTON USAGE (creates the given JQuery element)
       {
            targets: 0,  //optional
            rowAction: 
            {
                element: $("<button/>")
                    .addClass("btn btn-primary btn-sm m-btn--icon")
                    .text("My button")
                    .prepend($("<i/>").addClass("la la-sign-in"))
                    .click(function () {
                        console.log($(this).data());
                    })
            },
        },

     ---------------------------------------------------------------
     * LIST OF ITEMS USAGE
       {
           targets: 0, //optional
           rowAction: 
           {
                text: 'My actions', //optional. default value: Actions
                icon: 'bolt' //optional. default value: cog. See fa icon set https://fontawesome.com/v4.7.0/icons/
                items:
                    [
                        {
                            text: "My first action", //mandatory
                            icon: "thumbs-o-down",  //optional.
                            visible: true //optional. default value: true. Accepts boolean returning function too. Eg: function(){ return true/false;} ,
                            action: function (data) {
                                console.log(data.record);
                            }
                        },
                         {
                            text: "My second action",
                            icon: "thumbs-o-up",
                            visible: true, 
                            action: function (data) {
                                console.log(data.record);
                            }
                        }
                    ]
           }
        },
    *************************************************************************/
    var localize = function (key) {
        return abp.localization.getResource('AbpUi')(key);
    };

    var recordActions = function () {
        if (!$.fn.dataTableExt) {
            return;
        }

        var getVisibilityValue = function (visibilityField, record) {
            if (visibilityField === undefined) {
                return true;
            }

            if (abp.utils.isFunction(visibilityField)) {
                return visibilityField(record);
            } else {
                return visibilityField;
            }
        };

        var _createDropdownItem = function (record, fieldItem) {
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

                $a.append(fieldItem.text);
            }

            if (fieldItem.action) {
                $a.click(function (e) {
                    e.preventDefault();

                    if (!$(this).closest('li').hasClass('disabled')) {
                        if (fieldItem.confirmMessage) {
                            abp.message.confirm(fieldItem.confirmMessage({ record: record }))
                                .done(function (accepted) {
                                    if (accepted) {
                                        fieldItem.action({ record: record });
                                    }
                                });
                        } else {
                            fieldItem.action({ record: record });
                        }
                    }
                });
            }

            $a.appendTo($li);
            return $li;
        };

        var _createButtonDropdown = function (record, field) {
            var $container = $('<div/>')
                .addClass('dropdown')
                .addClass('action-button');

            var $dropdownButton = $('<button/>');
            
            if (field.icon !== undefined) {
                if (field.icon) {
                    $dropdownButton.append($("<i>").addClass("fa fa-" + field.icon + " mr-1"));
                }
            } else if (field.iconClass) {
                $dropdownButton.append($("<i>").addClass(field.iconClass + " mr-1"));
            } else {
                $dropdownButton.append($("<i>").addClass("fa fa-cog mr-1"));
            }

            if (field.text) {
                $dropdownButton.append(field.text);
            } else {
                $dropdownButton.append(localize("DatatableActionDropdownDefaultText"));
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

                var isVisible = getVisibilityValue(fieldItem.visible, record);
                if (!isVisible) {
                    continue;
                }

                var $dropdownItem = _createDropdownItem(record, fieldItem);

                if (fieldItem.enabled && !fieldItem.enabled({ record: record })) {
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

        var _createSingleButton = function (record, field) {
            $(field.element).data(record);

            var isVisible = getVisibilityValue(field.visible, record);

            if (isVisible) {
                return field.element;
            }

            return "";
        };

        var _createRowAction = function (record, field, tableInstance) {
            if (field.items && field.items.length > 0) {
                return _createButtonDropdown(record, field, tableInstance);
            } else if (field.element) {
                var $singleActionButton = _createSingleButton(record, field);
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

    }();

    /************************************************************************
    * AJAX extension for datatables                                         *
    *************************************************************************/
    var datatables = abp.utils.createNamespace(abp, 'libs.datatables');

    var ajaxActions = function () {
        datatables.createAjax = function (serverMethod, inputAction) {
            return function (requestData, callback, settings) {
                var input = inputAction ? inputAction() : {};

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
                        callback({
                            recordsTotal: result.totalCount,
                            recordsFiltered: result.totalCount,
                            data: result.items
                        });
                    });
                }
            };
        };
    }();

    /************************************************************************
    * Configuration/Options normalizer for datatables                       *
    *************************************************************************/
    var optionNormalizer = function () {

        var customizeRowActionColumn = function(column) {
            column.data = null;
            column.orderable = false;
            column.defaultContent = "";

            if (column.autoWidth === undefined) {
                column.autoWidth = false;
            }
        };

        datatables.normalizeConfiguration = function (configuration) {
            for (var i = 0; i < configuration.columnDefs.length; i++) {
                var column = configuration.columnDefs[i];
                if (!column.targets) {
                    column.targets = i;
                }

                if (column.rowAction) {
                    customizeRowActionColumn(column);
                }
            }

            configuration.language = {
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

            configuration.dom = '<"dataTable_filters"f>rt<"row dataTable_footer"<"col-auto"l><"col-auto"i><"col"p>>';

            return configuration;
        };

    }();

})(jQuery);