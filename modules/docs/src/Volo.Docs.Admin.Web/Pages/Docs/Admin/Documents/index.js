$(function () {
    var l = abp.localization.getResource('Docs');
    var service = window.volo.docs.admin.documentsAdmin;

    var getFormattedDate = function ($datePicker) {
        return $datePicker.data().datepicker.getFormattedDate('yyyy-mm-dd');
    };
    
    var comboboxItems = [];
    abp.ajax({
        url: abp.appPath + 'api/docs/admin/documents/GetUniqueListDocumentWithoutDetails',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            comboboxItems = data;
            fillOptions();
        }
    });
    
    
    var $projectId = $('#ProjectId');
    var $version = $('#Version');
    var $languageCode = $('#LanguageCode');
    var $format = $('#Format');
    
    function fillOptions() {
        var selectedVersion = $version.val();
        var selectedLanguageCode = $languageCode.val();
        var selectedFormat = $format.val();
        var selectedProjectId = $projectId.val();

        $version.empty();
        $languageCode.empty();
        $format.empty();
        
        $version.append($('<option/>').val(''));
        $languageCode.append($('<option/>').val(''));
        $format.append($('<option/>').val(''));

        
        comboboxItems.forEach(function (item) {
            if (!selectedProjectId || item.projectId === selectedProjectId) {
                appendVersion(item,selectedVersion,selectedLanguageCode,selectedFormat);
                appendLanguageCode(item,selectedLanguageCode,selectedVersion,selectedFormat);
                appendFormat(item,selectedFormat,selectedVersion,selectedLanguageCode);
            }
        });
        
    }
    function appendFormat(item,selectedFormat,selectedVersion,selectedLanguageCode) {
        if(selectedVersion && selectedVersion !== item.version) {
            return;
        }
        if(selectedLanguageCode && selectedLanguageCode !== item.languageCode) {
            return;
        }

        if($format.find('option[value="' + item.format + '"]').length === 0) {
            var formatOption = $('<option>',{
                value: item.format,
                text: item.format
            });
            if(selectedFormat === item.format) {
                formatOption.attr('selected', 'selected');
            }
            $format.append(formatOption);
        }
    }
    
    function appendLanguageCode(item,selectedLanguageCode,selectedVersion,selectedFormat) {
        if(selectedVersion && selectedVersion !== item.version) {
            return;
        }
        if(selectedFormat && selectedFormat !== item.format) {
            return;
        }
        
        if($languageCode.find('option[value="' + item.languageCode + '"]').length === 0) {
            var languageCodeOption = $('<option>',{
                value: item.languageCode,
                text: item.languageCode
            });
            if(selectedLanguageCode === item.languageCode) {
                languageCodeOption.attr('selected', 'selected');
            }
            $languageCode.append(languageCodeOption);
        }
    }
    function appendVersion(item,selectedVersion,selectedLanguageCode,selectedFormat) {
        if(selectedLanguageCode && item.languageCode !== selectedLanguageCode) {
            return;
        }
        if(selectedFormat && selectedFormat !== item.format) {
            return;
        }
        
        if($version.find('option[value="' + item.version + '"]').length === 0) {
            var versionOption = $('<option>', {
                value: item.version,
                text: item.version
            })
            if(selectedVersion === item.version) {
                versionOption.attr('selected', 'selected');
            }
            $version.append(versionOption);
        }
    }

    $projectId.on('change', function () {
        fillOptions();
    });
    
    $version.on('change', function () {
        fillOptions();
    });
    
    $languageCode.on('change', function () {
        fillOptions();
    });
    
    $format.on('change', function () {
        fillOptions();
    });
    
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
