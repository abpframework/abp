$(function () {
    var l = abp.localization.getResource("CmsKit");

    var createModal = new abp.ModalManager(abp.appPath + "CmsKit/Menus/CreateModal");
    var updateModal = new abp.ModalManager(abp.appPath + "CmsKit/Menus/UpdateModal");

    var menusService = volo.cmsKit.admin.menus.menuAdmin;

    var dataTable = $("#MenusTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollCollapse: true,
        scrollX: true,
        ordering: true,
        order: [[1, "desc"]],
        ajax: abp.libs.datatables.createAjax(menusService.getList),
        columnDefs: [
            {
                title: l("Details"),
                targets: 0,
                rowAction: {
                    items: [
                        {
                            text: l('MenuItems'),
                            visible: abp.auth.isGranted('CmsKit.Menus.Update'),
                            action: function (data) {
                                location.href = "/Cms/Menus/" + data.record.id + "/menu-items"
                            }
                        },
                        {
                            text: l('MakeMainMenu'),
                            visible: function (data) {
                                return abp.auth.isGranted('CmsKit.Menus.Update') && !data.isMainMenu
                            },
                            action: function (data) {
                                menusService
                                    .updateMainMenu(data.record.id, {isMainMenu: true})
                                    .then(function () {
                                        dataTable.ajax.reload();
                                    });
                            }
                        },
                        {
                            text: l('UnMakeMainMenu'),
                            visible: function (data) {
                                return abp.auth.isGranted('CmsKit.Menus.Update') && data.isMainMenu
                            },
                            action: function (data) {
                                menusService
                                    .updateMainMenu(data.record.id, {isMainMenu: false})
                                    .then(function () {
                                        dataTable.ajax.reload();
                                    });
                            }
                        },
                        {
                            text: l('Edit'),
                            visible: abp.auth.isGranted('CmsKit.Menus.Update'),
                            action: function (data) {
                                updateModal.open({id: data.record.id});
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('CmsKit.Menus.Delete'),
                            confirmMessage: function (data) {
                                return l("MenuDeletionConfirmationMessage", data.record.name)
                            },
                            action: function (data) {
                                menusService
                                    .delete(data.record.id)
                                    .then(function () {
                                        dataTable.ajax.reload();
                                    });
                            }
                        }
                    ]
                }
            },
            {
                title: l("Name"),
                orderable: true,
                data: "name",
                render: function (data, val, record) {

                    if (record.isMainMenu) {
                        return (
                            data
                            +
                            '<span class="badge badge-pill badge-info ml-1">'
                            +
                                l("MainMenu")
                            +
                            '</span>'
                        );
                    } else {
                        return data;
                    }
                }
            }
        ]
    }));

    $('#AbpContentToolbar button[name=CreateMenu]').on('click', function (e) {
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