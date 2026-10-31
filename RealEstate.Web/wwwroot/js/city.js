var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: { url: '/Admin/City/GetAll' },
        columns: [
            { data: 'name', width: '50%', render: $.fn.dataTable.render.text() },
            {
                data: 'isActive',
                width: '15%',
                render: function (data) {
                    return data
                        ? '<span class="badge bg-success">Po</span>'
                        : '<span class="badge bg-secondary">Jo</span>';
                }
            },
            {
                data: 'id',
                width: '35%',
                orderable: false,
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/Admin/City/Upsert?id=${data}" class="btn btn-primary mx-2">Ndrysho</a>
                        <a onClick="confirmDelete('/Admin/City/Delete/${data}', () => dataTable.ajax.reload())" class="btn btn-danger mx-2">Fshi</a>
                    </div>`;
                }
            }
        ]
    });
}
