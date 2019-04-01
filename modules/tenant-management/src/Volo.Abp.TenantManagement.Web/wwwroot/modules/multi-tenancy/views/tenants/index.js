﻿(function () {

    var l = abp.localization.getResource('AbpTenantManagement');
    var _tenantAppService = volo.abp.tenantManagement.tenant;

    var _editModal = new abp.ModalManager(abp.appPath + 'TenantManagement/Tenants/EditModal');
    var _createModal = new abp.ModalManager(abp.appPath + 'TenantManagement/Tenants/CreateModal');
    var _featuresModal = new abp.ModalManager(abp.appPath + 'FeatureManagement/FeatureManagementModal');

    $(function () {

        var _$wrapper = $('#TenantsWrapper');

        var _dataTable = _$wrapper.find('table').DataTable(abp.libs.datatables.normalizeConfiguration({
            order: [[1, "asc"]],
            ajax: abp.libs.datatables.createAjax(_tenantAppService.getList),
            columnDefs: [
                {
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    visible: function () { return true; }, //TODO: Check permission
                                    action: function (data) {
                                        _editModal.open({
                                            id: data.record.id
                                        });
                                    }
                                },
                                {
                                    text: l('Features'),
                                    visible: function () {
                                        return true; //TODO: Check permission
                                    },
                                    action: function (data) {
                                        _featuresModal.open({
                                            providerName: 'Tenant',
                                            providerKey: data.record.id
                                        });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    visible: function () { return true; }, //TODO: Check permission
                                    confirmMessage: function (data) { return l('TenantDeletionConfirmationMessage', data.record.name)},
                                    action: function (data) {
                                        _tenantAppService
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
                    data: "name"
                }
            ]
        }));

        _createModal.onResult(function () {
            _dataTable.ajax.reload();
        });

        _editModal.onResult(function () {
            _dataTable.ajax.reload();
        });

        _$wrapper.find('button[name=CreateTenant]').click(function (e) {
            e.preventDefault();
            _createModal.open();
        });
    });

})();
