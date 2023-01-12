$(function () {
    var l = abp.localization.getResource('Docs');
    var service = window.volo.docs.admin.documentsAdmin;

    var getFormattedDate = function ($datePicker) {
        return $datePicker.data('date');
    };

	moment.localeData().preparse = (s)=>s;
    moment.localeData().postformat = (s)=>s;
	
    $('.singledatepicker').daterangepicker({
        "singleDatePicker": true,
        "showDropdowns": true,
        "autoUpdateInput": false,
        "autoApply": true,
        "opens": "center",
        "drops": "auto",
        "minYear": 1901,
        "maxYear": 2199,
    });

    $('.singledatepicker').attr('autocomplete', 'off');

    $('.singledatepicker').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('l'));
        $(this).data('date', picker.startDate.locale('en').format('YYYY-MM-DD'));
    });


    var comboboxItems = [];
    
    service.getFilterItems()
        .then(function (result) {
            comboboxItems = result;
            fillOptions();
        }).catch(function (error) {
        abp.message.error(error);
    });


    var $projectId = $('#ProjectId');

    $projectId.on('change', function () {
        fillOptions();
    });
    
    var comboboxs = {
        version: $('#Version'),
        languageCode: $('#LanguageCode'),
        format: $('#Format')
    };
    
    for (var key in comboboxs) {
        comboboxs[key].on('change', function () {
            fillOptions();
        });
    }


    var selectedItem = getSelectedItem();

    function emptyComboboxs() {
        for (var key in comboboxs) {
            comboboxs[key].empty();
        }
    }
    
    function getSelectedItem() {
        var item = {};
        for (var key in comboboxs) {
            item[key] = comboboxs[key].val();
        }
        return item;
    }
    
    function SetComboboxsValues(item) {
        for (var key in comboboxs) {
            comboboxs[key].val(item[key]);
        }
    }
    
    function addComboboxsEmptyItem() {
        for (var key in comboboxs) {
            comboboxs[key].append($('<option/>').val('').text(''));
        }
    }

    function fillOptions() {
        
        selectedItem = getSelectedItem();
        
        var selectedProjectId = $projectId.val();

        emptyComboboxs();
        
        addComboboxsEmptyItem();

        var selectedProjectItems = comboboxItems.filter((item) => !selectedProjectId || item.projectId === selectedProjectId);
        
        for (var key in selectedItem) {
            var item = selectedProjectItems.find((item) => item[key] === selectedItem[key]);
            if (item) {
                selectedItem[key] = item[key];
            }else {
                selectedItem[key] = '';
            }
        }
        

        selectedProjectItems.forEach(function (item) {
            for (var key in comboboxs) {
                appendComboboxItem(comboboxs[key], item, key);
            }
        });
        
        SetComboboxsValues(selectedItem);
    }
    function appendComboboxItem($combobox, item , key) {
        $.each(item, function (index, value) {
            if(index !== key) {
                if(selectedItem[index] && selectedItem[index] !== value) {
                    return ;
                }}
        });
        if($combobox.find('option[value="' + item[key] + '"]').length === 0){
            $combobox.append($('<option/>').val(item[key]).text(item[key]));
        }
    }


    var getFilter = function () {
        return {
            projectId: $('#ProjectId').val(),
            name: $('#Name').val(),
            version: $('#Version').val(),
            languageCode: $('#LanguageCode').val(),
            format: $('#Format').val(),
            creationTimeMin: getFormattedDate($('#CreationTimeMin')),
            creationTimeMax: getFormattedDate($('#CreationTimeMax')),
            lastUpdatedTimeMin: getFormattedDate($('#LastUpdatedTimeMin')),
            lastUpdatedTimeMax: getFormattedDate($('#LastUpdatedTimeMax')),
            lastSignificantUpdateTimeMin: getFormattedDate($('#LastSignificantUpdateTimeMin')),
            lastSignificantUpdateTimeMax: getFormattedDate($('#LastSignificantUpdateTimeMax')),
            lastCachedTimeMin: getFormattedDate($('#LastCachedTimeMin')),
            lastCachedTimeMax: getFormattedDate($('#LastCachedTimeMax')),
        };
    };

    var parseDateToLocaleDateString = function (date) {
        var parsedDate = Date.parse(date);
        return new Date(parsedDate).toLocaleDateString(
            abp.localization.currentCulture.name
        );
    };

    var dataTable = $('#DocumentsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: true,
            scrollX: true,
            paging: true,
            searching: false,
            autoWidth: false,
            scrollCollapse: true,
            ajax: abp.libs.datatables.createAjax(service.getAll, getFilter),
            columnDefs: [
                {
                    rowAction: {
                        items: [
                            {
                                text: l('RemoveFromCache'),
                                visible: abp.auth.isGranted(
                                    'Docs.Admin.Documents'
                                ),
                                action: function (data) {
                                    service
                                        .removeFromCache(data.record.id)
                                        .then(function () {
                                            abp.notify.success(l('RemovedFromCache'));
                                            dataTable.ajax.reload();
                                        });
                                },
                            },
                            {
                                text: l('ReIndex'),
                                visible: abp.auth.isGranted(
                                    'Docs.Admin.Documents'
                                ),
                                confirmMessage: function (data) {
                                    return l('ReIndexDocumentConfirmation');
                                },
                                action: function (data) {
                                    service
                                        .reindex(data.record.id)
                                        .then(function () {
                                            abp.message.success(l('ReindexCompleted'));
                                            dataTable.ajax.reload();
                                        });
                                },
                            }
                        ],
                    },
                },
                {
                    target: 0,
                    data: 'projectName',
                },
                {
                    target: 1,
                    data: 'name',
                },
                {
                    target: 2,
                    data: 'version',
                },
                {
                    target: 3,
                    data: 'languageCode',
                },
                {
                    target: 4,
                    data: 'fileName',
                },
                {
                    target: 5,
                    data: 'format',
                },
                {
                    target: 6,
                    data: 'creationTime',
                    render: function (creationTime) {
                        if (!creationTime) {
                            return '';
                        }

                        return parseDateToLocaleDateString(creationTime);
                    },
                },
                {
                    target: 7,
                    data: 'lastUpdatedTime',
                    render: function (lastUpdatedTime) {
                        if (!lastUpdatedTime) {
                            return '';
                        }

                        return parseDateToLocaleDateString(lastUpdatedTime);
                    },
                },
                {
                    target: 8,
                    data: 'lastSignificantUpdateTime',
                    render: function (lastSignificantUpdateTime) {
                        if (!lastSignificantUpdateTime) {
                            return '';
                        }

                        return parseDateToLocaleDateString(lastSignificantUpdateTime);
                    },
                },
                {
                    target: 9,
                    data: 'lastCachedTime',
                    render: function (lastCachedTime) {
                        if (!lastCachedTime) {
                            return '';
                        }

                        return parseDateToLocaleDateString(lastCachedTime);
                    },
                },
            ],
        })
    );

    $("#FilterForm input[type='text']").keypress(function (e) {
        if (e.which === 13) {
            dataTable.ajax.reload();
        }
    });

    $('#SearchButton').click(function (e) {
        e.preventDefault();
        dataTable.ajax.reload();
    });

    $("#AdvancedFilterSectionToggler").click(function (e) {
        $("#AdvancedFilterSection").toggle();
    });
});
