$(function () {

    var l = abp.localization.getResource('Docs');

    var _dataTable = $('#DocumentsTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        scrollX: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        ajax: abp.libs.datatables.createAjax(volo.docs.admin.documentsAdmin.getAll),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l('RemoveFromCache'),
                                visible: abp.auth.isGranted('Docs.Admin.Documents'),
                                confirmMessage: function (data) { return l('RemoveFromCacheConfirmation'); },
                                action: function (data) {
                                    volo.docs.admin.documentsAdmin
                                        .removeFromCache(data.record.id)
                                        .then(function () {
                                            abp.message.success(l('RemovedFromCache'));
                                            _dataTable.ajax.reload();
                                        });
                                }
                            },
                            {
                                text: l('ReIndex'),
                                visible: abp.auth.isGranted('Docs.Admin.Documents'),
                                confirmMessage: function (data) { return l('ReIndexDocumentConfirmation'); },
                                action: function (data) {
                                    volo.docs.admin.documentsAdmin
                                        .reindex(data.record.id)
                                        .then(function () {
                                            abp.message.success(l('ReindexCompleted'));
                                            _dataTable.ajax.reload();
                                        });
                                }
                            },
                            {
                                text: l('DeleteFromDatabase'),
                                visible: abp.auth.isGranted('Docs.Admin.Documents'),
                                confirmMessage: function (data) { return l('DeleteDocumentFromDbConfirmation'); },
                                action: function (data) {
                                    volo.docs.admin.documentsAdmin
                                        .deleteFromDatabase(data.record.id)
                                        .then(function () {
                                            abp.message.success(l('Deleted'));
                                            _dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                }
            },
            {
                target: 1,
                data: "name"
            },
            {
                target: 2,
                data: "version"
            },
            {
                target: 3,
                data: "languageCode"
            },
            {
                target: 4,
                data: "fileName"
            },
            {
                target: 5,
                data: "format",
                render: function (data) {
                    if (data === 'md') {
                        return 'markdown';
                    }

                    return data;
                }
            },
            {
                target: 6,
                data: "creationTime",
                render: function (creationTime) {
                    if (!creationTime) {
                        return "";
                    }

                    var date = Date.parse(creationTime);
                    return (new Date(date)).toLocaleDateString(abp.localization.currentCulture.name);
                }
            },
            {
                target: 7,
                data: "lastUpdatedTime",
                render: function (lastUpdatedTime) {
                    if (!lastUpdatedTime) {
                        return "";
                    }

                    var date = Date.parse(lastUpdatedTime);
                    return (new Date(date)).toLocaleDateString(abp.localization.currentCulture.name);
                }
            },
            {
                target: 8,
                data: "lastSignificantUpdateTime",
                render: function (lastSignificantUpdateTime) {
                    if (!lastSignificantUpdateTime) {
                        return "";
                    }

                    var date = Date.parse(lastSignificantUpdateTime);
                    return (new Date(date)).toLocaleDateString(abp.localization.currentCulture.name);
                }
            },
            {
                target: 9,
                data: "lastCachedTime",
                render: function (lastCachedTime) {
                    if (!lastCachedTime) {
                        return "";
                    }

                    var date = Date.parse(lastCachedTime);
                    return (new Date(date)).toLocaleDateString(abp.localization.currentCulture.name);
                }
            }
        ]
    }));


    $("#ReIndexAllProjects").click(function (event) {
        abp.message.confirm(l('ReIndexAllProjectConfirmationMessage'))
            .done(function (accepted) {
                if (accepted) {
                    volo.docs.admin.projectsAdmin
                        .reindexAll()
                        .then(function () {
                            abp.message.success(l('SuccessfullyReIndexAllProject'));
                        });
                }
            });
    });

});
