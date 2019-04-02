$(function () {

    var l = abp.localization.getResource('BaseManagement');
    var _createModal = new abp.ModalManager(abp.appPath + 'BaseManagement/Products/Create');
    var _editModal = new abp.ModalManager(abp.appPath + 'BaseManagement/Products/Edit');

    var _dataTable = $('#ProductsTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[1, "desc"]],
        ajax: abp.libs.datatables.createAjax(productManagement.products.getListPaged),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l('Edit'),
                                visible: function () {
                                    return true; //TODO: Check permission
                                },
                                action: function (data) {
                                    _editModal.open({
                                        productId: data.record.id
                                    });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: function () {
                                    return true; //TODO: Check permission
                                },
                                confirmMessage: function (data) { return l('ProductDeletionWarningMessage'); },
                                action: function (data) {
                                    productManagement.products
                                        .delete(data.record.id)
                                        .then(function () {
                                            _dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                }
            },
            {
                target: 1,
                data: "code"
            },
            {
                target: 2,
                data: "name"
            },
            {
                target: 3,
                data: "price"
            },
            {
                target: 4,
                data: "stockCount"
            }
        ]
    }));


    $("#CreateNewProductButtonId").click(function () {
        _createModal.open();
    });

    _createModal.onClose(function () {
        _dataTable.ajax.reload();
    });

    _editModal.onResult(function () {
        _dataTable.ajax.reload();
    });

});