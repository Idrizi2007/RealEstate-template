var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: { url: '/Admin/Amenity/GetAll' },
        order: [],
        columns: [
            { data: 'name', width: '40%', render: $.fn.dataTable.render.text() },
            {
                data: 'iconClass',
                width: '25%',
                defaultContent: '',
                render: function (data) {
                    return data ? `<i class="${$('<div>').text(data).html()} fs-4"></i>` : '';
                }
            },
            {
                data: 'id',
                width: '35%',
                orderable: false,
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/Admin/Amenity/Upsert?id=${data}" class="btn btn-primary mx-2">Ndrysho</a>
                        <a onClick="confirmDelete('/Admin/Amenity/Delete/${data}', () => dataTable.ajax.reload())" class="btn btn-danger mx-2">Fshi</a>
                    </div>`;
                }
            }
        ]
    });
}
