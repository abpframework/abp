(function () {

    var l = abp.localization.getResource('AbpMultiTenancy');
    var _tenantAppService = volo.abp.multiTenancy.tenant;

    var _editModal = new abp.ModalManager(abp.appPath + 'MultiTenancy/Tenants/EditModal');
    var _createModal = new abp.ModalManager(abp.appPath + 'MultiTenancy/Tenants/CreateModal');

    var app = new Vue({
        el: '#TenantsWrapper',
        methods: {
            openCreateModal: function () {
                _createModal.open();
            }
        }
    });

    $(function () {

        var _$wrapper = $('#TenantsWrapper');
        var _$table = _$wrapper.find('table');
        var _$dataTable;

        $.when($.fn.dataTable.defaults.languageFileReady).then(function () {

            _$dataTable = _$table.DataTable({
                order: [[1, "asc"]],
                ajax: abp.libs.datatables.createAjax(_tenantAppService.getList),
                columnDefs: [
                    {
                        //TODO: Can we eleminate targets, data, orderable, autoWidth, defaultContent fields or make these values default
                        targets: 0,
                        data: null,
                        orderable: false,
                        autoWidth: false,
                        defaultContent: '',
                        rowAction: {
                            text: '<i class="fa fa-cog"></i> ' + l('Actions') + ' <span class="caret"></span>', //TODO: Add icon option and set text as only l('Actions')
                            items:
                            [
                                {
                                    //TODO: Allow to add icon
                                    text: l('Edit'),
                                    visible: function () { //TODO: Allow visible to be a boolean for simple cases
                                        return true;
                                    },
                                    action: function (data) {
                                        _editModal.open({
                                            id: data.record.id
                                        });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    visible: function () {
                                        return true;
                                    },
                                    action: function (data) {
                                        if (confirm(l('TenantDeletionConfirmationMessage', data.record.name))) {
                                            _tenantAppService
                                                .delete(data.record.id)
                                                .then(function () {
                                                    _dataTable.ajax.reload();
                                                });
                                        }
                                    }
                                }
                            ]
                        }
                    },
                    {
                        targets: 1,
                        data: "name"
                    }
                ]
            });

        });


        _createModal.onResult(function () {
            _$dataTable.ajax.reload();
        });

        _editModal.onResult(function () {
            _$dataTable.ajax.reload();
        });
    });

})();
