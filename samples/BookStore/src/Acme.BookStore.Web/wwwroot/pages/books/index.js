$(function() {
    $('#BooksTable').DataTable({
        ajax: abp.libs.datatables.createAjax(acme.bookStore.book.getList),
        columnDefs: [
            {
                targets: 0,
                data: "name"
            },
            {
                targets: 1,
                data: "type"
            },
            {
                targets: 2,
                data: "price"
            },
            {
                targets: 3,
                data: "publishDate"
            }
        ]
    });
});