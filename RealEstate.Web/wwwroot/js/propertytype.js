var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: { url: '/Admin/PropertyType/GetAll' },
        order: [],
        columns: [
            { data: 'name', width: '65%', render: $.fn.dataTable.render.text() },
            {
                data: 'id',
                width: '35%',
                orderable: false,
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/Admin/PropertyType/Upsert?id=${data}" class="btn btn-primary mx-2">Ndrysho</a>
                        <a onClick="confirmDelete('/Admin/PropertyType/Delete/${data}', () => dataTable.ajax.reload())" class="btn btn-danger mx-2">Fshi</a>
                    </div>`;
                }
            }
        ]
    });
}
