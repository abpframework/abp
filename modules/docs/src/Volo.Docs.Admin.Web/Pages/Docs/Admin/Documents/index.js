$(function () {
    var l = abp.localization.getResource('Docs');
    var service = window.volo.docs.admin.documentsAdmin;

    var getFormattedDate = function ($datePicker) {
        return $datePicker.data().datepicker.getFormattedDate('yyyy-mm-dd');
    };

    var getFilter = function () {
        return {
            projectId: $('#ProjectId').val(),
            name: $('#Name').val(),
            version: $('#Version').val(),
            languageCode: $('#LanguageCode').val(),
            fileName: $('#FileName').val(),
            format: $('#Format').val(),
            creationTimeMin: getFormattedDate($('#CreationTimeMin')),
            creationTimeMax: getFormattedDate($('#CreationTimeMax')),
            lastUpdatedTimeMin: getFormattedDate($('#LastUpdatedTimeMin')),
            lastUpdatedTimeMax: getFormattedDate($('#LastUpdatedTimeMax')),
            lastSignificantUpdateTimeMin: getFormattedDate(
                $('#LastSignificantUpdateTimeMin')
            ),
            lastSignificantUpdateTimeMax: getFormattedDate(
                $('#LastSignificantUpdateTimeMax')
            ),
            lastCachedTimeMin: getFormattedDate($('#LastCachedTimeMin')),
            lastCachedTimeMax: getFormattedDate($('#LastCachedTimeMax')),
        };
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
                                confirmMessage: function (data) {
                                    return l('RemoveFromCacheConfirmation');
                                },
                                action: function (data) {
                                    service
                                        .removeFromCache(data.record.id)
                                        .then(function () {
                                            abp.message.success(
                                                l('RemovedFromCache')
                                            );
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
                                            abp.message.success(
                                                l('ReindexCompleted')
                                            );
                                            dataTable.ajax.reload();
                                        });
                                },
                            },
                            {
                                text: l('DeleteFromDatabase'),
                                visible: abp.auth.isGranted(
                                    'Docs.Admin.Documents'
                                ),
                                confirmMessage: function (data) {
                                    return l(
                                        'DeleteDocumentFromDbConfirmation'
                                    );
                                },
                                action: function (data) {
                                    service
                                        .deleteFromDatabase(data.record.id)
                                        .then(function () {
                                            abp.message.success(l('Deleted'));
                                            dataTable.ajax.reload();
                                        });
                                },
                            },
                        ],
                    },
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

                        var date = Date.parse(creationTime);
                        return new Date(date).toLocaleDateString(
                            abp.localization.currentCulture.name
                        );
                    },
                },
                {
                    target: 7,
                    data: 'lastUpdatedTime',
                    render: function (lastUpdatedTime) {
                        if (!lastUpdatedTime) {
                            return '';
                        }

                        var date = Date.parse(lastUpdatedTime);
                        return new Date(date).toLocaleDateString(
                            abp.localization.currentCulture.name
                        );
                    },
                },
                {
                    target: 8,
                    data: 'lastSignificantUpdateTime',
                    render: function (lastSignificantUpdateTime) {
                        if (!lastSignificantUpdateTime) {
                            return '';
                        }

                        var date = Date.parse(lastSignificantUpdateTime);
                        return new Date(date).toLocaleDateString(
                            abp.localization.currentCulture.name
                        );
                    },
                },
                {
                    target: 9,
                    data: 'lastCachedTime',
                    render: function (lastCachedTime) {
                        if (!lastCachedTime) {
                            return '';
                        }

                        var date = Date.parse(lastCachedTime);
                        return new Date(date).toLocaleDateString(
                            abp.localization.currentCulture.name
                        );
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
});
