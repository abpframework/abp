$(function () {
    var l = abp.localization.getResource("CmsKit");

    var createModal = new abp.ModalManager(abp.appPath + "CmsKit/Tags/CreateModal");
    var updateModal = new abp.ModalManager(abp.appPath + "CmsKit/Tags/EditModal");

    var service = volo.cmsKit.admin.tags.tagAdmin;

    var getFilter = function () {
        return {
            filter: $('#CmsKitTagsWrapper input.page-search-filter-text').val()
        };
    };

    let dataTable = $("#TagsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollCollapse: true,
        scrollX: true,
        ordering: false,
        ajax: abp.libs.datatables.createAjax(service.getList, getFilter),
        columnDefs: [
            {
                title: l("Actions"),
                rowAction: {
                    items: [
                        {
                            text: l("Edit"),
                            visible: abp.auth.isGranted('CmsKit.Tags.Update'),
                            action: function (data) {
                                updateModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l("Delete"),
                            visible: abp.auth.isGranted('CmsKit.Tags.Delete'),
                            confirmMessage: function (data) {
                                return l("TagDeletionConfirmationMessage", data.record.name)
                            },
                            action: function (data) {
                                service
                                    .delete(data.record.id)
                                    .then(function () {
                                        dataTable.ajax.reload(null, false);
                                    });
                            }
                        }
                    ]
                }
            },
            {
                title: l("EntityType"),
                data: "entityType"
            },
            {
                title: l("Name"),
                data: "name"
            }
        ]
    }));

    $('#CmsKitTagsWrapper form.page-search-form').on('submit', function (e) {
        e.preventDefault();
        dataTable.ajax.reload();
    });

    $('#AbpContentToolbar button[name=NewButton]').on('click', function (e) {
        e.preventDefault();
        createModal.open();
    });

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    updateModal.onResult(function () {
        dataTable.ajax.reload();
    });
});