var abp = abp;
(function () {

    var datatables = abp.utils.createNamespace(abp, 'libs.datatables');

    datatables.createAjax = function(serverMethod, inputAction) {
        return function (requestData, callback, settings) {
            var input = inputAction ? inputAction() : {};

            //Paging
            if (settings.oInit.paging) {
                input.maxResultCount = requestData.length;
                input.skipCount = requestData.start;
            }

            //Sorting
            if (requestData.order && requestData.order.length > 0) {
                var orderingField = requestData.order[0];
                if (requestData.columns[orderingField.column].data) {
                    input.sorting = requestData.columns[orderingField.column].data + " " + orderingField.dir;
                }
            }

            //Text filter
            if (requestData.search && requestData.search.value !== "") {
                input.filter = requestData.search.value;
            }

            if (callback) {
                serverMethod(input).then(function (result) {
                    callback({
                        recordsTotal: result.totalCount,
                        recordsFiltered: result.totalCount,
                        data: result.items
                    });
                });
            }
        }
    }

    //TODO: Implement like we did before!
    datatables.createActionColumn = function (actions) {
        return {
            targets: 0,
            data: null,
            orderable: false,
            autoWidth: false,
            defaultContent: '',
            render: function (list, type, record, meta) {
                var htmlContent;

                if (actions && actions.length) {
                    var actionLinks = '';
                    for (var i = 0; i < actions.length; ++i) {
                        var action = actions[i];
                        actionLinks += '<a class="dropdown-item" href="#">' + action.text + '</a>';
                    }

                    htmlContent = '<div class="dropdown">' +
                        '<button class="btn btn-primary btn-sm dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                        'Actions' +
                        '</button>' +
                        '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                        actionLinks +
                        '</div>' +
                        '</div>';
                } else {
                    htmlContent = '-'; //TODO: ...?
                }

                return htmlContent;
            }
        }
    };

})();

