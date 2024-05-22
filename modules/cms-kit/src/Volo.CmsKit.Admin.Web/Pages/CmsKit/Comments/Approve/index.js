


    $(function () {

        var l = abp.localization.getResource("CmsKit");

        var commentsService = volo.cmsKit.admin.comments.commentAdmin;

        var detailsModal = new abp.ModalManager(abp.appPath + "CmsKit/Comments/DetailsModal");

        moment()._locale.preparse = (string) => string;
        moment()._locale.postformat = (string) => string;

        var getFormattedDate = function ($datePicker) {
            if (!$datePicker.val()) {
                return null;
            }
            var momentDate = moment($datePicker.val(), $datePicker.data('daterangepicker').locale.format);
            return momentDate.isValid() ? momentDate.toISOString() : null;
        };


        $('.singledatepicker').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            "autoUpdateInput": false,
            "autoApply": true,
            "opens": "center",
            "drops": "auto"
        });



        $('.singledatepicker').attr('autocomplete', 'off');

        $('.singledatepicker').on('apply.daterangepicker', function (ev, picker) {
            $(this).val(picker.startDate.format('l'));
        });


        var filterForm = $('#CmsKitCommentsFilterForm');

        var getFilter = function () {
            var filterObj = filterForm.serializeFormToObject();

            filterObj.creationStartDate = getFormattedDate($('#CreationStartDate'));
            filterObj.creationEndDate = getFormattedDate($('#CreationEndDate'));

            return filterObj;
        };


        var _dataTable = $('#CommentsTable').DataTable(abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: true,
            paging: true,
            scrollX: true,
            searching: false,
            scrollCollapse: true,
            ajax: abp.libs.datatables.createAjax(commentsService.getWaitingCommentsWithReplies, getFilter),
            columnDefs: [
                {
                    width: "10%",
                    title: l("Actions"),
                    targets: 0,
                    orderable: false,
                    rowAction: {
                        items: [
                            {
                                text: function (data) {
                                    return l('Approve');
                                },
                                action: function (data) {
                                    var newApprovalStatus = true;

                                    commentsService
                                        .updateApprovalStatus(data.record.id, { IsApproved: newApprovalStatus })
                                        .then(function () {
                                            _dataTable.ajax.reloadEx();
                                            var message = newApprovalStatus ? l('ApprovedSuccessfully') : l('ApprovalRevokedSuccessfully');
                                            abp.notify.success(message);
                                        })
                                        .catch(function (error) {
                                            abp.notify.error(error.message);
                                        });
                                }
                            },
                            {
                                text: function (data) {
                                    return l('Disapproved');
                                },
                                action: function (data) {
                                    var newApprovalStatus = false;

                                    commentsService
                                        .updateApprovalStatus(data.record.id, { IsApproved: newApprovalStatus })
                                        .then(function () {
                                            _dataTable.ajax.reloadEx();
                                            var message = newApprovalStatus ? l('ApprovedSuccessfully') : l('ApprovalRevokedSuccessfully');
                                            abp.notify.success(message);
                                        })
                                        .catch(function (error) {
                                            abp.notify.error(error.message);
                                        });
                                }
                            }
                        ]
                    }
                },
                {
                    width: "10%",
                    title: l("Username"),
                    orderable: false,
                    data: "author.userName",
                    render: function (data) {
                        if (data !== null) {
                            return GetFilterableDatatableContent('#Author', $.fn.dataTable.render.text().display(data)); //prevent against possible XSS
                        }
                        return "";
                    }
                },
                {
                    width: "15%",
                    title: l("EntityType"),
                    orderable: false,
                    data: "entityType",
                    render: function (data) {
                        if (data !== null) {
                            return GetFilterableDatatableContent('#EntityType', $.fn.dataTable.render.text().display(data));
                        }
                        return "";
                    }
                },
                {
                    title: l("Text"),
                    data: "text",
                    orderable: false,
                    render: function (data) {

                        var converter = new showdown.Converter();
                        var htmlContent = converter.makeHtml(data);
                        return (htmlContent);
                    }
                },
                {
                    width: "15%",
                    title: l("CreationTime"),
                    data: "creationTime",
                    orderable: true,
                    dataFormat: "datetime"
                }
            ]
        }));

        function GetFilterableDatatableContent(filterSelector, data) {
            return '<span class="datatableCell" data-field="' + filterSelector + '" data-val="' + data + '">' + data + '</span>';
        }

        $(document).on('click', '.datatableCell', function () {
            var inputSelector = $(this).attr('data-field');
            var value = $(this).attr('data-val');

            $(inputSelector).val(value);

            _dataTable.ajax.reloadEx();
        });

        filterForm.submit(function (e) {
            e.preventDefault();
            _dataTable.ajax.reloadEx();
        });
    });
