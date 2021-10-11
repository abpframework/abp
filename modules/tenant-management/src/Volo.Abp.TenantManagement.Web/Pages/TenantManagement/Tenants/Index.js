(function () {
    var l = abp.localization.getResource('AbpTenantManagement');
    var _tenantAppService = volo.abp.tenantManagement.tenant;

    var _editModal = new abp.ModalManager(
        abp.appPath + 'TenantManagement/Tenants/EditModal'
    );
    var _createModal = new abp.ModalManager(
        abp.appPath + 'TenantManagement/Tenants/CreateModal'
    );
    var _featuresModal = new abp.ModalManager(
        abp.appPath + 'FeatureManagement/FeatureManagementModal'
    );

    var _dataTable = null;

    abp.ui.extensions.entityActions.get('tenantManagement.tenant').addContributor(
        function(actionList) {
            return actionList.addManyTail(
                [
                    {
                        text: l('Edit'),
                        visible: abp.auth.isGranted(
                            'AbpTenantManagement.Tenants.Update'
                        ),
                        action: function (data) {
                            _editModal.open({
                                id: data.record.id,
                            });
                        },
                    },
                    {
                        text: l('Features'),
                        visible: abp.auth.isGranted(
                            'AbpTenantManagement.Tenants.ManageFeatures'
                        ),
                        action: function (data) {
                            _featuresModal.open({
                                providerName: 'T',
                                providerKey: data.record.id,
                            });
                        },
                    },
                    {
                        text: l('Delete'),
                        visible: abp.auth.isGranted(
                            'AbpTenantManagement.Tenants.Delete'
                        ),
                        confirmMessage: function (data) {
                            return l(
                                'TenantDeletionConfirmationMessage',
                                data.record.name
                            );
                        },
                        action: function (data) {
                            _tenantAppService
                                .delete(data.record.id)
                                .then(function () {
                                    _dataTable.ajax.reload();
                                });
                        },
                    }
                ]
            );
        }
    );

    abp.ui.extensions.tableColumns.get('tenantManagement.tenant').addContributor(
        function (columnList) {
            columnList.addManyTail(
                [
                    {
                        title: l("Actions"),
                        rowAction: {
                            items: abp.ui.extensions.entityActions.get('tenantManagement.tenant').actions.toArray()
                        }
                    },
                    {
                        title: l("TenantName"),
                        data: 'name',
                    }
                ]
            );
        },
        0 //adds as the first contributor
    );

    $(function () {
        var _$wrapper = $('#TenantsWrapper');

        _dataTable = _$wrapper.find('table').DataTable(
            abp.libs.datatables.normalizeConfiguration({
                order: [[1, 'asc']],
                processing: true,
                paging: true,
                scrollX: true,
                serverSide: true,
                ajax: abp.libs.datatables.createAjax(_tenantAppService.getList),
                columnDefs: abp.ui.extensions.tableColumns.get('tenantManagement.tenant').columns.toArray(),
            })
        );

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

        _$wrapper.find('button[name=ManageHostFeatures]').click(function (e) {
            e.preventDefault();
            _featuresModal.open({
                providerName: 'T'
            });
        });
    });
})();
